using AIA.Life.Business.Common;
using AIA.Life.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AIA.Presentation.AVOLife.Controllers
{
    [Authorize]
    [ErrorLogging]
    public class MasterController : Controller
    {
        // GET: Master
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetAddressMasters(string term)
        {
            CommonBusiness objBusiness = new CommonBusiness();
            AddressMaster objAddress = new AddressMaster();
            objAddress.Term = term;
            objAddress = objBusiness.GetAddresMasters(objAddress);
            List<SelectListItem> DropDownList = new List<SelectListItem>();
            foreach (var item in objAddress.LstAddressMaster)
            {
                DropDownList.Add(new SelectListItem { Text = item.Text, Value = item.Value });
            }
            return Json(DropDownList, JsonRequestBehavior.AllowGet);
        }
        public string GenerateRandomOTP()
        {
            int iOTPLength = 4;
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string sOTP = String.Empty;

            string sTempChars = String.Empty;

            Random rand = new Random();

            for (int i = 0; i < iOTPLength; i++)
            {

                int p = rand.Next(0, saAllowedCharacters.Length);

                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                sOTP += sTempChars;

            }

            return sOTP;

        }
    }
}