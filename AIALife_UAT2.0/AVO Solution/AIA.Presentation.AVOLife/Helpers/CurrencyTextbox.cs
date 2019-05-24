using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace AIA.Presentation.Helpers
{
    public static class CurrencyTextbox
    {

        public static MvcHtmlString CurrencyTextBox<tmodel, tpropoerty>(this HtmlHelper<tmodel> htmlHelper, Expression<Func<tmodel, tpropoerty>> expression, string disabled = "enabled")
        {
            var exptext = ExpressionHelper.GetExpressionText(expression);
            var value = htmlHelper.ViewData.Eval(exptext);

            string str = "<input type='text' id='currency" + exptext + "' onkeypress='return isNumberKey(this)' style='text-align:right;'  class='rupee' onblur='formatCurrency(this)' value='" + value + "'  disabled='" + disabled + "' >";
            MvcHtmlString strControl = new MvcHtmlString(str);

            return strControl;

        }
        public static MvcHtmlString CurrencyTextBoxGrid(this HtmlHelper htmlHelper, string Value, string id)
        {
            var exptext = id;//ExpressionHelper.GetExpressionText(expression);
                             // var value = htmlHelper.ViewData.Eval(exptext);
            if (Value == null)
            {
                Value = "0.00";
            }
            string str = "<input type='text' id='currency" + exptext + "' onkeypress='return isNumberKey(this)' style='text-align:right;border:none;background-color:transparent;box-shadow: inset 0px 1px 1px rgba(0,0,0,0);'  class='form-control rupee' onblur='FormatCurrency(this)' value=" + Value + " disabled='disabled' >";
            MvcHtmlString strControl = new MvcHtmlString(str);

            return strControl;

        }
        public static MvcHtmlString CurrencyTextBoxGrid1(this HtmlHelper htmlHelper, string Value, string id)
        {
            var exptext = id;//ExpressionHelper.GetExpressionText(expression);
                             // var value = htmlHelper.ViewData.Eval(exptext);
            if (Value == null || Value == "")
            {
                Value = "0.00";
            }
            string str = "<input type='text' id='currency" + exptext + "' onkeypress='return isNumberKey(this)' style='text-align:left;border:none;background-color:transparent;box-shadow: inset 0px 1px 1px rgba(0,0,0,0);'  class='form-control rupee' onblur='FormatCurrency(this)' value=" + Value + "" + " disabled='disabled' >";
            MvcHtmlString strControl = new MvcHtmlString(str);

            return strControl;

        }
        public static MvcHtmlString CurrencyTextBox1<tmodel, tpropoerty>(this HtmlHelper<tmodel> htmlHelper, Expression<Func<tmodel, tpropoerty>> expression, string disabled = "enabled")
        {
            var exptext = ExpressionHelper.GetExpressionText(expression);
            var value = htmlHelper.ViewData.Eval(exptext);

            string str = "<input type='text' id='currency" + exptext + "' onkeypress='return isNumberKey(this)' style='text-align:left;'  class='rupee form-control' onblur='formatCurrency(this)' value='" + value + "'  disabled='" + disabled + "' >";
            MvcHtmlString strControl = new MvcHtmlString(str);

            return strControl;

        }
    }
}