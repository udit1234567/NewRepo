using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.UserManagement
{
    public class UserExtension
    {
        public CDTAccounts tblCDTAccount { get; set; }
        public decimal UserExtensionID { get; set; }
        public decimal? NodeID { get; set; }
        //public Guid? UserID { get; set; }
        public int? Vendor_ID_PK { get; set; }
        public bool? isCDT { get; set; }
        public decimal? CDTID { get; set; }
        public int? CreditDays { get; set; }
        public bool? canMakeLive { get; set; }
        public bool? isCD { get; set; }
        public string CDAccountNo { get; set; }
        public string LockingMethod { get; set; }
        public bool? MakeLiveOnCHQACPT { get; set; }
        public bool? Status { get; set; }
        public DateTime? Effectivedate { get; set; }
        //public Guid? CreatedBy { get; set; }
        //public DateTime? CreatedDate { get; set; }
        public decimal? CreditLimit { get; set; }
        public decimal? LedgerBalance { get; set; }
        public decimal? AvailableBalance { get; set; }
        public bool? isBackdation { get; set; }
        public int? backdationDays { get; set; }
        public int? CreditDaysRemainder { get; set; }
        public bool? isPermOverRidden { get; set; }
        //public decimal UserExtensionID { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public Nullable<bool> PaymentType { get; set; }
        public Nullable<bool> FloatType { get; set; }
        public Nullable<decimal> FloatNo { get; set; }
        public Nullable<decimal> FloatLimit { get; set; }
        public Nullable<int> LimitDays { get; set; }
        public Nullable<decimal> AvailableFloat { get; set; }
        public Nullable<decimal> AdvDepNo { get; set; }
        public Nullable<decimal> AvailableAdvDeposit { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}




        
        
        
        
        
        
        
        
        
        
        
        
