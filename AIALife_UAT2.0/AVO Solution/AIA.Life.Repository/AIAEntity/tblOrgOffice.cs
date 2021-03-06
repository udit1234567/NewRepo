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
    
    public partial class tblOrgOffice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblOrgOffice()
        {
            this.tblHierarchyCoordinations = new HashSet<tblHierarchyCoordination>();
        }
    
        public decimal OrgOfficeID { get; set; }
        public Nullable<int> Org_CategoryID { get; set; }
        public Nullable<decimal> OrganizationID { get; set; }
        public Nullable<int> ConfigurationTypeID { get; set; }
        public string OfficeName { get; set; }
        public string OfficeCode { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNo { get; set; }
        public Nullable<int> LevelID { get; set; }
        public Nullable<int> ReportingOfficeID { get; set; }
        public string ReportingOfficeCode { get; set; }
        public Nullable<decimal> MailingAddressID { get; set; }
        public string ServiceTax { get; set; }
        public string EmailID { get; set; }
        public string SPOC_Name { get; set; }
        public Nullable<decimal> SPOC_AddressID { get; set; }
        public string SPOC_PhoneNo { get; set; }
        public string SPOC_EmailId { get; set; }
        public string SPOC_FaxNo { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public Nullable<bool> IsValid { get; set; }
        public Nullable<int> BankBranchID { get; set; }
        public string AccountNumber { get; set; }
        public string PayeeName { get; set; }
        public Nullable<decimal> MobileNo { get; set; }
        public Nullable<decimal> OfficePhone1 { get; set; }
        public Nullable<decimal> OfficePhone2 { get; set; }
        public string Email { get; set; }
        public string CommAddressId { get; set; }
        public string RegistrationAddressId { get; set; }
        public Nullable<bool> IsRegAddressSameAsCommAddress { get; set; }
        public Nullable<System.DateTime> EffectiveFrom { get; set; }
        public Nullable<bool> IsADO { get; set; }
        public Nullable<decimal> CoordinatingOfficeId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<decimal> OrgStructureID { get; set; }
        public Nullable<bool> IsPartnerHierarchy { get; set; }
        public Nullable<bool> IsPartnerCentralizedType { get; set; }
        public string ReportingEntityType { get; set; }
    
        public virtual tblAddress tblAddress { get; set; }
        public virtual tblAddress tblAddress1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblHierarchyCoordination> tblHierarchyCoordinations { get; set; }
        public virtual tblOrganization tblOrganization { get; set; }
        public virtual tblOrgStructure tblOrgStructure { get; set; }
    }
}
