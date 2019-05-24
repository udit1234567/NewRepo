using AIA.Data.Life.API.ControllerLogic.DashBoard;
using AIA.Life.Models.DashBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace AIA.Data.Life.API.Controllers
{
    public class DashBoardController : ApiController
    {
        public GraphDetails GenerateDashboardData(GraphDetails objGraphDetails)
        {
            DashBoardLogic objLogic = new DashBoardLogic();            
            return objLogic.GenerateDashboardData(objGraphDetails);
        }
    }
}