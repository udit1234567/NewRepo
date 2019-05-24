using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Business.UWRules
{
    public class UWRulesBusiness
    {

        public AIA.Life.Models.UWRules.UWRule LoadMasters(AIA.Life.Models.UWRules.UWRule objRule)
        {

            #region Call API
            objRule = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.UWRules.UWRule>(objRule, "LoadMasters", "UWFlow");
            #endregion
            return objRule;
        }
        public AIA.Life.Models.UWRules.UWRule SaveRuleInfo(AIA.Life.Models.UWRules.UWRule objRule)
        {

            #region Call API
            objRule = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.UWRules.UWRule>(objRule, "SaveRuleInfo", "UWFlow");
            #endregion
            return objRule;
        }

        public AIA.Life.Models.UWRules.UWRule SaveSetRuleInfo(AIA.Life.Models.UWRules.UWRule objRule)
        {

            #region Call API
            objRule = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.UWRules.UWRule>(objRule, "SaveSetRuleInfo", "UWFlow");
            #endregion
            return objRule;
        }
        public AIA.Life.Models.UWRules.UWRule FetchRuleSetCondition(AIA.Life.Models.UWRules.UWRule objRule)
        {

            #region Call API
            objRule = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.UWRules.UWRule>(objRule, "FetchRuleSetCondition", "UWFlow");
            #endregion
            return objRule;
        }


        public AIA.Life.Models.UWRules.UWRule DeleteRuleSet(AIA.Life.Models.UWRules.UWRule objRule)
        {

            #region Call API
            objRule = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.UWRules.UWRule>(objRule, "DeleteRuleSet", "UWFlow");
            #endregion
            return objRule;
        }
        public AIA.Life.Models.UWRules.UWRule DeleteRule(AIA.Life.Models.UWRules.UWRule objRule)
        {

            #region Call API
            objRule = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.UWRules.UWRule>(objRule, "DeleteRule", "UWFlow");
            #endregion
            return objRule;
        }

       
    }
}
