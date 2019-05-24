using AIA.Life.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AIA.Life.Models.UWDecision
{
    public class UWDecision
    {
        public UWDecision()
        {
            lstMemberDeviationrules = new List<MemberDeviationrules>();
            lstUWMedicalDocument = new List<UWDocument>();
            lstUWNonMedicalDocument = new List<UWDocument>();
            ObjMemberDeviationrules = new MemberDeviationrules();
        }

        public string DocumentType { get; set; }
        public string AdditionalMedicalDocument { get; set; }
        public string AdditionalNonMedicalDocument { get; set; }
        public string Decision { get; set; }
        public string UWReason { get; set; }
        public string UWMedicalCode { get; set; }
        public int? UWMonth { get; set; }
        public DateTime? DecisionDate { get; set; }
        public DateTime? CommencementDate { get; set; }
        public string MedicalFeePaidBy { get; set; }

        public List<MemberDeviationrules> lstMemberDeviationrules { get; set; }
        public MemberDeviationrules ObjMemberDeviationrules { get; set; }
        public UWDocument objUWDocument { get; set; }
        public List<UWDocument> lstUWMedicalDocument { get; set; }
        public List<UWDocument> lstUWNonMedicalDocument { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }

        public string Remarks { get; set; }

        public MemberSummary objMemberSummary { get; set; }

        public List<MemberUWHistory> objMemberUWHistory { get; set; }
    }
    public class MemberDeviationrules
    {
        public MemberDeviationrules()
        {
            LstDecision = new List<MasterListItem>();
        }
        public int MemberDeviationid { get; set; }
        public string DeviatIonCode { get; set; }
        public string Deviation { get; set; }
        public string Decision { get; set; }
        public int? RuleId { get; set; }
        public List<MasterListItem> LstDecision { get; set; }

    }

    public class UWDocument
    {
        public UWDocument()
        {
            LstStatus = new List<MasterListItem>();
        }
        public int DocumentId { get; set; }
        public string Document { get; set; }
        public string Status { get; set; }
        public string Link { get; set; }
        public string FileName { get; set; }
        public DateTime? DateTime { get; set; }
        public string Remarks { get; set; }
        public bool IsAdded { get; set; }
        public List<MasterListItem> LstStatus { get; set; }
        public List<FileLinks> LstFileLinks { get; set; }
        public bool IsNewDocumentAddedbyUW { get; set; }
        public string DocType { get; set; }

    }

    public class FileLinks
    {
        public string FileName { get; set; }
        public string Link { get; set; }
    }

    public class UWMemberLevelDeviationStatus
    {
        public string UserName { get; set; }
        public int MemberDeviationID { get; set; }
        public string Status { get; set; }
    }

    public class MemberSummary
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public int Occupation { get; set; }
        public string SAR { get; set; }
        public string FAL { get; set; }
        public string TotalAnnualPremium { get; set; }
        public string BMIValue { get; set; }
        public bool AFC { get; set; }
    }

    public class MemberUWHistory
    {
        public int Day { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public string Decision { get; set; }
        public string UWName { get; set; }
        public DateTime? ProposalReceivedDate { get; set; }
        public List<UWDocument> objListDocuments { get; set; }
    }



    public class MemberLevelDecisions
    {
        public string UserName { get; set; }
        public List<MemberDecisions> objDecisions { get; set; }
        public string Message { get; set; }

        public string Result { get; set; }
        public string ResultText { get; set; }
    }

    public class MemberDecisions
    {
        public string AssuredName { get; set; }
        public string Decision { get; set; }
    }
    public class FollowUp
    {
        public int FupNo { get; set; }
        public string SigningTrail { get; set; }
        public string FupType { get; set; }
        public int LifeNo { get; set; }
        public string FupCode { get; set; }
        public string FupStatus { get; set; }
        public DateTime FupReminder { get; set; }
        public string FupRemark { get; set; }
        public string JointLife { get; set; }
        public string CrtUser { get; set; }
        public DateTime CrtDate { get; set; }
        public string LastUpUser { get; set; }
        public DateTime RecvDate { get; set; }
        public DateTime ExpDate { get; set; }
    }
}
