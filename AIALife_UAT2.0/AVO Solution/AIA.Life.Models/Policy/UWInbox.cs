using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Policy
{

    public class UWInbox
    {
        public List<InboxProposals> LstProposals { get; set; }



        public string Message { get; set; }
        public string UserName { get; set; }

        public int UWPoolCount { get; set; }
        public int SubmittedProposals { get; set; }
        public int AllocationCount { get; set; }
        public int AcceptedCount { get; set; }
        public int RejectCount { get; set; }
        public int PostponeCount { get; set; }
        public int WithDrawnCount { get; set; }
        public int CounterOffer { get; set; }
        public int NotTakenCount { get; set; }
        public int ReferToSRUWCount { get; set; }
        public int OutStandingCount { get; set; }
    }

    public class InboxProposals
    {
        public int Index { get; set; }
        public string ProposalNo { get; set; }
        public string QuoteNo { get; set; }
        public string InsuredName { get; set; }
        public string NIC { get; set; }
        public string PlanName { get; set; }
        public decimal PolicyId { get; set; }
        public string PolicyTerm { get; set; }
        public DateTime? IssueDate { get; set; }
        public int? NoofDays { get; set; }
        public decimal? Premium { get; set; }
        public decimal? AdditionalPremium { get; set; }
        public string Decision { get; set; }
        public int? ProductPriority { get; set; }
        public int? ChannelPriority { get; set; }
        public DateTime? AllocatedDate { get; set; }
        public bool? ISAFC { get; set; }
        public decimal? SARVal { get; set; }
        public string IssuedDate { get; set; }
        public string Premiumlkr { get; set; }
        public string Channel { get; set; }
    }




}
