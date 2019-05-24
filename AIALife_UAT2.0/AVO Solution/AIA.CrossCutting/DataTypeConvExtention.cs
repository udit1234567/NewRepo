using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AIA.CrossCutting
{
    public static class DataTypeConvExtention
    {
        public static int ToInt(this string intExt)
        {
            int convertedValue;
            int.TryParse(intExt, out convertedValue);
            return convertedValue;
        }
        public static decimal ToDecimal(this string decimalExt)
        {
            decimal convertedValue;
            decimal.TryParse(decimalExt, out convertedValue);
            return convertedValue;
        }
        public static double ToDouble(this string doubleExt)
        {
            double convertedValue;
            double.TryParse(doubleExt, out convertedValue);
            return convertedValue;
        }
        public static DateTime ToDate(this string Date)
        {
            DateTime d = new DateTime();
            DateTime.TryParse(Date, out d);
            return d;
        }

       
        public static decimal GetValueOrDefault(this decimal? DecimalValue)
        {
            if (DecimalValue != null && DecimalValue.HasValue)
            {
                return DecimalValue.Value;
            }
            return decimal.Zero;
        }

        public static int GetValueOrDefault(this int? intValue)
        {
            if (intValue != null && intValue.HasValue)
            {
                return intValue.Value;
            }
            return 0;
        }
        public static DateTime ToDdmmyyyyFormat(this string stringValue)
        {
            DateTime dateInFormat = new DateTime();
            string[] dateFormats = { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MM/yyyy h:mm:ss tt", "dd-MM-yyyy h:mm:ss tt", "dd/MM/yyyy h:mm:ss", "dd-MM-yyyy h:mm:ss", "dd/MM/yyyy hh:mm:ss" };
            if (!string.IsNullOrEmpty(stringValue))
            {
                DateTime.TryParseExact(stringValue, dateFormats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateInFormat);
            }
            if (stringValue == "")
            {
                return dateInFormat;
            }
            else if (dateInFormat == new DateTime())
            {
                dateInFormat = Convert.ToDateTime(stringValue);
            }

            return dateInFormat;
        }

        public static string ToYyyyMMddFormat(this DateTime dateValue)
        {
            string dateFormat = String.Format("{0:yyyy-MM-dd}", dateValue);
            return dateFormat;
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
        public static string SaveAsXML<T>(this T obj, string FilePath = null, string FileName = null)
        {
            if (FileName == null)
            {
                FileName = obj.GetType().Name;
            }
            if (FilePath == null)
            {
                FilePath = "Sample";
            }
           

            XmlSerializer formatter = new XmlSerializer(obj.GetType());
            XDocument document = new XDocument();

            using (XmlWriter xmlWriter = document.CreateWriter())
            {
                formatter.Serialize(xmlWriter, obj);
            }
            if (document.Root != null) return document.Root.ToString();
            return "";

        }


        public static string ToddMMyyyyString(this DateTime dateValue)
        {
            string dateFormat = String.Format("{0:dd/MM/yyyy}", dateValue);
            return dateFormat;
        }


        public static object ToNonAnonymousType<T>(this object obj, T type)
        {

            //create instance of T type object:
            var tmp = Activator.CreateInstance(Type.GetType(type.ToString()));

            //loop through the properties of the object you want to covert:          
            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {
                try
                {

                    //get the value of property and try 
                    //to assign it to the property of T type object:
                    tmp.GetType().GetProperty(pi.Name).SetValue(tmp,
                                              pi.GetValue(obj, null), null);
                }
                catch { }
            }

            //return the T type object:         
            return tmp;
        }

        public static int? ToIntforObject(this object value)
        {
            int? convertedValue;
            string strForm = Convert.ToString(value);
            try
            {
                convertedValue = Convert.ToInt32(strForm);
            }
            catch
            {
                convertedValue = null;
            }
            return convertedValue;
        }
    
        public static bool IsNullOrEmpty(this DataSet ds)
        {

            if (ds != null)
            {
                foreach (DataTable table in ds.Tables)
                    if (table.Rows.Count != 0) return true;

                return false;
            }
            else
            {
                return false;
            }


        }
        //public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        //{
        //    HashSet<TKey> seenKeys = new HashSet<TKey>();
        //    foreach (TSource element in source)
        //    {
        //        if (seenKeys.Add(keySelector(element)))
        //        {
        //            yield return element;
        //        }
        //    }
        //}

        public static string ToXml(this object obj)
        {
            string strOutPut = string.Empty;
            if (obj != null)
            {
                using (var ms = new MemoryStream())
                {
                    var xw = XmlWriter.Create(ms);// Remember to stop using XmlTextWriter  
                    var serializer = new XmlSerializer(obj.GetType());
                    serializer.Serialize(xw, obj);
                    xw.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    var sr = new StreamReader(ms, System.Text.Encoding.UTF8);
                    strOutPut = sr.ReadToEnd();
                }
            }
            return strOutPut;
        }
        public static byte[] ConcatPdf(List<byte[]> pdf)
        {
            byte[] all;

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document();

                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.SetPageSize(PageSize.A4);
                doc.Open();
                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;

                PdfReader reader;
                foreach (byte[] p in pdf)
                {
                    reader = new PdfReader(p);
                    int pages = reader.NumberOfPages;

                    // loop over document pages
                    for (int i = 1; i <= pages; i++)
                    {
                        doc.SetPageSize(PageSize.A4);
                        doc.NewPage();
                        page = writer.GetImportedPage(reader, i);
                        cb.AddTemplate(page, 0, 0);
                    }
                }

                doc.Close();
                all = ms.GetBuffer();
                ms.Flush();
                ms.Dispose();
            }
            return all;
        }
        public static string RegexNic(string textValue)
        {
            textValue= textValue.Replace("\r\n", "");
            string textResult = string.Empty;
            var reg1 = Regex.Matches(textValue, "[0-9]{9}[x|X|v|V]");
            var reg2 = Regex.Matches(textValue, "(\\d{9,9})?\\-?\\d{9} +v");
            var reg3 = Regex.Matches(textValue, "(\\d{9,9})?\\-?\\d{9} +V");
            var reg4 = Regex.Matches(textValue, "([0-9]{12})");
            var reg5 = Regex.Matches(textValue, "(?<![\\w\\d])No.:(?![\\w\\d]) ([0-9]{12})");

            if (reg1.Count == 1 || reg2.Count == 1 || reg3.Count == 1)
            {
                textResult = reg1.Count == 1 ? reg1[0].Value : reg2.Count == 1 ? reg2[0].Value : reg3.Count == 1 ? reg3[0].Value : "";
            }
            else if (reg4.Count == 1 || reg5.Count == 1)
            {
                var result = Regex.Match(reg4.Count == 1 ? reg4[0].Value : (reg5.Count == 1 ? reg5[0].Value : ""), @"\d+").Value;
                textResult = result;
            }
            else
            {
                var reg6 = Regex.Matches(textValue, "[0-9]{9}");
                if (reg6.Count == 1)
                {
                    textResult = reg6.Count == 1 ? reg6[0].Value : "";
                }
            }
            
            return textResult;
        }
    }
}
