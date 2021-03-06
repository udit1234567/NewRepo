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
    
    public partial class tblProspectEducation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblProspectEducation()
        {
            this.tblProspectEducationDetails = new HashSet<tblProspectEducationDetail>();
            this.tblSpecialAchievements = new HashSet<tblSpecialAchievement>();
        }
    
        public decimal ProspectEducationID { get; set; }
        public Nullable<decimal> ProspectID { get; set; }
        public Nullable<decimal> QualificationID { get; set; }
        public Nullable<System.DateTime> MonthYearFrom { get; set; }
        public string SLIIQualification { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string SchoolName { get; set; }
        public string Grade { get; set; }
        public string MonthYearTo { get; set; }
    
        public virtual tblProspect tblProspect { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblProspectEducationDetail> tblProspectEducationDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblSpecialAchievement> tblSpecialAchievements { get; set; }
    }
}
