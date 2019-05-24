using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Integration.AgentOnBoarding
{
    public class BasicDetails
    {
        public string employeeType { get; set; }
        public string appointmentDate { get; set; }
        public string status { get; set; }
    }

    public class PersonalInformation
    {
        public string title { get; set; }
        public string gender { get; set; }
        public string dob { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string maritalStatuse { get; set; }
        public string nicNo { get; set; }
        public string nationality { get; set; }
        public string salesExperience { get; set; }
        public string industryExperience { get; set; }
        public string mobile1 { get; set; }
        public string mobile2 { get; set; }
    }

    public class CommunicationAddress
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string province { get; set; }
        public string district { get; set; }
        public string city_Town { get; set; }
        public string postCode { get; set; }
        //public string isRegistrationAddressSameAsCommunicationAddress? { get; set; }
}

public class PermanentAddress
{
    public string address1 { get; set; }
    public string address2 { get; set; }
    public string province { get; set; }
    public string district { get; set; }
    public string city_Town { get; set; }
    public string postCode { get; set; }
}

public class EmergencyContactDetails
{
    public string fullName { get; set; }
    public string address1 { get; set; }
    public string address2 { get; set; }
    public string province { get; set; }
    public string district { get; set; }
    public string city_Town { get; set; }
    public string zip_PinCode { get; set; }
    public string mobile1 { get; set; }
    public string mobile2 { get; set; }
    public string email { get; set; }
    public string relationship { get; set; }
    public string others { get; set; }
}

public class OfficialDetails
{
    public string printName { get; set; }
    public string position { get; set; }
    public string designation { get; set; }
    public string empNo { get; set; }
    public string epfNo { get; set; }
    public string userId { get; set; }
    public string oldEPFNo { get; set; }
    public string branchCode { get; set; }
}

public class ReportingDetails
{
    public string code { get; set; }
    public string name { get; set; }
    public string position { get; set; }
    public string designation { get; set; }
}

public class Output
{
    public BasicDetails basicDetails { get; set; }
    public PersonalInformation personalInformation { get; set; }
    public CommunicationAddress communicationAddress { get; set; }
    public PermanentAddress permanentAddress { get; set; }
    public EmergencyContactDetails emergencyContactDetails { get; set; }
    public OfficialDetails officialDetails { get; set; }
    public ReportingDetails reportingDetails { get; set; }
}

public class  EmployeeServiceResponse
{
    public List<Output> output { get; set; }
    public string message { get; set; }
    public string status { get; set; }
    public string empId { get; set; }
    public string epfNo { get; set; }
    public string companyCode { get; set; }
    }

}
