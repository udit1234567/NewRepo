using AIA.Life.Repository.AIAEntity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Integration.Services.MCash
{
    public class McashRequest
    {
        public McashRequest()
        {
            merchant_data = new merchant_data();
            utility_data = new utility_data();
        }
        public merchant_data merchant_data { get; set; }
        public utility_data utility_data { get; set; }
    }
    public class merchant_data
    {
        public string pin_code { get; set; }
        public string merchant_transaction_id { get; set; }
        public string merchant_id { get; set; }
        public string mobile_number { get; set; }
    }
    public class utility_data
    {
        public string merchant_outlet_code { get; set; }
        public string note { get; set; }
        public string utility_code { get; set; }
        public string utility_account_number { get; set; }
        public decimal transaction_amount { get; set; }
        public string customer_mobile_number { get; set; }
    }
    public static class McashBase
    {
        static string _url = ConfigurationManager.AppSettings["McashBaseUrl"].ToString();
        static string _clientID = ConfigurationManager.AppSettings["McashClientid"].ToString();
        public static string InvokeApi(string requestData)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
                request.Method = "POST";
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] data = encoder.GetBytes(requestData);
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();
                request.ContentType = "application/json";
                request.Headers.Add("x-ibm-client-id", _clientID);
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                //var result = JsonConvert.SerializeObject(response);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static bool AllwaysGoodCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }
    }
    public class McashIntegration
    {
        string _mcashMerchantID = ConfigurationManager.AppSettings["McashMerchantID"].ToString();
        string _mcashUtilityCode = ConfigurationManager.AppSettings["McashUtilityCode"].ToString();
        public bool PayUtilitiesDirect(Models.Payment.PaymentModel paymentModel)
        {
            McashRequest mcash = new McashRequest();
            mcash.merchant_data.merchant_id = _mcashMerchantID;
            mcash.merchant_data.merchant_transaction_id = paymentModel.TransactionNo;
            mcash.merchant_data.pin_code = paymentModel.McashPin;
            mcash.merchant_data.mobile_number = paymentModel.McashMobile;

            mcash.utility_data.customer_mobile_number = paymentModel.Mobile;
            mcash.utility_data.merchant_outlet_code = "AIA outlet";
            mcash.utility_data.transaction_amount =Convert.ToDecimal(paymentModel.PayableAmount);
            mcash.utility_data.utility_account_number = paymentModel.ProposalNo;
            mcash.utility_data.utility_code = _mcashUtilityCode;
            mcash.utility_data.note = "Nexgen ipos transaction";

            var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(mcash);
            jsonData = jsonData.Replace('_', '-');
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            #region  Log Input 
            tbllogxml objlogxml = new tbllogxml();
            objlogxml.Description = "MCash request";
            objlogxml.PolicyID = paymentModel.ProposalNo;
            objlogxml.UserID = paymentModel.UserName;
            objlogxml.XMlData = jsonData;
            objlogxml.CreatedDate = DateTime.Now;
            Context.tbllogxmls.Add(objlogxml);
            Context.SaveChanges();
            #endregion
            
            string result = McashBase.InvokeApi(jsonData);
            
            #region  Log output 
            tbllogxml outlogxml = new tbllogxml();
            outlogxml.Description = "MCash response";
            outlogxml.PolicyID = paymentModel.ProposalNo;
            outlogxml.UserID = paymentModel.UserName;
            outlogxml.XMlData = result;
            outlogxml.CreatedDate = DateTime.Now;
            Context.tbllogxmls.Add(outlogxml);
            Context.SaveChanges();
            #endregion
            
            Dictionary<string,string> res=Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
            
            string responseCode = res.Where(a => a.Key == "response-code").Select(a => a.Value).FirstOrDefault();
            if (responseCode == "1000")
                return true;
            else
            {
                paymentModel.Message = "Error";
                paymentModel.Error.ErrorMessage = res.Where(a => a.Key == "response").Select(a => a.Value).FirstOrDefault();
                return false;
            }
        }
    }
}
