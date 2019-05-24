using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.UserManagement
{
   public class ProductPayments
    {
        public string UserType { get; set; }
        public bool CdPayment { get; set; }
        public bool CdtPayment { get; set; }
        public bool ChequePayment { get; set; }
        public bool CashPayment { get; set; }
        public bool OnlinePayment { get; set; }
        public bool IVRPayment { get; set; }
        public bool PISPayment { get; set; }
        public string ProductCode { get; set; }
        public string DdlText { get; set; }
        public int DdlValue { get; set; }
        public int ProductClassID { get; set; }

        public bool ConfigView { get; set; }
        public bool ProductAndClass { get; set; }

        public int ddlProductClass { get; set; }
        public int ddlProduct { get; set; }

        public string additionalProducts { get; set; }

        public bool isConfigureChecked { get; set; }
        public bool isViewChecked { get; set; }
        public bool isProductClassChecked { get; set; }
        public bool isProductChecked { get; set; }
        public string gridData { get; set; }


        public List<ProductPayments> lstProductNames { get; set; }
    }
}
