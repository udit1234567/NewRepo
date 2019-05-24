using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace AIA.Life.Models.UserManagement
{
    public class Address
    {
        public decimal AddressID { get; set; }
        public int AddressTypeId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int? CityID { get; set; }
        public int? AreaID { get; set; }
        public int? DistrictID { get; set; }
        public int? StateID { get; set; }
        public int? Pincode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? Status { get; set; }
        public decimal? SourceRowID { get; set; }
        public string CountryID { get; set; }
        public string NearestLandmark { get; set; }

        [XmlIgnore]
        public IEnumerable<SelectListItem> LstAllStates { get; set; }
        [XmlIgnore]
        public IEnumerable<SelectListItem> LstAllCountries { get; set; }
        [XmlIgnore]
        public IEnumerable<SelectListItem> LstAllDistricts { get; set; }
        [XmlIgnore]
        public IEnumerable<SelectListItem> LstAllCities { get; set; }
        [XmlIgnore]
        public IEnumerable<SelectListItem> LstAllAreas { get; set; }
    }

    public class AddressClass
    {      
        public Address TestAddress { get; set; }
        
    }
    public class MasterList
    {
        public int ID_PK { get; set; }
        public string Value { get; set; }
    }
    public class AreaMasterList
    {
        public string Value { get; set; }
        public long AreaID { get; set; }
        public string Pincode { get; set; }
    }


}