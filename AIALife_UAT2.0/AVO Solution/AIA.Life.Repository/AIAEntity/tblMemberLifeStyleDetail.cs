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
    
    public partial class tblMemberLifeStyleDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblMemberLifeStyleDetail()
        {
            this.tblMemberAdditionalLifeStyleDetails = new HashSet<tblMemberAdditionalLifeStyleDetail>();
        }
    
        public int MemberLifeStyleID { get; set; }
        public decimal MemberID { get; set; }
        public string Height { get; set; }
        public string UnitofHeight { get; set; }
        public string Weight { get; set; }
        public string UnitofWeight { get; set; }
        public Nullable<bool> IsWeightSteady { get; set; }
        public Nullable<bool> IsSmoker { get; set; }
        public Nullable<bool> IsAlcoholic { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsNarcoticDrug { get; set; }
        public string HeightFeets { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblMemberAdditionalLifeStyleDetail> tblMemberAdditionalLifeStyleDetails { get; set; }
        public virtual tblPolicyMemberDetail tblPolicyMemberDetail { get; set; }
    }
}
