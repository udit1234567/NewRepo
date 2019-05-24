using AIA.Life.Models.Common;
using AIA.Life.Models.UWDecision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using System.Web.UI.WebControls;

namespace AIA.Life.Models.Policy
{
    public class Policy
    {
        public Policy()
        {
            lstGender = new List<Common.MasterListItem>();
            lstOccupation = new List<Common.MasterListItem>();
            objMemberDetails = new List<MemberDetails>();
            objDocumentUpload = new List<DocumentUpload>();
            LstBenifitDetails = new List<BenifitDetails>();
            lstDependentRelationship = new List<MasterListItem>();
            objFillMemberDetials = new MemberDetails();
            objProspectDetails = new MemberDetails();
            ObjLifeAssuredOtherInsurance = new List<LifeAssuredOtherInsurance>();
            Error = new Error();
            FollowUps = new List<FollowUp>();
            ObjAIALifeAssuredOtherInsurance = new List<AIALifeAssuredOtherInsurance>();
            ObjNomineeLifeAssuredDetails = new List<NomineeLifeAssuredDetails>();
            ObjectAIALifeAssuredOtherInsurance = new AIALifeAssuredOtherInsurance();
        }
        public string MainLifeAdditionalQuestion { get; set; }
        public string SpouseAdditionalQuestion { get; set; }
        // Added for AIA Demo
        public string RefNo { get; set; }
        public string CreatedBy { get; set; }  
        public List<Illustation> LstIllustation { get; set; }
        public List<UWInbox> LstUWInbox { get; set; }
        public string ServiceTraceID { get; set; }
        // till here
        // WealthPlannerQuestions
       // public List<WPQuestionsList> objLstWealthPlannerQuestions { get; set; }
        // Till here
        //Added Previous Insurance Details
        public int OnGoingProposalWithAIA { get; set; }
        public int NoOfOnGoingProposalWithAIA { get; set; }
        public int PreviousPolicyWithAIA { get; set; }
        public int NoOfPreviousPolicyWithAIA { get; set; }
        public List<LifeAssuredOtherInsurance> ObjLifeAssuredOtherInsurance { get; set; }
        public List<AIALifeAssuredOtherInsurance> ObjAIALifeAssuredOtherInsurance { get; set; }
        public AIALifeAssuredOtherInsurance ObjectAIALifeAssuredOtherInsurance { get; set; }

        public List<NomineeLifeAssuredDetails> ObjNomineeLifeAssuredDetails { get; set; }

        //Till here

        //Smart Pensions
        public string SmartPensionReceivingPeriod { get; set; }
        public string SmartPensionMonthlyIncome { get; set; }
        //till here
        // Added MaturityBenefit Filed 
        // Proposal From
        public string AgentCode { get; set; }
        public string BizDate { get; set; }
        public string MaturityBenefits { get; set; }
        public string Years { get; set; }
        public string TotalAnnualPremiumContribution { get; set; }
        public string ProposalDepositPremium { get; set; }
        public string BankName { get; set; }
        public string CreditCardNo { get; set; }
        public string BankAccountNo { get; set; }
        public string BranchName { get; set; }


        public int SelectedPolicy { get; set; }
        // till here
        public string Prefix { get; set; }
        public string UserName { get; set; }
        public string UserType { get; set; }
        public int ContactID { get; set; }
        public List<BenifitDetails> LstBenifitDetails { get; set; }
        public List<BenifitDetails> LstPremiumDetails { get; set; }
        public List<string> LstMedicalReports { get; set; }
        public List<string> ListAssured { get; set; }
        public List<MasterListItem> DropDownMemberDetails { get; set; }
        public decimal? AnnualPremium { get; set; }
        public decimal? HalfYearlyPremium { get; set; }
        public decimal? QuaterlyPremium { get; set; }
        public decimal? MonthlyPremium { get; set; }
        public decimal? Cess { get; set; }
        public decimal? VAT { get; set; }
        public decimal? PolicyFee { get; set; }

        public bool IsIntermSave { get; set; }
        public bool CounterOfferCase { get; set; }

        public string Suminsured { get; set; }
        public decimal? Premium { get; set; }
        public decimal? AdditionalPremium { get; set; }
        public string QuoteNo { get; set; }
        public string ProductCode { get; set; }
        public string MedicalLabName { get; set; }

        public bool ProposalFetch { get; set; }
        public string ProposalNo { get; set; }
        public string PlanName { get; set; }
        public string PlanCode { get; set; }
        public string ProposalPath { get; set; }
        public int ProductID { get; set; }
        public string Term { get; set; }
        public string PolicyTerm { get; set; }
        public string PaymentTerm { get; set; }
        public string PaymentFrequency { get; set; }
        public string PaymentPaidBy { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentReceiptPrefferdBy { get; set; }
        public string ModeOfCommunication { get; set; }
        public string others { get; set; }
        public string ProposalNeed { get; set; }


        public string PreferredLanguage { get; set; }
        public MemberDetails objProspectDetails { get; set; }
        public MemberDetails objFillMemberDetials { get; set; }
        public bool IsRegAsCommunication { get; set; }
        public string Message { get; set; }

        //UW 
        public string UWMessage { get; set; }
        public DateTime? UWDate { get; set; }
        public DateTime? UWDecisionDate { get; set; }
        public DateTime? UWCommencementDate { get; set; }
        public string UWLifeType { get; set; }
        public string MedicalDocuments { get; set; }
        public bool IsCLALetterReq { get; set; }
        public bool IsIllustartionReq { get; set; }
        public string TemplateCode { get; set; }

        // public string 


        public List<MasterListItem> ListPlan { get; set; }
        public List<MasterListItem> LstPolicyTerm { get; set; }

        public string HdnDocumentDetails { get; set; }
        public string AssuredName { get; set; }
        public int AssuredIndex { get; set; }

        public decimal PolicyID { get; set; }
        public string HdnDocument { get; set; }
        public bool ProcceedToPayment { get; set; }
        public PaymentInfo objPaymentInfo { get; set; }
        public bool IsSpouseCovered { get; set; }

        public List<SuspectReAllocation> objSuspectReAllocation { get; set; }
        // tilll here
        public List<MemberDetails> objMemberDetails { get; set; }
        public List<NomineeDetails> objNomineeDetails { get; set; }

        // For Demo
        public int? PolicyStageID { get; set; }
        public int? PolicyStageStatusID { get; set; }

        public string Signature { get; set; }
        public string ProposalSignature { get; set; }

        public byte[] ByteArray { get; set; }
        public byte[] ByteArray2 { get; set; }
        public byte[] ByteArray3 { get; set; }
        public byte[] ByteArray4 { get; set; }
        public string Sign1 { get; set; }
        public string Sign2 { get; set; }
        public string Sign3 { get; set; }
        public string STRHtml { get; set; }
        // Till Here

        // Medical Tests
        public string DoctorName { get; set; }
        public string LabName { get; set; }
        public string PaymentMadeByForDoctor { get; set; }
        public string PaymentMadeByForLab { get; set; }
        public string ReportsTobeSendTo { get; set; }

        // till here


        //Prospect Sign
        public string ProposerSignPath { get; set; }
        public string ProposerSignature { get; set; }
        public string WPProposerSignPath { get; set; }

        public string ProspectImagePath { get; set; }
        public string SpouseSignPath { get; set; }
        public string SpouseImagePath { get; set; }
        public string UploadSignPath { get; set; }



        public byte[] SpouserSignPath { get; set; }
        public byte[] ProspectSignPath { get; set; }
        //till here

        //Added Proposer & Spouse Details
        public DateTime? ProposerDate { get; set; }
        public string ProposerDocumentType { get; set; }
        public string ProposerCountry { get; set; }
        public string ProposerPlace { get; set; }
        public string ProposerNICNo { get; set; }
        public string ProposerAddress { get; set; }
        public string ProposerName { get; set; }
        public bool? ProposerWealthPlanner { get; set; }
        public bool? ProposerWealthPlannerPolicyDateing { get; set; }
        public DateTime? ProposerWealthPlannerPolicyDate { get; set; }
        public string ProposerWealthPlannerComments { get; set; }

        public DateTime? SpouseDate { get; set; }
        public string SpouseDocumentType { get; set; }
        public string SpouseCountry { get; set; }
        public string SpousePlace { get; set; }
        public string SpouseNICNo { get; set; }
        public string SpouseAddress { get; set; }
        public string SpouseName { get; set; }
        public bool? SpouseWealthPlanner { get; set; }
        public bool? SpouseWealthPlannerPolicyDateing { get; set; }
        public DateTime? SpouseWealthPlannerPolicyDate { get; set; }
        public string SpouseWealthPlannerComments { get; set; }

        public bool Declaration { get; set; }
        public bool WPDeclaration { get; set; }
        //Till here

        //Underwriter Review For Demo
        public string ReferredBy { get; set; }
        public string ReferralReason { get; set; }
        public string Decision { get; set; }
        public int NoofDays { get; set; }
        public List<MasterListItem> LstDecision { get; set; }
        public List<MasterListItem> LstReason { get; set; }
        public List<MasterListItem> LstMedicalCodes { get; set; }
        public string Rejection { get; set; }
        public IEnumerable<MasterListItem> LstRejection { get; set; }
        public string UWComments { get; set; }
        public bool IsExlusions { get; set; }
        public string ExlusionsDetails { get; set; }
        public bool IsLoading { get; set; }
        public string LoadingDetails { get; set; }
        public string HealthCheckupCategory { get; set; }
        public IEnumerable<MasterListItem> LstHealthCheckupCategory { get; set; }
        public string Underwriter { get; set; }
        public string UnderwriterName { get; set; }

        public IEnumerable<MasterListItem> LstUnderwriter { get; set; }
        public List<MasterListItem> LstDocument { get; set; }
        public List<MasterListItem> LstAdditionalMedicalDocument { get; set; }
        public List<MasterListItem> LstAdditionalNonMedicalDocument { get; set; }
        public List<MasterListItem> LstUWName { get; set; }
        public Error Error { get; set; }
        public string IsAfc { get; set; }
        //till here

        public List<DocumentUpload> objDocumentUploadedFiles { get; set; }
        public List<DocumentUpload> objDocumentUpload { get; set; }

        [XmlIgnore]
        public List<MasterListItem> ProposalMaritalStatuslist { get; set; }
        [XmlIgnore]
        public List<MasterListItem> MaritalStatuslist { get; set; }
        [XmlIgnore]
        public List<MasterListItem> Nationalities { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstGender { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstOccupation { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstMemberOccupation { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstLanguage { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstSalutation { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstBeneficiarySalutation { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstProposalNeed { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstDocumentName { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstRelations { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstRelationshipwithpolicyowner { get; set; }
        [XmlIgnore]
        public IEnumerable<MasterListItem> lstBeneficiaryRelations { get; set; }
         [XmlIgnore]
        public List<MasterListItem> LstPaymentfrequency { get; set; }
        [XmlIgnore]
        public List<MasterListItem> LstPaymentMethod { get; set; }
        [XmlIgnore]
        public List<MasterListItem> LstPaymentRelations { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstSateofHealth { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstCauseOfDeath { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstFamilyBackGroundRelationship { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstSmokeAndAlcholPer { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstSmokeAndAlcholQuantity { get; set; }
        [XmlIgnore]
        public List<MasterListItem> LstHeightFeets { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstSmokeTypes { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstAlcoholTypes { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstCurrentStatus { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstAerobicExercise { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstFruitOrVegetablePortions { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstFluidOrWater { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstDoctorNames { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstLabNames { get; set; }
        //Added Code By Sharique
        [XmlIgnore]
        public List<MasterListItem> lstHeight { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstWeight { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstCity { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstDependentRelationship { get; set; }
        [XmlIgnore]
        public List<MasterListItem> LstModeofCommunication { get; set; }
        [XmlIgnore]
        public List<MasterListItem> LstPreferredReceipt { get; set; }

        // Added MaturityBenefit List 
        [XmlIgnore]
        public List<MasterListItem> LstMaturityBenefits { get; set; }
        [XmlIgnore]
        public List<MasterListItem> LstResidentialStatus { get; set; }
        [XmlIgnore]
        public List<MasterListItem> LstBenefitYears { get; set; }

        [XmlIgnore]
        public List<MasterListItem> LstFillMemberCountryofOccupation { get; set; }
        //Till Here

        // Added Product Wise Policy Term 
        [XmlIgnore]
        public List<MasterListItem> LstEasyPensionPolicyTerm { get; set; }
        [XmlIgnore]
        public List<MasterListItem> LstSmartPensionPolicyTerm { get; set; }
        [XmlIgnore]
        public List<MasterListItem> LstSmartBuilderPolicyTerm { get; set; }
        [XmlIgnore]
        public List<MasterListItem> LstSmartBuilderPaymentTerm { get; set; }
        [XmlIgnore]
        public List<MasterListItem> LstSmartBuilderGoldPolicyTerm { get; set; }
        [XmlIgnore]
        public List<MasterListItem> LstSmartBuilderGoldPaymentTerm { get; set; }
        [XmlIgnore]
        public List<MasterListItem> LstPriorityValuePolicyTerm { get; set; }
        [XmlIgnore]
        public List<MasterListItem> LstPriorityValuePaymentTerm { get; set; }

        [XmlIgnore]
        public List<MasterListItem> lstPAQAssets { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstPAQLiabilities { get; set; }

        // Till Here
        // Age Proof List
        [XmlIgnore]
        public List<MasterListItem> LstAgeProof { get; set; }
        // Till Here
        [XmlIgnore]
        public List<MasterListItem> LstLifeType { get; set; }

        [XmlIgnore]
        public List<MasterListItem> LstUWStatus { get; set; }
        [XmlIgnore]
        public List<MasterListItem> LstSelectedMedicalDocuments { get; set; }
        public List<UWMedicalDocumentsList> LstMedicalDocumentsTypes { get; set; }
        public List<UWNonMedicalDocumentsList> LstNonMedicalDocumentsList { get; set; }

        public List<FollowUp> FollowUps { get; set; }
        public string MainLifeSign { get; set; }
        public string SpouseLifeSign { get; set; }
        public string WPSign { get; set; }
        public byte[] ProposerSignatureFile { get; set; }
        public byte[] SpouseSignatureFile { get; set; }
        public byte[] WPSignatureFile { get; set; }
        public string ProposalFilePath { get; set; }
        public string SpouseSignature { get; set; }
        public string WPProposerSignature { get; set; }

        public string NomineeRelationshipDetailsID { get; set; }

        public string LeadNo { get; set; }
        public string IntroducerCode { get; set; }
        public bool Deductible { get; set; }
        public string UWReason { get; set; }

        public string ProposalPayablePremium { get; set; }
        public bool IsClaGenerateQuote { get; set; }
        public DateTime RiskCommencementDate { get; set; }

        public string CommencementDate { get; set; }
        public string ProposalDate { get; set; }
        public string ProposalSubmittedDate { get; set; }
    }


    public class SuspectReAllocation
    {
        public int? SuspectReAllocationId { get; set; }
        public string SuspectReAllocationType { get; set; }
        public string SuspectReAllocationName { get; set; }
        public long SuspectReAllocationMobile { get; set; }
        public string SuspectReAllocationHome { get; set; }
        public string SuspectReAllocationWork { get; set; }
        public string SuspectReAllocationEmail { get; set; }
        public string SuspectReAllocationDecision { get; set; }
        public string SuspectReAllocationReportee { get; set; }
    }



    public class ProspectList
    {
        public int? ProspectListId { get; set; }
        public string ProspectListType { get; set; }
        public string ProspectListName { get; set; }
        public long ProspectListMobile { get; set; }
        public string ProspectListHome { get; set; }
        public string ProspectListWork { get; set; }
        public string ProspectListEmail { get; set; }
        public string ProspectListNicNo { get; set; }
    }

    // till here



    public class LifeAssuredMemberDetails
    {
        public int? MemberDetailsId { get; set; }
        public string AssuredName { get; set; }
        public string Relation { get; set; }
        public int? AgeNextBirthday { get; set; }
        public string Gender { get; set; }
    }

    public class LifeAssuredOtherInsurance
    {
        public decimal? OtherInsuranceId { get; set; }
        public string CompanyName { get; set; }
        public string PolicyNo { get; set; }
        public string TotalSAAtDeath { get; set; }
        public string AccidentalBenefitAmount { get; set; }
        public string IllNessBenifit { get; set; }
        public string PermanentDisability { get; set; }
        public string CriticalIllnessBenefit { get; set; }
       // public string Totalpermanentdisability { get; set; }
        public string HospitalizationPerDay { get; set; }
        public string HospitalizationReimbursement { get; set; }
        public string CurrentStatus { get; set; }
        public int RelationShipwithProposer { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class AIALifeAssuredOtherInsurance
    {
        public decimal? OtherInsuranceId { get; set; }
        public string CompanyName { get; set; }
        public string PolicyNo { get; set; }
        public string TotalSAAtDeath { get; set; }
        public string SumSAAtDeath { get; set; }
        public string AccidentalBenefitAmount { get; set; }
        public string SumAccidentalBenefitAmount { get; set; }
        public string IllNessBenifit { get; set; }
        public string SumIllNessBenifit { get; set; }
        public string PermanentDisability { get; set; }
        public string SumPermanentDisability { get; set; }
        public string CriticalIllnessBenefit { get; set; }
        public string SumCriticalIllnessBenefit { get; set; }
        public string HospitalizationPerDay { get; set; }
        public string SumHospitalizationPerDay { get; set; }
        public string HospitalizationReimbursement { get; set; }
        public string CurrentStatus { get; set; }
        public int RelationShipwithProposer { get; set; }
    }

    public class NomineeLifeAssuredDetails
    {
        public int Index { get; set; }
        public decimal MemberID { get; set; }
        public string RelationShipWithPropspectID { get; set; }
        public string RelationShipwithProposer { get; set; }
        public string Salutation { get; set; }
        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = string.IsNullOrEmpty(value) != true ? value.Trim() : value; }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = string.IsNullOrEmpty(value) != true ? value.Trim() : value;
            }
        }
        public string MiddleName { get; set; }
        public string MobileNo { get; set; }
        public string DateOfBirth { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        public string GenderText { get; set; }
        public string MaritialStatus { get; set; }
        public string NewNICNO { get; set; }
        public bool IsLifeAssuredSeleted { get; set; }
    }

    public class NomineeDetails
    {
        public int Index { get; set; }
        public decimal NomineeDetailsId { get; set; }
        public string NomineeName { get; set; }
        public DateTime? NomineeNicNoDOB { get; set; }
        public string NomineeNICNo { get; set; }
        public string NomineeRelationship { get; set; }
        public string NomineePercentage { get; set; }
        public string NomineeSalutation { get; set; }
        public string NomineeInitial { get; set; }
        public string NomineeSurname { get; set; }
        public string NomineeGender { get; set; }
        public string NomineeMaritalStatus { get; set; }
        public string NomineeAddress { get; set; }
        public string NomineeTelephone { get; set; }
        public string NomineeClientCode { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class PaymentInfo
    {
        public PaymentInfo()
        {
            objInstrumentDetails = new List<InstrumentDetails>();
        }
        public decimal? AmountPaid { get; set; }
        public string TransactionNo { get; set; }
        public List<InstrumentDetails> objInstrumentDetails { get; set; }
    }

    public class InstrumentDetails
    {
        public int InstumentID { get; set; }
        public string Name { get; set; }
        public decimal? InstrumentAmount { get; set; }
        public string InstrumentNo { get; set; }
        public DateTime? InstrumentDate { get; set; }
        public string PaymentMode { get; set; }
        public int? BankName { get; set; }
        public string BankBranch { get; set; }

    }

    public class UWMedicalDocumentsList
    {
        public int Index { get; set; }
        public int MedicalDocumentsID { get; set; }
        public string MedicalDocumentsName { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime MedicalDocumentsDate { get; set; }
        public int ISDeleted { get; set; }
    }

    public class UWNonMedicalDocumentsList
    {
        public int Index { get; set; }
        public int MedicalDocumentsID { get; set; }
        public string MedicalDocumentsName { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime NonMedicalDocumentsDate { get; set; }
        public int ISDeleted { get; set; }
    }

    public class OCRResponse
    {
        public string ServiceTraceID { get; set; }
        public string UserName { get; set; }
        public string NIC_Number { get; set; }
        public string DocType { get; set; }
        public string Filedata { get; set; }
        public string ErrorMessage { get; set; }
        public decimal ErrorCode { get; set; }    
            
    }

    public class ProposalMasters
    {
        public List<MasterListItem> ListPlan { get; set; }
        public List<MasterListItem> LstPolicyTerm { get; set; }       
        public List<MasterListItem> LstDecision { get; set; }
        public List<string> LstReason { get; set; }     
        public IEnumerable<MasterListItem> LstRejection { get; set; }     
        public IEnumerable<MasterListItem> LstHealthCheckupCategory { get; set; }     
        public IEnumerable<MasterListItem> LstUnderwriter { get; set; }
        public List<MasterListItem> LstDocument { get; set; }
        public List<MasterListItem> LstAdditionalMedicalDocument { get; set; }
        public List<MasterListItem> LstAdditionalNonMedicalDocument { get; set; }
        public List<MasterListItem> LstUWName { get; set; }     
        public List<DocumentUpload> objDocumentUploadedFiles { get; set; }
        public List<DocumentUpload> objDocumentUpload { get; set; }
        public List<MasterListItem> ProposalMaritalStatuslist { get; set; }
        public List<MasterListItem> MaritalStatuslist { get; set; }
        public List<MasterListItem> Nationalities { get; set; }
        public List<MasterListItem> lstGender { get; set; }
        public List<MasterListItem> lstOccupation { get; set; }
        public List<MasterListItem> lstMemberOccupation { get; set; }
        public List<MasterListItem> lstLanguage { get; set; }
        public List<MasterListItem> lstSalutation { get; set; }
        public List<MasterListItem> lstBeneficiarySalutation { get; set; }
        public List<MasterListItem> lstProposalNeed { get; set; }
        public List<MasterListItem> lstDocumentName { get; set; }
        public IEnumerable<MasterListItem> lstRelations { get; set; }
        public IEnumerable<MasterListItem> lstRelationshipwithpolicyowner { get; set; }
        public IEnumerable<MasterListItem> lstBeneficiaryRelations { get; set; }
        public List<MasterListItem> lstPAQAssets { get; set; }
        public List<MasterListItem> lstPAQLiabilities { get; set; }
        public List<MasterListItem> LstPaymentfrequency { get; set; }
        public List<MasterListItem> LstPaymentMethod { get; set; }
        public List<MasterListItem> LstPaymentRelations { get; set; }
        public List<MasterListItem> lstSateofHealth { get; set; }
        public List<MasterListItem> lstCauseOfDeath { get; set; }
        public List<MasterListItem> lstFamilyBackGroundRelationship { get; set; }
        public List<MasterListItem> lstSmokeAndAlcholPer { get; set; }
        public List<MasterListItem> lstSmokeAndAlcholQuantity { get; set; }
        public List<MasterListItem> LstHeightFeets { get; set; }
        public List<MasterListItem> lstSmokeTypes { get; set; }
        public List<MasterListItem> lstAlcoholTypes { get; set; }
        public List<MasterListItem> lstCurrentStatus { get; set; }
        public List<MasterListItem> lstAerobicExercise { get; set; }
        public List<MasterListItem> lstFruitOrVegetablePortions { get; set; }
        public List<MasterListItem> lstFluidOrWater { get; set; }
        public List<MasterListItem> lstDoctorNames { get; set; }
        public List<MasterListItem> lstLabNames { get; set; }
        public List<MasterListItem> lstHeight { get; set; }
        public List<MasterListItem> lstWeight { get; set; }
        public List<MasterListItem> lstCity { get; set; }
        public List<MasterListItem> lstDependentRelationship { get; set; }
        public List<MasterListItem> LstModeofCommunication { get; set; }
        public List<MasterListItem> LstPreferredReceipt { get; set; }
        public List<MasterListItem> LstMaturityBenefits { get; set; }
        public List<MasterListItem> LstResidentialStatus { get; set; }
        public List<MasterListItem> LstAgeProof { get; set; }
        public List<MasterListItem> LstBenefitYears { get; set; }
        public List<MasterListItem> LstFillMemberCountryofOccupation { get; set; }
        public List<MasterListItem> LstEasyPensionPolicyTerm { get; set; }
        public List<MasterListItem> LstSmartPensionPolicyTerm { get; set; }
        public List<MasterListItem> LstSmartBuilderPolicyTerm { get; set; }
        public List<MasterListItem> LstSmartBuilderPaymentTerm { get; set; }
        public List<MasterListItem> LstSmartBuilderGoldPolicyTerm { get; set; }
        public List<MasterListItem> LstSmartBuilderGoldPaymentTerm { get; set; }
        public List<MasterListItem> LstPriorityValuePolicyTerm { get; set; }
        public List<MasterListItem> LstPriorityValuePaymentTerm { get; set; }
        public List<MasterListItem> LstLifeType { get; set; }
        public List<MasterListItem> LstUWStatus { get; set; }
        public List<MasterListItem> LstSelectedMedicalDocuments { get; set; }
        public List<UWMedicalDocumentsList> LstMedicalDocumentsTypes { get; set; }
        public List<UWNonMedicalDocumentsList> LstNonMedicalDocumentsList { get; set; }
    }
}
   






