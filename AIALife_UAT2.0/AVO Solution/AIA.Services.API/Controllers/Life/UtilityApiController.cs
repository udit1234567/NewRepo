using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;
using AIA.CrossCutting;

namespace AIA.Services.API.Controllers.Life
{
    public class UtilityApiController : ApiController
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        [HttpPost]
        public string FileUpload(DomFile obj)
        {
            string Result = "";
            try
            {
                if (!string.IsNullOrEmpty(obj.Department_Code) && !string.IsNullOrEmpty(obj.Agent_Code) && !string.IsNullOrEmpty(obj.Policy_No) && !string.IsNullOrEmpty(obj.FileName))
                {
                    //string zipPath = @"E:\AIA\test1.zip";
                    byte[] FileBytes = Convert.FromBase64String(obj.Filedata); //System.IO.File.ReadAllBytes(zipPath); //

                    string Path = ConfigurationManager.AppSettings["DocumentUploadPath"]; //@"E:\AIA\uploaddocsmobility";
                    bool IsfolderExist = CheckImageExist(obj.Department_Code);

                    if (IsfolderExist == false)
                    {
                        var directoryInfo1 = new DirectoryInfo(Path);
                        directoryInfo1.CreateSubdirectory(obj.Department_Code);
                    }
                    Path = Path + "//" + obj.Department_Code;
                    IsfolderExist = CheckImageExist(obj.Department_Code + "//" + obj.Agent_Code);
                    if (IsfolderExist == false)
                    {
                        var directoryInfo1 = new DirectoryInfo(Path);
                        directoryInfo1.CreateSubdirectory(obj.Agent_Code);
                    }
                    Path = Path + "//" + obj.Agent_Code;
                    IsfolderExist = CheckImageExist(obj.Department_Code + "//" + obj.Agent_Code + "//" + obj.Policy_No);
                    if (IsfolderExist == false)
                    {
                        var directoryInfo1 = new DirectoryInfo(Path);
                        directoryInfo1.CreateSubdirectory(obj.Policy_No);
                    }
                    Path = Path + "//" + obj.Policy_No;
                    string zipPath1 = ConfigurationManager.AppSettings["DocumentUploadZipPath"];
                    Random rnd = new Random();
                    obj.FileName = rnd.Next(1, 9999)+ obj.FileName;
                    bool isExist = System.IO.File.Exists(zipPath1 + obj.FileName);
                    FileStream fs = new FileStream(zipPath1 + obj.FileName, FileMode.Append);
                    if (FileBytes.Count() > 0)
                    {
                        fs.Write(FileBytes, 0, FileBytes.Length);
                    }
                    fs.Close();
                    ZipFile fileToExtract = ZipFile.Read(zipPath1 + obj.FileName);
                    fileToExtract.ExtractAll(Path, ExtractExistingFileAction.DoNotOverwrite);

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

        public bool CheckImageExist(string FolderName)
        {
            bool isExist = false;
            var fullpathExtrACTED = @"E:\AIA\uploaddocsmobility\";
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
            catch(Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
            }
            return httpResponseMessage;
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
            public string FIlePath{ get; set; }
        }
    }
}
