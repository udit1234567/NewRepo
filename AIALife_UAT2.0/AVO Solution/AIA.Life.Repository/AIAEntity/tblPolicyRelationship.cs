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
    
    public partial class tblPolicyRelationship
    {
        public decimal PolicyRelationshipID { get; set; }
        public Nullable<decimal> PolicyID { get; set; }
        public Nullable<decimal> PolicyClientID { get; set; }
        public Nullable<int> RelationshipID { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<decimal> ProspectID { get; set; }
        public Nullable<decimal> OrganizationID { get; set; }
    
        public virtual tblOrganization tblOrganization { get; set; }
        public virtual tblProspect tblProspect { get; set; }
        public virtual tblPolicyClient tblPolicyClient { get; set; }
        public virtual tblPolicy tblPolicy { get; set; }
    }
}