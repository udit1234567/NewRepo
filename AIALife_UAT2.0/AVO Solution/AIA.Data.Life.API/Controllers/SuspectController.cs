using AIA.Data.Life.API.ControllerLogic.Common;
using AIA.Data.Life.API.ControllerLogic.Prospect;
using AIA.Life.Integration.Services.LifeAsiaIntegration;
using AIA.Life.Models.Opportunity;
using AIA.Life.Models.Policy;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;
using AIA.CrossCutting;

namespace AIA.Data.Life.API.Controllers
{
    public class SuspectController : ApiController
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        public AIA.Life.Models.Opportunity.Suspect SaveSuspect(AIA.Life.Models.Opportunity.Suspect objSuspect)
        {
            try
            {
                ProspectLogic objlogic = new ProspectLogic();

                return objlogic.SaveSuspect(objSuspect);
            }
            catch (Exception e)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = objSuspect.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(e);
                objSuspect.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + objSuspect.Error.ErrorCode + ". Sorry for the inconvenience caused";
                return objSuspect;
            }
          

        }
      

        public AIA.Life.Models.Opportunity.Prospect LoadSuspectPoolData(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            try
            {
                ProspectLogic objlogic = new ProspectLogic();
                objProspect.ObjSuspectPool = objlogic.GetSuspectPool(objProspect);
                return objProspect;

            }
            catch (Exception e)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = objProspect.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(e);
                objProspect.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + objProspect.Error.ErrorCode + ". Sorry for the inconvenience caused";
                return objProspect;
            }
            
        }
        public AIA.Life.Models.Opportunity.Prospect LoadAllocateSuspect(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            try
            {
                ProspectLogic objlogic = new ProspectLogic();
                objProspect.ObjSuspectPool = objlogic.GetAllocateSuspect(objProspect);
                return objProspect;

            }
            catch (Exception e)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = objProspect.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(e);
                objProspect.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + objProspect.Error.ErrorCode + ". Sorry for the inconvenience caused";
                return objProspect;
            }

        }
        
        public AIA.Life.Models.Opportunity.Prospect SaveProspect(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            try
            {
                ProspectLogic objlogic = new ProspectLogic();
                //if (string.IsNullOrEmpty(objProspect.ClientCode))
                //{
                //    objProspect = (Prospect)IL.ClientEnquiry(objProspect);
                //}
                objProspect.Error = new AIA.Life.Models.Common.Error();
                //if (string.IsNullOrEmpty(objProspect.Error.ErrorMessage))
                    objProspect = objlogic.SaveProspect(objProspect);
                return objProspect;
            }
            catch (Exception e)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = objProspect.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(e);
                objProspect.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + objProspect.Error.ErrorCode + ". Sorry for the inconvenience caused";
                return objProspect;
            }
            
        }
        public AIA.Life.Models.Opportunity.Prospect LoadProspectPool(AIA.Life.Models.Opportunity.Prospect objPolicyLoadData)
        {
            try
            {
                AIA.Data.Life.API.ControllerLogic.Prospect.ProspectLogic objProspectLogic = new ControllerLogic.Prospect.ProspectLogic();
                objPolicyLoadData.ObjProspectPool = objProspectLogic.GetProspectPool(objPolicyLoadData);
                return objPolicyLoadData;
            }
            catch (Exception e)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = objPolicyLoadData.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(e);
                objPolicyLoadData.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + objPolicyLoadData.Error.ErrorCode + ". Sorry for the inconvenience caused";
                return objPolicyLoadData;
            }

        }
        public AIA.Life.Models.Opportunity.Prospect SaveNeedAnalysis(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            try
            {
                ProspectLogic objlogic = new ProspectLogic();
                //if (string.IsNullOrEmpty(objProspect.ClientCode))
                //{
                //    objProspect = (Prospect)IL.ClientEnquiry(objProspect);
                //}
                objProspect.Error = new AIA.Life.Models.Common.Error();
                //if (string.IsNullOrEmpty(objProspect.Error.ErrorMessage))
                return objlogic.SaveNeedAnalysis(objProspect);
                //else
                    //return objProspect;

            }
            catch (Exception e)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = objProspect.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(e);
                objProspect.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + objProspect.Error.ErrorCode + ". Sorry for the inconvenience caused";
                return objProspect;
            }
           
        }
        [HttpPost]
        public AIA.Life.Models.Opportunity.Prospect DeleteOpportunityInfo(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            ProspectLogic objlogic = new ProspectLogic();
            return objlogic.DeleteOpportunity(objProspect);

        }
        public AIA.Life.Models.Opportunity.Prospect LoadContactInformation(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            try
            {
                ProspectLogic objlogic = new ProspectLogic();
                return objlogic.LoadContactInformation(objProspect);
            }
            catch (Exception e)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = objProspect.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(e);
                objProspect.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + objProspect.Error.ErrorCode + ". Sorry for the inconvenience caused";
                return objProspect;
            }

        }
        public AIA.Life.Models.Opportunity.LifeQuote LoadProspectMaster(AIA.Life.Models.Opportunity.LifeQuote objQuoteList)
        {
            ProspectLogic objlogic = new ProspectLogic();
            return objlogic.LoadProspectMaster(objQuoteList);
        }
        public AIA.Life.Models.Opportunity.LifeQuote LoadQuoteMaster(AIA.Life.Models.Opportunity.LifeQuote objQuoteList)
        {
            ProspectLogic objlogic = new ProspectLogic();
            return objlogic.LoadQuoteMaster(objQuoteList);
        }
        public AIA.Life.Models.Opportunity.QuoteList GetPlanCode(string Variant)
        {
            ProspectLogic objlogic = new ProspectLogic();
            QuoteList objQuoteList = new QuoteList();
            objQuoteList= objlogic.GetPlanCode(Convert.ToInt32(Variant));
            return objQuoteList;
        }
        public AIA.Life.Models.Opportunity.LifeQuote FetchSAM(LifeQuote objLifeQuote)
        {
            ProspectLogic objlogic = new ProspectLogic();             
            int age = objLifeQuote.objProspect.AgeNextBdy ?? 0;
            int variant = Convert.ToInt32(objLifeQuote.objProductDetials.Variant);
            objLifeQuote = objlogic.GetSAM(variant, age);
            return objLifeQuote;
        }
        public AIA.Life.Models.Opportunity.QuoteList GetVariant(string Plan)
        {
            ProspectLogic objlogic = new ProspectLogic();
            QuoteList objQuoteList = new QuoteList();
            objQuoteList = objlogic.GetVariant(Plan);
            return objQuoteList;
        }
        public AIA.Life.Models.Opportunity.QuoteList GetReason(string Decision)
        {
            ProspectLogic objlogic = new ProspectLogic();
            QuoteList objQuoteList = new QuoteList();
            objQuoteList = objlogic.GetReason(Decision);
            return objQuoteList;
        }
        public AIA.Life.Models.Opportunity.Suspect LoadType(AIA.Life.Models.Opportunity.Suspect objSuspect)
        {
            CommonBusiness ObjCommonBusiness = new CommonBusiness();
            return ObjCommonBusiness.LoadType(objSuspect);
        }
        public CampaignLeadType SaveCampaignLead(CampaignLeadType objCampaignLead)
        {
            ProspectLogic objlogic = new ProspectLogic();
            return objlogic.SaveCampaignLead(objCampaignLead);
        }
        
        public AIA.Life.Models.Opportunity.Suspect LoadSalutation(AIA.Life.Models.Opportunity.Suspect objSuspect)
        {
            CommonBusiness ObjCommonBusiness = new CommonBusiness();
            return ObjCommonBusiness.LoadMastersSalutation(objSuspect);
        }
        public AIA.Life.Models.Opportunity.Suspect LoadOccupation(AIA.Life.Models.Opportunity.Suspect objSuspect)
        {
            CommonBusiness ObjCommonBusiness = new CommonBusiness();
            return ObjCommonBusiness.LoadMastersOccupation(objSuspect);
        }
        
        public AIA.Life.Models.Opportunity.Prospect FetchNicDetails(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            //objProspect = (Prospect)IL.ClientEnquiry(objProspect);
            return objProspect;
        }
        //FetchNicverify
        public AIA.Life.Models.Opportunity.Prospect FetchNicverify(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            //objProspect = (Prospect)IL.ClientEnquiry(objProspect);
            ProspectLogic objlogic = new ProspectLogic();
            objlogic.GetNICValidate(objProspect);

            return objProspect;
        }
        public AIA.Life.Models.Opportunity.Prospect FetchNicverifyQuote(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            //objProspect = (Prospect)IL.ClientEnquiry(objProspect);
            ProspectLogic objlogic = new ProspectLogic();
            objlogic.GetNICValidateQuote(objProspect);

            return objProspect;
        }
        //FetchNicverifyPolicyIL
        public AIA.Life.Models.Opportunity.Prospect FetchNicverifyPolicyIL(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            
            ProspectLogic objlogic = new ProspectLogic();
            objlogic.GetNICValidateQuote(objProspect);
            if(!objProspect.NICAVAIL)
                objlogic.GetDetailsFromPolicyCLientIL(objProspect);

            return objProspect;
        }
        public AIA.Life.Models.Opportunity.SuspectPool AllocateLead(AIA.Life.Models.Opportunity.SuspectPool objSuspectPool)
        {
            try
            {
                ProspectLogic objlogic = new ProspectLogic();

                return objlogic.AllocateLead(objSuspectPool);
            }
            catch (Exception e)
            {
                return objSuspectPool;
            }


        }


    }
}