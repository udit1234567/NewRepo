using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIA.Life.Models.AgentonBoarding;
namespace AIA.Life.Business.AgentonBoarding
{
   public class AgentonBoardingBusiness
    {
        RecruitmentAgent obj = new RecruitmentAgent();
        public RecruitmentAgent LoadAgentOnBoardingRecruitmentData()
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(obj, "LoadAgentOnBoardingRecruitmentData", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent GetRecruitementProcessData(RecruitmentAgent objRecruitmentAgent,string TaskName)
        {
            #region Call API
            obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(objRecruitmentAgent, "AgentonBoarding", "GetRecruitementProcessData", "TaskName", TaskName);
            //obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(obj, "GetRecruitementProcessData", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent LoadProspectProcessData()
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(obj, "LoadProspectProcessData", "AgentonBoarding");
            #endregion
            return obj;
        }      
        public RecruitmentAgent LoadRecruitmentPosition()
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(obj, "LoadRecruitmentPosition", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent GetRecruitmentCareerLevels(string CareerLevel)
        {
            #region Call API
            obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(obj, "AgentonBoarding", "GetRecruitmentCareerLevels", "CareerLevel", CareerLevel);
            #endregion
            return obj;
        }
        public RecruitmentAgent GetRecruitmentDesignations(string Position)
        {
            #region Call API
            obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(obj, "AgentonBoarding", "GetRecruitmentDesignations", "Position", Position);
            #endregion
            return obj;
        }
        public RecruitmentAgent fetchGenderbasedonsalutation(string Salutation)
        {
            #region Call API
            obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(obj, "AgentonBoarding", "fetchGenderbasedonsalutation", "Salutation", Salutation);
            #endregion
            return obj;
        } 
        
        public RecruitmentAgent LoadInterviewTaskData(RecruitmentAgent objRecruitmentAgent)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(objRecruitmentAgent, "LoadInterviewTaskData", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent LoadInterviewAreaLevels(RecruitmentAgent objRecruitmentAgent)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(objRecruitmentAgent, "LoadInterviewAreaLevels", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent LoadEducationalQualificationDetails()
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(obj, "LoadEducationalQualificationDetails", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent LoadEducationalSubjectDetails()
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(obj, "LoadEducationalSubjectDetails", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent LoadEducationalGradeDetails()
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(obj, "LoadEducationalGradeDetails", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent LoadInterviewLevelResult()
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(obj, "LoadInterviewLevelResult", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent LoadSearchNICDetailsData(RecruitmentAgent objSearchAdvisor)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(objSearchAdvisor, "LoadSearchNICDetailsData", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent LoadFinanceOutstanding()
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(obj, "LoadFinanceOutstanding", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent LoadAgentSearchDetails()
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(obj, "LoadAgentSearchDetails", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent SaveProspect(RecruitmentAgent objRecruitmentAgent)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(objRecruitmentAgent, "SaveProspect", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent SaveSuspect(RecruitmentAgent objRecruitmentAgent)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(objRecruitmentAgent, "SaveSuspect", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent GetInterviewProspectData(string DataSerializer)
        {
            #region Call API
            obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(obj, "AgentonBoarding", "GetInterviewProspectData", "DataSerializer", DataSerializer);            
            #endregion
            return obj;
        }
        public RecruitmentAgent SaveconfirmProspectData(RecruitmentAgent objRecruitmentAgent)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(objRecruitmentAgent, "SaveconfirmProspectData", "AgentonBoarding");            
            #endregion
            return obj;
        }
        public RecruitmentAgent LoadSuspectCode()
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(obj, "LoadSuspectCode", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent GetProspectFromSuspect(string SuspectCode)
        {            
            #region Call API
            obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(obj, "AgentonBoarding", "GetProspectFromSuspect", "SuspectCode", SuspectCode);
            #endregion
            return obj;
        }
        public RecruitmentAgent GetProspectfromPendingProspect(string ProspectCode)
        {
            #region Call API
            obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(obj, "AgentonBoarding", "GetProspectfromPendingProspect", "ProspectCode", ProspectCode);
            #endregion
            return obj;
        }        
        public RecruitmentAgent SaveAgentonBoarding(RecruitmentAgent objRecruitmentAgent)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(objRecruitmentAgent, "SaveAgentonBoarding", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent SaveDatatoInterviewFromProspect(RecruitmentAgent objRecruitmentAgent)
        {

            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(objRecruitmentAgent, "SaveDatatoInterviewFromProspect", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent SaveDocumentDetails(RecruitmentAgent objRecruitmentAgent)
        {

            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(objRecruitmentAgent, "SaveDocumentDetails", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent GetProvinceMaster()
        {
            #region Call API
            obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(obj, "Master", "GetProvinceMaster");
            #endregion
            return obj;
        }
        public RecruitmentAgent GetProspectDistricts(string ProvinceCode)
        {
            #region Call API
            obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(obj, "Master", "GetProspectDistricts", "ProvinceCode", ProvinceCode);
            #endregion
            return obj;
        }
        public RecruitmentAgent GetBranchCode(string BranchCode)
        {
            #region Call API
            obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(obj, "AgentonBoarding", "GetBranchCode", "BranchCode", BranchCode);
            #endregion
            return obj;
        }
        public RecruitmentAgent GetBranchAndBankNames(string BranchCode)
        {
            #region Call API
            obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(obj, "AgentonBoarding", "GetBranchAndBankNames", "BranchCode", BranchCode);
            #endregion
            return obj;
        }
        public RecruitmentAgent GetBranchNames(string BranchName)
        {
            #region Call API
            obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(obj, "AgentonBoarding", "GetBranchNames", "BranchName", BranchName);
            #endregion
            return obj;
        }
        public RecruitmentAgent GetBankNamesOnBranchName(string BranchName)
        {
            #region Call API
            obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(obj, "AgentonBoarding", "GetBankNamesOnBranchName", "BranchName", BranchName);
            #endregion
            return obj;
        }
        public RecruitmentAgent GetBankNames(string BankName)
        {
            #region Call API
            obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(obj, "AgentonBoarding", "GetBankNames", "BankName", BankName);
            #endregion
            return obj;
        }
        public RecruitmentAgent GetProspectCity(string DistrictCode)
        {
            #region Call API
            obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(obj, "Master", "GetProspectCity", "DistrictCode", DistrictCode);
            #endregion
            return obj;
        }
        public RecruitmentAgent GetPostalCode(string CityCode)
        {
            #region Call API
            obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(obj, "Master", "GetPostalCode", "CityCode", CityCode);
            #endregion
            return obj;
        }
        public RecruitmentAgent SubmitAgentRecruitement(RecruitmentAgent objRecruitementSubmit)
        {

            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(objRecruitementSubmit, "SubmitAgentRecruitement", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent GetSubjectonQualification(string EducationalQualification)
        {
            #region Call API
            obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(obj, "AgentonBoarding", "GetSubjectonQualification", "EducationalQualification", EducationalQualification);
            #endregion
            return obj;
        }
        public RecruitmentAgent GetGrades()
        {
            #region Call API            
            obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(obj, "AgentonBoarding", "GetGrades", "");
            #endregion
            return obj;
        }
        public RecruitmentAgent SaveAuditDepartmentClearance(RecruitmentAgent objRecruitmentAgent)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(objRecruitmentAgent, "SaveAuditDepartmentClearance", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent SaveHRMDepartmentClearance(RecruitmentAgent objRecruitmentAgent)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(objRecruitmentAgent, "SaveHRMDepartmentClearance", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent SaveFinanceDepartmentClearance(RecruitmentAgent objRecruitmentAgent)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(objRecruitmentAgent, "SaveFinanceDepartmentClearance", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent LoadHRMClearanceData(RecruitmentAgent objRecruitmentAgent)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(objRecruitmentAgent, "LoadHRMClearanceData", "AgentonBoarding");
            //obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(objRecruitmentAgent, "AgentonBoarding", "LoadHRMClearanceData", "TaskName", TaskName);
            #endregion
            return obj;
        }
        public RecruitmentAgent LoadAuditClearanceProcessData()
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(obj, "LoadAuditClearanceProcessData", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent SubmitNICVerification(RecruitmentAgent objRecruitmentAgent)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(objRecruitmentAgent, "SubmitNICVerification", "AgentonBoarding");            
            #endregion
            return obj;
        }
        public RecruitmentAgent GetRecruitmentInterviewResult(string IBSlNo)
        {
            #region Call API            
            obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(obj, "AgentonBoarding", "GetRecruitmentInterviewResult", "IBSlNo", IBSlNo);
            #endregion
            return obj;
        }
        public RecruitmentAgent GetGridQualification(string EduQualification)
        {
            #region Call API
            obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(obj, "AgentonBoarding", "GetGridQualification", "EduQualification", EduQualification);
            #endregion
            return obj;
        }
        public RecruitmentAgent GetReferToUsers(string AgentUser)
        {
            #region Call API
            obj = WebApiLogic.GetPostParametersToAPI<RecruitmentAgent>(obj, "AgentonBoarding", "GetReferToUsers", "AgentUser", AgentUser);
            #endregion
            return obj;
        }
        public RecruitmentAgent FetchRecruitementProcessData(RecruitmentAgent objRecruitmentAgent)
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(objRecruitmentAgent, "FetchRecruitementProcessData", "AgentonBoarding");
            #endregion
            return obj;
        }
        public RecruitmentAgent FetchSuspectCode()
        {
            #region Call API
            obj = WebApiLogic.GetPostComplexTypeToAPI<RecruitmentAgent>(obj, "FetchSuspectCode", "AgentonBoarding");
            #endregion
            return obj;
        }
        
    }
}
