using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.EmailSMSDetails
{
   
    public class EmailDetails
    {
        public EmailDetails()
        {
            objRecievedDoc = new List<EmailSMSDetails.Documents>();
            objPendingDoc = new List<EmailSMSDetails.Documents>();
        }

        public string Response { get; set; }     
        public string MailTemplate { get; set; }
        public string Subject { get; set; }
        public string ToEmailAddress { get; set; }
        public string Name { get; set; }
        public string QuoteNumber { get; set; }
        public string PolicyNumber { get; set; }
       public string MobileNumber { get; set; }
        public string WPMobileNo { get; set; }
        public string ProposalNo { get; set; }
        public string EmailID { get; set; }
        public string AgentEmailID { get; set; }
        public string WPEmailID { get; set; }
        public string surName { get; set; }
        public string Salutation { get; set; }
        public string PolicyStartDate { get; set; }
        public string PolicyEndDate { get; set; }
        public string Premium { get; set; }
        public string ProductName { get; set; }
        public string PolicyTerm { get; set; }
        public string PremiumPayingTerm { get; set; }
        public Byte[] ByteArray { get; set; }
        public Byte[] ByteArray2 { get; set; }
        public Byte[] ByteArray3 { get; set; }
        public Byte[] ByteArray4 { get; set; }
        public Byte[] ByteArray5 { get; set; }
        public Byte[] ByteArray6 { get; set; }
        public Byte[] ByteArray8 { get; set; }
        public string ByteArray7 { get; set; }
        public string Req1 { get; set; }
        public string Duration { get; set; }
        public string TableQuotes { get; set; }
        public string TableNonMedicalQuotes { get; set; }
        public string UWDeclineDecision { get; set; }
        public string Environment { get; set; }
        public List<Documents> objRecievedDoc;
        public List<Documents> objPendingDoc;
    }
    public class Documents
    {
        public string DocumentName { get; set; }
        public string Member { get; set; }
    }
    public class SMSDetails
    {
        public string Response { get; set; }  
        public string SMSText { get; set; }
        public string SMSTemplate { get; set; }
        public string Category { get; set; }
        public string MobileNumber { get; set; }
        public string WPMobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string PolicyNo { get; set; }
        public string PolicyNumber { get; set; }
        public string QuoteNumber { get; set; }
        public string ProposalNumber { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonEmailID { get; set; }
        public string ContactPersonMobileNo { get; set; }
        public string surName { get; set; }
        public string Salutation { get; set; }
        public string Req1 { get; set; }
        public string Req2 { get; set; }
        public string Req3 { get; set; }
        public string Req4 { get; set; }
        public string PolicyStartDate { get; set; }
        public string PolicyEndDate { get; set; }
        public string Premium { get; set; }
        public string ProductName { get; set; }
        public string SMSEnvironment { get; set; }
        public string HealthconditionOccupation { get; set; }
        public string Months { get; set; }

        public string Name { get; set; }
        public string EmailID { get; set; }
    }
   
}
