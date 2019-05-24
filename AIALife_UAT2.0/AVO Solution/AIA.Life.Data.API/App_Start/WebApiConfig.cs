using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace AIA.Life.Data.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{Action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            // config.Routes.MapHttpRoute(
            //    name: "DefaultPostApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }
    }
}
