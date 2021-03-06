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
    
    public partial class tblContact
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblContact()
        {
            this.tblContactDependents = new HashSet<tblContactDependent>();
            this.tblContacts1 = new HashSet<tblContact>();
            this.tblDependants = new HashSet<tblDependant>();
            this.tblOpportunities = new HashSet<tblOpportunity>();
            this.tblLifeQQs = new HashSet<tblLifeQQ>();
            this.tblLifeNeedAnalysis = new HashSet<tblLifeNeedAnalysi>();
        }
    
        public int ContactID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public Nullable<int> ContactTypeID { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string Work { get; set; }
        public Nullable<int> Age { get; set; }
        public Nullable<int> OccupationID { get; set; }
        public string NICNO { get; set; }
        public Nullable<decimal> MaritalStatusID { get; set; }
        public Nullable<decimal> NationalityID { get; set; }
        public string MonthlyIncome { get; set; }
        public Nullable<int> FamilyMembersCount { get; set; }
        public Nullable<int> DependenceCount { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<decimal> AddressID { get; set; }
        public Nullable<int> ParentContactID { get; set; }
        public Nullable<bool> Isparent { get; set; }
        public string Relationship { get; set; }
        public string Employer { get; set; }
        public string ContactType { get; set; }
        public string LeadNo { get; set; }
        public string Place { get; set; }
        public string PassportNo { get; set; }
        public string CreatedBy { get; set; }
        public string ClientCode { get; set; }
        public string SpouseName { get; set; }
        public Nullable<System.DateTime> SpouseDOB { get; set; }
        public Nullable<int> SpouseAge { get; set; }
        public Nullable<int> CurrentAge { get; set; }
        public Nullable<int> SpouseCurrentAge { get; set; }
        public string IntroducerCode { get; set; }
    
        public virtual tblAddress tblAddress { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblContactDependent> tblContactDependents { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblContact> tblContacts1 { get; set; }
        public virtual tblContact tblContact1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblDependant> tblDependants { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblOpportunity> tblOpportunities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblLifeQQ> tblLifeQQs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblLifeNeedAnalysi> tblLifeNeedAnalysis { get; set; }
    }
}
