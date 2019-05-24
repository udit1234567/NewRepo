
using AIA.Life.Models.Opportunity;
using AIA.Life.Models.Policy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AIA.Services.API.Controllers.Life
{
    public class SuspectApiController : ApiController
    {
        public AIA.Life.Models.Opportunity.Prospect LoadSuspectPoolData(AIA.Life.Models.Opportunity.Prospect objPolicyData)
        {
            
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objPolicyData = objProspectBusiness.LoadSuspectPoolData(objPolicyData);
            return objPolicyData;
        }
        public AIA.Life.Models.Opportunity.Prospect LoadAllocateSuspect(AIA.Life.Models.Opportunity.Prospect objPolicyData)
        {

            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objPolicyData = objProspectBusiness.LoadAllocateSuspect(objPolicyData);
            return objPolicyData;
        }

        public AIA.Life.Models.Policy.Policy LoadSuspectReAllocationData(Policy objPolicyData)
        {
          
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objPolicyData = objProspectBusiness.LoadSuspectReAllocation(objPolicyData);
            return objPolicyData;
        }

        public AIA.Life.Models.Opportunity.Prospect LoadProspectPoolData(AIA.Life.Models.Opportunity.Prospect objPolicyData)
        {
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objPolicyData = objProspectBusiness.LoadProspectPool(objPolicyData);
            return objPolicyData;
        }

        public AIA.Life.Models.Policy.Policy LoadProspectListData(Policy objPolicyData)
        {
           
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objPolicyData = objProspectBusiness.LoadProspectList(objPolicyData);
            return objPolicyData;
        }

        public AIA.Life.Models.Policy.Policy LoadProspectReAllocateData(Policy objPolicyData)
        {
          
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objPolicyData = objProspectBusiness.LoadProspectReAllocate(objPolicyData);
            return objPolicyData;
        }
       
        public AIA.Life.Models.Opportunity.Suspect  SaveSuspect(AIA.Life.Models.Opportunity.Suspect objSuspect)
        {
          // string ObjSuspect= Newtonsoft.Json.JsonConvert.SerializeObject(objSuspect);
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objSuspect = objProspectBusiness.SaveSuspect(objSuspect);
            return objSuspect;
        }     
        
        [HttpPost]
        public AIA.Life.Models.Opportunity.Prospect DeleteOpportunityInfo(AIA.Life.Models.Opportunity.Prospect objSuspect)
        {
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objSuspect = objProspectBusiness.DeleteOpportunityInfo(objSuspect);
            return objSuspect;
        }

        public AIA.Life.Models.Opportunity.Prospect SaveProspect(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objProspect = objProspectBusiness.SaveProspect(objProspect);
            return objProspect;
        }

        public AIA.Life.Models.Opportunity.Prospect SaveNeedAnalysis(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objProspect = objProspectBusiness.SaveNeedAnalysis(objProspect);
            return objProspect;
        }
        public AIA.Life.Models.Opportunity.Prospect LoadContactInformation(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objProspect = objProspectBusiness.LoadContactInformation(objProspect);
            return objProspect;
        }
        public AIA.Life.Models.Opportunity.LifeQuote LoadProspectMaster(AIA.Life.Models.Opportunity.LifeQuote objQuoteList)
        {
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objQuoteList = objProspectBusiness.LoadProspectMaster(objQuoteList);
            return objQuoteList;
        }
        public AIA.Life.Models.Opportunity.LifeQuote LoadQuoteMaster(AIA.Life.Models.Opportunity.LifeQuote objQuoteList)
        {
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objQuoteList = objProspectBusiness.LoadQuoteMaster(objQuoteList);
            return objQuoteList;
        }
        public AIA.Life.Models.Opportunity.QuoteList GetPlanCode(string Variant)
        {
            QuoteList objQuoteList = new QuoteList();
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objQuoteList = objProspectBusiness.GetPlanCode(Variant);
            return objQuoteList;
        }
        public AIA.Life.Models.Opportunity.LifeQuote FetchSAM(LifeQuote objLifeQuote)
        {
            //objQuoteList = new QuoteList();
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objLifeQuote = objProspectBusiness.GetSAM(objLifeQuote);
            return objLifeQuote;
        }
        public AIA.Life.Models.Opportunity.QuoteList GetReason(string Decision)
        {
            QuoteList objQuoteList = new QuoteList();
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objQuoteList = objProspectBusiness.GetReason(Decision);
            return objQuoteList;
        }
        public AIA.Life.Models.Opportunity.QuoteList GetVariant(string Plan)
        {
            QuoteList objQuoteList = new QuoteList();
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objQuoteList = objProspectBusiness.GetVariant(Plan);         
            return objQuoteList;
        }
        public AIA.Life.Models.Opportunity.Prospect FetchNicDetails(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objProspect = objProspectBusiness.FetchNicDetails(objProspect);
            return objProspect;
        }
        public AIA.Life.Models.Opportunity.Prospect FetchNicverify(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objProspect = objProspectBusiness.FetchNicverify(objProspect);
            return objProspect;
        }
        public AIA.Life.Models.Opportunity.Prospect FetchNicverifyQuote(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objProspect = objProspectBusiness.FetchNicverifyQuote(objProspect);
            return objProspect;
        }
        //FetchNicverifyPolicyIL
        public AIA.Life.Models.Opportunity.Prospect FetchNicverifyPolicyIL(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objProspect = objProspectBusiness.FetchNicverifyPolicyIL(objProspect);
            return objProspect;
        }
        public AIA.Life.Models.Opportunity.Prospect FetchNicForModifySuspect(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objProspect = objProspectBusiness.FetchNicverify(objProspect);
            if(objProspect ==null)
            {
                objProspect = objProspectBusiness.FetchNicverifyPolicyIL(objProspect);
            }            
            return objProspect;
        }
        public AIA.Life.Models.Opportunity.Suspect LoadType(AIA.Life.Models.Opportunity.Suspect objSuspect)
        {
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objSuspect = objProspectBusiness.LoadType(objSuspect);
            return objSuspect;
        }
        public AIA.Life.Models.Opportunity.Suspect LoadSalutation(AIA.Life.Models.Opportunity.Suspect objSuspect)
        {
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objSuspect = objProspectBusiness.LoadSalutation(objSuspect);
            return objSuspect;
        }
        public AIA.Life.Models.Opportunity.Suspect LoadOccupation(AIA.Life.Models.Opportunity.Suspect objSuspect)
        {
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objSuspect = objProspectBusiness.LoadOccupation(objSuspect);
            return objSuspect;
        }
        public AIA.Life.Models.Opportunity.SuspectPool AllocateLead(AIA.Life.Models.Opportunity.SuspectPool objSuspectPool)
        {
           
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objSuspectPool = objProspectBusiness.AllocateLead(objSuspectPool);
            return objSuspectPool;
        }
        public CampaignLeadType SaveCampaignLead(CampaignLeadType objCampainLeadType)
        {
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();

            objCampainLeadType = objProspectBusiness.SaveCampaignLead(objCampainLeadType);
            return objCampainLeadType;
        }
    }
}