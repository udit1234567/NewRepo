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
    
    public partial class tblProposalPremium
    {
        public decimal PremiumID { get; set; }
        public Nullable<decimal> PolicyID { get; set; }
        public Nullable<decimal> BasicPremium { get; set; }
        public Nullable<decimal> AnnualPremium { get; set; }
        public Nullable<decimal> Cess { get; set; }
        public Nullable<decimal> BasicSumInsured { get; set; }
        public Nullable<decimal> VAT { get; set; }
        public Nullable<decimal> PolicyFee { get; set; }
        public Nullable<decimal> HalfYearlyPremium { get; set; }
        public Nullable<decimal> QuaterlyPremium { get; set; }
        public Nullable<decimal> MonthlyPremium { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<decimal> AdditionalPremium { get; set; }
        public Nullable<bool> isDeleted { get; set; }
    
        public virtual tblPolicy tblPolicy { get; set; }
    }
}