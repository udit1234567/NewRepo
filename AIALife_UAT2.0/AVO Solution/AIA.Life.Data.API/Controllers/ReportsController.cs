using AIA.Life.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AIA.Life.Data.API.Controllers
{
    public class ReportsController : ApiController
    {
        public UWDecisionReport FetchDataForUWDecisionReport(UWDecisionReport objUWReport)
        {
            AIA.Life.Data.API.ControllerLogic.Reports.ReportLogic objPolicyLogic = new AIA.Life.Data.API.ControllerLogic.Reports.ReportLogic();
            return objPolicyLogic.FetchDataForUWDecisionReport(objUWReport);
        }

        
    }
}
