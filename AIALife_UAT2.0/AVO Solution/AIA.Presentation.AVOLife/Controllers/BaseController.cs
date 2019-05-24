using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace AIA.Presentation.AVOLife.Controllers
{
    public class BaseController : Controller
    {

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
            {

                //cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
                //        Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
                //        null;
                cultureName = "en-IN";
            }
            //cultureName = "en-IN";

            var culture = CultureInfo.CreateSpecificCulture(cultureName);
            DateTimeFormatInfo englishDateTimeFormat = new CultureInfo("en-IN").DateTimeFormat;
            culture.DateTimeFormat = englishDateTimeFormat;
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            //Thread.CurrentThread.CurrentCulture.DateTimeFormat = englishDateTimeFormat;






            // Modify current thread's cultures           
            Thread.CurrentThread.CurrentCulture = culture; //new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }

    }
}