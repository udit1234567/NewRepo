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
    
    public partial class tblPolicyMemberClaimInfo
    {
        public decimal MemberClaimID { get; set; }
        public decimal MemberID { get; set; }
        public string CompanyName { get; set; }
        public string ProposalNo { get; set; }
        public string NatureOfClaim { get; set; }
        public Nullable<System.DateTime> DateOfClaim { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual tblPolicyMemberDetail tblPolicyMemberDetail { get; set; }
    }
}
