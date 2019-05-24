using AIA.Life.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AIA.Life.Models.UserManagement
{
   public class Float
    {
        public decimal IMDDetailsID { get; set; }
       public string IMDCode { get; set; }
       public string lineOfBusiness { get; set; }
       public string contractType { get; set; }
       public decimal limit { get; set; }
       public List<MasterListItem> lstImdCode { get; set; }
       public List<MasterListItem> lstLineOfBusiness { get; set; }
       public List<MasterListItem> lstContractType { get; set; }

        public string requestedUserID { get; set; }
        public string approvedUserID { get; set; }

        public DateTime? requestedDate { get; set; }
        public DateTime? approvedDate { get; set; }
        public DateTime? efectiveDate { get; set; }
        public string requestedDateStr { get; set; }
        public string approvedDateStr { get; set; }
        public string efectiveDateStr { get; set; }
        public bool? flag { get; set; }

        public List<FloatPermission> lstFloatPermission { get; set; }

    }

   public class FloatPermission
   {
       public decimal IMDDetailsID { get; set; }
       public string IMDCode { get; set; }
       public int lineOfBusiness { get; set; }
       public string contractTypeText { get; set; }
       public string lineOfBusinessText { get; set; }
       public int contractType { get; set; }
       public string autoTag { get; set; }
       public int allowableAging { get; set; }
       public string allowableAgingStr { get; set; }
       public decimal limit { get; set; }
       public int index { get; set; }
       public int productID { get; set; }
   }
}
