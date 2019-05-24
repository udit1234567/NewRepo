using AIA.Life.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Policy
{
    public class  LifeAssuredDetails
    {

        public string FullName { get; set; }
        public string CorporateName { get; set; }
        public string NameWithInitials { get; set; }
        public string PrefferedName { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public int? OccupationID { get; set; }
        public int? MaritalStatus { get; set; }
        public int? Nationality { get; set; }
        public string MobileNumber { get; set; }
        public string HomeNumber { get; set; }
        public string WorkNumber { get; set; }
        public string EmailId { get; set; }
        public string IdentificationType { get; set; }
        public string NICNo { get; set; }
        public decimal HdnClientIDExists { get; set; }
        public string ClientCode { get; set; }
        public bool ClientType { get; set; }
        public string Salutation { get; set; }
        public Address objAddress { get; set; }
        public string Language { get; set; }
        public int Age { get; set; }
        public string MonthlyIncome { get; set; }
        public string OLDNICNo { get; set; }
        public string NatureOfDuties { get; set; }
        public string RelationshipwithProspect { get; set; }
    }
}
