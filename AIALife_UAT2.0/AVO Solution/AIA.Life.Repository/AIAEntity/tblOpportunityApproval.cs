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
    
    public partial class tblOpportunityApproval
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblOpportunityApproval()
        {
            this.tblSupervisorDecisions = new HashSet<tblSupervisorDecision>();
        }
    
        public int OpportunityApprovalId { get; set; }
        public Nullable<int> ContactId { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public string NIC { get; set; }
        public string Stage { get; set; }
        public Nullable<System.DateTime> Createddate { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblSupervisorDecision> tblSupervisorDecisions { get; set; }
    }
}
