using AIA.Life.Models.Opportunity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Integration.Services
{
     public class QuoteInfo
    {
        public string UserName { get; set; }
        public string QuoteNo { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public bool IsForServices { get; set; }
        public ProductDetials objProductDetials { get; set; }
        public string AnnualPremium { get; set; }
        public string HalfYearlyPremium { get; set; }
        public string QuaterlyPremium { get; set; }
        public string MonthlyPremium { get; set; }
        public string Cess { get; set; }
        public string PolicyFee { get; set; }
        public string VAT { get; set; }
        public bool IsSelfCovered { get; set; }
        public bool IsSpouseCovered { get; set; }
        public bool IsChildCovered { get; set; }
        public string NoofChilds { get; set; }
        public List<QuoteMemberDetails> objQuoteMemberDetails { get; set; }

    }
}
