using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AIA.Life.Models.Common
{
    public class DocumentUpload
    {
        public int DocIndex { get; set; }
        public int DOCID { get; set; }
        public decimal? PolicyId { get; set; }
        public string Document_Name { get; set; }
        public string MemberType { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public string ExistingFileName { get; set; }
        public string HiddenFilePath { get; set; }

        public string Message { get; set; }
        public string UserName { get; set; }
        public List<DocumentUpload> objlstDocuments { get; set; }
    }

    public class DocumentUploadFile
    {
        public int DOCID { get; set; }
        public int Index { get; set; }
        public string DocName { get; set; }
        public string MemberType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Key { get; set; }
        public string ItemType { get; set; }
        public byte[] FileData { get; set; }
    }
    public class LeadDocumentUploadFile
    {
        public int DOCID { get; set; }
        public string DocName { get; set; }
      
    }

    public class DeletePolicyDocuments
    {

        public string Message { get; set; }
        public string UserName { get; set; }
        public string MemberType { get; set; }
        public decimal DocumentPolicyID { get; set; }
        public string DocumentName { get; set; }
        public decimal DocID { get; set; }
    }

    public class LdmsDocuments
    {
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public string AgentCode { get; set; }
        public string ProposalNo { get; set; }
        public string DocCode { get; set; }
    }
}
