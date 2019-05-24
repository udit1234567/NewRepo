using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AIA.Life.Models.Integration.AgentOnBoarding;
namespace AIA.Life.Integration.Services.AgentonBoarding
{
  public class AgentonBoardingIntegration
    {
        public AIA.Life.Models.AgentonBoarding.RecruitmentAgent SendAgentDatatoService(Models.AgentonBoarding.RecruitmentAgent objRecruitmentAgent)
        {
            RootObject objectData = MappingAgentDatatoServiceData(ref objRecruitmentAgent);
            string str = GetPostParametersToAPI("Life_Agent_Saving", "Agent", "SaveAgentMaster", objectData);
            RootObject DataSerialize = JsonConvert.DeserializeObject<RootObject>(str);
            objRecruitmentAgent.ServiceStatus = DataSerialize.Status;
            return objRecruitmentAgent;            
        }
        public AIA.Life.Models.AgentonBoarding.RecruitmentAgent FetchCreateEmployeeData(Models.AgentonBoarding.RecruitmentAgent objRecruitmentAgent)
        {
            EmployeeServiceResponse objectData = FetchingEmployeeData(ref objRecruitmentAgent);
            string str = GetPostParametersToAPI("HRM_Sending", "HRM", "employeeDetails", objectData);
            EmployeeServiceResponse DataSerialize = JsonConvert.DeserializeObject<EmployeeServiceResponse>(str);
            if(DataSerialize.output != null)
            {
                if(DataSerialize.output[0].basicDetails != null)
                {
                    var basicDetails = DataSerialize.output[0].basicDetails;
                    objRecruitmentAgent.EmployeeType = basicDetails.employeeType;
                    objRecruitmentAgent.AppointmentDate =Convert.ToDateTime(basicDetails.appointmentDate);
                    objRecruitmentAgent.Status = basicDetails.status;
                }
                if (DataSerialize.output[0].personalInformation != null)
                {
                    var personalInformation = DataSerialize.output[0].personalInformation;
                    objRecruitmentAgent.Title = personalInformation.title;
                    objRecruitmentAgent.FirstName = personalInformation.firstName;
                    objRecruitmentAgent.MiddleName = personalInformation.middleName;
                    objRecruitmentAgent.LastName = personalInformation.lastName;
                    objRecruitmentAgent.DOB =Convert.ToDateTime(personalInformation.dob);
                    objRecruitmentAgent.Gender = personalInformation.gender;
                    objRecruitmentAgent.MaritalStatus = personalInformation.maritalStatuse;
                    objRecruitmentAgent.Nationality = personalInformation.nationality;
                    objRecruitmentAgent.NICNO = personalInformation.nicNo;
                    objRecruitmentAgent.Profession = personalInformation.industryExperience;
                    objRecruitmentAgent.Salesexp = personalInformation.salesExperience;
                    objRecruitmentAgent.Industryexp = personalInformation.industryExperience;
                    if(personalInformation.mobile1 != null && personalInformation.mobile1 != " ")
                    //objRecruitmentAgent.MobileNo =Convert.ToInt64(personalInformation.mobile1);
                    if (personalInformation.mobile2 != null && personalInformation.mobile2 != " ")
                        objRecruitmentAgent.AltMobileNo= Convert.ToInt64(personalInformation.mobile2);                    

                    // objRecruitmentAgent.SLIIRegNo=personalInformation.ibs
                }
                if(DataSerialize.output[0].communicationAddress != null)
                {
                    var communicationAddress = DataSerialize.output[0].communicationAddress;
                    objRecruitmentAgent.objCommunicationAddress = new Models.Common.Address();
                    objRecruitmentAgent.objCommunicationAddress.Address1 = communicationAddress.address1;
                    objRecruitmentAgent.objCommunicationAddress.Address2 = communicationAddress.address2;
                    objRecruitmentAgent.objCommunicationAddress.State = communicationAddress.province;
                    objRecruitmentAgent.objCommunicationAddress.Pincode = communicationAddress.postCode;
                    objRecruitmentAgent.objCommunicationAddress.City = communicationAddress.city_Town;
                    objRecruitmentAgent.objCommunicationAddress.District = communicationAddress.district;
                }
                if (DataSerialize.output[0].permanentAddress != null)
                {
                    var permanentAddress = DataSerialize.output[0].permanentAddress;
                    objRecruitmentAgent.objPermanentAddress = new Models.Common.Address();
                    objRecruitmentAgent.objPermanentAddress.Address1 = permanentAddress.address1;
                    objRecruitmentAgent.objPermanentAddress.Address2 = permanentAddress.address2;
                    objRecruitmentAgent.objPermanentAddress.State = permanentAddress.province;
                    objRecruitmentAgent.objPermanentAddress.Pincode = permanentAddress.postCode;
                    objRecruitmentAgent.objPermanentAddress.City = permanentAddress.city_Town;
                    objRecruitmentAgent.objPermanentAddress.District = permanentAddress.district;
                }
                if (DataSerialize.output[0].emergencyContactDetails != null)
                {
                    var emergencyContactDetails = DataSerialize.output[0].emergencyContactDetails;
                    objRecruitmentAgent.objEmergencyContactAddress = new Models.Common.Address();
                    objRecruitmentAgent.objEmergencyContactAddress.Address1 = emergencyContactDetails.address1;
                    objRecruitmentAgent.objEmergencyContactAddress.Address2 = emergencyContactDetails.address2;
                    objRecruitmentAgent.objEmergencyContactAddress.State = emergencyContactDetails.province;
                    objRecruitmentAgent.objEmergencyContactAddress.Pincode = emergencyContactDetails.zip_PinCode;
                    objRecruitmentAgent.objEmergencyContactAddress.City = emergencyContactDetails.city_Town;
                    objRecruitmentAgent.objEmergencyContactAddress.District = emergencyContactDetails.district;
                    objRecruitmentAgent.EmergancyRelationship = emergencyContactDetails.relationship;
                    objRecruitmentAgent.EmergancyFirstName = emergencyContactDetails.fullName;
                    if (emergencyContactDetails.mobile1 != null && emergencyContactDetails.mobile1 != " ")
                        objRecruitmentAgent.EmergancyMobile =Convert.ToInt64(emergencyContactDetails.mobile1);
                    objRecruitmentAgent.EmergancyEmail = emergencyContactDetails.email;
                    if (emergencyContactDetails.mobile2 != null && emergencyContactDetails.mobile2 != " ")
                        objRecruitmentAgent.EmergancyAltMobile = Convert.ToInt64(emergencyContactDetails.mobile2);
                    objRecruitmentAgent.EmergancyRelationshipOthers = emergencyContactDetails.others;                    
                }
                if (DataSerialize.output[0].officialDetails != null)
                {
                    var officialDetails = DataSerialize.output[0].officialDetails;
                    objRecruitmentAgent.PrintName = officialDetails.printName;
                    objRecruitmentAgent.EmployeeOfficialDetailsPosition = officialDetails.position;
                    objRecruitmentAgent.Designation = officialDetails.designation;
                    objRecruitmentAgent.EPFNo = officialDetails.epfNo;
                    objRecruitmentAgent.OldEPFNo = officialDetails.oldEPFNo;
                    objRecruitmentAgent.BranchCode = officialDetails.branchCode;
                    //objRecruitmentAgent.ZoneCode=officialDetails.re
                }
                if (DataSerialize.output[0].reportingDetails != null)
                {
                    var reportingDetails = DataSerialize.output[0].reportingDetails;
                    objRecruitmentAgent.ReportingCode = reportingDetails.code;
                    objRecruitmentAgent.ReportingDesignation = reportingDetails.designation;
                    objRecruitmentAgent.ReportingPosition = reportingDetails.position;
                    objRecruitmentAgent.Name = reportingDetails.name;
                }
                }
            return objRecruitmentAgent;
        }
        public EmployeeServiceResponse FetchingEmployeeData(ref Models.AgentonBoarding.RecruitmentAgent objRecruitmentAgent)
        {
            EmployeeServiceResponse objRootObject = new EmployeeServiceResponse();
            objRootObject.epfNo = objRecruitmentAgent.EPFNo;
            objRootObject.empId = objRecruitmentAgent.EmployeeID;
            objRootObject.companyCode = "00001";//objRecruitmentAgent.CompanyCode;
            return objRootObject;
        }
        public RootObject MappingAgentDatatoServiceData(ref Models.AgentonBoarding.RecruitmentAgent objRecruitmentAgent)
        {
            RootObject objRootObject = new RootObject();
            objRootObject.agentMaster = new List<AgentMaster>();
            objRootObject.agentAddressDet = new List<AgentAddressDet>();
            objRootObject.settlementMaster = new List<SettlementMaster>(); 
                AgentMaster objAgentMaster =new  AgentMaster();
               AgentAddressDet objAgentAddressDet = new AgentAddressDet();
               SettlementMaster objSettlementMaster = new SettlementMaster();
                objAgentMaster.agentCode = objRecruitmentAgent.AgentCode;
                objAgentMaster.companyCode = "00003";
                objAgentMaster.agentName = objRecruitmentAgent.FirstName;
                objAgentMaster.appointmentDate = objRecruitmentAgent.AppointedDate;
                objAgentMaster.authorizedDate = Convert.ToString(objRecruitmentAgent.ApplicationDate);
                objAgentMaster.branchCode = objRecruitmentAgent.BranchCode;
               // objAgentMaster.createBy = objRecruitmentAgent.CreatedBy;
               objAgentMaster.createBy = "venkat";
                var create = DateTime.Now;
                objAgentMaster.createDt = create.Day + "-" + create.Month + "-" + create.Year; ;
                objAgentMaster.dateOfBirth = Convert.ToString(objRecruitmentAgent.DOB);
                objAgentMaster.districtCode = objRecruitmentAgent.objCommunicationAddress.DistrictCode;
                objAgentMaster.districtName = objRecruitmentAgent.objCommunicationAddress.District;
                objAgentMaster.epfNo = Convert.ToString(objRecruitmentAgent.EPFNo);
                objAgentMaster.epfNoActual = Convert.ToString(objRecruitmentAgent.EPFNo);
                //objAgentMaster.fax=objRecruitmentAgent
                objAgentMaster.firstName = objRecruitmentAgent.FirstName;
                objAgentMaster.lastName = objRecruitmentAgent.LastName;
                objAgentMaster.maritalStatus = objRecruitmentAgent.MaritalStatus;
                objAgentMaster.middleName = objRecruitmentAgent.MiddleName;
               // objAgentMaster.modifyBy = objRecruitmentAgent.CreatedBy;
            objAgentMaster.modifyBy = "venkat";
            var modifyDate = DateTime.Now;
                objAgentMaster.modifyDt = modifyDate.Day+"-"+ modifyDate.Month+"-"+ modifyDate.Year;
                objAgentMaster.nationality = objRecruitmentAgent.Nationality;
                objAgentMaster.nicNumber = objRecruitmentAgent.NICNO;
                objAgentAddressDet.pinCode = objRecruitmentAgent.objCommunicationAddress.Pincode;
                objAgentMaster.printName = objRecruitmentAgent.PrintName;
                objAgentMaster.provinceCode = Convert.ToString(objRecruitmentAgent.objCommunicationAddress.StateID);
                objAgentMaster.provinceName = objRecruitmentAgent.objCommunicationAddress.State;
                objAgentMaster.remarks = objRecruitmentAgent.InterviewDetailsNotes;
                objAgentMaster.sex = objRecruitmentAgent.Gender;
                objAgentMaster.sliiRegno = objRecruitmentAgent.SLIIRegNo;
                objAgentMaster.authorizedUser = objRecruitmentAgent.InterviewCode;
                objAgentMaster.agentCategory = objRecruitmentAgent.SpecifyProfession;
                objAgentMaster.masterAgent = objRecruitmentAgent.AgentCode;
                objAgentMaster.geographicalClassification = objRecruitmentAgent.Nationality;
                objAgentMaster.status = objRecruitmentAgent.Status;
                objAgentMaster.agentClass = objRecruitmentAgent.Designation;
                objAgentMaster.agentNature = "I";
                objAgentMaster.insuranceType = "LIFE";
                objAgentMaster.shortName = objRecruitmentAgent.FirstName;
                objAgentMaster.natureType = "S";
                objRootObject.agentMaster.Add(objAgentMaster);
            objAgentAddressDet.address1 = objRecruitmentAgent.objCommunicationAddress.Address1;
            objAgentAddressDet.address2 = objRecruitmentAgent.objCommunicationAddress.Address2;            
            objAgentAddressDet.cityCode = objRecruitmentAgent.objCommunicationAddress.CityCode;
            objAgentAddressDet.cityName = objRecruitmentAgent.objCommunicationAddress.City;                       
            objAgentAddressDet.countryName = objRecruitmentAgent.Nationality;          
            objAgentAddressDet.emailAddressLife = objRecruitmentAgent.EmailID;          
            objAgentAddressDet.telephone =Convert.ToString(objRecruitmentAgent.OfficePhNo);
            objAgentAddressDet.telephoneMobile = Convert.ToString(objRecruitmentAgent.MobileNo);
            objAgentAddressDet.telephoneMobileSecond = Convert.ToString(objRecruitmentAgent.AltMobileNo);
            objAgentAddressDet.telephoneResidence = Convert.ToString(objRecruitmentAgent.ResidencePhNo);
            objAgentAddressDet.addressType = "BLADR";
            objAgentAddressDet.serialNo = "1";
           // objRootObject.userId = objRecruitmentAgent.CreatedBy;
            objRootObject.userId = "venkat";
            objRootObject.agentAddressDet.Add(objAgentAddressDet);
            objSettlementMaster.contactInfo = Convert.ToString(objRecruitmentAgent.MobileNo);
            objSettlementMaster.settlementInstitution = objRecruitmentAgent.BankBranchCode;
            objSettlementMaster.settlementAccount = objRecruitmentAgent.AccountNo;
            objSettlementMaster.settlementBranch = objRecruitmentAgent.BranchCode;
            objSettlementMaster.settlementType = objRecruitmentAgent.PaymentMode;
            objRootObject.settlementMaster.Add(objSettlementMaster);
            return objRootObject;
        }
        public static string GetPostParametersToAPI(string statusTypee,string controllerName, string MethodName, object obj)
        {
            try
            {
                string _url = "http://secure.AIA.com:8080/";
                _url = _url + statusTypee+"/jersey/" + controllerName + "/" + MethodName;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
                request.Method = "POST";
                request.ContentType = "application/json";
                string requestData = JsonConvert.SerializeObject(obj);
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] data = encoder.GetBytes(requestData);                
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
    }
}
