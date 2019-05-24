using AIA.Life.Models.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace AIA.Life.Models.Common
{
    public class MemberDetails
    {
        
        public MemberDetails()
        {
            objBenifitDetails = new List<BenifitDetails>();
            
            LstQuestions = new List<QuestionsList>();
            objLststateofhelath = new List<QuestionsList>();
            objLstMedicalHistory = new List<QuestionsList>();
            objAdditionalQuestions = new List<QuestionsList>();
            objLstFamily = new List<QuestionsList>();
            objLstOtherInsuranceDetails = new List<QuestionsList>();
            Questions = new List<QuestionsList>();        
            ObjUwDecision = new UWDecision.UWDecision();
            LstMedicalQuestionnariesDetails = new List<MedicalQuestionnariesDetails>();
            LstMedicalDoctorsQuestionnariesDetails = new List<DoctorsMedicalQuestionnariesDetails>();
            LstMedicalTestQuestionnariesDetails = new List<TestMedicalQuestionnariesDetails>();
            LstMedicalCurrentQuestionnariesDetails = new List<CurrentMedicalQuestionnariesDetails>();
            LstConcurrentlyProposedInsurancePAQ1Details = new List<ConcurrentlyProposedInsurancePAQ1Details>();
            LstExistingPolicieswithAIAlnsurancePAQ2Details = new List<ExistingPolicieswithAIAlnsurancePAQ2Details>();
            LstTotalAnnualIncomePAQ3Details = new List<TotalAnnualIncomePAQ3Details>();
            LstAssetsandLiabilitiesPAQ4Details = new List<AssetsandLiabilitiesPAQ4Details>();
            objLstFamilyBackground = new List<LifeAssuredFamilyBackground>();
            objCommunicationAddress = new Address();
            objPermenantAddress = new Address();
            objLifeStyleQuetions = new LifeStyleQA();
            ClientRelations = new List<ClientRelation>();
            ObjTotalAnnualIncomePAQ3Details = new TotalAnnualIncomePAQ3Details();
            ObjAssetsandLiabilitiesPAQ4Details = new AssetsandLiabilitiesPAQ4Details();

            Error = new Error();
            Language = "E";
        }
      


        public Error Error { get; set; }
        public QuestionsList objQuestionsList { get; set; }
        public QuestionsList objQuestions { get; set; }
        public List<QuestionsList> LstQuestions { get; set; }
        public List<QuestionsList> LstEasyPensionQuestions { get; set; }
        public List<QuestionsList> objLststateofhelath { get; set; }
        public List<QuestionsList> objLstMedicalHistory { get; set; }
        public List<QuestionsList> objAdditionalQuestions { get; set; }
        public List<QuestionsList> objLstFamily { get; set; }
        public List<QuestionsList> objLstOtherInsuranceDetails { get; set; }
        public List<QuestionsList> Questions { get; set; }
        public List<QuestionsList> objLstWealthPlannerQuestions { get; set; }

        //Added QuestionnairesGridView List
        public List<MedicalQuestionnariesDetails> LstMedicalQuestionnariesDetails { get; set; }

        public List<DoctorsMedicalQuestionnariesDetails> LstMedicalDoctorsQuestionnariesDetails { get; set; }

        public List<TestMedicalQuestionnariesDetails> LstMedicalTestQuestionnariesDetails { get; set; }

        public List<CurrentMedicalQuestionnariesDetails> LstMedicalCurrentQuestionnariesDetails { get; set; }

        public List<ConcurrentlyProposedInsurancePAQ1Details> LstConcurrentlyProposedInsurancePAQ1Details { get; set; }

        public List<ExistingPolicieswithAIAlnsurancePAQ2Details> LstExistingPolicieswithAIAlnsurancePAQ2Details { get; set; }

        public List<TotalAnnualIncomePAQ3Details> LstTotalAnnualIncomePAQ3Details { get; set; }


        public List<AssetsandLiabilitiesPAQ4Details> LstAssetsandLiabilitiesPAQ4Details { get; set; }

        public AssetsandLiabilitiesPAQ4Details ObjAssetsandLiabilitiesPAQ4Details { get; set; }

        public TotalAnnualIncomePAQ3Details ObjTotalAnnualIncomePAQ3Details { get; set; }
        //Tiil here

        public bool IsLifeAssuredSeleted { get; set; }
        public bool IsOCRSeleted { get; set; }
        public bool IsOCRImageRecognition { get; set; }
        
        //Institution Details
        public string ContactPerson { get; set; }
        public string Designation { get; set; }
        public string BusinessRegistrationNo { get; set; }
        //Till Here


        //Added ClaimInfo GridView List
        public List<ClaimInformation> objClaimInfo { get; set; }
        public bool AreyouClaimedAnyPolicies { get; set; }
        //Tiil here
        //Added Proposal Details
        public bool CitizenShip { get; set; }
        public string Citizenship1 { get; set; }
        public string Citizenship2 { get; set; }
        public string SpecifyNationality { get; set; }
        public string SpecifyUSNationality { get; set; }
        public string ResidentialStatus { get; set; }
        public string Residential { get; set; }
        public bool? OccupationHazardousWork { get; set; }
        public string SpecifiyOccupationHazardousWork { get; set; }
        

        public string PassportNumber { get; set; }
        public string DrivingLicense { get; set; }
        public string USTaxpayerId { get; set; }
        public string CountryofOccupation { get; set; }
        public string LifeAssured { get; set; }

       
        //Till here

        public int Index { get; set; }
        public decimal MemberID { get; set; }
        public int QuoteMemberID { get; set; }
        public string RelationShipWithPropspect { get; set; }
        public string RelationShipWithPropspectID { get; set; }
        public string RelationShipWithPropspectText { get; set; }
        public string Salutation { get; set; }
        public string FullName { get; set; }
        public List<string> DiseasesSelected { get; set; }
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
        public DateTime? WeddingAnniversaryDate { get; set; }
        public string OtherMobileNo { get; set; }
        public string ExtraPremium { get; set; }

        private string _nameWithInitial;
        public string NameWithInitial
        {
            get { return _nameWithInitial; }
            set
            {
                _nameWithInitial = string.IsNullOrEmpty(value) != true ? value.Trim() : value;
            }
        }
        public string PrefferedName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        public string GenderText { get; set; }
        public string MaritialStatus { get; set; }
        public string OLDNICNo { get; set; }
        public string NewNICNO { get; set; }
        public int? OccupationID { get; set; }
        public string OccupationCode { get; set; }
        public string SalutationCode { get; set; }
        public string CompanyName { get; set; }
        public string CorporateName { get; set; }
        public string ProposerEmailID { get; set; }
        public string ProposerMobileNo { get; set; }
        public string NameOfDuties { get; set; }
        public string MonthlyIncome { get; set; }
        public string Nationality { get; set; }
        public string HomeNumber { get; set; }
        public string WorkNumber { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string BMIValue { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public int? INTBasicSumInsured { get; set; }
        public string BasicSumInsured { get; set; }
        public string Basicpremium { get; set; }
        public string Memberpremium { get; set; }
        public decimal _AnnualPremium { get; set; }// Previous Policy Annual Premium
        public decimal _CurrentproposalAnnualPremium { get; set; }
        public string OccuaptionClass { get; set; } // Occuaption class For UW Rules
        public string PreviousPolicyFlag { get; set; } //Prev Policy Flag
        public bool IsCommunicationAddressSameasProspect { get; set; }
        public Address objCommunicationAddress { get; set; }
        public Address objPermenantAddress { get; set; }
        public bool IsRegAsCommunication { get; set; }
        public List<BenifitDetails> objBenifitDetails { get; set; }
        public List<MedicalHistoryQuestions> objLstDiseaseHistory { get; set; }
        public MedicalHistoryQuestions objMedicalHistory { get; set; }
        public List<LifeAssuredFamilyBackground> objLstFamilyBackground { get; set; }   
        public UWDecision.UWDecision ObjUwDecision { get; set; }       
        public LifeStyleQA objLstLifeStyleQuestions { get; set; }
        public int NoofJsPolicies { get; set; }
        public bool AreyouCoveredUnderOtherPolicies { get; set; }
        public int NoofOtherPolicies { get; set; }
        public string TotalDeath { get; set; }
        public string TotalAccidental { get; set; }
        public string TotalCritical { get; set; }
        public string TotalHospitalization { get; set; }
        public string TotalHospitalizationReimbursement { get; set; }
        public List<LifeAssuredOtherInsurance> objLifeAssuredOtherInsurance { get; set; }
        

        //Added for Hidden Variables
        public bool IsSelfCovered { get; set; }
        public bool IsSpouseCoverd { get; set; }
        public bool IsSelfIsMainLife { get; set; }
        public int NoofChilds { get; set; }
        public string AssuredName { get; set; }
        public bool? IsproposerlifeAssured { get; set; }
        public bool IsSameasProposerAddress { get; set; }
        public LifeStyleQA objLifeStyleQuetions { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstAvgMonthlyIncome { get; set; }
        public List<MasterListItem> LstLoadingType { get; set; }
        public List<MasterListItem> LstLoadingBasis { get; set; }

        //[XmlIgnore]
        //public List<MasterListItem> lstPAQAssets { get; set; }
        //[XmlIgnore]
        //public List<MasterListItem> lstPAQLiabilities { get; set; }
        // Added for UW Model
        public bool IsUSCitizen { get; set; }
        public string ClientCode { get; set; }
        public string SAM { get; set; }
        

        public decimal SAR { get; set; }
        public decimal FAL { get; set; }
        public bool AFC { get; set; }

        //for IL Integration
        public string ProposalNo { get; set; }
        public int PolicyTerm { get; set; }
        public int PremiumTerm { get; set; }
        public int PensionTerm { get; set; }
        
        public bool AnyAdverseRemarks { get; set; }
        public string InceptionDate { get; set; }
        public bool Deductible { get; set; }
        public string MaturityBenefit { get; set; }
        public string LifeNum { get; set; }
        public List<ClientRelation> ClientRelations { get; set; }

        public decimal? ADDBSA { get; set; }
        public decimal? CIBSA { get; set; }
        public decimal? CIBHPSA { get; set; }
        public decimal? HIPSA { get; set; }
        public decimal? HIPHPSA { get; set; }
        public decimal? ASBSA { get; set; }
        public decimal? HECSA { get; set; }
        public decimal? HECHPSA { get; set; }
        public decimal? IPBSA { get; set; }

        public decimal? PrevOE { get; set; }
        public decimal? PrevHE { get; set; }
        public decimal? PrevRE { get; set; }

        public int MonthlySavingBenifit { get; set; }
        public string Language { get; set; }

        public int ClaimCount { get; set; }
        
        public bool IsUpdate { get; set; }
    }
    public class ClientRelation
    {
        public string Relation { get; set; }
        public string ClientCode { get; set; }
    }
    public class LifeStyleQA
    {

        public LifeStyleQA()
        {
            objSmokeDetails = new List<SmokeDetails>();
            objAlcoholDetails = new List<AlcoholDetails>();
            objNarcoticDrugDetails = new List<NarcoticDrugDetails>();
        }
        public int MemberLifeStyleID { get; set; }
        public int Height { get; set; }
        public int HeightFeets { get; set; }
        public int Weight { get; set; }
        public bool SteadyWeight { get; set; }
        public string HeightUnit { get; set; }
        public string WeightUnit { get; set; }

        public bool IsSmoker { get; set; }
        public string SmokeType { get; set; }
        public string SmokeQuantity { get; set; }
        public string SmokePerDay { get; set; }
        public string SmokeDuration { get; set; }

        public bool IsAlcholic { get; set; }
        public string AlcholType { get; set; }
        public string AlcholQuantity { get; set; }
        public string AlcholPerDay { get; set; }
        public string AlcholDuration { get; set; }

        public bool IsNarcoticDrugs { get; set; }

        public List<SmokeDetails> objSmokeDetails { get; set; }
        public List<AlcoholDetails> objAlcoholDetails { get; set; }
        public List<NarcoticDrugDetails> objNarcoticDrugDetails { get; set; }


    }
    public class SmokeDetails
    {
        public int AdditionalLifeStyleID { get; set; }
        public string SmokeType { get; set; }
        public string SmokeQuantity { get; set; }
        public string SmokePerDay { get; set; }
        public string SmokeDuration { get; set; }
        public bool Isdeleted { get; set; }
    }
    public class AlcoholDetails
    {
        public int AdditionalLifeStyleID { get; set; }
        public string AlcholType { get; set; }
        public string AlcholQuantity { get; set; }
        public string AlcholPerDay { get; set; }
        public string AlcholDuration { get; set; }
        public bool Isdeleted { get; set; }
    }

    public class NarcoticDrugDetails
    {
        public int AdditionalLifeStyleID { get; set; }
        public string NarcoticDrugType { get; set; }
        public string NarcoticDrugQuantity { get; set; }
        public string NarcoticDrugPerDay { get; set; }
        public string NarcoticDrugDuration { get; set; }
        public bool Isdeleted { get; set; }
    }

    public class LifeAssuredFamilyBackground
    {
        public LifeAssuredFamilyBackground(string _FamilyPerson)
        {
            FamilyPersonType = _FamilyPerson;
        }
        public LifeAssuredFamilyBackground()
        {

        }

        public string FamilyPersonType { get; set; }
        public int? FamilyBackgroundId { get; set; }
        public int? PresentAge { get; set; }
        public string StateOfHealth { get; set; }
        public int? AgeAtDeath { get; set; }
        public string Cause { get; set; }
        public bool IsSuffering { get; set; }
        public bool IsDeathBelowSixty { get; set; }
        public bool? AnyMemberOfFamily { get; set; }
        public bool? DeathBelow { get; set; }
        public string Details { get; set; }
        public bool Isdeleted { get; set; }

    }
    public class ClaimInformation
    {
        public decimal? OtherClaimId { get; set; }
        public string CompanyName { get; set; }
        public string NatureOfClaim { get; set; }
        public string PolicyNo { get; set; }
        public DateTime? DateOfClaim { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class QuestionsList
    {
        public int QuestionIndex { get; set; }
        public int QuestionID { get; set; }
        public string QuestionType { get; set; }
        public string ControlType { get; set; }
        public string Gender { get; set; }
        public int MemberQuestionID { get; set; }
        public string QuestionText { get; set; }
        public string Answer { get; set; }
        public string Details { get; set; }
        public string SubType { get; set; }
        public string SubControlType { get; set; }
        public string SubQuestion { get; set; }
        public string SubAnswer { get; set; }
        public string Value { get; set; }
        public string Master { get; set; }
        //public int? ParentID { get; set; }
        public int? SequenceNo { get; set; }
        public string[] Diseases { get; set; }
        public string SelectedDiseases { get; set; }
        public List<MasterListItem> LstDropDownvalues { get; set; }

        public List<QuestionsList> LstQuestionsTypes { get; set; }

       // public List<MedicalQuestionnariesDetails> LstMedicalQuestionnariesDetails { get; set; }
        

    }

    public class MedicalQuestionnariesDetails
    {
        public int Index { get; set; }
        public decimal QuestionsId { get; set; }
        public decimal otherQuestionsId { get; set; }
        public string varcharFiled9 { get; set; }
        public string varcharFiled10 { get; set; }
        public string varcharFiled11 { get; set; }
        public DateTime? DateFiled3 { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class DoctorsMedicalQuestionnariesDetails
    {
        public int Index { get; set; }
        public decimal QuestionsId { get; set; }
        public decimal otherQuestionsId { get; set; }
        public string varcharFiled1 { get; set; }
        public string varcharFiled2 { get; set; }
        public DateTime? DateFiled1 { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class ConcurrentlyProposedInsurancePAQ1Details
    {
        public int Index { get; set; }
        public decimal QuestionsId { get; set; }
        public string PAQvarcharFiled1 { get; set; }
        public string PAQvarcharFiled2 { get; set; }
        public string PAQvarcharFiled3 { get; set; }
        public string PAQvarcharFiled4 { get; set; }

        public bool IsDeleted { get; set; }
    }
    public class ExistingPolicieswithAIAlnsurancePAQ2Details
    {
        public int Index { get; set; }
        public decimal QuestionsId { get; set; }
        public string PAQvarcharFiled5 { get; set; }
        public string PAQvarcharFiled6 { get; set; }
        public string PAQvarcharFiled7 { get; set; }
        public string PAQvarcharFiled8 { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class TotalAnnualIncomePAQ3Details
    {
        public int Index { get; set; }
        public decimal QuestionsId { get; set; }
        public string PAQYearFiled1 { get; set; }
        public string PAQYearFiled2 { get; set; }
        public string PAQYearFiled3 { get; set; }
        //public string PAQAssetsTotal { get; set; }
        //public string PAQLiabilitiesTotal { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class AssetsandLiabilitiesPAQ4Details
    {
        public int Index { get; set; }
        public decimal QuestionsId { get; set; }
        public string PAQvarcharFiled9 { get; set; }
        public string PAQvarcharFiled10 { get; set; }
        public string PAQvarcharFiled11 { get; set; }
        public string PAQvarcharFiled12 { get; set; }

        public string PAQAssetsProperty { get; set; }
        public string PAQAssetsInvestment{ get; set; }
        public string PAQAssetsEquities { get; set; }
        public string PAQAssetsOther{ get; set; }

        public string PAQLiabilitiesMortgages { get; set; }
        public string PAQLiabilitiesLoans { get; set; }
        public string PAQLiabilitiesOthers { get; set; }
       

        public string PAQAssetsTotal { get; set; }
        public string PAQLiabilitiesTotal { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class TestMedicalQuestionnariesDetails
    {
        public int Index { get; set; }
        public decimal QuestionsId { get; set; }
        public decimal otherQuestionsId { get; set; }
        public string varcharFiled3 { get; set; }
        public string varcharFiled4 { get; set; }
        public string varcharFiled5 { get; set; }
        public DateTime? DateFiled2 { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class CurrentMedicalQuestionnariesDetails
    {
        public int Index { get; set; }
        public decimal QuestionsId { get; set; }
        public decimal otherQuestionsId { get; set; }
        public string varcharFiled6 { get; set; }
        public string varcharFiled7 { get; set; }
        public string varcharFiled8 { get; set; }
        public bool IsDeleted { get; set; }
    }
    

}

