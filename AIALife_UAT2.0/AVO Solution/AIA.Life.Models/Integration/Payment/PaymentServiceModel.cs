using AIA.Life.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AIA.Life.Models.Integration.Payment
{
    public class PaymentServiceModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Home { get; set; }
        public string Work { get; set; }
        public string Email { get; set; }

        public string TotalAmountPayable { get; set; }

        [XmlIgnore]
        public List<MasterListItem> lstSalutation { get; set; }

        public Address objAddress { get; set; }

        
        public List<PaymentProposal> ObjPaymentProposalPool { get; set; }
        public List<PaymentRenewal> ObjPaymentRenewalPool { get; set; }
        public List<RenewedPolicies> ObjPaymentRenewedPoliciesPool { get; set; }
        public List<RenewedAllPolicies> ObjPaymentRenewedAllPolicies { get; set; }
        public List<RenewedInforcePolicies> ObjPaymentRenewedInforcePolicies { get; set; }
        public List<RenewedPoliciesinGracePeriod> ObjPaymentRenewedPoliciesinGracePeriod { get; set; }
        public List<RenewedRunningLapsePolicies> ObjPaymentRenewedRunningLapsePolicies { get; set; }
        public List<RenewedLapsedPolicies> ObjPaymentRenewedLapsedPolicies { get; set; }

        public List<PolicyHolderRenewedPolicies> ObjPolicyHolderRenewedPolicies { get; set; }

    }
    //public class ClientConfig
    //{
    //    public string serviceEndpoint { get; set; }
    //    public string hmacSecret { get; set; }
    //    public string authToken { get; set; }
    //}
    //public class PaymentRealTimeRequest
    //{
    //    public int clientId { get; set; }
    //    public string transactionType { get; set; }
    //    public CreditCard creditCard { get; set; }
    //    public string transactionAmount { get; set; }
    //    public string clientRef { get; set; }
    //    public string comment { get; set; }
    //}
    //public class CreditCard
    //{
    //    public string holderName { get; set; }
    //    public string number { get; set; }
    //    public string secureId { get; set; }
    //    public string expiry { get; set; }
    //}
    public class Address
    {
        public decimal AddressID { get; set; }
        public int AddressTypeId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int? PincodeID { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
        public string PincodeNew { get; set; }
        public int? StateID { get; set; }
        public string Province { get; set; }
        public string ProvinceCode { get; set; }
        public string DistrictCode { get; set; }
        public string CityCode { get; set; }
        [XmlIgnore]
        public List<MasterListItem> LstPincode { get; set; }
    }

    public class PaymentProposal
    {
        public string PaymentProposalNumber { get; set; }
        public string PaymentProposalFirstName { get; set; }
        public string PaymentProposalLastName { get; set; }
        public string PaymentProposalActualPremiumAmt { get; set; }
        public string PaymentProposalDepositPaid { get; set; }
        public string PaymentProposalAmountPayable { get; set; }
        public string PaymentProposalDaysLeft { get; set; }
    }

    public class PaymentRenewal
    {
        public int PaymentRenewalCountPolicies { get; set; }
        public int PaymentRenewedCountPolicies { get; set; }
        public int PaymentRenewalPolicies { get; set; }
        public int PaymentCorrespondingDeposit { get; set; }
        public int PaymentRenewedPolicies { get; set; }
    }

    public class RenewedPolicies
    {
        public string PaymentRenewedFirstName { get; set; }
        public string PaymentRenewedLastName { get; set; }
        public string PaymentRenewedMobile { get; set; }
        public string PaymentRenewedHome { get; set; }
        public string PaymentRenewedWork { get; set; }
        public string PaymentRenewedNIC { get; set; }
        public string PaymentRenewedStarClassification { get; set; }
    }

    public class RenewedAllPolicies
    {
        public int PolicyId { get; set; }
        public int PolicyNumber { get; set; }
        public string FirstName { get; set; }
        public string Status { get; set; }
        public string PremiumDueDate { get; set; }
        public string PremiumAmount { get; set; }
        public string LastPaymentDate { get; set; }
        public string NoOfUnpaidPremiums { get; set; }
        public string TotalArrears { get; set; }
        public string DepositAmount { get; set; }
        public string TotalAmounttobePaid { get; set; }
        public string DaysLeftForLapse { get; set; }
    }

    public class RenewedInforcePolicies
    {
        public int PolicyId { get; set; }
        public string PolicyNumber { get; set; }
        public string FirstName { get; set; }
        public string PremiumDueDate { get; set; }
        public string PremiumAmount { get; set; }
        public string DepositAmount { get; set; }
        public string TotalAmounttobePaid { get; set; }
    }

    public class RenewedPoliciesinGracePeriod
    {
        public int PolicyId { get; set; }
        public string PolicyNumber { get; set; }
        public string FirstName { get; set; }
        public string PremiumDueDate { get; set; }
        public string PremiumAmount { get; set; }
        public string LastPaymentDate { get; set; }
        public string DepositAmount { get; set; }
        public string TotalAmounttobePaid { get; set; }
        public string DaysLeftForRunningLapse { get; set; }
    }

    public class RenewedRunningLapsePolicies
    {
        public int PolicyId { get; set; }
        public string PolicyNumber { get; set; }
        public string FirstName { get; set; }
        public string PremiumDueDate { get; set; }
        public string PremiumAmount { get; set; }
        public string LastPaymentDate { get; set; }
        public string NoOfUnpaidPremiums { get; set; }
        public string TotalArrears { get; set; }
        public string TotalLateFees { get; set; }
        public string DepositAmount { get; set; }
        public string TotalAmounttobePaid { get; set; }
        public string DaysLeftForLapse { get; set; }
    }

    public class RenewedLapsedPolicies
    {
        public int PolicyId { get; set; }
        public string PolicyNumber { get; set; }
        public string FirstName { get; set; }
        public string LapsedOn { get; set; }
        public string PremiumDueDate { get; set; }
        public string PremiumAmount { get; set; }
        public string LastPaymentDate { get; set; }
        public string NoOfUnpaidPremiums { get; set; }
        public string TotalArrears { get; set; }
        public string TotalLateFees { get; set; }
        public string DepositAmount { get; set; }
        public string TotalAmounttobePaid { get; set; }
    }

    public class PolicyHolderRenewedPolicies
    {
        public string PolicyHolderPolicyNo { get; set; }
        public int PolicyHolderPremiumAmount { get; set; }
        public DateTime? PolicyHolderLastPaymentDate { get; set; }
        public int PolicyHolderDepositAmount { get; set; }
        public DateTime? PolicyHolderNextPremiumDueDate { get; set; }
    }
}
