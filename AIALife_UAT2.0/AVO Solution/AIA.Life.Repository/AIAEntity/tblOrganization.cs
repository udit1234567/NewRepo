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
    
    public partial class tblOrganization
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblOrganization()
        {
            this.tblContactDetails = new HashSet<tblContactDetail>();
            this.tblCorporates = new HashSet<tblCorporate>();
            this.tblCorporates1 = new HashSet<tblCorporate>();
            this.tblCorporates2 = new HashSet<tblCorporate>();
            this.tblCorporates3 = new HashSet<tblCorporate>();
            this.tblOrgOffices = new HashSet<tblOrgOffice>();
            this.tblOrgStructures = new HashSet<tblOrgStructure>();
            this.tblPolicyRelationships = new HashSet<tblPolicyRelationship>();
            this.tblProspectDocuments = new HashSet<tblProspectDocument>();
        }
    
        public decimal OrganizationID { get; set; }
        public int Org_CategoryID { get; set; }
        public int ConfigurationTypeID { get; set; }
        public Nullable<int> OtherConfigureID { get; set; }
        public int Org_TypeID { get; set; }
        public string Org_Name { get; set; }
        public Nullable<decimal> RegisteredAddresID { get; set; }
        public Nullable<bool> IsRegisteredAddressSame { get; set; }
        public Nullable<decimal> CorporateAddressID { get; set; }
        public Nullable<int> MailingAddressReferenceID { get; set; }
        public Nullable<decimal> MailingAddressID { get; set; }
        public byte[] Org_Logo { get; set; }
        public string Org_Website { get; set; }
        public string Org_PhoneNO { get; set; }
        public string Org_FaxNO { get; set; }
        public Nullable<int> Org_Levels { get; set; }
        public string Regno { get; set; }
        public string Reg_Authority { get; set; }
        public Nullable<System.DateTime> Reg_Date { get; set; }
        public string Reg_no_st { get; set; }
        public string PANno { get; set; }
        public string TANno { get; set; }
        public string SPOC_Name { get; set; }
        public Nullable<decimal> SPOC_AdderssID { get; set; }
        public string SPOC_Phoneno { get; set; }
        public string SPOC_EmailId { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public Nullable<bool> IsValid { get; set; }
        public string Code { get; set; }
        public string CINNo { get; set; }
        public Nullable<int> BankBranchID { get; set; }
        public string AccountNumber { get; set; }
        public string PayeeName { get; set; }
        public string License { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Org_Type { get; set; }
        public Nullable<decimal> YearOfEstablishment { get; set; }
        public string RegistrationNo { get; set; }
        public Nullable<System.DateTime> EffectiveFrom { get; set; }
        public Nullable<decimal> MobileNo { get; set; }
        public Nullable<decimal> OfficePhone1 { get; set; }
        public Nullable<decimal> OfficePhone2 { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string CommAddressId { get; set; }
        public string RegistrationAddressId { get; set; }
        public Nullable<bool> IsRegAddressSameAsCommAddress { get; set; }
        public string PartnerType { get; set; }
        public string LicenseNo { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<System.DateTime> ContractEffectiveFrom { get; set; }
        public Nullable<System.DateTime> ContractEffectiveTo { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CorporateCode { get; set; }
        public string ChannelCode { get; set; }
        public string SubChannelCode { get; set; }
        public string PartnerCode { get; set; }
        public string GeoUnitCode { get; set; }
    
        public virtual tblAddress tblAddress { get; set; }
        public virtual tblAddress tblAddress1 { get; set; }
        public virtual tblAddress tblAddress2 { get; set; }
        public virtual tblAddress tblAddress3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblContactDetail> tblContactDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblCorporate> tblCorporates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblCorporate> tblCorporates1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblCorporate> tblCorporates2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblCorporate> tblCorporates3 { get; set; }
        public virtual tblMasCommonType tblMasCommonType { get; set; }
        public virtual tblMasCommonType tblMasCommonType1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblOrgOffice> tblOrgOffices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblOrgStructure> tblOrgStructures { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPolicyRelationship> tblPolicyRelationships { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblProspectDocument> tblProspectDocuments { get; set; }
    }
}
