using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.UserManagement
{
   public class UserProductPermissions:Policy.Policy
    {
        public decimal ProductPermID { get; set; }
        public Guid? UserID { get; set; }
        public int? ProductID { get; set; }
        public int? PermissionID { get; set; }
        public bool? IsCDTAllowed { get; set; }
        public int? BackdationDays { get; set; }
        public int? QuoteValidity { get; set; }
        public bool? IsCDAllowed { get; set; }
        public bool? PaymentLink { get; set; }
        public int? AgeOfVehicle { get; set; }
        public int? AdvanceDays { get; set; }
        public bool? isNBAllowed { get; set; }
        public bool? isROAllowed { get; set; }
        public bool? isUsedAllowed { get; set; }
        public bool? isHOPrintAllowed { get; set; }
        public bool? ParentNBAllowed { get; set; }
        public bool? ParentROAllowed { get; set; }
        public bool? ParentUsedAllowed { get; set; }
        public bool? ParentHOPrintAllowed { get; set; }
        public bool isMakeModelSelectAll { get; set; }//Added by akshay to save Model select all
        public string vhProductCodes { get; set; }
        public IEnumerable<checkBoxListValues> LstMakeModel { get; set; }
        public IEnumerable<checkBoxListValues> ListMasMakeModel { get; set; }

    }
}
