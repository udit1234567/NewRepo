using AIA.CrossCutting;
using AIA.Life.Models.Policy;
using AIA.Life.Models.UserManagement;
using Ionic.Zip;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Google.Apis.Auth.OAuth2;
using System.Text.RegularExpressions;
using Grpc.Auth;
using AIA.Life.Models.Common;
using AIA.Life.Models.Payment;
using System.Web;
using Google.Apis.Vision.v1;
using Google.Apis.Http;
using Newtonsoft.Json;
using Google.Apis.Services;
using Google.Apis.Vision.v1.Data;
using AIA.Life.Repository.AIAEntity;
using AIA.Life.Business.Common;
using AIA.Life.Business.UserManagement;
using AIA.Life.Business.Payment;
using AIA.Life.Business.Policy;

namespace AIA.Presentation.AVOLife.Controllers
{
    public class AIAServiceController : ApiController
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        CommonBusiness commonBusiness = new CommonBusiness();
        public IMOUsers PushUserDetails(IMOUsers objIMOUsers)
        {
            TpServiceLog tpServiceLog = new TpServiceLog();
            tpServiceLog.ServiceRequest = Newtonsoft.Json.JsonConvert.SerializeObject(objIMOUsers);
            TransactLog transactLog = new TransactLog();
            transactLog.SerivceTraceID = tpServiceLog.ServiceTraceID = objIMOUsers.ServiceTraceID;
            transactLog.UserName = objIMOUsers.CreatedBy;
            string Message = ValidateUserAuth(transactLog);
            if (Message != "" && Message != null)
            {
                objIMOUsers.Message = Message;
                return objIMOUsers;
            }

            UserManagementBusiness objUserManagementBusiness = new UserManagementBusiness();
            objIMOUsers = objUserManagementBusiness.FilltblMasIMOUserDetails(objIMOUsers);

            tpServiceLog.ServiceResponse = Newtonsoft.Json.JsonConvert.SerializeObject(objIMOUsers);
            commonBusiness.CreateServiceLog(tpServiceLog);
            return objIMOUsers;
        }
        [HttpPost]
        public AIA.Life.Models.Opportunity.LifeQuote SaveQuote(AIA.Life.Models.Opportunity.LifeQuote ObjQuote)
        {
            TpServiceLog tpServiceLog = new TpServiceLog();
            tpServiceLog.ServiceRequest = Newtonsoft.Json.JsonConvert.SerializeObject(ObjQuote);
            TransactLog transactLog = new TransactLog();
            transactLog.SerivceTraceID = tpServiceLog.ServiceTraceID = ObjQuote.ServiceTraceID;
            transactLog.UserName = ObjQuote.UserName;
            string Message = ValidateUserAuth(transactLog);
            if (Message != "" && Message != null)
            {
                ObjQuote.Message = Message;
                return ObjQuote;
            }
            string ObjSuspect = Newtonsoft.Json.JsonConvert.SerializeObject(ObjQuote);
            #region Proposer Signature
            if (ObjQuote.Signature != null)
            {
                ObjQuote.ProspectSign = Convert.FromBase64String(ObjQuote.Signature);
                string Proposerfilename = "Proposersignature.png";
                string ProposerdirectryPath = "";
                ProposerdirectryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Policy/ContactID_" + ObjQuote.objProspect.ContactID);
                //ProposerdirectryPath = HttpContext.Current.Server.MapPath("ContactID_" + ObjQuote.objProspect.ContactID);
                if (!Directory.Exists(ProposerdirectryPath))
                {
                    Directory.CreateDirectory(ProposerdirectryPath);
                }
                string Proposerfilepath = Path.Combine(ProposerdirectryPath, Proposerfilename);
                System.IO.File.WriteAllBytes(Proposerfilepath, ObjQuote.ProspectSign);
                ObjQuote.ProposerSignPath = Path.Combine("\\Policy", "ContactID_" + ObjQuote.objProspect.ContactID, Proposerfilename);
            }
            #endregion
            #region Spouse Signature
            if (ObjQuote.WPProposerSignature != null)
            {
                ObjQuote.WPSignature = Convert.FromBase64String(ObjQuote.WPProposerSignature);
                string WPfilename = "WPProposersignature.png";
                string WPdirectryPath = "";
                //WPdirectryPath = HttpContext.Current.Server.MapPath("ContactID_" + ObjQuote.objProspect.ContactID);
                WPdirectryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Policy/ContactID_" + ObjQuote.objProspect.ContactID);
                if (!Directory.Exists(WPdirectryPath))
                {
                    Directory.CreateDirectory(WPdirectryPath);
                }
                string WPfilepath = Path.Combine(WPdirectryPath, WPfilename);
                System.IO.File.WriteAllBytes(WPfilepath, ObjQuote.WPSignature);
                ObjQuote.WPProposerSignPath = Path.Combine("\\Policy", "ContactID_" + ObjQuote.objProspect.ContactID, WPfilename);
            }
            #endregion
            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            ObjQuote= objQuoteBusiness.SaveQuote(ObjQuote);
            string QuoteFilePath = ConfigurationManager.AppSettings["QuoteFilePath"];
            if (ObjQuote.Message == "Success")
            {
                try
                {
                    ReportsController objReportController = new ReportsController();
                    var byteArray = objReportController.ReportQuotation(ObjQuote.QuoteNo, ObjQuote.objProductDetials.PlanCode, ObjQuote.objProductDetials.PreferredLangauage);

                    ObjQuote.QuotePDFPath = QuoteFilePath + ObjQuote.QuoteNo + "_Quote.pdf";
                    System.IO.File.WriteAllBytes(ObjQuote.QuotePDFPath, byteArray);
                    ObjQuote.ByteArray = byteArray;
                    objQuoteBusiness.SendEmailAndSMSNotificationOnQuoteCreation(ObjQuote);
                }
                catch (Exception e)
                {
                    log4net.GlobalContext.Properties["ErrorCode"] = ObjQuote.Error.ErrorCode = Codes.GetErrorCode();
                    Logger.Error(e);
                    ObjQuote.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + ObjQuote.Error.ErrorCode + ". Sorry for the inconvenience caused";

                }
            }

            tpServiceLog.ServiceResponse = Newtonsoft.Json.JsonConvert.SerializeObject(ObjQuote);
            commonBusiness.CreateServiceLog(tpServiceLog);
            return ObjQuote;
        }
        public AIA.Life.Models.Policy.Policy SaveProposal(AIA.Life.Models.Policy.Policy objpolicy)
        {
            TpServiceLog tpServiceLog = new TpServiceLog();
            tpServiceLog.ServiceRequest = Newtonsoft.Json.JsonConvert.SerializeObject(objpolicy);
            TransactLog transactLog = new TransactLog();
            transactLog.SerivceTraceID = tpServiceLog.ServiceTraceID = objpolicy.ServiceTraceID;
            transactLog.UserName = objpolicy.UserName;
            string Message = ValidateUserAuth(transactLog);
            if (Message != "" && Message != null)
            {
                objpolicy.Message = Message;
                return objpolicy;
            }
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();

            if (objpolicy.MainLifeSign != null)
            {
                objpolicy.ProposerSignatureFile = Convert.FromBase64String(objpolicy.MainLifeSign);
            }
            if (objpolicy.SpouseLifeSign != null)
            {
                objpolicy.SpouseSignatureFile = Convert.FromBase64String(objpolicy.SpouseLifeSign);
            }
            if (objpolicy.WPSign != null)
            {
                objpolicy.WPSignatureFile = Convert.FromBase64String(objpolicy.WPSign);
            }
            objpolicy = objPolicyBusiness.SaveProposal(objpolicy);
            objpolicy = objPolicyBusiness.InvokeILModifyProposal(objpolicy);
            string QuoteFilePath = ConfigurationManager.AppSettings["QuoteFilePath"];
            if (objpolicy.Message == "Success")
            {

                try
                {
                    AVOAIALifeEntities Context = new AVOAIALifeEntities();
                    var prdID = Context.tblMasProductPlans.Where(a => a.PlanCode == objpolicy.PlanCode).Select(a => a.ProductId).FirstOrDefault();
                    var ProductCode = Context.tblProducts.Where(a => a.ProductId == prdID).Select(a => a.ProductCode).FirstOrDefault();
                    byte[] byteArray = null;
                    ReportsController objReportController = new ReportsController();
                    try
                    {
                        byteArray = objReportController.ProposalReport(objpolicy.QuoteNo, ProductCode, objpolicy.PreferredLanguage);
                    }
                    catch (Exception)
                    {

                        objpolicy.ByteArray = null;
                    }

                    try
                    {
                        objpolicy.ProposalFilePath = QuoteFilePath + objpolicy.QuoteNo + "_Proposal.pdf";
                        System.IO.File.WriteAllBytes(objpolicy.ProposalFilePath, byteArray);
                    }
                    catch (Exception ex)
                    {

                        objpolicy.ProposalFilePath = null;
                    }

                }
                catch (Exception e)
                {
                    log4net.GlobalContext.Properties["ErrorCode"] = objpolicy.Error.ErrorCode = Codes.GetErrorCode();
                    Logger.Error(e);
                    objpolicy.Error.ErrorMessage = "Please inform the IT HelpDesk on this application issue. Error Code is " + objpolicy.Error.ErrorCode + ". Sorry for the inconvenience caused";

                }
            }

            tpServiceLog.ServiceResponse = Newtonsoft.Json.JsonConvert.SerializeObject(objpolicy);
            commonBusiness.CreateServiceLog(tpServiceLog);
            return objpolicy;
        }
        public AIA.Life.Models.Policy.Policy LoadProposalInfo(AIA.Life.Models.Policy.Policy objpolicy)
        {
            TpServiceLog tpServiceLog = new TpServiceLog();
            tpServiceLog.ServiceRequest = Newtonsoft.Json.JsonConvert.SerializeObject(objpolicy);
            TransactLog transactLog = new TransactLog();
            transactLog.SerivceTraceID = tpServiceLog.ServiceTraceID = objpolicy.ServiceTraceID;
            transactLog.UserName = objpolicy.UserName;
            string Message = ValidateUserAuth(transactLog);
            if (Message != "" && Message != null)
            {
                objpolicy.Message = Message;
                return objpolicy;
            }
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            objpolicy = objPolicyBusiness.LoadProposalInfo(objpolicy);
            objPolicyBusiness.InvokeILWorkFlowAck(objpolicy);

            tpServiceLog.ServiceResponse = Newtonsoft.Json.JsonConvert.SerializeObject(objpolicy);
            commonBusiness.CreateServiceLog(tpServiceLog);
            return objpolicy;
        }
        public AIA.Life.Models.Common.ProposalDetails LoadPreviousProposalDetails(AIA.Life.Models.Common.ProposalDetails objProposal)
        {
            TpServiceLog tpServiceLog = new TpServiceLog();
            tpServiceLog.ServiceRequest = Newtonsoft.Json.JsonConvert.SerializeObject(objProposal);
            TransactLog transactLog = new TransactLog();
            transactLog.SerivceTraceID = tpServiceLog.ServiceTraceID = objProposal.ServiceTraceID;
            transactLog.UserName = objProposal.UserName;
            string Message = ValidateUserAuth(transactLog);
            if (Message != "" && Message != null)
            {
                objProposal.Message = Message;
                return objProposal;
            }
            
            objProposal = commonBusiness.GetPolicyDetails(objProposal);

            tpServiceLog.ServiceResponse = Newtonsoft.Json.JsonConvert.SerializeObject(objProposal);
            commonBusiness.CreateServiceLog(tpServiceLog);
            return objProposal;
        }
        public AIA.Life.Models.Common.SARFALDetails FetchSarAndFalDetails(AIA.Life.Models.Common.SARFALDetails sARFALDetails)
        {
            TpServiceLog tpServiceLog = new TpServiceLog();
            tpServiceLog.ServiceRequest = Newtonsoft.Json.JsonConvert.SerializeObject(sARFALDetails);
            TransactLog transactLog = new TransactLog();
            transactLog.SerivceTraceID = tpServiceLog.ServiceTraceID = sARFALDetails.ServiceTraceID;
            transactLog.UserName = sARFALDetails.UserName;
            string Message = ValidateUserAuth(transactLog);
            if (Message != "" && Message != null)
            {
                sARFALDetails.Message = Message;
                return sARFALDetails;
            }
            
            sARFALDetails = commonBusiness.FetchSarAndFalDetails(sARFALDetails);

            tpServiceLog.ServiceResponse = Newtonsoft.Json.JsonConvert.SerializeObject(sARFALDetails);
            commonBusiness.CreateServiceLog(tpServiceLog);
            return sARFALDetails;
        }

        [HttpPost]
        public string FileUpload(DomFile obj)
        {
            string Result = "";
            try
            {
                if (!string.IsNullOrEmpty(obj.Agent_Code) && !string.IsNullOrEmpty(obj.Policy_No) && !string.IsNullOrEmpty(obj.FileName))
                {
                    //string zipPath = @"E:\AIA\test1.zip";
                    byte[] FileBytes = Convert.FromBase64String(obj.Filedata); //System.IO.File.ReadAllBytes(zipPath); //

                    string Path = ConfigurationManager.AppSettings["DocumentUploadPath"]; //@"E:\AIA\uploaddocsmobility";
                    //bool IsfolderExist = CheckImageExist(obj.Department_Code);

                    //if (IsfolderExist == false)
                    //{
                    //    var directoryInfo1 = new DirectoryInfo(Path);
                    //    directoryInfo1.CreateSubdirectory(obj.Department_Code);
                    //}
                    //Path = Path + @"\" + obj.Department_Code;
                    bool IsfolderExist = CheckImageExist(obj.Agent_Code);
                    if (IsfolderExist == false)
                    {
                        var directoryInfo1 = new DirectoryInfo(Path);
                        directoryInfo1.CreateSubdirectory(obj.Agent_Code);
                    }
                    Path = Path + "//" + obj.Agent_Code;
                    IsfolderExist = CheckImageExist(obj.Agent_Code + @"\" + obj.Policy_No);
                    if (IsfolderExist == false)
                    {
                        var directoryInfo1 = new DirectoryInfo(Path);
                        directoryInfo1.CreateSubdirectory(obj.Policy_No);
                    }
                    Path = Path + @"\" + obj.Policy_No;
                    string zipPath1 = ConfigurationManager.AppSettings["DocumentUploadZipPath"];
                    Random rnd = new Random();
                    obj.FileName = rnd.Next(1, 9999) + obj.FileName;
                    bool isExist = System.IO.File.Exists(zipPath1 + obj.FileName);
                    FileStream fs = new FileStream(zipPath1 + obj.FileName, FileMode.Append);
                    if (FileBytes.Count() > 0)
                    {
                        fs.Write(FileBytes, 0, FileBytes.Length);
                    }
                    fs.Close();

                    #region File validations

                    using (ZipFile zip = ZipFile.Read(zipPath1 + obj.FileName))
                    {
                        foreach (ZipEntry e in zip)
                        {
                            try
                            {
                                using (var stream = e.OpenReader())
                                {

                                    if (!ValidateFileType.IsAllowedFileType(e.FileName, (Stream)stream))
                                    {
                                        Result = "Please Provide Valid File Format";
                                        return Result;
                                    }


                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    #endregion

                    #region Convert to PDF
                    List<DocumentUploadFile> LstDocumentUpload = new List<DocumentUploadFile>();
                    UtilityController objUtilityController = new UtilityController();
                    using (ZipFile zip = ZipFile.Read(zipPath1 + obj.FileName))
                    {
                        foreach (ZipEntry e in zip)
                        {
                            try
                            {
                                using (var stream = e.OpenReader())
                                {
                                    DocumentUploadFile objUpload = new DocumentUploadFile();
                                    MemoryStream ms = new MemoryStream();
                                    e.Extract(ms);
                                    byte[] bytes = new byte[ms.Length];
                                    bytes = objUtilityController.PdfGeneration(ms.ToArray());
                                    System.IO.File.WriteAllBytes(Path + "//" + e.FileName.Substring(0, e.FileName.IndexOf(".")) + ".pdf", bytes);
                                    objUpload.FileName = e.FileName.Split('.')[0].Split('_')[0];
                                    objUpload.FileData = bytes;
                                    LstDocumentUpload.Add(objUpload);

                                }

                                //DocumentUploadFile objUpload = new DocumentUploadFile();
                                //objUpload.FilePath = obj.FileName;.spli
                                //objUpload.ItemType = RequestDoc[i].ItemType;
                                //objUpload.MemberType = RequestDoc[i].MemberType;
                                //objUpload.FileData = fileData;
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }

                    byte[] ConcatFiles;
                    var directryPath = ConfigurationManager.AppSettings["FileUpload"] + "\\" + obj.Agent_Code + "\\" + obj.Policy_No + "\\LDMS";
                    if (!Directory.Exists(directryPath))
                    {
                        Directory.CreateDirectory(directryPath);
                    }
                    foreach (var fName in LstDocumentUpload.Select(a => a.FileName).Distinct().ToList())
                    {
                        ConcatFiles = CrossCutting.DataTypeConvExtention.ConcatPdf(LstDocumentUpload.Where(a => a.FileName == fName).Select(a => a.FileData).ToList());
                        //System.IO.File.WriteAllBytes(ConfigurationManager.AppSettings["LDMSFileUpload"] + "\\" + obj.Agent_Code + "\\" + obj.Policy_No + "\\" + fName + ".pdf", ConcatFiles);
                        System.IO.File.WriteAllBytes(directryPath + "\\" + fName + ".pdf", ConcatFiles);

                    }

                    #endregion

                    //ZipFile fileToExtract = ZipFile.Read(zipPath1 + obj.FileName);
                    //fileToExtract.ExtractAll(Path, ExtractExistingFileAction.DoNotOverwrite);


                    Result = Path;
                }
                else
                {
                    Result = "Kindly provide Department code, Agent Code, Document Name and policy number to proceed..!";

                }
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                Result = "Failure";
            }
            return Result;
        }
        
        public AIA.Life.Models.Policy.Policy SignatureUpload(AIA.Life.Models.Policy.Policy objPolicy)
        {
            try
            {

                #region Proposer Signature
                if (objPolicy.Sign1 != null)
                {
                    objPolicy.ByteArray = Convert.FromBase64String(objPolicy.Sign1);

                    string Proposerfilename = "Proposersignature.png";
                    string ProposerdirectryPath = "";
                    // ProposerdirectryPath = HttpContext.Current.Server.MapPath("ContactID_" + objPolicy.ContactID);
                    // ProposerdirectryPath = Server.MapPath("ContactID_" + objPolicy.ContactID);
                   // WPdirectryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Policy/ContactID_" + ObjQuote.objProspect.ContactID);
                    ProposerdirectryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Policy/ContactID_" + objPolicy.ContactID);
                    if (!Directory.Exists(ProposerdirectryPath))
                    {
                        Directory.CreateDirectory(ProposerdirectryPath);
                    }
                    string Proposerfilepath = Path.Combine(ProposerdirectryPath, Proposerfilename);
                    System.IO.File.WriteAllBytes(Proposerfilepath, objPolicy.ByteArray);
                    objPolicy.ProposalFilePath = Path.Combine("\\Policy", "ContactID_" + objPolicy.ContactID, Proposerfilename);

                }
                #endregion

                #region Spouse Signature
                if (objPolicy.Sign2 != null)
                {
                    objPolicy.ByteArray2 = Convert.FromBase64String(objPolicy.Sign2);

                    string Spousefilename = "Spousesignature.png";
                    string SpousedirectryPath = "";
                    SpousedirectryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Policy/ContactID_" + objPolicy.ContactID);
                    //SpousedirectryPath = HttpContext.Current.Server.MapPath("ContactID_" + objPolicy.ContactID);
                    if (!Directory.Exists(SpousedirectryPath))
                    {
                        Directory.CreateDirectory(SpousedirectryPath);
                    }
                    string Spousefilepath = Path.Combine(SpousedirectryPath, Spousefilename);
                    System.IO.File.WriteAllBytes(Spousefilepath, objPolicy.ByteArray2);
                    objPolicy.SpouseSignature = Path.Combine("\\Policy", "ContactID_" + objPolicy.ContactID, Spousefilename);

                }
                #endregion

                #region WP Signature
                if (objPolicy.Sign3 != null)
                {
                    objPolicy.ByteArray3 = Convert.FromBase64String(objPolicy.Sign3);

                    string WPfilename = "WPProposersignature.png";
                    string WPdirectryPath = "";
                    WPdirectryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Policy/ContactID_" + objPolicy.ContactID);
                    //WPdirectryPath = HttpContext.Current.Server.MapPath("ContactID_" + objPolicy.ContactID);
                    if (!Directory.Exists(WPdirectryPath))
                    {
                        Directory.CreateDirectory(WPdirectryPath);
                    }
                    string WPfilepath = Path.Combine(WPdirectryPath, WPfilename);
                    System.IO.File.WriteAllBytes(WPfilepath, objPolicy.ByteArray3);
                    objPolicy.WPProposerSignature = Path.Combine("\\Policy", "ContactID_" + objPolicy.ContactID, WPfilename);
                    #endregion
                }
                objPolicy.Message = "Success";
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                objPolicy.Error.ErrorMessage = ex.Message;
                objPolicy.Message = "Failure";
            }

            return objPolicy;
        }
        public bool CheckImageExist(string FolderName)
        {
            bool isExist = false;
            //  var fullpathExtrACTED = @"E:\AIA\uploaddocsmobility\";
            var fullpathExtrACTED = ConfigurationManager.AppSettings["DocumentUploadPath"];
            var directoryInfo = new DirectoryInfo(fullpathExtrACTED);
            if (Directory.Exists(fullpathExtrACTED + "/" + FolderName))
            {
                isExist = true;
            }
            return isExist;
        }
        [HttpPost]
        public HttpResponseMessage DownloadFile(DowFile dowFile)
        {
            HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                string format = dowFile.FileFormat;
                string bookPath_Pdf = dowFile.FIlePath;
                string reqBook = bookPath_Pdf;
                string bookName = dowFile.FileName + "." + format.ToLower();
                //converting Pdf file into bytes array  
                var dataBytes = File.ReadAllBytes(reqBook);
                //adding bytes to memory stream   
                var dataStream = new MemoryStream(dataBytes);


                httpResponseMessage.Content = new StreamContent(dataStream);
                httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                httpResponseMessage.Content.Headers.ContentDisposition.FileName = bookName;
                httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
            }
            return httpResponseMessage;
        }

        public AIA.Life.Models.Opportunity.Suspect SaveSuspect(AIA.Life.Models.Opportunity.Suspect objSuspect)
        {
            TpServiceLog tpServiceLog = new TpServiceLog();
            tpServiceLog.ServiceRequest = Newtonsoft.Json.JsonConvert.SerializeObject(objSuspect);
            TransactLog transactLog = new TransactLog();
            transactLog.SerivceTraceID = tpServiceLog.ServiceTraceID = objSuspect.ServiceTraceID;
            transactLog.UserName = objSuspect.UserName;
            string Message = ValidateUserAuth(transactLog);
            if (Message != "" && Message != null)
            {
                objSuspect.Message = Message;
                return objSuspect;
            }
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objSuspect = objProspectBusiness.SaveSuspect(objSuspect);

            tpServiceLog.ServiceResponse = Newtonsoft.Json.JsonConvert.SerializeObject(objSuspect);
            commonBusiness.CreateServiceLog(tpServiceLog);
            return objSuspect;
        }
        public AIA.Life.Models.Opportunity.Prospect FetchNicDetails(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            //string ObjSuspect= Newtonsoft.Json.JsonConvert.SerializeObject(objProspect);
            TpServiceLog tpServiceLog = new TpServiceLog();
            tpServiceLog.ServiceRequest = Newtonsoft.Json.JsonConvert.SerializeObject(objProspect);
            TransactLog transactLog = new TransactLog();
            transactLog.SerivceTraceID = tpServiceLog.ServiceTraceID = objProspect.ServiceTraceID;
            transactLog.UserName = objProspect.UserName;
            string Message = ValidateUserAuth(transactLog);
            if (Message != "" && Message != null)
            {
                objProspect.Message = Message;
                return objProspect;
            }
            if (!string.IsNullOrEmpty(objProspect.NIC))
            {
                objProspect.NICAVAIL = false;
                AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
                objProspect = objProspectBusiness.FetchNicverifyPolicyIL(objProspect);
            }
            else
            {
                objProspect.ErrorMessage = "Nic is not provided";
            }
            tpServiceLog.ServiceResponse = Newtonsoft.Json.JsonConvert.SerializeObject(objProspect);
            commonBusiness.CreateServiceLog(tpServiceLog);
            return objProspect;
        }
        public AIA.Life.Models.Opportunity.Prospect SaveProspect(AIA.Life.Models.Opportunity.Prospect objProspect)
        {
            TpServiceLog tpServiceLog = new TpServiceLog();
            tpServiceLog.ServiceRequest = Newtonsoft.Json.JsonConvert.SerializeObject(objProspect);
            TransactLog transactLog = new TransactLog();
            transactLog.SerivceTraceID = tpServiceLog.ServiceTraceID = objProspect.ServiceTraceID;
            transactLog.UserName = objProspect.UserName;
            string Message = ValidateUserAuth(transactLog);
            if (Message != "" && Message != null)
            {
                objProspect.Message = Message;
                return objProspect;
            }
            ProspectController Prospect = new ProspectController();
            if (objProspect.NotePad != null)
            {
                objProspect.NotePadByteArray = Convert.FromBase64String(objProspect.NotePad);

                string Proposerfilename = "NotePad.png";
                string ProposerdirectryPath = "";
                //ProposerdirectryPath = HttpContext.Current.Server.MapPath("ContactID_" + objProspect.ContactID);
                ProposerdirectryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Policy/ContactID_" + objProspect.ContactID);
                if (!Directory.Exists(ProposerdirectryPath))
                {
                    Directory.CreateDirectory(ProposerdirectryPath);
                }
                string Proposerfilepath = Path.Combine(ProposerdirectryPath, Proposerfilename);
                System.IO.File.WriteAllBytes(Proposerfilepath, objProspect.NotePadByteArray);
                objProspect.objNeedAnalysis.NotePadPath = Path.Combine("\\Policy", "ContactID_" + objProspect.ContactID, Proposerfilename);

            }
            Prospect.GetByteArray(objProspect);
            if (!string.IsNullOrEmpty(objProspect.objNeedAnalysis.ByteArraygraph))
            {
                objProspect.ByteArrayGraph = Convert.FromBase64String(objProspect.objNeedAnalysis.ByteArraygraph);
                objProspect.ByteArrayGraph = Prospect.Appgeneratepdf(objProspect.ByteArrayGraph, objProspect);

            }
            AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();
            objProspect = objProspectBusiness.SaveProspect(objProspect);
            if (objProspect.ProspectStage == 4)
            {
                AIA.Life.Business.Quote.QuoteBusiness QuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
                objProspect = QuoteBusiness.SendEmailAndSMSNotificationOnSAveProspect(objProspect);
            }

            tpServiceLog.ServiceResponse = Newtonsoft.Json.JsonConvert.SerializeObject(objProspect);
            commonBusiness.CreateServiceLog(tpServiceLog);
            return objProspect;
        }
        [HttpPost]
        public Login UserLogin(Login objLogin)
        {
            try
            {
                TpServiceLog tpServiceLog = new TpServiceLog();
                tpServiceLog.ServiceRequest = Newtonsoft.Json.JsonConvert.SerializeObject(objLogin);

                TransactLog transactLog = new TransactLog();
                transactLog.SerivceTraceID = tpServiceLog.ServiceTraceID = objLogin.ServiceTraceID;
                transactLog.UserName = objLogin.UserName;
                string Message = ValidateUserAuth(transactLog);
                if (Message != "" && Message != null)
                {
                    objLogin.Message = Message;
                    return objLogin;
                }

                UserManagementBusiness objUserManagementBusiness = new UserManagementBusiness();
                objLogin = objUserManagementBusiness.UserLogin(objLogin);

                tpServiceLog.ServiceResponse = Newtonsoft.Json.JsonConvert.SerializeObject(objLogin);
                commonBusiness.CreateServiceLog(tpServiceLog);
                return objLogin;
            }
            catch (Exception ex)
            {

                return objLogin;
            }
        }

        public ProposalStatus FetchProposalStatus(ProposalStatus proposalStatus)
        {
            TpServiceLog tpServiceLog = new TpServiceLog();
            tpServiceLog.ServiceRequest = Newtonsoft.Json.JsonConvert.SerializeObject(proposalStatus);
            TransactLog transactLog = new TransactLog();
            transactLog.SerivceTraceID = tpServiceLog.ServiceTraceID = proposalStatus.ServiceTraceID;
            transactLog.UserName = proposalStatus.UserName;
            string Message = ValidateUserAuth(transactLog);
            if (Message != "" && Message != null)
            {
                proposalStatus.Message = Message;
                return proposalStatus;
            }
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            
            proposalStatus = objPolicyBusiness.FetchProposalStatus(proposalStatus);

            tpServiceLog.ServiceResponse = Newtonsoft.Json.JsonConvert.SerializeObject(proposalStatus);
            commonBusiness.CreateServiceLog(tpServiceLog);
            return proposalStatus;
        }
        public string SaveCampaignLead(AIA.Life.Models.Opportunity.CampaignLeadType objCampaignLeadType)
        {
            if (!string.IsNullOrEmpty(objCampaignLeadType.Code) && !string.IsNullOrEmpty(objCampaignLeadType.Description))
            {
                AIA.Life.Business.Prospect.ProspectBusiness objProspectBusiness = new AIA.Life.Business.Prospect.ProspectBusiness();

                objCampaignLeadType = objProspectBusiness.SaveCampaignLead(objCampaignLeadType);
                
                objCampaignLeadType.Message = "Campaign LeadType Entered Successfully";
            }
            else
            {
                objCampaignLeadType.Message = "Fail";
            }
            return objCampaignLeadType.Message;

        }
        [HttpPost]
        public AIA.Life.Models.Common.AppVersion GetLatestVersion(AIA.Life.Models.Common.AppVersion objVersion)
        {
            //objVersion = new AppVersion();
            TpServiceLog tpServiceLog = new TpServiceLog();
            tpServiceLog.ServiceRequest = Newtonsoft.Json.JsonConvert.SerializeObject(objVersion);
            TransactLog transactLog = new TransactLog();
            transactLog.SerivceTraceID = tpServiceLog.ServiceTraceID = objVersion.ServiceTraceID;
            transactLog.UserName = objVersion.UserName;
            string Message = ValidateUserAuth(transactLog);
            if (Message != "" && Message != null)
            {
                objVersion.Message = Message;
                return objVersion;
            }
            
            objVersion = commonBusiness.GetLatestVersion(objVersion);

            tpServiceLog.ServiceResponse = Newtonsoft.Json.JsonConvert.SerializeObject(objVersion);
            commonBusiness.CreateServiceLog(tpServiceLog);
            return objVersion;
        }
        [HttpPost]
        public AIA.Life.Models.Common.AppVersion UpdateLatestVersion(AIA.Life.Models.Common.AppVersion objVersion)
        {
            TpServiceLog tpServiceLog = new TpServiceLog();
            tpServiceLog.ServiceRequest = Newtonsoft.Json.JsonConvert.SerializeObject(objVersion);
            TransactLog transactLog = new TransactLog();
            transactLog.SerivceTraceID = tpServiceLog.ServiceTraceID = objVersion.ServiceTraceID;
            transactLog.UserName = objVersion.UserName;
            string Message = ValidateUserAuth(transactLog);
            if (Message != "" && Message != null)
            {
                objVersion.Message = Message;
                return objVersion;
            }
            
            objVersion = commonBusiness.UpdateLatestVersion(objVersion);

            tpServiceLog.ServiceResponse = Newtonsoft.Json.JsonConvert.SerializeObject(objVersion);
            commonBusiness.CreateServiceLog(tpServiceLog);
            return objVersion;
        }

        [HttpPost]
        public UserToken GenerateTokenID(UserToken objLogin)
        {
            try
            {
                UserManagementBusiness objUserManagementBusiness = new UserManagementBusiness();
                objLogin = objUserManagementBusiness.GenerateTokenID(objLogin);
                return objLogin;
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                return objLogin;
            }
        }
        public string ValidateUserAuth(TransactLog transactLog)
        {
            string ErrorMessage = string.Empty;
            UserManagementBusiness objUserManagementBusiness = new UserManagementBusiness();
            var result = objUserManagementBusiness.ValidateUserAuth(transactLog);
            if (!string.IsNullOrEmpty(result.Message))
            {
                if (result.Message == "Session Time out.")
                {
                    log4net.GlobalContext.Properties["ErrorCode"] = "001XX76";
                }
                else if (result.Message == "Invalid Session.")
                {
                    log4net.GlobalContext.Properties["ErrorCode"] = "002XX77";
                }
                else
                {
                    log4net.GlobalContext.Properties["ErrorCode"] = "003XX78";
                }
                Logger.Error(result.Message);
                ErrorMessage = result.Message + " error code " + log4net.GlobalContext.Properties["ErrorCode"].ToString();

            }
            return ErrorMessage;
        }
        [HttpPost]
        public void InvokeReceiptCheck(PaymentModel objPaymentModel)
        {
            objPaymentModel = new PaymentModel();
            PaymentBusiness objPaymentBusiness = new PaymentBusiness();
            
            objPaymentModel = objPaymentBusiness.FetchPendingPayments(objPaymentModel); 
            if (objPaymentModel.lstPaymentItems.Count() > 0)
            {
                foreach (var item in objPaymentModel.lstPaymentItems)
                {
                    PaymentModel paymentModel = new PaymentModel();
                    paymentModel.ProposalNo = item.ProposalNo;
                    paymentModel.QuoteNo = item.QuoteNo;
                    paymentModel.PayableAmount =Convert.ToString(item.Premium);
                    paymentModel.UserName = item.UserName;
                    paymentModel.lstPaymentItems.Add(item);
                    paymentModel = objPaymentBusiness.CheckPaymentStatusUpdate(paymentModel);
                    if (!string.IsNullOrEmpty(paymentModel.Message) && paymentModel.Message == "Success" && string.IsNullOrEmpty(paymentModel.UWMessage))
                        paymentModel = PostPolicyIssuanceTriggers(paymentModel);
                }
            }

        }
        [HttpPost]
        public void CallBizDate(Policy policy)
        {
            policy = new Policy();
            PolicyBusiness policyBusiness = new PolicyBusiness();
            policyBusiness.CallBizDate(policy);
        }
        [HttpPost]
        public PaymentModel PostPolicyIssuanceTriggers(PaymentModel objPaymentModel)
        {
            #region policy issuance triggers
            ReportsController objReportController = new ReportsController();
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            List<byte[]> lstBytes = new List<byte[]>();
            try
            {
                if (objPaymentModel.lstPaymentItems.Count > 0)
                {
                    byte[] quoteBytes = objReportController.ReportQuotation(objPaymentModel.QuoteNo, objPaymentModel.lstPaymentItems[0].PlanCode, objPaymentModel.lstPaymentItems[0].PreferredLanguage);
                    if (quoteBytes != null)
                        lstBytes.Add(quoteBytes);
                    byte[] a = objReportController.ReportForIllustrationPDF(objPaymentModel.QuoteNo, objPaymentModel.lstPaymentItems[0].PlanCode, objPaymentModel.lstPaymentItems[0].PreferredLanguage, objPaymentModel.ProposalNo);
                    byte[] b = objReportController.ReportForPolicySchedule(objPaymentModel.ProposalNo, objPaymentModel.lstPaymentItems[0].ProductCode, objPaymentModel.lstPaymentItems[0].PreferredLanguage);
                    byte[] c = objReportController.ReportForCoveringLetter(objPaymentModel.ProposalNo, objPaymentModel.lstPaymentItems[0].PlanCode, objPaymentModel.lstPaymentItems[0].PreferredLanguage);
                    List<byte[]> vs = new List<byte[]>();
                    if (c != null)
                        vs.Add(c);
                    if (b != null)
                        vs.Add(b);
                    if (a != null)
                        vs.Add(a);
                    objPaymentModel.ByteArray2 = CrossCutting.DataTypeConvExtention.ConcatPdf(vs);
                    objPaymentModel.ByteArray3 = objReportController.ProposalReport(objPaymentModel.QuoteNo, objPaymentModel.lstPaymentItems[0].ProductCode, objPaymentModel.lstPaymentItems[0].PreferredLanguage);
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
                    documents.AgentCode = objPaymentModel.UserName;
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
            return objPaymentModel;
        }

        [HttpPost]
        public OCRResponse gooleVisionTextDecoderApi(OCRResponse objResp)
        {
          
            TpServiceLog tpServiceLog = new TpServiceLog();
            //objResp = new OCRResponse();
            tpServiceLog.ServiceRequest = Newtonsoft.Json.JsonConvert.SerializeObject(objResp);
            TransactLog transactLog = new TransactLog();
            transactLog.SerivceTraceID = tpServiceLog.ServiceTraceID = objResp.ServiceTraceID;
            transactLog.UserName = objResp.UserName;
            string Message = ValidateUserAuth(transactLog);
            if (Message != "" && Message != null)
            {
                objResp.ErrorMessage = Message;
                return objResp;
            }
            if (!string.IsNullOrEmpty(objResp.Filedata))
            {
                objResp = commonBusiness.gooleVisionTextDecoderApi(objResp);
            }
            else
            {
                objResp = new OCRResponse();
                objResp.ErrorMessage = "Image not found";
                objResp.ErrorCode = 1;
            }
                

            tpServiceLog.ServiceResponse = Newtonsoft.Json.JsonConvert.SerializeObject(objResp);
            commonBusiness.CreateServiceLog(tpServiceLog);
            return objResp;
        }

        

        [HttpPost]
        public string GenerateProposalPayment(string QuoteNo)
        {
            QuoteNo = CrossCutting_EncryptDecrypt.Encrypt(QuoteNo);
            return QuoteNo;
        }
        public void SMSReminder()
        {
            AIA.Life.Models.Opportunity.SMSReminder objSMSReminder = new Life.Models.Opportunity.SMSReminder();
            AIA.Life.Business.Quote.QuoteBusiness objQuoteBusiness = new AIA.Life.Business.Quote.QuoteBusiness();
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            var objLst = Context.Usp_GetOutStandingDocList().ToList();
            var NoOfDays = 0;
            var PolicyID = "";
            foreach (var item in objLst)
            {
                objSMSReminder.NoOfDays = Convert.ToInt32(item.NoOfDays);
                objSMSReminder.PolicyID = Convert.ToString(item.PolicyId);
            //}
            //if (objLst.Count>0)
            //{
                objQuoteBusiness.SMSReminder(objSMSReminder);
            }
        }
        
        public class DomFile
        {
            public string Filedata { get; set; }
            public string FileName { get; set; }
            public string Department_Code { get; set; }
            public string Agent_Code { get; set; }
            public string Policy_No { get; set; }
            //public string Document_Code { get; set; }
        }
        public class DowFile
        {
            public string FileFormat { get; set; }
            public string FileName { get; set; }
            public string FIlePath { get; set; }
        }
        [HttpGet]
        public AIA.Life.Models.UserManagement.ResouceManagent ResourceList(string UserName,string ServiceTraceID)
        {
            AIA.Life.Models.UserManagement.ResouceManagent objResouceManagent = new ResouceManagent();
            objResouceManagent.UserName = UserName;
            objResouceManagent.ServiceTraceID = ServiceTraceID;
            TpServiceLog tpServiceLog = new TpServiceLog();
            tpServiceLog.ServiceRequest = Newtonsoft.Json.JsonConvert.SerializeObject(objResouceManagent);
            TransactLog transactLog = new TransactLog();
            transactLog.SerivceTraceID = tpServiceLog.ServiceTraceID = objResouceManagent.ServiceTraceID;
            transactLog.UserName = objResouceManagent.UserName;
            string Message = ValidateUserAuth(transactLog);
            if (Message != "" && Message != null)
            {
                objResouceManagent.Message = Message;
                return objResouceManagent;
            }

            objResouceManagent = commonBusiness.ContentListDetails(objResouceManagent);

            tpServiceLog.ServiceResponse = Newtonsoft.Json.JsonConvert.SerializeObject(objResouceManagent);
            commonBusiness.CreateServiceLog(tpServiceLog);
            return objResouceManagent;
        }
    }
}

