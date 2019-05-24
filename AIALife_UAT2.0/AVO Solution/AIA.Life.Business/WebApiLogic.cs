using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using Newtonsoft.Json;
namespace AIA.Life.Business
{
   public static class WebApiLogic
    {
        static string _url = ConfigurationManager.AppSettings["JSDataLifeApiUrl"].ToString();
        public static T GetPostComplexTypeToAPI<T>(T obj, string MethodName, string controllerName)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_url);
                client.Timeout = TimeSpan.FromSeconds(900);
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
        
        public static HttpStatusCode GetPostComplexTypeToAPI(object obj, string MethodName, string controllerName, ref HttpResponseMessage response)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_url);
                client.Timeout = TimeSpan.FromSeconds(500);
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                response = client.PostAsJsonAsync("api/" + controllerName + "/" + MethodName, obj).Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response.StatusCode;
        }

        public static string ConvertToXML(this object obj)
        {
            XmlSerializer formatter = new XmlSerializer(obj.GetType());
            XDocument document = new XDocument();

            using (XmlWriter xmlWriter = document.CreateWriter())
            {
                formatter.Serialize(xmlWriter, obj);
            }
            if (document.Root != null) return document.Root.ToString();
            else return "";

        }
        public static T GetPostParametersToAPI<T>(T objType, string controllerName, string MethodName, string paramName = null, string ParamVal = null, object obj = null)
        {
            try
            {
                string _url = ConfigurationManager.AppSettings["JSDataLifeApiUrl"].ToString();
                if (paramName != null && ParamVal != null)
                    _url = _url + "api/" + controllerName + "/" + MethodName + "?" + paramName + "=" + ParamVal;
                else
                    _url = _url + "api/" + controllerName + "/" + MethodName;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
                if (obj != null)
                {
                    request.Timeout = 500000;
                    request.Method = "POST";
                    string requestData = JsonConvert.SerializeObject(obj);
                    ASCIIEncoding encoder = new ASCIIEncoding();
                    byte[] data = encoder.GetBytes(requestData);
                    data = Encoding.UTF8.GetBytes(obj.ConvertToXML());
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(data, 0, data.Length);
                    dataStream.Close();
                }
                else
                    request.Method = "GET";
                request.ContentType = "application/json";
                WebResponse response = request.GetResponse();
               string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                //var result = JsonConvert.SerializeObject(response);
                objType = JsonConvert.DeserializeObject<T>(result);
                return objType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
