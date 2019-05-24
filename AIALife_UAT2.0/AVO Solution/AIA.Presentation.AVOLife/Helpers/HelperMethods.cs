
using AIA.Life.Repository.AIAEntity;
using AIA.Life.Models.Common;
using AIAResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using AIA.Presentation.AVOLife.Controllers;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using AIA.Life.Business.AgentonBoarding;

namespace AIA.Presentation.Helpers
{
    public static class HelperMethods
    {

        //it is for creating leftmenu based on list of menus returned from the database
        public static MvcHtmlString iNubeLeftMenu<TModel>(this HtmlHelper<TModel> html, string appName, string userName)
        {

            var product = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
            AVOAIALifeEntities Context = new AVOAIALifeEntities();

            IEnumerable<Menu> lstMenuTypes;


            List<USP_GetMenuHierarchy_Result> data = Context.USP_GetMenuHierarchy(appName, userName).ToList();
            lstMenuTypes = data.Select(o => new Menu { MenuId = o.MenuId, MenuType = o.MenuType, LIcon = o.LIcon, IconType = o.IconType, Action = o.Action, DisplayName = o.DisplayName, ParentId = o.ParentId }).AsEnumerable();

            var str = string.Empty;
            var subString = string.Empty;
            // var finalResult = string.Empty;
            //string CultureInfo = Helpers.HelperMethods.GetCulture();
            //removed for menus to make english added by praveen
            string CultureInfo = "en-IN";
            foreach (Menu obj in lstMenuTypes.Where(o => o.ParentId == decimal.Zero))
            {
                var iconPath = string.Empty;
                if (obj.IconType == "ICON")
                {
                    iconPath = "<i class='" + obj.LIcon + " pull-left' ></i>";
                    // iconPath = "<i class='" + obj.LIcon + "'></i>";
                }
                else
                {

                    obj.LIcon = obj.LIcon.Replace("../..//", "~/");
                    obj.LIcon = obj.LIcon.Replace("../../", "~/");
                    var take = VirtualPathUtility.ToAbsolute(obj.LIcon);
                    if (!string.IsNullOrEmpty(take))
                        take = take.Replace("iNube", product);
                    iconPath = "<img src=" + take + " class='pull-left' style='height:25px;margin-right:10px;' />" + "<i class=''></i>";
                    //iconPath = "<img src=" + take + " height=32px />" + "<i class=''></i>";

                }

                string DisplayName = Resources.GetMenu(obj.DisplayName, CultureInfo);
                if (string.IsNullOrEmpty(DisplayName))
                {
                    DisplayName = obj.DisplayName;
                }

                subString = (str == string.Empty ? "selected" : "arrow");
                str = str + "<li>"
                    + "<a href=" + obj.Action + ">"
                    + iconPath
                    // + "<span class='title pull-left'>" + obj.DisplayName + "</span>"
                    + "<span class='title pull-left'>" + DisplayName + "</span>"

                    // + "<span class=title>" + obj.DisplayName + "</span>"
                    + "<span class=" + subString + "></span>" + "</a>" + subMenu(obj.MenuId, lstMenuTypes, string.Empty) + "</li>"
                    + "<legend style='border-bottom-color: #fff; border-bottom-width: 1px; margin-bottom: auto; border-bottom-style: solid;'></legend>";

            }
            MvcHtmlString strResult = new MvcHtmlString(str);

            return strResult;
        }
        public static List<int?> GetPermissions(string LoginID)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
               Guid? UserId = Context.tblUserDetails.Where(a => a.LoginID == LoginID).Select(a => a.UserID).FirstOrDefault();

                List<int?> lstPermissionId = Context.tblUserPermissions.Where(a => a.UserID == UserId).Select(a => a.PermissionId).ToList();
            
                return lstPermissionId;          


        }
        public static string subMenu(decimal? MenuID, IEnumerable<Menu> lstMenuTypes, string result)
        {
            // string CultureInfo = Helpers.HelperMethods.GetCulture();
            //removed for menus to make english added by praveen
            string CultureInfo = "en-IN";
             var isGrand = false;
            // string temp = string.Empty;
            var lstData = lstMenuTypes.Where(o => o.ParentId == MenuID);
            //var SubTemp = string.Empty;
            var product = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
            if (lstData.Count() > 0)
            {
                isGrand = true;
                result = result + "<ul class=sub-menu>";
            }
            foreach (var data in lstData)
            {
                var iconPath = string.Empty;
                if (data.IconType == "ICON")
                {
                    iconPath = "<i class='" + data.LIcon + "'></i>";
                }
                else
                {
                    if (!string.IsNullOrEmpty(data.LIcon))
                        data.LIcon = data.LIcon.Replace("iNube", product);
                    iconPath = "<img src=" + data.LIcon + " height=22px />" + "<i class=''></i>";

                }
                string DisplayName = Resources.GetMenu(data.DisplayName, CultureInfo);
                if (string.IsNullOrEmpty(DisplayName))
                {
                    DisplayName = data.DisplayName;
                }
                result = result + " <li>" + "<a href=" + data.Action + ">" + iconPath
               + DisplayName + "</a>";

                result = subMenu(data.MenuId, lstMenuTypes, result);


                result = result + "</li>";

            }


            if (isGrand)
            {
                result = result + "</ul>";
            }
            return result;
        }

        public static object GetPropertyValue(this object car, string propertyName)
        {
            try
            {
                return car.GetType().GetProperties().Single(pi => pi.Name == propertyName).GetValue(car, null);
            }
            catch { return null; }
        }

        public static MvcHtmlString AddressFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
        {
            var expText = ExpressionHelper.GetExpressionText(expression);

            var address = html.ViewData.Eval(expText) as AIA.Life.Models.Common.Address;
            var splittedExpText = expText.Split('.');
            var length = splittedExpText.Length > 1 ? splittedExpText.Length - 2 : splittedExpText.Length - 1;
            var textId = splittedExpText[length];
            if (address == null)
            {
                address = new AIA.Life.Models.Common.Address();
                address.LstPincode = new List<AIA.Life.Models.Common.MasterListItem>();
            }
            #region Call API
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            var address1 = obj.FillAddressMasterList();
            #endregion
            address.LstPincode = address1.LstPincode;
            var pincode = address.LstPincode.Select(a => a.Value);
            string autoCompleteId = textId + "addrAutoComplete";
            string fieldPrefix = html.ViewData.TemplateInfo.HtmlFieldPrefix;
            if (!string.IsNullOrEmpty(fieldPrefix))
            {
                expText = fieldPrefix + "." + expText;
            }
            MvcHtmlString mvcString = new MvcHtmlString("<script type='text/javascript'>" +
                                                 "$(document).ready(function () {$('#" + textId + "Pincode').autocomplete({" +
                                                         " source:'/Policy/GetAddress'," +
                                                         " minLength: 2," +
                                                         "change: function(event, ui) {" +
                                                         "if (ui.item) {" +
                                                         "writeMultipleMessage('error', '','" + textId + "Pincode');" +
                                                        "} else {" +
                                                         "  $('#" + textId + "Pincode').val('');" +
                                                         "writeMultipleMessage('error', 'Please select a value from list', '" + textId + "Pincode');" +
                                                          " ErrorCode = false;" +
                                                             " }" +
                                                            " }," +
                                                         "select: function (event, ui) {" +
                                                         "SelectAutoFillAddress(event, ui,\"" + textId + "\");" +
                                                         " return false;" + " }," +
                                                         " focus: function (event, ui) {$('#" + textId + "Pincode').val(ui.item.value); return false;" +
                                                         " }," + " });" + " });" +
                                                          " </script>" +
                                                            "<div class='addressControl' objectName='" + textId + "'>" +
                                                            "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                "<label  id='" + textId + "lblAddress1'>Address1<span class=\"mandatory\"> *</span></label>" +

                                                                 "<input type='text' class='form-control' value='" + address.Address1 + "' maxlength='30' id='" + textId + "Address1' name='" + expText + ".Address1' placeholder = 'Enter Text' onblur='validateTextBox1(" + textId + "Address1)'/>" +//class='textboxaslabel'onclick = 'removeFocus(this)'

                                                            "</div>" +
                                                            "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                               "<label  id='" + textId + "lblAddress2'>Address2<span class=\"mandatory\">*</span></label>" +

                                                                "<input class='form-control' type='text' class='form-control text-right' maxlength='30'  value='" + address.Address2 + "' id='" + textId + "Address2' name='" + expText + ".Address2' placeholder = 'Enter Text'/>" +//class='textboxaslabel' onclick = 'removeFocus(this)'

                                                            "</div>" +
                                                            "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                "<label  id='" + textId + "lblAddress1'>Address3<span class=\"mandatory\"></span></label>" +

                                                                 "<input type='text' class='form-control' value='" + address.Address3 + "' maxlength='30'  id='" + textId + "Address3' name='" + expText + ".Address3' placeholder = 'Enter Text'/>" +

                                                            "</div>" +
                                                            "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4' >" +
                                                                     "<label  id='" + textId + "lblArea'>Pincode<span class=\"mandatory\"> *</span></label>" +

                                                                     "<input type='text' class='form-control' value='" + address.Pincode + "' id='" + textId + "Pincode' name='" + expText + ".Pincode' onkeypress = 'return isNumberKey(event)' placeholder = 'Enter Text' onblur='validateTextBox1(" + textId + "PincodeNew)'/>" +

                                                        "</div>" +
                                                         "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                    "<label  id='" + textId + "lblCity'>City<span class=\"mandatory\"> *</span></label>" +
                                                                    "<div  id='divCity'>" +
                                                                    "<input type='text' class='form-control' value='" + address.City + "' id='" + textId + "City' name='" + expText + ".City' placeholder = 'Enter Text'/>" +
                                                                    "</div>" +
                                                                "</div>" +
                                                                 "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                    "<label  id='" + textId + "lblddlCity'>District<span class=\"mandatory\"> *</span></label>" +
                                                                    "<div  id='divDistrict'>" +
                                                                    "<input type='text' class='form-control' value='" + address.District + "' id='" + textId + "District' name='" + expText + ".District' placeholder = 'Enter Text'/>" +
                                                                    "</div>" +
                                                                "</div>" +
                                                            "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                "<label  id='" + textId + "lblState'>State<span class=\"mandatory\"> *</span></label>" +
                                                                "<div id='divState'>" +
                                                                "<input type='text' class='form-control' value='" + address.State + "' id='" + textId + "State' name='" + expText + ".State' placeholder = 'Enter Text' />" +

                                                              "</div>" +
                                                            "</div>" +

                                                                "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                     "<label  id='" + textId + "lblCountry'>Country<span class=\"mandatory\"> *</span></label>" +
                                                                     "<div id='divCountry'>" +
                                                                     "<input type='text' class='form-control' value='" + address.Country + "' id='" + textId + "Country' name='" + expText + ".Country' placeholder = 'Enter Text'/>" +
                                                                        "</div>" +
                                                        "</div>" + "</div>");
            return mvcString;
        }

        public static MvcHtmlString AddressForJS<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
        {
            var expText = ExpressionHelper.GetExpressionText(expression);

            var address = html.ViewData.Eval(expText) as AIA.Life.Models.Common.Address;
            var splittedExpText = expText.Split('.');
            var length = splittedExpText.Length > 1 ? splittedExpText.Length - 2 : splittedExpText.Length - 1;
            var textId = splittedExpText[length];
            if (address == null)
            {
                address = new AIA.Life.Models.Common.Address();
                address.LstPincode = new List<AIA.Life.Models.Common.MasterListItem>();
            }
            #region Call API
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            var address1 = obj.FillAddressMasterList();
            #endregion
            address.LstPincode = address1.LstPincode;
            var pincode = address.LstPincode.Select(a => a.Value);
            string autoCompleteId = textId + "addrAutoComplete";
            string fieldPrefix = html.ViewData.TemplateInfo.HtmlFieldPrefix;
            if (!string.IsNullOrEmpty(fieldPrefix))
            {
                expText = fieldPrefix + "." + expText;
            }
            MvcHtmlString mvcString = new MvcHtmlString("<script type='text/javascript'>" +
                                                 "$(document).ready(function () {$('#" + textId + "Pincode').autocomplete({" +
                                                         " source:'/Policy/GetAddress'," +
                                                         " minLength: 2," +
                                                         "change: function(event, ui) {" +
                                                         "if (ui.item) {" +
                                                         "writeMultipleMessage('error', '','" + textId + "Pincode');" +
                                                        "} else {" +
                                                         "  $('#" + textId + "Pincode').val('');" +
                                                         "writeMultipleMessage('error', 'Please select a value from list', '" + textId + "Pincode');" +
                                                          " ErrorCode = false;" +
                                                             " }" +
                                                            " }," +
                                                         "select: function (event, ui) {" +
                                                         "SelectAutoFillAddress(event, ui,\"" + textId + "\");" +
                                                         " return false;" + " }," +
                                                         " focus: function (event, ui) {$('#" + textId + "Pincode').val(ui.item.value); return false;" +
                                                         " }," + " });" + " });" +
                                                          " </script>" +
                                                            "<div class='addressControl' objectName='" + textId + "'>" +
                                                            "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                "<label  id='" + textId + "lblAddress1'>Address1<span class=\"mandatory\"> *</span></label>" +

                                                                 "<input type='text' class='form-control' value='" + address.Address1 + "' maxlength='30' id='" + textId + "Address1' name='" + expText + ".Address1' placeholder = 'Enter Text' onblur='validateTextBox1(" + textId + "Address1)'/>" +//class='textboxaslabel'onclick = 'removeFocus(this)'

                                                            "</div>" +
                                                            "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                "<label  id='" + textId + "lblAddress2'>Address2<span class=\"mandatory\"></span></label>" +

                                                                "<input class='form-control' type='text' class='form-control text-right' maxlength='30'  value='" + address.Address2 + "' id='" + textId + "Address2' name='" + expText + ".Address2' placeholder = 'Enter Text'/>" +//class='textboxaslabel' onclick = 'removeFocus(this)'

                                                            "</div>" +
                                                            "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                "<label  id='" + textId + "lblAddress1'>Address3<span class=\"mandatory\"></span></label>" +

                                                                 "<input type='text' class='form-control' value='" + address.Address3 + "' maxlength='30'  id='" + textId + "Address3' name='" + expText + ".Address3' placeholder = 'Enter Text'/>" +

                                                            "</div>" +
                                                            "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4' >" +
                                                                     "<label  id='" + textId + "lblArea'>Pincode<span class=\"mandatory\"> *</span></label>" +

                                                                     "<input type='text' class='form-control' value='" + address.Pincode + "' id='" + textId + "Pincode' name='" + expText + ".Pincode' onkeypress = 'return isNumberKey(event)' placeholder = 'Enter Text' onblur='validateTextBox1(" + textId + "PincodeNew)'/>" +

                                                        "</div>" +
                                                         "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                    "<label  id='" + textId + "lblCity'>City<span class=\"mandatory\"> *</span></label>" +
                                                                    "<div  id='divCity'>" +
                                                                    "<input type='text' class='form-control' value='" + address.City + "' id='" + textId + "City' name='" + expText + ".City' placeholder = 'Enter Text'/>" +
                                                                    "</div>" +
                                                                "</div>" +
                                                                 "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                    "<label  id='" + textId + "lblddlCity'>District<span class=\"mandatory\"> *</span></label>" +
                                                                    "<div  id='divDistrict'>" +
                                                                    "<input type='text' class='form-control' value='" + address.District + "' id='" + textId + "District' name='" + expText + ".District' placeholder = 'Enter Text'/>" +
                                                                    "</div>" +
                                                                "</div>" +
                                                            "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                "<label  id='" + textId + "lblState'>State<span class=\"mandatory\"> *</span></label>" +
                                                                "<div id='divState'>" +
                                                                "<input type='text' class='form-control' value='" + address.State + "' id='" + textId + "State' name='" + expText + ".State' placeholder = 'Enter Text' />" +

                                                              "</div>" +
                                                            "</div>" +

                                                                "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                     "<label  id='" + textId + "lblCountry'>Country<span class=\"mandatory\"> *</span></label>" +
                                                                     "<div id='divCountry'>" +
                                                                     "<input type='text' class='form-control' value='" + address.Country + "' id='" + textId + "Country' name='" + expText + ".Country' placeholder = 'Enter Text'/>" +
                                                                        "</div>" +
                                                        "</div>" + "</div>");
            return mvcString;
        }

        public static MvcHtmlString AddressForNew<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
        {
            var expText = ExpressionHelper.GetExpressionText(expression);
            var address = html.ViewData.Eval(expText) as AIA.Life.Models.Common.Address;
            var recruitmentAgent = html.ViewData.Eval(expText) as AIA.Life.Models.AgentonBoarding.RecruitmentAgent;
            var Province = html.ViewData.Eval(expText) as AIA.Life.Models.AgentonBoarding.RecruitmentAgent;
            var District = html.ViewData.Eval(expText) as AIA.Life.Models.AgentonBoarding.RecruitmentAgent;
            var City = html.ViewData.Eval(expText) as AIA.Life.Models.AgentonBoarding.RecruitmentAgent;

            var splittedExpText = expText.Split('.');
            var length = splittedExpText.Length > 1 ? splittedExpText.Length - 2 : splittedExpText.Length - 1;
            //var textId = splittedExpText[length];
            string textId = string.Empty;
            foreach (var item in splittedExpText)
            {
                textId = textId + item;
            }
            if (address == null)
            {
                address = new AIA.Life.Models.Common.Address();
                address.LstPincode = new List<AIA.Life.Models.Common.MasterListItem>();
            }
            if (recruitmentAgent == null)
            {
                recruitmentAgent = new AIA.Life.Models.AgentonBoarding.RecruitmentAgent();
                recruitmentAgent.LstCareerLevel = new List<AIA.Life.Models.Common.MasterListItem>();
            }
            if (Province == null)
            {
                Province = new AIA.Life.Models.AgentonBoarding.RecruitmentAgent();
                Province.LstProvince = new List<Address>();
            }
            #region Call API
            AgentonBoardingBusiness objAgentonBoardingLogic = new AgentonBoardingBusiness();
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            var address1 = obj.FillAddressMasterList();
            var recruitmentAgent1 = objAgentonBoardingLogic.LoadAgentOnBoardingRecruitmentData();
            var province1 = objAgentonBoardingLogic.GetProvinceMaster();
            #endregion
            address.LstPincode = address1.LstPincode;
            recruitmentAgent.LstCareerLevel = recruitmentAgent1.LstCareerLevel;
            Province.LstProvince = province1.LstProvince;
            var pincode = address.LstPincode.Select(a => a.Value);
            //string autoCompleteId = textId + "addrAutoComplete";
            string fieldPrefix = html.ViewData.TemplateInfo.HtmlFieldPrefix;
            if (!string.IsNullOrEmpty(fieldPrefix))
            {
                expText = fieldPrefix + "." + expText;
            }

            StringBuilder optionProvince = new StringBuilder();
            StringBuilder optionDistrict = new StringBuilder();
            StringBuilder optionCity = new StringBuilder();
            foreach (var item in (List<Address>)Province.LstProvince)
            {
                // item.Province.Insert(0, "Select");
                optionProvince = optionProvince.Append("<option value='" + item.ProvinceCode + "'>" + item.Province + "</option>");

            }
            optionProvince.Insert(0, "<option value='\'>Select</option>");
            MvcHtmlString mvcString = new MvcHtmlString(
                                                            //"<script type='text/javascript'>" +
                                                            //                                 "$(document).ready(function () {$('#" + textId + "Pincode').autocomplete({" +
                                                            //                                         " source:'../../Policy/GetAddress'," +
                                                            //                                         " minLength: 2," +
                                                            //                                         "change: function(event, ui) {" +
                                                            //                                         "if (ui.item) {" +
                                                            //                                         "writeMultipleMessage('error', '','" + textId + "Pincode');" +
                                                            //                                        "} else {" +
                                                            //                                         "  $('#" + textId + "Pincode').val('');" +
                                                            //                                         "writeMultipleMessage('error', 'Please select a value from list', '" + textId + "Pincode');" +
                                                            //                                         " ErrorCode = false;" +
                                                            //                                             " }" +
                                                            //                                            " }," +
                                                            //                                         "select: function (event, ui) {" +
                                                            //                                         "SelectAutoFillAddress(event, ui,\"" + textId + "\");" +
                                                            //                                         " return false;" + " }," +
                                                            //                                         " focus: function (event, ui) {$('#" + textId + "Pincode').val(ui.item.value); return false;" +
                                                            //                                         " }," + " });" + " });" +
                                                            //                                          " </script>" +
                                                            "<div class='addressControl' objectName='" + textId + "'>" +
                                                            "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                "<label  id='" + textId + "lblAddress1'>Address1<span class=\"mandatory\">*</span></label>" +
                                                                 "<input type='text' class='form-control' value='" + address.Address1 + "' maxlength='30' id='" + textId + "Address1' name='" + expText + ".Address1' placeholder = 'Enter Text' onchange='ClearErrOnChange(), RemoveRegAddSame(" + textId + "Address1)'/>" +//class='textboxaslabel'onclick = 'removeFocus(this)'
                                                            "</div>" +
                                                            "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                "<label  id='" + textId + "lblAddress2'>Address2<span class=\"mandatory\">*</span></label>" +
                                                                "<input class='form-control' type='text' class='form-control text-right' maxlength='30'  value='" + address.Address2 + "' id='" + textId + "Address2' name='" + expText + ".Address2' placeholder = 'Enter Text' onchange='ClearErrOnChange(), RemoveRegAddSame(" + textId + "Address2)'/>" +//class='textboxaslabel' onclick = 'removeFocus(this)'
                                                            "</div>" +
                                                            "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                "<label  id='" + textId + "lblProvince'>Province<span class=\"mandatory\">*</span></label>" +
                                                                "<select id='" + textId + "ddlProvince' name='" + expText + ".State' class='form-control' onchange='FillDistrictCommAddress(" + textId + "ddlProvince)'>'" + optionProvince.ToString() + "'</select>" +
                                                            "</div>" +
                                                            "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                    "<label  id='" + textId + "lblddlDistrict'>District<span class=\"mandatory\">*</span></label>" +
                                                                    "<div  id='divDistrict'>" +
                                                                    "<select id='" + textId + "ddlDistrict' name='" + expText + ".District' class='form-control' onchange='FillCityCommAddress(" + textId + "ddlDistrict)'>'" + optionDistrict.ToString() + "'</select>" +
                                                                    "</div>" +
                                                            "</div>" +
                                                            "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                    "<label  id='" + textId + "lblCity'>City/Town<span class=\"mandatory\">*</span></label>" +
                                                                    "<div  id='divCity'>" +
                                                                    "<select id='" + textId + "ddlCity' name='" + expText + ".City' class='form-control' onchange='FillPostalCodeCommAddress(" + textId + "ddlCity)'>'" + optionCity.ToString() + "'</select>" +
                                                                    "</div>" +
                                                             "</div>" +
                                                             "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                    "<label  id='" + textId + "lblPincode'>Postal Code<span class=\"mandatory\">*</span></label>" +
                                                                    "<div  id='divPincode'>" +
                                                                    "<input type='text' class='form-control' value='" + address.Pincode + "' id='" + textId + "Pincode' name='" + expText + ".Pincode' placeholder = 'Enter Text'/>" +
                                                                    "</div>" +
                                                             "</div>" +
                                                        "</div>");
            return mvcString;
        }

        public static MvcHtmlString AddressForNewProspectScreen<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
        {
            var expText = ExpressionHelper.GetExpressionText(expression);
            var address = html.ViewData.Eval(expText) as AIA.Life.Models.Common.Address;
            var recruitmentAgent = html.ViewData.Eval(expText) as AIA.Life.Models.AgentonBoarding.RecruitmentAgent;
            var Province = html.ViewData.Eval(expText) as AIA.Life.Models.AgentonBoarding.RecruitmentAgent;
            var District = html.ViewData.Eval(expText) as AIA.Life.Models.AgentonBoarding.RecruitmentAgent;
            var City = html.ViewData.Eval(expText) as AIA.Life.Models.AgentonBoarding.RecruitmentAgent;

            var splittedExpText = expText.Split('.');
            var length = splittedExpText.Length > 1 ? splittedExpText.Length - 2 : splittedExpText.Length - 1;
            //var textId = splittedExpText[length];
            string textId = string.Empty;
            foreach (var item in splittedExpText)
            {
                textId = textId + item;
            }

            if (address == null)
            {
                address = new AIA.Life.Models.Common.Address();
                address.LstPincode = new List<AIA.Life.Models.Common.MasterListItem>();
            }
            if (recruitmentAgent == null)
            {
                recruitmentAgent = new AIA.Life.Models.AgentonBoarding.RecruitmentAgent();
                recruitmentAgent.LstCareerLevel = new List<AIA.Life.Models.Common.MasterListItem>();
            }
            if (Province == null)
            {
                Province = new AIA.Life.Models.AgentonBoarding.RecruitmentAgent();
                Province.LstProvince = new List<Address>();
            }
            #region Call API
            AgentonBoardingBusiness objAgentonBoardingLogic = new AgentonBoardingBusiness();
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            var address1 = obj.FillAddressMasterList();
            var recruitmentAgent1 = objAgentonBoardingLogic.LoadAgentOnBoardingRecruitmentData();
            var province1 = objAgentonBoardingLogic.GetProvinceMaster();
            #endregion
            address.LstPincode = address1.LstPincode;
            recruitmentAgent.LstCareerLevel = recruitmentAgent1.LstCareerLevel;
            Province.LstProvince = province1.LstProvince;
            var pincode = address.LstPincode.Select(a => a.Value);
            string autoCompleteId = textId + "addrAutoComplete";
            string fieldPrefix = html.ViewData.TemplateInfo.HtmlFieldPrefix;
            if (!string.IsNullOrEmpty(fieldPrefix))
            {
                expText = fieldPrefix + "." + expText;
            }

            StringBuilder optionProvince = new StringBuilder();
            StringBuilder optionDistrict = new StringBuilder();
            StringBuilder optionCity = new StringBuilder();
            //foreach (var item in (IEnumerable<MasterListItem>)recruitmentAgent.LstCareerLevel)
            //{
            //    options = options.Append("<option value='" + item.ID + "'>" + item.Value + "</option>");
            //}
            // optionProvince.Append("<option value=\'\'>Select</option>");
            foreach (var item in (List<Address>)Province.LstProvince)
            {

                optionProvince = optionProvince.Append("<option value='" + item.ProvinceCode + "'>" + item.Province + "</option>");

            }

            MvcHtmlString mvcString = new MvcHtmlString("<script type='text/javascript'>" +
                                                 "$(document).ready(function () {$('#" + textId + "Pincode').autocomplete({" +
                                                         " source:'../../Policy/GetAddress'," +
                                                         " minLength: 1," +
                                                         "change: function(event, ui) {" +
                                                         "if (ui.item) {" +
                                                         "writeMultipleMessage('error', '','" + textId + "Pincode');" +
                                                        "} else {" +
                                                         "  $('#" + textId + "Pincode').val('');" +
                                                         "writeMultipleMessage('error', 'Please select a value from list', '" + textId + "Pincode');" +
                                                         " ErrorCode = false;" +
                                                             " }" +
                                                            " }," +
                                                         "select: function (event, ui) {" +
                                                         /*"SelectAutoFillAddress(event, ui,\"" + textId + "\");"*/"FillPostalCodeCommAddress('" + textId + "Pincode');" +
                                                         " return false;" + " }," +
                                                         " focus: function (event, ui) {$('#" + textId + "Pincode').val(ui.item.value); return false;" +
                                                         " }," + " });" + " });" +
                                                          " </script>" +
                                                            "<div class='addressControl' objectName='" + textId + "'>" +
                                                            " <div class='col-md-4 col-sm-4 col-xs-12 col-lg-4'><div class='form-group'>" +
                                                                "<label  id='" + textId + "lblAddress1'>"+@Resources.GetLabelName("Address 1")+"<span class=\"mandatory\" id=\"mandatoryAddress1\">*</span></label>" +
                                                                 "<input type='text' class='form-control' value='" + address.Address1 + "' maxlength='30' id='" + textId + "Address1' name='" + expText + ".Address1' placeholder = 'Enter Text'/>" +//class='textboxaslabel'onclick = 'removeFocus(this)'
                                                            "</div></div>" +
                                                            " <div class='col-md-4 col-sm-4 col-xs-12 col-lg-4'><div class='form-group'>" +
                                                                "<label  id='" + textId + "lblAddress2'>"+ @Resources.GetLabelName("Address 2") + "<span class=\"mandatory\" id=\"mandatoryAddress2\">*</span></label>" +
                                                                "<input class='form-control' type='text' class='form-control text-right' maxlength='30'  value='" + address.Address2 + "' id='" + textId + "Address2' name='" + expText + ".Address2' placeholder = 'Enter Text' />" +//class='textboxaslabel' onclick = 'removeFocus(this)'
                                                            "</div></div>" +
                                                               " <div class='col-md-4 col-sm-4 col-xs-12 col-lg-4'><div class='form-group'>" +
                                                                "<label  id='" + textId + "lblAddress3'>"+ @Resources.GetLabelName("Address 3") + "</label>" +
                                                                "<input class='form-control' type='text' class='form-control text-right' maxlength='30'  value='" + address.Address3 + "' id='" + textId + "Address3' name='" + expText + ".Address3' placeholder = 'Enter Text' />" +//class='textboxaslabel' onclick = 'removeFocus(this)'
                                                            "</div></div>" +
                                                            "<div class='col-md-4 col-sm-4 col-xs-12 col-lg-4'><div class='form-group'>" +
                                                                    "<label  id='" + textId + "lblPincode'>"+ @Resources.GetLabelName("Postal Code|City") + "<span class=\"mandatory\" id=\"mandatorylblPincode\">*</span></label>" +
                                                                    "<div  id='divPincode'>" +
                                                                    "<input type='text' class='form-control' value='" + address.Pincode + "' id='" + textId + "Pincode' name='" + expText + ".Pincode'  placeholder = 'Enter Postal Code|City'/>" +
                                                                    "</div>" +
                                                             "</div></div>" +
                                                              "<div class='col-md-4 col-sm-4 col-xs-12 col-lg-4'><div class='form-group'>" +
                                                                    "<label  id='" + textId + "lblddlDistrict'>"+ @Resources.GetLabelName("District") + "<span class=\"mandatory\" id=\"mandatorylblddlDistrict\">*</span></label>" +
                                                                    "<div  id='divDistrict'>" +
                                                                     "<input type='text' class='form-control' value='" + address.District + "' id='" + textId + "ddlDistrict' name='" + expText + ".District' readonly='readonly' />" + //readonly='readonly'
                                                                                                                                                                                                                    //"<select id='" + textId + "ddlDistrict' name='" + expText + ".District' class='form-control' onchange='FillCityCommAddress(" + textId + "ddlDistrict)'>'" + optionDistrict.ToString() + "'</select>" +
                                                                    "</div>" +
                                                            "</div></div>" +
                                                            "<div class='col-md-4 col-sm-4 col-xs-12 col-lg-4'><div class='form-group'>" +
                                                                "<label  id='" + textId + "lblProvince'>"+ @Resources.GetLabelName("Province") + "<span class=\"mandatory\" id=\"mandatorylblProvince\">*</span></label>" +
                                                                 "<input type='text' class='form-control' value='" + address.State + "' id='" + textId + "ddlProvince' name='" + expText + ".State' readonly='readonly' />" + //readonly='readonly'
                                                                                                                                                                                                          //"<select id='" + textId + "ddlProvince' name='" + expText + ".State' class='form-control' onchange='FillDistrictCommAddress(" + textId + "ddlProvince)'><option disabled selected value>Select</option>" + optionProvince.ToString() + "</select>" +
                                                            "</div></div>" +
                                                           
                                                            //"<div class='col-md-4 col-sm-4 col-xs-12 col-lg-4'><div class='form-group'>" +
                                                            //        "<label  id='" + textId + "lblCity'>City/Town<span class=\"mandatory\">*</span></label>" +
                                                            //        "<div  id='divCity'>" +
                                                            //        "<select id='" + textId + "ddlCity' name='" + expText + ".City' class='form-control' onchange='FillPostalCodeCommAddress(" + textId + "ddlCity)'>'" + optionCity.ToString() + "'</select>" +
                                                            //        "</div>" +
                                                            // "</div></div>" +
                                                             
                                                        "</div>");
            return mvcString;
        }
        public static MvcHtmlString AddressForReference<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
        {
            var expText = ExpressionHelper.GetExpressionText(expression);
            var address = html.ViewData.Eval(expText) as AIA.Life.Models.Common.Address;
            var recruitmentAgent = html.ViewData.Eval(expText) as AIA.Life.Models.AgentonBoarding.RecruitmentAgent;
            var Province = html.ViewData.Eval(expText) as AIA.Life.Models.AgentonBoarding.RecruitmentAgent;
            var District = html.ViewData.Eval(expText) as AIA.Life.Models.AgentonBoarding.RecruitmentAgent;
            var City = html.ViewData.Eval(expText) as AIA.Life.Models.AgentonBoarding.RecruitmentAgent;

            var splittedExpText = expText.Split('.');
            var length = splittedExpText.Length > 1 ? splittedExpText.Length - 2 : splittedExpText.Length - 1;
            //var textId = splittedExpText[length];
            string textId = string.Empty;
            foreach (var item in splittedExpText)
            {
                textId = textId + item;
            }

            if (address == null)
            {
                address = new AIA.Life.Models.Common.Address();
                address.LstPincode = new List<AIA.Life.Models.Common.MasterListItem>();
            }
            if (recruitmentAgent == null)
            {
                recruitmentAgent = new AIA.Life.Models.AgentonBoarding.RecruitmentAgent();
                recruitmentAgent.LstCareerLevel = new List<AIA.Life.Models.Common.MasterListItem>();
            }
            if (Province == null)
            {
                Province = new AIA.Life.Models.AgentonBoarding.RecruitmentAgent();
                Province.LstProvince = new List<Address>();
            }
            #region Call API
            AgentonBoardingBusiness objAgentonBoardingLogic = new AgentonBoardingBusiness();
            AIA.Life.Business.Policy.PolicyBusiness obj = new AIA.Life.Business.Policy.PolicyBusiness();
            var address1 = obj.FillAddressMasterList();
            var recruitmentAgent1 = objAgentonBoardingLogic.LoadAgentOnBoardingRecruitmentData();
            var province1 = objAgentonBoardingLogic.GetProvinceMaster();
            #endregion
            address.LstPincode = address1.LstPincode;
            recruitmentAgent.LstCareerLevel = recruitmentAgent1.LstCareerLevel;
            Province.LstProvince = province1.LstProvince;
            var pincode = address.LstPincode.Select(a => a.Value);
            //string autoCompleteId = textId + "addrAutoComplete";
            string fieldPrefix = html.ViewData.TemplateInfo.HtmlFieldPrefix;
            if (!string.IsNullOrEmpty(fieldPrefix))
            {
                expText = fieldPrefix + "." + expText;
            }

            StringBuilder optionProvince = new StringBuilder();
            StringBuilder optionDistrict = new StringBuilder();
            StringBuilder optionCity = new StringBuilder();
            foreach (var item in (List<Address>)Province.LstProvince)
            {
                optionProvince = optionProvince.Append("<option value='" + item.ProvinceCode + "'>" + item.Province + "</option>");
            }

            MvcHtmlString mvcString = new MvcHtmlString(
                                                            //"<script type='text/javascript'>" +
                                                            //                                 "$(document).ready(function () {$('#" + textId + "Pincode').autocomplete({" +
                                                            //                                         " source:'../../Policy/GetAddress'," +
                                                            //                                         " minLength: 2," +
                                                            //                                         "change: function(event, ui) {" +
                                                            //                                         "if (ui.item) {" +
                                                            //                                         "writeMultipleMessage('error', '','" + textId + "Pincode');" +
                                                            //                                        "} else {" +
                                                            //                                         "  $('#" + textId + "Pincode').val('');" +
                                                            //                                         "writeMultipleMessage('error', 'Please select a value from list', '" + textId + "Pincode');" +
                                                            //                                         " ErrorCode = false;" +
                                                            //                                             " }" +
                                                            //                                            " }," +
                                                            //                                         "select: function (event, ui) {" +
                                                            //                                         "SelectAutoFillAddress(event, ui,\"" + textId + "\");" +
                                                            //                                         " return false;" + " }," +
                                                            //                                         " focus: function (event, ui) {$('#" + textId + "Pincode').val(ui.item.value); return false;" +
                                                            //                                         " }," + " });" + " });" +
                                                            //                                          " </script>" +
                                                            "<div class='addressControl' objectName='" + textId + "'>" +
                                                            "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                "<label  id='" + textId + "lblAddress1'>Address1</label>" +
                                                                 "<input type='text' class='form-control' value='" + address.Address1 + "' maxlength='30' id='" + textId + "Address1' name='" + expText + ".Address1' placeholder = 'Enter Text' onblur='validateTextBox1(" + textId + "Address1)'/>" +//class='textboxaslabel'onclick = 'removeFocus(this)'
                                                            "</div>" +
                                                            "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                "<label  id='" + textId + "lblAddress2'>Address2</label>" +
                                                                "<input class='form-control' type='text' class='form-control text-right' maxlength='30'  value='" + address.Address2 + "' id='" + textId + "Address2' name='" + expText + ".Address2' placeholder = 'Enter Text'/>" +//class='textboxaslabel' onclick = 'removeFocus(this)'
                                                            "</div>" +
                                                            "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                "<label  id='" + textId + "lblProvince'>Province</label>" +
                                                                "<select id='" + textId + "ddlProvince' name='" + expText + ".State' class='form-control' onchange='FillDistrictCommAddress(" + textId + "ddlProvince)'>'" + optionProvince.ToString() + "'</select>" +
                                                            "</div>" +
                                                            "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                    "<label  id='" + textId + "lblddlDistrict'>District</label>" +
                                                                    "<div  id='divDistrict'>" +
                                                                    "<select id='" + textId + "ddlDistrict' name='" + expText + ".District' class='form-control' onchange='FillCityCommAddress(" + textId + "ddlDistrict)'>'" + optionDistrict.ToString() + "'</select>" +
                                                                    "</div>" +
                                                            "</div>" +
                                                            "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                    "<label  id='" + textId + "lblCity'>City/Town</label>" +
                                                                    "<div  id='divCity'>" +
                                                                    "<select id='" + textId + "ddlCity' name='" + expText + ".City' class='form-control' onchange='FillPostalCodeCommAddress(" + textId + "ddlCity)'>'" + optionCity.ToString() + "'</select>" +
                                                                    "</div>" +
                                                             "</div>" +
                                                             "<div class='form-group col-md-4 col-sm-6 col-xs-12 col-lg-4'>" +
                                                                    "<label  id='" + textId + "lblPincode'>Zip/Pin Code</label>" +
                                                                    "<div  id='divPincode'>" +
                                                                    "<input type='text' class='form-control' value='" + address.Pincode + "' id='" + textId + "Pincode' name='" + expText + ".Pincode' placeholder = 'Enter Text'/>" +
                                                                    "</div>" +
                                                             "</div>" +
                                                        "</div>");
            return mvcString;
        }
        public static string GetCulture()
        {
            string CultureName = "";
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.User != null)
                {
                    try
                    {
                        HttpCookie CultureCookie = new HttpCookie("_culture");
                        CultureCookie = HttpContext.Current.Request.Cookies["_culture"];
                        if (CultureCookie != null)
                        {
                            if (CultureCookie != null)
                            {
                                CultureName = CultureCookie.Value.ToString();

                            }
                        }

                        return CultureName != null ? CultureName : "";
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            if (string.IsNullOrEmpty(CultureName))
            {
                return string.Empty;
            }
            return CultureName;

        }
        public static MvcHtmlString iNubeDashboardFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string UserName, string Type)
        {
            var str = string.Empty;
            PolicyController objPolicyLogic = new PolicyController();
            string RoleId = GetRoleId();
            Type = (Type == null) ? GetApplicationName() : Type;
            //str = objPolicyLogic.BuildingDashboard(UserName, Type, RoleId);
            MvcHtmlString strResult = new MvcHtmlString(str);

            return strResult;
        }
        public static MvcHtmlString CurrencyTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, object value = null)
        {
            var expText = ExpressionHelper.GetExpressionText(expression);
            string onchange = string.IsNullOrEmpty(Convert.ToString(htmlAttributes.GetPropertyValue("onchange"))) == true ? "" : "onchange = '" + htmlAttributes.GetPropertyValue("onchange") + "'";
            string onfocus = string.IsNullOrEmpty(Convert.ToString(htmlAttributes.GetPropertyValue("onfocus"))) == true ? "" : "onfocus = '" + htmlAttributes.GetPropertyValue("onfocus") + "'";
            string onblur = string.IsNullOrEmpty(Convert.ToString(htmlAttributes.GetPropertyValue("onblur"))) == true ? "" : "onblur = '" + htmlAttributes.GetPropertyValue("onblur") + "'";
            MvcHtmlString mvcString = new MvcHtmlString("<input type='text' class='" + htmlAttributes.GetPropertyValue("class") + "' id='" + htmlAttributes.GetPropertyValue("id") + "' style='" + htmlAttributes.GetPropertyValue("style")+ "'" +
                " value='" + value + "' onkeypress='" + htmlAttributes.GetPropertyValue("onkeypress") + "' "+ "' onkeyup='" + htmlAttributes.GetPropertyValue("onkeyup") + "' " + onchange +   onfocus + onblur + " maxlength='" + htmlAttributes.GetPropertyValue("maxlength") + "' " + onchange + " />" +
                "<input type='hidden' id='hdn" + htmlAttributes.GetPropertyValue("id") + "' value='" + value + "' name='" + expText + "'/>" +
                "<script>" +
                "$(document).ready(function() { " +
                   "$('#" + htmlAttributes.GetPropertyValue("id") + "').val($('#hdn" + htmlAttributes.GetPropertyValue("id") + "').val().replace" +
                    @"(/\B(?=(\d{3})+(?!\d))/g, ',')"+
                    ");"+
                "});" +
                //"$('#" + htmlAttributes.GetPropertyValue("id") + "').change(function() {" +
                //    "" + htmlAttributes.GetPropertyValue("onchange") +
                //    "this.value = parseInt(this.value.replace(/,/g, '')).toString()" + @".replace(/\B(?=(\d{3})+(?!\d))/g, ',');" +
                //    "$('#hdn" + htmlAttributes.GetPropertyValue("id") + "').val(this.value.replace(/,/g, ''));"+
                //"});" +
                "</script>");
            return mvcString;
        }
        public static MvcHtmlString CurrencyLabelFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, object value = null)
        {
            var expText = ExpressionHelper.GetExpressionText(expression);
            string onchange = string.IsNullOrEmpty(Convert.ToString(htmlAttributes.GetPropertyValue("onchange"))) == true ? "" : "onchange = '" + htmlAttributes.GetPropertyValue("onchange") + "'";

            MvcHtmlString mvcString = new MvcHtmlString("<label class='" + htmlAttributes.GetPropertyValue("class") + "' id='" + htmlAttributes.GetPropertyValue("id") + "' style='" + htmlAttributes.GetPropertyValue("style") +
                "' onkeypress='" + htmlAttributes.GetPropertyValue("onkeypress") + "' " + onchange + " /></label>"+
                "<input type='hidden' id='hdn" + htmlAttributes.GetPropertyValue("id") + "' value='" + value + "' name='" + expText + "'/>" +
                "<script>" +
                "$(document).ready(function() { " +
                   "$('#" + htmlAttributes.GetPropertyValue("id") + "').text($('#hdn" + htmlAttributes.GetPropertyValue("id") + "').val().replace" +
                    @"(/\B(?=(\d{3})+(?!\d))/g, ',')" +
                    ");" +
                "});" +
                //"$('#" + htmlAttributes.GetPropertyValue("id") + "').change(function() {" +
                //    "" + htmlAttributes.GetPropertyValue("onchange") +
                //    "this.value = parseInt(this.value.replace(/,/g, '')).toString()" + @".replace(/\B(?=(\d{3})+(?!\d))/g, ',');" +
                //    "$('#hdn" + htmlAttributes.GetPropertyValue("id") + "').val(this.value.replace(/,/g, ''));"+
                //"});" +
                "</script>");
            return mvcString;
        }
        public static MvcHtmlString CurrencyLabel<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, object value = null)
        {
            var expText = ExpressionHelper.GetExpressionText(expression);
            string onchange = string.IsNullOrEmpty(Convert.ToString(htmlAttributes.GetPropertyValue("onchange"))) == true ? "" : "onchange = '" + htmlAttributes.GetPropertyValue("onchange") + "'";

            MvcHtmlString mvcString = new MvcHtmlString("<label class='" + htmlAttributes.GetPropertyValue("class") + "' id='" + htmlAttributes.GetPropertyValue("id") + "' style='" + htmlAttributes.GetPropertyValue("style") +
                "' onkeypress='" + htmlAttributes.GetPropertyValue("onkeypress") + "' " + onchange + " /></label>" +
                "<input type='hidden' id='hdn" + htmlAttributes.GetPropertyValue("id") + "' value='" + value +"'/>" +
                "<script>" +
                "$(document).ready(function() {" +
                   "$('#" + htmlAttributes.GetPropertyValue("id") + "').text($('#hdn" + htmlAttributes.GetPropertyValue("id") + "').val().replace" +
                    @"(/\B(?=(\d{3})+(?!\d))/g, ',')" +
                    ");" +
                "});" +
                //"$('#" + htmlAttributes.GetPropertyValue("id") + "').change(function() {" +
                //    "" + htmlAttributes.GetPropertyValue("onchange") +
                //    "this.value = parseInt(this.value.replace(/,/g, '')).toString()" + @".replace(/\B(?=(\d{3})+(?!\d))/g, ',');" +
                //    "$('#hdn" + htmlAttributes.GetPropertyValue("id") + "').val(this.value.replace(/,/g, ''));"+
                //"});" +
                "</script>");
            return mvcString;
        }
        public static MvcHtmlString ReadCurrencyTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, object value = null)
        {
            var expText = ExpressionHelper.GetExpressionText(expression);
            string onchange = string.IsNullOrEmpty(Convert.ToString(htmlAttributes.GetPropertyValue("onchange"))) == true ? "" : "onchange = '" + htmlAttributes.GetPropertyValue("onchange") + "'";

            MvcHtmlString mvcString = new MvcHtmlString("<input type='text' class='" + htmlAttributes.GetPropertyValue("class") + "' id='" + htmlAttributes.GetPropertyValue("id") + "' style='" + htmlAttributes.GetPropertyValue("style")+ "' readonly='" + htmlAttributes.GetPropertyValue("readonly") + "'" +
                " value='" + value + "' onkeypress='" + htmlAttributes.GetPropertyValue("onkeypress") + "' " + onchange + " />" +
                "<input type='hidden' id='hdn" + htmlAttributes.GetPropertyValue("id") + "' value='" + value + "' name='" + expText + "'/>" +
                "<script>" +
                "$(document).ready(function() {" +
                   "$('#" + htmlAttributes.GetPropertyValue("id") + "').val($('#hdn" + htmlAttributes.GetPropertyValue("id") + "').val().replace" +
                    @"(/\B(?=(\d{3})+(?!\d))/g, ',')" +
                    ");" +
                "});" +
                //"$('#" + htmlAttributes.GetPropertyValue("id") + "').change(function() {" +
                //    "" + htmlAttributes.GetPropertyValue("onchange") +
                //    "this.value = parseInt(this.value.replace(/,/g, '')).toString()" + @".replace(/\B(?=(\d{3})+(?!\d))/g, ',');" +
                //    "$('#hdn" + htmlAttributes.GetPropertyValue("id") + "').val(this.value.replace(/,/g, ''));"+
                //"});" +
                "</script>");
            return mvcString;
        }
        public static string GetRoleId()
        {
            string RoleId = null;
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    try
                    {
                        HttpCookie RoleCookie = new HttpCookie("RoleId");
                        RoleCookie = HttpContext.Current.Request.Cookies["RoleId"];
                        RoleId = RoleCookie.Value.ToString();

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return RoleId;
        }
        public static string GetApplicationName()
        {
            string AppName = null;
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    try
                    {
                        HttpCookie AppCookie = new HttpCookie("AppName");
                        AppCookie = HttpContext.Current.Request.Cookies["AppName"];
                        AppName = AppCookie.Value.ToString();

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return AppName;
        }

        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }

    public class Menu
    {
        public decimal? MenuId { get; set; }
        public string MenuType { get; set; }
        public string LIcon { get; set; }
        public string Action { get; set; }
        public string DisplayName { get; set; }
        public decimal? ParentId { get; set; }
        public string IconType { get; set; }
    }
    public class JsonResponseFactory
    {
        public static object ErrorResponse(string error)
        {
            return new { Success = false, Message = error };
        }

        public static object SuccessResponse()
        {
            return new { Success = true };
        }
        public static object SuccessResponse(string Message)
        {
            return new { Success = true, Message = Message };
        }

        public static object SuccessResponse(object referenceObject)
        {
            return new { Success = true, Object = referenceObject };
        }

        public static object SuccessResponse(string Message, object referenceObject)
        {
            return new { Success = true, Message = Message, Object = referenceObject };
        }
    }

}