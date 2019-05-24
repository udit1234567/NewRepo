using AIA.Life.Business.UWRules;
using AIA.Life.Models.UWRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace AIA.Presentation.AVOLife.Controllers
{
    [Authorize]
    [ErrorLogging]
    public class RuleController : BaseController
    {
        UWRulesBusiness objUWRulesBusiness = new UWRulesBusiness();
        // GET: Rule
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ConfigureRules()
        {
            UWRule objRule = new UWRule();
            objRule = objUWRulesBusiness.LoadMasters(objRule);
            return View(objRule);
        }
        public ActionResult SaveRuleInfo(UWRule objRule)
        {
           
            objRule = objUWRulesBusiness.SaveRuleInfo(objRule);
            var ObjResponse = new { Message = objRule.Message};
            return Json(ObjResponse, JsonRequestBehavior.AllowGet);
            
        }
        
        public ActionResult SetRule()
        {
            UWRule objRule = new UWRule();
            objRule.RuleSetType = "Rules";
            objRule = objUWRulesBusiness.LoadMasters(objRule);
            var ObjResponse = new { Message = objRule.Message };
            return View(objRule);
        }

        public ActionResult FetchRuleParameters(int RuleID)
        {
            UWRule objRule = new UWRule();
            objRule.RuleID = RuleID;
            objRule.RuleSetType = "RuleParameters";
            objRule = objUWRulesBusiness.LoadMasters(objRule);
            objRule.RuleID = RuleID;
            return PartialView("~/Views/Rule/PartialRuleParameters.cshtml", objRule);
        }

        public ActionResult SaveSetRuleInfo(UWRule objRule)
        {

            objRule = objUWRulesBusiness.SaveSetRuleInfo(objRule);
            var ObjResponse = new { Message = objRule.Message };
            return Json(objRule, JsonRequestBehavior.AllowGet);

        }

        public ActionResult LoadRuleInfo(int RuleID)
        {
            UWRule objRule = new UWRule();
            objRule.RuleID = RuleID;
            objRule.RuleSetType = "RuleInfo";
            objRule.IsEditMode = true;
            objRule = objUWRulesBusiness.LoadMasters(objRule);
            objRule.RuleID = RuleID;
            return View("~/Views/Rule/ConfigureRules.cshtml", objRule);
        }

        public ActionResult FetchRuleSetCondition(int RuleSetID)
        {
            UWRule objRule = new UWRule();
            objRule.RuleSetID = RuleSetID;
            objRule = objUWRulesBusiness.FetchRuleSetCondition(objRule);
            var ObjResponse = new { Message = objRule.Message, Description = objRule.Description };
            return Json(objRule, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DeleteRuleSet(int RuleSetID)
        {
            UWRule objRule = new UWRule();
            objRule.RuleSetID = RuleSetID;
            objRule = objUWRulesBusiness.DeleteRuleSet(objRule);
            var ObjResponse = new { Message = objRule.Message };
            return Json(objRule, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DeleteRule(int RuleID)
        {
            UWRule objRule = new UWRule();
            objRule.RuleID = RuleID;
            objRule = objUWRulesBusiness.DeleteRule(objRule);
            var ObjResponse = new { Message = objRule.Message };
            return Json(objRule, JsonRequestBehavior.AllowGet);

        }
       


    }
}