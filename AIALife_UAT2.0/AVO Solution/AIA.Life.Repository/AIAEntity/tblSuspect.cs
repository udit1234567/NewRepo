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
    
    public partial class tblSuspect
    {
        public decimal SuspectID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Nullable<decimal> MobileNo { get; set; }
        public string EmailID { get; set; }
        public Nullable<decimal> ProspectID { get; set; }
        public string SuspectCode { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<decimal> Status { get; set; }
        public Nullable<bool> IsSelected { get; set; }
    
        public virtual tblProspect tblProspect { get; set; }
    }
}