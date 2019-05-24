using AIA.Life.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AIA.Services.API.Controllers.Life
{
    public class ReportsApiController : ApiController
    {

        public UWDecisionReport FetchDataForUWDecisionReport(UWDecisionReport objUWReport)
        {
            AIA.Life.Business.Reports.ReportsBusiness objReportsBusiness = new AIA.Life.Business.Reports.ReportsBusiness();
            return objReportsBusiness.FetchDataForUWDecisionReport(objUWReport);
        }
    }
}
