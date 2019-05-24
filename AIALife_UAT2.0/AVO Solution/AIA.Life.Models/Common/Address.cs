using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AIA.Life.Models.Common
{

    public class Address
    {
        public Address()
        {
            LstPincode = new List<MasterListItem>();
        }
        public decimal AddressID { get; set; }
        public int AddressTypeId { get; set; }
        private string _address1;
        public string Address1
        {
            get { return _address1; }
            set { _address1 = string.IsNullOrEmpty(value) == false ? value.Trim() : value; }
        }
        private string _address2;
        public string Address2
        {
            get { return _address2; }
            set { _address2 = string.IsNullOrEmpty(value) == false ? value.Trim() : value; }
        }
        private string _address3;
        public string Address3
        {
            get { return _address3; }
            set { _address3 = string.IsNullOrEmpty(value) == false ? value.Trim() : value; }
        }
        public int? PincodeID { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
        public string PincodeNew { get; set; }
        public int? StateID { get; set; }
        public string Province { get; set; }
        public string ProvinceCode { get; set; }
        public string DistrictCode { get; set; }
        public string CityCode { get; set; }
        [XmlIgnore]
        public List<MasterListItem> LstPincode { get; set; }
    }
    public class AddressMaster
    {
        public string Term { get; set; }
        public List<MasterListItem> LstAddressMaster { get; set; }
    }

    public class PolicyDetails
    {
        public List<PreviousPolicyDetails> PreviousProposalsList { get; set; }
        public List<OnGoingProposalDetails> OnGoingProposalsList { get; set; }
        public bool AFC { get; set; }
    }
}
