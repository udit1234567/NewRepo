using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Business.Reports
{
   public class ReportsBusiness
    {

        public AIA.Life.Models.Reports.UWDecisionReport FetchDataForUWDecisionReport(AIA.Life.Models.Reports.UWDecisionReport objUWDecisionReport)
        {
            #region Call API
            objUWDecisionReport = WebApiLogic.GetPostComplexTypeToAPI<AIA.Life.Models.Reports.UWDecisionReport>(objUWDecisionReport, "FetchDataForUWDecisionReport", "Reports");
            #endregion
            return objUWDecisionReport;
        }
    }
}
