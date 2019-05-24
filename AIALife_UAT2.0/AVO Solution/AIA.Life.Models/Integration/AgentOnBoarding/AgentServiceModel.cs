using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Integration.AgentOnBoarding
{

    public class AgentMaster
    {
        public string companyCode { get; set; }
        public string agentCode { get; set; }
        public string agentName { get; set; }
        public string natureType { get; set; }
        public string masterAgent { get; set; }
        public string geographicalClassification { get; set; }
        public string status { get; set; }
        public string remarks { get; set; }
        public string createBy { get; set; }
        public string createDt { get; set; }
        public string modifyBy { get; set; }
        public string modifyDt { get; set; }
        public string agentClass { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string districtCode { get; set; }
        public string districtName { get; set; }
        public string provinceCode { get; set; }
        public string provinceName { get; set; }
        public string branchCode { get; set; }
        public string agentNature { get; set; }
        public string appointmentDate { get; set; }
        public string insuranceType { get; set; }
        public string shortName { get; set; }
        public string nicNumber { get; set; }
        public string maritalStatus { get; set; }
        public string sex { get; set; }
        public string generalCommissionStatus { get; set; }
        public string lifeCommissionStatus { get; set; }
        public string groupAgentCode { get; set; }
        public string dateOfBirth { get; set; }
        public string nationality { get; set; }
        public string printName { get; set; }
        public string authorizedUser { get; set; }
        public string authorizedDate { get; set; }
        public string epfNo { get; set; }
        public string sliiRegno { get; set; }
        public string lifeORCStatus { get; set; }
        public string generalORCStatus { get; set; }
        public string primaryLine { get; set; }
        public string termnReason { get; set; }
        public string agentCategory { get; set; }
        public string autoUwrOption { get; set; }
        public string epfNoActual { get; set; }
        public string categoryCode { get; set; }
        public string resignDate { get; set; }
        public string bulkPaymentFlag { get; set; }
        public string payTaxFlag { get; set; }
        public string replacementAgent { get; set; }
    }

    public class AgentAddressDet
    {
        public string addressType { get; set; }
        public string serialNo { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string cityCode { get; set; }
        public string cityName { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public string pinCode { get; set; }
        public string telephone { get; set; }
        public string fax { get; set; }
        public string emailAddressLife { get; set; }
        public string telex { get; set; }
        public string websiteAddress { get; set; }
        public string telephoneResidence { get; set; }
        public string telephoneMobile { get; set; }
        public string pagerNo { get; set; }
        public string emailAddressGeneral { get; set; }
        public string telephoneMobileSecond { get; set; }
    }

    public class SettlementMaster
    {
        public string settlementType { get; set; }
        public string settlementInstitution { get; set; }
        public string settlementAccount { get; set; }
        public string settlementCurrency { get; set; }
        public string contactInfo { get; set; }
        public string settlementBranch { get; set; }
        public string instrumentType { get; set; }
    }

    public class RootObject
    {
        public string userId { get; set; }
        public List<AgentMaster> agentMaster { get; set; }
        public List<AgentAddressDet> agentAddressDet { get; set; }
        public List<SettlementMaster> settlementMaster { get; set; }      
        public string __invalid_name__Message{ get; set; }
        public string __invalid_name__Error{ get; set; }
        public string Status { get; set; }

    }
}
