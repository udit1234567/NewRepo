using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AIA.Presentation.AVOLife.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult AuthorizeError(string errorMessage)
        {
            ViewBag.Message = errorMessage;
            return View();
        }
    }
}