using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Policy
{


    public class ProposalInbox
    {
        public ProposalInbox()
        {
            objProposalDetails = new List<InboxDetails>();
        }
        public string Message { get; set; }
        public string UserName { get; set; }
        public string QuoteNo { get; set; }
        public List<InboxDetails> objProposalDetails { get; set; }
        public List<SubmittedProposals> LstSubmittedProposals { get; set; }
    }

    public class InboxDetails
    {
        public string QuoteNo { get; set; }
        public decimal PolicyID { get; set; }
        public string ProposalNo { get; set; }
        public string FirstName { get; set; }
        public string NIC { get; set; }
        public string MobileNo { get; set; }
        public string Home { get; set; }
        public string Work { get; set; }
        public string Salutation { get; set; }
        public string Surname { get; set; }
        public string LeadNo { get; set; }
        public string Banca { get; set; }
        public string PreferredLanguage { get; set; }
        public string ProductCode { get; set; }
        public string ProposalStatus { get; set; }
        public string FullName { get; set; }

        public string DaysInForCancellation { get; set; }
    }

    public class SubmittedProposals
    {
        public int Index { get; set; }
        public string PropId { get; set; }
        public string ProposalNo { get; set; }
        public string QuoteNo { get; set; }
        public string Name { get; set; }
        public string NicNo { get; set; }
        public string SubmittedPropMobile { get; set; }
        public string SubmittedPropPolicyTerm { get; set; }
        public string SubmittedPropHome { get; set; }
        public string SubmittedPropWork { get; set; }
        public string SubmittedPropEmail { get; set; }
        public string Status { get; set; }
        public string SubmittedPropInforce { get; set; }
        public string Salutation { get; set; }
        public string Surname { get; set; }
        public string LeadNo { get; set; }
        public string Banca { get; set; }
        public string FullName { get; set; }
    }

}
