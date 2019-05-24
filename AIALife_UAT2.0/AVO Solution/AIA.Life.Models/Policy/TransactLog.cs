using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Policy
{
   public class TransactLog
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string SerivceTraceID { get; set; }
        public string Message { get; set; }
    }
}
