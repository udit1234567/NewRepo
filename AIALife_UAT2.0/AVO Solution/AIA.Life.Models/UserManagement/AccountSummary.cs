using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.UserManagement
{
   public class AccountSummary:CreateUserModel
    {
        public string transName { get; set; }
        public decimal? transAmount { get; set; }
        public List<AccountSummary> lstTransactions { get; set; }
        public decimal? transAmt { get; set; }
        public DateTime? transDateTime { get; set; }
        public string transDate { get; set; }
        public string transType { get; set; }
        public decimal? transAvailableBal { get; set; }
        public int slNo { get; set; }
    }
}
