using AIA.Life.Models.DashBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Business.DashBoard
{
   public  class DashBoardBL
    {
        public GraphDetails GenerateDashboardData(GraphDetails objGraphDetails)
        {
            #region Call API
            objGraphDetails = WebApiLogic.GetPostComplexTypeToAPI<GraphDetails>(objGraphDetails, "GenerateDashboardData", "DashBoard");
            #endregion
            return objGraphDetails;
        }
        public GraphDetails SaveDashboardData(GraphDetails objGraphDetails)
        {
            #region Call API
            objGraphDetails = WebApiLogic.GetPostComplexTypeToAPI<GraphDetails>(objGraphDetails, "GetAppointmentGrid", "DashBoard");
            #endregion
            return objGraphDetails;
        }
    }
}
