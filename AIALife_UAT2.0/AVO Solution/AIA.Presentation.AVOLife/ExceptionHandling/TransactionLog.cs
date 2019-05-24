using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AIA.Life.Repository.AIAEntity;
namespace AIA.Presentation.AVOLife.ExceptionHandling
{
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx == null || ctx.Session.IsNewSession)
            {
                //filterContext.Result = new RedirectResult("~/Account/LogOff");
                //return;
                base.OnActionExecuting(filterContext);

            }
            base.OnActionExecuting(filterContext);
        }
    }
}
