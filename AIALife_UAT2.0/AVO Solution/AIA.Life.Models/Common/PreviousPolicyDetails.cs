using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Common
{
    public class PreviousPolicyDetails
    {
        public string POLICYNO { get; set; }
        public decimal? POlPREM { get; set; }
        public string LongDesc { get; set; }
        public decimal? ADB { get; set; }
        public decimal? CI { get; set; }
        public decimal? WOB { get; set; }
        public decimal? SumInsured { get; set; }
        public decimal? HDB { get; set; }

    }
    public class SARDetails
    {
        public decimal? SAR { get; set; }
        public decimal? ANNPREM { get; set; }
    }
    public class SARFALDetails
    {
        public int MemberID { get; set; }
        public decimal? SAR { get; set; }
        public decimal? FAL { get; set; }
        public Error Error { get; set; }
        public string ServiceTraceID { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string Nic { get; set; }
    }
    public class OnGoingProposalDetails
    {
        public string POLICYNO { get; set; }
        public decimal POlPREM { get; set; }
        public string LongDesc { get; set; }
        public decimal? ADB { get; set; }
        public decimal? CI { get; set; }
        public decimal? WOB { get; set; }
        public decimal? SumInsured { get; set; }
        public decimal? HDB { get; set; }

    }
    public class ProposalDetails
    {
        public List<PreviousPolicyDetails> PreviousProposalsList { get; set; }
        public List<OnGoingProposalDetails> OnGoingProposalsList { get; set; }
        public SARDetails SARDetails { get; set; }
        public List<SARFALDetails> SARFALDetails { get; set; }
        public string NICNo { get; set; }
        public string QuoteNo { get; set; }
        public Error Error { get; set; }
        public string ServiceTraceID { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
    }

}
