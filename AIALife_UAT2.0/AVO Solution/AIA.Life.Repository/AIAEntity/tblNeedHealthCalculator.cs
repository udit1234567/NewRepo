//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AIA.Life.Repository.AIAEntity
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblNeedHealthCalculator
    {
        public int Id { get; set; }
        public Nullable<int> NeedAnalysisID { get; set; }
        public string HospitalBills { get; set; }
        public string HospRetireExp { get; set; }
        public string HealthAdversities { get; set; }
        public string AnnualAmountHealthExp { get; set; }
        public string CoverageHealthExp { get; set; }
        public string AdequacyHealthExp { get; set; }
        public Nullable<decimal> CriticalillnessReq { get; set; }
        public Nullable<decimal> CriticalIllenssFund { get; set; }
        public Nullable<decimal> CriticalIllnessGap { get; set; }
        public Nullable<decimal> HospReq { get; set; }
        public Nullable<decimal> HospFund { get; set; }
        public Nullable<decimal> HospGap { get; set; }
        public Nullable<decimal> AddLossReq { get; set; }
        public Nullable<decimal> AddLossFund { get; set; }
        public Nullable<decimal> AddLossGap { get; set; }
    
        public virtual tblLifeNeedAnalysi tblLifeNeedAnalysi { get; set; }
    }
}
