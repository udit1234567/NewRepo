using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIA.Life.Models.Common;
namespace AIA.Life.Models.Hierarchy
{
    public class HierarchyData
    {
        public string EntityType { get; set; }
        public List<MasterListItem> LstEntityTypes { get; set; }
        public string ReportEntityType { get; set; }
        public List<MasterListItem> LstReportEntityType { get; set; }
        public string SubChannel { get; set; }
        public IEnumerable<MasterListItem> LstSubChannel { get; set; }
        public string GeoUnitsSubChannel { get; set; }
        public IEnumerable<MasterListItem> LstGeoUnitsSubChannel { get; set; }
        public string GeoUnitsTypes { get; set; }
        public IEnumerable<MasterListItem> LstGeoUnitsTypes { get; set; }
        public List<SearchHierarchy> LstSearchHierarchy { get; set; }
        public string PointofContractData { get; set; }
        public string CoordinationData { get; set; }
        public string GeoUnitData { get; set; }
        public string GeoUnitNameGridCount { get; set; }
        public string ParentEntityGridCount { get; set; }
        public string Name { get; set; }
        public string ReportingCode { get; set; }
        public string ReportingName { get; set; }
        public int? YearofEstablish { get; set; }
        public string RegNo { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public long? Mobile1 { get; set; }
        public long? Mobile2 { get; set; }
        public long? OfficePhone { get; set; }
        public long? OfficePhone1 { get; set; }
        public long? OfficePhone2 { get; set; }
        public string Fax { get; set; }
        public long? ResidencePhone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Provience { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string City_Town { get; set; }
        public bool IsRegAsCommunication { get; set; }
        public string Relationship { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Middlename { get; set; }
        public List<HeirarchyHistroyDetails> ObjHeirarchyHistroyDetails { get; set; }
        public string HistoryDetails { get; set; }
        public IEnumerable<MasterListItem> LstHistoryDetails { get; set; }
        public List<HierachyStatusDetails> ObjHierarchyStatusDetails { get; set; }
        public string Status { get; set; }
        public List<MasterListItem> LstStatus { get; set; }
        public string Effectivefrom { get; set; }
        public bool IsPartnerHierarchy { get; set; }
        public List<GEOUnitDetails> ObjGEOUnitDetails { get; set; }
        public IEnumerable<MasterListItem> LstGEOParententity { get; set; }
        public List<ManpowerDetails> ObjManpowerDetails { get; set; }
        public List<Coordination> ObjCoordination { get; set; }
        public string PartnerType { get; set; }
        public string GeoPartner { get; set; }
        public List<MasterListItem> LstPartnertypes { get; set; }
        public List<MasterListItem> LstGeoPartner { get; set; }
        public bool IsPartnerInsuranceType { get; set; }
        public string LicenseNo { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExperiyDate { get; set; }
        public DateTime? ContractEffetiveFrom { get; set; }
        public DateTime? ContractEffetiveTo { get; set; }
        public List<HierarchyTarget> ObjHierarchyTarget { get; set; }
        public string Salutation { get; set; }
        public string SLIIRegNo { get; set; }
        public List<DocumentDetails> LstdocumentName { get; set; }
        public List<CoordinationSearchData> ObjCoordinationSearchData { get; set; }
        public string Position { get; set; }
        public IEnumerable<MasterListItem> LstPosition { get; set; }
        public List<PointOfContacts> LstPointOfContacts { get; set; }
        public IEnumerable<MasterListItem> LstSalutation { get; set; }
        public Address objCommunicationAddress { get; set; }
        public Address objRegistrationAddress { get; set; }
        public List<ViewHierarchyTree> LstViewHierarchyTree { get; set; }
        public List<OrgStructureTree> LstOrgStructureTree { get; set; }
        public string Message { get; set; }
        public string CorporateCode { get; set; }
        public string ChannelCode { get; set; }
        public string SubChannelCode { get; set; }
        public string PartnerCode { get; set; }
        public string UserName { get; set; }
        public string GeoUnitCode { get; set; }
        public List<Address> LstProvince { get; set; }
        public List<Address> LstDistrict { get; set; }
        public List<Address> LstCity { get; set; }
        public string SearchCode { get; set; }
        public string SearchName { get; set; }
        public string SearchStatus { get; set; }
        public string SearchEntityType { get; set; }
        public string CoordinationChannel { get; set; }
        public string CoordinationSubChannel { get; set; }
        public string CoordinationSearchCode { get; set; }
        public string CoordinationSearchGeoUnitName { get; set; }
        public IEnumerable<MasterListItem> LstCoordinationChannel { get; set; }
        public IEnumerable<MasterListItem> LstCoordinationSubChannel { get; set; }
        public List<MasterListItem> LstReportingEntityCode { get; set; }
        public List<HierarchyData> LstReportingEntityCodeName { get; set; }
        public List<MasterListItem> LstReportingEntityName { get; set; }
        public int DocumentUploadCount { get; set; }
        public string SearchAction { get; set; }
        public IEnumerable<MasterListItem> LstSearchAction { get; set; }
        public string SearchSubAction { get; set; }
        public IEnumerable<MasterListItem> LstSearchSubAction { get; set; }
        public string SearchHierarchyData { get; set; }
        public int OrgStructureIDChannel { get; set; }
        public int OrgStructureIDSubChannel { get; set; }
        public int ReportingEntityId { get; set; }
        public string SearchTerm { get; set; }
        public string SearchCodeGrid { get; set; }
        public List<MasterListItem> LstCoordinationGeoUnitCode { get; set; }
        public List<HierarchyData> LstCoordinationGeoUnitName { get; set; }
        //public string CoordinationSearchIDSerializer { get; set; }
        //public string PartnerSerializer { get; set; }
        //public string GeoUnitTypeSerializer { get; set; }
        //public string GeoUnitNameSerializer { get; set; }
        //public string GeoUnitCodeSerializer { get; set; }
        public int CommunicationAddressId { get; set; }
        public int RegistrationAddressId { get; set; }
        public bool IsRegAddressSameAsCommAddress { get; set; }
    }
    public class HeirarchyHistroyDetails
    {
        public string Code { get; set; }
        public string EntityType { get; set; }
        //public DateTime? StartDate { get; set; }
        //public DateTime? EndDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Repository { get; set; }
        //public DateTime? DateMode { get; set; }
        public string DateMode { get; set; }
        public string User { get; set; }       
    }
    public class HierachyStatusDetails
    {
        public string Code { get; set; }
        public string EntityType { get; set; }
        //public DateTime? StartDate { get; set; }
        //public DateTime? EndDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; }
        //public DateTime? DateMode { get; set; }
        public string DateMode { get; set; }
        public string User { get; set; }
    }
    public class TreeView
    {
        public string ItemDesc { get; set; }
        public int ItemId { get; set; }
        public int Parent { get; set; }
        public int depth { get; set; }
        public string ItemType { get; set; }
        public bool? IsSelected { get; set; }
        public bool? IsIndet { get; set; }
        public List<TreeView> items { get; set; }
        public decimal? ProductId { get; set; }
    }
    public class PermissionTree
    {
        public List<TreeView> objTree { get; set; }
        public bool isIMEIChecked { get; set; }
        public string IMEINumber { get; set; }
    }

    public class ViewHierarchyTree
    {
        public int? ItemId { get; set; }
        public string ItemDescription { get; set; }
        public string ItemNavUrl { get; set; }
        public int? ParentId { get; set; }
    }

    public class OrgStructureTree
    {
        public int? ItemId { get; set; }
        public string ItemDescription { get; set; }
        public string ItemNavUrl { get; set; }
        public int? ParentId { get; set; }
    }

    public class GEOUnitDetails
    {
        public int? GeounitID { get; set; }
        public string Geounitname { get; set; }
        public string Parententity { get; set; }
        public IEnumerable<MasterListItem> LstParententity { get; set; }
    }
    public class ManpowerDetails
    {
        public string Position { get; set; }
        public string Designation { get; set; }
        public int NoofEmployees { get; set; }
    }
    //public class Coordination
    //{
    //    public int CoordinateID { get; set; }
    //    public bool IsChkCoordinateID { get; set; }
    //    public string Partner { get; set; }
    //    public string Geounittype { get; set; }
    //    public string Name { get; set; }
    //    public string Code { get; set; }
    //}
    public class Coordination
    {
        public int CoordinationSearchID { get; set; }
        public bool IsCoordinationSearch { get; set; }
        public int CoordinateID { get; set; }
        public string Code { get; set; }
        public string Geounitname { get; set; }
        public string Geounittype { get; set; }
        public string Partner { get; set; }
        public string Subchannel { get; set; }
        public string Channel { get; set; }
    }
    public class HierarchyTarget
    {
        public string FinancialYear { get; set; }
        public string Targets { get; set; }
        public string Achievements { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
    public class DocumentDetails
    {
        public string DocumentName { get; set; }
        public string FileUploadname { get; set; }
        public int DocumentID { get; set; }
        public string ExistingFileName { get; set; }
        public byte[] FileContent { get; set; }
        public int Index { get; set; }
    }
    public class CoordinationSearchData
    {
        public int CoordinationSearchID { get; set; }
        public bool IsCoordinationSearch { get; set; }
        public int CoordinateID { get; set; }
        public string Code { get; set; }
        public string Geounitname { get; set; }
        public string Geounittype { get; set; }
        public string Partner { get; set; }
        public string Subchannel { get; set; }
        public string Channel { get; set; }
    }
    public class PointOfContacts
    {
        public int Index { get; set; }
        public string Position { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SLIIRegNo { get; set; }
        public string LastName { get; set; }
        public long? Mobile1 { get; set; }
        public long? Mobile2 { get; set; }
        public long? OfficePhone { get; set; }
        public long? ResidencePhone { get; set; }
        public string Email { get; set; }      
    }
    public class SearchHierarchy
    {
        public int SearchID { get; set; }
        public string OfficeID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string EntityType { get; set; }
        public string ParentType { get; set; }
        public string Channel { get; set; }
        public string SubChannel { get; set; }
        public string Partner { get; set; }
        public string LastModifiedDate { get; set; }
        public string User { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public bool IsPartner { get; set; }
    }
}
