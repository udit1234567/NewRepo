using AIA.Life.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AIA.Data.Life.API.Controllers
{
    public class ReportsController : ApiController
    {
        public UWDecisionReport FetchDataForUWDecisionReport(UWDecisionReport objUWReport)
        {
            AIA.Data.Life.API.ControllerLogic.Reports.ReportLogic objPolicyLogic = new AIA.Data.Life.API.ControllerLogic.Reports.ReportLogic();
            return objPolicyLogic.FetchDataForUWDecisionReport(objUWReport);
        }

        
    }
}
