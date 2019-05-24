using System.Web;
using System.Web.Mvc;

namespace AIA.Life.Data.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
