using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Integration.Payment
{
    #region Renewal Summary Page Class
    public class PaymentSummaryAPIRequest
    {
        public string userId { get; set; }
        public string companyCode { get; set; }
        public string agentCode { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
    }
    public class AgentRenevalSummary
    {
        public int renewableCount { get; set; }
        public int renewableValue { get; set; }
        public int renewedCount { get; set; }
        public int renewedValue { get; set; }
        public int depositBalance { get; set; }
    }
    public class PaymentSummaryAPIResponse
    {
        public string agentCode { get; set; }
        public AgentRenevalSummary agentRenevalSummary { get; set; }
        public string status { get; set; }
        public string massage { get; set; }
        public string errCode { get; set; }
    }
    #endregion

    #region Renewed Client Policies Summary
    public class Output
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string mobileTelNo { get; set; }
        public string homeTelNo { get; set; }
        public string workTelNo { get; set; }
        public string nic { get; set; }
    }
    public class PaymentProposalSummaryAPIResponse
    {
        public List<Output> output { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
    public class PaymentProposalSummaryAPIRequest
    {
        public string companyCode { get; set; }
        public string agentCode { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string renewalFlag { get; set; }
    }
    #endregion
}

namespace AIA.Life.Models.Integration.Payment.RenewedAgentClients
{
    public class RenewedAgentsClientsInfoRequest
    {
        public string companyCode { get; set; }
        public string agentCode { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string proposerCode { get; set; }
    }
    public class Output
    {
        public string salutation { get; set; }
        public string firstName { get; set; }
        public string middlename { get; set; }
        public string lastName { get; set; }
        public string mobileTelNo { get; set; }
        public string homeTelNo { get; set; }
        public string workTelNo { get; set; }
        public string email { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string province { get; set; }
        public string district { get; set; }
        public string city { get; set; }
        public string postCode { get; set; }
        public string policyNo { get; set; }
        public string premiumAmount { get; set; }
        public string lastPaymentDate { get; set; }
        public string depositAmount { get; set; }
        public string nextPremiumDueDate { get; set; }
    }

    public class RenewedAgentsClientsInfoRespone
    {
        public List<Output> output { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
}

namespace AIA.Life.Models.Integration.Payment.RenewableAgentClients
{
    public class RenewableAgentsClientsInfoRequest
    {
        public string companyCode { get; set; }
        public string agentCode { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string proposerCode { get; set; }
    }
    public class Output
    {
        public string salutation { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string mobileTelNo { get; set; }
        public string homeTelNo { get; set; }
        public string workTelNo { get; set; }
        public string email { get; set; }
        public string policyNo { get; set; }
        public string status { get; set; }
        public string premiumDueDate { get; set; }
        public string premiumAmount { get; set; }
        public string lastPaymentDate { get; set; }
        public string noOfunpaidPremiums { get; set; }
        public string totalAreas { get; set; }
        public string depositAmount { get; set; }
        public string totalAmountToBePaid { get; set; }
        public string daysLeftforLaps { get; set; }
    }

    public class RenewableAgentsClientsInfoResponse
    {
        public List<Output> output { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
}

namespace AIA.Life.Models.Integration.Payment.Inforce
{
    #region Renewed Inforce Policies
    public class RenewedInforcePoliciesDetailsAPIRequest
    {
        public string companyCode { get; set; }
        public string agentCode { get; set; }
        public object fromDate { get; set; }
        public string toDate { get; set; }
        public string proposerCode { get; set; }
    }
    public class Output
    {
        public string salutation { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string mobileTelNo { get; set; }
        public string homeTelNo { get; set; }
        public string workTelNo { get; set; }
        public string email { get; set; }
        public string policyNo { get; set; }
        public string premiumDueDate { get; set; }
        public string premiumAmount { get; set; }
        public string depositAmount { get; set; }
        public string totalAmountToBePaid { get; set; }
    }
    public class RenewedInforcePoliciesDetailsAPIResponse
    {
        public List<Output> output { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
    #endregion
}

namespace AIA.Life.Models.Integration.Payment.InGrace
{
    #region Renewed Policies In Grace period
    public class RenewedInGracePoliciesDetailsAPIRequest
    {
        public string companyCode { get; set; }
        public string agentCode { get; set; }
        public object fromDate { get; set; }
        public string toDate { get; set; }
        public string proposerCode { get; set; }
    }

    public class Output
    {
        public string salutation { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string mobileTelNo { get; set; }
        public string homeTelNo { get; set; }
        public string workTelNo { get; set; }
        public string email { get; set; }
        public string policyNo { get; set; }
        public string premiumDueDate { get; set; }
        public string premiumAmount { get; set; }
        public string lastPaymentDate { get; set; }
        public string depositAmount { get; set; }
        public string totalAmountToBePaid { get; set; }
        public string daysLeftforLaps { get; set; }
    }

    public class RenewedInGracePoliciesDetailsAPIResponse
    {
        public List<Output> output { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
    #endregion
}

namespace AIA.Life.Models.Integration.Payment.RunningLapse
{
    #region Renewed Policies RunningLapse
    public class RenewedRunningLapsePoliciesDetailsAPIRequest
    {
        public string companyCode { get; set; }
        public string agentCode { get; set; }
        public object fromDate { get; set; }
        public string toDate { get; set; }
        public string proposerCode { get; set; }
    }

    public class Output
    {
        public string salutation { get; set; }
        public string firstName { get; set; }
        public string middlename { get; set; }
        public string lastName { get; set; }
        public string mobileTelNo { get; set; }
        public string homeTelNo { get; set; }
        public string workTelNo { get; set; }
        public string email { get; set; }
        public string policyNo { get; set; }
        public string premiumDueDate { get; set; }
        public string premiumAmount { get; set; }
        public string lastPaymentDate { get; set; }
        public string noOfunpaidPremiums { get; set; }
        public string totalAreas { get; set; }
        public string totalLateFees { get; set; }
        public string depositAmount { get; set; }
        public string totalAmountToBePaid { get; set; }
        public string daysLeftforLaps { get; set; }
    }

    public class RenewedRunningLapsePoliciesDetailsAPIResponse
    {
        public List<Output> output { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
    #endregion
}

namespace AIA.Life.Models.Integration.Payment.LapsedPolicies
{
    #region Renewal Lapsed Policies
    public class RenewedLapsedPoliciesDetailsAPIRequest
    {
        public string companyCode { get; set; }
        public string agentCode { get; set; }
        public object fromDate { get; set; }
        public string toDate { get; set; }
        public string proposerCode { get; set; }
    }
    public class Output
    {
        public string salutation { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string mobileTelNo { get; set; }
        public string homeTelNo { get; set; }
        public string workTelNo { get; set; }
        public string email { get; set; }
        public string policyNo { get; set; }
        public string lapsedOn { get; set; }
        public string premiumDueDate { get; set; }
        public string premiumAmount { get; set; }
        public string lastPaymentDate { get; set; }
        public string noOfunpaidPremiums { get; set; }
        public string totalAreas { get; set; }
        public string totalLateFees { get; set; }
        public string depositAmount { get; set; }
        public string totalAmountToBePaid { get; set; }
    }

    public class RenewedLapsedPoliciesDetailsAPIResponse
    {
        public List<Output> output { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
    #endregion
}