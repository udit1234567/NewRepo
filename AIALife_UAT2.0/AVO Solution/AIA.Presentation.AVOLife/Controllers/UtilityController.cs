using AIA.Life.Repository.AIAEntity;
using AIA.Life.Models.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;

namespace AIA.Presentation.AVOLife.Controllers
{
    [Authorize]
    [ErrorLogging]
    public class UtilityController : Controller
    {
        // GET: Utility
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CalculatorHome()
        {
            try
            {
                return View("CalculatorHome");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region Calculators
        public ActionResult RetirementCalculator()
        {
            try
            {

                ViewData["monthlyExpense"] = Request["MonthlyExpense"];
                ViewData["netWorth"] = Request["NetWorth"];
                return View("_PartialRetirementCalculator");
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult HumanLifeValueCalculator()
        {
            try
            {
                return View("_PartialHumanLifeValueCalculator");
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult ChildEducationCalculator()
        {
            try
            {
                return View("_PartialChildEducationCalculator");
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult ChildMarriageCalculator()
        {
            try
            {
                return View("_PartialChildMarriageCalculator");

            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public ActionResult InvestmentsCalculator()
        {
            try
            {
                return View("_PartialInvestmentCalculator");

            }
            catch (Exception ex)
            {

                return null;
            }
        }
        #endregion

        #region File Upload
        //public ActionResult SavePolicyDocuments(int ContactID, string ProposalNo , string objLstDoc, int Req = 0)
        //{
        //    try
        //    {
                

        //        AVOAIALifeEntities Context = new AVOAIALifeEntities();
        //        //string objLstDoc = "";
        //        string DepartmentCode = "", AgentCode = "", DocumentCode = "";
        //        if (ProposalNo == "")
        //        {
        //            ProposalNo = "PBBBQ123456";
        //        }

        //        string UserName = User.Identity.Name;
        //        AgentCode = Context.tblMasIMOUsers.Where(a => a.UserID == UserName).Select(a => a.AgentCode).FirstOrDefault();
               
        //        //DepartmentCode = GetMaritialStatus();
        //        List<DocumentUploadFile> LstDocumentUpload = new List<DocumentUploadFile>();
        //        List<DocumentUploadFile> RequestDoc = new List<DocumentUploadFile>();
        //        string FileName = "";
        //        Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
        //        RequestDoc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DocumentUploadFile>>(objLstDoc, settings);

        //        string StrWriteException = string.Empty;
        //        for (var i = 0; i < RequestDoc.Count(); i++)
        //        {                                           
                    
        //            if (string.IsNullOrEmpty(RequestDoc[i].FilePath))
        //            {
        //                if (Request.Files[RequestDoc[i].Key] != null)
        //                {
        //                    string Key = RequestDoc[i].Key;
        //                    DocumentUploadFile objUpload = new DocumentUploadFile();
        //                    Random rnd = new Random();
        //                    int num = rnd.Next(1, 9999);
        //                    FileName = Request.Files[Key].FileName;
        //                    objUpload.FileName = RequestDoc[i].DocName;
        //                    //objUpload.DOCID = RequestDoc[i].DOCID;



        //                    FileName = FileName.Replace(".", "_" + num.ToString() + ".");
        //                    var FileStreamBytes = Request.Files[Key].InputStream;
        //                    HttpPostedFileBase file = Request.Files[Key];
        //                    var DocumentNames = RequestDoc[i].DocName;
        //                    DocumentCode = Context.tblMasDocuments.Where(a=>a.DocumentName == DocumentNames).Select(a=>a.DocumentCode).FirstOrDefault();
        //                    DepartmentCode = Context.tblMasDocuments.Where(a => a.DocumentName == DocumentNames).Select(a => a.DocumentType).FirstOrDefault();
        //                   // DepartmentCode = "UW";
        //                    if (RequestDoc[i].DocName=="SpouseImage")
        //                    {
        //                        DocumentCode = "PRD012";
        //                    }
        //                    if(DocumentCode==null)
        //                    {
        //                        DocumentCode = "PRD011";
        //                    }
                            
        //                    if (file.ContentLength <= 4000000)
        //                    {
        //                        var ext = Path.GetExtension(FileName);
        //                        string _Contact = Convert.ToString(ContactID);
        //                        string _DepartmentCode = Convert.ToString(DepartmentCode);
        //                        string _AgentCode = Convert.ToString(AgentCode);
        //                        string _ProposalNo = Convert.ToString(ProposalNo);
        //                        //string _DepartmentCode = DepartmentCode;
        //                        string _DocumentCode = DocumentCode;

        //                        DocumentUpload(file, FileName, _AgentCode, _ProposalNo, _DocumentCode);
        //                    }
        //                    else
        //                    {
        //                        StrWriteException = "File Size Exceeded";
        //                        var objResult = new { StrWriteException = StrWriteException };
        //                        return Json(objResult, JsonRequestBehavior.AllowGet);
        //                    }

                            
        //                    string directryPath = ConfigurationManager.AppSettings["FileUpload"] + "\\" + AgentCode + "\\" + ProposalNo;
        //                    var exts =Path.GetExtension(FileName);
        //                    exts = exts.ToLower();
        //                    if (exts == ".png" || exts == ".jpeg" || exts == ".jpg")
        //                    {
        //                        exts = ".pdf";
        //                    }
        //                    FileName = Path.Combine(directryPath, DocumentCode+""+exts);
        //                    objUpload.FilePath = FileName;
        //                    objUpload.ItemType = RequestDoc[i].ItemType;
        //                    objUpload.MemberType = RequestDoc[i].MemberType;
        //                    objUpload.Index = i;
        //                    LstDocumentUpload.Add(objUpload);
        //                }
        //            }
        //            else
        //            {
        //                DocumentUploadFile objUpload = new DocumentUploadFile();
        //                objUpload.FilePath = RequestDoc[i].FilePath;
        //                objUpload.FileName = RequestDoc[i].DocName;
        //                objUpload.ItemType = RequestDoc[i].ItemType;
        //                objUpload.MemberType = RequestDoc[i].MemberType;
                        
        //                objUpload.Index = i;
        //                LstDocumentUpload.Add(objUpload);
        //            }

        //        }
        //        string JsonData = JsonConvert.SerializeObject(LstDocumentUpload);
        //        var objRes = new { DocData = JsonData, StrWriteException = StrWriteException };
        //        return Json(objRes, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        var objRes = new { StrWriteException = "Some error occured" };
        //        return Json(objRes, JsonRequestBehavior.AllowGet);
        //    }

        //}
        public ActionResult SavePolicyDocuments(int ContactID, string ProposalNo, string objLstDoc, HttpPostedFileBase UploadFiles, int Req = 0)
        {
            try
            {
                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                //string objLstDoc = "";
                string DepartmentCode = "", AgentCode = "", DocumentCode = "";
                if (ProposalNo == "")
                {
                    ProposalNo = "PBBBQ123456";
                }

                string UserName = User.Identity.Name;
                AgentCode = Context.tblMasIMOUsers.Where(a => a.UserID == UserName).Select(a => a.AgentCode).FirstOrDefault();

                //DepartmentCode = GetMaritialStatus();
                List<DocumentUploadFile> LstDocumentUpload = new List<DocumentUploadFile>();
                List<DocumentUploadFile> RequestDoc = new List<DocumentUploadFile>();
                string FileName = "";
                Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
                RequestDoc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DocumentUploadFile>>(objLstDoc, settings);
                byte[] fileData = null;
                string StrWriteException = string.Empty;
                string directryPath = ConfigurationManager.AppSettings["FileUpload"] + "\\" + AgentCode + "\\" + ProposalNo;
                string _DepartmentCode = Convert.ToString(DepartmentCode);
                string _AgentCode = Convert.ToString(AgentCode);
                string _ProposalNo = Convert.ToString(ProposalNo);
                int count = 0;
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    var ReqDoc = RequestDoc.Where(a => a.Key == Request.Files.GetKey(i)).FirstOrDefault();
                    fileData = null;
                    count = i;
                    if (string.IsNullOrEmpty(ReqDoc.FilePath))
                    {
                        if (Request.Files[i] != null)
                        {
                            string Key = RequestDoc[i].Key;
                            DocumentUploadFile objUpload = new DocumentUploadFile();
                            Random rnd = new Random();
                            int num = rnd.Next(1, 99999);
                            FileName = Request.Files[i].FileName;
                            objUpload.FileName = ReqDoc.DocName;
                            //objUpload.DOCID = RequestDoc[i].DOCID;
                            FileName = FileName.Replace(".", "_" + num.ToString() + ".");
                            var FileStreamBytes = Request.Files[i].InputStream;
                            HttpPostedFileBase file = Request.Files[i];
                            var DocumentNames = ReqDoc.DocName;
                            DocumentCode = Context.tblMasDocuments.Where(a => a.DocumentName == DocumentNames).Select(a => a.DocumentCode).FirstOrDefault();
                            DepartmentCode = Context.tblMasDocuments.Where(a => a.DocumentName == DocumentNames).Select(a => a.DocumentType).FirstOrDefault();
                            // DepartmentCode = "UW";
                            if (ReqDoc.DocName == "SpouseImage")
                            {
                                DocumentCode = "PRD012";
                            }
                            if (DocumentCode == null)
                            {
                                DocumentCode = "PRD011";
                            }

                            if (file.ContentLength <= 4000000)
                            {
                                var ext = Path.GetExtension(FileName);
                                string _Contact = Convert.ToString(ContactID);
                                //string _DepartmentCode = DepartmentCode;
                                string _DocumentCode = DocumentCode;

                                fileData = DocumentUpload(file, FileName, _AgentCode, _ProposalNo, _DocumentCode,null,"Proposal");
                            }
                            else
                            {
                                StrWriteException = "File Size Exceeded";
                                var objResult = new { StrWriteException = StrWriteException };
                                return Json(objResult, JsonRequestBehavior.AllowGet);
                            }
                            var exts = Path.GetExtension(FileName);
                            exts = exts.ToLower();
                            if (exts == ".png" || exts == ".jpeg" || exts == ".jpg")
                            {
                                exts = ".pdf";
                            }
                            var names = FileName.Split('.');
                            var number = names[0].Split('_');
                            var nums = number[1];
                            FileName = Path.Combine(directryPath, DocumentCode +"_"+ nums + "" + exts);

                            objUpload.FilePath = FileName;
                            objUpload.ItemType = ReqDoc.ItemType;
                            objUpload.MemberType = ReqDoc.MemberType;
                            objUpload.FileData = fileData;
                            objUpload.Index = i;
                            objUpload.DOCID = ReqDoc.DOCID;
                            LstDocumentUpload.Add(objUpload);
                        }
                    }
                    else
                    {
                        DocumentUploadFile objUpload = new DocumentUploadFile();
                        objUpload.FilePath = ReqDoc.FilePath;
                        objUpload.FileName = ReqDoc.DocName;
                        objUpload.ItemType = ReqDoc.ItemType;
                        objUpload.MemberType = ReqDoc.MemberType;
                        objUpload.FileData = fileData;
                        objUpload.Index = i;
                        objUpload.DOCID = ReqDoc.DOCID;
                        LstDocumentUpload.Add(objUpload);
                    }

                }
              foreach(var obj in RequestDoc.Where(a=>a.FileName==""))
                {
                    DocumentUploadFile objUpload = new DocumentUploadFile();
                    objUpload.FilePath = null;
                    objUpload.FileName = obj.DocName;
                    objUpload.ItemType = obj.ItemType;
                    objUpload.MemberType = obj.MemberType;
                    objUpload.FileData = null;
                    objUpload.Index = count;
                    objUpload.DOCID = obj.DOCID;
                    LstDocumentUpload.Add(objUpload);
                    count++;

                }



                byte[] ConcatFiles;
                directryPath = ConfigurationManager.AppSettings["FileUpload"] + "\\" + AgentCode + "\\" + ProposalNo + "\\LDMS";
                if (!Directory.Exists(directryPath))
                {
                    Directory.CreateDirectory(directryPath);
                }
                var PolicyID = Context.tblPolicies.Where(a => a.ProposalNo == ProposalNo).Select(a => a.PolicyID).FirstOrDefault();
                var RecievedFiles = Context.tblPolicyDocuments.Where(a => a.PolicyID == PolicyID && a.Decision == "2370").ToList();
                List<DocumentUploadFile> LstTotalFiles = new List<DocumentUploadFile>();
                foreach (var item in RecievedFiles)
                {
                    DocumentUploadFile objDoc = new DocumentUploadFile();
                    objDoc.FileName = item.FileName;
                    objDoc.FilePath = item.FilePath;
                    objDoc.FileData = GetFile(item.FilePath);
                    if (objDoc.FileData != null)
                    {
                        LstTotalFiles.Add(objDoc);
                    }

                }
                foreach (var fName in LstDocumentUpload.Where(a=>a.FilePath!=null).Concat(LstTotalFiles).Select(a => a.FileName).Distinct().ToList())
                {
                    ConcatFiles = CrossCutting.DataTypeConvExtention.ConcatPdf(LstDocumentUpload.Where(a => a.FileName == fName && a.FilePath !=null).Concat(LstTotalFiles.Where(a => a.FileName == fName)).Select(a => a.FileData).ToList());
                    DocumentCode = Context.tblMasDocuments.Where(a => a.DocumentName == fName).Select(a => a.DocumentCode).FirstOrDefault();
                    System.IO.File.WriteAllBytes(directryPath + "\\" + DocumentCode + ".pdf", ConcatFiles);
                }
                foreach (var objUpload in LstDocumentUpload)
                {
                    objUpload.FileData = null;
                }

                string JsonData = JsonConvert.SerializeObject(LstDocumentUpload);
                var objRes = new { DocData = JsonData, StrWriteException = StrWriteException };
                return Json(objRes, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var objRes = new { StrWriteException = "Some error occured" };
                return Json(objRes, JsonRequestBehavior.AllowGet);
            }

        }
        public Byte[] GetFile(string FilePath)
        {
            try
            {
                if (!System.IO.File.Exists(FilePath))
                {
                    return null;
                }
                else
                {
                    return System.IO.File.ReadAllBytes(FilePath);
                }
            }
            catch (Exception)
            {

                return null;
            }
           
        }
        public AIA.Life.Models.Policy.Policy SavePolicyUploadDocuments(AIA.Life.Models.Policy.Policy ObjPolicy)
        {
            try
            {


                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                //string objLstDoc = "";
                string DepartmentCode = "", AgentCode = "", DocumentCode = "";
                if (ObjPolicy.ProposalNo == "")
                {
                    ObjPolicy.ProposalNo = "PBBBQ123456";
                }

                string UserName = ObjPolicy.UserName;
                AgentCode = Context.tblMasIMOUsers.Where(a => a.UserID == UserName).Select(a => a.AgentCode).FirstOrDefault();

                //DepartmentCode = GetMaritialStatus();
                List<DocumentUploadFile> LstDocumentUpload = new List<DocumentUploadFile>();
                List<DocumentUploadFile> RequestDoc = new List<DocumentUploadFile>();
                string FileName = "";
                Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
                RequestDoc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DocumentUploadFile>>(ObjPolicy.HdnDocumentDetails, settings);

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
                            //objUpload.DOCID = RequestDoc[i].DOCID;



                            FileName = FileName.Replace(".", "_" + num.ToString() + ".");
                            var FileStreamBytes = Request.Files[Key].InputStream;
                            HttpPostedFileBase file = Request.Files[Key];
                            var DocumentNames = RequestDoc[i].DocName;
                            DocumentCode = Context.tblMasDocuments.Where(a => a.DocumentName == DocumentNames).Select(a => a.DocumentCode).FirstOrDefault();
                            DepartmentCode = Context.tblMasDocuments.Where(a => a.DocumentName == DocumentNames).Select(a => a.DocumentType).FirstOrDefault();
                            // DepartmentCode = "UW";
                            if (RequestDoc[i].DocName == "SpouseImage")
                            {
                                DocumentCode = "PRD012";
                            }
                            if (DocumentCode == null)
                            {
                                DocumentCode = "PRD011";
                            }

                            if (file.ContentLength <= 4000000)
                            {
                                var ext = Path.GetExtension(FileName);
                                string _Contact = Convert.ToString(ObjPolicy.ContactID);
                                string _DepartmentCode = Convert.ToString(DepartmentCode);
                                string _AgentCode = Convert.ToString(AgentCode);
                                string _ProposalNo = Convert.ToString(ObjPolicy.ProposalNo);
                                //string _DepartmentCode = DepartmentCode;
                                string _DocumentCode = DocumentCode;

                                DocumentUpload(file, FileName, _AgentCode, _ProposalNo, _DocumentCode);
                            }
                            else
                            {
                                ObjPolicy.Error.ErrorMessage = "File Size Exceeded";
                                return ObjPolicy;
                            }


                            string directryPath = ConfigurationManager.AppSettings["FileUpload"] + "\\" + AgentCode + "\\" + ObjPolicy.ProposalNo;
                            var exts = Path.GetExtension(FileName);
                            exts = exts.ToLower();
                            if (exts == ".png" || exts == ".jpeg" || exts == ".jpg")
                            {
                                exts = ".pdf";
                            }
                            FileName = Path.Combine(directryPath, DocumentCode + "" + exts);
                            objUpload.FilePath = FileName;
                            objUpload.ItemType = RequestDoc[i].ItemType;
                            objUpload.MemberType = RequestDoc[i].MemberType;
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
                        objUpload.MemberType = RequestDoc[i].MemberType;

                        objUpload.Index = i;
                        LstDocumentUpload.Add(objUpload);
                    }

                }
                ObjPolicy.HdnDocumentDetails = JsonConvert.SerializeObject(LstDocumentUpload);
                ObjPolicy.Error.ErrorMessage = "Success";
                return ObjPolicy;
            }
            catch (Exception ex)
            {
                ObjPolicy.Error.ErrorMessage = ex.Message;
                return ObjPolicy;
            }

        }
        public ActionResult SaveLeadDocuments(int ContactID, string objLstDoc, int Req = 0)
        {
            try
            {


                AVOAIALifeEntities Context = new AVOAIALifeEntities();
                //string objLstDoc = "";
                string DepartmentCode = "", AgentCode = "", DocumentCode = "";
                

                string UserName = User.Identity.Name;
                AgentCode = Context.tblMasIMOUsers.Where(a => a.UserID == UserName).Select(a => a.AgentCode).FirstOrDefault();

                //DepartmentCode = GetMaritialStatus();
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
                            var DocumentNames = RequestDoc[i].DocName;
                            //DocumentCode = Context.tblMasDocuments.Where(a => a.DocumentName == DocumentNames).Select(a => a.DocumentCode).FirstOrDefault();
                            //if (RequestDoc[i].DocName == "SpouseImage")
                            //{
                            //    DocumentCode = "PRD012";
                            //}
                            //if (DocumentCode == null)
                            //{
                            //    DocumentCode = "PRD011";
                            //}
                            DepartmentCode = "Document";
                            if (file.ContentLength <= 1000000)
                            {
                                var ext = Path.GetExtension(FileName);
                                string _Contact = Convert.ToString(ContactID);
                                string _DepartmentCode = Convert.ToString(DepartmentCode);
                                string _AgentCode = Convert.ToString(AgentCode);
                                //string _DepartmentCode = DepartmentCode;
                                string _DocumentCode = DocumentCode;

                                LeadDocumentUpload(file, FileName, _AgentCode, _Contact, _DocumentCode);
                            }
                            else
                            {
                                StrWriteException = "File Size Exceeded";
                                var objResult = new { StrWriteException = StrWriteException };
                                return Json(objResult, JsonRequestBehavior.AllowGet);
                            }


                            string directryPath = ConfigurationManager.AppSettings["FileUpload"]  + "\\" + AgentCode + "\\" + ContactID;
                            var exts = Path.GetExtension(FileName);
                            FileName = Path.Combine(directryPath, DocumentCode + "" + exts);
                            objUpload.FilePath = FileName;
                            objUpload.ItemType = RequestDoc[i].ItemType;
                            objUpload.MemberType = RequestDoc[i].MemberType;
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
                        objUpload.MemberType = RequestDoc[i].MemberType;

                        objUpload.Index = i;
                        LstDocumentUpload.Add(objUpload);
                    }

                }
                string JsonData = JsonConvert.SerializeObject(LstDocumentUpload);
                var objRes = new { DocData = JsonData, StrWriteException = StrWriteException };
                return Json(objRes, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var objRes = new { StrWriteException = "Some error occured" };
                return Json(objRes, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult SaveDocuments(string strUser = null, int Req = 0)
        {
            try
            {


                List<DocumentUploadFile> LstDocumentUpload = new List<DocumentUploadFile>();
                string FileName = "";


                string StrWriteException = string.Empty;
                for (var i = 0; i <= Req; i++)
                {
                    if (Request.Files[i] != null)
                    {
                        DocumentUploadFile objUpload = new DocumentUploadFile();
                        Random rnd = new Random();
                        int num = rnd.Next(1, 9999);

                        FileName = Request.Files[i].FileName;
                        FileName = FileName.Replace(".", "_" + num.ToString() + ".");
                        var FileStreamBytes = Request.Files[i].InputStream;

                        HttpPostedFileBase file = Request.Files[i];
                        if (file.ContentLength <= 1000000)
                        {
                            var ext = Path.GetExtension(FileName);
                            DocumentUpload(file, FileName, strUser);
                        }
                        else
                        {
                            StrWriteException = "File Size Exceeded";
                            var objResult = new { StrWriteException = StrWriteException };
                            return Json(objResult, JsonRequestBehavior.AllowGet);
                        }
                        string directryPath = Server.MapPath("ContactID_" + strUser);
                        FileName = strUser + "_" + FileName;
                        FileName = Path.Combine(directryPath, FileName);
                        objUpload.FileName = FileName;
                        objUpload.Index = i;
                        LstDocumentUpload.Add(objUpload);

                    }

                }

                var objRes = new { ObjDocuments = LstDocumentUpload, StrWriteException = StrWriteException };
                return Json(objRes, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var objRes = new { StrWriteException = "Some error occured" };
                return Json(objRes, JsonRequestBehavior.AllowGet);
            }

        }
        public byte[] DocumentUpload(HttpPostedFileBase file, string FileName = null, string AgentCode = null,string ProposalNo = null, string DocumentCode = null, string strUser = null,string source = null)
        {
            byte[] data = null;

            try
            {
                if (file != null)
                {
                    //BinaryReader reader = new BinaryReader(new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));

                    AVOAIALifeEntities Context = new AVOAIALifeEntities();
                    string UserName = User.Identity.Name;
                    AgentCode = Context.tblMasIMOUsers.Where(a => a.UserID == UserName).Select(a => a.AgentCode).FirstOrDefault();
                    if (ProposalNo == "")
                    {
                        ProposalNo = "PBBBQ100023";
                    }
                    Stream s;
                    s = file.InputStream;
                    var extension = Path.GetExtension(FileName);
                    //DepartmentCode = "UW";
                    extension = extension.ToLower();
                    BinaryReader reader = new BinaryReader(s);
                    data = reader.ReadBytes(file.ContentLength);
                    if (extension == ".png" || extension == ".jpeg" || extension == ".jpg")
                    {
                        data = PdfGeneration(data);
                        file = (HttpPostedFileBase)new MemoryPostedFile(data);
                        var Name = FileName.Split('.');
                        FileName = Name[0]+".pdf";
                    }
                    string directryPath = ConfigurationManager.AppSettings["FileUpload"]  + "\\" + AgentCode + "\\" + ProposalNo;
                    if (!Directory.Exists(directryPath))
                    {
                        Directory.CreateDirectory(directryPath);
                    }
                    
                    var fileName = Path.GetFileName(FileName);
                    var ext = Path.GetExtension(FileName);
                   // Random rnd = new Random();
                    //int num = rnd.Next(1, 9999);
                    var names = fileName.Split('.');
                    var number = names[0].Split('_');
                    var num = number[1];
                    if (FileName != null)
                    {
                        fileName = DocumentCode + "_" + num + "" + ext;
                        var filename = Path.Combine(directryPath, fileName);

                        //file.SaveAs(filename);
                        System.IO.File.WriteAllBytes(filename, data);
                        if (source != "Proposal")
                        {
                            System.IO.File.Copy(filename, ConfigurationManager.AppSettings["FileUpload"] + "\\" + fileName);

                        }
                    }
                    else
                    {
                        fileName = Path.GetFileName(file.FileName);
                        var name = fileName.Split('.');
                        name[0] = name[0] + "_" + num;
                        fileName = name[0] +"."+name[1];
                        var filename = Path.Combine(directryPath, fileName);
                        //file.SaveAs(filename);
                        System.IO.File.WriteAllBytes(filename, data);

                    }


                }
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator.";
            }
            return data;

        }

        public void LeadDocumentUpload(HttpPostedFileBase file, string FileName = null, string DepartmentCode = null, string AgentCode = null, string ContactID = null, string DocumentCode = null, string strUser = null)
        {
            try
            {
                if (file != null)
                {
                    //BinaryReader reader = new BinaryReader(new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));

                    AVOAIALifeEntities Context = new AVOAIALifeEntities();
                    string UserName = User.Identity.Name;
                    AgentCode = Context.tblMasIMOUsers.Where(a => a.UserID == UserName).Select(a => a.AgentCode).FirstOrDefault();
                    //if (ProposalNo == "")
                    //{
                    //    ProposalNo = "PBBBQ100023";
                    //}

                    DepartmentCode = "Document";
                    string directryPath = ConfigurationManager.AppSettings["FileUpload"] + "\\" + DepartmentCode + "\\" + AgentCode + "\\" + ContactID;
                    if (!Directory.Exists(directryPath))
                    {
                        Directory.CreateDirectory(directryPath);
                    }

                    var fileName = Path.GetFileName(file.FileName);
                    var ext = Path.GetExtension(file.FileName);

                    if (FileName != null)
                    {
                        fileName = DocumentCode + "" + ext;
                        var filename = Path.Combine(directryPath, fileName);

                        file.SaveAs(filename);
                        System.IO.File.Copy(filename, ConfigurationManager.AppSettings["FileUpload"] + "\\" + fileName);
                    }
                    else
                    {
                        fileName = Path.GetFileName(file.FileName);
                        var filename = Path.Combine(directryPath, fileName);
                        file.SaveAs(filename);
                    }


                }
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator.";
            }
        }
        #endregion

        #region Download Documents
        public void DownloadUploadedfile(string FileName)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            DocumentDetails objDocumentDetails = new DocumentDetails();
            tblProspect objtblProspect = null;

            var directorypath = System.Web.HttpContext.Current.Server.MapPath("Upload");
            var PolicyFileName = Path.Combine(directorypath, Path.GetFileName(FileName));
            string result = PolicyFileName;
            string Filecontenttype = Context.tblProspectDocuments.Where(a => a.ProspectID == objtblProspect.ProspectID).FirstOrDefault().DocType;
            objDocumentDetails.FileContent = Encoding.ASCII.GetBytes(Context.tblProspectDocuments.Where(a => a.ProspectID == objtblProspect.ProspectID).FirstOrDefault().DocPath);
            System.Web.HttpContext.Current.Response.ContentType = Filecontenttype;
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(PolicyFileName));
            System.Web.HttpContext.Current.Response.Charset = "";
            System.Web.HttpContext.Current.Response.OutputStream.Write(objDocumentDetails.FileContent, 0, objDocumentDetails.FileContent.Length);
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.Close();
        }
        #endregion

        public void DownloadPolicyDocument(string FilePath)
        {
            try
            {
                Uri uri = new Uri(FilePath);
                if (uri.IsFile)
                {
                    string _fileName = System.IO.Path.GetFileName(uri.LocalPath);
                    Response.Clear();
                    Response.ContentType = System.IO.Path.GetExtension(FilePath);
                    Response.ContentType = "application/octet-stream";
                    Response.AppendHeader("Content-Disposition", "attachment;filename=" + _fileName);
                    Response.TransmitFile(FilePath);
                    Response.End();
                }
            }
            catch (Exception ex)
           {

            }
        }
      
        public JsonResult UploadFile()
        {
            try
            {
                HttpPostedFileBase file = Request.Files[0];
                string fileName = string.Empty;
                string path = string.Empty;
                byte[] data = null;
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
        public byte[] PdfGeneration(byte[] byteImage)
        {
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(byteImage);
           
            using (MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                Document document = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                //string path2 = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "signatureimage";
                //string filename2 = "test.pdf";
                //string filepath2 = System.IO.Path.Combine(path2, filename2);
                // PdfWriter.GetInstance(document, new FileStream(filepath2, FileMode.Create));
                image.SetAbsolutePosition(0, 0);
                image.ScaleAbsoluteHeight(document.PageSize.Height);
                image.ScaleAbsoluteWidth(document.PageSize.Width);
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, memoryStream);
                document.Open();
                document.Add(image);
                document.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                return bytes;
            }
        }
        public class DocumentDetails
        {
            public int DocumentID { get; set; }
            public string DocumentName { get; set; }
            public string FileName { get; set; }
            public int Index { get; set; }
            public string DocumentAprroval { get; set; }
            public string Remarks { get; set; }
            public string ExistingFileName { get; set; }
            public byte[] FileContent { get; set; }
        }
        public class MemoryPostedFile : HttpPostedFileBase
        {
            private readonly byte[] fileBytes;

            public MemoryPostedFile(byte[] fileBytes, string fileName = null)
            {
                this.fileBytes = fileBytes;
                this.FileName = fileName;
                this.InputStream = new MemoryStream(fileBytes);
            }

            public override int ContentLength => fileBytes.Length;

            public override string FileName { get; }

            public override Stream InputStream { get; }
        }
    }
}