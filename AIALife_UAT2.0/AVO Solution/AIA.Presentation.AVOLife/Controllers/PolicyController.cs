using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AIA.Life.Models;
using AIA.Life.Models.Policy;
using Grid.Mvc.Ajax.GridExtensions;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.IO;
using AIA.Presentation.AVOLife.Helpers;
using System.Drawing.Imaging;
using System.Drawing;
using AIA.Presentation.Helpers;
using Newtonsoft.Json;
using AIA.Life.Models.Opportunity;
using AIA.Life.Models.UWDecision;
using AIA.Life.Models.Common;
using AIA.CrossCutting;
using log4net;
using Newtonsoft.Json.Linq;
using System.Threading;
using AIA.Life.Models.NeedAnalysis;
using System.Configuration;
using iTextSharp.text;
using iTextSharp.text.pdf;
using AIA.Life.Models.Payment;
using AIA.Life.Repository.AIAEntity;
using AIA.Life.Business.Payment;
using AIA.Presentation.AVOLife.ExceptionHandling;
using AIA.Life.Business.Common;
using System.Globalization;


//using AIA.Life.Business.Common;

namespace AIA.Presentation.AVOLife.Controllers
{
    [Authorize]
    [ErrorLogging]
    [SessionTimeout]
    public class PolicyController : BaseController
    {
        private string Username = string.Empty;
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        public PolicyController()
        {
            Username = System.Web.HttpContext.Current.User.Identity.Name;
        }
        AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
        public ActionResult UWProposal(string ProposalNo)
        {
            AIA.Life.Models.Policy.Policy objpolicy = new AIA.Life.Models.Policy.Policy();
            objpolicy.ProposalNo = ProposalNo;
            objpolicy.QuoteNo = ProposalNo;
            objpolicy = objPolicyBusiness.FetchProposalInfo(objpolicy);
            return View(objpolicy);
        }

        public ActionResult LoadPolicyPreviousInsuranceGrid(AIA.Life.Models.Policy.Policy objPolicyList)
        {
            AIA.Life.Models.Policy.Policy objPolicy = new Policy();
            objPolicy.objProspectDetails = objPolicyList.objProspectDetails;
            objPolicy = objPolicyBusiness.LoadPolicyPreviousInsuranceGrid(objPolicyList);

            return PartialView("~/Views/Policy/_PartialLifePrevoiusInsuranceDetailsGrid.cshtml", objPolicy);
        }

        public ActionResult UWInbox()
        {
            AIA.Life.Models.Policy.UWInbox objUWInbox = new Life.Models.Policy.UWInbox();
            objUWInbox.UserName = Username;
            objUWInbox = objPolicyBusiness.FetchUWProposals(objUWInbox);
            return View(objUWInbox);
        }

        public JsonResult GetAutoRelationShip(AIA.Life.Models.Policy.Policy objPolicy, string prefix)
        {
            objPolicy.Prefix = prefix;
            objPolicy = objPolicyBusiness.LoadMastersRelationship(objPolicy);
            List<string> LstRelationShip = objPolicy.lstRelations.Where(a => a.Text.ToUpper().StartsWith(prefix.ToUpper())).Select(a => a.Text).Distinct().ToList();
            return Json(LstRelationShip, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProposalPendingRequirements()
        {
            AIA.Life.Models.Policy.UWInbox objUWInbox = new Life.Models.Policy.UWInbox();
            objUWInbox.UserName = Username;
            objUWInbox.Message = "Pending";
            TempData["Load"] = "FirstTime";
            objUWInbox = objPolicyBusiness.FetchUWProposals(objUWInbox);
            return View(objUWInbox);
        }

        public ActionResult ProposalPendingRequirementsNextPage(int? Page)
        {
            try
            {
                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                AIA.Life.Models.Policy.UWInbox objUWInbox = new Life.Models.Policy.UWInbox();
                objUWInbox.UserName = Username;
                objUWInbox.Message = "Pending";
                objUWInbox = objPolicyBusiness.FetchUWProposals(objUWInbox);
                var grid = aj.CreateAjaxGrid(objUWInbox.LstProposals.AsQueryable(), Page.HasValue ? Page.Value : 1, Page.HasValue, 2);
                return Json(new
                {
                    Html = RenderPartialViewToString("~/Views/Allocation/_PartialAllocateGrid.cshtml", objUWInbox),
                    HasItems = grid.HasItems
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return View("Error");
            }

        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProductDetails()
        {
            return View();
        }

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Quote()
        {
            AIA.Life.Models.Opportunity.LifeQuote objQuote = new AIA.Life.Models.Opportunity.LifeQuote();
            objQuote.UserName = Username;
            TempData["Load"] = "FirstTime";
            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            objQuote = objQuoteBusiness.LoadQuotationPool(objQuote);
            ViewBag.QuotationPoolCount = objQuote.ObjQuotationPool.Count;
            return View();
        }

        public ActionResult ProposalHome()
        {
            ProposalInbox obj = new ProposalInbox();
            obj.UserName = Username;

            obj = objPolicyBusiness.FetchProposalIncompleteDetails(obj);
            ViewBag.ProposalCount = obj.objProposalDetails.Count;
            obj = objPolicyBusiness.FetchProposalSubmittedDetails(obj);
            ViewBag.SubmittedProposals = obj.LstSubmittedProposals.Count;


            //ViewBag.count = objUWInbox.LstProposals.Count();
            UWInbox objUWInbox = new UWInbox();
            objUWInbox.UserName = Username;
            objUWInbox.Message = "Pending";
            objUWInbox = objPolicyBusiness.FetchUWProposals(objUWInbox);
            ViewBag.count = objUWInbox.LstProposals.Count();

            return View();
        }

        //Approval
        public ActionResult Approval()
        {
            return View();
        }

        public ActionResult SearchQuotationReAllocateDetails()
        {
            try
            {
                Policy obj = new Policy();
                obj.UserName = Username;
                AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
                obj = objQuoteBusiness.LoadQuotationReAllocate(obj);
                return PartialView("~/Views/Policy/PartialQuotationReAllocateGrid.cshtml", obj);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        public ActionResult proposalIncomplete()
        {
            ProposalInbox obj = new ProposalInbox();
            obj.UserName = Username;
            obj = objPolicyBusiness.FetchProposalIncompleteDetails(obj);
            ViewBag.Details = obj;
            TempData["Load"] = "FirstTime";
            return View(obj);
        }

        public ActionResult _PartialIncompleteProposalDetails()
        {
            try
            {
                ProposalInbox obj = new ProposalInbox();
                obj.UserName = Username;
                TempData["Load"] = "FirstTime";
                obj = objPolicyBusiness.FetchProposalIncompleteDetails(obj);

                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                var grid = (AjaxGrid<InboxDetails>)new AjaxGridFactory().CreateAjaxGrid((obj.objProposalDetails).AsQueryable(), 1, false, 2);

                return PartialView(grid);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        public ActionResult SearchproposalIncompleteDetails(int? Page)
        {
            try
            {
                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                AIA.Life.Models.Policy.ProposalInbox obj = new AIA.Life.Models.Policy.ProposalInbox();
                obj.UserName = Username;

                obj = objPolicyBusiness.FetchProposalIncompleteDetails(obj);
                var grid = aj.CreateAjaxGrid(obj.objProposalDetails.AsQueryable(), Page.HasValue ? Page.Value : 1, Page.HasValue, 2);
                ViewBag.Details = obj;

                return Json(new
                {
                    Html = RenderPartialViewToString("~/Views/Policy/_PartialIncompleteProposalDetails.cshtml", obj),
                    HasItems = grid.HasItems
                }, JsonRequestBehavior.AllowGet);


                //ProposalInbox obj = new ProposalInbox();
                //obj.UserName = Username;
                //obj = WebApiLogic.GetPostComplexTypeToAPI<ProposalInbox>(obj, "FetchProposalIncompleteDetails", "LifePolicyApi");

                //var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                //var grid = aj.CreateAjaxGrid(obj.objProposalDetails.AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, 2);

                //return Json(new
                //{
                //    Html = RenderPartialViewToString("~/Views/Policy/_PartialIncompleteProposalDetails.cshtml", grid),
                //    HasItems = grid.HasItems
                //}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        public ActionResult ProposalSubmitted()
        {
            ProposalInbox obj = new ProposalInbox();
            obj.UserName = Username;
            obj = objPolicyBusiness.FetchProposalSubmittedDetails(obj);
            ViewBag.Details = obj;
            TempData["Load"] = "FirstTime";
            return View(obj);
        }

        public ActionResult _PartialSubmittedProposalDetails()
        {
            try
            {
                ProposalInbox obj = new ProposalInbox();
                obj.UserName = Username;
                TempData["Load"] = "FirstTime";
                obj = objPolicyBusiness.FetchProposalSubmittedDetails(obj);

                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                var grid = (AjaxGrid<InboxDetails>)new AjaxGridFactory().CreateAjaxGrid((obj.objProposalDetails).AsQueryable(), 1, false, 2);

                return PartialView(grid);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        public ActionResult SearchproposalSumittedDetails(int? Page)
        {
            try
            {
                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                AIA.Life.Models.Policy.ProposalInbox obj = new AIA.Life.Models.Policy.ProposalInbox();
                obj.UserName = Username;

                obj = objPolicyBusiness.FetchProposalSubmittedDetails(obj);
                var grid = aj.CreateAjaxGrid(obj.objProposalDetails.AsQueryable(), Page.HasValue ? Page.Value : 1, Page.HasValue, 2);
                ViewBag.Details = obj;

                return Json(new
                {
                    Html = RenderPartialViewToString("~/Views/Policy/PartialSubmittedProposalsGrid.cshtml", obj),
                    HasItems = grid.HasItems
                }, JsonRequestBehavior.AllowGet);


                //ProposalInbox obj = new ProposalInbox();
                //obj.UserName = Username;
                //obj = WebApiLogic.GetPostComplexTypeToAPI<ProposalInbox>(obj, "FetchProposalIncompleteDetails", "LifePolicyApi");

                //var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                //var grid = aj.CreateAjaxGrid(obj.objProposalDetails.AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, 2);

                //return Json(new
                //{
                //    Html = RenderPartialViewToString("~/Views/Policy/_PartialIncompleteProposalDetails.cshtml", grid),
                //    HasItems = grid.HasItems
                //}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        public ActionResult QuotationReAllocate()
        {
            return View();
        }

        public ActionResult ProposalReAllocate()
        {
            return View();
        }

        public ActionResult CreateQuotation()
        {
            LifeQuote objQuote = new LifeQuote();

            ViewBag.PageName = "Quotation";
            AIA.Life.Models.Opportunity.Prospect objProspect = new AIA.Life.Models.Opportunity.Prospect();
            objProspect.UserName = Username;
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objProspect = objProspectBusiness.LoadContactInformation(objProspect);

            //objQuote = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Opportunity.LifeQuote>(objQuote, "LoadQuoteMaster", "SuspectApi");
            //ViewBag.Type = "CreateQuotation";
            QuoteList objQuoteList = new QuoteList();
            objQuoteList.objProspect = objProspect;
            objQuoteList.objListQuote.Add(objQuote);
            return View("~/Views/Policy/Quotation.cshtml", objQuoteList);
        }

        public ActionResult Quotation(string ContactID)
        {
            AIA.Life.Models.Opportunity.QuoteList objQuoteList = new AIA.Life.Models.Opportunity.QuoteList();
            AIA.Life.Models.Opportunity.Prospect objProspect = new AIA.Life.Models.Opportunity.Prospect();
            objProspect.UserName = objProspect.CreatedBy = Username;
            objProspect.ContactID = Convert.ToInt32(CrossCutting.CrossCutting_EncryptDecrypt.Decrypt(ContactID));
            #region Check user authorization
            AuthorizeUser authorizeUser = new AuthorizeUser();
            authorizeUser.UserName = Username;
            authorizeUser.ContactId = objProspect.ContactID;
            authorizeUser = new AIA.Life.Business.Common.CommonBusiness().CheckAuthorisation(authorizeUser);
            if (!string.IsNullOrEmpty(authorizeUser.Error.ErrorMessage))
            {
                return RedirectToAction("AuthorizeError", "Error", new { errorMessage = authorizeUser.Error.ErrorMessage });
            }
            #endregion
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objProspect = objProspectBusiness.LoadContactInformation(objProspect);
            //objProspect.objNeedAnalysis.dlladversities.Add(new SelectListItem { Text = "Critical Illnesses" });
            //objProspect.objNeedAnalysis.dlladversities.Add(new SelectListItem { Text = "Major Surgeries" });
            //objProspect.objNeedAnalysis.dlladversities.Add(new SelectListItem { Text = "Lost of Income" });
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
            objQuoteList.objProspect = objProspect;

            #region Added for Multiple Quotes
            if (objProspect.objNeedAnalysis.SelectedProducts != null)
            {
                var LstProducts = objProspect.objNeedAnalysis.SelectedProducts.Split(',');
                for (int i = 0; i < LstProducts.Count(); i++)
                {
                    AIA.Life.Models.Opportunity.LifeQuote objQuote = new AIA.Life.Models.Opportunity.LifeQuote();
                    objQuote.UserName = Username;
                    AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
                    objQuote = objQuoteBusiness.LoadMastersForQuote(objQuote);
                    objQuote.Contactid = Convert.ToInt32(CrossCutting.CrossCutting_EncryptDecrypt.Decrypt(ContactID));
                    objQuote.objProductDetials.Plan = objProspect.ListPlan.Where(a => a.Text == LstProducts[i]).Select(b => b.Value).FirstOrDefault();
                    objQuote.QuoteIndex = i;
                    objQuote.objProspect.AgeNextBdy = objProspect.AgeNextBdy;
                    objQuote.objProspect.DateofBirth = objProspect.DateofBirth;
                    objQuote.objProspect.Occupation = objProspect.Occupation;
                  
                    objQuote.objProspect.Gender = objProspect.Gender;
                    //Added by Udit for Smoke
                    objQuote.objProspect.IsSmoke = objProspect.IsSmoke;
                    // objQuote.objProspect.
                    #region Fill Spouse Details


                    if (objProspect.MaritalStatus == "M")
                    {
                        if (objQuote.objSpouseDetials == null)
                        { objQuote.objSpouseDetials = new Life.Models.Opportunity.SpouseDetails(); }
                        objQuote.objSpouseDetials.Occupation = objProspect.objNeedAnalysis.objSpouseDetails.OccuaptionID;
                        if (objProspect.Gender == "M")
                        {
                            objQuote.objSpouseDetials.Gender = "F";
                        }
                        else if (objProspect.Gender == "F")
                        {
                            objQuote.objSpouseDetials.Gender = "M";
                        }
                        objQuote.objSpouseDetials.AgeNextBirthday = objProspect.objNeedAnalysis.objSpouseDetails.AgeNextBirthday;
                        objQuote.objSpouseDetials.DOB = objProspect.objNeedAnalysis.objSpouseDetails.DateOfBirth;
                        //objQuote.IsSpouseCovered = true;
                    }

                    #endregion

                    #region Child Info

                    if (objProspect.objNeedAnalysis.objDependents.Count() > 0)
                    {
                        objQuote.IsChildCovered = true;
                        objQuote.NoofChilds = Convert.ToString(objProspect.objNeedAnalysis.objDependents.Count());
                    }
                    objQuote.objProspect.lstDependentRelationship = objProspect.lstDependentRelationship;
                    #endregion
                    objQuote.QuotationType = "Lead";
                    QuoteList obj = new QuoteList();
                    objQuoteList.objListQuote.Add(objQuote);
                }
            }
            else
            {
                AIA.Life.Models.Opportunity.LifeQuote objQuote = new AIA.Life.Models.Opportunity.LifeQuote();
                objQuote.UserName = Username;
                AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
                objQuote = objQuoteBusiness.LoadMastersForQuote(objQuote);
                objQuote.QuotationType = "Direct";
                objQuote.QuoteIndex = 0;
                if (objQuote.objSpouseDetials == null)
                { objQuote.objSpouseDetials = new Life.Models.Opportunity.SpouseDetails(); }
                objQuoteList.objListQuote.Add(objQuote);
            }

            foreach (PreviousInsuranceList item in objQuoteList.objProspect.objPreviousInsuranceList)
            {
                PrevPolicy obj = new PrevPolicy();
                obj.PolicyNo = item.PolicyNumber;
                obj.MaturityFund = 0;
                objQuoteList.objProspect.objNeedAnalysis.objPrevPolicy.Add(obj);
            }
            #endregion            
            objProspect.objNeedAnalysis.Stage = "Quotation";
            for (int i = 0; i < objQuoteList.objProspect.objNeedAnalysis.objFinancialNeeds.Count; i++)
            {
                if (!String.IsNullOrEmpty(objQuoteList.objProspect.objNeedAnalysis.objFinancialNeeds[i].Name))
                {
                    if (objQuoteList.objProspect.objNeedAnalysis.objFinancialNeeds[i].Name.ToUpper() == "HIGHER EDUCATION")
                    {
                        objQuoteList.objProspect.objNeedAnalysis.objFinancialNeeds[i].ImagePath = "/Content/Images/FnaIcons/Education@0,25x.png";
                    }
                    else if (objQuoteList.objProspect.objNeedAnalysis.objFinancialNeeds[i].Name.ToUpper() == "WEDDING")
                    {
                        objQuoteList.objProspect.objNeedAnalysis.objFinancialNeeds[i].ImagePath = "/Content/Images/FnaIcons/Children Wedding@0,25x.png";
                    }
                    else if (objQuoteList.objProspect.objNeedAnalysis.objFinancialNeeds[i].Name.ToUpper() == "PENSION FUND")
                    {
                        objQuoteList.objProspect.objNeedAnalysis.objFinancialNeeds[i].ImagePath = "/Content/Images/FnaIcons/Gratutity@0,25x.png";
                    }
                    else if (objQuoteList.objProspect.objNeedAnalysis.objFinancialNeeds[i].Name.ToUpper() == "BUY CAR/PROPERTY")
                    {
                        objQuoteList.objProspect.objNeedAnalysis.objFinancialNeeds[i].ImagePath = "/Content/Images/FnaIcons/Buy car_property@0,25x.png";
                    }
                    else if (objQuoteList.objProspect.objNeedAnalysis.objFinancialNeeds[i].Name.ToUpper() == "OTHER")
                    {
                        objQuoteList.objProspect.objNeedAnalysis.objFinancialNeeds[i].ImagePath = "/Content/Images/FnaIcons/Other@0,25x.png";
                    }
                }

            }

            return View(objQuoteList);
        }
        
        public ActionResult ModifyQuote(string QuoteNo, bool IsForCounterOffer = false, DateTime? RiskCommencementDate = null)
        {    
            if (RiskCommencementDate == null)
                RiskCommencementDate = DateTime.Now;
            AIA.Life.Models.Opportunity.LifeQuote objQuote = new AIA.Life.Models.Opportunity.LifeQuote();
            objQuote.QuoteNo = CrossCutting_EncryptDecrypt.Decrypt(QuoteNo);
            objQuote.UserName = Username;
            if (!IsForCounterOffer)
            {
                #region Check user authorization
                AuthorizeUser authorizeUser = new AuthorizeUser();
                authorizeUser.UserName = Username;
                authorizeUser.QuoteNo = objQuote.QuoteNo;
                authorizeUser = new AIA.Life.Business.Common.CommonBusiness().CheckAuthorisation(authorizeUser);
                if (!string.IsNullOrEmpty(authorizeUser.Error.ErrorMessage))
                {
                    return RedirectToAction("AuthorizeError", "Error", new { errorMessage = authorizeUser.Error.ErrorMessage });
                }
                #endregion
            }
            objQuote.IsModifyQuote = true;
            objQuote.RiskCommencementDate = RiskCommencementDate;
            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            objQuote = objQuoteBusiness.FetchQuoteData(objQuote);

            objQuote.IsModifyQuote = true;
            AIA.Life.Models.Opportunity.QuoteList objQuoteList = new AIA.Life.Models.Opportunity.QuoteList();
            objQuoteList.objProspect = objQuote.objProspect;
            objQuoteList.ObjLifeQuote = objQuote;
            objQuoteList.PrevQuoteNo = objQuote.PrevQuoteNo;
            //objQuoteList.QuoteProposerSignPath = objQuote.ProposerSignPath;
            //objQuoteList.QuoteWPProposerSignPath = objQuote.WPProposerSignPath;
            if (IsForCounterOffer)
            { objQuote.IsForCounterOffer = true; objQuoteList.IsForCounterOffer = true; }
            objQuoteList.objListQuote.Add(objQuote);

            foreach (PreviousInsuranceList item in objQuote.objProspect.objPreviousInsuranceList)
            {
                PrevPolicy obj = new PrevPolicy();
                obj.PolicyNo = item.PolicyNumber;
                obj.MaturityFund = 0;
                objQuote.objProspect.objNeedAnalysis.objPrevPolicy.Add(obj);
            }
            objQuoteList.objProspect.objNeedAnalysis.Stage = "Quotation";
            for (int i = 0; i < objQuote.objProspect.objNeedAnalysis.objFinancialNeeds.Count; i++)
            {
                if (!String.IsNullOrEmpty(objQuote.objProspect.objNeedAnalysis.objFinancialNeeds[i].Name))
                {
                    if (objQuote.objProspect.objNeedAnalysis.objFinancialNeeds[i].Name.ToUpper() == "HIGHER EDUCATION")
                    {
                        objQuote.objProspect.objNeedAnalysis.objFinancialNeeds[i].ImagePath = "/Content/Images/FnaIcons/Education@0,25x.png";
                    }
                    else if (objQuote.objProspect.objNeedAnalysis.objFinancialNeeds[i].Name.ToUpper() == "WEDDING")
                    {
                        objQuote.objProspect.objNeedAnalysis.objFinancialNeeds[i].ImagePath = "/Content/Images/FnaIcons/Children Wedding@0,25x.png";
                    }
                    else if (objQuote.objProspect.objNeedAnalysis.objFinancialNeeds[i].Name.ToUpper() == "PENSION FUND")
                    {
                        objQuote.objProspect.objNeedAnalysis.objFinancialNeeds[i].ImagePath = "/Content/Images/FnaIcons/Gratutity@0,25x.png";
                    }
                    else if (objQuote.objProspect.objNeedAnalysis.objFinancialNeeds[i].Name.ToUpper() == "BUY CAR/PROPERTY")
                    {
                        objQuote.objProspect.objNeedAnalysis.objFinancialNeeds[i].ImagePath = "/Content/Images/FnaIcons/Buy car_property@0,25x.png";
                    }
                    else if (objQuote.objProspect.objNeedAnalysis.objFinancialNeeds[i].Name.ToUpper() == "OTHER")
                    {
                        objQuote.objProspect.objNeedAnalysis.objFinancialNeeds[i].ImagePath = "/Content/Images/FnaIcons/Other@0,25x.png";
                    }
                }
            }
            return PartialView("~/Views/Policy/Quotation.cshtml", objQuoteList);
        }

        public string SendQuoteMail(string QuoteNo, string ProductCode, string PreferredLanguage, string EmailAddress)
        {
            try
            {
                AIA.Life.Models.Opportunity.LifeQuote objQuote = new AIA.Life.Models.Opportunity.LifeQuote();
                objQuote.QuoteNo = CrossCutting_EncryptDecrypt.Decrypt(QuoteNo);
                // objQuote.objProductDetials.PreferredLangauage = CrossCutting_EncryptDecrypt.Decrypt(PreferredLanguage);
                // objQuote.objProductDetials.PlanCode = CrossCutting_EncryptDecrypt.Decrypt(ProductCode);
                objQuote.UserName = Username;

                objQuote.IsModifyQuote = false;
                AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
                objQuote = objQuoteBusiness.FetchQuoteData(objQuote);
                ReportsController objReportController = new ReportsController();
                objQuote.ByteArray = objReportController.ReportQuotation(objQuote.QuoteNo, objQuote.objProductDetials.PlanCode, objQuote.objProductDetials.PreferredLangauage);
                objQuote.EmailAddress = EmailAddress;
                objQuote.objProspect.Email = EmailAddress;
                objQuote = objQuoteBusiness.SendEmailAndSMSNotificationOnQuoteCreation(objQuote);
                return "Quotation emailed successfully";
            }
            catch (Exception ex)
            {
                ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                return "Email Sending Failed";
            }

        }

        public string FetchEmailAddress(string QuoteNo)
        {
            try
            {
                AIA.Life.Models.Opportunity.LifeQuote objQuote = new AIA.Life.Models.Opportunity.LifeQuote();
                objQuote.QuoteNo = CrossCutting_EncryptDecrypt.Decrypt(QuoteNo);
                objQuote.UserName = Username;
                objQuote.IsModifyQuote = false;
                AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
                objQuote = objQuoteBusiness.FetchQuoteData(objQuote);
                AIA.Life.Repository.AIAEntity.AVOAIALifeEntities entities = new Life.Repository.AIAEntity.AVOAIALifeEntities();
                string Emailofuser = entities.tblUserDetails.Where(a => a.LoginID == Username).Select(a => a.Email).FirstOrDefault();
                objQuote.EmailAddress = Emailofuser + "," + objQuote.objProspect.Email;
                return objQuote.EmailAddress;
            }
            catch (Exception ex)
            {
                ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                return "Fail";
            }

        }

        public ActionResult GetDynamicTabs(AIA.Life.Models.Opportunity.QuoteList objQuoteList)
        {
            AIA.Life.Models.Opportunity.LifeQuote objQuote = objQuoteList.objListQuote[objQuoteList.SelectedQuote];
            objQuote.objProspect = objQuoteList.objProspect;
            objQuote.ListAssured = new List<string>();
            objQuote.objQuoteMemberDetails = new List<Life.Models.Opportunity.QuoteMemberDetails>();
            #region Deriving Tabs based on captured Data
            //QuoteList objQuoteList = new QuoteList();
            //if (objQuote.IsSelfCovered)
            //{
            Life.Models.Opportunity.QuoteMemberDetails obj = new Life.Models.Opportunity.QuoteMemberDetails();
            obj.Relationship = "267";
            obj.Assured = "MainLife";
            obj.AgeNextBirthDay = objQuote.objProspect.AgeNextBdy ?? 0;
           //  objQuote.objProspect.IsSmoke;
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objQuote = objProspectBusiness.LoadQuoteMaster(objQuote);

            //objQuote.lstSumInsured = objQuoteList.ObjLifeQuote.lstSumInsured;
            objQuote.objQuoteMemberDetails.Add(obj);
            //}
            if (objQuote.IsSpouseCovered)
            {
                string AssuedName = string.Empty;
                Life.Models.Opportunity.QuoteMemberDetails objS = new Life.Models.Opportunity.QuoteMemberDetails();
                //if (!objQuote.IsSelfCovered)
                //{
                //    AssuedName = "MainLife";
                //}
                //else
                //{
                AssuedName = "Spouse";
                //}
                objS.Relationship = "268";
                objS.Assured = AssuedName;
                objQuote.objQuoteMemberDetails.Add(objS);
                objQuote.objSpouseDetials.AssuredName = AssuedName;
                objQuote.ListAssured.Add(AssuedName);
            }
            if (objQuote.IsChildCovered)
            {

                int Count = Convert.ToInt32(objQuote.NoofChilds);
                for (int i = 1; i <= Count; i++)
                {
                    string Child = "Child" + i;
                    objQuote.ListAssured.Add(Child);
                    objQuote.objChildDetials[i - 1].Assured = Child;
                    Life.Models.Opportunity.QuoteMemberDetails objC = new Life.Models.Opportunity.QuoteMemberDetails();
                    objC.Assured = Child;
                    objC.Relationship = objQuote.objChildDetials[i - 1].Relationship;
                    objC.AgeNextBirthDay = objQuote.objChildDetials[i - 1].AgeNextBirthday;
                    objQuote.objQuoteMemberDetails.Add(objC);
                }
            }
            #endregion
            //return PartialView(objQuote);
            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            objQuote = objQuoteBusiness.LoadBenefits(objQuote);
            objQuoteList.objListQuote[objQuoteList.SelectedQuote] = objQuote;
            return PartialView("~/Views/Policy/GetDynamicTabs.cshtml", objQuoteList);
        }

        public ActionResult GetBenifits(AIA.Life.Models.Opportunity.LifeQuote objQuote)
        {

            objQuote.UserName = Username;
            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            objQuote = objQuoteBusiness.LoadBenefits(objQuote);
            return PartialView("~/Views/Prospect/PartialBenifitDetails.cshtml", objQuote);
        }

        public ActionResult SaveQuotation(AIA.Life.Models.Opportunity.QuoteList objQuoteList)
        {
            AIA.Life.Models.Opportunity.LifeQuote objQuote = objQuoteList.objListQuote[objQuoteList.SelectedQuote];
            #region Proposer Signature
            if (!string.IsNullOrEmpty(objQuote.Signature))
            {
                SignatureToImage objSigToImg = new SignatureToImage();
                string filename = "Proposersignature.png";

                string directryPath = Server.MapPath("ContactID");
                if (!Directory.Exists(directryPath))
                {
                    Directory.CreateDirectory(directryPath);
                }
                string filepath = Path.Combine(directryPath, filename);

                var img = objSigToImg.SigJsonToImage(objQuote.Signature);
                objQuote.ProspectSign = ImageToByte(img);
                img.Save(filepath, ImageFormat.Png);

                objQuote.ProposerSignPath = filepath;

            }
            #endregion
            #region WP Proposer Signature
            if (!string.IsNullOrEmpty(objQuote.WPProposerSignature))
            {
                SignatureToImage objSigToImg = new SignatureToImage();
                string filename = "WPProposersignature.png";

                string directryPath = Server.MapPath("ContactID");
                if (!Directory.Exists(directryPath))
                {
                    Directory.CreateDirectory(directryPath);
                }
                string filepath = Path.Combine(directryPath, filename);

                var img = objSigToImg.SigJsonToImage(objQuote.WPProposerSignature);
                objQuote.WPSignature = ImageToByte(img);
                img.Save(filepath, ImageFormat.Png);
                objQuote.WPProposerSignPath = filepath;
            }
            #endregion
            objQuote.IsForCounterOffer = objQuoteList.IsForCounterOffer;
            if (objQuote.IsForCounterOffer)
            {
                objQuote.PrevQuoteNo = objQuoteList.PrevQuoteNo;
                Policy policy = new Policy();
                policy.QuoteNo = objQuote.PrevQuoteNo;
            }
            objQuote.UserName = Username;
            objQuote.objProspect = objQuoteList.objProspect;
            objQuote.ObjQuotationPreviousInsurance = objQuoteList.ObjQuotationPreviousInsurance;


            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            objQuote = objQuoteBusiness.SaveQuote(objQuote);

            if (objQuote.Message == "Success" && !objQuote.IsForCounterOffer)
            {
                try
                {
                    ReportsController objReportController = new ReportsController();
                    objQuote.ByteArray = objReportController.ReportQuotation(objQuote.QuoteNo, objQuote.objProductDetials.PlanCode, objQuote.objProductDetials.PreferredLangauage);
                    //if (objQuote.IsForCounterOffer)
                    //{
                    //    objQuote.ByteArray1 = objReportController.ReportForCLADocument(objQuote.QuoteNo, objQuote.ProposalNo);

                    //}
                    objQuoteBusiness.SendEmailAndSMSNotificationOnQuoteCreation(objQuote);
                }
                catch (Exception)
                {


                }
            }

            var ObjResponse = new { Message = objQuote.Message, QuoteNo = objQuote.QuoteNo, ErrorMessage = objQuote.Error.ErrorMessage };

            return Json(ObjResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveCreateQuotation(AIA.Life.Models.Opportunity.LifeQuote objQuote)
        {

            objQuote.UserName = Username;
            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            objQuote = objQuoteBusiness.SaveCreateQuote(objQuote);
            var ObjResponse = new { Message = objQuote.Message, QuoteNo = objQuote.QuoteNo };
            return Json(ObjResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult QuotationPool()
        {
            AIA.Life.Models.Opportunity.LifeQuote objQuote = new AIA.Life.Models.Opportunity.LifeQuote();
            objQuote.UserName = Username;
            TempData["Load"] = "FirstTime";
            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            objQuote = objQuoteBusiness.LoadQuotationPool(objQuote);
            return View(objQuote);
        }

        public ActionResult PartialQuotationPoolGrid()
        {
            try
            {
                AIA.Life.Models.Opportunity.LifeQuote objQuote = new AIA.Life.Models.Opportunity.LifeQuote();
                objQuote.UserName = Username;
                AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
                objQuote = objQuoteBusiness.LoadQuotationPool(objQuote);

                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                var grid = (AjaxGrid<QuotionPool>)new AjaxGridFactory().CreateAjaxGrid((objQuote.ObjQuotationPool).AsQueryable(), 1, false, 2);

                return PartialView(grid);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        public ActionResult SearchQuotationPoolDetails(int? page)
        {
            try
            {
                AIA.Life.Models.Opportunity.LifeQuote objQuote = new AIA.Life.Models.Opportunity.LifeQuote();
                objQuote.UserName = Username;
                AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
                objQuote = objQuoteBusiness.LoadQuotationPool(objQuote);

                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                var grid = aj.CreateAjaxGrid(objQuote.ObjQuotationPool.AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, 2);

                return Json(new
                {
                    Html = RenderPartialViewToString("~/Views/Policy/PartialQuotationPoolGrid.cshtml", grid),
                    HasItems = grid.HasItems
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        public ActionResult Proposal(string QuoteNo)
        {
            Policy objpolicy = new Policy();
            QuoteNo = CrossCutting_EncryptDecrypt.Decrypt(QuoteNo);
            objpolicy.QuoteNo = QuoteNo;
            objpolicy.UserName = Username;
            #region Check user authorization
            AuthorizeUser authorizeUser = new AuthorizeUser();
            authorizeUser.UserName = Username;
            authorizeUser.QuoteNo = QuoteNo;
            authorizeUser = new AIA.Life.Business.Common.CommonBusiness().CheckAuthorisation(authorizeUser);
            if (!string.IsNullOrEmpty(authorizeUser.Error.ErrorMessage))
            {
                return RedirectToAction("AuthorizeError", "Error", new { errorMessage = authorizeUser.Error.ErrorMessage });
            }
            #endregion
            //objpolicy.objFillMemberDetials.OccupationID = OccupationID;
            objpolicy = objPolicyBusiness.LoadProposalInfo(objpolicy);
            objPolicyBusiness.InvokeILWorkFlowAck(objpolicy);
            objpolicy.objNomineeDetails = new List<NomineeDetails>();
            // Added to Avoid Duplicates
            if (objpolicy.PolicyID > 0)
            {
                return RedirectToAction("ModifyProposal", new { PolicyID = CrossCutting_EncryptDecrypt.Encrypt(objpolicy.PolicyID.ToString()) });
            }
            if (System.Web.Configuration.WebConfigurationManager.AppSettings["PublishEnvironment"] == "SIT")
            {
                return View(objpolicy);
            }
            else
            {
                if (string.IsNullOrEmpty(objpolicy.ProposalNo))
                    return View("Error", objpolicy.Error);
                else
                    return View(objpolicy);
            }

        }
        public ActionResult CreateProposal(AIA.Life.Models.Opportunity.QuoteList objQuoteList)
        {
            return RedirectToAction("Proposal", new { QuoteNo = CrossCutting_EncryptDecrypt.Encrypt(objQuoteList.objListQuote[objQuoteList.SelectedQuote].QuoteNo) });
        }
        public ActionResult SubmitCounterOfferQuote(AIA.Life.Models.Opportunity.QuoteList objQuoteList)
        {
            Policy policy = new Policy();
            policy.QuoteNo = objQuoteList.objListQuote[objQuoteList.SelectedQuote].QuoteNo;
            policy = objPolicyBusiness.SubmitCounterOfferQuote(policy);
            return RedirectToAction("ModifyProposal", new { PolicyID = CrossCutting_EncryptDecrypt.Encrypt(policy.PolicyID.ToString()), userType = "UW" });
        }
        public ActionResult CancelCounterOfferQuote(AIA.Life.Models.Opportunity.QuoteList objQuoteList)
        {
            Policy policy = new Policy();
            policy.QuoteNo = objQuoteList.objListQuote[objQuoteList.SelectedQuote].QuoteNo;
            string result = policy.QuoteNo.Substring(policy.QuoteNo.Length - 2,2);
            int prevVersion = Convert.ToInt32(result) - 1;
            string value = prevVersion.ToString("D2");
            policy.QuoteNo = policy.QuoteNo.Remove(policy.QuoteNo.Length - 2, 2) + value;
            policy = objPolicyBusiness.SubmitCounterOfferQuote(policy);
            return RedirectToAction("ModifyProposal", new { PolicyID = CrossCutting_EncryptDecrypt.Encrypt(policy.PolicyID.ToString()), userType = "UW" });
        }
        public ActionResult ProposalLifeAssuredDetails(AIA.Life.Models.Policy.Policy objpolicy)
        {

            objpolicy.UserName = Username;
            //objpolicy.objFillMemberDetials.OccupationID = OccupationID;
            objpolicy = objPolicyBusiness.FetchProposalLifeAssuredDetails(objpolicy);
            return Json(objpolicy.objProspectDetails, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NomineeDetailsWithMemberDetails(AIA.Life.Models.Policy.Policy objpolicy, int RelationValue)
        {
            objpolicy.UserName = Username;
            //objpolicy.objFillMemberDetials.OccupationID = OccupationID;
            if (RelationValue == 2498)
            {
                objpolicy.NomineeRelationshipDetailsID = "270";
            }
            if (RelationValue == 2524)
            {
                objpolicy.NomineeRelationshipDetailsID = "269";
            }
            objpolicy = objPolicyBusiness.FetchProposalNomineeDetailsWithMemberDetails(objpolicy);
            //return Json(objpolicy.objProspectDetails, JsonRequestBehavior.AllowGet);

            //objpolicy = WebApiLogic.GetPostComplexTypeToAPI<Life.Models.Policy.Policy>(objpolicy, "ResidentialStatusQuestions", "LifePolicyApi");
            return PartialView("~/Views/Policy/_PartialNomineeLifeAssuredDetailsGrid.cshtml", objpolicy);
        }


        public ActionResult ModifyProposal(string PolicyID, string userType = null)
        {
            Policy objpolicy = new Policy();
            objpolicy.PolicyID = Convert.ToDecimal(CrossCutting_EncryptDecrypt.Decrypt(PolicyID));
            objpolicy.UserName = Username;
            objpolicy.UserType = userType;
            if (string.IsNullOrEmpty(userType))
            {
                #region Check user authorization
                AuthorizeUser authorizeUser = new AuthorizeUser();
                authorizeUser.UserName = Username;
                authorizeUser.PolicyId = objpolicy.PolicyID;
                authorizeUser = new AIA.Life.Business.Common.CommonBusiness().CheckAuthorisation(authorizeUser);
                if (!string.IsNullOrEmpty(authorizeUser.Error.ErrorMessage))
                {
                    return RedirectToAction("AuthorizeError", "Error", new { errorMessage = authorizeUser.Error.ErrorMessage });
                }
                #endregion
            }
            objpolicy = objPolicyBusiness.FetchProposalInfo(objpolicy);
            if (!string.IsNullOrEmpty(userType))
            {
                objpolicy.Underwriter = "UW";
            }
            return View("Proposal", objpolicy);
        }

        public ActionResult ProposalOccupationQuestions(Life.Models.Policy.Policy objpolicy, int OccupationID)
        {

            if (objpolicy.objMemberDetails != null)
            {
                for (int i = 0; i < objpolicy.objMemberDetails.Count(); i++)
                {
                    objpolicy.objMemberDetails[i].OccupationID = OccupationID;
                }
            }

            objpolicy.UserName = Username;
            objpolicy = objPolicyBusiness.OccupationQuestions(objpolicy);
            return PartialView("~/Views/Policy/_PartialAdditionalQuestionnaire.cshtml", objpolicy);
        }

        public ActionResult ProposalResidentialStatusQuestions(Life.Models.Policy.Policy objpolicy, string ResidentialStatusID, string OccuPationID, int MemberAssuredIndex, string MemberOccupationHazardousWork, decimal SAR)
        {

            if (objpolicy.objMemberDetails != null)
            {

                for (int i = 0; i < objpolicy.objMemberDetails.Count(); i++)
                {
                    if (ResidentialStatusID == "SL")
                    {
                        objpolicy.objMemberDetails[i].ResidentialStatus = "ABC";
                    }
                    if (ResidentialStatusID != "SL")
                    {
                        objpolicy.objMemberDetails[i].ResidentialStatus = "SL";
                    }
                    if (SAR >= 5000000)
                    {
                        objpolicy.objMemberDetails[i].SAR = Convert.ToDecimal(5000000.0);
                    }

                    objpolicy.objMemberDetails[i].OccupationID = Convert.ToInt32(OccuPationID);
                    if (MemberOccupationHazardousWork != "undefined")
                    {
                        objpolicy.objMemberDetails[i].OccupationHazardousWork = Convert.ToBoolean(MemberOccupationHazardousWork);
                    }

                    objpolicy.AssuredIndex = MemberAssuredIndex;
                }
            }

            objpolicy.UserName = Username;

            objpolicy = objPolicyBusiness.ResidentialStatusQuestions(objpolicy);

            //foreach (var item in objpolicy.objMemberDetails)
            //{
            //    if (item.AssuredName == "MainLife")
            //    {
            //        objpolicy.AssuredIndex = 0;
            //    }
            //    else if (item.AssuredName == "Spouse")
            //    {
            //        objpolicy.AssuredIndex = 1;
            //    }
            //}
            return PartialView("~/Views/Policy/_PartialAdditionalQuestionnaire.cshtml", objpolicy);
        }

        public ActionResult GetMHDProposal(AIA.Life.Models.Policy.Policy objPolicy, string AssuredName)
        {

            // objPolicy.SelectedDiseases = items;
            objPolicy.AssuredName = AssuredName;
            objPolicy = objPolicyBusiness.LoadMHDProposalInfo(objPolicy);
            return PartialView("_PartialDiseasesQuestionnaire", objPolicy);
        }

        public ActionResult GetAssuredMemberDetails(AIA.Life.Models.Policy.Policy objPolicy)
        {
            objPolicy.UserName = Username;
            objPolicy = objPolicyBusiness.AssuredMemberDetails(objPolicy);
            return PartialView("~/Views/Policy/_PartialQuestionnaire.cshtml", objPolicy);
        }

        public ActionResult GetBenifitDetails(AIA.Life.Models.Policy.Policy objPolicy)
        {
            objPolicy.UserName = Username;
            objPolicy = objPolicyBusiness.BenefitDetails(objPolicy);
            return PartialView("~/Views/Policy/BenifitAndMedicalHistoryDetails.cshtml", objPolicy);
        }
        public static byte[] ImageToByte(System.Drawing.Image img)
        {

            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }


        public ActionResult SaveProposal(AIA.Life.Models.Policy.Policy objPolicy)
        {
            try
            {
                #region Proposer Signature
                if (!string.IsNullOrEmpty(objPolicy.Signature))
                {
                    SignatureToImage objSigToImg = new SignatureToImage();
                    string filename = "Proposersignature.png";

                    string directryPath = Server.MapPath("ContactID_" + objPolicy.ContactID);
                    if (!Directory.Exists(directryPath))
                    {
                        Directory.CreateDirectory(directryPath);
                    }
                    string filepath = Path.Combine(directryPath, filename);

                    var img = objSigToImg.SigJsonToImage(objPolicy.Signature);

                    //Bitmap bmap = (Bitmap)img;
                    //Color col;
                    ////Graphics g = Graphics.FromImage(img);
                    //for (int i = 0; i < img.Width; i++)
                    //{
                    //    for (int j = 0; j < img.Height; j++)
                    //    {
                    //        col = bmap.GetPixel(i, j);
                    //        //var averageValue = ((int)col.R+ (int)col.R + (int)col.R) / 3;
                    //        //bmap.SetPixel(i, j, Color.White);
                    //        bmap.SetPixel(i, j,
                    //        Color.FromArgb(000, 000, 000));
                    //        //Color.FromArgb(averageValue, averageValue, averageValue));
                    //        //Color.FromName("Black"));
                    //    }
                    //}

                    objPolicy.ByteArray = ImageToByte(img);
                    img.Save(filepath, ImageFormat.Png);
                    objPolicy.Signature = Path.Combine("\\Policy", "ContactID_" + objPolicy.ContactID, filename);
                    objPolicy.ProposalFilePath = Path.Combine("\\Policy", "ContactID_" + objPolicy.ContactID, filename);


                    objPolicy.ProposerSignatureFile = ImageToByte(img);
                }
                #endregion

                #region WP Proposer Signature
                if (!string.IsNullOrEmpty(objPolicy.WPProposerSignature))
                {
                    SignatureToImage objSigToImg = new SignatureToImage();
                    string filename = "WPProposersignature.png";

                    string directryPath = Server.MapPath("ContactID_" + objPolicy.ContactID);
                    if (!Directory.Exists(directryPath))
                    {
                        Directory.CreateDirectory(directryPath);
                    }
                    string filepath = Path.Combine(directryPath, filename);

                    var img = objSigToImg.SigJsonToImage(objPolicy.WPProposerSignature);
                    objPolicy.ByteArray = ImageToByte(img);
                    img.Save(filepath, ImageFormat.Png);
                    objPolicy.WPProposerSignature = Path.Combine("\\Policy", "ContactID_" + objPolicy.ContactID, filename);
                    objPolicy.WPSignatureFile = ImageToByte(img);
                }
                #endregion

                #region Spouse Signature
                Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
                List<DocumentUploadFile> objLstDoc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DocumentUploadFile>>(objPolicy.HdnDocumentDetails, settings);

                if (!string.IsNullOrEmpty(objPolicy.SpouseSignature))
                {
                    SignatureToImage objSigToImg = new SignatureToImage();
                    string filename = "Spousesignature.png";

                    string directryPath = Server.MapPath("ContactID_" + objPolicy.ContactID);
                    if (!Directory.Exists(directryPath))
                    {
                        Directory.CreateDirectory(directryPath);
                    }
                    string filepath = Path.Combine(directryPath, filename);

                    var img = objSigToImg.SigJsonToImage(objPolicy.SpouseSignature);
                    objPolicy.ByteArray = ImageToByte(img);
                    img.Save(filepath, ImageFormat.Png);
                    objPolicy.SpouseSignature = Path.Combine("\\Policy", "ContactID_" + objPolicy.ContactID, filename);
                    objPolicy.SpouseSignatureFile = ImageToByte(img);
                }

                #endregion

                objPolicy.UserName = Username;
                objPolicy = objPolicyBusiness.SaveProposal(objPolicy);

                // Proposal Emails will come at the time of Policy Issuance as per Client
                //if (objPolicy.Message == "Success")
                //{
                //    try
                //    {
                //        ReportsController objProposalReport = new ReportsController();
                //        objPolicy.ByteArray = objProposalReport.ProposalReport(objPolicy.QuoteNo, objPolicy.PlanCode);
                //        WebApiLogic.FireForgetAPI(objPolicy, "SendEmailAndSMSNotificationOnSaveProposal", "LifePolicyApi");
                //    }
                //    catch (Exception ex)
                //    {

                //    }
                //}
                //if (objPolicy.ProcceedToPayment)
                //{
                //    objPolicyBusiness.InvokeILModifyProposal(objPolicy);
                //    Thread.Sleep(60000);
                //    return RedirectToAction("ProposalPayment", "Payment", new { QuoteNo = CrossCutting_EncryptDecrypt.Encrypt(objPolicy.QuoteNo) });
                //}
                //else
                //{
                var ObjResponse = new { Message = objPolicy.Message, Quote = objPolicy.QuoteNo };
                return Json(ObjResponse, JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception e)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = objPolicy.Error.ErrorCode = Codes.GetErrorCode();

                objPolicy.Message = "Please inform the IT HelpDesk on this application issue. Error Code is " + objPolicy.Error.ErrorCode+" Sorry for the inconvenience caused";
                throw;
            }

        }
        public ActionResult ProcessProposal(Policy policy)
        {
            return View(policy);
        }
        public ActionResult ProccedToPayment(Policy policy)
        {
            policy = objPolicyBusiness.InvokeILModifyProposalAVO(policy);
            //Thread.Sleep(115000);
            return RedirectToAction("ProposalPayment", "Payment", new { QuoteNo = CrossCutting_EncryptDecrypt.Encrypt(policy.QuoteNo) });
        }
        public ActionResult gooleVisionTextDecoderApi(string AgeProofDocumentType)
        {
            OCRResponse objResp = new OCRResponse();
            objResp.UserName = Username;
            objResp.DocType = AgeProofDocumentType;
            var file = Request.Files[0].InputStream;
            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(file))
            {
                fileData = binaryReader.ReadBytes(Request.Files[0].ContentLength);
            }
            objResp.Filedata = Convert.ToBase64String(fileData);
            //string fileName = string.Empty;
            //string path = string.Empty;
            //if (file != null && file.ContentLength > 0)
            //{
            //    Stream s;
            //    s = file.InputStream;

            //}

            AIA.Life.Business.Common.CommonBusiness obj = new AIA.Life.Business.Common.CommonBusiness();
            objResp = obj.gooleVisionTextDecoderApi(objResp);
            return Json(objResp, JsonRequestBehavior.AllowGet);
        }



        public ActionResult SubmitPolicyDocuments(AIA.Life.Models.Policy.Policy objPolicy)
        {
            objPolicy = objPolicyBusiness.SubmitPolicyDocuments(objPolicy);
            if (objPolicy.Message == "Success")
            {
                //if (objPolicy.PolicyStageStatusID == CrossCuttingConstants.PolicyStageStatusCounterOffer && (objPolicy.AdditionalPremium <= 0 || objPolicy.AdditionalPremium == null))
                //{
                //    objPolicyBusiness.InvokeILModifyProposalAVO(objPolicy);
                //    Thread.Sleep(60000);
                //}
                if (objPolicy.ProcceedToPayment)
                {
                    return RedirectToAction("ProposalPayment", "Payment", new { QuoteNo = CrossCutting_EncryptDecrypt.Encrypt(objPolicy.QuoteNo) });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                return RedirectToAction("Index", "Home");// Need to Redirect To Error page
            }
        }

        public ActionResult CalculateQuotePremium(AIA.Life.Models.Opportunity.QuoteList objQuoteList)
        {
            try
            {
                LifeQuote objLifeQuote = objQuoteList.objListQuote[objQuoteList.SelectedQuote];
                objLifeQuote.objProspect = objQuoteList.objProspect;
                objLifeQuote.UserName = Username;
                objLifeQuote = objPolicyBusiness.CalculateQuotePremium(objLifeQuote);
                objQuoteList.objListQuote[objQuoteList.SelectedQuote] = objLifeQuote;
                objLifeQuote.STRHtml = RenderPartialViewToString("~/Views/Prospect/PartialBenifitOverview.cshtml", objLifeQuote);
                objLifeQuote.STRPremiumHtml = RenderPartialViewToString("~/Views/Policy/PartialIllustration.cshtml", objQuoteList);
                objLifeQuote.STRBenefitHtml = RenderPartialViewToString("~/Views/Policy/GetDynamicTabs.cshtml", objQuoteList);
                if (string.IsNullOrEmpty(objLifeQuote.Error.ErrorMessage))
                    objLifeQuote.Message = "Success";
                return Json(objLifeQuote, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CalculateProposalPremium(AIA.Life.Models.Policy.Policy objLifeProposal)
        {
            try
            {
                objLifeProposal.UserName = Username;
                //objLifeProposal = objPolicyBusiness.CalculateProposalPremium(objLifeProposal);
                objLifeProposal.STRHtml = RenderPartialViewToString("~/Views/Policy/PartialIllustration.cshtml", objLifeProposal.LstIllustation);
                return Json(objLifeProposal, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ProductLifeDetails(AIA.Life.Models.Policy.Policy objProposal)
        {
            objProposal.UserName = Username;
            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            objProposal = objQuoteBusiness.LoadMastersForProposalDetails(objProposal);
            //objProposal = WebApiLogic.GetPostComplexTypeToAPI<Life.Models.Policy.Policy>(objProposal, "LoadProposalBenefits", "LifeQuoteApi");
            objProposal = objPolicyBusiness.LoadProposalBenefits(objProposal);
            objProposal = objPolicyBusiness.FetchProposalInfo(objProposal);
            return PartialView("~/Views/Policy/PartialProposalBenifitCovers.cshtml", objProposal);
        }

        public ActionResult GetProposalBenifits(AIA.Life.Models.Opportunity.LifeQuote objQuote)
        {
            objQuote.UserName = Username;
            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            objQuote = objQuoteBusiness.LoadBenefits(objQuote);
            return PartialView("~/Views/Policy/PartialProposalBenifitCovers.cshtml", objQuote);
        }

        public ActionResult SaveUWRemarks(AIA.Life.Models.Policy.Policy objProposal)
        {
            objProposal.UserName = Username;
            objProposal.IsIntermSave = true;
            foreach (var Member in objProposal.objMemberDetails)
            {
                if (Member.ObjUwDecision.UWMedicalCode != null)
                {
                    var items = JsonConvert.DeserializeObject<List<string>>(Member.ObjUwDecision.UWMedicalCode);
                    List<string> SelectedMedicalCodes = new List<string>();
                    foreach (var item in items)
                    {
                        SelectedMedicalCodes.Add(item);
                    }
                    string MedicalCode = (string.Join(",", SelectedMedicalCodes.Select(x => x.ToString()).ToArray()));
                    Member.ObjUwDecision.UWMedicalCode = MedicalCode;
                }
            }
            objProposal = objPolicyBusiness.SaveUWRemarks(objProposal);
            var ObjResponse = new { Message = objProposal.Message };
            return Json(ObjResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SubmitUWRemarks(AIA.Life.Models.Policy.Policy objProposal, bool IsCounterOFFERCase = false)
        {
            if (objProposal == null)
                objProposal =(Policy) TempData["Proposal"];
            objProposal.UserName = Username;
            objProposal.IsIntermSave = false;
            objProposal.CounterOfferCase = IsCounterOFFERCase;
            foreach (var Member in objProposal.objMemberDetails)
            {
                if (Member.ObjUwDecision.UWMedicalCode != null)
                {
                    var items = JsonConvert.DeserializeObject<List<string>>(Member.ObjUwDecision.UWMedicalCode);
                    List<string> SelectedMedicalCodes = new List<string>();
                    foreach (var item in items)
                    {
                        SelectedMedicalCodes.Add(item);
                    }
                    string MedicalCode = (string.Join(",", SelectedMedicalCodes.Select(x => x.ToString()).ToArray()));
                    Member.ObjUwDecision.UWMedicalCode = MedicalCode;
                }
            }
            
            if(string.IsNullOrEmpty(objProposal.Error.WarningMessage))
                objProposal = objPolicyBusiness.SaveUWRemarks(objProposal);
            var ObjResponse = new { Message = objProposal.Message, ProposalNo = objProposal.ProposalNo, Decision = objProposal.Decision, ErrorMessage = objProposal.Error.ErrorMessage, WarningMessage = objProposal.Error.WarningMessage };
            try
            {
                if (objProposal.Message == "Success")
                {
                    #region Email Integration
                    ReportsController objReportController = new ReportsController();
                    var SendEmail = false;
                    for (int i = 0; i < objProposal.objMemberDetails.Count; i++)
                    {
                        for (int j = 0; j < objProposal.objMemberDetails[i].ObjUwDecision.lstUWNonMedicalDocument.Count; j++)
                        {
                            if (objProposal.objMemberDetails[i].ObjUwDecision.lstUWNonMedicalDocument[j].Status == "2370" && objProposal.objMemberDetails[i].ObjUwDecision.lstUWNonMedicalDocument[j].Document != "Age Proof")
                            {
                                SendEmail = true;
                            }
                        }
                        for (int j = 0; j < objProposal.objMemberDetails[i].ObjUwDecision.lstUWMedicalDocument.Count; j++)
                        {
                            if (objProposal.objMemberDetails[i].ObjUwDecision.lstUWMedicalDocument[j].Status == "2370")
                            {
                                SendEmail = true;
                            }
                        }
                    }
                    if (SendEmail)
                    {
                        objPolicyBusiness.SendDocumentsEmail(objProposal);
                    }
                    switch (objProposal.Decision)
                    {
                        case CrossCutting.CrossCuttingConstants.UWDecisionAccepted:
                        #region policy issuance triggers
                        PaymentModel objPaymentModel = new PaymentModel();
                        objPaymentModel.UserName = objProposal.CreatedBy;
                        objPaymentModel.ProposalNo = objProposal.ProposalNo;
                        objPaymentModel.QuoteNo = objProposal.QuoteNo;
                        PaymentBusiness objPaymentBusiness = new PaymentBusiness();
                        objPaymentModel = objPaymentBusiness.FetchProposals(objPaymentModel);

                        List<byte[]> lstBytes = new List<byte[]>();
                        AVOAIALifeEntities Context = new AVOAIALifeEntities();
                        try
                        {
                            if (objPaymentModel.lstPaymentItems.Count > 0)
                            {
                                int prdID = objPaymentModel.lstPaymentItems[0].ProductID;
                                var obj = Context.tblMasProductPlans.Where(a => a.ProductId == prdID).FirstOrDefault();
                                var ProductCode = Context.tblProducts.Where(a => a.ProductId == prdID).Select(a => a.ProductCode).FirstOrDefault();
                                if (obj != null)
                                {
                                    byte[] quoteBytes = objReportController.ReportQuotation(objPaymentModel.QuoteNo, obj.PlanCode, objPaymentModel.lstPaymentItems[0].PreferredLanguage);
                                    if (quoteBytes != null)
                                        lstBytes.Add(quoteBytes);
                                    byte[] a = objReportController.ReportForIllustrationPDF(objPaymentModel.QuoteNo, obj.PlanCode, objPaymentModel.lstPaymentItems[0].PreferredLanguage, objPaymentModel.ProposalNo);
                                    byte[] b = objReportController.ReportForPolicySchedule(objPaymentModel.ProposalNo, ProductCode, objPaymentModel.lstPaymentItems[0].PreferredLanguage);
                                    byte[] c = objReportController.ReportForCoveringLetter(objPaymentModel.ProposalNo, obj.PlanCode, objPaymentModel.lstPaymentItems[0].PreferredLanguage);
                                    List<byte[]> vs = new List<byte[]>();
                                    if (c != null)
                                        vs.Add(c);
                                    if (b != null)
                                        vs.Add(b);
                                    if (a != null)
                                        vs.Add(a);
                                    objPaymentModel.ByteArray2 = concatAndAddContent(vs);
                                    objPaymentModel.ByteArray3 = objReportController.ProposalReport(objPaymentModel.QuoteNo, ProductCode, objPaymentModel.lstPaymentItems[0].PreferredLanguage);
                                    if (objPaymentModel.ByteArray2 != null)
                                        lstBytes.Add(objPaymentModel.ByteArray2);
                                    if (objPaymentModel.ByteArray3 != null)
                                        lstBytes.Add(objPaymentModel.ByteArray3);
                                }
                            }
                            //objPolicyBusiness.EmailNotificationOnUWDecision(objProposal);
                            objPolicyBusiness.PostPolicyIssuanceTriggers(objPaymentModel);
                        }
                        catch (Exception e)
                        {
                            log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                            Logger.Error(e);
                        }
                        try
                        {
                            var CreatedBy = Context.tblPolicies.Where(a => a.ProposalNo == objPaymentModel.ProposalNo).Select(a => a.Createdby).FirstOrDefault();
                            var AgentCode = Context.tblUserDetails.Where(a => a.UserID.ToString() == CreatedBy).Select(a => a.LoginID).FirstOrDefault();
                            for (int i = 0; i < lstBytes.Count; i++)
                            {
                                LdmsDocuments documents = new LdmsDocuments();
                                documents.SourcePath = ConfigurationManager.AppSettings["DocumentUploadPath"];
                                documents.DocCode = i == 0 ? "PRD004" : (i == 1 ? "PRD009" : (i == 2 ? "PRD001" : ""));
                                documents.AgentCode = AgentCode;
                                documents.ProposalNo = objPaymentModel.ProposalNo;
                                documents.SourcePath = documents.SourcePath + @"\UW\" + documents.AgentCode + @"\" + documents.ProposalNo + @"\";
                                if (!Directory.Exists(documents.SourcePath))
                                {
                                    Directory.CreateDirectory(documents.SourcePath);
                                }
                                documents.SourcePath = documents.SourcePath + documents.DocCode + ".pdf";
                                System.IO.File.WriteAllBytes(documents.SourcePath, lstBytes[i]);
                                objPolicyBusiness.UploadDocumentsLDMS(documents);
                            }
                        }
                        catch (Exception ex)
                        {
                            log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                            Logger.Error(ex);
                        }
                        #endregion
                        break;
                        case CrossCutting.CrossCuttingConstants.UWDecisionOutStandingReq:
                        case CrossCutting.CrossCuttingConstants.UWDecisionAcceptwithloading:
                        case CrossCutting.CrossCuttingConstants.UWDecisionWithDrawn:
                            if (objProposal.Decision == CrossCutting.CrossCuttingConstants.UWDecisionOutStandingReq && !SendEmail)
                            {
                                objPolicyBusiness.EmailNotificationOnUWDecision(objProposal);
                            }
                            else if (objProposal.Decision != CrossCutting.CrossCuttingConstants.UWDecisionOutStandingReq)
                            {
                                objPolicyBusiness.EmailNotificationOnUWDecision(objProposal);
                            }
                            break;
                        case CrossCutting.CrossCuttingConstants.UWDecisionDecline:
                        case CrossCutting.CrossCuttingConstants.UWDecisionNotTaken:
                        case CrossCutting.CrossCuttingConstants.UWDecisionPostPone:
                        objProposal.ByteArray = objReportController.GenererateUWReports(objProposal.ProposalNo, "English", objProposal.Decision);
                        objPolicyBusiness.EmailNotificationOnUWDecision(objProposal);
                        break;
                        case CrossCuttingConstants.UWDecisionCounterOffer:
                            if(objProposal.IsClaGenerateQuote)
                            {
                                AIA.Life.Models.Opportunity.LifeQuote objQuote = new AIA.Life.Models.Opportunity.LifeQuote();
                                objQuote.QuoteNo = objProposal.QuoteNo;
                                AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
                                objQuote = objQuoteBusiness.FetchQuoteData(objQuote);
                                objQuote.IsForCounterOffer = true;
                                objQuote.ByteArray = objReportController.ReportQuotation(objQuote.QuoteNo, objQuote.objProductDetials.PlanCode, objQuote.objProductDetials.PreferredLangauage);
                                objQuote.ByteArray1 = objReportController.ReportForCLADocument(objQuote.QuoteNo, objProposal.ProposalNo);
                                objQuoteBusiness.SendEmailAndSMSNotificationOnQuoteCreation(objQuote);
                            }
                            
                            if (objProposal.IsCLALetterReq && objProposal.IsIllustartionReq)
                            {
                                objProposal.TemplateCode = "T012";
                                objProposal.ByteArray = objReportController.ReportForCLADocument(objProposal.QuoteNo, objProposal.ProposalNo);
                                objProposal.ProspectSignPath = objReportController.ReportForIllustrationPDF(objProposal.QuoteNo, objProposal.PlanCode, string.Empty, objProposal.ProposalNo);
                                objPolicyBusiness.EmailNotificationOnUWDecision(objProposal);
                            }
                            else if (objProposal.IsCLALetterReq == true && objProposal.IsIllustartionReq == false)
                            {
                                objProposal.TemplateCode = "T011";
                                objProposal.ByteArray = objReportController.ReportForCLADocument(objProposal.QuoteNo, objProposal.ProposalNo);
                                objPolicyBusiness.EmailNotificationOnUWDecision(objProposal);
                            }
                            break;
                    }

                    #endregion
                    #region SMS Integration
                    if (
                         objProposal.Decision == CrossCutting.CrossCuttingConstants.UWDecisionOutStandingReq ||
                        objProposal.Decision == CrossCutting.CrossCuttingConstants.UWDecisionDecline ||
                        objProposal.Decision == CrossCutting.CrossCuttingConstants.UWDecisionPostPone ||
                        objProposal.Decision == CrossCutting.CrossCuttingConstants.UWDecisionWithDrawn ||
                        objProposal.Decision == CrossCutting.CrossCuttingConstants.UWDecisionNotTaken ||
                        //objProposal.Decision == CrossCutting.CrossCuttingConstants.UWDecisionAccepted ||
                        objProposal.Decision == CrossCutting.CrossCuttingConstants.UWDecisionAcceptwithloading
                        )
                    {
                        objPolicyBusiness.SMSNotificationOnUWDecision(objProposal);
                    }


                    #endregion

                }
            }
            catch (Exception ex)
            {
                ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
                log4net.GlobalContext.Properties["ErrorCode"] = objProposal.Error.ErrorCode = Codes.GetErrorCode();
                Logger.Error(ex);
            }

            return Json(ObjResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerateQuoteCla(AIA.Life.Models.Policy.Policy objProposal)
        {
            objProposal.UserName = Username;
            objProposal.IsIntermSave = false;
            foreach (var Member in objProposal.objMemberDetails)
            {
                if (Member.ObjUwDecision.UWMedicalCode != null)
                {
                    var items = JsonConvert.DeserializeObject<List<string>>(Member.ObjUwDecision.UWMedicalCode);
                    List<string> SelectedMedicalCodes = new List<string>();
                    foreach (var item in items)
                    {
                        SelectedMedicalCodes.Add(item);
                    }
                    string MedicalCode = (string.Join(",", SelectedMedicalCodes.Select(x => x.ToString()).ToArray()));
                    Member.ObjUwDecision.UWMedicalCode = MedicalCode;
                }
            }
            
            objProposal = objPolicyBusiness.SaveBeforeClaGenerateQuote(objProposal);

            return RedirectToAction("ModifyQuote", new { QuoteNo = CrossCutting_EncryptDecrypt.Encrypt(objProposal.QuoteNo), IsForCounterOffer = true, RiskCommencementDate = objProposal.RiskCommencementDate });
        }
        public ActionResult CheckAgeChangeForMembers(Policy objProposal)
        {
            if (objProposal.Decision == "184")
            {
                CommonBusiness common = new CommonBusiness();
                LifeAssuredAge lifeAssuredAge = new LifeAssuredAge() { QuoteNo = objProposal.QuoteNo, Rcd = objProposal.RiskCommencementDate };
                lifeAssuredAge = common.CheckAgeChangeQuoteMembers(lifeAssuredAge);
                if (lifeAssuredAge.MainLifeAge)
                    objProposal.Error.WarningMessage = "The age of Main Life in the proposal is different from age in quotation – Do you want to opt for Counter Offer?";
                else if (lifeAssuredAge.SpouseAge)
                    objProposal.Error.WarningMessage = "The age of Spouse in the proposal is different from age in quotation – Do you want to opt for Counter Offer?";
                else if (lifeAssuredAge.Child1Age)
                    objProposal.Error.WarningMessage = "The age of Child1 in the proposal is different from age in quotation – Do you want to opt for Counter Offer?";
                else if (lifeAssuredAge.Child2Age)
                    objProposal.Error.WarningMessage = "The age of Child2 in the proposal is different from age in quotation – Do you want to opt for Counter Offer?";
                else if (lifeAssuredAge.Child3Age)
                    objProposal.Error.WarningMessage = "The age of Child3 in the proposal is different from age in quotation – Do you want to opt for Counter Offer?";
                else if (lifeAssuredAge.Child4Age)
                    objProposal.Error.WarningMessage = "The age of Child4 in the proposal is different from age in quotation – Do you want to opt for Counter Offer?";
                else if (lifeAssuredAge.Child5Age)
                    objProposal.Error.WarningMessage = "The age of Child5 in the proposal is different from age in quotation – Do you want to opt for Counter Offer?";
            }
            else
            {
                TempData["Proposal"] = objProposal;
                return RedirectToAction("SubmitUWRemarks", new { objProposal = objProposal});
            }
            if (string.IsNullOrEmpty(objProposal.Error.WarningMessage))
            {
                TempData["Proposal"] = objProposal;
                return RedirectToAction("SubmitUWRemarks", new { objProposal = objProposal });
            }
            else
            {
                var ObjResponse = new { Message = objProposal.Message, ProposalNo = objProposal.ProposalNo, Decision = objProposal.Decision, ErrorMessage = objProposal.Error.ErrorMessage, WarningMessage = objProposal.Error.WarningMessage };
                return Json(ObjResponse, JsonRequestBehavior.AllowGet);
            }
        }
        public static byte[] concatAndAddContent(List<byte[]> pdf)
        {
            byte[] all;

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document();

                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.SetPageSize(PageSize.A4);
                doc.Open();
                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;

                PdfReader reader;
                foreach (byte[] p in pdf)
                {
                    reader = new PdfReader(p);
                    int pages = reader.NumberOfPages;

                    // loop over document pages
                    for (int i = 1; i <= pages; i++)
                    {
                        doc.SetPageSize(PageSize.A4);
                        doc.NewPage();
                        page = writer.GetImportedPage(reader, i);
                        cb.AddTemplate(page, 0, 0);
                    }
                }

                doc.Close();
                all = ms.GetBuffer();
                ms.Flush();
                ms.Dispose();
            }
            return all;
        }

        /// <summary>
        /// To Derive policy level decision Based on MemberLevelDecision
        /// </summary>
        /// <param name="objProposal"></param>
        /// <returns></returns>

        public ActionResult DerivePolicyLevelDecision(List<MemberDecisions> MemberDecisions)
        {
            MemberLevelDecisions obj = new MemberLevelDecisions();
            obj.objDecisions = MemberDecisions;
            obj.UserName = Username;
            obj = objPolicyBusiness.DerivePolicyLevelDecision(obj);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Counter Offer Submit Case
        /// </summary>
        /// <param name="objpolicy"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult CounterOfferSubmit(AIA.Life.Models.Policy.Policy objpolicy)
        {
            objpolicy.UserName = Username;
            objpolicy = objPolicyBusiness.CounterOfferSubmit(objpolicy);
            if (objpolicy.Message == "Success")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public void GenerateReport()
        {
            Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(@"data source = issdbs01; database = AVOAIA; Uid = sa_policyjs; Pwd = iNube@123;");
            con.Open();
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_ProposalForm";
            List<DataSet> dslst = new List<DataSet>();
            DataSet ds = new DataSet();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dslst.Add(ds);
                ReportParameter[] parameters = new ReportParameter[1];

                byte[] bytes = GenerateRDLCReports(dslst, parameters, @"~/Reports/iNube Proposal Form.rdlc");
                RenderReports(bytes, "iNube Proposal Form");
            }
        }

        public byte[] GenerateRDLCReports(List<System.Data.DataSet> dsPayementStmt, ReportParameter[] parameters, string ReportPath)
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = Server.MapPath(ReportPath);
            //if (parameters != null)
            //{
            //    viewer.LocalReport.SetParameters(parameters);
            //}
            viewer.LocalReport.DataSources.Clear();
            int count = 0;
            foreach (DataSet item in dsPayementStmt)
            {
                count++;
                viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet" + count, item.Tables[0]));
            }
            viewer.LocalReport.Refresh();
            viewer.LocalReport.Refresh();
            byte[] bytes;
            try
            {
                bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                return bytes;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public void RenderReports(byte[] bytes, string fileName)
        {
            if (bytes != null)
            {
                Response.Clear();
                Response.ClearContent();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".pdf");
                Response.OutputStream.Write(bytes, 0, bytes.Length);
                Response.Flush();
                try
                {
                    Response.End();
                }
                catch
                {

                }
            }
        }

        public string SigJsonToImage(string json)
        {
            string filename = "signature" + "UniqueText" + ".png";
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "signatureimage";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = System.IO.Path.Combine(path, filename);
            using (FileStream fs = new FileStream(filepath, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(json);
                    bw.Write(data);
                    bw.Close();
                }
            }
            return filename;
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            try
            {
                
                // Html = RenderPartialViewToString("~/Views/Policy/PartialIllustration.cshtml", grid);
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
            catch (Exception ex)
            {
                return "Error";
            }
        }

        public ActionResult GetAddress(string term)
        {

            List<string> Address = new List<string>();
            AIA.Life.Models.Common.Address address = new Life.Models.Common.Address();
            #region Call API
            var address1 = objPolicyBusiness.FillAddressMasterList();
            #endregion

            foreach (var list in address1.LstPincode)
            {
                Address.Add(list.Value);
            }
            var result = Address.Where(l => l.Contains(term.ToUpper()));
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        // Test
        public ActionResult TestService()
        {
            #region Call API
            AIA.Life.Models.Integration.Services.ProposalInfo objProposalInfo = new AIA.Life.Models.Integration.Services.ProposalInfo();
            objProposalInfo.PolicyNo = "12345";
            objProposalInfo.ProposalNo = "12345";
            objProposalInfo = objPolicyBusiness.UpdateUWInfo(objProposalInfo);
            #endregion

            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadPreviousInsuranceGrid(AIA.Life.Models.Opportunity.QuoteList objQuoteList)
        {
            AIA.Life.Models.Opportunity.LifeQuote objQuote = new LifeQuote();
            objQuote.objProspect = objQuoteList.objProspect;
            objQuote.ObjQuotationPreviousInsurance = objQuoteList.ObjQuotationPreviousInsurance;
            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            objQuote = objQuoteBusiness.LoadPreviousInsuranceGrid(objQuote);
            if (objQuote.objPreviousInsuranceList.Count == 0)
            {
                objQuote.objPreviousInsuranceList.Add(new PreviousInsuranceList());
            }
            //else
            //{
            //    int NoOfOnGoingProposalWithAIA = Convert.ToInt32(objQuote.ObjQuotationPreviousInsurance.NoOfOnGoingProposalWithAIA);
            //    List<PreviousInsuranceList> lstPrevious = new List<PreviousInsuranceList>();
            //    int j = 0;
            //    for (int i = NoOfOnGoingProposalWithAIA; i > 0; i--)
            //    {
            //        lstPrevious.Add(objQuote.objPreviousInsuranceList[j]);
            //        j++;
            //        if (objQuote.objPreviousInsuranceList.Count < NoOfOnGoingProposalWithAIA)
            //        {
            //            objQuote.objPreviousInsuranceList.Add(new PreviousInsuranceList());
            //        }
            //    }
            //    int NoOfPreviousPolicyWithAIA = Convert.ToInt32(objQuote.ObjQuotationPreviousInsurance.NoOfPreviousPolicyWithAIA);


            //    for (int i = NoOfPreviousPolicyWithAIA; i > 0; i--)
            //    {

            //        lstPrevious.Add(objQuote.objPreviousInsuranceList[j]);
            //        j++;
            //        if (objQuote.objPreviousInsuranceList.Count < NoOfPreviousPolicyWithAIA)
            //        {
            //            objQuote.objPreviousInsuranceList.Add(new PreviousInsuranceList());
            //        }
            //    }
            //    objQuote.objPreviousInsuranceList = new List<PreviousInsuranceList>();
            //    objQuote.objPreviousInsuranceList.AddRange(lstPrevious);
            //}



            return PartialView("~/Views/Prospect/_PartialPrevoiusInsuranceDetailsGrid.cshtml", objQuote);
        }

        public ActionResult GetPlanCode(int plan)
        {
            try
            {
                QuoteList objQuoteList = new QuoteList();
                AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
                objQuoteList = objProspectBusiness.GetPlanCode(plan.ToString());
                if (objQuoteList.ObjLifeQuote.objProductDetials.PlanCode != null)
                {
                    return Json(objQuoteList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(objQuoteList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception Ex)
            {
                return View("Error");
            }
        }

        public ActionResult GetSAM(int plan, int Age)
        {
            try
            {
                LifeQuote objLifeQuote = new LifeQuote();
                objLifeQuote.objProspect.AgeNextBdy = Age;
                objLifeQuote.objProductDetials.Variant = Convert.ToString(plan);
                AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
                objLifeQuote = objProspectBusiness.GetSAM(objLifeQuote);
                if (objLifeQuote.lstSAM != null)
                {
                    return Json(objLifeQuote.lstSAM, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(objLifeQuote.lstSAM, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception Ex)
            {
                return View("Error");
            }
        }

        public ActionResult GetVariant(string Plan)
        {
            try
            {
                QuoteList objQuoteList = new QuoteList();
                if (Plan != null && Plan != "")
                {
                    AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
                    objQuoteList = objProspectBusiness.GetVariant(Plan.ToString());
                }
                if (objQuoteList.ObjLifeQuote.objProductDetials.PlanCode != null)
                {
                    return Json(objQuoteList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(objQuoteList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception Ex)
            {
                return View("Error");
            }
        }

        public ActionResult GetReasons(string Decision)
        {
            try
            {
                QuoteList objQuoteList = new QuoteList();
                if (Decision != string.Empty)
                {
                    AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
                    objQuoteList = objProspectBusiness.GetReason(Decision);
                }
                if (objQuoteList.objListReason != null)
                {
                    return Json(objQuoteList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(objQuoteList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception Ex)
            {
                return View("Error");
            }
        }

        public ActionResult GetChildDetails(QuoteList objQuoteList)
        {

            LifeQuote objLifeQuote = new LifeQuote();
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objLifeQuote = objProspectBusiness.LoadProspectMaster(objLifeQuote);

            objQuoteList.objListQuote[objQuoteList.SelectedQuote].objProspect.lstGender = objLifeQuote.objProspect.lstGender;

            int NoOfChild = Convert.ToInt32(objQuoteList.objListQuote[objQuoteList.SelectedQuote].NoofChilds);
            for (int i = NoOfChild; i > 0; i--)
            {

                objQuoteList.objListQuote[objQuoteList.SelectedQuote].objChildDetials.Add(new ChildDetails());

            }
            return PartialView("~/Views/Prospect/_PartialChildDetails.cshtml", objQuoteList);
        }

        public ActionResult DeleteChildDetails(QuoteList objQuoteList)
        {
            ModelState.Clear();
            LifeQuote objLifeQuote = new LifeQuote();
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objLifeQuote = objProspectBusiness.LoadProspectMaster(objLifeQuote);
            
            objQuoteList.objListQuote[objQuoteList.SelectedQuote].objProspect.lstGender = objLifeQuote.objProspect.lstGender;
            
            int NoOfChild = Convert.ToInt32(objQuoteList.objListQuote[objQuoteList.SelectedQuote].NoofChilds);
            int deleteIndex = Convert.ToInt32(objQuoteList.objListQuote[objQuoteList.SelectedQuote].ChildDeleteIndex);
            List<ChildDetails> childDetails = new List<ChildDetails>();
            for (int i = 0; i < NoOfChild; i++)
            {
                if (i != deleteIndex)
                {
                    childDetails.Add(objQuoteList.objListQuote[objQuoteList.SelectedQuote].objChildDetials[i]);
                }
            }
            objQuoteList.objListQuote[objQuoteList.SelectedQuote].NoofChilds = (NoOfChild - 1).ToString();
            objQuoteList.objListQuote[objQuoteList.SelectedQuote].objChildDetials = new List<ChildDetails>();
            objQuoteList.objListQuote[objQuoteList.SelectedQuote].objChildDetials.AddRange(childDetails);
            return PartialView("~/Views/Prospect/_PartialChildDetails.cshtml", objQuoteList);
        }

        /// <summary>
        /// To Display items that are assigned to Under Writer 
        /// </summary>
        /// <returns></returns>
        public ActionResult PartialUWPoolGrid()
        {
            try
            {
                UWInbox obj = new UWInbox();
                obj.UserName = Username;
                TempData["Load"] = "FirstTime";
              
                obj = objPolicyBusiness.FetchUWProposals(obj);
                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                var grid = (AjaxGrid<InboxProposals>)new AjaxGridFactory().CreateAjaxGrid((obj.LstProposals).AsQueryable(), 1, false,2);
                ViewBag.count = obj.LstProposals.Count();
                return PartialView("~/Views/Policy/UWInbox.cshtml", grid);
            }
            catch (Exception e)
            {
                return View("Error");
            }

        }

        /// <summary>
        /// To Display Processed cases by Under Writer
        /// </summary>
        /// <returns></returns>
        public ActionResult PartialUWProcessedPoolGrid(string Decision)
        {
            try
            {
                UWInbox obj = new UWInbox();
                obj.UserName = Username;
                TempData["Load"] = "FirstTime";
                ViewBag.decision = Decision;
                obj.Message = "Processed";
                obj = objPolicyBusiness.FetchUWProposals(obj);
                if (!string.IsNullOrEmpty(Decision))
                {
                    obj.LstProposals = obj.LstProposals.Where(a => a.Decision == Decision).ToList();
                }
                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                var grid = (AjaxGrid<InboxProposals>)new AjaxGridFactory().CreateAjaxGrid((obj.LstProposals).AsQueryable(), 1, false,2);
                ViewBag.count = obj.LstProposals.Count();
                
                return PartialView("~/Views/Policy/PartialUWProcessedInbox.cshtml", grid);
            }
            catch (Exception e)
            {
                return View("Error");
            }

        }

        /// <summary>
        /// To Display Next Page in processed pool of Under Writer
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult ProcessedPoolGridNextPage(int? page, string decision)
        {
            try
            {

                UWInbox obj = new UWInbox();
                obj.UserName = Username;
                obj.Message = "Processed";
                obj = objPolicyBusiness.FetchUWProposals(obj);
                if (!string.IsNullOrEmpty(decision))
                {
                    obj.LstProposals = obj.LstProposals.Where(a => a.Decision == decision).ToList();
                }
                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                var grid = aj.CreateAjaxGrid(obj.LstProposals.AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, 2);

                return Json(new
                {
                    Html = RenderPartialViewToString("~/Views/Policy/PartialUWProcessedInbox.cshtml", grid),
                    HasItems = grid.HasItems
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        /// <summary>
        /// To Dispay next Page  in  UW pool 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult SearchUWPoolDetails(int? page)
        {
            try
            {

                UWInbox obj = new UWInbox();
                obj.UserName = Username;
                obj = objPolicyBusiness.FetchUWProposals(obj);
                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                var grid = aj.CreateAjaxGrid(obj.LstProposals.AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, 2);

                return Json(new
                {
                    Html = RenderPartialViewToString("~/Views/Policy/UWInbox.cshtml", grid),
                    HasItems = grid.HasItems
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        /// <summary>
        /// To Display Existing Rider information Against each Member To Under Writer
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public ActionResult FetchLoadigInfo(decimal MemberID)
        {

            MemberDetails objMemberDetails = new MemberDetails();
            objMemberDetails.MemberID = MemberID;
            objMemberDetails = objPolicyBusiness.FetchLoadigInfo(objMemberDetails);
            return PartialView("~/Views/Policy/_PartialMemberLoadingInfo.cshtml", objMemberDetails);
        }

        /// <summary>
        /// To Update Status Once Decision has been Taken by Under Writer
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="DeviationID"></param>
        /// <returns></returns>
        public ActionResult UpdateMemberLevelDeviation(string Status, int DeviationID)
        {
            UWMemberLevelDeviationStatus objstatus = new UWMemberLevelDeviationStatus();
            objstatus.UserName = Username;
            objstatus.Status = Status;
            objstatus.MemberDeviationID = DeviationID;
            objstatus = objPolicyBusiness.UpdateMemberLevelDeviation(objstatus);

            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeletePolicyDocument(string MemberType, string DocumentType, string DocID)
        {
            DeletePolicyDocuments objPolicyDocuments = new DeletePolicyDocuments();
            objPolicyDocuments.UserName = Username;
            objPolicyDocuments.MemberType = MemberType;
            objPolicyDocuments.DocumentName = DocumentType;
            objPolicyDocuments.DocID = Convert.ToInt32(DocID);
            objPolicyDocuments = objPolicyBusiness.DeletePolicyDocument(objPolicyDocuments);
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteDocumentLinkPolicyDocument(string MemberType, string DocumentType, decimal PolicyIDDocument)
        {
            DeletePolicyDocuments objPolicyDocuments = new DeletePolicyDocuments();

            objPolicyDocuments.UserName = Username;
            objPolicyDocuments.MemberType = MemberType;
            objPolicyDocuments.DocumentName = DocumentType;
            objPolicyDocuments.DocumentPolicyID = PolicyIDDocument;
            objPolicyDocuments = objPolicyBusiness.DeleteDocumentLinkPolicyDocument(objPolicyDocuments);
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// To Calculate Extra Premium on applied loading On Each Rider
        /// </summary>
        /// <param name="objMemberDetails"></param>
        /// <returns></returns>
        public ActionResult CalculateLoadingPremium(MemberDetails objMemberDetails)
        {
            Policy objPolicy = new Policy();
            objPolicy.UserName = Username;
            objPolicy.objMemberDetails = new List<MemberDetails>();
            objPolicy.objMemberDetails.Add(objMemberDetails);
            objPolicy = objPolicyBusiness.CalculateLoadingPremium(objPolicy);
            if (objPolicy.Message == "Success")
            {
                MemberDetails objMemberDetailsResult = objPolicy.objMemberDetails[0];
                return PartialView("~/Views/Policy/_PartialMemberLoadingInfo.cshtml", objMemberDetailsResult);
            }
            else
            {
                return PartialView("~/Views/Policy/_PartialMemberLoadingInfo.cshtml", objMemberDetails);
            }
        }

        /// <summary>
        /// To Save  Member level Loading Info of Riders
        /// </summary>
        /// <param name="objMemberDetails"></param>
        /// <returns></returns>
        public ActionResult SaveLoadingDetails(MemberDetails objMemberDetails)
        {
            Policy objPolicy = new Policy();
            objPolicy.UserName = Username;
            objPolicy.objMemberDetails = new List<MemberDetails>();
            objPolicy.objMemberDetails.Add(objMemberDetails);
            objPolicy = objPolicyBusiness.SaveLoadingDetails(objPolicy);
            
            return Json(objPolicy.Message, JsonRequestBehavior.AllowGet);
        }

        public string GenerateMedicalLetter(string QuoteNo, string ProductCode, string MedicalLabName)
        {

            AIA.Life.Models.Policy.Policy objpolicy = new Policy();
            objpolicy.QuoteNo = QuoteNo;
            objpolicy.ProductCode = ProductCode;
            objpolicy.MedicalLabName = MedicalLabName;
            objpolicy.UserName = System.Web.HttpContext.Current.User.Identity.Name;

            ReportsController objReportController = new ReportsController();
            objpolicy.ByteArray = objReportController.PendingRequirementsProposalMedicalLetter(QuoteNo, ProductCode, MedicalLabName);

            objpolicy =objPolicyBusiness.SendMedicalLetterMail(objpolicy);


            //byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath(@"~\Uploads\SampleLabLetter.pdf"));
            //return File(bytes, "application/pdf", "LabLetter.pdf");


            return objpolicy.Message;
        }

        public int GetChildAge(string Dob)
        {
            DateTime Now = DateTime.Now;
            // var dob= Dob.ToString("dd-MM-yyyy hh:mm:ss tt");
            DateTime DateOfBirth = Convert.ToDateTime(Dob);

            int Days = (DateTime.Now - DateOfBirth).Days;
            return Days;
        }

        public JsonResult InvokeILClientCreation(AIA.Life.Models.Policy.Policy objPolicy)
        {
            objPolicy.UserName = Username;
            objPolicy = objPolicyBusiness.InvokeILClientCreation(objPolicy);
            return Json(objPolicy.objMemberDetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult InvokeILClientCreationForBeneficiary(AIA.Life.Models.Policy.Policy objPolicy)
        {
            objPolicy.UserName = Username;
            objPolicy = objPolicyBusiness.InvokeILClientCreationForBeneficiary(objPolicy);
            return Json(objPolicy.objNomineeDetails, JsonRequestBehavior.AllowGet);
        }
    }

}