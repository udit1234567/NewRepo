using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIA.Life.Models.Common;
using System.Xml.Serialization;

namespace AIA.Life.Models.AgentonBoarding
{
    public class RecruitmentAgent
    {
        public int ProspectID { get; set; }
        public string ProspectCode { get; set; }
        public string SuspectCode { get; set; }
        public int Prospectdatacount { get; set; }
        public string Message { get; set; }
        public int SuspectCount { get; set; }
        public string RoleID { get; set; }
        public string RoleName { get; set; }
         public string SuspectBulkUploads { get; set; }     
        public string ConfirmProspectSerializer { get; set; }
        public string SerializerScheduleDate { get; set; }
        public string SerializerScheduleTime { get; set; }
        public int TotalInterviewPoints { get; set; }
        public int TotalInterviewPointsLevel2 { get; set; }
        public int TotalInterviewPointsLevel3 { get; set; }
        public int TotalInterviewPointsLevel4 { get; set; }
        public int TotalInterviewGuidePoints { get; set; }
        public int PendingProspectCount { get; set; }
        public string FianaceCleranceData { get; set; }
        public int InterviewLevel1MarksCount { get; set; }
        public int InterviewLevel2MarksCount { get; set; }
        public int InterviewLevel3MarksCount { get; set; }
        public int InterviewLevel4MarksCount { get; set; }
        public int SuspectID { get; set; }
        public string SuspectLastName { get; set; }
        public long? SuspectMobileNo { get; set; }
        public string SuspectEmailID { get; set; }
        public int InterviewTaskCount { get; set; }
        public int ApprovedCasesCount { get; set; }
        public int CodeCreationCount { get; set; }
        public int IBSLExamCount { get; set; }
        public string FirstName { get; set; }
        public string GetInterviewList { get; set; }
        public int InterviewCountLevel1 { get; set; }
        public int InterviewCountLevel2 { get; set; }
        public int InterviewCountLevel3 { get; set; }
        public int InterviewCountLevel4 { get; set; }
        public bool IsInterviewCountLevel2 { get; set; }
        public bool IsInterviewCountLevel3 { get; set; }
        public bool IsInterviewCountLevel4 { get; set; }
        public string GetDocumentList { get; set; }
        public int FileUploadCount { get; set; }
        public int DocumentAprrovalCount { get; set; }
        public int RemarksCount { get; set; }
        public string UnitLeader { get; set; }
        public string TeamLeader { get; set; }
        public string FSA { get; set; }
        public string MiddleName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        public string EmailID { get; set; }
        public string CreatedBy { get; set; }
        public decimal? PhoneNum { get; set; }
        public string Title { get; set; }
        public int? Age { get; set; }    
        public int? InterviewerAge { get; set; }
        public string AgentCode { get; set; }
        public string NICNO { get; set; }
        public string SLIIRegNo { get; set; }
        public string RecruitmentType { get; set; }
        public string CareerLevel { get; set; }
        public int MathematicsCount { get; set; }
        public int LanguageCount { get; set; }
        public int SubjectFailCount { get; set; }
        public string QualificationValidate { get; set; }
        public string SubjectValidate { get; set; }
        public string GradeValidate { get; set; }
        public string CareerLevelInterview { get; set; }
        public string OfficialCareerLevel { get; set; }
        public string Designation { get; set; }
        public string AgentType { get; set; }
        public bool IsAgentPendingProspect { get; set; }
        public bool IsAgentConfirmProspect { get; set; }
        public bool IsAgentInterviewSchedule { get; set; }
        public bool IsAgentInterviewReSchedule { get; set; }
        public string Salutation { get; set; }
        public List<RecruitmentAgent> ObjRecruitmentAgent { get; set; }
        public IEnumerable<MasterListItem> LstRecruitmentType { get; set; }
        public IEnumerable<MasterListItem> LstCareerLevel { get; set; }
        public IEnumerable<MasterListItem> LstDesignation { get; set; }
        public IEnumerable<MasterListItem> LstAgentType { get; set; }
        public IEnumerable<MasterListItem> LstSalutation { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string QualificationDetails { get; set; }
        public IEnumerable<MasterListItem> LstGender { get; set; }
        public IEnumerable<MasterListItem> LstMaritalStatus { get; set; }
        public IEnumerable<MasterListItem> LstQualificationDetails { get; set; }
        public IEnumerable<MasterListItem> LstHighestQualification { get; set; }
        public string QualificationSpecify { get; set; }
        public string Year_MonthPassed { get; set; }
        public string SLIIQualification { get; set; }
        public string Grade { get; set; }
        public string SchoolName { get; set; }
        public IEnumerable<MasterListItem> LstSLIIQualification { get; set; }
        public string Nationality { get; set; }
        public string Profession { get; set; }
        public IEnumerable<MasterListItem> LstNationality { get; set; }
        public string Salesexp { get; set; }
        public string Industryexp { get; set; }
        public IEnumerable<MasterListItem> LstSalesexp { get; set; }
        public IEnumerable<MasterListItem> LstProfessionexp { get; set; }        
        public IEnumerable<MasterListItem> LstIndustryexp { get; set; }
        public IEnumerable<MasterListItem> LstPresentProfession { get; set; }
        public string SpecialAcheievemts { get; set; }
        public string TraningGridData { get; set; }
        public string PreviousInsurerName { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public string ApplicationNo { get; set; }
        public string AppointedDate { get; set; }
        public string BankName { get; set; }
        public string BankBranchCode { get; set; }
        public string BankBranchName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string AccountNo { get; set; }
        public long? MobileNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        //public int? Pincode { get; set; }
        public string Pincode { get; set; }
        public string Distric { get; set; }
        public string County { get; set; }
        public string City_Town { get; set; }
        public string Provience { get; set; }
        public bool IsRegAsCommunication { get; set; }
        public string Position { get; set; }
        public string EmployeeOfficialDetailsPosition { get; set; }
        public string PositionInterview { get; set; }
        public IEnumerable<MasterListItem> LstPosition { get; set; }
        public IEnumerable<MasterListItem> LstEmployeeOffcialDtlPosition { get; set; }
        public string InterviewLevels { get; set; }
        public string InterviewCode { get; set; }
        public string InterviewName { get; set; }
        public string InterviewNotes { get; set; }
        public IEnumerable<MasterListItem> LstInterviewlevels { get; set; }
        public IEnumerable<MasterListItem> LstInterviewGuideArea { get; set; }
        public IEnumerable<MasterListItem> LstInterviewGuideCategory { get; set; }
        public IEnumerable<MasterListItem> LstInterviewGuide { get; set; }
        public IEnumerable<MasterListItem> LstInterviewGuidePoints { get; set; }
        public IEnumerable<MasterListItem> LstInterviewGuideRemarks { get; set; }
        public List<InterviewGuide> LstGridInterviewGuide { get; set; }
        public List<InterviewGuideTasksData> ObjGridInterviewGuideTasksData { get; set; }
        public List<CodeCreationTasksData> LstGridCodeCreationTasksData { get; set; }
        public List<IBSLExamTasksData> LstGridIBSLExamTasksData { get; set; }
        public List<SuspectTaskDetails> LstSuspectTaskDetails { get; set; }
        public List<ProspectTaskDetails> LstPendingProspectTaskDetails { get; set; }        
        public List<ProspectTaskDetails> LstProspectTaskDetails { get; set; }
        public int? ProspectConfirmedCount { get; set; }
        public List<ChallengerHeader> ObjChallengerHeader { get; set; }
        public List<ChallengerBody> ObjChallengerBody { get; set; }
        public List<Challengerrenewal> ObjChallengerrenewal { get; set; }
        public string InterviewTaskdata { get; set; }
        public string InterviewerResult { get; set; }
        public List<MasterListItem> LstInterviewerResult { get; set; }
        public string Interviewlevelreferto { get; set; }
        public string InterviewerDesignation { get; set; }
        public string InterviewerDesignationInterview { get; set; }
        public IEnumerable<MasterListItem> LstInterviewerDesignation { get; set; }
        public string InterviewerSalary { get; set; }
        public string InterviewerTravel { get; set; }
        public string InterviewBranchCode { get; set; }
        public string InterviewReportTo { get; set; }
        public List<GeneralChallengeBody> ObjGeneralChallengedata { get; set; }
        public List<EducationalDetails> LstEducationalDetails { get; set; }
        public IEnumerable<MasterListItem> LstQualification { get; set; }
        public IEnumerable<MasterListItem> LstSubjects { get; set; }
        public IEnumerable<MasterListItem> LstGrade { get; set; }
        public List<TrainingDetails> ObjTrainingDetails { get; set; }
        public List<DocumentDetails> LstdocumentName { get; set; }
        public string Filedetails { get; set; }
        public List<DocumentDetails> LstHOApproval { get; set; }
        public List<SearchNICDetails> ObjSearchNICDetails { get; set; }
        public List<CleranceFinaceDepartment> LstCleranceFinaceDepartment { get; set; }
        public string Clearancefinancedepartment { get; set; }
        public List<EvaluationDetails> LstEvaluationDetails { get; set; }
        public string SearchAction { get; set; }
        public IEnumerable<MasterListItem> LstSearchAction { get; set; }
        public string SearchSubAction { get; set; }
        public IEnumerable<MasterListItem> LstSearchSubAction { get; set; }
        public List<MasterListItem> LstNomineeRelation { get; set; }
        public string EmployeeType { get; set; }
        public string EmployeeID { get; set; }
        public string CompanyCode { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string Status { get; set; }
        public string PrintName { get; set; }
        public string EPFNo { get; set; }
        public string OldEPFNo { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string PreviousAdvisorCode { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string ReportingCode { get; set; }
        public string ReportingName { get; set; }
        public string OfficialPosition { get; set; }
        public string ReportingPosition { get; set; }
        public string OfficialDesignation { get; set; }
        public string ReportingDesignation { get; set; }
        public DateTime? ReportingEffectiveFrom { get; set; }
        public IEnumerable<MasterListItem> LstEmployeeType { get; set; }
        public IEnumerable<MasterListItem> LstStatus { get; set; }
        public string HistoryDetails { get; set; }
        public IEnumerable<MasterListItem> LstHistoryDetails { get; set; }
        public List<RecruitmentHistoryDetails> LstRecruitmentHistoryDetails { get; set; }
        public List<RecruitmentStatusDetails> LstRecruitmentStatusDetails { get; set; }
        public string Presentprofession { get; set; }
        public string NeedforIncome { get; set; }
        public IEnumerable<MasterListItem> LstNeedforIncome { get; set; }
        public string OthersNeedforIncome { get; set; }
        public long? AltMobileNo { get; set; }
        public long? OfficePhNo { get; set; }
        public long? ResidencePhNo { get; set; }
        public string EmergancyFirstName { get; set; }
        public string EmergancyMiddleName { get; set; }
        public string EmergancyLastName { get; set; }
        //public string EmergancyAddress1 { get; set; }
        //public string EmergancyAddress2 { get; set; }
        //public string EmergancyProvience { get; set; }
        //public string EmergancyDistrict { get; set; }
        //public string EmergancyCity_Town { get; set; }
        //public int? EmergancyPincode { get; set; }
        public long? EmergancyMobile { get; set; }
        public long? EmergancyAltMobile { get; set; }
        public string EmergancyEmail { get; set; }
        public string EmergancyRelationship { get; set; }
        public string EmergancyRelationshipOthers { get; set; }
        public IEnumerable<MasterListItem> LstEmergancyRelationship { get; set; }
        public IEnumerable<MasterListItem> LstNonRefOneRelationship { get; set; }
        public IEnumerable<MasterListItem> LstNonRefTwoRelationship { get; set; }        
        public List<MasterListItem> ListSpecialAchievements { get; set; }
        public string FirstCode_EPFNo { get; set; }
        public string FirstRefName { get; set; }
        public string FirstRefCompany { get; set; }
        public string FirstRefDesignation { get; set; }
        public string FirstRefRelation { get; set; }
        public List<MasterListItem> LstFirstRefRelation { get; set; }
        public string SecondCode_EPFNo { get; set; }
        public string SecondRefName { get; set; }
        public string SpecifyProfession { get; set; }
        public string SecondRefCompany { get; set; }
        public string SecondRefDesignation { get; set; }
        public string SecondRefRelation { get; set; }
        public List<MasterListItem> LstSecondRefRelation { get; set; }
        public string NonRefFirstname { get; set; }
        public string NonRefFirstOccupation { get; set; }
        public string NonRefFirstSpecifyOccupation { get; set; }
        public string NonRefSecondSpecifyOccupation { get; set; }
        public List<MasterListItem> LstNonRefFirstOccupation { get; set; }
        public long? NonRefFirstMobNo { get; set; }
        public string NonRefFirstAddressLine1 { get; set; }
        public string NonRefFirstAddressLine2 { get; set; }
        public string NonRefFirstProvience { get; set; }
        public List<MasterListItem> LstNonRefFirstProvience { get; set; }
        public string NonRefFirstDistrict { get; set; }
        public List<MasterListItem> LstNonRefFirstDistrict { get; set; }
        public string NonRefFirstCity_Town { get; set; }
        public List<MasterListItem> LstNonRefFirstCity_Town { get; set; }
        public int? NonRefFirstPincode { get; set; }
        public string NonRefsecondname { get; set; }
        public string NonRefSecondOccupation { get; set; }
        public List<MasterListItem> LstNonRefSecondOccupation { get; set; }
        public long? NonRefSecondMobNo { get; set; }
        public string NonRefSecondAddressLine1 { get; set; }
        public string NonRefSecondAddressLine2 { get; set; }
        public string NonRefSecondProvience { get; set; }
        public List<MasterListItem> LstNonRefSecondProvience { get; set; }
        public string NonRefSecondDistrict { get; set; }
        public List<MasterListItem> LstNonRefSecondDistrict { get; set; }
        public string NonRefSecondCity_Town { get; set; }
        public List<MasterListItem> LstNonRefSecondCity_Town { get; set; }
        public int? NonRefSecondPincode { get; set; }
        public bool IsChkFirstEmployeeRef { get; set; }
        public bool IsChkSecondEmployeeRef { get; set; }
        public List<AgentNominee> LstAgentNominee { get; set; }
        public List<MasterListItem> LstTrainingStatus { get; set; }        
        public string PaymentFor { get; set; }
        public string PaymentNominee { get; set; }                
        public string PaymentMode { get; set; }
        public List<MasterListItem> LstPaymentModes { get; set; }
        public List<MasterListItem> LstPaymentFor { get; set; }
        public List<MasterListItem> LstPaymentNominee { get; set; }
        public List<AgentBankInformation> LstAgentBankInformation { get; set; }
        public List<MasterListItem> LstBranchCode { get; set; }
        public List<MasterListItem> LstBranchName { get; set; }
        public List<MasterListItem> LstBankName { get; set; }
        public List<MasterListItem> LstAgentReferUsers { get; set; }
        public List<RecruitmentAgent> LstBranchBankNames { get; set; }
        public List<RecruitmentAgent> LstBankNamesOnBranchName { get; set; }
        public RecruitmentExam ObjRecruitmentExam { get; set; }
        public List<RecruitmentExam> LstRecruitmentExam { get; set; }
        public List<MasterListItem> LstExamStatus { get; set; }        
        public DateTime ExamDate { get; set; }
        public string ErrorMessage { get; set; }
        public List<InterviewResultView> LstInterviewResultView { get; set; }
        public Address objCommunicationAddress { get; set; }
        public Address objPermanentAddress { get; set; }
        public Address objEmergencyContactAddress { get; set; }
        public Address objReferenceOneDetailsAddress { get; set; }
        public Address objReferenceEmpTwoDetailsAddress { get; set; }
        public List<Address> LstProvince { get; set; }
        public List<Address> LstDistrict { get; set; }
        public List<Address> LstCity { get; set; }
        public List<MasterListItem> LstExamInstitue { get; set; }
        public List<MasterListItem> LstExamMode { get; set; }
        public List<MasterListItem> LstExamLanguage { get; set; }
        public List<MasterListItem> LstExamCenterCode { get; set; }
        public string TrainingType { get; set; }
        public string TrainingMode { get; set; }        
        public DateTime? TrainingStartDate { get; set; }
        public DateTime? TrainingEndDate { get; set; }
        public string TrainingPlace { get; set; }
        public string TrainingCandidateNo { get; set; }
        public string TrainingStatus { get; set; }
        public List<MasterListItem> LstTrainingType { get; set; }
        public List<MasterListItem> LstTrainingMode { get; set; }
        public string InterviewDetailsNotes { get; set; }
        public string HiddenInterviewNotes { get; set; }
        public string ExamGrid { get; set; }
        public bool IsProspectNICBlocked { get; set; }
        public bool IsAgentNICVerificationProspect { get; set; }
        public bool IsCleranceFormOver { get; set; }
        public int NICVerificationCount { get; set; }
        public List<NICVerificationTaskDetails> LstNICVerificationTaskDetails { get; set; }
        public int AuditClearanceID { get; set; }
        public string AuditRecommendation { get; set; }
        public string AuditTarget { get; set; }
        public string AuditAchievement { get; set; }
        public string AuditVariance { get; set; }
        public string AuditRemarks { get; set; }        
        public int HRMClearanceID { get; set; }
        public string HRMDepartment { get; set; }
        public DateTime? HRMResignationDate { get; set; }
        public string HRMReasonForLeaving { get; set; }
        public DateTime? HRMPeriodOfEmployement { get; set; }
        public decimal? HRMPreviousSalary { get; set; }
        public string HRMPreviousTravel { get; set; }
        public string HRMRecommendation { get; set; }
        public int FinanceClearanceID { get; set; }
        public string FinanceRecommendation { get; set; }
        public string ServiceStatus { get; set; }
        public string InterviewInitiator { get; set; }
        public string ProspectStatus { get; set; }
        public string ProspectSubStatus { get; set; }        
    }
    public class InterviewGuide
    {
        public string Area { get; set; }
        public string Category { get; set; }
        public int? Guide { get; set; }
        public string InterviewLevel1 { get; set; }
        public string InterviewLevel2 { get; set; }
        public string InterviewLevel3 { get; set; }
        public string InterviewLevel4 { get; set; }
        public string Remarks { get; set; }
        public int InterviewID { get; set; }

    }
    public class InterviewGuideTasksData
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string ProspectName { get; set; }
        public string ReceivedFrom { get; set; }
        public int Age { get; set; }
        public int InterviewID { get; set; }
        public int ProspectCount { get; set; }
        public string ReceiveAppDate { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public string ScheduleDateIntv { get; set; }
        public DateTime? ScheduleTime { get; set; }
        public string ScheduleTimeIntv { get; set; }
        public bool isTaskID { get; set; }
        public int ProspectID { get; set; }
    }
    public class CodeCreationTasksData
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string ProspectName { get; set; }
        public string ReceivedFrom { get; set; }
        public int Age { get; set; }
        public int InterviewID { get; set; }
        public int ProspectCount { get; set; }
        public string ReceiveAppDate { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public string ScheduleDateIntv { get; set; }
        public DateTime? ScheduleTime { get; set; }
        public string ScheduleTimeIntv { get; set; }
        public bool isTaskID { get; set; }
        public int ProspectID { get; set; }
    }
    public class IBSLExamTasksData
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string ProspectName { get; set; }
        public string ReceivedFrom { get; set; }
        public int Age { get; set; }
        public int InterviewID { get; set; }
        public int ProspectCount { get; set; }
        public string ReceiveAppDate { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public string ScheduleDateIntv { get; set; }
        public DateTime? ScheduleTime { get; set; }
        public string ScheduleTimeIntv { get; set; }
        public bool isTaskID { get; set; }
        public int ProspectID { get; set; }
    }
    public class SuspectTaskDetails
    {
        public string SuspectCode { get; set; }
        public int SuspectID { get; set; }
        public string FirstName { get; set; }
        public string SuspectLastName { get; set; }
        public long? SuspectMobileNo { get; set; }
        public string SuspectEmailID { get; set; }
    }
    public class ProspectTaskDetails
    {
        public bool IsProspectCode { get; set; }
        public int ProspectID { get; set; }
        public string ProspectCode { get; set; }
        public string ProspectFirstName { get; set; }
        public string ProspectLastName { get; set; }
        public long? ProspectMobileNo { get; set; }
        public string ProspectEmailID { get; set; }
        public DateTime? ProspectReceivedDate { get; set; }
        public string ProspectReceivedFrom { get; set; }
        public int? ProspectAge { get; set; }
        public string ProspectTime { get; set; }
        public int SRNo { get; set; }
        public DateTime? InterviewScheduleDate { get; set; }
        public DateTime? InterviewScheduleTime { get; set; }

    }
    public class ChallengerHeader
    {
        public string Chanllengerheader { get; set; }
    }
    public class ChallengerBody
    {
        public string ChallengerBodydata { get; set; }
    }
    public class Challengerrenewal
    {
        public string ChallengerRenewaldata { get; set; }
    }
    public class GeneralChallengeBody
    {
        public string GeneralChallengeData { get; set; }
    }
    public class EducationalDetails
    {
        public int Index { get; set; }
        public decimal? EducationID { get; set; }
        public string Qualification { get; set; }
        public string Subjects { get; set; }
        public string Grade { get; set; }
    }
    public class TrainingDetails
    {
        public int TrainingID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Mode { get; set; }
        public string CandiateNo { get; set; }
        public string Place { get; set; }
        public string Status { get; set; }
    }
    public class DocumentDetails
    {
        public int DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string FileName { get; set; }
        public int Index { get; set; }
        public string DocumentAprroval { get; set; }
        public string Remarks { get; set; }
        public string ExistingFileName { get; set; }
        public byte[] FileContent { get; set; }
    }
    public class SearchNICDetails
    {
        public int? SearchID { get; set; }
        public string NICNO { get; set; }
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string AgentType { get; set; }
        public string DOB { get; set; }
        public string Status { get; set; }
        public decimal? StatusID { get; set; }
        public string SLIIRegNo { get; set; }
        public string Substatus{ get; set; }
        public string SubstatusID { get; set; }

    }
    public class CleranceFinaceDepartment
    {
        public int? FinanceId { get; set; }
        public string FinanceOutstanding { get; set; }
        public Decimal? FinanceAmount { get; set; }
        public string FinanceCheckedby { get; set; }
    }
    public class EvaluationDetails
    {
        public int? EvaluationID { get; set; }
        public string EvaluationCriteria { get; set; }
        public string EvaluationGuide { get; set; }
        public int? EvaluationPoints { get; set; }
    }
    public class RecruitmentHistoryDetails
    {
        public string Code { get; set; }
        public string EntityType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Repository { get; set; }
        public DateTime? DateMode { get; set; }
        public string User { get; set; }
    }
    public class RecruitmentStatusDetails
    {
        public string Code { get; set; }
        public string EntityType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public DateTime? DateMode { get; set; }
        public string User { get; set; }
    }
    public class EmergancyContact
    {
       
    }
    public class AgentNominee
    {
        public int NomineeID { get; set; }
        public string NomineeName { get; set; }
        public DateTime NomineeDOB { get; set; }
        public string NomineeRelation { get; set; }        
        public string NomineeNICNo { get; set; }
        public decimal? NomineePercentage { get; set; }
    }
    public class AgentBankInformation
    {
        public int BankID { get; set; }
        public int PaymentID { get; set; }
        public string PaymentFor { get; set; }
        public string PaymentNominee { get; set; }
        public string BankName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string AccountNo { get; set; }
        public string PaymentMode { get; set; }
        //public string BankName { get; set; }
        public string BankBranchCode { get; set; }
        public string BankBranchName { get; set; }
    }
    public class RecruitmentExam
    {
        public int ExamID { get; set; }
        public string ExamMode { get; set; }
        public string Institue { get; set; }
        public string CenterCode { get; set; }
        public string Place { get; set; }
        public string Language { get; set; }
        public string RollNo { get; set; }
        public DateTime? ExamDate { get; set; }
        public string ExamStatus { get; set; }
        public string ExamGridDate { get; set; }
    }
    public class InterviewResultView
    {
        public int ResultID { get; set; }
        public string InterviewResultLevels { get; set; }
        public string InterviewName { get; set; }
        public string InterviewResult { get; set; }
    }
    public class NICVerificationTaskDetails
    {
        public bool IsProspectCode { get; set; }
        public int ProspectID { get; set; }
        public string ProspectCode { get; set; }
        public string ProspectNICNo { get; set; }
        public string ProspectFirstName { get; set; }
        public string ProspectLastName { get; set; }
        public string ProspectGender { get; set; }
        public string ProspectEmailID { get; set; }
        public DateTime? ProspectReceivedDate { get; set; }
        public string ProspectReceivedFrom { get; set; }               
        public int SRNo { get; set; }
    }
}
