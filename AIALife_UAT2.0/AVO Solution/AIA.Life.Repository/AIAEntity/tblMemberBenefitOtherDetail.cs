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
    
    public partial class tblMemberBenefitOtherDetail
    {
        public decimal MemberBenifitDetailsID { get; set; }
        public decimal MemberBenifitID { get; set; }
        public string LoadingType { get; set; }
        public string LoadingBasis { get; set; }
        public string LoadingAmount { get; set; }
        public string Exclusion { get; set; }
        public string ExtraPremium { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string TotalPremium { get; set; }
    
        public virtual tblPolicyMemberBenefitDetail tblPolicyMemberBenefitDetail { get; set; }
        public virtual tblPolicyMemberBenefitDetail tblPolicyMemberBenefitDetail1 { get; set; }
    }
}