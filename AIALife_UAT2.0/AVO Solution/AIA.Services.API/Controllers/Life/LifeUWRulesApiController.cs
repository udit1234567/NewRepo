using AIA.Life.Business.UWRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AIA.Services.API.Controllers.Life
{
    public class LifeUWRulesApiController : ApiController
    {
        
        public AIA.Life.Models.UWRules.UWRule LoadMasters(AIA.Life.Models.UWRules.UWRule objRule)
        {
            UWRulesBusiness obj = new UWRulesBusiness();
            return obj.LoadMasters(objRule);
        }

        public AIA.Life.Models.UWRules.UWRule SaveRuleInfo(AIA.Life.Models.UWRules.UWRule objRule)
        {
            UWRulesBusiness obj = new UWRulesBusiness();
            return obj.SaveRuleInfo(objRule);
        }
        
        public AIA.Life.Models.UWRules.UWRule SaveSetRuleInfo(AIA.Life.Models.UWRules.UWRule objRule)
        {
            UWRulesBusiness obj = new UWRulesBusiness();
            return obj.SaveSetRuleInfo(objRule);
        }
        public AIA.Life.Models.UWRules.UWRule FetchRuleSetCondition(AIA.Life.Models.UWRules.UWRule objRule)
        {
            UWRulesBusiness obj = new UWRulesBusiness();
            return obj.FetchRuleSetCondition(objRule);
        }

        public AIA.Life.Models.UWRules.UWRule DeleteRuleSet(AIA.Life.Models.UWRules.UWRule objRule)
        {
            UWRulesBusiness obj = new UWRulesBusiness();
            return obj.DeleteRuleSet(objRule);
        }
        public AIA.Life.Models.UWRules.UWRule DeleteRule(AIA.Life.Models.UWRules.UWRule objRule)
        {
            UWRulesBusiness obj = new UWRulesBusiness();
            return obj.DeleteRule(objRule);
        }

      
    }
}
