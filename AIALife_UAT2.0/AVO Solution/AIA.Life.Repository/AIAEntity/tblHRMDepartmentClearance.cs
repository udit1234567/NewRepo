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
    
    public partial class tblHRMDepartmentClearance
    {
        public decimal HRMClearanceID { get; set; }
        public Nullable<decimal> ProspectID { get; set; }
        public string EPFNo { get; set; }
        public string Department { get; set; }
        public Nullable<System.DateTime> DateOfResignation { get; set; }
        public string ReasonForLeaving { get; set; }
        public Nullable<System.DateTime> PeriodOfEmployement { get; set; }
        public Nullable<decimal> PreviousSalary { get; set; }
        public string PreviousTravel { get; set; }
        public string HRMRecommendation { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual tblProspect tblProspect { get; set; }
    }
}
