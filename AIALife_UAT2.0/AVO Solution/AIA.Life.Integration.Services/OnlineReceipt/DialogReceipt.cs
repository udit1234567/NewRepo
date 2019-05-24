using AIA.Life.Models.Payment;
using AIA.Life.Repository.AIAEntity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Integration.Services.OnlineReceipt
{
    public class DialogReceipt
    {
        public string SendDialogOnlinePremium(PaymentModel paymentModel)
        {
            AIAOnlinePremiumService.LifeClient lifeClient = new AIAOnlinePremiumService.LifeClient();
            string instType = ConfigurationManager.AppSettings["OPSINSTTYPE"].ToString();
            string clientID = ConfigurationManager.AppSettings["OPSCLIENTID"].ToString();
            string password = ConfigurationManager.AppSettings["OPSPWD"].ToString();
            using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
            {
                #region  Log Input 

                tbllogxml objlogxml = new tbllogxml();
                objlogxml.Description = "online reciept request";
                objlogxml.PolicyID = paymentModel.ProposalNo;
                objlogxml.XMlData = paymentModel.ProposalNo + "|" + instType + "|" + paymentModel.TransactionNo + "|" + paymentModel.PayableAmount + "|" + clientID + "|" + password;
                objlogxml.CreatedDate = DateTime.Now;
                Context.tbllogxmls.Add(objlogxml);
                Context.SaveChanges();

                #endregion
                string receipt = lifeClient.uploadPayment(paymentModel.ProposalNo, instType, paymentModel.TransactionNo, Convert.ToDouble(paymentModel.PayableAmount), clientID, password);
                #region  Log Output 

                tbllogxml objlogxml1 = new tbllogxml();
                objlogxml1.Description = "online reciept response";
                objlogxml1.PolicyID = paymentModel.ProposalNo;
                objlogxml1.XMlData = receipt;
                objlogxml1.CreatedDate = DateTime.Now;
                Context.tbllogxmls.Add(objlogxml1);
                Context.SaveChanges();

                #endregion
                return receipt;
            }
            
        }
    }
}
