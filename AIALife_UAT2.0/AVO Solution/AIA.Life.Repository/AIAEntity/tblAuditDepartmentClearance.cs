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
    
    public partial class tblAuditDepartmentClearance
    {
        public decimal AuditClearanceID { get; set; }
        public Nullable<decimal> ProspectID { get; set; }
        public string AuditRecommendation { get; set; }
        public string Target { get; set; }
        public string Achievement { get; set; }
        public string Variance { get; set; }
        public string AuditRemarks { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual tblProspect tblProspect { get; set; }
    }
}
