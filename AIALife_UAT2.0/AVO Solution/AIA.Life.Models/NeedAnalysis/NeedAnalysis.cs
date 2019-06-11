using AIA.Life.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AIA.Life.Models.NeedAnalysis
{
    public class NeedAnalysis
    {
        public NeedAnalysis()
        {
            objSpouseDetails = new SpouseDetails();
            objDependents = new List<Dependents>();
            objFamilyIncome = new FamilyIncome();
            objCalculator = new Models.NeedAnalysis.Calculator();
            dlladversities = new List<MasterListItem>();
            dllcoverage = new List<MasterListItem>();
            dllannualamount = new List<MasterListItem>();

            CurrencyList = new List<MasterListItem>();

            dlladequacy = new List<MasterListItem>();
            objGCEAL = new List<Models.NeedAnalysis.GCEAL>();
            objLocal = new List<Models.NeedAnalysis.LocalStudies>();
            objHigherEdu = new List<Models.NeedAnalysis.HigherEduDegree>();
            objHigherForeign = new List<Models.NeedAnalysis.HigherForeignDegree>();
            objWedding = new List<Models.NeedAnalysis.Wedding>();
            objHouse = new List<Models.NeedAnalysis.House>();
            objCar = new List<Models.NeedAnalysis.Car>();
            objForeignTour = new List<Models.NeedAnalysis.ForeignTour>();
            objOthers = new List<Models.NeedAnalysis.Others>();
            objFinancialNeeds = new List<Models.NeedAnalysis.FinancialNeeds>();
            objDependants = new List<Models.NeedAnalysis.Dependants>();
            objFinancialNeed = new Models.NeedAnalysis.Needs();
            objAssetsAndLiabilities = new AssetsAndLiabilities();
            objNeeds = new List<Needs>();
            dllChildRelatioship = new List<Common.MasterListItem>();
            dllChildName = new List<Common.MasterListItem>();
            objPrevPolicy = new List<Models.NeedAnalysis.PrevPolicy>();
        }

        public string Stage { get; set; }  
        public List<MasterListItem> dllChildName { get; set; }
        public int? DependantCount { get; set; }  
        public List<GCEAL> objGCEAL { get; set; }
        public List<LocalStudies> objLocal { get; set; }
        public List<HigherEduDegree> objHigherEdu { get; set; }
        public List<HigherForeignDegree> objHigherForeign { get; set; }
        public List<Wedding> objWedding { get; set; }
        public List<House> objHouse { get; set; }
        public List<Car> objCar { get; set; }
        public List<ForeignTour> objForeignTour { get; set; }
        public List<Others> objOthers { get; set; }
        public List<FinancialNeeds> objFinancialNeeds { get; set; }
        public List<Dependants> objDependants { get; set; }
        public FutureFinancial objFutureFinancial { get; set; }  
        public bool chkRetirement1 { get; set; }
        public bool chkRetirement2 { get; set; }
        public bool chkRetirement3 { get; set; }
        public bool chkRetirement4 { get; set; }
        public bool chkRetirement5 { get; set; }
        public bool chkHealth1 { get; set; }
        public bool chkHealth2 { get; set; }
        public bool chkHealth3 { get; set; }
        public bool chkHealth4 { get; set; }
        public bool chkHealth5 { get; set; }
        public bool chkProtection1 { get; set; }
        public bool chkProtection2 { get; set; }
        public bool chkProtection3 { get; set; }
        public bool chkProtection4 { get; set; }
        public bool chkProtection5 { get; set; }
        public bool chkSaving1 { get; set; }
        public bool chkSaving2 { get; set; }
        public bool chkSaving3 { get; set; }
        public bool chkSaving4 { get; set; }
        public bool chkSaving5 { get; set; }
        public bool chkconfirm { get; set; }
        public bool chkprodconfirm { get; set; }
        public bool chkEducation1 { get; set; }
        public bool chkEducation2 { get; set; }
        public bool chkEducation3 { get; set; }
        public bool chkEducation4 { get; set; }
        public bool chkEducation5 { get; set; }
        

        public long? Financial0 { get; set; }  
        public long? Financial1 { get; set; }
        public long? Financial2 { get; set; }
        public long? Financial3 { get; set; }
        public long? Financial4 { get; set; }
        public long? Assets0 { get; set; }
        public long? Assets1 { get; set; }
        public long? Assets2 { get; set; }
        public long? Assets3 { get; set; }
        public long? Assets4 { get; set; }
        public long? Assets5 { get; set; }
        public long? Liabilityone0 { get; set; }
        public long? Liabilityone1 { get; set; }
        public long? Liabilityone2 { get; set; }
        public long? Liabilityone3 { get; set; }
       
        public long? Liabilitytwo0 { get; set; }
        public long? Liabilitytwo1 { get; set; }
        public long? Liabilitytwo2 { get; set; }
        public long? Liabilitytwo3 { get; set; }
        public long? Liabilitytwo4 { get; set; }
        public long? Liabilitytwo5 { get; set; }
        public long? Income0 { get; set; }
        public long? Income1 { get; set; }
        public long? Income2 { get; set; }
        public long? Income3 { get; set; }
        public long? Expense0 { get; set; }
        public long? Expense1 { get; set; }
        public long? Expense2 { get; set; }
        public long? Expense3 { get; set; }
        public long? Expense4 { get; set; }
        public long? Expense5 { get; set; }
        public long? SavingTarget { get; set; } 
        public int? ProIntrestRate { get; set; }
        public List<MasterListItem> dlladversities { get; set; }
        public List<MasterListItem> dllcoverage { get; set; } 
        public List<MasterListItem> dllannualamount { get; set; }
        public List<MasterListItem> dllChildRelatioship { get; set; }
        public List<MasterListItem> dlladequacy { get; set; }
        public List<MasterListItem> dllRelationship { get; set; }
        public List<MasterListItem> CurrencyList { get; set; }
        public List<SaveRow> objSaveRow { get; set; }
        public string[] objadversities { get; set; }
        public string adversities { get; set; }
        public string objannualamount { get; set; }
        public string objcoverage { get; set; }
        public string objadequacy { get; set; }
        
        public long? EduGapTotal { get; set; }
        public long? SavingGapTotal { get; set; }
        public long? SavingReqTotal { get; set; }
        public long? SavingEstTotal { get; set; }
        public long? SavingCurrentTotal { get; set; }
        public long? EduLumpSum { get; set; }
        public long? EduMaturity { get; set; }
        public long? MonthlyEduExpense { get; set; }
        public long? AnnualEduExpense { get; set; }
        public long? AnnualSaveExpense { get; set; }
        public long? MonthlySaveExpense { get; set; }
        public int? EduInflationRate { get; set; }
        public int? SavInflationRate { get; set; }
        public long? MonthlyEarning { get; set; }
        public int? YearsofEarning { get; set; }
        public long? EstimatedIncome { get; set; }
        public long? FutureFund { get; set; }
        public long? AvailableFund { get; set; }
        public long? EmergencyFund { get; set; }
        public long? CriticalRequiremenent { get; set; }
        public long? CriticalFund { get; set; }
        public long? CriticalGap { get; set; }
        public long? HospitalizationRequiremenent { get; set; }
        public long? HospitalizationFund { get; set; }
        public long? HospitalizationGap { get; set; }
        public long? additionalexpenseRequiremenent { get; set; }
        public long? additionalexpenseFund { get; set; }
        public long? additionalexpenseGap { get; set; }
        public int NeedAnalysisID { get; set; }
        public DateTime? ProspectDOB { get; set; }
        public SpouseDetails objSpouseDetails { get; set; }
        public Calculator objCalculator { get; set; }
        public List<Dependents> objDependents { get; set; }
        public EstimateDetails objEstimateDetails { get; set; }
        public AssetsAndLiabilities objAssetsAndLiabilities { get; set; }
        public FamilyIncome objFamilyIncome { get; set; }
        public List<Needs> objNeeds { get; set; }
        public Needs objFinancialNeed { get; set; }
        public string Total { get; set; }
        public string ProductSelected { get; set; }
        public string Comments { get; set; }
        public string NeedAnalysisFileAttachment { get; set; }
        public DateTime? DateOfNextMeeting { get; set; }
        public string TimeOfNextMeeting { get; set; }
        public string PurposeOfMeeting { get; set; }
        //public string PurposeOfMeeting1 { get; set; }
        public string UploadSignPath { get; set; }
        public string NotePadPath { get; set; }
        public string UploadDocPath { get; set; }
        public byte[] ProspectSign { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string SelectedProducts { get; set; }
        public string HospitalBills { get; set; }
        public string ByteArraygraph { get; set; }
        public string HospitalRtrExp { get; set; } 
        public long? WealthRequirement { get; set; }
        public int? WealthRequirement2017 { get; set; }
        public long? LivingExpense { get; set; }
        public int? LivingExpense2017 { get; set; }
        public long? FinancialExpense { get; set; }
        public long? FinancialExpense2017 { get; set; }
        public long? TotalExpense { get; set; }
        public long? TotalExpense2017 { get; set; }
        public List<PrevPolicy> objPrevPolicy { get; set; }
        public long? EmergencyFund1 { get; set; }
        public long? EmergencyFund2 { get; set; }
        public long? EmergencyFund3 { get; set; }
        public string Policy1 { get; set; }
        public string Policy2 { get; set; }
        public string Policy3 { get; set; }
        public long? MaturityFund1 { get; set; }
        public long? MaturityFund2 { get; set; }
        public long? MaturityFund3 { get; set; }
        public long? TotalEmergencyFund { get; set; }
        public long? TotalMaturityFund { get; set; }
        public long? EmergencyFundGap { get; set; }
        public long? MaturityFundGap { get; set; }
        public int? FNAFromYear { get; set; }
        public int? FNAToYear { get; set; }
        public int? FNAInflationRate { get; set; }
        public int? FNAPlanNoYear { get; set; } 
        public int? FNAIntrestRate { get; set; }
        public int? CalculatorFromYear { get; set; }
        public int? CalculatorToYear { get; set; }
        public int? CalculatorInflationRate { get; set; }
        public int? CalculatorPlanNoYears { get; set; }
        public int? CalculatorIntrestRate { get; set; }
        public long? CriticalIllnessRequirement { get; set; }
        public long? HospitalRequirement { get; set; }
        public long? AdditionalRequirement { get; set; }
        public long? CriticalIllnessAvailable { get; set; }
        public long? HospitalAvailable { get; set; }
        public long? AdditionalAvailable { get; set; }
        public long? CriticalIllnessGap { get; set; }
        public long? HospitalGap { get; set; }
        public long? AdditionalGap { get; set; }
        public long? TotalRequirement { get; set; }
        public long? TotalAvailable { get; set; }
        public long? TotalGap { get; set; }
        public string TypeofHospitalization { get; set; }
    }
    public class FutureFinancial
    {
        public string Image { get; set; }
        public string Name { get; set; } 

    }
    public class SaveRow
    {
        public int DaughterWedding { get; set; }
    }
    public class Calculator 
    {
        public long? FoodExpense { get; set; } 
        public long? WaterExpense { get; set; } 
        public long? RentExpense { get; set; } 
        public long? LeaseExpense { get; set; }
        public long? TransportExpense { get; set; }
        public long? MedicineExpense { get; set; } 
        public long? EducationExpense { get; set; }
        public long? ClothesExpense { get; set; }
        public long? EntertainmentExpense { get; set; }
        public long? CharityExpense { get; set; }
        public long? OtherExpense { get; set; }
        public long? EstimatedFoodExpense { get; set; } 
        public long? EstimatedWaterExpense { get; set; }
        public long? EstimatedRentExpense { get; set; }
        public long? EstimatedLeaseExpense { get; set; }
        public long? EstimatedTransportExpense { get; set; }
        public long? EstimatedMedicineExpense { get; set; }
        public long? EstimatedEducationExpense { get; set; }
        public long? EstimatedClothesExpense { get; set; }
        public long? EstimatedEntertainmentExpense { get; set; }
        public long? EstimatedCharityExpense { get; set; }
        public long? EstimatedOtherExpense { get; set; }
        public long? CurrentEPFBalance { get; set; }
        public long? EstimatedEPFBalance { get; set; }
        public long? CurrentETFBalance { get; set; }
        public long? EstimatedETFBalance { get; set; }
        public long? MonthlyAllocation20 { get; set; }
        public long? MonthlyAllocation3 { get; set; }
        public long? Salary { get; set; }
       
        public long? CurrentGratuityFund { get; set; }
        public long? EstimatedGratuityFund { get; set; }
        public long? TotalMonthlyExpense { get; set; }
        public long? EstimatedTotalMonthlyExpense { get; set; }
        public long? TotalRetirementFund { get; set; }
        public long? FundBalanceTotal { get; set; }
        public long? PerAnnumIncome { get; set; }
        public long? Exsitingotherincome { get; set; }
        public long? EstimatedAnnuallivingExpenses { get; set; }
        public long? AnnualIncomeSurplus { get; set; }
        public long? MonthlyPensionGap { get; set; }  
    }


    public class SpouseDetails
    {
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int AgeNextBirthday { get; set; }
        public int CurrentAge{ get; set; }
        public string MaritialStatus { get; set; }
        public string OccuaptionID { get; set; }
        public string Employer { get; set; }
        public int ContactID { get; set; }
    }

    public class Dependents
    {
        public string DependentName { get; set; }
        public string Relationship { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int Age { get; set; }
        public int ContactID { get; set; }
        public bool IsDeleted { get; set; }
    }


    public class EstimateDetails
    {
        public int? AnnualLivingExpense { get; set; }
        public int? AnnualVacation { get; set; }
        public int? InstalmentsApartmentsVehicles { get; set; }
        public int? LoanPayment { get; set; }
        public int? OtherPayment { get; set; }
        public int? TotalAnnualExpense { get; set; }

        public int? Food { get; set; }
        public decimal? HouseElectricityWaterRent { get; set; }
        public decimal? Clothes { get; set; }

        public decimal? Transport { get; set; }
        public decimal? FamilyEducation { get; set; }
        public decimal? HealthCare { get; set; }

        public decimal? SpecialEvents { get; set; }
        public decimal? MaidAndOtherHelpers { get; set; }
        public decimal? OtherMontly { get; set; }
        public decimal? TotalMonthlyExp { get; set; }
        public decimal? MonthlyInstallments { get; set; }

    }
    public class PrevPolicy
    {
        public long? MaturityFund { get; set; }
        public string PolicyNo { get; set; }
    }
    public class AssetsAndLiabilities
    {
        public long? SurPlusAssets { get; set; }
        public long? NetAssests { get; set; }
        public decimal? LandOrHouse { get; set; }
        public decimal? MotorVehicle { get; set; }
        public decimal? BankDeposits { get; set; }
        public decimal? Investments { get; set; }
        public long? AssetsTotal { get; set; }
        public string MotorVehicleType { get; set; }
        public decimal? FixedDeposit { get; set; }
        public decimal? Shares { get; set; }
        public decimal? Jewellery { get; set; }
        public decimal? Loans { get; set; }
        public decimal? Mortgauges { get; set; }
        public decimal? leases { get; set; }
        public decimal? Insuredleases { get; set; }
        public decimal? others { get; set; }
        public decimal? Liab_Total { get; set; }
        public decimal? LiabilityOthers { get; set; }
        public int? InsuredLiabilityOthers { get; set; }
        public long? LumpsumRequirement { get; set; }
        public int MyProperty { get; set; }

        public long? LiabilityTotal { get; set; }
        public long? InsuredLiabilityTotal { get; set; }
        public int? Bank { get; set; }
        public int? InsuredBank { get; set; }
        public int? Borrowing { get; set; }
        public int? InsuredBorrowing { get; set; }


    }


    public class FamilyIncome
    {
        public int? AnnualSalary { get; set; }
        public int? AnnualInterest { get; set; }
        public long? IncomeLumpsumRequirement { get; set; } 
        public decimal? ProspectMonthlyIncome { get; set; }
        public decimal? Rent { get; set; }
        public decimal? OtherIncome { get; set; }
        public long? TotalIncome { get; set; }
        public long? TotalExpense { get; set; } 
        public decimal? SpouseMonthlyIncome { get; set; }
        public decimal? HouseHoldTotal { get; set; }
        public decimal? CapitalReq { get; set; }
        public decimal? PersonalLifeInsurance { get; set; }
        public decimal? SavingsAndInvestments { get; set; }
        public decimal? TotalProtection { get; set; }
        public decimal? GapIdentified { get; set; }
        public decimal TotalDeath { get; set; }
        public decimal TotalAccidental { get; set; }
        public decimal TotalCritical { get; set; }
        public decimal TotalHospitalization { get; set; }
        public decimal RateOfInterest { get; set; }
        public List<PrevInsuranceDetails> objLstPrevInsuranceDetails { get; set; }
        public bool? IsOtherInsurance { get; set; }
        public int? NoOfJanashaktiPolicy { get; set; }
        public int? NoOfOtherPolicies { get; set; }

    }

    public class PrevInsuranceDetails
    {
        public string Company { get; set; }
        public string PolicyOrProposalNo { get; set; }
        public string Death { get; set; }
        public string Accidental { get; set; }
        public string Critical { get; set; }
        public string Hospitalization { get; set; }
        public string CurrentStatus { get; set; }
        public bool IsDeleted { get; set; }
    }


    public class Needs
    {
        public int? EduRequirement { get; set; }
        public int? EduEstimate { get; set; }
        public int? EduFundBalance { get; set; }
        public int? EduGap { get; set; }
        public int? WeddingRequirement { get; set; }
        public int? WeddingEstimate { get; set; }
        public int? WeddingFundBalance { get; set; }
        public int? WeddingGap { get; set; }
        public int? PensionRequirement { get; set; }
        public int? PensionEstimate { get; set; }
        public int? PensionFundBalance { get; set; }
        public int? PensionGap { get; set; }
        public int? PropertyRequirement { get; set; }
        public int? PropertyEstimate { get; set; }
        public int? PropertyFundBalance { get; set; }
        public int? PropertyGap { get; set; }
        public int? OtherRequirement { get; set; }
        public int? OtherEstimate { get; set; }
        public int? OtherFundBalance { get; set; }
        public int? OtherGap { get; set; }
        public long? RequirementTotal { get; set; }
        public long? EstimateTotal { get; set; }
        public long? FundBalanceTotal { get; set; }
        public long? GapTotal { get; set; }
        
        public int PKNeedID { get; set; }
        public string NeedName { get; set; }
        public int NeedID { get; set; } // changed string to int
        public bool IsNeedOpted { get; set; }
        public string Value { get; set; }
        public string Priority { get; set; }
        public string PlanSuggested { get; set; }
        public string ImagePath { get; set; }
    }
    public class GCEAL
    {
        public long? CurrRequirement { get; set; }
        public int? Term { get; set; }
        public int? MaturityAge { get; set; }
        public long? EstAmount { get; set; }
        public long? AvailableFund { get; set; }
        public long? Gap { get; set; }
        public string Relationship { get; set; }
        public int? Age { get; set; }  
    }
    public class LocalStudies
    {
        public long? CurrRequirement { get; set; }
        public int? Term { get; set; }
        public int? MaturityAge { get; set; }
        public long? EstAmount { get; set; }
        public long? AvailableFund { get; set; }
        public long? Gap { get; set; }
        public string Relationship { get; set; }
        public int? Age { get; set; }
    }
    public class HigherEduDegree
    {
        public long? CurrRequirement { get; set; }
        public int? MaturityAge { get; set; }
        public int? Term { get; set; }
        public long? EstAmount { get; set; }
        public long? AvailableFund { get; set; }
        public long? Gap { get; set; }
        public string Relationship { get; set; }
        public int? Age { get; set; }
    }
    public class HigherForeignDegree
    {
        public long? CurrRequirement { get; set; }
        public int? MaturityAge { get; set; }
        public int? Term { get; set; }
        public long? EstAmount { get; set; }
        public long? AvailableFund { get; set; }
        public long? Gap { get; set; }
        public string Relationship { get; set; }
        public int? Age { get; set; }
    }
    public class Wedding
    {
        public long? CurrRequirement { get; set; }
        public int? Term { get; set; }
        public long? EstAmount { get; set; }
        public int? MaturityAge { get; set; }
        public long? AvailableFund { get; set; }
        public long? Gap { get; set; }
        public string Relationship { get; set; }
        public int? Age { get; set; }
    }
    public class House
    {
        public long? CurrRequirement { get; set; }
        public int? Term { get; set; }
        public long? EstAmount { get; set; }
        public int? MaturityAge { get; set; }
        public long? AvailableFund { get; set; }
        public long? Gap { get; set; }
        public string Relationship { get; set; }
        public int? Age { get; set; }
    }
    public class Car
    {
        public long? CurrRequirement { get; set; }
        public int? Term { get; set; }
        public long? EstAmount { get; set; }
        public int? MaturityAge { get; set; }
        public long? AvailableFund { get; set; }
        public long? Gap { get; set; }
        public string Relationship { get; set; }
        public int? Age { get; set; }
    }
    public class ForeignTour
    {
        public long? CurrRequirement { get; set; }
        public int? Term { get; set; }
        public long? EstAmount { get; set; }
        public int? MaturityAge { get; set; }
        public long? AvailableFund { get; set; }
        public long? Gap { get; set; }
        public string Relationship { get; set; }
        public int? Age { get; set; }
    }
    public class Others
    {
        public long? CurrRequirement { get; set; }
        public int? Term { get; set; }
        public long? EstAmount { get; set; }
        public int? MaturityAge { get; set; }
        public long? AvailableFund { get; set; }
        public long? Gap { get; set; }
        public string Relationship { get; set; }
        public int? Age { get; set; }
    }
    public class FinancialNeeds
    {
        public long? CurrReq { get; set; }
        public long? EstAmount { get; set; }
        public long? FundBalance { get; set; }
        public string Relationship { get; set; }
        public string ImagePath { get; set; }
        public string Name { get; set; }  
        public long? Gap { get; set; }  

    }
    public class Dependants
    {
        public string Name { get; set; }
        public DateTime? DOB { get; set; }
        public int? AgeNextBirthday { get; set; }
        public string Relationship { get; set; }  
    }
}
 