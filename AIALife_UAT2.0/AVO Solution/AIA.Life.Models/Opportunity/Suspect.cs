using AIA.Life.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Opportunity
{
    public  class Suspect
    {
        public Suspect()
        {
            LstSalutation = new List<MasterListItem>();
            LstOccupation = new List<MasterListItem>();
            LstType = new List<MasterListItem>();
            Error = new Common.Error();
        }
        public Error Error { get; set; }
        public string Prefix { get; set; }
        public string CreatedBy { get; set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = string.IsNullOrEmpty(value) != true ? value.Trim() : value; }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = string.IsNullOrEmpty(value) != true ? value.Trim() : value;
            }
        }

        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Work { get; set; }
        public string Home { get; set; }
        public string NIC { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }        
        public string Place { get; set; }
        public string Passport { get; set; }
        public string Message { get; set; }      
        public int ContactID { get; set; }
        public List<MasterListItem> LstSalutation { get; set; }
        public List<MasterListItem> LstOccupation { get; set; }
        public DateTime LeadCreationDate { get; set; }
        public string AgentCode { get; set; }
        public string SamsLeadNumber { get; set; }
        public string AutoCompleteValue { get; set; }
        public List<MasterListItem> LstType { get; set; }
        public List<MasterListItem> LstIntroducerCode { get; set; }
        public string ClientCode { get; set; }
        public string Role { get; set; }
        public string IntroducerCode { get; set; }
        public string ServiceTraceID { get; set; }
        public string UserName { get; set; }
    }
}
