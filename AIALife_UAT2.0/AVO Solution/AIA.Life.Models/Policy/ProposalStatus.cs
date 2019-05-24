using AIA.Life.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Policy
{
    public class ProposalStatus
    {
        public ProposalStatus()
        {
            Proposals = new List<ProposalList>();
            Error = new Error();
        }
        public List<ProposalList> Proposals { get; set; }
        public Error Error { get; set; }
        public string ServiceTraceID { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
    }
    public class ProposalList
    {
        public string ProposalNo { get; set; }
        public string Status { get; set; }
    }
}
