using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Integration.Services.SamsIntegration
{
    public class SamsBase
    {
        string _url = ConfigurationManager.AppSettings["SamsBaseUrl"].ToString();
        string _userName = ConfigurationManager.AppSettings["SamsUserName"].ToString();
        string _password = ConfigurationManager.AppSettings["SamsPassword"].ToString();
        public void InvokeApi<RequestType, ResponseType>(RequestType obj, string urlExtension,ref ResponseType res)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AllwaysGoodCertificate);
                var httpClientHandler = new HttpClientHandler()
                {
                    Credentials = new NetworkCredential(_userName, _password),
                };
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient(httpClientHandler);
                client.BaseAddress = new Uri(_url);
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                response = client.PostAsJsonAsync(urlExtension, obj).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                    res= response.Content.ReadAsAsync<ResponseType>().Result;
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
        internal Error AuthenticateSams(User user)
        {
            Error error = new Error();
            InvokeApi<User, Error>(user, "user-management/auth-user/", ref error);
            return error;
        }
    }
    public class User
    {
        public string loginPassword { get; set; }
        public string userName { get; set; }
    }
    public class Error
    {
        public string errorCode { get; set; }
        public string errorDescription { get; set; }
    }
    public class LeadCreation
    {
        public string AvoLeadNumber { get; set; }
        public string Flag { get; set; }
        public string UserId { get; set; }
        public string AgentCode { get; set; }
        public string AvoLeadType { get; set; }
        public string LeadTitle { get; set; }
        public string LeadFirstName { get; set; }
        public string LeadLastName { get; set; }
        public string LeadContactNumber { get; set; }
        public string LeadEmail { get; set; }
        public string LeadPlace { get; set; }
        public string Channel { get; set; }
        public int SamsLeadNumber { get; set; }
        public string CreatedDateTime { get; set; }
        public string LeadContactOffice { get; set; }
        public string LeadContactResident { get; set; }
        public string LeadIntroducer { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string DOB { get; set; }
        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public string NIC { get; set; }
        public string AssignedTo { get; set; }
    }
    public class LeadStatus
    {
        public int samsLeadNumber { get; set; }
        public int status { get; set; }
        public string statusDateTime { get; set; }
    }
    public class LeadResponse
    {
        public LeadResponse()
        {
            header = new Header();
            body = new Body();
        }
        public Header header { get; set; }
        public Body body { get; set; }
        public string StatusCode { get; set; }
        public string statusCodeValue { get; set; }
    }
    public class Header
    {

    }
    public class Body
    {
        public string AvoLeadNumber { get; set; }
        public string SamsLeadNumber { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
    }
    public class MessageRequest
    {
        public string destination { get; set; }
        public string message { get; set; }
    }
    public class MessageResponse
    {
        public string errorCode { get; set; }
        public string errorDescription { get; set; }
        public string adiReferenceNo { get; set; }
    }
}

