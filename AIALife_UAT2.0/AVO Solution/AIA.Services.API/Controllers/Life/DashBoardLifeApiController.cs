using AIA.Life.Business.DashBoard;
using AIA.Life.Models.DashBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AIA.Services.API.Controllers.Life
{
    public class DashBoardLifeApiController : ApiController
    {
        DashBoardBL objDashboardLogic = new DashBoardBL();
        public GraphDetails GenerateDashboardData(GraphDetails objGraphDetails)
        {
            DashBoardBL objBusiness = new DashBoardBL();
            return objBusiness.GenerateDashboardData(objGraphDetails);
             
        }
        public GraphDetails SaveCalendarAppointment(GraphDetails objGraphDetails)
        {
            return objDashboardLogic.SaveDashboardData(objGraphDetails);
        }
    }
}