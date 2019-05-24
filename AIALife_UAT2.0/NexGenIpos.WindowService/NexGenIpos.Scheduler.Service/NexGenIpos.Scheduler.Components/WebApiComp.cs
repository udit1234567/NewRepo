using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NexGenIpos.Scheduler.Components
{
    public static class WebApiComp
    {
        static string _url = ConfigurationManager.AppSettings["ApiUrl"].ToString();
        public static T GetPostComplexTypeToAPI<T>(T obj, string MethodName, string controllerName)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_url);
                client.Timeout = TimeSpan.FromSeconds(500);
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                response = client.PostAsJsonAsync("api/" + controllerName + "/" + MethodName, obj).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                    obj = response.Content.ReadAsAsync<T>().Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }
        
        public static void FireForgetAPI(object obj, string MethodName, string controllerName)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_url);
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.PostAsJsonAsync("api/" + controllerName + "/" + MethodName, obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
