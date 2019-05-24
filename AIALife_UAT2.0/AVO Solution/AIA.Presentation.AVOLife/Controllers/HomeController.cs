using AIA.Life.Models.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AIA.Presentation.AVOLife.Controllers
{
    [Authorize]
    [ErrorLogging]
    public class HomeController : BaseController
    {
        private string Username = string.Empty;
        public HomeController()
        {
            Username = System.Web.HttpContext.Current.User.Identity.Name;
        }
        public ActionResult UnAuthorized()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetLandingDashboards(string UserName, string CloseWindow = "false")
        {

            if (!string.IsNullOrEmpty(UserName))
            {
                ViewBag.UserName = UserName;
                Username = CrossCutting.CrossCutting_EncryptDecrypt.Decrypt(UserName);
            }
            AIA.Life.Repository.AIAEntity.AVOAIALifeEntities entities = new Life.Repository.AIAEntity.AVOAIALifeEntities();
            string roleName = entities.usp_GetCurrentUserRole(Username).FirstOrDefault();
            if (roleName == "FPC-Banca")
            {
                return View("BancaHome");
            }
            else if (roleName == "WP" || roleName == "FM" || roleName == "TECHNO" || roleName == "RM" || roleName == "ZM" || roleName == "DSF")
            {
                return View("AgentHome");
            }
            else
            {
                return View("UnAuthorized");
            }
        }
        public ActionResult Index()
        {
            AIA.Life.Repository.AIAEntity.AVOAIALifeEntities entities = new Life.Repository.AIAEntity.AVOAIALifeEntities();
            string roleName = entities.usp_GetCurrentUserRole(HttpContext.User.Identity.Name).FirstOrDefault();
            if (roleName == "UW User" || roleName== "SUPADMIN")
            {
                return RedirectToAction("UnderwriterHome", "Home");
            }
            else if (roleName == "FPC-Banca")
            {
                return RedirectToAction("BancaHome", "Home");
            }
            else if (roleName == "WP" || roleName == "FM" || roleName == "TECHNO" || roleName == "RM" || roleName== "ZM" || roleName=="DSF")
            {
                return RedirectToAction("AgentHome", "Home");
            }
            else
            {
                return View("UnAuthorized");
            }
        }
        public ActionResult AgentHome()
        {
            return View();
        }
        public ActionResult BancaHome()
        {
            return View();
        }
        public ActionResult UnderwriterHome()
        {
            UWInbox obj = new UWInbox();
            obj.UserName = Username;
            AIA.Life.Business.Policy.PolicyBusiness objPolicyBusiness = new AIA.Life.Business.Policy.PolicyBusiness();
            obj = objPolicyBusiness.FetchUWProposalCount(obj);
            ViewBag.UWPoolCount = obj.UWPoolCount;
            return View(obj);
        }
        public class FinancialSummary
        {
            public FinancialSummary(int _FinancialSummaryID, int? _NoOfProposals, string _FinancialYear)
            {
                this.FinancialSummaryID = _FinancialSummaryID;
                this.NoOfProposals = _NoOfProposals;
                this.FinancialYear = _FinancialYear;
            }
            public int FinancialSummaryID { get; set; }
            public Nullable<int> NoOfProposals { get; set; }
            public string FinancialYear { get; set; }
        }

        public class FullCalender
        {
            public int id { get; set; }
            public string title { get; set; }
            public string start { get; set; }
            public bool? allDay { get; set; }
        }
        public ActionResult GetFinancialSummary()
        {
            List<FinancialSummary> lstFinancialSummary = new List<FinancialSummary>();
            FinancialSummary obj1 = new FinancialSummary(2, 100, "2014");
            FinancialSummary obj2 = new FinancialSummary(3, 200, "2015");
            FinancialSummary obj3 = new FinancialSummary(8, 400, "2016");
            FinancialSummary obj4 = new FinancialSummary(9, 500, "2017");
            lstFinancialSummary.Add(obj1);
            lstFinancialSummary.Add(obj2);
            lstFinancialSummary.Add(obj3);
            lstFinancialSummary.Add(obj4);
            return Json(lstFinancialSummary, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FetchCalenderEvents(int category)
        {
            List<FullCalender> lstCalender = new List<FullCalender>();
            for (int j = 0; j < 12; j++)
            {
                for (int i = 0; i < 30; i++)
                {
                    FullCalender obj = new FullCalender();
                    obj.title = (i + 10 * j).ToString();
                    obj.start = DateTime.Now.Year.ToString() + "-" + j.ToString().PadLeft(2, '0') + "-" + (DateTime.Now.Day - i).ToString().PadLeft(2, '0');
                    lstCalender.Add(obj);
                }
            }

            return Json(lstCalender, JsonRequestBehavior.AllowGet);
        }
        public ActionResult HomeMain()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult NewLayout()
        {
            return View();
        }

        public ActionResult LoadKnowledge()
        {
           

            return PartialView("~/Views/Home/_PartialKnowledgeCenter.cshtml");
        }
    }
}