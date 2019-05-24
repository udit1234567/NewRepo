using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Common
{
    public class Error
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string WarningMessage { get; set; }
    }
    public class TpServiceLog
    {
        public string ServiceRequest { get; set; }
        public string ServiceResponse { get; set; }
        public string ServiceTraceID { get; set; }
    }
    public class ILStatus
    {
        public string ServiceName { get; set; }
        public string ProposalNo { get; set; }
        public string ServiceStatus { get; set; }
    }
}
