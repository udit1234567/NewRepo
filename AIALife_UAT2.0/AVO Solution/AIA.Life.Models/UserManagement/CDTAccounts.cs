using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.UserManagement
{
   public class CDTAccounts
    {
        public decimal CDTID { get; set; }
        public int AccountNo { get; set; }
        public decimal? InitialAmount { get; set; }
        public decimal? AvailableBalance { get; set; }
        public decimal? LedgerBalance { get; set; }
        public bool IsLocked { get; set; }
        public string PaymentType { get; set; }
        public string Remark { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
