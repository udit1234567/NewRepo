using System.Web.Mvc;

namespace JanaShakthi.Services.API.Areas.Life
{
    public class LifeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Life";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Life_default",
                "api/Life/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }

    }
}