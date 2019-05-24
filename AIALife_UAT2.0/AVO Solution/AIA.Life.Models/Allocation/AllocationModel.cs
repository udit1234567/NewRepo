using AIA.Life.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Allocation
{
    public class AllocationModel
    {

        public AllocationModel()
        {
            objUWdetails = new List<UWDetails>();
            objChannelDetails = new List<ChannelDetails>();
        }
        public string UserName { get; set; }
        public string Message { get; set; }
        public List<UWDetails> objUWdetails { get; set; }
        public List<ChannelDetails> objChannelDetails { get; set; }
        public List<AllocationSummary> objAllocationSummary { get; set; }

    }

    public class UWDetails
    {
        public bool IsChecked { get; set; }
        public string UWName { get; set; }
        public string ID { get; set; }
        public bool Availabiliy { get; set; }
    }

    public class ChannelDetails
    {

        public string ChannelName { get; set; }
        public decimal ChannelId { get; set; }
        public bool Availabiliy { get; set; }
    }

    public class AllocationSummary
    {
        public decimal NodeId { get; set; }
        public string LoginId { get; set; }
        public string ChannelName { get; set; }
        public Int32 AllocationCount { get; set; }

    }


    public class ManualAllocation
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        
        public List<AllocationProposals> objLstAllocationProposals { get; set; }
        public List<AllocationProposals> objLstResetProposals { get; set; }
        public List<MasterListItem> LstUWName { get; set; }
    }

    public class AllocationProposals
    {
        public decimal PolicyID { get; set; }
        public bool ISSelected { get; set; }
        public int Index { get; set; }
        public string ProposalNoDisplay { get; set; }
        public string ProposalNo { get; set; }
        public string UWName { get; set; }
        public string AssignTo { get; set; }
    }


}
