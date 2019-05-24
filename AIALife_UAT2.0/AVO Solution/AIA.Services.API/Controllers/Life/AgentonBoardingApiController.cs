using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AIA.Life.Business.AgentonBoarding;
using System.Web.Http;
using AIA.Life.Models.AgentonBoarding;
//using System.Web.Http.Cors;
namespace AIA.Services.API.Controllers
{
    //[EnableCors(origins: "http://localhost:61095/", headers: "*", methods: "*")]
    public class AgentonBoardingApiController : ApiController
    {
        AgentonBoardingBusiness objAgentonBoardingLogic = new AgentonBoardingBusiness();
        public RecruitmentAgent LoadAgentOnBoardingRecruitmentData()
        {
            return objAgentonBoardingLogic.LoadAgentOnBoardingRecruitmentData();
        }
        public RecruitmentAgent GetRecruitementProcessData(RecruitmentAgent objRecruitmentAgent, string TaskName)
        {
            return objAgentonBoardingLogic.GetRecruitementProcessData(objRecruitmentAgent,TaskName);
        }
        public RecruitmentAgent LoadProspectProcessData()
        {
            return objAgentonBoardingLogic.LoadProspectProcessData();
        }
        public RecruitmentAgent LoadHRMClearanceData(RecruitmentAgent objRecruitmentAgent)
        {
            return objAgentonBoardingLogic.LoadHRMClearanceData(objRecruitmentAgent);
        }
        public RecruitmentAgent LoadAuditClearanceProcessData()
        {
            return objAgentonBoardingLogic.LoadAuditClearanceProcessData();
        }
        public RecruitmentAgent LoadRecruitmentPosition()
        {
            return objAgentonBoardingLogic.LoadRecruitmentPosition();
        }
        public RecruitmentAgent GetRecruitmentCareerLevels(string CareerLevel)
        {
            return objAgentonBoardingLogic.GetRecruitmentCareerLevels(CareerLevel);
        }
        public RecruitmentAgent GetRecruitmentDesignations(string Position)
        {
            return objAgentonBoardingLogic.GetRecruitmentDesignations(Position);
        }
        public RecruitmentAgent fetchGenderbasedonsalutation(string Salutation)
        {
            return objAgentonBoardingLogic.fetchGenderbasedonsalutation(Salutation);
        }  
        
        public RecruitmentAgent LoadInterviewTaskData(RecruitmentAgent objRecruitmentAgent)
        {
            return objAgentonBoardingLogic.LoadInterviewTaskData(objRecruitmentAgent);
        }
        public RecruitmentAgent LoadInterviewAreaLevels(RecruitmentAgent objRecruitmentAgent)
        {
            return objAgentonBoardingLogic.LoadInterviewAreaLevels(objRecruitmentAgent);
        }
        public RecruitmentAgent LoadEducationalQualificationDetails()
        {
            return objAgentonBoardingLogic.LoadEducationalQualificationDetails();
        }
        public RecruitmentAgent LoadEducationalSubjectDetails()
        {
            return objAgentonBoardingLogic.LoadEducationalSubjectDetails();
        }
        public RecruitmentAgent LoadEducationalGradeDetails()
        {
            return objAgentonBoardingLogic.LoadEducationalGradeDetails();
        }
        public RecruitmentAgent LoadInterviewLevelResult()
        {
            return objAgentonBoardingLogic.LoadInterviewLevelResult();
        }
        public RecruitmentAgent LoadSearchNICDetailsData(RecruitmentAgent objSearchAdvisor)
        {
            return objAgentonBoardingLogic.LoadSearchNICDetailsData(objSearchAdvisor);
        }
        public RecruitmentAgent LoadFinanceOutstanding()
        {
            return objAgentonBoardingLogic.LoadFinanceOutstanding();
        }
        public RecruitmentAgent LoadAgentSearchDetails()
        {
            return objAgentonBoardingLogic.LoadAgentSearchDetails();
        }
        public RecruitmentAgent SaveProspect(RecruitmentAgent objRecruitmentAgent)
        {
            return objAgentonBoardingLogic.SaveProspect(objRecruitmentAgent);
        }      
        public RecruitmentAgent SaveSuspect(RecruitmentAgent objRecruitmentAgent)
        {
            return objAgentonBoardingLogic.SaveSuspect(objRecruitmentAgent);
        }
        public RecruitmentAgent GetInterviewProspectData(string DataSerializer)
        {
            return objAgentonBoardingLogic.GetInterviewProspectData(DataSerializer);
        }
        public RecruitmentAgent SaveconfirmProspectData(RecruitmentAgent objRecruitmentAgent)
        {
            return objAgentonBoardingLogic.SaveconfirmProspectData(objRecruitmentAgent);
        }
        public RecruitmentAgent LoadSuspectCode()
        {
            return objAgentonBoardingLogic.LoadSuspectCode();
        }
        public RecruitmentAgent GetProspectFromSuspect(string SuspectCode)
        {
            return objAgentonBoardingLogic.GetProspectFromSuspect(SuspectCode);
        }
        public RecruitmentAgent GetProspectfromPendingProspect(string ProspectCode)
        {
            return objAgentonBoardingLogic.GetProspectfromPendingProspect(ProspectCode);
        }        
       public RecruitmentAgent SaveAgentonBoarding(RecruitmentAgent objRecruitmentAgent)
        {
            return objAgentonBoardingLogic.SaveAgentonBoarding(objRecruitmentAgent);
        }
        public RecruitmentAgent SaveDatatoInterviewFromProspect(RecruitmentAgent objRecruitmentAgent)
        {
            return objAgentonBoardingLogic.SaveDatatoInterviewFromProspect(objRecruitmentAgent);
        }
        public RecruitmentAgent SaveDocumentDetails(RecruitmentAgent objRecruitmentAgent)
        {
            return objAgentonBoardingLogic.SaveDocumentDetails(objRecruitmentAgent);
        }
        public RecruitmentAgent ProvinceMaster()
        {
            return objAgentonBoardingLogic.GetProvinceMaster();
        }
        public RecruitmentAgent GetProspectDistricts(string ProvinceCode)
        {
            return objAgentonBoardingLogic.GetProspectDistricts(ProvinceCode);
        }
        public RecruitmentAgent GetBranchCode(string BranchCode)
        {
            return objAgentonBoardingLogic.GetBranchCode(BranchCode);
        }
        public RecruitmentAgent GetBranchAndBankNames(string BranchCode)
        {
            return objAgentonBoardingLogic.GetBranchAndBankNames(BranchCode);
        }
        public RecruitmentAgent GetBranchNames(string BranchName)
        {
            return objAgentonBoardingLogic.GetBranchNames(BranchName);
        }
        public RecruitmentAgent GetBankNamesOnBranchName(string BranchName)
        {
            return objAgentonBoardingLogic.GetBankNamesOnBranchName(BranchName);
        }
        public RecruitmentAgent GetBankNames(string BankName)
        {
            return objAgentonBoardingLogic.GetBankNames(BankName);
        }
        public RecruitmentAgent GetProspectCity(string DistrictCode)
        {
            return objAgentonBoardingLogic.GetProspectCity(DistrictCode);
        }
        public RecruitmentAgent GetPostalCode(string CityCode)
        {
            return objAgentonBoardingLogic.GetPostalCode(CityCode);
        }
        public RecruitmentAgent SubmitAgentRecruitement(RecruitmentAgent objRecruitementSubmit)
        {
            return objAgentonBoardingLogic.SubmitAgentRecruitement(objRecruitementSubmit);
        }
        public RecruitmentAgent GetSubjectonQualification(string EducationalQualification)
        {
            return objAgentonBoardingLogic.GetSubjectonQualification(EducationalQualification);
        }
        public RecruitmentAgent GetGrades()
        {
            return objAgentonBoardingLogic.GetGrades();
        }
        public RecruitmentAgent SaveAuditDepartmentClearance(RecruitmentAgent objRecruitmentAgent)
        {
            return objAgentonBoardingLogic.SaveAuditDepartmentClearance(objRecruitmentAgent);
        }
        public RecruitmentAgent SaveHRMDepartmentClearance(RecruitmentAgent objRecruitmentAgent)
        {
            return objAgentonBoardingLogic.SaveHRMDepartmentClearance(objRecruitmentAgent);
        }
        public RecruitmentAgent SaveFinanceDepartmentClearance(RecruitmentAgent objRecruitmentAgent)
        {
            return objAgentonBoardingLogic.SaveFinanceDepartmentClearance(objRecruitmentAgent);
        }
        public RecruitmentAgent SubmitNICVerification(RecruitmentAgent objRecruitmentAgent)
        {
            return objAgentonBoardingLogic.SubmitNICVerification(objRecruitmentAgent);
        }        
        public RecruitmentAgent GetRecruitmentInterviewResult(string IBSlNo)
        {
            return objAgentonBoardingLogic.GetRecruitmentInterviewResult(IBSlNo);
        }
        public RecruitmentAgent GetGridQualification(string EduQualification)
        {
            return objAgentonBoardingLogic.GetGridQualification(EduQualification);
        }
        public RecruitmentAgent GetReferToUsers(string AgentUser)
        {
            return objAgentonBoardingLogic.GetReferToUsers(AgentUser);
        }
        public RecruitmentAgent FetchRecruitementProcessData(RecruitmentAgent objRecruitmentAgent)
        {
            return objAgentonBoardingLogic.FetchRecruitementProcessData(objRecruitmentAgent);
        }
        public RecruitmentAgent FetchSuspectCode()
        {
            return objAgentonBoardingLogic.FetchSuspectCode();
        }
    }
}