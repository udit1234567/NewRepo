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
    
    public partial class tblNeedFinancialNeed
    {
        public int FinancialNeedID { get; set; }
        public Nullable<int> NeedAnalysisID { get; set; }
        public Nullable<decimal> CurrentReq { get; set; }
        public Nullable<decimal> EstimatedAmount { get; set; }
        public Nullable<decimal> FundBalance { get; set; }
        public Nullable<decimal> Gap { get; set; }
        public string Relationship { get; set; }
        public string Name { get; set; }
    
        public virtual tblLifeNeedAnalysi tblLifeNeedAnalysi { get; set; }
    }
}