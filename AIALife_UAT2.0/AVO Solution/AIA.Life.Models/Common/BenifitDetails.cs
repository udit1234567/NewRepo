using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Common
{
    public class BenifitDetails
    {
        public BenifitDetails(string _AssuredMember)
        {
            AssuredMember = _AssuredMember;
        }
        public BenifitDetails()
        {
            objBenefitMemberRelationship = new List<AssuredRelation>();
            BenefitLoadings = new List<BenefitLoading>();
        }
        public decimal MemberBenifitID { get; set; }
        public int BenefitID { get; set; }
        public string BenifitName { get; set; }
        public string AssuredMember { get; set; }
        public bool BenifitOpted { get; set; }
        public bool Mandatory { get; set; }
        public string RiderSuminsured { get; set; }
        public List<AssuredRelation> objBenefitMemberRelationship { get; set; }
        public string RiderCode { get; set; }
        public string MinSumInsured { get; set; }
        public string MaxSumInsured { get; set; }
        public string RiderPremium { get; set; }
        public int RiderID { get; set; }
        public string RelationshipWithProspect { get; set; }
        public string CalType { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public decimal MemberBenefitDetailID { get; set; }
        public string LoadingType { get; set; }
        public string LoadingAmount { get; set; }
        public string DiscountAmount { get; set; }
        public string ExtraPremium { get; set; }
        public string LoadingBasis { get; set; }
        public string Exclusion { get; set; }
        public string ActualRiderPremium { get; set; }
        public string AnnualRiderPremium { get; set; }
        public string TotalPremium { get; set; }
        public bool IsDeleted { get; set; }
        public int RiderLoadingIndex { get; set; }
        public string MemberID { get; set; }
        public string LoadingPercentage { get; set; }
        public string LoadinPerMille { get; set; }
        public string AnnualModeLoadingAmount { get; set; }
        public string AnnualModeDiscountAmount { get; set; }
        public string AnnualModeAnnualpremium { get; set; }
        public List<BenefitLoading> BenefitLoadings { get; set; }
    }
    public class BenefitLoading
    {
        public string LoadingBasis { get; set; }
        public string LoadingType { get; set; }
        public string LoadingPercentage { get; set; }
    }
    public class AssuredRelation
    {
        public string RiderSI { get; set; }
        public string Member_Relationship { get; set; }
        public string Rider_Premium { get; set; }
        public string Assured_Name { get; set; }
        public string ActualRiderPremium { get; set; }
        public string LoadingAmount { get; set; }


    }

    // Added for Demo
    public class Illustation
    {
        public int PolicyYear { get; set; }
        public int BasicPremium { get; set; }
        public int MainBenefitsPremium { get; set; }
        public int AdditionalBenefitsPremiums { get; set; }
        public int TotalPremium { get; set; }
        public long FundBalanceDiv4 { get; set; }
        public long SurrenderValueDiv4 { get; set; }
        public long DrawDownDiv4 { get; set; }
        public long PensionBoosterDiv4 { get; set; }
        public long FundBalanceDiv8 { get; set; }
        public long SurrenderValueDiv8 { get; set; }
        public long DrawDownDiv8 { get; set; }
        public long PensionBoosterDiv8 { get; set; }
        public long FundBalanceDiv12 { get; set; }
        public long SurrenderValueDiv12 { get; set; }
        public long DrawDownDiv12 { get; set; }
        public long PensionBoosterDiv12 { get; set; }
        public long FundBalanceDiv5 { get; set; }
        public long FundBalanceDiv6 { get; set; }
         public long FundBalanceDiv7 { get; set; }
        public long FundBalanceDiv9 { get; set; }
        public long FundBalanceDiv10 { get; set; }
        public long FundBalanceDiv11 { get; set; }
        public long DrawDownDiv5 { get; set; }
        public long DrawDownDiv6 { get; set; }
        public long DrawDownDiv7 { get; set; }
        public long DrawDownDiv9 { get; set; }
        public long DrawDownDiv10 { get; set; }
        public long DrawDownDiv11 { get; set; }
        public long UnAllocatedPremium { get; set; }
        public long FundBoosterDiv12 { get; set; }
        

    }
    public class DrawDownDetails
    {
        public int DrawDownID { get; set; }
        public int LifeQQID { get; set; }
        public int PaymentFrequency { get; set; }
        public long DrawDownDiv4 { get; set; }
        public long DrawDownDiv8 { get; set; }
        public long DrawDownDiv12 { get; set; }
        
    }
}
