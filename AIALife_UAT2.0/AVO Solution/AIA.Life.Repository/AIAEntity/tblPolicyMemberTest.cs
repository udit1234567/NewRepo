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
    
    public partial class tblPolicyMemberTest
    {
        public int MemberTestID { get; set; }
        public decimal MemberID { get; set; }
        public string TestName { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual tblPolicyMemberDetail tblPolicyMemberDetail { get; set; }
    }
}
