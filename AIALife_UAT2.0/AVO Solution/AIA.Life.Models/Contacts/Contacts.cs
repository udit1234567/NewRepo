using JanaShakthi.Life.Models.Common;
using JanaShakthi.Life.Models.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JanaShakthi.Life.Models.Contacts
{
    

    public class Contacts
    {
        public Contacts()
        {
            lstGender = new List<Common.MasterListItem>();
            lstOccupation = new List<Common.MasterListItem>();
            lstSalutation = new List<Common.MasterListItem>();
        }

        public int? SuspectId { get; set; }
        public string SuspectName { get; set; }
        public long SuspectMobile { get; set; }
        public string SuspectHome { get; set; }
        public string SuspectWork { get; set; }
        public string SuspectEmailID { get; set; }

        public int? ProspectId { get; set; }
        public string ProspectType { get; set; }        
        public Address objAddress { get; set; }
        public ClientDetails objClientDetails { get; set; }

        [XmlIgnore]
        public List<MasterListItem> lstSalutation { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstOccupation { get; set; }
        [XmlIgnore]
        public List<MasterListItem> MaritalStatuslist { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstGender { get; set; }

    }
}
