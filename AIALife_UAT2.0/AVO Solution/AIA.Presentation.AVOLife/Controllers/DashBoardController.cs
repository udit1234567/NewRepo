using Grid.Mvc.Ajax.GridExtensions;
using AIA.Life.Models.Common;
using AIA.Life.Models.DashBoard;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AIA.Life.Business.DashBoard;

namespace AIA.Presentation.AVOLife.Controllers
{
    [Authorize]
    [ErrorLogging]
    public class DashBoardController : BaseController
    {
        DashBoardBL objDashboardLogic = new DashBoardBL();
        // GET: DashBoard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UnderWriter()
        {
            return View();
        }
        public ActionResult SeeSaw()
        {
            return View();
        }
        public ActionResult PieChart()
        {
            return View("~/Views/DashBoard/PartialPieChart.cshtml", 1);
        }
        public ActionResult MakePieChart(int ReportID)
        {
            GraphDetails objGraphDetails = new GraphDetails();
            objGraphDetails.GraphType = "PIE";
            objGraphDetails.IsDrillDown = true;
            #region Call API
            objGraphDetails.ReportID = ReportID;
            objGraphDetails = objDashboardLogic.GenerateDashboardData(objGraphDetails);
            #endregion
            return View("~/Views/DashBoard/PartialPieChart.cshtml", objGraphDetails);
        }
        public ActionResult MakeBarChart(int ReportID)
        {
            GraphDetails objGraphDetails = new GraphDetails();
            objGraphDetails.GraphType = "Column";
            objGraphDetails.IsDrillDown = true;
            #region Call API
            objGraphDetails.ReportID = ReportID;
            objGraphDetails = objDashboardLogic.GenerateDashboardData(objGraphDetails);
            #endregion
            return View("~/Views/DashBoard/PartialColumnChart.cshtml", objGraphDetails);
        }
        public ActionResult DemoGraph(string ReportType)
        {
            GraphDetails obj = new GraphDetails();
            obj.GraphType = ReportType;
            return View(obj);
        }
        public ActionResult DemoGraphGI(string ReportType)
        {
            GraphDetails obj = new GraphDetails();
            obj.GraphType = ReportType;
            return View(obj);
        }
        public ActionResult myGoalList()
        {
            return View();
        }
        public ActionResult GetmyActions()
        {
            return View();
        }
        public ActionResult MyCalendar()
        {
            GraphDetails obj = new GraphDetails();
            return View(obj);
        }
        public ActionResult RenewalPopup()
        {
            return PartialView("RenewalPopup");
        }
        public ActionResult GetAppointmentGrid(GraphDetails objAppointdetails)
        {
            try
            {
                AjaxGrid<MyCalendar> usergrid = null;
                //objAppointdetails = WebApiLogic.GetPostComplexTypeToAPI<GraphDetails>(objAppointdetails, "LoadAppointmentData", "DashBoardLifeApi");
                //var GridUserData = objAppointdetails.appointmentList.AsQueryable();
                ViewBag.Details = objAppointdetails;
                TempData["Load"] = "FirstTime";
               // usergrid = (AjaxGrid<MyCalendar>)new AjaxGridFactory().CreateAjaxGrid("", 1, false);
                return PartialView("~/Views/DashBoard/_AppointmentGrid.cshtml", usergrid);
            }
            catch (Exception Ex)
            {
                return View("Error");
            }
        }
        public ActionResult saveAppointmentfiles(int AppointmentID, string objLstDoc, int Req = 0)
        {
            try
            {
                List<DocumentUploadFile> LstDocumentUpload = new List<DocumentUploadFile>();
                List<DocumentUploadFile> RequestDoc = new List<DocumentUploadFile>();
                string FileName = "";
                Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
                RequestDoc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DocumentUploadFile>>(objLstDoc, settings);
                string StrWriteException = string.Empty;
                for (var i = 0; i < RequestDoc.Count(); i++)
                {
                    if (string.IsNullOrEmpty(RequestDoc[i].FilePath))
                    {
                        if (Request.Files[RequestDoc[i].Key] != null)
                        {
                            string Key = RequestDoc[i].Key;
                            DocumentUploadFile objUpload = new DocumentUploadFile();
                            Random rnd = new Random();
                            int num = rnd.Next(1, 9999);
                            FileName = Request.Files[Key].FileName;
                            objUpload.FileName = RequestDoc[i].DocName;
                            FileName = FileName.Replace(".", "_" + num.ToString() + ".");
                            var FileStreamBytes = Request.Files[Key].InputStream;
                            HttpPostedFileBase file = Request.Files[Key];
                            if (file.ContentLength <= 1000000)
                            {
                                var ext = Path.GetExtension(FileName);
                                DocumentUpload(file, FileName);
                            }
                            else
                            {
                                StrWriteException = "File Size Exceeded";
                                var objResult = new { StrWriteException = StrWriteException };
                                return Json(objResult, JsonRequestBehavior.AllowGet);
                            }
                            string directryPath = Server.MapPath("AppointmentID_" + AppointmentID);
                            FileName = Path.Combine(directryPath, FileName);
                            objUpload.FilePath = FileName;
                            objUpload.ItemType = RequestDoc[i].ItemType;
                            objUpload.Index = i;
                            LstDocumentUpload.Add(objUpload);
                        }
                    }
                    else
                    {
                        DocumentUploadFile objUpload = new DocumentUploadFile();
                        objUpload.FilePath = RequestDoc[i].FilePath;
                        objUpload.FileName = RequestDoc[i].DocName;
                        objUpload.ItemType = RequestDoc[i].ItemType;
                        objUpload.Index = i;
                        LstDocumentUpload.Add(objUpload);
                    }

                }
                string JsonData = JsonConvert.SerializeObject(LstDocumentUpload);
                ViewBag.UploadStatus = JsonData;
                var objRes = new { DocData = JsonData, StrWriteException = StrWriteException };
                return Json(objRes, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var objRes = new { StrWriteException = "Some error occured" };
                return Json(objRes, JsonRequestBehavior.AllowGet);
            }
        }
        public void DocumentUpload(HttpPostedFileBase file, string FileName = null, string strUser = null)
        {
            try
            {
                if (file != null)
                {
                    {
                        string directryPath = Server.MapPath("AppointmentID_" + strUser);
                        if (!Directory.Exists(directryPath))
                        {
                            Directory.CreateDirectory(directryPath);
                        }
                        var fileName = Path.GetFileName(file.FileName);

                        if (FileName != null)
                            fileName = strUser + "_" + FileName;
                        var filename = Path.Combine(directryPath, fileName);
                        file.SaveAs(filename);
                        System.IO.File.Copy(filename, ConfigurationManager.AppSettings["FileUpload"] + "\\" + strUser + "_" + fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator.";
            }
        }
        public ActionResult myProfile()
        {
            return PartialView("_getmyProfile");
        }
        public ActionResult retriggerMail()
        {
            return PartialView("_retriggerMail");
        }
        public ActionResult premiumCalculator()
        {
            return PartialView("_premiumCalculator");
        }
        public ActionResult locator()
        {
            return PartialView("_locator");
        }
        public ActionResult statusEnquiry()
        {
            return PartialView("_statusEnquiry");
        }
    }
}