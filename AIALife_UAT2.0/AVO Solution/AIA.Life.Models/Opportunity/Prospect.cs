
using AIA.Life.Models.Common;
using AIA.Life.Models.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace AIA.Life.Models.Opportunity
{
    public class Prospect
    {
        public Prospect()
        {
            objNeedAnalysis = new NeedAnalysis.NeedAnalysis();
            objAddress = new Address();
            ObjSuspectPool = new List<SuspectPool>();
            ObjProspectPool = new List<ProspectPool>();
            lstSalutation = new List<MasterListItem>();
            ListPlan = new List<MasterListItem>();
            lstOccupation = new List<MasterListItem>();
            lstGender = new List<MasterListItem>();
            MaritalStatuslist = new List<MasterListItem>();
            lstRelations = new List<MasterListItem>();
            lstMotorVehicle = new List<MasterListItem>();
            lstDependentRelationship = new List<MasterListItem>();
            lstAvgMonthlyIncome = new List<MasterListItem>();
            lstCurrentStatus = new List<MasterListItem>();
            lstNeedsPriority = new List<MasterListItem>();
            lstPurposeOfMeeting = new List<MasterListItem>();
            LstPensionPeriod = new List<MasterListItem>();
            LstRetirementAge = new List<MasterListItem>();
            LstDrawDownPeriod = new List<MasterListItem>();
            LstType = new List<MasterListItem>();
            ListVariant = new List<MasterListItem>();
            LstMaturityBenefits = new List<MasterListItem>();
            Error = new Error();
            objPreviousInsuranceList = new List<Opportunity.PreviousInsuranceList>();
        }
        public string WPName { get; set; }
        public string WPCode { get; set; }
        public string WPPhone { get; set; }
        public string HdnAutOccupation { get; set; }
        public List<PreviousInsuranceList> objPreviousInsuranceList { get; set; }
        public Error Error { get; set; }
        public string hdnValue { get; set; }
        public string CreatedBy { get; set; }
        public List<MasterListItem> LstType { get; set; }
        public List<MasterListItem> LstPensionPeriod { get; set; }
        public List<MasterListItem> LstRetirementAge { get; set; }
        public List<MasterListItem> LstDrawDownPeriod { get; set; }
        public List<MasterListItem> LstMaturityBenefits { get; set; }        
        public string Prefix { get; set; }
        public string UserName { get; set; }
        public bool IsForServices { get; set; }
        public string Type { get; set; }
        public string Salutation { get; set; }

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
        public string PassPort { get; set; }
        public string Place { get; set; }
        public string Email { get; set; }
        public string Work { get; set; }
        public string Home { get; set; }
        public int? AgeNextBdy { get; set; }
        public int? CurrentAge { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string Occupation { get; set; }
        
        public string Nationality { get; set; }
        public string EmployerName { get; set; }
        public string NIC { get; set; }
        public bool BMI_Exceed { get; set; }
        public bool NICAVAIL { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string DisplayMaritalStatus { get; set; }
        public string AvgMonthlyIncome { get; set; }
        public string Message { get; set; }
        public int ContactID { get; set; }
        public string AssignedTo { get; set; }
        public Address objAddress { get; set; }
        public NeedAnalysis.NeedAnalysis objNeedAnalysis { get; set; }
        public List<NeedAnalysis.FutureFinancial> objFutureFinancial { get; set; }  
        public string Upload { get; set; }

        public string Signature { get; set; }
        public string NotePad { get; set; }
        public byte[] NotePadByteArray { get; set; }
        public byte[] ByteArrayGraph { get; set; } 
        public byte[] ByteArray { get; set; }
        public byte[] ByteArray1 { get; set; }
        public byte[] ByteArray2 { get; set; }
        public byte[] ByteArray3 { get; set; }
        public byte[] ByteArray4 { get; set; }
        public byte[] ByteArray5 { get; set; }
        public byte[] ByteArray6 { get; set; }
        public string ProtectionByteArraygraph { get; set; }
        public string HealthByteArraygraph { get; set; }

        public List<SuspectPool> ObjSuspectPool { get; set; }
        public List<ProspectPool> ObjProspectPool { get; set; }

        public bool IsConfirmedProspect { get; set; }
        public bool IsNeedAnalysisCompleted { get; set; }
        public int ProspectStage { get; set; }
        public string SendEmail { get; set; }
        public string ReasonForRemove { get; set; }

        public List<MasterListItem> ListVariant { get; set; }
        public List<MasterListItem> ListPlan { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstSalutation { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstOccupation { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstGender { get; set; }
        [XmlIgnore]
        public List<MasterListItem> MaritalStatuslist { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstRelations { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstMotorVehicle { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstDependentRelationship { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstAvgMonthlyIncome { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstCurrentStatus { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstNeedsPriority { get; set; }
        [XmlIgnore]
        public List<MasterListItem> lstPurposeOfMeeting { get; set; }
        public string ClientCode { get; set; }
        public string SamsLeadNumber { get; set; }
        public string IntroducerCode { get; set; }
        public string ServiceTraceID { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class SuspectPool
    {
        public SuspectPool()
        {
            lstAssignedTo = new List<MasterListItem>();
        }
        public string UserName { get; set; }
        public int? SelectedSuspect { get; set; }
        public int? SuspectId { get; set; }
        public string SuspectType { get; set; }
        public string SuspectName { get; set; }
        public string SuspectLastName { get; set; }
        public string SuspectMobile { get; set; }
        public string SuspectWork { get; set; }
        public string SuspectEmail { get; set; }
        public int SuspectDaysleft { get; set; }
        public string NIC { get; set; }
        public string Title { get; set; }
        public string Place { get; set; }
        public string LeadNo { get; set; }
        public string LeadDate { get; set; }
        public string Salutation { get; set; }
        public int ContactId { get; set; }
        public bool IsSelected { get; set; }
        public string Passport { get; set; }
        public string AssignedTo { get; set; }
        public int RowId { get; set; }
        public List<MasterListItem> lstAssignedTo { get; set; }   
        public string Message { get; set; }  
          public string FullName { get; set; }
    }
    public class ProspectPool
    {
        public int? ProspectId { get; set; }
        public string ProspectType { get; set; }
        public string ProspectName { get; set; }
        public string ProspectLastName { get; set; }
        public string ProspectMobile { get; set; }
        public string ProspectHome { get; set; }
        public string ProspectWork { get; set; }
        public string ProspectEmail { get; set; }
        public string ProspectNicNo { get; set; }
        public string LeadNo { get; set; }
        public string LeadDate { get; set; }
        public string Dob { get; set; }
        public string Place { get; set; }
        public string Salutation { get; set; }
        public int ProspectDaysleft { get; set; }
        public string FullName { get; set; }
    }

}
