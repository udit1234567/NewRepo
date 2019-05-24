using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.Common
{
   public class ProductMasters
    {

        public string ProductName { get; set; }
        public string PlanCode { get; set; }
        public int ProductID { get; set; }

        public List<MasterListItem> LstPolicyTerm { get; set; }
        public List<MasterListItem> LstPremiumTerm { get; set; }

    }
}
