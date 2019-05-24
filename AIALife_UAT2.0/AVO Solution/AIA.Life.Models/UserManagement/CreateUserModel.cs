using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AIA.Life.Models.Common;
using System.Xml.Serialization;

namespace AIA.Life.Models.UserManagement
{
    public class CreateUserModel
    {
        public CreateUserModel()
        {
            ddlRoles = new List<SelectListItem>();
            LstAuthLimit = new List<MasterListItem>();
            lstUserstatus = new List<MasterListItem>();
        }
        //Added BY Anish
        //public string UserCode { get; set; }
        public bool UserExist { get; set; }
        public string PageName { get; set; }
        public decimal NodeID { get; set; }
        public string UserStatus { get; set; }
        public string DeviceName { get; set; }
        public string DeviceID { get; set; }
        public string ReportingCode { get; set; }
        public int? AuthLimit { get; set; }  
        public List<CreateUserModel> LstCreateUserModel { get; set; }
        public List<DeviceDetails> LstDeviceDetails { get; set; }
        public DeviceDetails objDeviceDetails { get; set; }
        //End
        public List<MasterListItem> LstAuthLimit { get; set; }
        public List<MasterListItem> LstUserRole { get; set; }
        public string IMEINumber { get; set; } 
        public string Designation { get; set; }  
        public List<SelectListItem> ddlRoles { get; set; }
        public string AdvisorCode { get; set; }
        public string AgentName { get; set; }
        public string NICNo { get; set; }
        public string ReportingBranch { get; set; }
        public string ReportingManager { get; set; }
        public string LicenseNumber { get; set; }
        public string Gender { get; set; }
        public IEnumerable<MasterListItem> LstGender { get; set; }
        public string Message { get; set; }
        public string Password { get; set; }
        public List<int?> PermissionIDs { get; set; }
        public List<int?> IndetPerm { get; set; }
        public string permissionType { get; set; }
        public Guid UserID { get; set; }
        public bool IsMenuPermissionSaved { get; set; }
        public string OTP { get; set; }
        public bool Status { get; set; }
        public int EditUserID { get; set; }

        public decimal userDetailsID { get; set; }
        public bool Parent { get; set; }
        public IMDUsers imdusers { get; set; }
        public string UserIdName { get; set; }
        public decimal ReportingUserId { get; set; }
        public string ReportingUserName { get; set; }
        public string EmailId { get; set; }
        public string officeTelNo { get; set; }
        public string MobileNo { get; set; }
        public string Createdby { get; set; }
        public string modifiedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ChildId { get; set; }
        public string UserPhoto { get; set; }
        public int UserPhotoId { get; set; } //Added
        public string Secret_Question { get; set; }
        public string Secret_Answer { get; set; }
        public string IMDCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string corporateName { get; set; }
        public string PanNo { get; set; }
        public List<MasterListItem> ParentUsers { get; set; }
        public bool DeactivateUser { get; set; }
        public bool ReactivateUser { get; set; }
        public string DeactivateStatus { get; set; }
        public string ErrorMessage { get; set; }


        public string UserCode { get; set; }
        public string HierarchyModel { get; set; }
        public bool CDPaymentMode { get; set; }
        public string BalanceAvailable { get; set; }
        public int CreditDays { get; set; }
        public string LockingMethod { get; set; }
        public string Cheque { get; set; }
        public string DD { get; set; }
        public bool CDT { get; set; }
        public bool CD { get; set; }
        public bool IsCdt { get; set; }
        public bool IsFloat { get; set; }
        public string IsCdtMakeLive { get; set; }
        //added by soumya
        
        public UserDetails userDetails { get; set; }
        public UserAccount userAccount { get; set; }
        //Added by Monica
       




        

        //public UserDetails userDetails { get; set; }
        public UserExtension userExtension { get; set; }
        public CDTAccounts cdtAccount { get; set; }
        public OfficeDetails officeDetails { get; set; }
       // public tblUserAccountPermission userAccountPermission { get; set; }
        public CDTAccounts TblCdtccountObj { get; set; }
        public int txtUser_ID { get; set; }
        public string UserName { get; set; }
        public string UserCatagory { get; set; }
        public string MiddleName { get; set; }
        public string photo { get; set; }
        public string TotalAmount { get; set; }
        public decimal AssignedAmount { get; set; }
        public string MaxNoofLevelcreation { get; set; }
        public decimal RemainingAmt { get; set; }
        public decimal agentCode { get; set; }
        public string smCode { get; set; }

        //variables for dropdown lists     
        public string Country_Name { get; set; }
        public string State_Name { get; set; }       
        public int State_ID_PK { get; set; }
        public string City_or_Village_Name { get; set; }      
        public int City_or_Village_ID_PK { get; set; }
        public string District_Name { get; set; }      
        public int District_ID_PK { get; set; }       
        public string Region_Name { get; set; }
        public int Region_ID_PK { get; set; }       
      
        public int Branch_ID_PK { get; set; }
        public string Zone_Name { get; set; }       
        public int ZoneId { get; set; }
        public string catagory {get; set; } 
     
        public string UserFirstName { get; set; }
        public string UserRole { get; set; }

        //Internal User Details
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeRole { get; set; }
        public string EmployeeBranchName { get; set; }
        public string EmployeeGender { get; set; }
        public string EmployeeDepartment { get; set; }
        public DateTime? employeeDOB { get; set; }
        public DateTime? employeeDOJ { get; set; }
        public string employeeBranchLocation { get; set; }
        public string employeeMobile { get; set; }
        public Guid employeeRoleID { get; set; }

        public MasEmployee TblMasEmployeeObj { get; set; }
        public MasBranch TblMasBranchObj { get; set; }


        public string GarageCodeForPortal { get; set; }
        //public string GarageCodeForIntimation { get; set; }
        public string CorporateStateName { get; set; }
        public string CorporateRgiclBranch { get; set; }
      //  public string UserCompanyName { get; set; }
      //  public string UserLicenceNumber { get; set; }

        public string branhCode { get; set; }

        public Address objuserAddress { get; set; }
        public Address objAgencyUserAddress { get; set; }
        public bool isCD { get; set; } 
        public string userBranch { get; set; }
        public string ddlText { get; set; }
        public int ddlValue { get; set; }
        public int? ddlValues { get; set; }
        public string ddlStrValue { get; set; }
        public bool isBackdation { get; set; }
        public int? backdationDays { get; set; }
        public int? quoteValidity { get; set; }
        public Guid? ddlGuidValue { get; set; }
        public bool internalUserMultipleMap { get; set; }
        public bool IsPaymentLink { get; set; }
        //variables for list view with check box
        
        [XmlIgnore]
        public IEnumerable<CreateUserModel> ListHnin { get; set; }
        [XmlIgnore]
        public IEnumerable<CreateUserModel> ListIrc { get; set; }
        [XmlIgnore]
        public IEnumerable<CreateUserModel> ListPytMode { get; set; }
        [XmlIgnore]
        public IEnumerable<CreateUserModel> ListReplenishmentMode { get; set; }
        [XmlIgnore]
        public IEnumerable<CreateUserModel> ListAgent { get; set; }
        [XmlIgnore]
        public IEnumerable<IMDUsers> ListUserData { get; set; }
        public string hdnHninLst { get; set; }
        public string hdnIrcLst { get; set; }
        public string hdnAgentList { get; set; }

        [XmlIgnore]
        public IEnumerable<MasterListItem> lstUserstatus { get; set; }
        [XmlIgnore]
        public IEnumerable<SelectListItem> ListIMDType { get; set; }
        [XmlIgnore]
        public IEnumerable<SelectListItem> ListSMName { get; set; }
        [XmlIgnore]
        public IEnumerable<SelectListItem> ListParentType { get; set; }
        [XmlIgnore]
        public IEnumerable<SelectListItem> ListOffice { get; set; }
        [XmlIgnore]
        public IEnumerable<SelectListItem> ListImdLevel { get; set; }
      
     
        //till here
        public bool IsSelected { get; set; }
        public DateTime? userCreatedDate { get; set; }

        public string RoleName { get; set; }
        public string userType { get; set; }
        
        public string lockedReason { get; set; }

        public string hdnAdditionalContactInfo { get; set; }
        public string appName { get; set; }
       
        public List<CreateUserModel> Items { get; set; }
        public string CommuCode { get; set; }
        public decimal PolCommuCodeId { get; set; }
        public string text { get; set; }
        public string HdnPolicyData { get; set; }
        public int UniqueNo { get; set; }
        public decimal OutStandingCredit { get; set; }
        public string Salutation { get; set; }
        [XmlIgnore]
        public IEnumerable<SelectListItem> ListOfBranch { get; set; }
        [XmlIgnore]
        public IEnumerable<SelectListItem> ListOfUserLevel { get; set; }
        [XmlIgnore]
        public IEnumerable<SelectListItem> ListParent { get; set; }
        [XmlIgnore]
        public IEnumerable<SelectListItem> ListUserType { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> ListSalutation { get; set; }
        public List<checkBoxListValues> ListBoxVendor { get; set; }
        //public List<checkBoxListValues> listofvendor { get; set; }
        public List<checkBoxListValues> ListBoxAgent { get; set; }
       // public List<checkBoxListValues> listofagent { get; set; }
        public List<checkBoxListValues> ListBoxIRC { get; set; }
        public List<checkBoxListValues> listofirc { get; set; }
        //------------------------------CheckBoxListFor Privilages-------------------------------------------
        public List<ListTest> AvaillableReplenishment { get; set; }
        public List<ListTest> SelectedReplenishment { get; set; }
        //------------------------------CheckBoxListFor partialProductview-------------------------------------------
        public List<ListProduct> ProductNameList { get; set; }
        public List<ListProduct> ProductIdList { get; set; }
        // --------------List for permission page-----
        [XmlIgnore]
        public IEnumerable<SelectListItem> ListLockingMethod { get; set; }
        public List<checkBoxListValues> ListBoxRepMode { get; set; }
        public List<IMDUsers> IMDUsersList { get; set; }
        [XmlIgnore]
        public IEnumerable<UserBranchMapping> FgbranchDetails { get; set; }
        //public UserBranchMapping Fgbranch { get; set; }
        [XmlIgnore]
        public IEnumerable<CreateUserModel> ListData { get; set; }
        [XmlIgnore]
        public IEnumerable<CreateUserModel> ClientData { get; set; }
        public UserPhoto userPhotoDetails { get; set; }
        public List<MasterListItem> secretQuestions { get; set; }
        //------------------------------Added for User Branch Mapping Details-------------------------------------------
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstBranchCode { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstBancaBranchCode { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstReceiptingBankCode { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstAreaCode { get; set; }
       
    }
    
   
        public class UserBranchMapping
    {
        public decimal BranchMapId { get; set; }
        public decimal? UserDetailsId { get; set; }
        public string FGBranchCode { get; set; }
        public string FGBancaBranchCode { get; set; }
        public string FGBancaBranchDescription { get; set; }
        public string Receipting { get; set; }
        public string AreaCode { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Index { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> listAreaCode { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> listReceipting { get; set; }
  
    }
    //public class FGBranchDetails
    //{
    //    public string FGBancaBranch_Code { get; set; }
    //    public string FGBranch_Code { get; set; }
    //    public string FGBancaBranch_Description { get; set; }
    //    public string Receipting { get; set; }
    //    public string AreaCode { get; set; }
    //    public string Index { get; set; }
    //}
    public class checkBoxListValues
    {
       public string ChkId { get; set; }
        public string ChkName { get; set; }
        public bool SelectedValue { get; set; }
    }
    public class ListTest//----------Demo CheckBoxListFor-------------
    {
        public int CheckBoxval { get; set; }
        public string Listval { get; set; }
        
        public bool IsSelectAnyMode { get; set; }
    }
    public class TreeView
    {
        public string ItemDesc { get; set; }
        public int ItemId { get; set; }
        public int Parent { get; set; }
        public int depth { get; set; }
        public string ItemType { get; set; }
        public bool? IsSelected { get; set; }
        public bool? IsIndet { get; set; }
        public List<TreeView> items { get; set; }
        public decimal? ProductId { get; set; }
    }
    public class PermissionTree
    {
        public List<TreeView> objTree { get; set; }
        public bool isIMEIChecked { get; set; }
        public string IMEINumber { get; set; }
        public bool isRecommendation { get; set; }
        public bool isRaiseInspection { get; set; }
        public string AppId { get; set; }
        public Guid UserID { get; set; }
    }
    public class ListProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public bool SelectProduct { get; set; }
    }
    public class OfficeDetails
    {
        public string Office { get; set; }
        public string Channel { get; set; }
        public string SM { get; set; }
        public string Referral { get; set; }
    }
    public class IMDUsers
    {
        public string IMDType { get; set; }
        public string IMDCode { get; set; }
        public string IMDName { get; set; }
        public string UserCode { get; set; }
        public string UserIdName { get; set; }
        public string FGBranch { get; set; }
        public DateTime? DateOfCreation { get; set; }
        public string IMDStatus { get; set; }
        public int NodeID { get; set; }
        public bool isEditReq { get; set; }       
        [XmlIgnore]
        public IEnumerable<MasterListItem> ListBranchData { get; set; }
        public string FGChannel { get; set; }
    }

    public class UserPhoto
    {
        public decimal UserPhotoID { get; set; }
        public decimal? UserDetailsID { get; set; }
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public string ContentType { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string userName { get; set; }
        public string gender { get; set; }
    }

    public class ChildIDs
    {
        public string UserCode { get; set; }
        public string UserIdName { get; set; }
        public string ReportingUserId { get; set; }
        public string ReportingUserName { get; set; }
        public decimal NodeID { get; set; }
        public IEnumerable<ChildIDs> ListChildID { get; set; }
    }
    public class SearchInternalUser
    {
        public Guid? UserId { get; set; }
        public int EmpId { get; set; }
        public string StaffCode { get; set; }
        public string Branch { get; set; }
        public string BranchCode { get; set; }
        public string ChannelCode { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }

        public string Function { get; set; }
        public int Department { get; set; }
        public string DepartmentDesc { get; set; }

        public IEnumerable<MasterListItem> lstManagerStaffCode { get; set; }
        public IEnumerable<MasterListItem> lstChannelCode { get; set; }
        public IEnumerable<MasterListItem> lstStaffCode { get; set; }

        public string LineMangStaffCode { get; set; }
        public string[] MultiBranchCode { get; set; }
        public string[] MultiChannelCode { get; set; }

        public string LineManager { get; set; }
        public string SMCode { get; set; }
        public string StaffCorEmailID { get; set; }
        public string StaffName { get; set; }
        [XmlIgnore]
        public IEnumerable<SearchInternalUser> ListInterUser { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstUserFunction { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstUserTitle { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstBranchCode { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstDepartment { get; set; }


        public string EmployeeExcel { get; set; }
        public bool isDisabled { get; set; }
        public bool isResigned { get; set; }
        public bool? isSearch { get; set; }



        public decimal resigDate { get; set; }
        public decimal lWDate { get; set; }
        public string PrimaryBranch { get; set; }
        public string SecondaryBranch { get; set; }
        public string ModifiedOn { get; set; }



        public string resignationDate { get; set; }
        public string lastWorkingDate { get; set; }
        public string TransactionStatus { get; set; }
        public string IMDMappedTo { get; set; }




        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }

    }
    //this class specifically used for internal user download functionality (Offline Excel Format)
    public class DownloadInternalUser
    {
        public string StaffCode { get; set; }
        public string StaffName { get; set; }
        public string Function { get; set; }
        public string Title { get; set; }
        public string BranchCode { get; set; }
        public string ChannelCode { get; set; }
        public string LineManagerStaffCode { get; set; }
        public string LineManager { get; set; }
        public string EmailID { get; set; }
    }

    //this class specifically used for Campaign creation functionality
    public class CreateCampaign
    {
        public decimal CampaignDetailsID { get; set; }
        public string Title { get; set; }
        public DateTime PeriodFrom { get; set; }
        public DateTime PeriodTo { get; set; }
        public int AutoCampaignWithdrawal { get; set; }
        public string ListofAgentseligible { get; set; }
        public bool isSelect { get; set; }
        public bool isActive { get; set; }
        public GridCampaignDocumentDetails campaignDocs { get; set; }
        public int Index { get; set; }
        public string FilesOfTypeA { get; set; }
        public string FilesOfTypeB { get; set; }
        public string FilesType { get; set; }
        public string EmployeeExcel { get; set; }
        public string strPeriodFrom { get; set; }

        public decimal CampaignTarget { get; set; }
        public decimal CampaignGWPAchievement { get; set; }
        public decimal BalancePremiumToBeDoneToQualify { get; set; }
        public decimal NoOfDaysRemaining { get; set; }

        [XmlIgnore]
        public IEnumerable<GridCampaignDocumentDetails> ListFilesOfTypeA { get; set; }
        [XmlIgnore]
        public IEnumerable<GridCampaignDocumentDetails> ListFilesOfTypeB { get; set; }
        [XmlIgnore]
        public IEnumerable<CreateCampaign> ListCampaignData { get; set; }
    }

    public class GridCampaignDocumentDetails
    {
        public string NameOfTheDocument { get; set; }
        public DateTime DateOfUpload { get; set; }
        public string StrDateOfUpload { get; set; }
        [XmlIgnore]
        public IEnumerable<GridCampaignDocumentDetails> ListCampaignDocumentDetails { get; set; }
    }
    public class DeviceDetails
    {
        public string DeviceID { get; set; }
        public string DeviceName { get; set; }
        public string UPdatedDate { get; set; }
        public string UserID { get; set; }
    }
    public class AppNotification
    {
        public AppNotification()
        {
            LstSubRecepient = new List<MasterListItem>();
        }
        public IEnumerable<MasterListItem> LstRecepient { get; set; }
        public List<MasterListItem> LstSubRecepient { get; set; }
        public string AllDeviceTokenID { get; set; }
        public int SelectRecepient { get; set; }
        public string SelectSubRecepient { get; set; }
        public bool ISMandatory { get; set; }
        public string Message { get; set; }
        public string VersionNo { get; set; }
        public string UploadPath { get; set; }   
        public string WhatsNew { get; set; }
        public string Result { get; set; }
        public string SearchRecepient { get; set; }
        public decimal? HdnNodeID { get; set; }
        public string HdnLSTRecepient { get; set; }
        public string ScheduleDate { get; set; }
        public string ScheduleTime { get; set; }
        public List<UserSearch> lstUserSearch { get; set; }
    }    
    public class FCMResponse
    {
        public long multicast_id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
        public int canonical_ids { get; set; }
        public List<FCMResult> results { get; set; }
    }
    public class FCMResult
    {
        public string message_id { get; set; }
    }
    public class UserSearch
    {
        public decimal NodeId { get; set; }
        public string  Value { get; set; }
        public Guid? Userid { get; set; }
        public string DeviceID { get; set; }
        public string DeviceType { get; set; }
        public string ParmData { get; set; }
        public List<UserSearch> LstUserSerch { get; set; }
    }


}
