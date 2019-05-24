using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AIA.CrossCutting;
using AIA.Life.Repository.AIAEntity;
using log4net;

namespace AIA.Presentation.AVOLife
{
    public class ErrorLogging : System.Web.Http.Filters.ExceptionFilterAttribute, IExceptionFilter
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        #region IExceptionFilter Members

        /// <summary>
        /// This method will fire everytime
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnException(ExceptionContext filterContext)
        {
            string UserName = "";
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity != null)
                UserName = LoginUser.GetUserName();

            string ErrorCode = Codes.GetErrorCode();
            log4net.GlobalContext.Properties["ErrorCode"] = ErrorCode;
            Logger.Error(filterContext.Exception);

            //This will execute in case Non Ajax call
            //if (!filterContext.HttpContext.Request.IsAjaxRequest())
            //{
            var CustomError = System.Configuration.ConfigurationManager.AppSettings["ShowCustomErrorPage"];
            if (CustomError != null && CustomError.ToString() == "true")
            {
                if (filterContext.HttpContext.Response.StatusCode != 404)
                {
                    var routeData = new RouteData();
                    routeData.Values["controller"] = "Error";
                    routeData.Values["action"] = "PageNotFoundError";

                    //IController errorsController = new 
                    var exception = HttpContext.Current.Server.GetLastError();
                    var httpException = exception as HttpException;

                    // assigning new route to controls
                    var rc = new RequestContext
                                 (
                                     new HttpContextWrapper(HttpContext.Current),
                                     routeData

                                 );
                    rc.RouteData.Values.Add("ErrorCode", ErrorCode);

                    //errorsController.Execute(rc);
                    filterContext.ExceptionHandled = true;
                }
            }


        }

        #endregion



    }

}
