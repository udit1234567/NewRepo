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
    
    public partial class tblProspectEducationDetail
    {
        public decimal ProspectEducationDetailsID { get; set; }
        public Nullable<decimal> ProspectEducationID { get; set; }
        public Nullable<decimal> SubjectID { get; set; }
        public string Grade { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string QualificationGceId { get; set; }
    
        public virtual tblProspectEducation tblProspectEducation { get; set; }
    }
}