using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JanaShakthi.Life.Business.Policy
{
     public  class PolicyBusiness
    {

        public JanaShakthi.Life.Models.Policy.Policy LoadMasters(JanaShakthi.Life.Models.Policy.Policy objpolicy)
        {
            #region Call API
            objpolicy = WebApiLogic.GetPostComplexTypeToAPI<JanaShakthi.Life.Models.Policy.Policy>(objpolicy, "LoadMasters", "Policy");
            #endregion
            return objpolicy;
        }
        public JanaShakthi.Life.Models.Common.Address FillAddressMasterList()
        {
            JanaShakthi.Life.Models.Common.Address objAddress = new JanaShakthi.Life.Models.Common.Address();

            #region Call API
            objAddress = WebApiLogic.GetPostComplexTypeToAPI<JanaShakthi.Life.Models.Common.Address>(objAddress, "FillAddressMasterList", "Policy");
            #endregion
            return objAddress;
        }

    }
}
