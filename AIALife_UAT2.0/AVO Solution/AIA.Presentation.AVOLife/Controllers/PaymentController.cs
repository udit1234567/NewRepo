using Grid.Mvc.Ajax.GridExtensions;
using AIA.Life.Models.Integration.Payment;
using AIA.Life.Models.Payment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using AIA.Life.Repository.AIAEntity;
using gateway_client_csharp.au.com.gateway.client;
using gateway_client_csharp.au.com.gateway.client.payment;
using gateway_client_csharp.au.com.gateway.client.config;
using gateway_client_csharp.au.com.gateway.client.enums;
using gateway_client_csharp.au.com.gateway.client.component;
using System.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;
using AIA.Life.Models.Common;
using log4net;
using AIA.CrossCutting;
using AIA.Life.Business.Payment;
using AIA.Presentation.AVOLife.ExceptionHandling;

namespace AIA.Presentation.AVOLife.Controllers
{

    [Authorize]
    [ErrorLogging]
    [SessionTimeout]
    public class PaymentController : BaseController
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        AVOAIALifeEntities Context = new AVOAIALifeEntities();
        PaymentBusiness objPaymentBusiness = new PaymentBusiness();
        private string Username = string.Empty;
        public PaymentController()
        {
            Username = System.Web.HttpContext.Current.User.Identity.Name;
        }
        // GET: Payment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Home()
        {

            return View();
        }

        public ActionResult MyCollections()
        {
            return View();
        }

        public ActionResult MyCollectionProposalPayment()
        {
            PaymentServiceModel objPaymentModel = new PaymentServiceModel();
            //objPaymentModel = objPaymentBusiness.FetchPaymentProposals(objPaymentModel);
            if (objPaymentModel.ObjPaymentProposalPool == null)
            {
                objPaymentModel.ObjPaymentProposalPool = new List<PaymentProposal>();
            }
            return View(objPaymentModel);
        }

        public ActionResult PaymentPolicyView()
        {
            PaymentServiceModel objPaymentModel = new PaymentServiceModel();
            TempData["Load"] = "FirstTime";
            //objPaymentModel = objPaymentBusiness.FetchRenewedAllPolicies(objPaymentModel);
            return View(objPaymentModel);
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
        //[HttpPost]
        [AllowAnonymous]
        public ActionResult AppProposalPayment(string QuoteNo, string UserName, string CloseWindow = "false")
        {
            if(!string.IsNullOrEmpty(UserName))
                Username = CrossCutting.CrossCutting_EncryptDecrypt.Decrypt(UserName);
            //  return RedirectToAction("ProposalPayment", new { QuoteNo = QuoteNo, AppUserName = Username, CloseWindow = CloseWindow });
            return ProposalPaymentInternal(QuoteNo, Username, CloseWindow);
        }

        public ActionResult ProposalPayment(string QuoteNo, string AppUserName = null, string CloseWindow = "false")
        {
            return ProposalPaymentInternal(QuoteNo, Username, CloseWindow);
        }
        private ActionResult ProposalPaymentInternal(string QuoteNo, string AppUserName = null, string CloseWindow = "false")
        {
            PaymentModel objPaymentModel = new PaymentModel();
            objPaymentModel.QuoteNo = CrossCutting.CrossCutting_EncryptDecrypt.Decrypt(QuoteNo);

            #region Check user authorization
            AuthorizeUser authorizeUser = new AuthorizeUser();
            if (!string.IsNullOrEmpty(AppUserName))
            {
                authorizeUser.UserName = objPaymentModel.UserName = AppUserName;
            }
            else
            {
                authorizeUser.UserName = objPaymentModel.UserName = Username;

            }
            authorizeUser.QuoteNo = objPaymentModel.QuoteNo;
            authorizeUser = new AIA.Life.Business.Common.CommonBusiness().CheckAuthorisation(authorizeUser);
            if (!string.IsNullOrEmpty(authorizeUser.Error.ErrorMessage))
            {
                return RedirectToAction("AuthorizeError", "Error", new { errorMessage = authorizeUser.Error.ErrorMessage });
            }
            #endregion

            objPaymentModel.CloseWindow = CloseWindow;
            TempData["CloseWindow"] = CloseWindow;
            objPaymentModel = objPaymentBusiness.FetchProposals(objPaymentModel);
            if (objPaymentModel.lstPaymentItems.Count > 0)
            {
                int planID = Convert.ToInt32(objPaymentModel.lstPaymentItems[0].PlanId);
                var obj = Context.tblMasProductPlans.Where(a => a.PlanId == planID).FirstOrDefault();
                if (objPaymentModel.lstPaymentItems[0].PrefferedMode == "12")
                {
                    switch (obj.PlanCode)
                    {
                        case "SBB":
                            {
                                if (objPaymentModel.lstPaymentItems[0].Premium < 12000)
                                {
                                    objPaymentModel.lstPaymentItems[0].Premium = objPaymentModel.lstPaymentItems[0].Premium * 2;
                                }
                                break;
                            }
                        case "SBF":
                            {
                                objPaymentModel.lstPaymentItems[0].Premium = objPaymentModel.lstPaymentItems[0].Premium * 3;
                                break;
                            }
                        case "PPG":
                            {
                                objPaymentModel.lstPaymentItems[0].Premium = objPaymentModel.lstPaymentItems[0].Premium * 3;
                                break;
                            }
                        case "PPH":
                            {
                                if (objPaymentModel.lstPaymentItems[0].Premium < 12000)
                                {
                                    objPaymentModel.lstPaymentItems[0].Premium = objPaymentModel.lstPaymentItems[0].Premium * 2;
                                }
                                break;
                            }
                        case "EPB":
                            {
                                if (objPaymentModel.lstPaymentItems[0].Premium < 12000)
                                {
                                    objPaymentModel.lstPaymentItems[0].Premium = objPaymentModel.lstPaymentItems[0].Premium * 2;
                                }
                                break;
                            }
                    }
                }
            }
            return View("~/Views/Payment/ProposalPayment.cshtml", objPaymentModel);
        }
        [AllowAnonymous]
        public JsonResult SaveProposalPaymentInfo(PaymentModel objPaymentModel)
        {
            objPaymentModel.UserName = Username;

            objPaymentModel.QuoteNo = CrossCutting.CrossCutting_EncryptDecrypt.Decrypt(objPaymentModel.QuoteNo);

            objPaymentModel = objPaymentBusiness.SaveProposalPaymentInfo(objPaymentModel);

            #region policy issuance triggers
            if (!string.IsNullOrEmpty(objPaymentModel.Message) && objPaymentModel.Message == "Success" && string.IsNullOrEmpty(objPaymentModel.UWMessage))
            {
                AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
                ReportsController objReportController = new ReportsController();
                List<byte[]> lstBytes = new List<byte[]>();
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
                            }
                            objPaymentModel.ByteArray3 = objReportController.ProposalReport(objPaymentModel.QuoteNo, ProductCode, objPaymentModel.lstPaymentItems[0].PreferredLanguage);
                            if (objPaymentModel.ByteArray2 != null)
                                lstBytes.Add(objPaymentModel.ByteArray2);
                            if (objPaymentModel.ByteArray3 != null)
                                lstBytes.Add(objPaymentModel.ByteArray3);
                            
                        }
                   

                    objPolicyBusiness.PostPolicyIssuanceTriggers(objPaymentModel);
                }
                catch (Exception e)
                {
                    log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                    Logger.Error(e);
                }
                try
                {
                    for (int i = 0; i < lstBytes.Count; i++)
                    {
                        LdmsDocuments documents = new LdmsDocuments();
                        documents.SourcePath = ConfigurationManager.AppSettings["DocumentUploadPath"];
                        documents.DocCode = i == 0 ? "PRD004" : (i == 1 ? "PRD009" : (i == 2 ? "PRD001" : ""));
                        documents.AgentCode = Username;
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
            }
            #endregion

            var ObjResponse = new { Message = objPaymentModel.Message, ProposalNo = objPaymentModel.ProposalNo, UWMessage = objPaymentModel.UWMessage, ErrorMessage = objPaymentModel.Error.ErrorMessage };
            return Json(ObjResponse, JsonRequestBehavior.AllowGet);

        }
        [AllowAnonymous]
        public ActionResult GetTransationNumber()
        {
            var ObjTransationNumber = AIA.CrossCutting.Codes.GetTransationNo();
            return Json(ObjTransationNumber, JsonRequestBehavior.AllowGet);

        }
        [AllowAnonymous]
        public JsonResult MCashPayment(PaymentModel objPaymentModel)
        {
            objPaymentModel.UserName = Username;
            objPaymentModel = objPaymentBusiness.MCashPayment(objPaymentModel);
            #region policy issuance triggers
            if (!string.IsNullOrEmpty(objPaymentModel.Message) && objPaymentModel.Message == "Success")
            {
                objPaymentModel = objPaymentBusiness.FetchProposals(objPaymentModel);
                AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
                ReportsController objReportController = new ReportsController();
                List<byte[]> lstBytes = new List<byte[]>();
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

                    objPolicyBusiness.PostPolicyIssuanceTriggers(objPaymentModel);
                }
                catch (Exception e)
                {
                    log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                    Logger.Error(e);
                }
                try
                {
                    for (int i = 0; i < lstBytes.Count; i++)
                    {
                        LdmsDocuments documents = new LdmsDocuments();
                        documents.SourcePath = ConfigurationManager.AppSettings["DocumentUploadPath"];
                        documents.DocCode = i == 0 ? "PRD004" : (i == 1 ? "PRD009" : (i == 2 ? "PRD001" : ""));
                        documents.AgentCode = Username;
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
            }
            #endregion
            var ObjResponse = new { Message = objPaymentModel.Message, ProposalNo = objPaymentModel.ProposalNo, UWMessage = objPaymentModel.UWMessage, ErrorMessage = objPaymentModel.Error.ErrorMessage };
            return Json(ObjResponse, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult SampathBankPG(PaymentModel objPaymentModel)
        {
            objPaymentModel.UserName = Username;
            if (TempData.ContainsKey("CloseWindow"))
                TempData["CloseWindows"]  = TempData["CloseWindow"].ToString();
            GatewayClient client = new GatewayClient(make_ClientConfig_Sampath());
            PaymentInitResponse paymentInitResponse = client.payment.init(make_PaymentInitRequest(objPaymentModel));
            objPaymentModel.ReqId = paymentInitResponse.reqid;
            objPaymentModel = objPaymentBusiness.SavePGTransaction(objPaymentModel);

            return View("~/Views/Payment/LoadSampathPG.cshtml", paymentInitResponse);
        }

        [AllowAnonymous]
        public ActionResult OnlinePaymentResponse(string clientRef, string reqid)
        {
            PaymentCompleteRequest paymentCompleteRequest = new PaymentCompleteRequest();
            paymentCompleteRequest.reqid = reqid;
            return View("~/Views/Payment/ProcessPayment.cshtml", paymentCompleteRequest);
        }
        [AllowAnonymous]
        public ActionResult ProcessSampathBankResponse(PaymentCompleteRequest paymentCompleteRequest)
        {
            if (TempData.ContainsKey("CloseWindows"))
                ViewBag.CloseWindow = TempData["CloseWindows"].ToString();
            int clientID = Convert.ToInt32(ConfigurationManager.AppSettings["CLIENTID"].ToString());
            paymentCompleteRequest.clientId = clientID;
            GatewayClient gatewayClient = new GatewayClient(make_ClientConfig_Sampath());
            PaymentCompleteResponse paymentCompleteResponse = gatewayClient.payment.complete(paymentCompleteRequest);
            PaymentModel objPaymentModel = new PaymentModel();
            objPaymentModel.UserName = Username;
            objPaymentModel.ReqId = paymentCompleteRequest.reqid;
            objPaymentModel.PayableAmount = Convert.ToString(Convert.ToInt32(paymentCompleteResponse.transactionAmount.paymentAmount/100));
            objPaymentModel.PGResponse = paymentCompleteResponse.responseCode + "|" + paymentCompleteResponse.responseText;
            objPaymentModel = objPaymentBusiness.UpdatePGTransaction(objPaymentModel);
            string msg = string.Empty;
            msg = objPaymentModel.Error.ErrorMessage;
            ViewBag.Message = msg;
            if (string.IsNullOrEmpty(objPaymentModel.Error.ErrorMessage))
            {
                AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
                if (!string.IsNullOrEmpty(objPaymentModel.UWMessage))
                {
                    msg = objPaymentModel.UWMessage;
                }
                else if (objPaymentModel.Message == "Success")
                {
                    msg = "Your policy has been issued. Please refer policy number " + objPaymentModel.ProposalNo + " for future reference";
                    #region policy issuance triggers
                    if (!string.IsNullOrEmpty(objPaymentModel.Message) && objPaymentModel.Message == "Success")
                    {
                        objPaymentModel = objPaymentBusiness.FetchProposals(objPaymentModel);
                        ReportsController objReportController = new ReportsController();
                        List<byte[]> lstBytes = new List<byte[]>();
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
                            objPolicyBusiness.PostPolicyIssuanceTriggers(objPaymentModel);
                        }
                        catch (Exception e)
                        {
                            log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                            Logger.Error(e);
                        }
                        try
                        {
                            for (int i = 0; i < lstBytes.Count; i++)
                            {
                                LdmsDocuments documents = new LdmsDocuments();
                                documents.SourcePath = ConfigurationManager.AppSettings["DocumentUploadPath"];
                                documents.DocCode = i == 0 ? "PRD004" : (i == 1 ? "PRD009" : (i == 2 ? "PRD001" : ""));
                                documents.AgentCode = Username;
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
                    }
                    #endregion
                }
                else
                {
                    msg = objPaymentModel.Message;
                }

                ViewBag.Message = msg;
                return View("~/Views/Payment/PaymentSuccess.cshtml");
            }
            else
                return View("~/Views/Payment/PaymentFailure.cshtml");
        }
        private ClientConfig make_ClientConfig_Sampath()
        {
            string serviceEndPoint = ConfigurationManager.AppSettings["SAMPATHSERVICE"].ToString();
            string hmaSecret = ConfigurationManager.AppSettings["HMASECRET"].ToString();
            string authToken = ConfigurationManager.AppSettings["AUTHTOKEN"].ToString();
            ClientConfig clientConfig = new ClientConfig();
            clientConfig.serviceEndpoint = serviceEndPoint;
            clientConfig.authToken = authToken;
            clientConfig.hmacSecret = hmaSecret;
            return clientConfig;
        }
        private PaymentInitRequest make_PaymentInitRequest(PaymentModel paymentModel)
        {
            int clientID = Convert.ToInt32(ConfigurationManager.AppSettings["CLIENTID"].ToString());
            PaymentInitRequest paymentInitRequest = new PaymentInitRequest();
            paymentInitRequest.clientId = clientID;
            paymentInitRequest.transactionType = Enums.TransactionType.PURCHASE.ToString();
            paymentInitRequest.clientRef = "NexGen iPoS";
            paymentInitRequest.comment = "Payment Transaction";
            paymentInitRequest.transactionAmount = make_TransactionAmount(paymentModel);
            paymentInitRequest.redirect = make_Redirect();
            paymentInitRequest.tokenize = false;

            return paymentInitRequest;
        }

        private TransactionAmount make_TransactionAmount(PaymentModel paymentModel)
        {

            TransactionAmount transactionAmount = new TransactionAmount();
            if (ConfigurationManager.AppSettings["PublishEnvironment"].ToString() == "SIT" || ConfigurationManager.AppSettings["PublishEnvironment"].ToString() == "UAT")
                transactionAmount.paymentAmount = 100;
            else
                transactionAmount.paymentAmount = (int.Parse(paymentModel.PayableAmount) * 100);
            transactionAmount.currency = "LKR";

            return transactionAmount;
        }

        private Redirect make_Redirect()
        {
            Redirect redirect = new Redirect();
            redirect.returnUrl = ConfigurationManager.AppSettings["SAMPATHRESPONSEURL"].ToString(); ;
            redirect.returnMethod = "POST";

            return redirect;
        }
        public FileResult TestPDF(string QuoteNo, string PolicyNo, string ProductCode, string PreferredLanguage, string ISAfc, HttpContextBase context = null)
        {
            //string proposalNo = "50132406";
            //string quoteNo = "Q180006564-01";
            //string planCode = "PPG";
            ReportsController objReportController = new ReportsController();
          
           
                byte[] a = objReportController.ReportForIllustrationPDF(QuoteNo, ProductCode, PreferredLanguage, PolicyNo);
                byte[] b = objReportController.ReportForPolicySchedule(PolicyNo, ProductCode, PreferredLanguage);
                byte[] c = null;
                if (ISAfc == "True")
                {
                    c = objReportController.ReportForAFCCoveringLetter(PolicyNo, ProductCode, PreferredLanguage);
                }
                else
                {
                    c = objReportController.ReportForCoveringLetter(PolicyNo, ProductCode, PreferredLanguage);
                }

                List<byte[]> vs = new List<byte[]>();
                vs.Add(c);
                vs.Add(b);
                vs.Add(a);
                PaymentController objPayment = new Controllers.PaymentController();
                return File(concatAndAddContent(vs), "application/pdf", PolicyNo + ".pdf");
          
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
    }
}