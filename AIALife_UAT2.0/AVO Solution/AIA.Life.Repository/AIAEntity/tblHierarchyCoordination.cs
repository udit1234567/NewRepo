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
    
    public partial class tblHierarchyCoordination
    {
        public decimal CoordinationID { get; set; }
        public Nullable<decimal> PartnerID { get; set; }
        public Nullable<decimal> OrgOfficeID { get; set; }
        public string Partner { get; set; }
        public string UnitType { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    
        public virtual tblOrgOffice tblOrgOffice { get; set; }
    }
}
