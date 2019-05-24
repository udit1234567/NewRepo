using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Opportunity
{
   public  class ProductDetials
    {

        public string Plan { get; set; }
        public string Variant { get; set; }
        public string PlanCode { get; set; }
        public string PolicyTerm { get; set; }
        public string PremiumTerm { get; set; }
        public string PensionPeriod { get; set; }
        public string RetirementAge { get; set; }
        public string DrawDownPeriod { get; set; }
        public string MaturityBenefits { get; set; }        
        public string PreferredLangauage { get; set; }
        public string PreferredMode { get; set; }
        public int MonthlySurvivorIncome { get; set; }
        public long BasicSumInsured { get; set; }        
        public string AnnualPremium { get; set; }
        public int SAM { get; set; }
        public int APCP { get; set; }
        public bool IsFamilyFloater { get; set; }
        public bool Deductable { get; set; }
        public string IsAfc { get; set; }
        public string ModalPremium { get; set; }
    }
}
