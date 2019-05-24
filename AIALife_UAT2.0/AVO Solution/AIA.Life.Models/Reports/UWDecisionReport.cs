using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Reports
{
    public class UWDecisionReport
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public string PolicyNo { get; set; }
        public string Product { get; set; }
        public DateTime? Commencement { get; set; }
        public string ProposalNo { get; set; }
        public string QuoteNo { get; set; }


        public string Decision { get; set; }
        public string Remarks { get; set; }
        public DateTime? DateTime { get; set; }
        public List<AssuredMembers> objMemberDetails { get; set; }
        public List<string> ListAssured { get; set; }

        public List<RiderInfo> objLstRiderInfo { get; set; }
    }

    public class AssuredMembers
    {
        public string AssuredName { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Occupation { get; set; }
        public string BMI { get; set; }
        public string MedicalSAR { get; set; }
        public string FinancialSAR { get; set; }
        public string MedicalRequirements { get; set; }
        public string FinancialRequirements { get; set; }
        public string PreviousPolicies { get; set; }
        public string PreviousClaims { get; set; }
        
        public List<UWDeviationInfo> objUWDeviations { get; set; }

        public List<string> ListMedicalRequirements { get; set; }
        public List<string> ListFianacialRequirements { get; set; }
        public string Decision { get; set; }
        public string Remarks { get; set; }
    }

    public class RiderInfo
    {
        public string RiderName { get; set; }
        public string RiderCode { get; set; }
        public int RiderID { get; set; }
        public List<RiderRelation> objBenefitMemberRelationship { get; set; }
    }


    public class RiderRelation
    {
      
        public string Assured_Name { get; set; }
        public string Member_Relationship { get; set; }
        public string RiderCurrentSI { get; set; }
        public string RiderTotalSI { get; set; }
        public string Rider_Premium { get; set; }
    }


    public class UWDeviationInfo
    {

        public string DeviationMessage { get; set; }
        public string Decision { get; set; }
        public DateTime? Date { get; set; }
        public string User { get; set; }
        public string Remarks { get; set; }
    }

}
