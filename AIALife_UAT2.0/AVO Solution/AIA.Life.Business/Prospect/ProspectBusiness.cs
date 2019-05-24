
using AIA.Life.Models.Opportunity;
using AIA.Life.Models.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Business.Prospect
{
   public  class ProspectBusiness
    {

        public AIA.Life.Models.Opportunity.Prospect LoadSuspectPoolData(AIA.Life.Models.Opportunity.Prospect objPolicy)
        {


            #region Call API
            objPolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.Prospect>(objPolicy, "LoadSuspectPoolData", "Suspect");
            #endregion
           return objPolicy;
        }

        public AIA.Life.Models.Opportunity.Prospect LoadAllocateSuspect(AIA.Life.Models.Opportunity.Prospect objPolicy)
        {


            #region Call API
            objPolicy = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.Prospect>(objPolicy, "LoadAllocateSuspect", "Suspect");
            #endregion
            return objPolicy;
        }
        

        //Code-Suspect Pool, Quotation Pool, SuspectReAllocation etc 
        public AIA.Life.Models.Policy.Policy LoadSuspectPool(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Models.Policy.Policy objPolicySuspect = new Models.Policy.Policy();
            #region Call API
            objPolicySuspect = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objPolicySuspect, "LoadSuspectPool", "Policy");
            #endregion
            return objPolicySuspect;
        }

        //public Contacts SaveSuspectDetails(Contacts objSuspect)
        //{
        //    //Contacts objSuspectDetails = new Contacts();
        //    //#region Call API
        //    //objSuspectDetails = WebApiLogic.GetPostComplexTypeToAPI<Contacts>(objSuspect, "SaveSuspectDetails", "Suspect");
        //    //#endregion
        //    return objSuspectDetails;
        //}

        public AIA.Life.Models.Opportunity.Prospect  LoadProspectPool(AIA.Life.Models.Opportunity.Prospect objPolicyData)
        {
            #region Call API
            objPolicyData = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.Prospect>(objPolicyData, "LoadProspectPool", "Suspect");
            #endregion
            return objPolicyData;
        }

        public AIA.Life.Models.Policy.Policy LoadProspectList(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Models.Policy.Policy objPolicyProspectList = new Models.Policy.Policy();
            #region Call API
            objPolicyProspectList = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objPolicyProspectList, "LoadProspectList", "Policy");
            #endregion
            return objPolicyProspectList;
        }

        public AIA.Life.Models.Policy.Policy LoadProspectReAllocate(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Models.Policy.Policy objProspectReAllocate = new Models.Policy.Policy();
            #region Call API
            objProspectReAllocate = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objProspectReAllocate, "LoadProspectReAllocate", "Policy");
            #endregion
            return objProspectReAllocate;
        }

        public AIA.Life.Models.Policy.Policy LoadSuspectReAllocation(AIA.Life.Models.Policy.Policy objPolicy)
        {
            AIA.Life.Models.Policy.Policy objPolicySuspectReAllocation = new Models.Policy.Policy();
            #region Call API
            objPolicySuspectReAllocation = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Policy.Policy>(objPolicySuspectReAllocation, "LoadSuspectReAllocation", "Policy");
            #endregion
            return objPolicySuspectReAllocation;
        }

        public AIA.Life.Models.Opportunity.Suspect SaveSuspect(AIA.Life.Models.Opportunity.Suspect objSuspect)
        {
            #region Call API
            objSuspect = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.Suspect>(objSuspect, "SaveSuspect", "Suspect");
            #endregion
            return objSuspect;
        }        
        public AIA.Life.Models.Opportunity.Prospect DeleteOpportunityInfo(AIA.Life.Models.Opportunity.Prospect objSuspect)
        {
            #region Call API
            objSuspect = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.Prospect>(objSuspect, "DeleteOpportunityInfo", "Suspect");
            #endregion
            return objSuspect;
        }

        public AIA.Life.Models.Opportunity.Prospect SaveProspect(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            #region Call API
            objProspect = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.Prospect>(objProspect, "SaveProspect", "Suspect");
            #endregion
            return objProspect;
        }

        public AIA.Life.Models.Opportunity.Prospect SaveNeedAnalysis(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            #region Call API
            objProspect = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.Prospect>(objProspect, "SaveNeedAnalysis", "Suspect");
            #endregion
            return objProspect;
        }
        public AIA.Life.Models.Opportunity.Prospect LoadContactInformation(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            #region Call API
            objProspect = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.Prospect>(objProspect, "LoadContactInformation", "Suspect");
            #endregion
            return objProspect;
        }
        public AIA.Life.Models.Opportunity.LifeQuote LoadProspectMaster(AIA.Life.Models.Opportunity.LifeQuote objQuoteList)
        {
            #region Call API
            objQuoteList = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.LifeQuote>(objQuoteList, "LoadProspectMaster", "Suspect");
            #endregion
            return objQuoteList;
        }
        public AIA.Life.Models.Opportunity.LifeQuote LoadQuoteMaster(AIA.Life.Models.Opportunity.LifeQuote objQuoteList)
        {
            #region Call API
            objQuoteList = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.LifeQuote>(objQuoteList, "LoadQuoteMaster", "Suspect");
            #endregion
            return objQuoteList;
        }
        public AIA.Life.Models.Opportunity.QuoteList GetPlanCode(string Variant)
        {
            #region Call API
            QuoteList objQuoteList = new QuoteList();
            objQuoteList = WebApiLogic.GetPostParametersToAPI<AIA.Life.Models.Opportunity.QuoteList>(objQuoteList, "Suspect", "GetPlanCode", "Variant", Variant);
            #endregion
            return objQuoteList;
        }
        public AIA.Life.Models.Opportunity.LifeQuote GetSAM(LifeQuote objLifeQuote)
        {
            #region Call API
            //QuoteList objQuoteList = new QuoteList();
            //objQuoteList.objProspect.Age = age;
            objLifeQuote = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.LifeQuote>(objLifeQuote, "FetchSAM","Suspect");
            #endregion
            return objLifeQuote;
        }
        public AIA.Life.Models.Opportunity.QuoteList GetVariant(string Plan)
        {
            #region Call API
            QuoteList objQuoteList = new QuoteList();
            objQuoteList = WebApiLogic.GetPostParametersToAPI<AIA.Life.Models.Opportunity.QuoteList>(objQuoteList, "Suspect", "GetVariant", "Plan", Plan);
            #endregion
            return objQuoteList;
        }
        public AIA.Life.Models.Opportunity.QuoteList GetReason(string Decision)
        {
            #region Call API
            QuoteList objQuoteList = new QuoteList();
            objQuoteList = WebApiLogic.GetPostParametersToAPI<AIA.Life.Models.Opportunity.QuoteList>(objQuoteList, "Suspect", "GetReason", "Decision", Decision);
            #endregion
            return objQuoteList;
            
        }
        public AIA.Life.Models.Opportunity.Suspect LoadType(AIA.Life.Models.Opportunity.Suspect objSuspect)
        {
            #region Call API
            objSuspect = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.Suspect>(objSuspect, "LoadType", "Suspect");
            #endregion
            return objSuspect;
        }
        public CampaignLeadType SaveCampaignLead(CampaignLeadType objCampainLeadType)
        {
            #region Call API
            objCampainLeadType = WebApiLogic.GetPostComplexTypeToAPI<CampaignLeadType>(objCampainLeadType, "SaveCampaignLead", "Suspect");
            #endregion
            return objCampainLeadType;
        }
        
        public AIA.Life.Models.Opportunity.Suspect LoadSalutation(AIA.Life.Models.Opportunity.Suspect objSuspect)
        {
            #region Call API
            objSuspect = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.Suspect>(objSuspect, "LoadSalutation", "Suspect");
            #endregion
            return objSuspect;
        }
        public AIA.Life.Models.Opportunity.Suspect LoadOccupation(AIA.Life.Models.Opportunity.Suspect objSuspect)
        {
            #region Call API
            objSuspect = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.Suspect>(objSuspect, "LoadOccupation", "Suspect");
            #endregion
            return objSuspect;
        }
        
        public AIA.Life.Models.Opportunity.Prospect FetchNicDetails(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            #region Call API
            objProspect = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.Prospect>(objProspect, "FetchNicDetails", "Suspect");
            #endregion
            return objProspect;
        }

        public AIA.Life.Models.Opportunity.Prospect FetchNicverify(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            #region Call API
            objProspect = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.Prospect>(objProspect, "FetchNicverify", "Suspect");
            #endregion
            return objProspect;
        }
        public AIA.Life.Models.Opportunity.Prospect FetchNicverifyQuote(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            #region Call API
            objProspect = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.Prospect>(objProspect, "FetchNicverifyQuote", "Suspect");
            #endregion
            return objProspect;
        }
        public AIA.Life.Models.Opportunity.Prospect FetchNicverifyPolicyIL(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            #region Call API
            objProspect = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.Prospect>(objProspect, "FetchNicverifyPolicyIL", "Suspect");
            #endregion
            return objProspect;
        }

        public AIA.Life.Models.Opportunity.SuspectPool AllocateLead(AIA.Life.Models.Opportunity.SuspectPool objSuspectPool)
        {
            #region Call API
            objSuspectPool = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.SuspectPool>(objSuspectPool, "AllocateLead", "Suspect");
            #endregion
            return objSuspectPool;
        }
    }
}
