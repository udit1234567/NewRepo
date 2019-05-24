using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Common
{
    public class AppVersion
    {
        public string AppName { get; set; }
        public string CloudPath { get; set; }
        public string VersionNo { get; set; }
        public DateTime CreatedDate { get; set; }
        public Error Error { get; set; }
        public string ServiceTraceID { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string AppType { get; set; }
        public bool isMandatory { get; set; }
    }
}
