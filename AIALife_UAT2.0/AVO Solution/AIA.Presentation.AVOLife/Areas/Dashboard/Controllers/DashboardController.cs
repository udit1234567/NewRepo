using AIA.Life.Business.DashBoard;
using AIA.Life.Models.DashBoard;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AIA.Presentation.AVOLife.Areas.Dashboard.Controllers
{
    //[Authorize]
    public class DashBoardController : Controller
    {
        private string Username = string.Empty;
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        public DashBoardController()
        {
            Username = System.Web.HttpContext.Current.User.Identity.Name;
        }
        public ActionResult ReportViewer(int ReportID)
        {
            GraphDetails objGraphDetails = new GraphDetails();
            return View(objGraphDetails);
        }
        public ActionResult PartialBarChart(int ReportID)
        {
            ViewBag.UserName = CrossCutting.CrossCutting_EncryptDecrypt.Encrypt(Username);
            return View("_PartialBarChart",ReportID);
        }
        public ActionResult PartialFunnelChart(int ReportID)
        {
            ViewBag.UserName = CrossCutting.CrossCutting_EncryptDecrypt.Encrypt(Username);
            return View("_PartialFunnelChart", ReportID);
        }
        public ActionResult PartialPieChart(int ReportID)
        {
            ViewBag.UserName = CrossCutting.CrossCutting_EncryptDecrypt.Encrypt(Username);
            return View("_PartialPieChart", ReportID);
        }
        public JsonResult MakeChart(int ReportID, string UserName)
        {
            if (!string.IsNullOrEmpty(UserName))
                Username = CrossCutting.CrossCutting_EncryptDecrypt.Decrypt(UserName);
            GraphDetails objGraphDetails = new GraphDetails();
            DashBoardBL dashBoard = new DashBoardBL();
            objGraphDetails.ReportID = ReportID;
            objGraphDetails.UserName = Username;
            objGraphDetails = dashBoard.GenerateDashboardData(objGraphDetails);

            return Json(objGraphDetails, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult MakePieChart(int ReportID)
        //{
        //    GraphDetails objGraphDetails = new GraphDetails();
        //    objGraphDetails.GraphType = "PIE";
        //    objGraphDetails.IsDrillDown = true;
        //    #region Call API
        //    objGraphDetails.ReportID = ReportID;
        //    objGraphDetails = objDashboardLogic.GenerateDashboardData(objGraphDetails);
        //    #endregion
        //    return View("~/Views/DashBoard/PartialPieChart.cshtml", objGraphDetails);
        //}
        //public ActionResult MakeBarChart(int ReportID)
        //{
        //    GraphDetails objGraphDetails = new GraphDetails();
        //    objGraphDetails.GraphType = "Column";
        //    objGraphDetails.IsDrillDown = true;
        //    #region Call API
        //    objGraphDetails.ReportID = ReportID;
        //    objGraphDetails = objDashboardLogic.GenerateDashboardData(objGraphDetails);
        //    #endregion
        //    return View("~/Views/DashBoard/PartialColumnChart.cshtml", objGraphDetails);
        //}
    }
}