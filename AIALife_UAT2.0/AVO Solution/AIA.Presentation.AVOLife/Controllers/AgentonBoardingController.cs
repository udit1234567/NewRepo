using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AIA.Life.Models.AgentonBoarding;
using AIA.Life.Models.Common;
using AIA.Presentation.AVOLife.Models;
using Grid.Mvc.Ajax.GridExtensions;
using AIA.Life.Repository.AIAEntity;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Web.Hosting;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;
using AIA.Life.Business.AgentonBoarding;

namespace AIA.Presentation.AVOLife.Controllers
{
    [Authorize]
    [ErrorLogging]
    public class AgentonBoardingController : BaseController
    {
        string Username;
        string RoleID;
        string userID;
        string RoleName;
        private ApplicationUserManager _userManager;
        AVOAIALifeEntities Context = new AVOAIALifeEntities();
        AgentonBoardingBusiness objAgentonBoardingLogic = new AgentonBoardingBusiness();
        public AgentonBoardingController()
        {
            Username = System.Web.HttpContext.Current.User.Identity.Name;
            userID = System.Web.HttpContext.Current.User.Identity.GetUserId();
            AspNetUser selectedUser = Context.AspNetUsers.FirstOrDefault(u => u.UserName == Username);
            if (selectedUser != null)
            {
                List<AspNetRole> selectedUsersRoles = selectedUser.AspNetRoles.ToList();
                foreach (var item in selectedUsersRoles)
                {
                    RoleName = item.Name;
                    RoleID = item.Id;
                }
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
        public ActionResult FetchDateMonth(string NIC)
        {
            try
            {
                string NoofDays = null;
                string Year = null;
                string day = null;
                string month = null;
                string checkdays = null;
                if (NIC.Length == 12)
                {
                    NoofDays = NIC.Substring(4, 3);
                    Year = NIC.Substring(0, 4);
                }
                else if (NIC.Length == 10)
                {
                    NoofDays = NIC.Substring(2, 3);
                    Year = NIC.Substring(0, 2);

                }
                string Date = null;
                if (NoofDays != null && NoofDays != "" && NoofDays != "000")
                {
                    int NICYear = Convert.ToInt32(Year);
                    int TotalDays = Convert.ToInt32(NoofDays);
                    if (TotalDays <= 366 && TotalDays >= 0)
                    {
                        if (NICYear % 4 != 0)
                        {
                            if (TotalDays > 59)
                            {
                                TotalDays = TotalDays - 1;
                            }
                        }
                        double Days = Convert.ToDouble(TotalDays - 1);
                        DateTime Dates = DateTime.Parse("01-jan-" + Year).AddDays(Days);
                        if (Dates.Day < 10)
                        {
                            day = "0" + Dates.Day.ToString();
                        }
                        else
                        {
                            day = Dates.Day.ToString();
                        }
                        if (Dates.Month < 10)
                        {
                            month = "0" + Dates.Month.ToString();
                        }
                        else
                        {
                            month = Dates.Month.ToString();
                        }
                        Date = day + "/" + month + "/" + Dates.Year;
                    }
                    else if (TotalDays > 500 && TotalDays <= 866)
                    {
                        if (NICYear % 4 != 0)
                        {
                            if (TotalDays > 559)
                            {
                                TotalDays = TotalDays - 1;
                            }
                        }
                        double Days = Convert.ToDouble((TotalDays - 1) - 500);
                        DateTime Dates = DateTime.Parse("01-jan-" + Year).AddDays(Days);
                        if (Dates.Day < 10)
                        {
                            day = "0" + Dates.Day.ToString();
                        }
                        else
                        {
                            day = Dates.Day.ToString();
                        }
                        if (Dates.Month < 10)
                        {
                            month = "0" + Dates.Month.ToString();
                        }
                        else
                        {
                            month = Dates.Month.ToString();
                        }
                        Date = day + "/" + month + "/" + Dates.Year;
                    }
                    else
                    {
                        Date = "Error";
                    }
                }
                else
                {
                    Date = "Error";
                }
                return Json(Date, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                var Date = "Error";
                return Json(Date, JsonRequestBehavior.AllowGet);
            }

        }
        //public ActionResult UploadFilePath(string documentData, string ProspectCode, string PersonalDOB)
        public ActionResult UploadFilePath(string documentData, string ProspectCode)
        {
            try
            {

                List<DocumentDetails> lstDocumentUpload = new List<DocumentDetails>();
                RecruitmentAgent objRecruitmentAgent = new RecruitmentAgent();
                lstDocumentUpload = JsonConvert.DeserializeObject<List<DocumentDetails>>(documentData);

                List<HttpPostedFileBase> files = new List<HttpPostedFileBase>();
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase filebase = Request.Files[file];
                    DocumentUpload(filebase, ProspectCode);
                    files.Add(filebase);

                }
                //if (PersonalDOB != null)
                //{
                //    objRecruitmentAgent.DOB = Convert.ToDateTime(PersonalDOB, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                //}
                objRecruitmentAgent.CreatedBy = Username;
                objRecruitmentAgent.RoleID = RoleID;
                objRecruitmentAgent.RoleName = RoleName;
                objRecruitmentAgent.ProspectCode = ProspectCode;
                objRecruitmentAgent.LstdocumentName = lstDocumentUpload;
                objRecruitmentAgent = objAgentonBoardingLogic.SaveDocumentDetails(objRecruitmentAgent);
                objRecruitmentAgent.RoleID = RoleID;
                objRecruitmentAgent.RoleName = RoleName;
                return Json(objRecruitmentAgent, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //string ErrorCode = objError.WriteError("Policy", "UploadFilePath", ex);
                string ErrorCode = "0000";
                return Json(ErrorCode, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public void DocumentUpload(HttpPostedFileBase file, string ProspectCode)
        {
            try
            {

                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        string directryPath = Server.MapPath("Upload/" + ProspectCode);
                        if (!Directory.Exists(directryPath))
                        {
                            Directory.CreateDirectory(directryPath);
                        }
                        var fileName = Path.GetFileName(file.FileName);

                        var filename = Path.Combine(directryPath, Path.GetFileName(file.FileName));
                        file.SaveAs(filename);
                    }
                }

            }
            catch (Exception ex)
            {
                //ErrorLogging objErrorLog = new ErrorLogging();
                //var ErrorCode = objErrorLog.WriteException(ex, "PersonalAccident", "DocumentUpload");
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ")";
            }

        }
        public void DownloadUploadedfile(string ProspectCode, string FileName)
        {
            //tblProspectDocument objtblDocuments = new tblProspectDocument();
            //RecruitmentAgent objRecruitmentAgent = new RecruitmentAgent();
            //DocumentDetails objDocumentDetails = new DocumentDetails();
            //tblProspect objtblProspect = null;
            //if (ProspectCode != null)
            //{
            //    objtblProspect = Context.tblProspects.Where(a => a.ProspectCode == ProspectCode).FirstOrDefault();
            //}
            //var directorypath = System.Web.HttpContext.Current.Server.MapPath("Upload/"+ ProspectCode);
            //var PolicyFileName = Path.Combine(directorypath, Path.GetFileName(FileName));
            //string result = PolicyFileName;
            //string Filecontenttype = Context.tblProspectDocuments.Where(a => a.ProspectID == objtblProspect.ProspectID).FirstOrDefault().DocType;
            //objDocumentDetails.FileContent = Encoding.ASCII.GetBytes(Context.tblProspectDocuments.Where(a => a.ProspectID == objtblProspect.ProspectID).FirstOrDefault().DocPath);
            //System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
            //System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(PolicyFileName));
            //System.Web.HttpContext.Current.Response.Charset = "";
            //System.Web.HttpContext.Current.Response.OutputStream.Write(objDocumentDetails.FileContent, 0, objDocumentDetails.FileContent.Length);
            //System.Web.HttpContext.Current.Response.Flush();
            //System.Web.HttpContext.Current.Response.Close();            
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName);
            string aaa = Server.MapPath("Upload/" + ProspectCode);
            Response.TransmitFile(Server.MapPath("Upload/" + ProspectCode + "/" + FileName));
            Response.End();
            //if (System.IO.File.Exists(result))
            //{
            //    System.Web.HttpContext.Current.Response.OutputStream.Dispose();
            //    System.IO.File.Delete(result);
            //}

        }

        
        public ActionResult FetchDistrict(string ProvinceCode)
        {
            try
            {
                RecruitmentAgent objRecruitement = new RecruitmentAgent();
                if (ProvinceCode != null && ProvinceCode != "")
                    objRecruitement = objAgentonBoardingLogic.GetProspectDistricts(ProvinceCode);
                if (objRecruitement.LstDistrict != null)
                {
                    return Json(objRecruitement.LstDistrict, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(objRecruitement.LstDistrict, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception Ex)
            {
                return View("Error");
            }
        }
        public ActionResult FetchCity(string DistrictCode)
        {
            try
            {
                RecruitmentAgent objRecruitement = new RecruitmentAgent();
                if (DistrictCode != null && DistrictCode != "")
                    objRecruitement = objAgentonBoardingLogic.GetProspectCity(DistrictCode);
                if (objRecruitement.LstCity != null)
                {
                    return Json(objRecruitement.LstCity, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(objRecruitement.LstCity, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception Ex)
            {
                return View("Error");
            }
        }
        public ActionResult FetchPostalCode(string CityCode)
        {
            try
            {
                RecruitmentAgent objRecruitement = new RecruitmentAgent();
                if (CityCode != null && CityCode != "")
                    objRecruitement = objAgentonBoardingLogic.GetPostalCode(CityCode);
                if (objRecruitement.Pincode != null)
                {
                    return Json(objRecruitement, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(objRecruitement, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception Ex)
            {
                return View("Error");
            }
        }
        public int FetchNoofDays(string date, string Year)
        {
            try
            {
                int NoofDays = 0;
                if (date != null)
                {
                    DateTime d1 = DateTime.Parse("01-jan-" + Year);
                    DateTime d2 = Convert.ToDateTime(date, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                    TimeSpan sp = d2 - d1;
                    NoofDays = Math.Abs(sp.Days);
                }
                return NoofDays;
            }
            catch (Exception Ex)
            {
                return 0;
            }
        }
        public ActionResult CheckingNICNumberExist(string NICNo)
        {
            try
            {
                var Result = (from obj in Context.tblProspects
                              where obj.NIC == NICNo
                              select obj).ToList();
                if (Result.Count > 0)
                {
                    return Json("fail", JsonRequestBehavior.AllowGet);
                }
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }
        public void GenerateReport(string AgentCode)
        {
            Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();
            System.Data.SqlClient.SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            con.Open();
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "USP_GetAggrementpdf";
            cmd.Parameters.AddWithValue("@AgentCode", AgentCode);
            List<DataSet> dslst = new List<DataSet>();
            DataSet ds = new DataSet();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dslst.Add(ds);
                ReportParameter[] parameters = new ReportParameter[1];
                parameters[0] = new ReportParameter("AgentCode", AgentCode);

                byte[] bytes = GenerateRDLCReports(dslst, null, @"~/Reports/AgentonBoarding/ARAgreement.rdlc");
                RenderReports(bytes, "AIA Appointment Letter");
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
                catch (Exception ex)
                {

                }
            }

        }
        public void GenerateIdentificationReport(string ProspectCode)
        {
            Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();
            System.Data.SqlClient.SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            con.Open();
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[USP_GetIdentificationform]";
            cmd.Parameters.AddWithValue("@ProspectCode", ProspectCode);
            List<DataSet> dslst = new List<DataSet>();
            DataSet ds = new DataSet();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dslst.Add(ds);
                ReportParameter[] parameters = new ReportParameter[1];
                parameters[0] = new ReportParameter("ProspectCode", ProspectCode);

                byte[] bytes = GenerateRDLCReports(dslst, null, @"~/Reports/AgentonBoarding/Identificationform.rdlc");
                RenderReports(bytes, "AIA Identification form");
            }

        }
        public void GenerateSLIIReport(string ProspectCode)
        {
            Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();
            System.Data.SqlClient.SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            con.Open();
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "USP_GetSLIIEXAMAPPLICATIONPDF";
            cmd.Parameters.AddWithValue("@ProspectCode", ProspectCode);
            List<DataSet> dslst = new List<DataSet>();
            DataSet ds = new DataSet();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dslst.Add(ds);
                ReportParameter[] parameters = new ReportParameter[1];
                parameters[0] = new ReportParameter("ProspectCode", ProspectCode);

                byte[] bytes = GenerateRDLCReports(dslst, null, @"~/Reports/AgentonBoarding/SLIIExamination.rdlc");
                RenderReports(bytes, "AIA SLII Examination Letter");
            }

        }
        public void GeneratePersonalData(string ProspectCode)
        {
            Microsoft.Reporting.WebForms.ReportViewer vr = new Microsoft.Reporting.WebForms.ReportViewer();
            System.Data.SqlClient.SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            //System.Data.SqlClient.SqlConnection(@"data source = issdbs01; database = AVOJSLife; Uid = sa_policyjs; Pwd = iNube@123;");
            con.Open();
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[USP_Getpersonaldatapdf]";
            cmd.Parameters.AddWithValue("@ProspectCode", ProspectCode);
            List<DataSet> dslst = new List<DataSet>();
            DataSet ds = new DataSet();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dslst.Add(ds);
                ReportParameter[] parameters = new ReportParameter[1];
                parameters[0] = new ReportParameter("ProspectCode", ProspectCode);

                byte[] bytes = GenerateRDLCReports(dslst, null, @"~/Reports/AgentonBoarding/PersonalDataForm.rdlc");
                RenderReports(bytes, "AIA Personal Data Letter");
            }

        }
        public async Task<string> GetRolesForUser(string userId)
        {
            using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
            {
                var rolesForUser = await userManager.GetRolesAsync(userId);
                return rolesForUser[0];
            }
        }
        public ActionResult BulkSuspectUpload(string documentData)
        {
            try
            {
                //Upload and save the file
                List<HttpPostedFileBase> files = new List<HttpPostedFileBase>();
                var filename = string.Empty;
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase filebase = Request.Files[file];
                    if (filebase != null)
                    {
                        if (filebase.ContentLength > 0)
                        {
                            string directryPath = Server.MapPath("Upload/");
                            if (!Directory.Exists(directryPath))
                            {
                                Directory.CreateDirectory(directryPath);
                            }
                            var fileName = Path.GetFileName(filebase.FileName);

                            filename = Path.Combine(directryPath, Path.GetFileName(filebase.FileName));
                            filebase.SaveAs(filename);
                        }
                    }
                    files.Add(filebase);
                }
                //foreach (string file in Request.Files)
                //{
                //    string excelPath = Server.MapPath("~/Files/") + Path.GetFileName(Request.Files[file]);
                //    SuspectBulkUploads.SaveAs(excelPath);
                //}

                string conString = string.Empty;
                string extension = Path.GetExtension(filename);
                switch (extension)
                {
                    case ".xls": //Excel 97-03
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 or higher
                        conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
                        break;

                }
                conString = string.Format(conString, filename);
                using (OleDbConnection excel_con = new OleDbConnection(conString))
                {
                    excel_con.Open();
                    string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                    DataTable dtExcelData = new DataTable();

                    //[OPTIONAL]: It is recommended as otherwise the data will be considered as String by default.
                    dtExcelData.Columns.AddRange(new DataColumn[11]
                    {
                 new DataColumn("SL.No", typeof(string)),
                 new DataColumn("FirstName", typeof(string)),
                new DataColumn("MiddleName", typeof(string)),
                new DataColumn("LastName", typeof(string)),
                new DataColumn("MobileNo", typeof(long)),
                new DataColumn("EmailID",typeof(string)),
                //new DataColumn("ProspectID",typeof(string)),
                new DataColumn("SuspectCode",typeof(string)),
                new DataColumn("CreatedBy",typeof(string)),
                new DataColumn("CreatedDate",typeof(DateTime)),
                new DataColumn("IsActive",typeof(bool)),
                new DataColumn("Status",typeof(int)),
                    });
                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                    {
                        oda.Fill(dtExcelData);
                    }
                    excel_con.Close();

                    string consString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(consString))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {
                            //Set the database table name
                            sqlBulkCopy.DestinationTableName = "dbo.tblSuspect_Temp";

                            //[OPTIONAL]: Map the Excel columns with that of the database table
                            sqlBulkCopy.ColumnMappings.Add("SuspectID", "SL.No");
                            sqlBulkCopy.ColumnMappings.Add("FirstName", "FirstName");
                            sqlBulkCopy.ColumnMappings.Add("MiddleName", "MiddleName");
                            sqlBulkCopy.ColumnMappings.Add("LastName", "LastName");
                            sqlBulkCopy.ColumnMappings.Add("MobileNo", "MobileNo");
                            sqlBulkCopy.ColumnMappings.Add("EmailID", "EmailID");
                            //sqlBulkCopy.ColumnMappings.Add("null", "ProspectID");
                            sqlBulkCopy.ColumnMappings.Add("SuspectCode", "SuspectCode");
                            sqlBulkCopy.ColumnMappings.Add("CreatedBy", "CreatedBy");
                            sqlBulkCopy.ColumnMappings.Add("CreatedDate", "CreatedDate");
                            sqlBulkCopy.ColumnMappings.Add("IsActive", "IsActive");
                            sqlBulkCopy.ColumnMappings.Add("Status", "Status");
                            con.Open();
                            sqlBulkCopy.WriteToServer(dtExcelData);
                            con.Close();
                        }
                    }
                }

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
    }
}