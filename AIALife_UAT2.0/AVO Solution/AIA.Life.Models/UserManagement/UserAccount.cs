using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.UserManagement
{
   public class UserAccount
    {
        //public int UserAccountID { get; set; }
        public string UserType { get; set; }
        public string UserCategory { get; set; }

        public string PrimaryContact { get; set; }

        public DateTime? EffectiveDate { get; set; }
        public DateTime? AccountExpiryDate { get; set; }
        public bool? Locked { get; set; }
        //public string PayType { get; set; }
        public decimal? CreditDayLimit { get; set; }
        //public bool? Status { get; set; }
        //public Guid? CreatedBy { get; set; }
        //public DateTime? CreatedDate { get; set; }
        public int? UserMode { get; set; }
        //public int? NoofLevels { get; set; }
        public int? HierarchyModel { get; set; }
        //public Guid? UserID { get; set; }
        public string RGICLBranch { get; set; }
        //public string CompanyName { get; set; }
        //public string LicenceNumber { get; set; }

       // public string PolicyBookingType { get; set; }



        public int UserAccountID { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> NoofLevels { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public string PolicyBookingType { get; set; }
        public string PayType { get; set; }
        public string CompanyName { get; set; }
        public string LicenceNumber { get; set; }
    }
}

