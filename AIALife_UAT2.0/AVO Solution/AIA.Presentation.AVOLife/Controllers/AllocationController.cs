using AIA.Life.Business.Allocation;
using AIA.Life.Models.Allocation;
using AIA.Life.Repository.AIAEntity;
using Grid.Mvc.Ajax.GridExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AIA.Presentation.AVOLife.Controllers
{
    [Authorize]
    [ErrorLogging]
    public class AllocationController : BaseController
    {
        private string Username = string.Empty;
        AllocationBusiness objAllocationBusiness = new AllocationBusiness();
        public AllocationController()
        {
            Username = System.Web.HttpContext.Current.User.Identity.Name;
        }
        // GET: Allocation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllocationDetails()
        {
            AIA.Life.Models.Allocation.AllocationModel objAllocationModel = new Life.Models.Allocation.AllocationModel();
            objAllocationModel = objAllocationBusiness.LoadAllocationDetails(objAllocationModel);
            return PartialView("~/Views/Allocation/_PartialAllocationScreen.cshtml", objAllocationModel);
        }
        public ActionResult ManualAllocationDetails()
        {
            AIA.Life.Models.Allocation.ManualAllocation objAllocationModel = new Life.Models.Allocation.ManualAllocation();
            objAllocationModel = objAllocationBusiness.ManualAllocationDetails(objAllocationModel);
            TempData["Load"] = "FirstTime";

            ViewBag.UWDetails = objAllocationModel.LstUWName;
            return View("~/Views/Allocation/_PartialManualAllocationDetails.cshtml", objAllocationModel);
        }
        public ActionResult ManualAllocationGrid()
        {
            AIA.Life.Models.Allocation.ManualAllocation objAllocationModel = new Life.Models.Allocation.ManualAllocation();
            objAllocationModel = objAllocationBusiness.ManualAllocationDetails(objAllocationModel); ;
            TempData["Load"] = "FirstTime";
            ViewBag.AllocationCurrentPage = 1;
            ViewBag.AllocateUWDetails = objAllocationModel.LstUWName;
            var Data = (AjaxGrid<AllocationProposals>)new AjaxGridFactory().CreateAjaxGrid((objAllocationModel.objLstAllocationProposals).AsQueryable(), 1, false,2);         
            return PartialView("~/Views/Allocation/_PartialAllocateGrid.cshtml", Data);
        }
        public ActionResult ManualResetGrid()
        {
            AIA.Life.Models.Allocation.ManualAllocation objAllocationModel = new Life.Models.Allocation.ManualAllocation();
            objAllocationModel = objAllocationBusiness.ManualAllocationDetails(objAllocationModel);
            TempData["Load"] = "FirstTime";
            ViewBag.ResetCurrentPage = 1;
            ViewBag.UWDetails = objAllocationModel.LstUWName;
           var Data = (AjaxGrid<AllocationProposals>)new AjaxGridFactory().CreateAjaxGrid((objAllocationModel.objLstResetProposals).AsQueryable(), 1, false,2);
            return PartialView("~/Views/Allocation/_PartialResetGrid.cshtml", Data);
        }
        public ActionResult LoadNxtPageForManualAllocation(int? Page)
        {
            try
            {
                
                AIA.Life.Models.Allocation.ManualAllocation objAllocationModel = new Life.Models.Allocation.ManualAllocation();
                objAllocationModel.UserName = Username;

                objAllocationModel = objAllocationBusiness.ManualAllocationDetails(objAllocationModel);
                ViewBag.AllocateUWDetails = objAllocationModel.LstUWName;
                
                ViewBag.AllocationCurrentPage  = Convert.ToInt32( Page);
                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                var grid = aj.CreateAjaxGrid(objAllocationModel.objLstAllocationProposals.AsQueryable(), Page.HasValue ? Page.Value : 1, Page.HasValue, 2);
                return Json(new
                {
                    Html = RenderPartialViewToString("~/Views/Allocation/_PartialAllocateGrid.cshtml", grid),
                    HasItems = grid.HasItems
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        public ActionResult LoadNxtPageForManualProposalReset(int? Page)
        {
            try
            {
                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                AIA.Life.Models.Allocation.ManualAllocation objAllocationModel = new Life.Models.Allocation.ManualAllocation();
                objAllocationModel.UserName = Username;
                objAllocationModel = objAllocationBusiness.ManualAllocationDetails(objAllocationModel);
                var grid = aj.CreateAjaxGrid(objAllocationModel.objLstResetProposals.AsQueryable(), Page.HasValue ? Page.Value : 1, Page.HasValue, 2);
                ViewBag.ResetCurrentPage = Convert.ToInt32(Page);
                ViewBag.UWDetails = objAllocationModel.LstUWName;
                return Json(new
                {
                    Html = RenderPartialViewToString("~/Views/Allocation/_PartialResetGrid.cshtml", grid),
                    HasItems = grid.HasItems
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            try
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
            catch (Exception ex)
            {
                return "Error";
            }
        }
        public ActionResult SaveAllocation(AllocationModel objAllocationModel)
        {
            objAllocationModel = objAllocationBusiness.SaveAllocation(objAllocationModel);
            return PartialView("~/Views/Allocation/PartialAllocationSummary.cshtml", objAllocationModel);
        }

        public ActionResult ResetAllocation(AllocationModel objAllocationModel)
        {
            objAllocationModel = objAllocationBusiness.ResetAllocation(objAllocationModel);
            return Json(objAllocationModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveManualAllocation(string Selected)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            ManualAllocation objAllocationModel = new ManualAllocation();
            objAllocationModel.objLstAllocationProposals = new List<AllocationProposals>();

            var SelectedArray = Selected.Split(',');
            for (int i = 0; i < SelectedArray.Count(); i++)
            {
                var Combined = SelectedArray[i].Split('-');
                var ProposalNo = Combined[0]+ "-" + Combined[1];
                var LoginID = Combined[2];
                var UserID = Context.tblUserDetails.Where(a => a.LoginID == LoginID).Select(a => a.UserID).FirstOrDefault();
                AllocationProposals obj = new AllocationProposals();
                obj.ISSelected = true;
                obj.ProposalNo = ProposalNo;
                obj.AssignTo = UserID.ToString();
                objAllocationModel.objLstAllocationProposals.Add(obj);
               

            }
           
            objAllocationModel = objAllocationBusiness.SaveManualAllocation(objAllocationModel);

            return Json(objAllocationModel.Message, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ResetManualAllocation(string Selected)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            ManualAllocation objManualAllocation = new ManualAllocation();
            objManualAllocation.objLstResetProposals = new List<AllocationProposals>();

            var SelectedArray = Selected.Split(',');
            for (int i = 0; i < SelectedArray.Count(); i++)
            {
                var Combined = SelectedArray[i].Split('-');
                var ProposalNo = Combined[0];
                var LoginID = Combined[1];
                var UserID = Context.tblUserDetails.Where(a => a.LoginID == LoginID).Select(a => a.UserID).FirstOrDefault();
                AllocationProposals obj = new AllocationProposals();
                obj.ISSelected = true;
                obj.ProposalNo = ProposalNo;
                obj.AssignTo = UserID.ToString();
                objManualAllocation.objLstResetProposals.Add(obj);


            }
            objManualAllocation = objAllocationBusiness.ResetManualAllocation(objManualAllocation);
            return Json(objManualAllocation.Message, JsonRequestBehavior.AllowGet);
        }

    }
}