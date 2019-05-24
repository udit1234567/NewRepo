using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace AIA.Presentation.Helpers
{
    public static class DateTimePickers
    {
        //public static MvcHtmlString iNubeDatePickerFor<tmodel, tpropoerty>(this HtmlHelper<tmodel> datehelper, Expression<Func<tmodel, tpropoerty>> expression, object htmlattributes = null, string onChange = null)
        //{
        //    string datepickerid = null;
        //    var exptext = ExpressionHelper.GetExpressionText(expression);
        //    var value = datehelper.ViewData.Eval(exptext);
        //    DateTime Mindate = DateTime.MinValue;
        //    string MaxDate = "12/21/2099";
        //    string style = string.Empty;
        //    if (value != null)
        //    {
        //        if (value is DateTime? && ((DateTime)value) != DateTime.MinValue)
        //            value = ((DateTime)value).ToShortDateString();
        //        else if (value is DateTime && (DateTime)value != DateTime.MinValue)
        //            value = ((DateTime)value).ToShortDateString();
        //        else if (value is string)
        //            value = (string)value;
        //    }

        //    var splittedexptext = exptext.Split('.');
        //    var textid = string.Join("", splittedexptext);
        //    var htmlAttributesDict = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlattributes);
        //    var htmlAttributesDictFor = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlattributes);
        //    if (datepickerid != null && datepickerid != "")
        //    {
        //    }
        //    else
        //    {
        //        object controlId = new object();
        //        //var MinValue = htmlAttributesDictFor.TryGetValue("MinDate", out controlId);
        //        var id = htmlAttributesDict.FirstOrDefault(a => a.Key == "id");
        //        htmlAttributesDict.TryGetValue("id", out controlId);
        //        if (controlId != null && controlId.ToString() != "" && controlId.ToString() != null)
        //            datepickerid = controlId.ToString();
        //    }

        //    object ValuehtmlAttribute = new object();
        //    htmlAttributesDict.TryGetValue("MinDate", out ValuehtmlAttribute);
        //    if (ValuehtmlAttribute != null && ValuehtmlAttribute.ToString() != null)
        //    {
        //        Mindate = (DateTime)htmlattributes.GetPropertyValue("MinDate");
        //    }

        //    htmlAttributesDict.TryGetValue("Maxdate", out ValuehtmlAttribute);
        //    if (ValuehtmlAttribute != null && ValuehtmlAttribute.ToString() != null)
        //    {
        //        MaxDate = (string)htmlattributes.GetPropertyValue("Maxdate");
        //        //MaxDate = ((DateTime)MaxDate).ToShortDateString();
        //    }

        //    htmlAttributesDictFor.Add("data-role", "htmlDatePicker");
        //    var box2 = datehelper.TextBoxFor(expression, "{0:dd/MM/yyyy}", htmlAttributesDictFor);
        //    string str = "<div id='div" + datepickerid + "' class='input-group date' data-date-format='DD/MM/YYYY' style='width:75%'>" +
        //        box2 + "<span class='input-group-addon' style='padding:0px'><span class='glyphicon glyphicon-calendar'></span></span>" + "</div>";
        //    string evnt = string.IsNullOrEmpty(onChange) ? "" : ".change(" + onChange + ").on('changeDate'," + onChange + ")";
            
        //    str = str + "<script type=\"text/javascript\"> $(function () { $('#div" + datepickerid + "').datetimepicker({format: 'DD/MM/YYYY', useCurrent: false,pickTime: false})" + evnt + "; });</script>";
            
        //    MvcHtmlString result = new MvcHtmlString(str);
        //    return result;
        //}

        public static MvcHtmlString iNubeDatePickerFor<tmodel, tpropoerty>(this HtmlHelper<tmodel> datehelper, Expression<Func<tmodel, tpropoerty>> expression, object htmlattributes = null, string onChange = null)
        {
            string datepickerid = null;
            var exptext = ExpressionHelper.GetExpressionText(expression);
            string disabled = "false";
            var value = datehelper.ViewData.Eval(exptext);
           string Mindate = "12/21/1918";           
            string MaxDate = "12/21/2099";
            if (value != null)
            {
                if (value is DateTime? && ((DateTime)value) != DateTime.MinValue)
                    value = ((DateTime)value).ToShortDateString();
                else if (value is DateTime && (DateTime)value != DateTime.MinValue)
                    value = ((DateTime)value).ToShortDateString();
                else if (value is string)
                    value = (string)value;
            }

            var splittedexptext = exptext.Split('.');
            var textid = string.Join("", splittedexptext);
            var htmlAttributesDict = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlattributes);
            var htmlAttributesDictFor = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlattributes);
            if (datepickerid != null && datepickerid != "")
            {
            }
            else
            {
                object controlId = new object();
                //var MinValue = htmlAttributesDictFor.TryGetValue("MinDate", out controlId);
                var id = htmlAttributesDict.FirstOrDefault(a => a.Key == "id");
                htmlAttributesDict.TryGetValue("id", out controlId);
                if (controlId != null && controlId.ToString() != "" && controlId.ToString() != null)
                    datepickerid = controlId.ToString();
            }

            object ValuehtmlAttribute = new object();
            htmlAttributesDict.TryGetValue("MinDate", out ValuehtmlAttribute);
            if (ValuehtmlAttribute != null && ValuehtmlAttribute.ToString() != null)
            {
                DateTime MinimunDate =Convert.ToDateTime( htmlattributes.GetPropertyValue("MinDate"));
                Mindate = MinimunDate.Month + "/" + MinimunDate.Day + "/" + MinimunDate.Year;
                //Mindate = Convert.ToString(htmlattributes.GetPropertyValue("MinDate"));
            }

            htmlAttributesDict.TryGetValue("Maxdate", out ValuehtmlAttribute);
            if (ValuehtmlAttribute != null && ValuehtmlAttribute.ToString() != null)
            {
                // MaxDate = (string)htmlattributes.GetPropertyValue("Maxdate");
                DateTime MaximumDate = Convert.ToDateTime(htmlattributes.GetPropertyValue("Maxdate"));
                MaxDate = MaximumDate.Month + "/" + MaximumDate.Day + "/" + MaximumDate.Year;

            }

            htmlAttributesDictFor.Add("data-role", "htmlDatePicker");
            
            var box2 = datehelper.TextBoxFor(expression, "{0:dd/MM/yyyy}", htmlAttributesDictFor);
            string str = "<div id='div" + datepickerid + "' class='input-group date' data-date-format='DD/MM/YYYY'>" +
                box2 + "<span class='input-group-addon' style='padding:0px'><i class='fa fa-calendar '></i></span>" + "</div>";
            string evnt = string.IsNullOrEmpty(onChange) ? "" : ".change(" + onChange + ").on('changeDate'," + onChange + ")";
            //str = str + "<script type=\"text/javascript\"> $(function () { $('#div" + datepickerid + "').datetimepicker({format:moment().format('DD/MM/YYYY') , useCurrent: false,minDate:'" + Mindate + "',maxDate:'" + MaxDate + "'})" + evnt + "; });</script>";
            str = str + "<script type=\"text/javascript\"> $(function () { $('#div" + datepickerid + "').datetimepicker({format: 'DD/MM/YYYY', widgetPositioning:{horizontal: 'auto',vertical: 'bottom'},useCurrent: false,minDate:'" + Mindate + "',maxDate:'" + MaxDate + "'})" + evnt + "; });</script>";
           // str = str + "<script type=\"text/javascript\"> $(function () { $('#div" + datepickerid + "').datetimepicker({format: 'DD/MM/YYYY',useCurrent: false,minDate:'" + Mindate + "',maxDate:'" + MaxDate + "'})" + evnt + "; });</script>";

            MvcHtmlString result = new MvcHtmlString(str);
            return result;
        }
        public static MvcHtmlString iNubeDatePicker1For<tmodel, tpropoerty>(this HtmlHelper<tmodel> datehelper, Expression<Func<tmodel, tpropoerty>> expression, object htmlattributes = null, string onChange = null)
        {
            string datepickerid = null;
            var exptext = ExpressionHelper.GetExpressionText(expression);
            string disabled = "false";
            var value = datehelper.ViewData.Eval(exptext);
            string Mindate = "12/21/1918";
            string MaxDate = "12/21/2099";
            if (value != null)
            {
                if (value is DateTime? && ((DateTime)value) != DateTime.MinValue)
                    value = ((DateTime)value).ToShortDateString();
                else if (value is DateTime && (DateTime)value != DateTime.MinValue)
                    value = ((DateTime)value).ToShortDateString();
                else if (value is string)
                    value = (string)value;
            }

            var splittedexptext = exptext.Split('.');
            var textid = string.Join("", splittedexptext);
            var htmlAttributesDict = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlattributes);
            var htmlAttributesDictFor = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlattributes);
            if (datepickerid != null && datepickerid != "")
            {
            }
            else
            {
                object controlId = new object();
                //var MinValue = htmlAttributesDictFor.TryGetValue("MinDate", out controlId);
                var id = htmlAttributesDict.FirstOrDefault(a => a.Key == "id");
                htmlAttributesDict.TryGetValue("id", out controlId);
                if (controlId != null && controlId.ToString() != "" && controlId.ToString() != null)
                    datepickerid = controlId.ToString();
            }

            object ValuehtmlAttribute = new object();
            htmlAttributesDict.TryGetValue("MinDate", out ValuehtmlAttribute);
            if (ValuehtmlAttribute != null && ValuehtmlAttribute.ToString() != null)
            {
                DateTime MinimunDate = Convert.ToDateTime(htmlattributes.GetPropertyValue("MinDate"));
                Mindate = MinimunDate.Month + "/" + MinimunDate.Day + "/" + MinimunDate.Year;
                //Mindate = Convert.ToString(htmlattributes.GetPropertyValue("MinDate"));
            }

            htmlAttributesDict.TryGetValue("Maxdate", out ValuehtmlAttribute);
            if (ValuehtmlAttribute != null && ValuehtmlAttribute.ToString() != null)
            {
                // MaxDate = (string)htmlattributes.GetPropertyValue("Maxdate");
                DateTime MaximumDate = Convert.ToDateTime(htmlattributes.GetPropertyValue("Maxdate"));
                MaxDate = MaximumDate.Month + "/" + MaximumDate.Day + "/" + MaximumDate.Year;

            }

            htmlAttributesDictFor.Add("data-role", "htmlDatePicker");

            var box2 = datehelper.TextBoxFor(expression, "{0:dd/MM/yyyy}", htmlAttributesDictFor);
            string str = "<div id='div" + datepickerid + "' class='input-group date' data-date-format='DD/MM/YYYY'>" +
                box2 + "<span class='input-group-addon' style='padding:0px'><i class='fa fa-calendar '></i></span>" + "</div>";
            string evnt = string.IsNullOrEmpty(onChange) ? "" : ".change(" + onChange + ").on('changeDate'," + onChange + ")";
            //str = str + "<script type=\"text/javascript\"> $(function () { $('#div" + datepickerid + "').datetimepicker({format:moment().format('DD/MM/YYYY') , useCurrent: false,minDate:'" + Mindate + "',maxDate:'" + MaxDate + "'})" + evnt + "; });</script>";
            str = str + "<script type=\"text/javascript\"> $(function () { $('#div" + datepickerid + "').datetimepicker({format: 'DD/MM/YYYY', widgetPositioning:{horizontal: 'auto',vertical: 'bottom'},useCurrent: false,minDate:'" + Mindate + "',maxDate:'" + MaxDate + "'})" + evnt + "; });</script>";
            // str = str + "<script type=\"text/javascript\"> $(function () { $('#div" + datepickerid + "').datetimepicker({format: 'DD/MM/YYYY',useCurrent: false,minDate:'" + Mindate + "',maxDate:'" + MaxDate + "'})" + evnt + "; });</script>";

            MvcHtmlString result = new MvcHtmlString(str);
            return result;
        }

        public static MvcHtmlString DatePickerFor<tmodel, tpropoerty>(this HtmlHelper<tmodel> datehelper, Expression<Func<tmodel, tpropoerty>> expression, object htmlattributes = null, string onChange = null, string width = "75%")
        {
            string datepickerid = null;
            var exptext = ExpressionHelper.GetExpressionText(expression);
            var value = datehelper.ViewData.Eval(exptext);
            DateTime Mindate = DateTime.MinValue;
            string MaxDate = "12/21/2099";
            string style = string.Empty;
            if (value != null)
            {
                if (value is DateTime? && ((DateTime)value) != DateTime.MinValue)
                    value = ((DateTime)value).ToShortDateString();
                else if (value is DateTime && (DateTime)value != DateTime.MinValue)
                    value = ((DateTime)value).ToShortDateString();
                else if (value is string)
                    value = (string)value;
            }

            var splittedexptext = exptext.Split('.');
            var textid = string.Join("", splittedexptext);
            var htmlAttributesDict = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlattributes);
            var htmlAttributesDictFor = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlattributes);
            if (datepickerid != null && datepickerid != "")
            {
            }
            else
            {
                object controlId = new object();
                //var MinValue = htmlAttributesDictFor.TryGetValue("MinDate", out controlId);
                var id = htmlAttributesDict.FirstOrDefault(a => a.Key == "id");
                htmlAttributesDict.TryGetValue("id", out controlId);
                if (controlId != null && controlId.ToString() != "" && controlId.ToString() != null)
                    datepickerid = controlId.ToString();
            }

            object ValuehtmlAttribute = new object();
            htmlAttributesDict.TryGetValue("MinDate", out ValuehtmlAttribute);
            if (ValuehtmlAttribute != null && ValuehtmlAttribute.ToString() != null)
            {
                Mindate = (DateTime)htmlattributes.GetPropertyValue("MinDate");
            }

            htmlAttributesDict.TryGetValue("Maxdate", out ValuehtmlAttribute);
            if (ValuehtmlAttribute != null && ValuehtmlAttribute.ToString() != null)
            {
                MaxDate = (string)htmlattributes.GetPropertyValue("Maxdate");
                //MaxDate = ((DateTime)MaxDate).ToShortDateString();
            }

            htmlAttributesDictFor.Add("data-role", "htmlDatePicker");
            var box2 = datehelper.TextBoxFor(expression, "{0:dd/MM/yyyy}", htmlAttributesDictFor);
            string str = "<div id='div" + datepickerid + "' class='input-group date' style='width:" + width + "'>" +
                box2 + "<span class='input-group-addon' style='padding:0px'><span class='glyphicon glyphicon-calendar'></span></span>" + "</div>";
            string evnt = string.IsNullOrEmpty(onChange) ? "" : ".change(" + onChange + ").on('changeDate'," + onChange + ")";
            str = str + "<script type=\"text/javascript\"> $(function () { $('#div" + datepickerid + "').datepicker({format: 'dd/mm/yyyy',todayHighlight: true});});</script>";
            MvcHtmlString result = new MvcHtmlString(str);
            return result;
        }



        public static MvcHtmlString iNubeDatePickerForGrid(this HtmlHelper datehelper, string Value, string id, object htmlattributes = null, string onChange = null)
        {
            string datepickerid = null;
          
            DateTime Mindate = DateTime.MinValue;
            DateTime MaxDate = DateTime.MaxValue;
          
            var htmlAttributesDict = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlattributes);
            var htmlAttributesDictFor = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlattributes);
            if (id != null && id != "")
            {
                datepickerid = id;
            }
          

            object ValuehtmlAttribute = new object();
            htmlAttributesDict.TryGetValue("MinDate", out ValuehtmlAttribute);
            if (ValuehtmlAttribute != null && ValuehtmlAttribute.ToString() != null)
            {
                Mindate = (DateTime)htmlattributes.GetPropertyValue("MinDate");
            }

            htmlAttributesDict.TryGetValue("MaxDate", out ValuehtmlAttribute);
            if (ValuehtmlAttribute != null && ValuehtmlAttribute.ToString() != null)
            {
                MaxDate = (DateTime)htmlattributes.GetPropertyValue("MaxDate");
                
            }

            if (Value != null && Value != "")
            {
                Value = DateTime.Parse(Value).ToShortDateString();
            }
            if (Value == "01-01-0001")
            {
                Value = "";
            }
            

            var box2 = datehelper.TextBox(id, Value, "{0:dd/MM/yyyy}", htmlAttributesDictFor);
            string str = "<div id='div" + datepickerid + "' class='input-group date' data-date-format='DD/MM/YYYY' style=''>" +
                box2 + "<span class='input-group-addon' style='padding:0px'><i class='fa fa-calendar'></i></span>" + "</div>";
            string evnt = string.IsNullOrEmpty(onChange) ? "" : ".change(" + onChange + ").on('changeDate'," + onChange + ")";
             str = str + "<script type=\"text/javascript\"> $(function () { $('#div" + datepickerid + "').datetimepicker({format: 'DD/MM/YYYY', useCurrent: false,pickTime: false,minDate:'" + Mindate + "',maxDate:'" + MaxDate + "'})" + evnt + "; });</script>";
            //str = str + "<script type=\"text/javascript\"> $(function () { $('#" + datepickerid + "').datepicker({format: 'dd/mm/yyyy', useCurrent: false,pickTime: false})" + evnt + "; });</script>";
            MvcHtmlString result = new MvcHtmlString(str);
            return result;
        }

        public static MvcHtmlString iNubeDatePickerForGridNew(this HtmlHelper datehelper, string Value, string Name, string ID, object htmlattributes = null, string onChange = null)
        {
            string datepickerid = null;
           
            //DateTime Mindate = DateTime.MinValue;
            //DateTime MaxDate = DateTime.MaxValue;
            string Mindate = "12/21/1918";
            string MaxDate = "12/21/2099";
            var htmlAttributesDict = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlattributes);
            var htmlAttributesDictFor = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlattributes);
            if (ID != null && ID != "")
            {
                datepickerid = ID;
            }
            


            object ValuehtmlAttribute = new object();
            htmlAttributesDict.TryGetValue("MinDate", out ValuehtmlAttribute);
            if (ValuehtmlAttribute != null && ValuehtmlAttribute.ToString() != null)
            {
                DateTime MinimunDate = Convert.ToDateTime(htmlattributes.GetPropertyValue("MinDate"));
                Mindate = MinimunDate.Month + "/" + MinimunDate.Day + "/" + MinimunDate.Year;
                //Mindate = Convert.ToString(htmlattributes.GetPropertyValue("MinDate"));
            }

            htmlAttributesDict.TryGetValue("Maxdate", out ValuehtmlAttribute);
            if (ValuehtmlAttribute != null && ValuehtmlAttribute.ToString() != null)
            {
                // MaxDate = (string)htmlattributes.GetPropertyValue("Maxdate");
                DateTime MaximumDate = Convert.ToDateTime(htmlattributes.GetPropertyValue("Maxdate"));
                MaxDate = MaximumDate.Month + "/" + MaximumDate.Day + "/" + MaximumDate.Year;

            }



            if (Value != null && Value != "")
            {
                Value = DateTime.Parse(Value).ToShortDateString();
                if (!string.IsNullOrEmpty(Value))
                {
                    Value = Value.Replace("-", "/");
                }
            }
            if (Value == "01-01-0001")
            {
                Value = "";
            }


            var box2 = datehelper.TextBox(Name, Value, "{0:dd/MM/yyyy}", htmlAttributesDictFor);
            string str = "<div id='div" + datepickerid + "'  class='input-group date feild-margin2' data-date-format='DD/MM/YYYY' style=''>" +
                box2 + "<span class='input-group-addon' style='padding:0px'><span class='fa fa-calendar'></span></span>" + "</div>";
            string evnt = string.IsNullOrEmpty(onChange) ? "" : ".change(" + onChange + ").on('changeDate'," + onChange + ")";
            str = str + "<script type=\"text/javascript\"> $(function () { $('#div" + datepickerid + "').datetimepicker({format: 'DD/MM/YYYY', useCurrent: false,minDate:'" + Mindate + "',maxDate:'" + MaxDate + "'})" + evnt + "; });</script>";
            // str = str + "<script type=\"text/javascript\"> $(function () { $('#div" + datepickerid + "').datetimepicker({format: 'DD/MM/YYYY',widgetPositioning:{horizontal: 'auto',vertical: 'bottom'},useCurrent: false,minDate:'" + Mindate + "',maxDate:'" + MaxDate + "'})" + evnt + "; });</script>";
            MvcHtmlString result = new MvcHtmlString(str);
            
            return result;
        }

        public static MvcHtmlString iNubeDatePickerForGridViewNew(this HtmlHelper datehelper, string Value, string Name, string ID, object htmlattributes = null, string onChange = null)
        {
            string datepickerid = null;

            //DateTime Mindate = DateTime.MinValue;
            //DateTime MaxDate = DateTime.MaxValue;
            string Mindate = "12/21/1918";
            string MaxDate = "12/21/2099";
            var htmlAttributesDict = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlattributes);
            var htmlAttributesDictFor = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlattributes);
            //if (ID != null && ID != "")
            //{
            //    datepickerid = ID;
            //}
            if (datepickerid != null && datepickerid != "")
            {
            }
            else
            {
                object controlId = new object();
                //var MinValue = htmlAttributesDictFor.TryGetValue("MinDate", out controlId);
                var id = htmlAttributesDict.FirstOrDefault(a => a.Key == "id");
                htmlAttributesDict.TryGetValue("id", out controlId);
                if (controlId != null && controlId.ToString() != "" && controlId.ToString() != null)
                    datepickerid = controlId.ToString();
            }


            object ValuehtmlAttribute = new object();
            htmlAttributesDict.TryGetValue("MinDate", out ValuehtmlAttribute);
            if (ValuehtmlAttribute != null && ValuehtmlAttribute.ToString() != null)
            {
                DateTime MinimunDate = Convert.ToDateTime(htmlattributes.GetPropertyValue("MinDate"));
                Mindate = MinimunDate.Month + "/" + MinimunDate.Day + "/" + MinimunDate.Year;
                //Mindate = Convert.ToString(htmlattributes.GetPropertyValue("MinDate"));
            }

            htmlAttributesDict.TryGetValue("Maxdate", out ValuehtmlAttribute);
            if (ValuehtmlAttribute != null && ValuehtmlAttribute.ToString() != null)
            {
                // MaxDate = (string)htmlattributes.GetPropertyValue("Maxdate");
                DateTime MaximumDate = Convert.ToDateTime(htmlattributes.GetPropertyValue("Maxdate"));
                MaxDate = MaximumDate.Month + "/" + MaximumDate.Day + "/" + MaximumDate.Year;

            }



            if (Value != null && Value != "")
            {
                Value = Convert.ToString(Value).ToString();
                if (!string.IsNullOrEmpty(Value))
                {
                    Value = Value.Replace("-", "/");
                }
            }
            if (Value == "01-01-0001")
            {
                Value = "";
            }


            var box2 = datehelper.TextBox(Name, Value, "{0:dd/MM/yyyy}", htmlAttributesDictFor);
            string str = "<div id='div" + datepickerid + "' class='input-group date feild-margin2' data-date-format='DD/MM/YYYY' style=''>" +
                box2 + "<span class='input-group-addon' style='padding:0px'><span class='fa fa-calendar'></span></span>" + "</div>";
            string evnt = string.IsNullOrEmpty(onChange) ? "" : ".change(" + onChange + ").on('changeDate'," + onChange + ")";
            str = str + "<script type=\"text/javascript\"> $(function () { $('#div" + datepickerid + "').datetimepicker({format: 'DD/MM/YYYY', useCurrent: false,minDate:'" + Mindate + "',maxDate:'" + MaxDate + "'})" + evnt + "; });</script>";
            // str = str + "<script type=\"text/javascript\"> $(function () { $('#div" + datepickerid + "').datetimepicker({format: 'DD/MM/YYYY',widgetPositioning:{horizontal: 'auto',vertical: 'bottom'},useCurrent: false,minDate:'" + Mindate + "',maxDate:'" + MaxDate + "'})" + evnt + "; });</script>";
            MvcHtmlString result = new MvcHtmlString(str);
            return result;
        }

        public static MvcHtmlString iNubeBackDatePickerForGridViewNew(this HtmlHelper datehelper, string Value, string Name, string ID, object htmlattributes = null, string onChange = null)
        {
            string datepickerid = null;

            //DateTime Mindate = DateTime.MinValue;
            //DateTime MaxDate = DateTime.MaxValue;
            //string Mindate = "12/21/1918";
            string Mindate1 = Convert.ToString(DateTime.Now.AddMonths(-6));
            string[] Mindate2 = Mindate1.Replace('-', '/').Split(' ').ToArray();
            string MinDate = Convert.ToString(Mindate2[0]);
            string MaxDate = "12/21/2099";
            var htmlAttributesDict = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlattributes);
            var htmlAttributesDictFor = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlattributes);
            //if (ID != null && ID != "")
            //{
            //    datepickerid = ID;
            //}
            if (datepickerid != null && datepickerid != "")
            {
            }
            else
            {
                object controlId = new object();
                //var MinValue = htmlAttributesDictFor.TryGetValue("MinDate", out controlId);
                var id = htmlAttributesDict.FirstOrDefault(a => a.Key == "id");
                htmlAttributesDict.TryGetValue("id", out controlId);
                if (controlId != null && controlId.ToString() != "" && controlId.ToString() != null)
                    datepickerid = controlId.ToString();
            }


            object ValuehtmlAttribute = new object();
            htmlAttributesDict.TryGetValue("Mindate", out ValuehtmlAttribute);
            if (ValuehtmlAttribute != null && ValuehtmlAttribute.ToString() != null)
            {
                DateTime MinimunDate = Convert.ToDateTime(htmlattributes.GetPropertyValue("Mindate"));
                MinDate = MinimunDate.Month + "/" + MinimunDate.Day + "/" + MinimunDate.Year;
                //MinDate = Convert.ToString(htmlattributes.GetPropertyValue("Mindate"));
            }

            htmlAttributesDict.TryGetValue("Maxdate", out ValuehtmlAttribute);
            if (ValuehtmlAttribute != null && ValuehtmlAttribute.ToString() != null)
            {
                // MaxDate = (string)htmlattributes.GetPropertyValue("Maxdate");
                DateTime MaximumDate = Convert.ToDateTime(htmlattributes.GetPropertyValue("Maxdate"));
                MaxDate = MaximumDate.Month + "/" + MaximumDate.Day + "/" + MaximumDate.Year;

            }



            if (Value != null && Value != "")
            {
                Value = Convert.ToString(Value).ToString();
                if (!string.IsNullOrEmpty(Value))
                {
                    Value = Value.Replace("-", "/");
                }
            }
            if (Value == "01-01-0001")
            {
                Value = "";
            }
            
            var box2 = datehelper.TextBox(Name, Value, "{0:dd/MM/yyyy}", htmlAttributesDictFor);
            string str = "<div id='div" + datepickerid + "' class='input-group date feild-margin2' data-date-format='DD/MM/YYYY' style=''>" +
                box2 + "<span class='input-group-addon' style='padding:0px'><span class='fa fa-calendar'></span></span>" + "</div>";
            string evnt = string.IsNullOrEmpty(onChange) ? "" : ".change(" + onChange + ").on('changeDate'," + onChange + ")";
            str = str + "<script type=\"text/javascript\"> $(function () { $('#div" + datepickerid + "').datetimepicker({format: 'DD/MM/YYYY', useCurrent: false,minDate:'" + MinDate + "',maxDate:'" + MaxDate + "'})" + evnt + "; });</script>";
            // str = str + "<script type=\"text/javascript\"> $(function () { $('#div" + datepickerid + "').datetimepicker({format: 'DD/MM/YYYY',widgetPositioning:{horizontal: 'auto',vertical: 'bottom'},useCurrent: false,minDate:'" + Mindate + "',maxDate:'" + MaxDate + "'})" + evnt + "; });</script>";
            MvcHtmlString result = new MvcHtmlString(str);
            return result;
        }

        public static MvcHtmlString iNubeDatePickerForNew(this HtmlHelper datehelper, string Value, string id, object htmlattributes = null, string onChange = null, string modelParam = null)
        {
            string datepickerid = null;

            DateTime Mindate = DateTime.MinValue;
            DateTime MaxDate = DateTime.MaxValue;

            var htmlAttributesDict = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlattributes);
            var htmlAttributesDictFor = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlattributes);
            if (id != null && id != "")
            {
                datepickerid = id;
            }
           

            object ValuehtmlAttribute = new object();
            htmlAttributesDict.TryGetValue("MinDate", out ValuehtmlAttribute);
            if (ValuehtmlAttribute != null && ValuehtmlAttribute.ToString() != null)
            {
                Mindate = (DateTime)htmlattributes.GetPropertyValue("MinDate");
            }

            htmlAttributesDict.TryGetValue("MaxDate", out ValuehtmlAttribute);
            if (ValuehtmlAttribute != null && ValuehtmlAttribute.ToString() != null)
            {
                MaxDate = (DateTime)htmlattributes.GetPropertyValue("MaxDate");
                
            }
            if (Value != null && Value != "")
            {
                Value = DateTime.Parse(Value).ToShortDateString();
            }
            if (Value == "01-01-0001")
            {
                Value = "";
            }
            

            var box2 = datehelper.TextBox(modelParam, Value, "{0:dd/MM/yyyy}", htmlAttributesDictFor);
            string str = "<div id='div" + datepickerid + "' class='input-group date' data-date-format='DD/MM/YYYY' style=''>" +
                box2 + "<span class='input-group-addon' style='padding:0px'><span class='glyphicon glyphicon-calendar'></span></span>" + "</div>";
            string evnt = string.IsNullOrEmpty(onChange) ? "" : ".change(" + onChange + ").on('changeDate'," + onChange + ")";
            
            str = str + "<script type=\"text/javascript\"> $(function () { $('#" + datepickerid + "').datetimepicker({format: 'DD/MM/YYYY', useCurrent: false,pickTime: false})" + evnt + "; });</script>";
            MvcHtmlString result = new MvcHtmlString(str);
            return result;
        }

    }
}