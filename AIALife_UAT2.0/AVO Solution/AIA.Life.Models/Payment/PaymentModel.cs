using AIA.Life.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AIA.Life.Models.Payment
{
    public class PaymentModel
    {

        public PaymentModel()
        {
            lstPaymentItems = new List<PaymentItems>();
            lstInsumentDetails = new List<InstrumentDetails>();
            LstInstrumentType = new List<MasterListItem>();
            Error = new Error();
            PreIssueValidations = new List<string>();

            lstPayableCurrency = new List<MasterListItem>();
        }
        public string Mobile { get; set; }
        public string BranchName { get; set; }
        public string Remarks{ get; set; }
        public string PaymentChanel { get; set; }
        public List<MasterListItem> LstInstrumentType { get; set; }
        public string ChequeNo { get; set; }
        public string CheqAmount { get; set; }
        public string ChequeBranch { get; set; }       
        public string UserName { get; set; }
        public string Message { get; set; }
        public string ProposalNo { get; set; }
        public string QuoteNo { get; set; }
        public string CloseWindow { get; set; }
        public bool PayforMultipleRecords { get; set; }
        public string NICNO { get; set; }
        public string PolicyNo { get; set; }
        public List<PaymentItems> lstPaymentItems { get; set; }
        public string PayableAmount { get; set; }
        public decimal PayingAmount { get; set; }

        public string PayableCurrency { get; set; }
        public string TotalPayableAmount { get; set; }
        public List<MasterListItem> lstPayableCurrency { get; set; }

        public string PaymentOption { get; set; }
        public DateTime? Instrumentdate { get; set; }
        public DateTime? CashDate { get; set; }
        public List<InstrumentDetails> lstInsumentDetails { get; set; }

        public string UWMessage { get; set; }

        public decimal? chequeAmount { get; set; }
        public decimal? ddAmount { get; set; }
        public decimal? cashAmount { get; set; }
        public string SelectedPayment { get; set; }
        public string SelectedSubPayment { get; set; }
        public string TransactionNo { get; set; }
        public string PGResponse { get; set; }
        public string McashMobile { get; set; }
        public string McashPin { get; set; }

      
        public Error Error { get; set; }
        public List<string> PreIssueValidations { get; set; }
        public byte[] ByteArray { get; set; }
        public byte[] ByteArray5 { get; set; }
        public byte[] ByteArray6 { get; set; }
        public byte[] ByteArray2 { get; set; }
        public byte[] ByteArray3 { get; set; }
        public byte[] ByteArray4 { get; set; }

        public string BizDate { get; set; }
        public string ReqId { get; set; }
    }

    public class PaymentItems
    {
        public int? PlanId { get; set; }  
        public string PrefferedMode { get; set; }
        public double PaymentId { get; set; }
        public bool IsSelected { get; set; }
        public int Index { get; set; }
        public decimal PolicyId { get; set; }
        public string QuoteNo { get; set; }
        public string ProposalNo { get; set; }
        public string InsuredName { get; set; }
        public string InsuredLastName { get; set; }
        public string PlanName { get; set; }
        public string PolicyTerm { get; set; }
        public DateTime? IssueDate { get; set; }
        public decimal? Premium { get; set; }
        public string AdditionalPremium { get; set; }
        public string CustomerMobile { get; set; }
        public int MyProperty { get; set; }
        public string PreferredLanguage { get; set; }
        public string Salutation { get; set; }
        public DateTime? PolicyStartDate { get; set; }
        public DateTime? PolicyEndDate { get; set; }
        public string ProductName { get; set; }
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string PlanCode { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string IsAfc { get; set; }
        public string UserName { get; set; }
    }

    public class InstrumentDetails
    {

        public string ClientName { get; set; }
        public string ProposalNumber { get; set; }
        public string MethodofPayment { get; set; }
        public string PremiumAmount { get; set; }
        public string InstrumentNo { get; set; }
        public string AmountPaid { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public DateTime? Instrumentdate { get; set; }
        //  public DateTime? Cashdate { get; set; }
        // Need To Add other parameters

    }

}
