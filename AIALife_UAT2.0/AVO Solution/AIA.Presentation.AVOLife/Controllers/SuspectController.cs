using Grid.Mvc.Ajax.GridExtensions;
using AIA.Life.Models.Opportunity;
using AIA.Life.Models.Policy;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using AIA.Life.Models.NeedAnalysis;
using AIA.Presentation.AVOLife.ExceptionHandling;
using AIA.Life.Models.Common;

namespace AIA.Presentation.AVOLife.Controllers
{
    [Authorize]
    [ErrorLogging]
    [SessionTimeout]
    public class SuspectController : BaseController
    {
        private string _username = string.Empty;
        AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
        public SuspectController()
        {
            _username = System.Web.HttpContext.Current.User.Identity.Name;
        }
        // GET: Suspect
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Suspect()
        {
            Prospect obj = new Prospect();
            TempData["Load"] = "FirstTime";
            obj.CreatedBy = _username;
            obj = objProspectBusiness.LoadSuspectPoolData(obj);
            ViewBag.SuspectPoolCount = obj.ObjSuspectPool.Count;
            return View();
        }

        public ActionResult NewSuspect()
        {
            Suspect ObjSuspect = new Suspect();
            ObjSuspect.CreatedBy = _username;
            ObjSuspect = objProspectBusiness.LoadType(ObjSuspect);

            return View(ObjSuspect);
        }
        public ActionResult Allocate()
        {
            try
            {
                Prospect obj = new Prospect();
                TempData["Load"] = "FirstTime";
                obj.CreatedBy = _username;
                obj = objProspectBusiness.LoadSuspectPoolData(obj);
                return View(obj);
            }
            catch (Exception e)
            {
                return View("Error");
            }

        }

        public ActionResult ModifySuspect()
        {
            return View();
        }
        public ActionResult SuspectPool()
        {
            try
            {
                Prospect obj = new Prospect();
                TempData["Load"] = "FirstTime";
                obj.CreatedBy = _username;
                obj = objProspectBusiness.LoadSuspectPoolData(obj);

                //AjaxGrid<SuspectPool> ajaxgrid = null;
                //ajaxgrid = (AjaxGrid<SuspectPool>)new AjaxGridFactory().CreateAjaxGrid((obj.ObjSuspectPool).AsQueryable(), 1, false);
                //ViewBag.Data = 0;
                //return PartialView("PartialSuspectPoolGrid", ajaxgrid);
                return View(obj);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        public ActionResult PartialSuspectPoolGrid()
        {
            try
            {
                Prospect objProspect = new Prospect();
                objProspect.CreatedBy = _username;
                objProspect = objProspectBusiness.LoadSuspectPoolData(objProspect);

                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                var grid = (AjaxGrid<SuspectPool>)new AjaxGridFactory().CreateAjaxGrid((objProspect.ObjSuspectPool).AsQueryable(), 1, false, 2);

                return PartialView(grid);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }
        public ActionResult PartialAllocateSuspectGrid()
        {
            try
            {
                Prospect objProspect = new Prospect();
                objProspect.CreatedBy = _username;
                objProspect = objProspectBusiness.LoadAllocateSuspect(objProspect);

                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                var grid = (AjaxGrid<SuspectPool>)new AjaxGridFactory().CreateAjaxGrid((objProspect.ObjSuspectPool).AsQueryable(), 1, false, 2);

                return PartialView(grid);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }
        public ActionResult SearchSuspectAllocateDetails(int? page)
        {
            try
            {
                Prospect objProspect = new Prospect();
                objProspect.CreatedBy = _username;
                objProspect = objProspectBusiness.LoadAllocateSuspect(objProspect);

                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                var grid = aj.CreateAjaxGrid(objProspect.ObjSuspectPool.AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, 2);

                return Json(new
                {
                    Html = RenderPartialViewToString("~/Views/Suspect/PartialAllocateSuspectGrid.cshtml", grid),
                    HasItems = grid.HasItems
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }
        //Duplicate of SuspectPool
        public ActionResult SearchSuspectPoolDetails(int? page)
        {
            try
            {
                Prospect objProspect = new Prospect();
                objProspect.CreatedBy = _username;
                objProspect = objProspectBusiness.LoadSuspectPoolData(objProspect);

                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                var grid = aj.CreateAjaxGrid(objProspect.ObjSuspectPool.AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, 2);

                return Json(new
                {
                    Html = RenderPartialViewToString("~/Views/Suspect/PartialSuspectPoolGrid.cshtml", grid),
                    HasItems = grid.HasItems
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        public string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        public ActionResult SearchSuspectReAllocationDetails()
        {
            try
            {
                Policy obj = new Policy();
                obj = objProspectBusiness.LoadSuspectReAllocation(obj);
                return PartialView("~/Views/Suspect/PartialSuspectReAllocationGrid.cshtml", obj);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        public ActionResult SaveSuspect(AIA.Life.Models.Opportunity.Suspect objSuspect)
        {
            try
            {
                objSuspect.CreatedBy = _username;
                objSuspect = objProspectBusiness.SaveSuspect(objSuspect);
                var ObjResponse = new { Message = objSuspect.Message, ContactID = objSuspect.ContactID, Error = objSuspect.Error.ErrorMessage };
                return Json(ObjResponse, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(objSuspect.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult LoadSuspectInformation(string ContactID)

        {
            try
            {
                AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
                objProspect.ContactID = Convert.ToInt32(CrossCutting.CrossCutting_EncryptDecrypt.Decrypt(ContactID.ToString()));
                objProspect.CreatedBy = _username;
                #region Check user authorization
                AuthorizeUser authorizeUser = new AuthorizeUser();
                authorizeUser.UserName = _username;
                authorizeUser.ContactId = objProspect.ContactID ;
                authorizeUser = new AIA.Life.Business.Common.CommonBusiness().CheckAuthorisation(authorizeUser);
                if (!string.IsNullOrEmpty(authorizeUser.Error.ErrorMessage))
                {
                    return RedirectToAction("AuthorizeError", "Error", new { errorMessage = authorizeUser.Error.ErrorMessage });
                }
                #endregion
                objProspect = objProspectBusiness.LoadContactInformation(objProspect);
                foreach (PreviousInsuranceList item in objProspect.objPreviousInsuranceList)
                {
                    PrevPolicy obj = new PrevPolicy();
                    obj.PolicyNo = item.PolicyNumber;
                    obj.MaturityFund = 0;
                    objProspect.objNeedAnalysis.objPrevPolicy.Add(obj);
                }

                ViewBag.Type = "Suspect";
                //objProspect.objNeedAnalysis.dlladversities.Add(new SelectListItem { Text = "Critical Illnesses" });
                //objProspect.objNeedAnalysis.dlladversities.Add(new SelectListItem { Text = "Major Surgeries" });
                //objProspect.objNeedAnalysis.dlladversities.Add(new SelectListItem { Text = "Loss of Income" });
                //objProspect.objNeedAnalysis.dlladversities.Add(new SelectListItem { Text = "Pre and Post Hospitalization Expenses" });
                //objProspect.objNeedAnalysis.dlladversities.Add(new SelectListItem { Text = "Excess Payments/Taxes" });

                //objProspect.objNeedAnalysis.dllannualamount.Add(new SelectListItem { Text = "Below LKR 100,000" });
                //objProspect.objNeedAnalysis.dllannualamount.Add(new SelectListItem { Text = "LKR 100,000 - 200,000" });
                //objProspect.objNeedAnalysis.dllannualamount.Add(new SelectListItem { Text = "LKR 200,000 - 300,000" });
                //objProspect.objNeedAnalysis.dllannualamount.Add(new SelectListItem { Text = "LKR 300,000 - 400,000" });
                //objProspect.objNeedAnalysis.dllannualamount.Add(new SelectListItem { Text = "LKR 400,000 - 500,000" });
                //objProspect.objNeedAnalysis.dllannualamount.Add(new SelectListItem { Text = "Above LKR 500,000" });

                //objProspect.objNeedAnalysis.dllcoverage.Add(new SelectListItem { Text = "Local" });
                //objProspect.objNeedAnalysis.dllcoverage.Add(new SelectListItem { Text = "Global" });

                //objProspect.objNeedAnalysis.dlladequacy.Add(new SelectListItem { Text = "Yes" });
                //objProspect.objNeedAnalysis.dlladequacy.Add(new SelectListItem { Text = "No" });
                if (!string.IsNullOrEmpty(objProspect.NIC))
                {
                    objProspect.NICAVAIL = false;
                    objProspect = objProspectBusiness.FetchNicverify(objProspect);
                    if (objProspect == null)
                    {
                        objProspect = objProspectBusiness.FetchNicverifyPolicyIL(objProspect);
                    }
                }
                if (objProspect.objNeedAnalysis.ProspectSign != null)
                {
                    objProspect.Signature = Convert.ToString(objProspect.objNeedAnalysis.ProspectSign);

                }
                objProspect.objNeedAnalysis.Stage = "Lead";
                for (int i = 0; i < objProspect.objNeedAnalysis.objFinancialNeeds.Count; i++)
                {
                    if (objProspect.objNeedAnalysis.objFinancialNeeds[i].Name.ToUpper() == "HIGHER EDUCATION")
                    {
                        objProspect.objNeedAnalysis.objFinancialNeeds[i].ImagePath = "/Content/Images/FnaIcons/Education@0,25x.png";
                    }
                    else if (objProspect.objNeedAnalysis.objFinancialNeeds[i].Name.ToUpper() == "WEDDING")
                    {
                        objProspect.objNeedAnalysis.objFinancialNeeds[i].ImagePath = "/Content/Images/FnaIcons/Children Wedding@0,25x.png";
                    }
                    else if (objProspect.objNeedAnalysis.objFinancialNeeds[i].Name.ToUpper() == "PENSION FUND")
                    {
                        objProspect.objNeedAnalysis.objFinancialNeeds[i].ImagePath = "/Content/Images/FnaIcons/Gratutity@0,25x.png";
                    }
                    else if (objProspect.objNeedAnalysis.objFinancialNeeds[i].Name.ToUpper() == "BUY CAR/PROPERTY")
                    {
                        objProspect.objNeedAnalysis.objFinancialNeeds[i].ImagePath = "/Content/Images/FnaIcons/Buy car_property@0,25x.png";
                    }
                    else if (objProspect.objNeedAnalysis.objFinancialNeeds[i].Name.ToUpper() == "OTHER")
                    {
                        objProspect.objNeedAnalysis.objFinancialNeeds[i].ImagePath = "/Content/Images/FnaIcons/Other@0,25x.png";
                    }
                }
                return View("~/Views/Prospect/CreateProspect.cshtml", objProspect);
            }

            catch (Exception e)
            {
                return View();
            }
        }

        public ActionResult DeleteOpportunityInfo(int ContactId)
        {
            AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            objProspect.ContactID = ContactId;
            objProspect = objProspectBusiness.DeleteOpportunityInfo(objProspect);
            var ObjResponse = new { Message = objProspect.Message };
            return Json(ObjResponse, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNicDetails(string NIC)
        {
            AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            objProspect.NIC = NIC;
            objProspect = objProspectBusiness.FetchNicDetails(objProspect);
            return Json(objProspect, JsonRequestBehavior.AllowGet);
        }
        public JsonResult verifyBMI(string Format, string Height, string weight)
        {
            AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            decimal _Height = 0;
            decimal limit = 0;
            if (Format != null)
            {
                if (Format == "2411")//Ft
                {
                    _Height = (Convert.ToDecimal(Height) * (30.48m));
                }
                else if (Format == "2412")//Inches
                {
                    _Height = (Convert.ToDecimal(Height) * (2.54m));

                }
                else if (Format == "2413")// CMS
                {
                    _Height = Convert.ToDecimal(Height);
                }
                decimal Height_new = _Height / 100;
                decimal Weight = Convert.ToDecimal(weight);
                string bmi = Convert.ToString(Weight / (Height_new * Height_new));
                limit = Weight / (Height_new * Height_new);
            }
            if (limit < 17 || limit > 32)
            {
                objProspect.BMI_Exceed = true;
            }
            else { objProspect.BMI_Exceed = false; }

            // objProspect = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.Prospect>(objProspect, "FetchNicDetails", "SuspectApi");
            return Json(objProspect, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNicValues(string NIC)
        {
            AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            objProspect.NIC = NIC;
            objProspect.NICAVAIL = false;
            objProspect = objProspectBusiness.FetchNicverify(objProspect);
            return Json(objProspect, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNicValuesQuote(string NIC)
        {
            AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            objProspect.NIC = NIC;
            objProspect.NICAVAIL = false;
            objProspect = objProspectBusiness.FetchNicverifyQuote(objProspect);
            return Json(objProspect, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOnFail_ContactValues(string NIC)
        {
            AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            objProspect.NIC = NIC;

            //objProspect.NICAVAIL = false;
            objProspect = objProspectBusiness.FetchNicverifyPolicyIL(objProspect);
            return Json(objProspect, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSalutation(string prefix)
        {
            AIA.Life.Models.Opportunity.Suspect objSuspect = new Suspect();
            objSuspect.Prefix = prefix;
            objSuspect = objProspectBusiness.LoadSalutation(objSuspect);
            List<string> LstSalutation = objSuspect.LstSalutation.Where(a => a.Text.ToUpper().StartsWith(prefix.ToUpper())).Select(a => a.Text).Distinct().ToList();
            return Json(LstSalutation, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetOccupation(string prefix)
        {
            AIA.Life.Models.Opportunity.Suspect objSuspect = new Suspect();
            objSuspect.Prefix = prefix;
            objSuspect = objProspectBusiness.LoadOccupation(objSuspect); ;
            List<string> LstOccupation = objSuspect.LstOccupation.Where(a => a.Text.ToUpper().Contains(prefix.ToUpper())).OrderByDescending(a => a.Text.ToUpper().StartsWith(prefix.ToUpper())).Select(a => a.Text).Distinct().ToList();
            return Json(objSuspect.LstOccupation, JsonRequestBehavior.AllowGet);

        }
        public ActionResult AllocateLead(string ContactId, string AssignedTo)
        {
            AIA.Life.Models.Opportunity.SuspectPool objSuspectPool = new SuspectPool();
            objSuspectPool.ContactId = Convert.ToInt32(CrossCutting.CrossCutting_EncryptDecrypt.Decrypt(ContactId));
            objSuspectPool.AssignedTo = AssignedTo;
            objSuspectPool.UserName = _username;
            objSuspectPool = objProspectBusiness.AllocateLead(objSuspectPool);
            return Json(objSuspectPool, JsonRequestBehavior.AllowGet);
        }

    }
}