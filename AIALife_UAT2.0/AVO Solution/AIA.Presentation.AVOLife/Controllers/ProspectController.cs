using Grid.Mvc.Ajax.GridExtensions;
using AIA.Life.Models.Opportunity;
using AIA.Presentation.AVOLife.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AIA.Presentation.AVOLife.ExceptionHandling;
using AIA.Life.Models.NeedAnalysis;
using Rotativa.MVC;
using AIA.Life.Models.Common;
using System.Web.Routing;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;
using AIA.Life.Repository.AIAEntity;

namespace AIA.Presentation.AVOLife.Controllers
{
    [Authorize]
    [ErrorLogging]
    [SessionTimeout]
    public class ProspectController : BaseController
    {
        AVOAIALifeEntities Context = new AVOAIALifeEntities();
        private string _username = string.Empty;
        AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
        public ProspectController()
        {
            _username = System.Web.HttpContext.Current.User.Identity.Name;
        }
        // GET: Prospect
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Prospect()
        {
            Prospect obj = new Prospect();
            obj.ProspectStage = 2;// Set Stage as Prospect
            TempData["Load"] = "FirstTime";
            obj.CreatedBy = _username;
            obj = objProspectBusiness.LoadProspectPool(obj);
            ViewBag.ProspectPoolCount = obj.ObjProspectPool.Count;
            obj.ProspectStage = 4;// Set Stage as Prospect
            TempData["Load"] = "FirstTime";
            obj.CreatedBy = _username;
            obj = objProspectBusiness.LoadProspectPool(obj);
            ViewBag.NeedAnalysisCount = obj.ObjProspectPool.Count;
            return View();
        }
        public ActionResult ProspectPool()
        {
            Prospect obj = new Prospect();
            obj.ProspectStage = 2;// Set Stage as Prospect
            TempData["Load"] = "FirstTime";
            obj.CreatedBy = _username;
            obj = objProspectBusiness.LoadProspectPool(obj);
            //ViewBag.Details = obj;
            //TempData["Load"] = "FirstTime";
            //AjaxGrid<ProspectPool> ajaxgrid = null;
            //ajaxgrid = (AjaxGrid<ProspectPool>)new AjaxGridFactory().CreateAjaxGrid((obj.ObjProspectPool).AsQueryable(), 1, false);
            //ViewBag.Data = 0;
            //return PartialView("PartialProspectPoolGrid", ajaxgrid);
            return View(obj);
        }
        public ActionResult PartialProspectPoolGrid()
        {
            try
            {
                Prospect objProspect = new Prospect();
                objProspect.ProspectStage = 2;
                objProspect.CreatedBy = _username;
                objProspect = objProspectBusiness.LoadProspectPool(objProspect);

                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                var grid = (AjaxGrid<ProspectPool>)new AjaxGridFactory().CreateAjaxGrid((objProspect.ObjProspectPool).AsQueryable(), 1, false,2);

                return PartialView(grid);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }
        public ActionResult _PartialNeedAnalysisCompletedPoolGrid()  
        {
            try
            {
                Prospect objProspect = new Prospect();
                objProspect.ProspectStage = 4;
                objProspect.CreatedBy = _username;
                objProspect = objProspectBusiness.LoadProspectPool(objProspect);

                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                var grid = (AjaxGrid<ProspectPool>)new AjaxGridFactory().CreateAjaxGrid((objProspect.ObjProspectPool).AsQueryable(), 1, false,2);


                return PartialView(grid);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }
        //Duplicate of ProspectPool
        public ActionResult SearchProspectPoolDetails(int? page)
        {
            try
            {
                Prospect objProspect = new Prospect();
                objProspect.ProspectStage = 2;
                objProspect.CreatedBy = _username;
                objProspect = objProspectBusiness.LoadProspectPool(objProspect);

                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                var grid = aj.CreateAjaxGrid(objProspect.ObjProspectPool.AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, 2);

                return Json(new
                {
                    Html = RenderPartialViewToString("~/Views/Prospect/PartialProspectPoolGrid.cshtml", grid),
                    HasItems = grid.HasItems
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }
        public ActionResult SearchNeedAnalysisCompletedPoolDetails(int? page) 
        {
            try
            {
                Prospect objProspect = new Prospect();
                objProspect.ProspectStage = 4;
                objProspect.CreatedBy = _username;
                objProspect = objProspectBusiness.LoadProspectPool(objProspect);

                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                var grid = aj.CreateAjaxGrid(objProspect.ObjProspectPool.AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, 2);

                return Json(new
                {
                    Html = RenderPartialViewToString("~/Views/Prospect/_PartialNeedAnalysisCompletedPoolGrid.cshtml", grid),
                    HasItems = grid.HasItems
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        public ActionResult ConfirmedProspect()
        {
            Prospect obj = new Prospect();
            obj.ProspectStage = 3;// Set Stage as  Confirmed Prospect
            obj = objProspectBusiness.LoadProspectPool(obj);
            return View(obj);
        }
        public ActionResult NeedAnalysisCompleted()
        {
            Prospect obj = new Prospect();
            obj.ProspectStage = 4;// Set Stage as Need Analysis
            TempData["Load"] = "FirstTime";
            obj.CreatedBy = _username;
            obj = objProspectBusiness.LoadProspectPool(obj);
            ViewBag.NeedAnanlysisCompleted = obj.ObjProspectPool.Count;
            return View(obj);
        }
        public ActionResult ProspectReallocate()
        {
            return View();
        }
        public ActionResult ProspectScreen()
        {
            return View();
        }
       
        public ActionResult SaveProspect(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            //string path1 = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "signatureimage";
            //string filename1 = "tst.png";
            //string filepath1 = System.IO.Path.Combine(path1, filename1);

            //var t = objProspect.GraphByteArray.Substring(22);  
            if (!string.IsNullOrEmpty(objProspect.objNeedAnalysis.ByteArraygraph))
            {
                try
                {
                    var t = objProspect.objNeedAnalysis.ByteArraygraph.Split(',')[1];
                    objProspect.ByteArrayGraph = Convert.FromBase64String(t);
                    objProspect.ByteArrayGraph = generatepdf(objProspect.ByteArrayGraph, objProspect);
                    TempData["FNAGraphGenerated"] = Convert.ToBase64String(objProspect.ByteArrayGraph);
                }
                catch (Exception ex)
                {

                  
                }
               

            }
            //Image image;
            //using (MemoryStream ms = new MemoryStream(objProspect.ByteArrayGraph))
            //{
            //    image = Image.FromStream(ms);
            //}
            //image.Save(filepath1, System.Drawing.Imaging.ImageFormat.Png);

            //SmtpClient objClient = new SmtpClient();
            //MailMessage message = new MailMessage();
            //string SuccessFile = "Success_FNA_.png";
            //System.Net.Mail.Attachment attach_Success6 = new System.Net.Mail.Attachment(new MemoryStream(objProspect.ByteArrayGraph), SuccessFile);
            //message.Attachments.Add(attach_Success6);
            //message.To.Add("imad@inubesolutions.com");
            //message.Subject = "Test";
            //objClient.Send(message);
            #region NotePad
            if (!string.IsNullOrEmpty(objProspect.NotePad))
            {
                SignatureToImage objSigToImg = new SignatureToImage();
                string filename = "NotePad" + objProspect.ContactID + ".png";
                string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "NotePadimage";
                string filepath = System.IO.Path.Combine(path, filename);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var img = objSigToImg.SigJsonToImage(objProspect.NotePad);
                objProspect.NotePadByteArray = ImageToByte(img);
                img.Save(filepath, ImageFormat.Png);
                objProspect.objNeedAnalysis.NotePadPath = filepath;
            }

            #endregion

            #region Signature
            if (!string.IsNullOrEmpty(objProspect.Signature))
            {
                SignatureToImage objSigToImg = new SignatureToImage();
                string filename = "signature" + objProspect.ContactID + ".png";
                string directryPath = Server.MapPath("ContactID_" + objProspect.ContactID);
                if (!Directory.Exists(directryPath))
                {
                    Directory.CreateDirectory(directryPath);
                }
                string filepath = Path.Combine(directryPath, filename);
                var img = objSigToImg.SigJsonToImage(objProspect.Signature);
                objProspect.ByteArray = ImageToByte(img);
                img.Save(filepath, ImageFormat.Png);
                objProspect.objNeedAnalysis.UploadSignPath = Path.Combine("\\Prospect", "ContactID_" + objProspect.ContactID, filename);
            }

            #endregion

            //HttpPostedFileBase filebase = Request.Files[0];
            //DocumentUpload(filebase, objProspect);

            #region Upload
            //if (!string.IsNullOrEmpty(objProspect.Upload))
            //{
            //FileInfo fi1 = new FileInfo(objProspect.Upload);
            //string old = objProspect.Upload;
            //HttpPostedFileBase file = Request.Files[0];
            //var extension = objProspect.Upload.Split('.');
            //string filename = "upload" + objProspect.Name + "." + extension[1];
            //string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "Uploads";
            //string filepath = Path.Combine(path, filename);
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}
            //if (!System.IO.File.Exists(filepath))
            //{
            //    file.SaveAs(filepath);
            //}
            //else
            //{
            //}
            //var file = objProspect.Upload.Split('\\');
            //var tempaddress = Directory.EnumerateFiles(@"c:\", file[2], SearchOption.AllDirectories).ToList();
            //tempaddress.AddRange(Directory.EnumerateFiles(@"d:\", file[2], SearchOption.AllDirectories).ToList());
            //string text;
            //foreach (var item in tempaddress)
            //{
            //    text = System.IO.File.ReadAllText(item);
            //    if (!System.IO.File.Exists(filepath))
            //    {
            //        System.IO.File.Create(filepath);
            //        System.IO.File.WriteAllText(filepath, text);

            //    }
            //    else
            //    {
            //    }
            //}
            //string text = System.IO.File.ReadAllText(tempaddress);

            //    objProspect.objNeedAnalysis.UploadDocPath = filepath;
            //}

            #endregion

            objProspect.objNeedAnalysis.ProspectSign = objProspect.ByteArray;
            objProspect.CreatedBy = _username;
            objProspect.ByteArray = null;
            objProspect = GetByteArray(objProspect);
            objProspect = objProspectBusiness.SaveProspect(objProspect);
            if (objProspect.Message == "Success" && objProspect.SendEmail == "Quote")
            {
                var Response = SendEmailAndSMSNotificationOnSAveProspect(objProspect);
            }
            var ObjResponse = new { Message = objProspect.Message, ContactID = objProspect.ContactID, Status = objProspect.ProspectStage, Error = objProspect.Error.ErrorMessage};
            return Json(ObjResponse, JsonRequestBehavior.AllowGet);

        }
        public string SendEmailAndSMSNotificationOnSAveProspect(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
             AIA.Life.Business.Quote.QuoteBusiness QuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            objProspect=QuoteBusiness.SendEmailAndSMSNotificationOnSAveProspect(objProspect);

            return objProspect.Message;
        }
        public Prospect GetByteArray(Prospect objProspect)
        {
            if (objProspect.objNeedAnalysis.objCalculator.MonthlyPensionGap != null && objProspect.objNeedAnalysis.objCalculator.MonthlyPensionGap != 0)
            {
                objProspect.ByteArray = RetirementCalcPdf(objProspect);
            }
            if ((objProspect.objNeedAnalysis.CriticalGap != null && objProspect.objNeedAnalysis.CriticalGap != 0) && (objProspect.objNeedAnalysis.HospitalizationGap != null && objProspect.objNeedAnalysis.HospitalizationGap != 0) && (objProspect.objNeedAnalysis.additionalexpenseGap != null && objProspect.objNeedAnalysis.additionalexpenseGap != 0))
            {
                objProspect.ByteArray1 = HealthCalcPdf(objProspect);
            }
            if ((objProspect.objNeedAnalysis.EduMaturity != null && objProspect.objNeedAnalysis.EduMaturity != 0) && (objProspect.objNeedAnalysis.EduLumpSum != null && objProspect.objNeedAnalysis.EduLumpSum != 0))
            {
                objProspect.ByteArray2 = EduCalcPdf(objProspect);
            }
            if (objProspect.objNeedAnalysis.SavingTarget != null && objProspect.objNeedAnalysis.SavingTarget != 0)
            {
                objProspect.ByteArray3 = SavingCalcPdf(objProspect);
            }
            if ((objProspect.objNeedAnalysis.FutureFund != null && objProspect.objNeedAnalysis.FutureFund != 0) && (objProspect.objNeedAnalysis.EmergencyFund != null && objProspect.objNeedAnalysis.EmergencyFund != 0))
            {
                objProspect.ByteArray4 = HumanValueCalcPdf(objProspect);
            }
            if ((objProspect.objNeedAnalysis.EmergencyFundGap != null && objProspect.objNeedAnalysis.EmergencyFundGap != 0))
            {
                objProspect.ByteArray5 = FNAPdf(objProspect);
            }
            return objProspect;
        }
        public string DownloadGraph(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            if (!string.IsNullOrEmpty(objProspect.objNeedAnalysis.ByteArraygraph))
            {
                var t = objProspect.objNeedAnalysis.ByteArraygraph.Split(',')[1];
                objProspect.ByteArrayGraph = Convert.FromBase64String(t);
                objProspect.ByteArrayGraph = generatepdf(objProspect.ByteArrayGraph, objProspect);
                TempData["FNAGraph"] = Convert.ToBase64String(objProspect.ByteArrayGraph);
                //System.Drawing.Image image;
                //using (MemoryStream ms = new MemoryStream(objProspect.ByteArrayGraph))
                //{
                //    image = System.Drawing.Image.FromStream(ms);
                //}
                string Proposerfilename = "Proposersignature.pdf";
                string ProposerdirectryPath = "";
                ProposerdirectryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Policy/ContactID_1111");
                //ProposerdirectryPath = HttpContext.Current.Server.MapPath("ContactID_" + ObjQuote.objProspect.ContactID);
                if (!Directory.Exists(ProposerdirectryPath))
                {
                    Directory.CreateDirectory(ProposerdirectryPath);
                }
                string Proposerfilepath = Path.Combine(ProposerdirectryPath, Proposerfilename);
                System.IO.File.WriteAllBytes(Proposerfilepath, objProspect.ByteArrayGraph);
                return "Success";
                //try
                //{
                //    if (objProspect.ByteArrayGraph != null)
                //    {
                //        Response.Clear();
                //        Response.ClearContent();
                //        Response.Buffer = true;
                //        Response.ContentType = "application/pdf";
                //        Response.AddHeader("content-disposition", "attachment;filename=" + objProspect.Name + ".pdf");
                //        Response.OutputStream.Write(objProspect.ByteArrayGraph, 0, objProspect.ByteArrayGraph.Length);
                //        Response.Flush();
                //        try
                //        {
                //            Response.End();
                //        }

                //        catch (Exception ex)
                //        {
                //            throw ex;
                //        }
                //        //return new FileStreamResult(ms, "application/pdf");
                //    }
                //}
                //catch (Exception E)
                //{ }

            }
            else
            {
                return "Failure";

            }
        }
        public FileStreamResult DownloadFNA()
        {
            if (TempData.ContainsKey("FNAGraph"))
            {
                var temp = TempData["FNAGraph"];
                var FNA = Convert.FromBase64String(temp.ToString());
                //MemoryStream ms = new MemoryStream(temp);
                Response.Clear();
                Response.ClearContent();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=FNAGraph.pdf");
                Response.OutputStream.Write(FNA, 0, FNA.Length);
                Response.Flush();
                try
                {
                    Response.End();
                }
                catch (Exception Ex)
                {

                }

            }
            return null;
        }
        public void GraphRenderReports(byte[] bytes, string fileName)
        {
            try
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
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public byte[] generatepdf(byte[] byteImage, AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(byteImage);
            image.ScaleToFit( 600, 769);
            using (MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                Document document = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                //string path2 = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "signatureimage";
                //string filename2 = "test.pdf";
                //string filepath2 = System.IO.Path.Combine(path2, filename2);
                // PdfWriter.GetInstance(document, new FileStream(filepath2, FileMode.Create));
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, memoryStream);
                document.Open();
                string text1 = "Need Identification Graph of "+objProspect.Salutation+" "+objProspect.Name.First().ToString().ToUpper() + objProspect.Name.Substring(1) +" " + objProspect.LastName.First().ToString().ToUpper() + objProspect.LastName.Substring(1);
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 16, iTextSharp.text.Font.NORMAL);
                //var titleFont1 = FontFactory.GetFont("Arial", 25, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                // text1Title.Font = times;
                Paragraph text1Title = new Paragraph(text1, times);
                text1Title.Alignment = Element.ALIGN_CENTER;

                string text2 = "I accept that I have understood my priority needs through the questionaire. However I am open to look at different product possibilities.";
                Paragraph text2Title = new Paragraph(text2);
                var titleFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                text2Title.Font = titleFont;
                text2Title.Alignment = Element.ALIGN_JUSTIFIED;
                document.Add(text1Title);
                document.Add(image);
                document.Add(text2Title);
                document.Close();
                byte[] bytes = memoryStream.ToArray();

                memoryStream.Close();

                //Response.Clear();
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("Content-Disposition", "attachment; filename=test.pdf");
                //Response.ContentType = "application/pdf";
                //Response.Buffer = true;
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //Response.BinaryWrite(bytes);
                //Response.End();

                return bytes;
            }
        }
        public byte[] Appgeneratepdf(byte[] byteImage, AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(byteImage);
            //image.ScalePercent(0.4f * 135);
            image.ScaleAbsolute(555f,375f);
            string imageURL = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "/Content/Images/AIA Pdf image.jpg";
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
            jpg.ScalePercent(0.4f * 175);
            using (MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                Document document = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                string path2 = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "signatureimage";
                string filename2 = "test.pdf";
                string filepath2 = System.IO.Path.Combine(path2, filename2);
                // PdfWriter.GetInstance(document, new FileStream(filepath2, FileMode.Create));
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, memoryStream);
                document.Open();
                string text1 = "Need Identification Graph of " + objProspect.Salutation + " " + objProspect.Name + " " + objProspect.LastName;
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 16, iTextSharp.text.Font.NORMAL);
                //var titleFont1 = FontFactory.GetFont("Arial", 25, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                // text1Title.Font = times;
                Paragraph text1Title = new Paragraph(text1, times);
                text1Title.Alignment = Element.ALIGN_CENTER;

                string text2 = "I accept that I have understood my priority needs through the questionaire. However I am open to look at different product possibilities.";
                Paragraph text2Title = new Paragraph(text2);
                var titleFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                text2Title.Font = titleFont;
                text2Title.Alignment = Element.ALIGN_JUSTIFIED;
                document.Add(text1Title);
                document.Add(jpg);
                document.Add(image);
                document.Add(text2Title);
                document.Close();
                byte[] bytes = memoryStream.ToArray();
                System.IO.File.WriteAllBytes(filepath2, bytes);

                memoryStream.Close();

                //Response.Clear();
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("Content-Disposition", "attachment; filename=test.pdf");
                //Response.ContentType = "application/pdf";
                //Response.Buffer = true;
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //Response.BinaryWrite(bytes);
                //Response.End();

                return bytes;
            }
        }

        public Byte[] RetirementCalcPdf(Prospect objProspect)
        {
            ProspectController ProspectController = ProspectController.CreateController<ProspectController>();

            //AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            //objProspect.ProspectStage = 4;// Set Stage as Need Analysis
            //objProspect.ContactID = ContactID;
            //objProspect = WebApiLogic.GetPostComplexTypeToAPI<Prospect>(objProspect, "LoadContactInformation", "SuspectApi");
            objProspect.CreatedBy = _username;
            objProspect.WPPhone = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.MobileNo).FirstOrDefault();
            objProspect.WPCode = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.AgentCode).FirstOrDefault();
            var pdf = new ViewAsPdf("~/Views/Reports/RetirementCalcReport.cshtml", objProspect);
            Byte[] pdfData = pdf.BuildPdf(ProspectController.ControllerContext);
            TempData["FNAGraph"] = Convert.ToBase64String(pdfData);

            return pdfData;
        }
        public string RetirementCalc(Prospect objProspect)
        {
            ProspectController ProspectController = ProspectController.CreateController<ProspectController>();

            //AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            //objProspect.ProspectStage = 4;// Set Stage as Need Analysis
            //objProspect.ContactID = ContactID;
            //objProspect = WebApiLogic.GetPostComplexTypeToAPI<Prospect>(objProspect, "LoadContactInformation", "SuspectApi");
            objProspect.CreatedBy = _username;
            objProspect.WPPhone = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.MobileNo).FirstOrDefault();
            objProspect.WPCode = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.AgentCode).FirstOrDefault();
            var pdf = new ViewAsPdf("~/Views/Reports/RetirementCalcReport.cshtml", objProspect);
            Byte[] pdfData = pdf.BuildPdf(ProspectController.ControllerContext);
            if (pdfData != null)
            {
                TempData["DownloadPDF"] = Convert.ToBase64String(pdfData);
                return "Success";
            }
            else
            {
                return "Failure";
            }
            
        }
        public string HealthCalc(Prospect objProspect)
        {
            ProspectController ProspectController = ProspectController.CreateController<ProspectController>();

            //AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            //objProspect.ProspectStage = 4;// Set Stage as Need Analysis
            //objProspect.ContactID = ContactID;
            //objProspect = WebApiLogic.GetPostComplexTypeToAPI<Prospect>(objProspect, "LoadContactInformation", "SuspectApi");
            objProspect.CreatedBy = _username;
            objProspect.WPPhone = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.MobileNo).FirstOrDefault();
            objProspect.WPCode = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.AgentCode).FirstOrDefault();
            objProspect.objNeedAnalysis.adversities = String.Join(",", objProspect.objNeedAnalysis.objadversities);
            var pdf = new ViewAsPdf("~/Views/Reports/HealthCalcReport.cshtml", objProspect);
            Byte[] pdfData = pdf.BuildPdf(ProspectController.ControllerContext);
            if (pdfData != null)
            {
                TempData["DownloadPDF"] = Convert.ToBase64String(pdfData);
                return "Success";
            }
            else
            {
                return "Failure";
            }

        }
        public string HumanValueCalc(Prospect objProspect)
        {
            ProspectController ProspectController = ProspectController.CreateController<ProspectController>();

            //AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            //objProspect.ProspectStage = 4;// Set Stage as Need Analysis
            //objProspect.ContactID = ContactID;
            //objProspect = WebApiLogic.GetPostComplexTypeToAPI<Prospect>(objProspect, "LoadContactInformation", "SuspectApi");
            objProspect.CreatedBy = _username;
            objProspect.WPPhone = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.MobileNo).FirstOrDefault();
            objProspect.WPCode = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.AgentCode).FirstOrDefault();
            var pdf = new ViewAsPdf("~/Views/Reports/HumanValueCalcReport.cshtml", objProspect);
            Byte[] pdfData = pdf.BuildPdf(ProspectController.ControllerContext);
            if (pdfData != null)
            {
                TempData["DownloadPDF"] = Convert.ToBase64String(pdfData);
                return "Success";
            }
            else
            {
                return "Failure";
            }

        }
        public FileStreamResult DownloadPDF(string PDF)
        {
            if (TempData.ContainsKey("DownloadPDF"))
            {
                var temp = TempData["DownloadPDF"];
                var FNA = Convert.FromBase64String(temp.ToString());
                //MemoryStream ms = new MemoryStream(temp);
                Response.Clear();
                Response.ClearContent();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename="+ PDF + ".pdf");
                Response.OutputStream.Write(FNA, 0, FNA.Length);
                Response.Flush();
                try
                {
                    Response.End();
                }
                catch (Exception Ex)
                {

                }

            }
            return null;
        }

        public Byte[] HealthCalcPdf(Prospect objProspect)
        {
            ProspectController ProspectController = ProspectController.CreateController<ProspectController>();

            //AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            //objProspect.ProspectStage = 4;// Set Stage as Need Analysis
            //objProspect.ContactID = ContactID;
            //objProspect = WebApiLogic.GetPostComplexTypeToAPI<Prospect>(objProspect, "LoadContactInformation", "SuspectApi");
            objProspect.CreatedBy = _username;
            objProspect.WPPhone = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.MobileNo).FirstOrDefault();
            objProspect.WPCode = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.AgentCode).FirstOrDefault();
            objProspect.objNeedAnalysis.adversities = String.Join(",", objProspect.objNeedAnalysis.objadversities);

            var pdf = new ViewAsPdf("~/Views/Reports/HealthCalcReport.cshtml", objProspect);
            Byte[] pdfData = pdf.BuildPdf(ProspectController.ControllerContext);
            return pdfData;
        }
        public Byte[] EduCalcPdf(Prospect objProspect)
        {
            ProspectController ProspectController = ProspectController.CreateController<ProspectController>();

            //AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            //objProspect.ProspectStage = 4;// Set Stage as Need Analysis
            //objProspect.ContactID = ContactID;
            //objProspect = WebApiLogic.GetPostComplexTypeToAPI<Prospect>(objProspect, "LoadContactInformation", "SuspectApi");
            objProspect.CreatedBy = _username;
            objProspect.WPPhone = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.MobileNo).FirstOrDefault();
            objProspect.WPCode = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.AgentCode).FirstOrDefault();
            var pdf = new ViewAsPdf("~/Views/Reports/EduCalcReport.cshtml", objProspect);
            Byte[] pdfData = pdf.BuildPdf(ProspectController.ControllerContext);
            return pdfData;
        }
        public Byte[] SavingCalcPdf(Prospect objProspect)
        {
            ProspectController ProspectController = ProspectController.CreateController<ProspectController>();

            //AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            //objProspect.ProspectStage = 4;// Set Stage as Need Analysis
            //objProspect.ContactID = ContactID;
            //objProspect = WebApiLogic.GetPostComplexTypeToAPI<Prospect>(objProspect, "LoadContactInformation", "SuspectApi");
             objProspect.CreatedBy = _username;
            objProspect.WPPhone = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.MobileNo).FirstOrDefault();
            objProspect.WPCode = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.AgentCode).FirstOrDefault();
            var pdf = new ViewAsPdf("~/Views/Reports/SavingCalcReport.cshtml", objProspect);
            Byte[] pdfData = pdf.BuildPdf(ProspectController.ControllerContext);
            return pdfData;
        }

        public string SavingCalc(Prospect objProspect)
        {
            ProspectController ProspectController = ProspectController.CreateController<ProspectController>();

            //AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            //objProspect.ProspectStage = 4;// Set Stage as Need Analysis
            //objProspect.ContactID = ContactID;
            //objProspect = WebApiLogic.GetPostComplexTypeToAPI<Prospect>(objProspect, "LoadContactInformation", "SuspectApi");
            objProspect.CreatedBy = _username;
            objProspect.WPPhone = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.MobileNo).FirstOrDefault();
            objProspect.WPCode = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.AgentCode).FirstOrDefault();
            var pdf = new ViewAsPdf("~/Views/Reports/SavingCalcReport.cshtml", objProspect);
            Byte[] pdfData = pdf.BuildPdf(ProspectController.ControllerContext);
            if (pdfData != null)
            {
                TempData["DownloadPDF"] = Convert.ToBase64String(pdfData);
                return "Success";
            }
            else
            {
                return "Failure";
            }
        }

        public string EduCalc(Prospect objProspect)
        {
            ProspectController ProspectController = ProspectController.CreateController<ProspectController>();

            //AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            //objProspect.ProspectStage = 4;// Set Stage as Need Analysis
            //objProspect.ContactID = ContactID;
            //objProspect = WebApiLogic.GetPostComplexTypeToAPI<Prospect>(objProspect, "LoadContactInformation", "SuspectApi");
            objProspect.CreatedBy = _username;
            objProspect.WPPhone = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.MobileNo).FirstOrDefault();
            objProspect.WPCode = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.AgentCode).FirstOrDefault();
            var pdf = new ViewAsPdf("~/Views/Reports/EduCalcReport.cshtml", objProspect);
            Byte[] pdfData = pdf.BuildPdf(ProspectController.ControllerContext);
            if (pdfData != null)
            {
                TempData["DownloadPDF"] = Convert.ToBase64String(pdfData);
                return "Success";
            }
            else
            {
                return "Failure";
            }
        }
        public string FNACalc(Prospect objProspect) 
        {
            ProspectController ProspectController = ProspectController.CreateController<ProspectController>();

            //AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            //objProspect.ProspectStage = 4;// Set Stage as Need Analysis
            //objProspect.ContactID = ContactID;
            //objProspect = WebApiLogic.GetPostComplexTypeToAPI<Prospect>(objProspect, "LoadContactInformation", "SuspectApi");
            objProspect.CreatedBy = _username;
            objProspect.WPPhone = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.MobileNo).FirstOrDefault();
            objProspect.WPCode = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.AgentCode).FirstOrDefault();
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
            objProspect.CreatedBy = _username;
            objProspect.WPPhone = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.MobileNo).FirstOrDefault();
            objProspect.WPCode = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.AgentCode).FirstOrDefault();
            var pdf = new ViewAsPdf("~/Views/Reports/FNAReport.cshtml", objProspect);
            Byte[] pdfData = pdf.BuildPdf(ProspectController.ControllerContext);
            if (pdfData != null)
            {
                TempData["DownloadPDF"] = Convert.ToBase64String(pdfData);
                return "Success";
            }
            else
            {
                return "Failure";
            }
        }
        public Byte[] HumanValueCalcPdf(Prospect objProspect)
        {
            ProspectController ProspectController = ProspectController.CreateController<ProspectController>();
            //AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            //objProspect.ProspectStage = 4;// Set Stage as Need Analysis
            //objProspect.ContactID = ContactID;
            //objProspect = WebApiLogic.GetPostComplexTypeToAPI<Prospect>(objProspect, "LoadContactInformation", "SuspectApi");
            objProspect.CreatedBy = _username;
            objProspect.WPPhone = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.MobileNo).FirstOrDefault();
            objProspect.WPCode = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.AgentCode).FirstOrDefault();
            var pdf = new ViewAsPdf("~/Views/Reports/HumanValueCalcReport.cshtml", objProspect);
            Byte[] pdfData = pdf.BuildPdf(ProspectController.ControllerContext);
            return pdfData;
        }
        public Byte[] FNAPdf(Prospect objProspect)
        {
            ProspectController ProspectController = ProspectController.CreateController<ProspectController>();

            //AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
            //objProspect.ProspectStage = 4;// Set Stage as Need Analysis
            //objProspect.ContactID = ContactID;
            //objProspect = WebApiLogic.GetPostComplexTypeToAPI<Prospect>(objProspect, "LoadContactInformation", "SuspectApi");
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
            objProspect.CreatedBy = _username;
            objProspect.WPPhone = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.MobileNo).FirstOrDefault();
            objProspect.WPCode = Context.tblMasIMOUsers.Where(a => a.UserID == objProspect.CreatedBy).Select(a => a.AgentCode).FirstOrDefault();
            var pdf = new ViewAsPdf("~/Views/Reports/FNAReport.cshtml", objProspect);
            Byte[] pdfData = pdf.BuildPdf(ProspectController.ControllerContext);
            return pdfData;
        }
        public JsonResult UploadFilePath()
        {
            try
            {
                HttpPostedFileBase file = Request.Files[0];
                string fileName = string.Empty;
                string path = string.Empty;
                if (file != null && file.ContentLength > 0)
                {
                    Stream s;
                    s = file.InputStream;
                    fileName = Path.GetFileName(file.FileName);
                    bool flag = ValidateFileType.IsAllowedFileType(fileName, s);
                    if (flag)
                    {
                        var extension = fileName.Split('.');
                        fileName = "upload" + "Name" + "." + extension[1];
                        //string newFolder = Path.Combine(Server.MapPath("~/Uploads/"), "00001");
                        //System.IO.Directory.CreateDirectory(newFolder);
                        path = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                        file.SaveAs(path);
                        return Json(path, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("error", JsonRequestBehavior.AllowGet);
                    }

                }
                return Json(path, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json("OOPS!!Something went wrong while uploading file, your Error code: ", JsonRequestBehavior.AllowGet);
            }
        }

        //public void UploadFilePath()
        //{
        //    try
        //    {
        //        if (Request.Files.Count > 0)
        //        {
        //            System.Web.HttpFileCollectionBase files = Request.Files;
        //            for (int i = 0; i < files.Count; i++)
        //            {
        //                HttpPostedFileBase file = files[i];
        //                string fname = Server.MapPath("~/uploads/" + file.FileName);
        //                file.SaveAs(fname);
        //            }
        //            Response.ContentType = "text/plain";
        //            Response.Write("File Uploaded Successfully!");
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        //return Json("OOPS!!Something went wrong while uploading file, your Error code: ", JsonRequestBehavior.AllowGet);
        //    }
        //}
        public static byte[] ImageToByte(System.Drawing.Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        //  public ActionResult SaveNeedAnalysis(AIA.Life.Models.Opportunity.Prospect objProspect, AIA.Life.Models.NeedAnalysis.NeedAnalysis objNeedAnalysis)
        public ActionResult SaveNeedAnalysis(AIA.Life.Models.Opportunity.Prospect objProspect)
        {

            #region Signature
            if (!string.IsNullOrEmpty(objProspect.Signature))
            {
                SignatureToImage objSigToImg = new SignatureToImage();
                string filename = "signature" + objProspect.Name + ".png";
                string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "signatureimage";
                string filepath = System.IO.Path.Combine(path, filename);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var img = objSigToImg.SigJsonToImage(objProspect.Signature);
                objProspect.ByteArray = ImageToByte(img);
                img.Save(filepath, ImageFormat.Png);
                objProspect.objNeedAnalysis.UploadSignPath = filepath;
            }

            #endregion

            //HttpPostedFileBase filebase = Request.Files[0];
            //DocumentUpload(filebase, objProspect);

            #region Upload
            //if (!string.IsNullOrEmpty(objProspect.Upload))
            //{
            //FileInfo fi1 = new FileInfo(objProspect.Upload);
            //string old = objProspect.Upload;
            //HttpPostedFileBase file = Request.Files[0];
            //var extension = objProspect.Upload.Split('.');
            //string filename = "upload" + objProspect.Name + "." + extension[1];
            //string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "Uploads";
            //string filepath = Path.Combine(path, filename);
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}
            //if (!System.IO.File.Exists(filepath))
            //{
            //    file.SaveAs(filepath);
            //}
            //else
            //{
            //}
            //var file = objProspect.Upload.Split('\\');
            //var tempaddress = Directory.EnumerateFiles(@"c:\", file[2], SearchOption.AllDirectories).ToList();
            //tempaddress.AddRange(Directory.EnumerateFiles(@"d:\", file[2], SearchOption.AllDirectories).ToList());
            //string text;
            //foreach (var item in tempaddress)
            //{
            //    text = System.IO.File.ReadAllText(item);
            //    if (!System.IO.File.Exists(filepath))
            //    {
            //        System.IO.File.Create(filepath);
            //        System.IO.File.WriteAllText(filepath, text);

            //    }
            //    else
            //    {
            //    }
            //}
            //string text = System.IO.File.ReadAllText(tempaddress);

            //    objProspect.objNeedAnalysis.UploadDocPath = filepath;
            //}

            #endregion

            objProspect.objNeedAnalysis.ProspectSign = objProspect.ByteArray;

            objProspect = objProspectBusiness.SaveProspect(objProspect);
            var ObjResponse = new { Message = objProspect.Message, Status = objProspect.ProspectStage };
            return Json(ObjResponse, JsonRequestBehavior.AllowGet);
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
        public ActionResult LoadProspectInformation(string ContactID)
        {
            try
            {
                AIA.Life.Models.Opportunity.Prospect objProspect = new Prospect();
                objProspect.ContactID =Convert.ToInt32(CrossCutting.CrossCutting_EncryptDecrypt.Decrypt(ContactID));
                objProspect.CreatedBy = _username;
                #region Check user authorization
                AuthorizeUser authorizeUser = new AuthorizeUser();
                authorizeUser.UserName = _username;
                authorizeUser.ContactId = objProspect.ContactID;
                authorizeUser = new AIA.Life.Business.Common.CommonBusiness().CheckAuthorisation(authorizeUser);
                if (!string.IsNullOrEmpty(authorizeUser.Error.ErrorMessage))
                {
                    return RedirectToAction("AuthorizeError", "Error", new { errorMessage = authorizeUser.Error.ErrorMessage });
                }
                #endregion
                objProspect = objProspectBusiness.LoadContactInformation(objProspect);
                //objProspect.objNeedAnalysis.dlladversities.Add(new SelectListItem { Text = "Critical Illnesses" });
                //objProspect.objNeedAnalysis.dlladversities.Add(new SelectListItem { Text = "Excess Payments/Taxes" });
                //objProspect.objNeedAnalysis.dlladversities.Add(new SelectListItem { Text = "Loss of Income" });
                //objProspect.objNeedAnalysis.dlladversities.Add(new SelectListItem { Text = "Major Surgeries" });
                //objProspect.objNeedAnalysis.dlladversities.Add(new SelectListItem { Text = "Pre and Post Hospitalization Expenses" });

                //objProspect.objNeedAnalysis.dllannualamount.Add(new SelectListItem { Text = "Above LKR 500,000" });
                //objProspect.objNeedAnalysis.dllannualamount.Add(new SelectListItem { Text = "Below LKR 100,000" });
                //objProspect.objNeedAnalysis.dllannualamount.Add(new SelectListItem { Text = "LKR 100,000 - 200,000" });
                //objProspect.objNeedAnalysis.dllannualamount.Add(new SelectListItem { Text = "LKR 200,000 - 300,000" });
                //objProspect.objNeedAnalysis.dllannualamount.Add(new SelectListItem { Text = "LKR 300,000 - 400,000" });
                //objProspect.objNeedAnalysis.dllannualamount.Add(new SelectListItem { Text = "LKR 400,000 - 500,000" });

                //objProspect.objNeedAnalysis.dllcoverage.Add(new SelectListItem { Text = "Global" });
                //objProspect.objNeedAnalysis.dllcoverage.Add(new SelectListItem { Text = "Local" });

                //objProspect.objNeedAnalysis.dlladequacy.Add(new SelectListItem { Text = "No" });
                //objProspect.objNeedAnalysis.dlladequacy.Add(new SelectListItem { Text = "Yes" });

                //objProspect.objNeedAnalysis.dllRelationship.Add(new SelectListItem { Text = "Daughter" });
                //objProspect.objNeedAnalysis.dllRelationship.Add(new SelectListItem { Text = "Son" });
                //objProspect.Signature = Convert.ToString(byteArrayToImage(objProspect.objNeedAnalysis.ProspectSign));
                objProspect.objNeedAnalysis.Stage = "Prospect";
                objProspect.objNeedAnalysis.dllChildName.Add(new MasterListItem { Text = "OTHER", Value = "OTHER" });
                foreach (PreviousInsuranceList item in objProspect.objPreviousInsuranceList)
                {
                    PrevPolicy obj = new PrevPolicy();
                    obj.PolicyNo = item.PolicyNumber;
                    obj.MaturityFund = 0;
                    objProspect.objNeedAnalysis.objPrevPolicy.Add(obj);
                }
                for (int i = 0; i < objProspect.objNeedAnalysis.objFinancialNeeds.Count; i++)
                {
                    if (objProspect.objNeedAnalysis.objFinancialNeeds[i].Name != null)
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
                }
                return View("~/Views/Prospect/CreateProspect.cshtml", objProspect);
            }
            catch (Exception e)
            {
                return View();
            }
        }
        public ActionResult GetChildren(Prospect objProspect)
        {
            int NoOfChild = Convert.ToInt32(objProspect.objNeedAnalysis.DependantCount);

            LifeQuote objLifeQuote = new LifeQuote();
            objLifeQuote.Contactid = objProspect.ContactID;
            objLifeQuote = objProspectBusiness.LoadProspectMaster(objLifeQuote);
            objProspect.objNeedAnalysis.dllChildRelatioship = objLifeQuote.objProspect.objNeedAnalysis.dllChildRelatioship;
            var obj = objProspectBusiness.LoadContactInformation(objProspect);
            objProspect.objNeedAnalysis.objDependants = obj.objNeedAnalysis.objDependants;
            var ChildCount = objProspect.objNeedAnalysis.objDependants.Count;
            //objProspect.objNeedAnalysis.DependantCount = NoOfChild;
            //objProspect.objNeedAnalysis.dllChildRelatioship.Add(new MasterListItem { Text = "Son", Value = "Son" });
            //objProspect.objNeedAnalysis.dllChildRelatioship.Add(new MasterListItem { Text = "Daughter", Value = "Daughter" });

            if (objProspect.objNeedAnalysis.objDependants.Count < NoOfChild)
            {
                for (int i = NoOfChild; i > ChildCount; i--)
                {
                    objProspect.objNeedAnalysis.objDependants.Add(new Dependants());
                }
            }
            
            return PartialView("~/Views/Prospect/_PartialChildren.cshtml", objProspect);
        }

        public ActionResult QuoteGetChildren(QuoteList objQuote) 
        {
            int NoOfChild = Convert.ToInt32(objQuote.objProspect.objNeedAnalysis.objDependants.Count);
            objQuote.objProspect.objNeedAnalysis.DependantCount = NoOfChild;
            LifeQuote objLifeQuote = new LifeQuote();
            objLifeQuote.Contactid = objQuote.objProspect.ContactID;
            objLifeQuote = objProspectBusiness.LoadProspectMaster(objLifeQuote);
            objQuote.objProspect.objNeedAnalysis.dllChildRelatioship = objLifeQuote.objProspect.objNeedAnalysis.dllChildRelatioship;
            objQuote.objProspect.objNeedAnalysis.objDependants = objLifeQuote.objProspect.objNeedAnalysis.objDependants;
            var ChildCount = objQuote.objProspect.objNeedAnalysis.objDependants.Count;
            //objProspect.objNeedAnalysis.DependantCount = NoOfChild;
            //objProspect.objNeedAnalysis.dllChildRelatioship.Add(new MasterListItem { Text = "Son", Value = "Son" });
            //objProspect.objNeedAnalysis.dllChildRelatioship.Add(new MasterListItem { Text = "Daughter", Value = "Daughter" });

            if (objQuote.objProspect.objNeedAnalysis.objDependants.Count < NoOfChild)
            {
                for (int i = NoOfChild; i > ChildCount; i--)
                {
                    objQuote.objProspect.objNeedAnalysis.objDependants.Add(new Dependants());
                }
            }

            return PartialView("~/Views/Prospect/_PartialChildren.cshtml", objQuote.objProspect);
        }
        public ActionResult QuotationGetChildren(int ContactID, int DependantCount)
        {
            int NoOfChild = Convert.ToInt32(DependantCount);
            LifeQuote objLifeQuote = new LifeQuote();
            objLifeQuote.Contactid = ContactID;
            objLifeQuote = objProspectBusiness.LoadProspectMaster(objLifeQuote);
            //objQuote.objProspect.objNeedAnalysis.dllChildRelatioship = objLifeQuote.objProspect.objNeedAnalysis.dllChildRelatioship;
            //objQuote.objProspect.objNeedAnalysis.objDependants = objLifeQuote.objProspect.objNeedAnalysis.objDependants;
            var ChildCount = objLifeQuote.objProspect.objNeedAnalysis.objDependants.Count;
            //objProspect.objNeedAnalysis.DependantCount = NoOfChild;
            //objProspect.objNeedAnalysis.dllChildRelatioship.Add(new MasterListItem { Text = "Son", Value = "Son" });
            //objProspect.objNeedAnalysis.dllChildRelatioship.Add(new MasterListItem { Text = "Daughter", Value = "Daughter" });

            if (objLifeQuote.objProspect.objNeedAnalysis.objDependants.Count < NoOfChild)
            {
                for (int i = NoOfChild; i > ChildCount; i--)
                {
                    objLifeQuote.objProspect.objNeedAnalysis.objDependants.Add(new Dependants());
                }
            }
            objLifeQuote.objProspect.objNeedAnalysis.DependantCount = NoOfChild;
            return PartialView("~/Views/Prospect/_PartialChildren.cshtml", objLifeQuote.objProspect);
        }
        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }
        /// <summary>
        /// Creates an instance of an MVC controller from scratch 
        /// when no existing ControllerContext is present       
        /// </summary>
        /// <typeparam name="T">Type of the controller to create</typeparam>
        /// <returns>Controller Context for T</returns>
        /// <exception cref="InvalidOperationException">thrown if HttpContext not available</exception>
        public static T CreateController<T>(RouteData routeData = null)
                    where T : Controller, new()
        {
            // create a disconnected controller instance
            T controller = new T();

            // get context wrapper from HttpContext if available
            HttpContextBase wrapper = null;
            if (System.Web.HttpContext.Current != null)
                wrapper = new HttpContextWrapper(System.Web.HttpContext.Current);
            else
                throw new InvalidOperationException(
                    "Can't create Controller Context if no active HttpContext instance is available.");

            if (routeData == null)
                routeData = new RouteData();

            // add the controller routing if not existing
            if (!routeData.Values.ContainsKey("controller") && !routeData.Values.ContainsKey("Controller"))
                routeData.Values.Add("controller", controller.GetType().Name
                                                            .ToLower()
                                                            .Replace("controller", ""));

            controller.ControllerContext = new ControllerContext(wrapper, routeData, controller);
            return controller;
        }

    }
}
