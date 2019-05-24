using AIA.Life.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace AIA.Life.Models.UserManagement
{
    public class ImdcodeCreationModel
    {
        public decimal ImdDetailsID { get; set; }
        [XmlIgnore]
        public IEnumerable<GEBDetails> lstgebDetails { get; set; }
        [XmlIgnore]
        public IEnumerable<UINDetails> lstuinDetails { get; set; }
        public BankDetails bankdetails { get; set; }
        [XmlIgnore]
        public IEnumerable<CommissionDetails> lstCommissionDetails { get; set; }
        [XmlIgnore]
        public IEnumerable<BranchDetails> lstBranchDetails { get; set; }
        public decimal ImdType { get; set; }
        public decimal ImdStatus { get; set; }
        public string Remarks { get; set; }
        public string Clientcode { get; set; }
        public string ImdDescription { get; set; }
        public string ClientType { get; set; }
        //for client search
        public short tempClientType { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string CorporateName { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> MaritalStatus { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> GenderList { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> SalutationList { get; set; }
        public DateTime? ImdDoB { get; set; }
        public string Contact { get; set; }
        public Common.Address AddressLine1 { get; set; }
        [XmlIgnore]
        public IEnumerable<SelectListItem> Pincode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public decimal? officeTelephone { get; set; }
        public string tempTelephoneNo { get; set; }
        public decimal? Mobile { get; set; }
        public string tempMobileNo { get; set; }
        public string Pan { get; set; }
        public string email { get; set; }
        [XmlIgnore]
        public IEnumerable<SelectListItem> EmployeeCode { get; set; }
        public string LicenceNumber { get; set; }
        public decimal FGChannel { get; set; }
        [XmlIgnore]
        public IEnumerable<SelectListItem> IrdaChannel { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public DateTime? LicenseIssueDate { get; set; }
        public DateTime? LicenseExpiryDate { get; set; }
        public DateTime? ImdTerminationDate { get; set; }
        public string CommisionTable { get; set; }
        public string IrdaCode { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string EmployeeName { get; set; }
        public string SmCode { get; set; }
        public string NewSmCode { get; set; }
        public string NewSmName { get; set; }
        //public string mappingcode1 { get; set; }--------Commented by Ganesh
        //public string mappingcode2 { get; set; }
        public bool? DebtorType { get; set; }
        public int? DTerritory { get; set; }
        public Guid? Createdby { get; set; }
        public string createdByName { get; set; }
        public Guid? modifiedBy { get; set; }
        public string modifiedByName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string Imdcode { get; set; }
        public string ImdName { get; set; }
        public string PanHolderName { get; set; }
        public bool Panverified { get; set; }
        public string panVerifiedBy { get; set; }
        public DateTime? PanVerificationDate { get; set; }
        public ImdLoginAuthentication LoginAuthentication { get; set; }
        //public decimal marital { get; set; }
        //public string gender1 { get; set; }
        //public string salutation1 { get; set; }
        [XmlIgnore]
        public IEnumerable<SelectListItem> ListIMDType { get; set; }
        public string IRDA { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstChannel { get; set; }
        // public CommissionDetails commission { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstIRDAChannel { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstTerritory { get; set; }
        public string SearchClientCode { get; set; }
        public int ClientID { get; set; } // added
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public string ClientPAN { get; set; }
        public string ClientCorporateName { get; set; }
        [XmlIgnore]
        public IEnumerable<ImdcodeCreationModel> ListData { get; set; }
        [XmlIgnore]
        public IEnumerable<ImdcodeCreationModel> ClientData { get; set; }
        // public CommissionDetails commissionDetails { get; set; }
        public bool GEBFlag { get; set; }

        //commissiondetails ddls
        [XmlIgnore]
        public IEnumerable<CommissionDetails> CommissionData { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstPremiumClass { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstContractType { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstCommission { get; set; }
        //Branchdetails ddls
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstBranchCode { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstEmpcode { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstSMCode { get; set; }

        public List<MasterListItem> lstIMDStatus { get; set; }
        public List<MasterListItem> lstPaymentType { get; set; }
        public List<MasterListItem> lstAcountType { get; set; }
        public string ClientStatusCode { get; set; }
        public string CompanyDoctorIndicator { get; set; }
        //Added for IMDHistory
        public IMDHistory imdHistory { get; set; }
        //List Properties
        public List<BranchDetails> listBranchDetails { get; set; }
        public List<ProductDetails> listProductDetails { get; set; }
        public List<CommissionDetails> listCommissionDetails { get; set; }
        public List<UINDetails> listUINDetails { get; set; }
        public List<GEBDetails> listGEBDetails { get; set; }

        public string Gender { get; set; }
        public string MarriedIndicator { get; set; }
        public string Nationality { get; set; }
        public string Salutation { get; set; }
        public string ServicingBranch { get; set; }
        public DateTime? InceptionDate { get; set; }
        public string CLNType { get; set; }
        public string NameFormat { get; set; }
        //Added for Address
        public string addrType { get; set; }
        public string preferedContact { get; set; }
        //Added for Address
        public string actn { get; set; }
        public DateTime? dateEnd { get; set; }
        public string licence { get; set; }
        public string stca { get; set; }
        public string agentStatus { get; set; }
        public string stce { get; set; }
        public string zgebflag { get; set; }
        public string clientNumber { get; set; }
        public string accRecon { get; set; }
        public string agentBranch { get; set; }
        public string accountNumber { get; set; }
        public string bankIndicator { get; set; }
        public string district { get; set; }
        public string outstandingIndicator { get; set; }
        public string debitNoteToAgentFlag { get; set; }
        public string repLvla { get; set; }
        public string commTabl { get; set; }
        public string debtType { get; set; }
        public string transferCase { get; set; }
        public string debtorBrokerFlag { get; set; }
        public string territoryCode { get; set; }
        public string autoMarryFlag { get; set; }
        public string seqNo { get; set; }
        //public string reportingLevel { get; set; }

        //Added for storing branches
        public string branchObj { get; set; }

        //TDS option details
        public TDSDetails tdsDetails { get; set; }

        //85/88- IMD creation Enhancement
        public bool? isTransferCase { get; set; }
        public string reportsTo { get; set; }
        public string aadhaarNo { get; set; }
        public string productObj { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstLobMaster { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstProductMaster { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstContractTypeMaster { get; set; }
        [XmlIgnore]
        public IEnumerable<ProductDetails> lstProductDetails { get; set; }

        //View More Branch Details
        public bool isViewMore { get; set; }

        public ImdcodeCreationModel()
        {
            ClientStatusCode = "AC";
            CompanyDoctorIndicator = "N";
            //Gender = "M";
            //MarriedIndicator = "S";
            Nationality = "IND";
            //Salutation = "MR";
            ServicingBranch = "11";
            //CLNType = "N";
            //ClientID = 40074581;
            NameFormat = "2";
            addrType = "B";
            preferedContact = "M";
            actn = "A";
            stca = "S";
            agentStatus = "A";
            clientNumber = "~";
            accRecon = "01";
            agentBranch = "X";
            accountNumber = "~";
            bankIndicator = "01";
            district = "001";
            outstandingIndicator = "N";
            debitNoteToAgentFlag = "N";
            repLvla = "1";
            commTabl = "TA039";
            transferCase = "N";
            zgebflag = "N";
            autoMarryFlag = "N";
        }

    }

    public class GEBDetails
    {
        public DateTime? Datefrom { get; set; }
        public DateTime? Dateto { get; set; }
        public decimal? Ricession { get; set; }
        public decimal? Ricommission { get; set; }
        public string Index { get; set; }
    }
    public class UINDetails
    {
        public DateTime? Datefrom { get; set; }
        public DateTime? Dateto { get; set; }
        public string uinNumber { get; set; }
        public string Index { get; set; }
    }
    public class CommissionDetails
    {
        public string premiumclass { get; set; }
        public string ContractType { get; set; }
        public string Commission { get; set; }
        public string Index { get; set; }
    }
    public class BankDetails
    {
        public string PaymentType { get; set; }
        public string AccountType { get; set; }
        //public decimal? AccountNumber { get; set; }
        public string AccountNumber { get; set; }//changed by Akshay Conflict in type PAsia 27/05/2016
        public string SecurityCode { get; set; }
        public string IFSCCode { get; set; }
        public string MICRCode { get; set; }
        public string BankCode { get; set; }
        public string BranchName { get; set; }
        public string SwiftCode { get; set; }
    }
    public class TDSDetails
    {
        public decimal TDS { get; set; }
        public bool isLTDS { get; set; }
        public decimal lowTDSRates { get; set; }
        public string lowerTDSCertificateNumber { get; set; }
        public DateTime? validityPeriodFrom { get; set; }
        public DateTime? validityPeriodTo { get; set; }
        public DateTime? LTDSCertificateReceivedDate { get; set; }
        public decimal thresholdAmount { get; set; }
        public string serviceTaxRegistrationNumber { get; set; }
    }
    public class BranchDetails
    {
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string BranchCodeValue { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string SmCode { get; set; }
        public string NewSmCode { get; set; }
        public string NewSmName { get; set; }
        public string Index { get; set; }
        public string company { get; set; }
        public string fgChannel { get; set; }
        public int fgChannelID { get; set; }
        public string imdCode { get; set; }
        public string imdName { get; set; }
        public bool rowIndexID { get; set; }
        public BranchDetails()
        {
            company = "1";
        }
    }
    public class ImdLoginAuthentication
    {
        public string userID { get; set; }
        public string Password { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Pan { get; set; }
        public string EmailID { get; set; }
        public string mobileno { get; set; }
        [XmlIgnore]
        public IEnumerable<SelectListItem> hintQuetn { get; set; }
        public string hintAns { get; set; }

    }
    public class DuplicatePanDetails
    {
        public string Imdcode { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string CorporateName { get; set; }
        public string FGChannel { get; set; }
        public string FGChannelCode { get; set; }
        [XmlIgnore]
        public IEnumerable<DuplicatePanDetails> ListPanDetails { get; set; }
    }
    public class IMDHistory
    {
        public string imdCode { get; set; }
        public string imdName { get; set; }
        public string createdBy { get; set; }
        public DateTime? createDate { get; set; }
        public string modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public DateTime? orderDate { get; set; }//Added by akshay to give order in grid
        [XmlIgnore]
        public IEnumerable<IMDHistory> ListIMDHistory { get; set; }
    }
    public class CustomerDetails
    {
        public int CustomerID { get; set; }
        public decimal? AdressID { get; set; }
        public int ClientType { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CorporateName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string AltContactno { get; set; }
        public string AltEmailID { get; set; }
        public string Fax { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? Status { get; set; }
        public Guid? UserID { get; set; }
        public string PassportNo { get; set; }
        public int? BusinessType { get; set; }
        public string MiddleName { get; set; }
        public string CustUniqueID { get; set; }
        public decimal? MaritalStatusID { get; set; }
        public decimal? NationalityID { get; set; }
        public string PanNo { get; set; }
        public string AadharNo { get; set; }
    }

    public class NotificationDetails
    {
        public string QuoteNo { get; set; }
        public string BranchCode { get; set; }
        public string createdBy { get; set; }
        public DateTime? createDate { get; set; }
        public string ProductCode { get; set; }
        public string QuoteStatus { get; set; }
        public string Reason { get; set; }
        public string Remarks { get; set; }
        [XmlIgnore]
        public IEnumerable<NotificationDetails> ListNotifications { get; set; }
    }
    public class ProductDetails
    {
        public string lob { get; set; }
        public string product { get; set; }
        public string contractType { get; set; }
        public string sumInsuredLimit { get; set; }
        public decimal sumInsuredLimitDecimal { get; set; }
        public string Index { get; set; }
        public int permissionId { get; set; }
    }
}