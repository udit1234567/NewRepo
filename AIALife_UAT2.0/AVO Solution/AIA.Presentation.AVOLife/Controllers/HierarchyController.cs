using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AIA.Life.Models.Hierarchy;
using AIA.Life.Models.Common;
using Newtonsoft.Json;
using AIA.Life.Repository.AIAEntity;
using System.IO;
using System.Text;
using Grid.Mvc.Ajax.GridExtensions;
//using System.Web.UI;
//using System.Web.UI.WebControls;

namespace AIA.Presentation.AVOLife.Controllers
{
    [Authorize]
    [ErrorLogging]
    public class HierarchyController : BaseController
    {
        string UserName;
        AVOAIALifeEntities Context = new AVOAIALifeEntities();
        public HierarchyController()
        {
            UserName = System.Web.HttpContext.Current.User.Identity.Name;
        }
        HierarchyData objHierarchy = new HierarchyData();
        public ActionResult LoadHirerachyEntityTypes()
        {
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchy, "Loadentitytypes", "HierarchyApi");
            objHierarchy.ObjHeirarchyHistroyDetails = new List<HeirarchyHistroyDetails>();
            objHierarchy.ObjHierarchyStatusDetails = new List<HierachyStatusDetails>();
            objHierarchy.ObjGEOUnitDetails = new List<GEOUnitDetails>();
            objHierarchy.LstPointOfContacts = new List<PointOfContacts>();
            if (objHierarchy.objCommunicationAddress == null)
            {
                objHierarchy.objCommunicationAddress = new Address();
            }
            if (objHierarchy.objRegistrationAddress == null)
            {
                objHierarchy.objRegistrationAddress = new Address();
            }
            if (objHierarchy.LstReportEntityType == null)
            {
                objHierarchy.LstReportEntityType = new List<MasterListItem>();
            }
            if (objHierarchy.LstGeoUnitsSubChannel == null)
            {
                objHierarchy.LstGeoUnitsSubChannel = new List<MasterListItem>();
            }
            if (objHierarchy.LstGeoUnitsTypes == null)
            {
                objHierarchy.LstGeoUnitsTypes = new List<MasterListItem>();
            }
            if (objHierarchy.LstSubChannel == null)
            {
                objHierarchy.LstSubChannel = new List<MasterListItem>();
            }
            if (objHierarchy.LstEntityTypes == null)
            {
                objHierarchy.LstEntityTypes = new List<MasterListItem>();
            }
            if (objHierarchy.LstSearchAction == null)
            {
                objHierarchy.LstSearchAction = new List<MasterListItem>();
            }
            if (objHierarchy.LstGEOParententity == null)
            {
                objHierarchy.LstGEOParententity = new List<MasterListItem>();
            }
            if (objHierarchy.LstOrgStructureTree == null)
            {
                objHierarchy.LstOrgStructureTree = new List<OrgStructureTree>();
            }
            if (objHierarchy.ObjCoordination == null)
            {
                objHierarchy.ObjCoordination = new List<Coordination>();
            }
            if (objHierarchy.ObjCoordinationSearchData == null)
            {
                objHierarchy.ObjCoordinationSearchData = new List<CoordinationSearchData>();
            }
            if (objHierarchy.LstGeoPartner == null)
            {
                objHierarchy.LstGeoPartner = new List<MasterListItem>();
            }
            if (objHierarchy.LstCoordinationSubChannel == null)
            {
                objHierarchy.LstCoordinationSubChannel = new List<MasterListItem>();
            }
            return View("~/Views/Hierarchy/Hierarchy.cshtml", objHierarchy);
        }
        public ActionResult LoadCoordinationDetails()
        {
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchy, "LoadCoordinationChannels", "HierarchyApi");
            if (objHierarchy.LstCoordinationSubChannel == null)
            {
                objHierarchy.LstCoordinationSubChannel = new List<MasterListItem>();
            }
            return PartialView("~/Views/Hierarchy/PartialhierarchyCoordinationpopup.cshtml", objHierarchy);
        }
        public ActionResult LoadCoordinationSearchDetails(HierarchyData objHierarchyData)
        {           
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "LoadCoordinationSearchDetails", "HierarchyApi");
            if (objHierarchy.ObjCoordinationSearchData ==  null)
            {
                objHierarchy.ObjCoordinationSearchData = new List<CoordinationSearchData>();
            }
            return PartialView("~/Views/Hierarchy/Partialhierarchycoordinationpopupgrid.cshtml", objHierarchy);           
        }
        public ActionResult LoadCoordinationSrchDetails(HierarchyData objHierarchyData, string CoordinationCode, string CoordinationName)
        {
            if (CoordinationCode != null)
            {
                objHierarchyData.CoordinationSearchCode = CoordinationCode;
            }
            if (CoordinationName != "")
            {
                objHierarchyData.CoordinationSearchGeoUnitName = CoordinationName;
            }
            AjaxGrid<SearchHierarchy> usergrid = null;
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "LoadCoordinationSrchDetails", "HierarchyApi");
            var GridUserData = objHierarchy.ObjCoordination.AsQueryable();             
            return PartialView("~/Views/Hierarchy/PartialGeoUnitCoordinationDetails.cshtml", objHierarchy);                              
        }
        //public ActionResult PaymentTree(Guid? userId)
        //{
        //    try
        //    {
        //        PermissionTree obj = new PermissionTree();
        //        obj.objTree = new List<TreeView>();
        //        ViewBag.UserId = userId;
        //        obj.objTree = _iMasFacade.PaymentPermissionTree("IMD", userId, "Payment");

        //        return PartialView("~/Areas/UserManagement/Views/UserManagement/PaymentModes.cshtml", obj);
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("Error");
        //    }
        //}
        public ActionResult MenuTree(Guid? userId, string AppId = "IMD")
        {
            try
            {
                PermissionTree obj = new PermissionTree();
                obj.objTree = new List<TreeView>();
                ViewBag.UserId = userId;
                obj = WebApiLogic.GetPostComplexTypeToAPI<PermissionTree>(obj, "MenuPermissionTree", "HierarchyApi"); //_iMasFacade.MenuPermissionTree(AppId, userId, "Menu");
                return PartialView("~/Views/Hierarchy/MenuTree.cshtml", obj);
            }
            catch (Exception ex)
            {

                return View("Error");
            }
        }
        public ActionResult FetchDistrict(string ProvinceCode)
        {
            try
            {
                objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyApi", "GetProspectDistricts", "ProvinceCode", ProvinceCode);
                if (objHierarchy.LstDistrict != null)
                {
                    return Json(objHierarchy.LstDistrict, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(objHierarchy.LstDistrict, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception Ex)
            {
                return View("Error");
            }
        }
        public ActionResult FetchCity(string DistrictCode)
        {
            try
            {
                objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyApi", "GetProspectCity", "DistrictCode", DistrictCode);
                if (objHierarchy.LstCity != null)
                {
                    return Json(objHierarchy.LstCity, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(objHierarchy.LstCity, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception Ex)
            {
                return View("Error");
            }
        }
        public ActionResult FetchPostalCode(string CityCode)
        {
            try
            {
                objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyApi", "GetPostalCode", "CityCode", CityCode);
                if (objHierarchy.Pincode != null)
                {
                    return Json(objHierarchy.Pincode, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(objHierarchy.Pincode, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception Ex)
            {
                return View("Error");
            }
        }
        public ActionResult ProductTree(Guid? userId)
        {
            try
            {
                PermissionTree obj = new PermissionTree();
                obj.objTree = new List<TreeView>();
                ViewBag.UserId = userId;
                obj = WebApiLogic.GetPostComplexTypeToAPI<PermissionTree>(obj, "ProductPermissionTree", "HierarchyApi");
                //obj.objTree = _iMasFacade.ProductPermissionTree("IMD", userId, "Product");
                //_iMasFacade.FetchIMIENumber(obj, userId);
                return PartialView("~/Views/Hierarchy/ProductTree.cshtml", obj);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        public ActionResult FetchHierachyCodes(string EntityID, string EntityValue)
        {
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyApi", "GetHierachyCodes", "EntityID", EntityID);

            return Json(objHierarchy, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveHierarchyBasics(HierarchyData objHierarchyData)
        {
            objHierarchyData = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "SaveHierarchy", "HierarchyApi");
            return Json(objHierarchyData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveHierarchyCommunication(HierarchyData objHierarchyData)
        {
            objHierarchyData = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "SaveHierarchy", "HierarchyApi");
            return Json(objHierarchyData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveHierarchyPointofContact(HierarchyData objHierarchyData)
        {
            try
            {
                if (objHierarchyData.PointofContractData != null)
                    objHierarchyData.LstPointOfContacts = JsonConvert.DeserializeObject<List<PointOfContacts>>(objHierarchyData.PointofContractData);
                objHierarchyData = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "SaveHierarchy", "HierarchyApi");
                return Json(objHierarchyData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        //public ActionResult SaveHierarchyCoordinationDetails(HierarchyData objHierarchyData, string DataSerializer, string DataSerializerPartner, string DataSerializerGeoUnitType, string DataSerializerGeoUnitName, string DataSerializerGeoUnitCode)
        //{
        //    //objHierarchyData.CoordinationSearchIDSerializer = DataSerializer;
        //    objHierarchyData.PartnerSerializer = DataSerializerPartner;
        //    objHierarchyData.GeoUnitTypeSerializer = DataSerializerGeoUnitType;
        //    objHierarchyData.GeoUnitNameSerializer = DataSerializerGeoUnitName;
        //    objHierarchyData.GeoUnitCodeSerializer = DataSerializerGeoUnitCode;            

        //    objHierarchyData = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "SaveHierarchy", "HierarchyApi");
       
        //    return Json(objHierarchyData, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult SaveHierarchyCoordinationDetails(HierarchyData objHierarchyData, string GetCoordinationGridData = null)
        {
            try
            {
                objHierarchyData.ObjCoordination = new List<Coordination>();
                if (GetCoordinationGridData != null)
                    objHierarchyData.ObjCoordination = JsonConvert.DeserializeObject<List<Coordination>>(GetCoordinationGridData);
                objHierarchyData = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "SaveHierarchy", "HierarchyApi");
                return Json(objHierarchyData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return View("Error");
            }
        }
        public ActionResult SaveHierarchyGeoUnit(HierarchyData objHierarchyData)
        {
            try
            {
                if (objHierarchyData.GeoUnitData != null)
                    objHierarchyData.ObjGEOUnitDetails = JsonConvert.DeserializeObject<List<GEOUnitDetails>>(objHierarchyData.GeoUnitData);
                objHierarchyData = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "SaveHierarchy", "HierarchyApi");
                return Json(objHierarchyData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UploadFilePath(string documentData, string PartnerCode, string EntityType)
        {
            try
            {

                List<DocumentDetails> lstDocumentUpload = new List<DocumentDetails>();
                HierarchyData objHierarchyData = new HierarchyData();
                lstDocumentUpload = JsonConvert.DeserializeObject<List<DocumentDetails>>(documentData);

                List<HttpPostedFileBase> files = new List<HttpPostedFileBase>();
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase filebase = Request.Files[file];
                    DocumentUpload(filebase);
                    files.Add(filebase);

                }
                objHierarchyData.UserName = UserName;
                objHierarchyData.EntityType = EntityType;
                objHierarchyData.PartnerCode = PartnerCode;
                objHierarchyData.Code = PartnerCode;
                objHierarchyData.LstdocumentName = lstDocumentUpload;
                objHierarchyData = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "SaveDocumentDetails", "HierarchyApi");
                return Json(objHierarchyData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //string ErrorCode = objError.WriteError("Policy", "UploadFilePath", ex);
                string ErrorCode = "0000";
                return Json(ErrorCode, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public void DocumentUpload(HttpPostedFileBase file)
        {
            try
            {

                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        string directryPath = Server.MapPath("Upload");
                        if (!Directory.Exists(directryPath))
                        {
                            Directory.CreateDirectory(directryPath);
                        }
                        var fileName = Path.GetFileName(file.FileName);

                        var filename = Path.Combine(directryPath, Path.GetFileName(file.FileName));
                        file.SaveAs(filename);
                    }
                }

            }
            catch (Exception ex)
            {
                //ErrorLogging objErrorLog = new ErrorLogging();
                //var ErrorCode = objErrorLog.WriteException(ex, "PersonalAccident", "DocumentUpload");
                ViewBag.ExceptionMessage = "Internal Error Occurred in the system :  Please contact Administrator(Error Code:" + ")";
            }

        }
        public void DownloadUploadedfile(string PartnerCode, string FileName)
        {
            tblProspectDocument objtblDocuments = new tblProspectDocument();
            HierarchyData objHierarchyData = new HierarchyData();
            DocumentDetails objDocumentDetails = new DocumentDetails();
            tblOrganization objtblOrganization = null;
            if (PartnerCode != null)
            {
                objtblOrganization = Context.tblOrganizations.Where(a => a.PartnerCode == PartnerCode).FirstOrDefault();
            }
            var directorypath = System.Web.HttpContext.Current.Server.MapPath("Upload");
            var PolicyFileName = Path.Combine(directorypath, Path.GetFileName(FileName));
            string result = PolicyFileName;
            string Filecontenttype = Context.tblProspectDocuments.Where(a => a.OrganizationID == objtblOrganization.OrganizationID).FirstOrDefault().DocType;
            objDocumentDetails.FileContent = Encoding.ASCII.GetBytes(Context.tblProspectDocuments.Where(a => a.OrganizationID == objtblOrganization.OrganizationID).FirstOrDefault().DocPath);
            System.Web.HttpContext.Current.Response.ContentType = Filecontenttype;
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(PolicyFileName));
            System.Web.HttpContext.Current.Response.Charset = "";
            System.Web.HttpContext.Current.Response.OutputStream.Write(objDocumentDetails.FileContent, 0, objDocumentDetails.FileContent.Length);
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.Close();

            if (System.IO.File.Exists(result))
            {
                System.Web.HttpContext.Current.Response.OutputStream.Dispose();
                System.IO.File.Delete(result);
            }

        }
        public ActionResult FetchPartnerReporting(string SubChannelID)
        {
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyApi", "GetPartnerReporting", "SubChannelID", SubChannelID);
            return Json(objHierarchy, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchHierarchy()
        {
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchy, "Loadentitytypes", "HierarchyApi");
            if (objHierarchy.LstSearchAction == null)
            {
                objHierarchy.LstSearchAction = new List<MasterListItem>();
            }
            if(objHierarchy.LstSearchHierarchy==null)
            {
                objHierarchy.LstSearchHierarchy = new List<SearchHierarchy>();
            }
            return View("~/Views/Hierarchy/SearchHierarchy.cshtml", objHierarchy);
        }
        public ActionResult FetchEntityTypes()
        {
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchy, "FetchEntityTypes", "HierarchyApi");
            return Json(objHierarchy.LstEntityTypes, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FetchStatus()
        {
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchy, "FetchStatus", "HierarchyApi");
            return Json(objHierarchy.LstStatus, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchHierarchyGrid(HierarchyData objHierarchyData)
        {
            try
            {
                AjaxGrid<SearchHierarchy> usergrid = null;
                objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "LoadSearchHierarchyData", "HierarchyApi");
                var GridUserData = objHierarchy.LstSearchHierarchy.AsQueryable();
                ViewBag.Details = objHierarchyData;
                TempData["Load"] = "FirstTime";
                usergrid = (AjaxGrid<SearchHierarchy>)new AjaxGridFactory().CreateAjaxGrid(GridUserData, 1, false);                
                return PartialView("~/Views/Hierarchy/PartialSearchHierarchyGrid.cshtml", usergrid);

            }
            catch (Exception Ex)
            {
                return View("Error");
            }
        }
        [HttpGet]
        public ActionResult LoadNxtSearchDetailsPage(int? Page, string Code, string Name, string Status, string EntityType)        
        {
            try
            {
                var aj = new Grid.Mvc.Ajax.GridExtensions.AjaxGridFactory();
                List<SearchHierarchy> lstIMD = new List<SearchHierarchy>();
                HierarchyData objHierarchyData = new HierarchyData();
                objHierarchyData.SearchCode = Code;
                objHierarchyData.SearchName = Name;
                objHierarchyData.SearchStatus = Status;
                objHierarchyData.SearchEntityType = EntityType;
                objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "LoadSearchHierarchyData", "HierarchyApi");
                var grid = aj.CreateAjaxGrid(objHierarchy.LstSearchHierarchy.AsQueryable(), Page.HasValue ? Page.Value : 1, Page.HasValue, 2);
                ViewBag.Details = objHierarchy;

                return Json(new
                {
                    Html = RenderPartialViewToString("~/Views/Hierarchy/PartialSearchHierarchyGrid.cshtml", grid),
                    HasItems = grid.HasItems
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        protected string RenderPartialViewToString(string viewName, object model)
        {
            try
            {
                if (string.IsNullOrEmpty(viewName))
                    viewName = ControllerContext.RouteData.GetRequiredString("action");

                ViewData.Model = model;

                using (var sw = new StringWriter())
                {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                    var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                    viewResult.View.Render(viewContext, sw);

                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }
        public ActionResult SaveHierarchyLicense(HierarchyData objHierarchyData)
        {
            objHierarchyData = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "SaveHierarchy", "HierarchyApi");
            return Json(objHierarchyData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CoordinationPopoupGridTasks(HierarchyData objHierarchyData, string getCoordinationDetails = null)
        {
            try
            {
                objHierarchyData.ObjCoordinationSearchData = new List<CoordinationSearchData>();
                if (getCoordinationDetails != null)
                    objHierarchyData.ObjCoordinationSearchData = JsonConvert.DeserializeObject<List<CoordinationSearchData>>(getCoordinationDetails);
                objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchyData, "GetCoordinationDataGrid", "HierarchyApi");
                return PartialView("~/Views/Hierarchy/Partialgeounitscoordinationsdetails.cshtml", objHierarchy);          
            }
            catch (Exception Ex)
            {
                return View("Error");
            }
        }
        public ActionResult FetchSubChannelGeoPartner(string GeoSubChannelId)
        {
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyApi", "GetSubChannelGeoPartner", "GeoSubChannelId", GeoSubChannelId);
            return Json(objHierarchy.LstGeoPartner, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FetchCoordinateSubChannel(HierarchyData objHierarchyData)
        {            
            objHierarchyData = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "LoadCoordinateSubChannel", "HierarchyApi");
            return Json(objHierarchyData.LstCoordinationSubChannel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FetchGeoUnitTypes(string PartnerTypId)
        {
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyApi", "GetGeoUnitTypes", "PartnerTypId", PartnerTypId);
            return Json(objHierarchy.LstGeoUnitsTypes, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadReportingEntityCode(HierarchyData objHierarchyData, string ReportingEntityType, string ReportingEntityCode)
        {
            if (ReportingEntityType != null && ReportingEntityCode != null)
            {
                objHierarchyData.ReportEntityType = ReportingEntityType;
                objHierarchyData.SearchTerm = ReportingEntityCode;
            }
            //objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchyData, "HierarchyApi", "GetReportingEntityCode", "ReportingEntityCode", ReportingEntityCode);
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "LoadReportingEntityCode", "HierarchyApi");
            return Json(objHierarchy.LstReportingEntityCode, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadReportingEntityCodeAndName(string ReportingEntityCode)
        {
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyApi", "GetReportingEntityCodeAndName", "ReportingEntityCode", ReportingEntityCode);
            return Json(objHierarchy.LstReportingEntityCodeName, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadReportingEntityName(HierarchyData objHierarchyData, string ReportingEntityType, string ReportingEntityName)
        {
            if (ReportingEntityType != null && ReportingEntityName != null)
            {
                objHierarchyData.ReportEntityType = ReportingEntityType;
                objHierarchyData.SearchTerm = ReportingEntityName;
            }
            //objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyApi", "GetReportingEntityName", "ReportingEntityName", ReportingEntityName);
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "LoadReportingEntityName", "HierarchyApi");
            return Json(objHierarchy.LstReportingEntityName, JsonRequestBehavior.AllowGet);
        }        
        public ActionResult LoadPartnerCode(string PartnerTypeId)
        {
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyApi", "GetPartnerCode", "PartnerTypeId", PartnerTypeId);
            return Json(objHierarchy.Code, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadGeoSubChannelParentEntity(string SubChannelCode)
        {
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyApi", "GetGeoSubChannelParentEntity", "SubChannelCode", SubChannelCode);
            return Json(objHierarchy.LstGEOParententity, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadGeoPartnerParentEntity(string PartnerCode)
        {
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyApi", "GetGeoPartnerParentEntity", "PartnerCode", PartnerCode);
            return Json(objHierarchy.LstGEOParententity, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Hierarchy(HierarchyData objHierarchyData)
        {
            objHierarchyData = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "FetchHierarchyProcessData", "HierarchyApi");

            if (objHierarchyData.objCommunicationAddress == null)
            {
                objHierarchyData.objCommunicationAddress = new Address();
            }
            if (objHierarchyData.objRegistrationAddress == null)
            {
                objHierarchyData.objRegistrationAddress = new Address();
            }
            if (objHierarchyData.LstReportEntityType == null)
            {
                objHierarchyData.LstReportEntityType = new List<MasterListItem>();
            }
            if (objHierarchyData.LstGeoUnitsSubChannel == null)
            {
                objHierarchyData.LstGeoUnitsSubChannel = new List<MasterListItem>();
            }
            if (objHierarchyData.LstGeoUnitsTypes == null)
            {
                objHierarchyData.LstGeoUnitsTypes = new List<MasterListItem>();
            }
            if (objHierarchyData.LstSubChannel == null)
            {
                objHierarchyData.LstSubChannel = new List<MasterListItem>();
            }
            if (objHierarchyData.LstEntityTypes == null)
            {
                objHierarchyData.LstEntityTypes = new List<MasterListItem>();
            }
            if (objHierarchyData.LstSearchAction == null)
            {
                objHierarchyData.LstSearchAction = new List<MasterListItem>();
            }
            if (objHierarchyData.LstPartnertypes == null)
            {
                objHierarchyData.LstPartnertypes = new List<MasterListItem>();
            }
            if (objHierarchyData.LstStatus == null)
            {
                objHierarchyData.LstStatus = new List<MasterListItem>();
            }
            if (objHierarchyData.LstHistoryDetails == null)
            {
                objHierarchyData.LstHistoryDetails = new List<MasterListItem>();
            }
            if (objHierarchyData.ObjHeirarchyHistroyDetails == null)
            {
                objHierarchyData.ObjHeirarchyHistroyDetails = new List<HeirarchyHistroyDetails>();
            }
            if (objHierarchyData.ObjHierarchyStatusDetails == null)
            {
                objHierarchyData.ObjHierarchyStatusDetails = new List<HierachyStatusDetails>();
            }
            if (objHierarchyData.ObjGEOUnitDetails == null)
            {
                objHierarchyData.ObjGEOUnitDetails = new List<GEOUnitDetails>();
            }
            if (objHierarchyData.LstPointOfContacts == null)
            {
                objHierarchyData.LstPointOfContacts = new List<PointOfContacts>();
            }
            if (objHierarchyData.LstViewHierarchyTree == null)
            {
                objHierarchyData.LstViewHierarchyTree = new List<ViewHierarchyTree>();
            }
            if (objHierarchyData.ObjHierarchyTarget == null)
            {
                objHierarchyData.ObjHierarchyTarget = new List<HierarchyTarget>();
            }
            if (objHierarchyData.ObjManpowerDetails == null)
            {
                objHierarchyData.ObjManpowerDetails = new List<ManpowerDetails>();
            }
            if (objHierarchyData.ObjCoordination == null)
            {
                objHierarchyData.ObjCoordination = new List<Coordination>();
            }
            if (objHierarchyData.LstCoordinationChannel == null)
            {
                objHierarchyData.LstCoordinationChannel = new List<MasterListItem>();
            }
            if (objHierarchyData.LstCoordinationSubChannel == null)
            {
                objHierarchyData.LstCoordinationSubChannel = new List<MasterListItem>();
            }
            if (objHierarchyData.LstSalutation == null)
            {
                objHierarchyData.LstSalutation = new List<MasterListItem>();
            }
            if (objHierarchyData.LstdocumentName == null)
            {
                objHierarchyData.LstdocumentName = new List<DocumentDetails>();
            }
            if (objHierarchyData.LstOrgStructureTree == null)
            {
                objHierarchyData.LstOrgStructureTree = new List<OrgStructureTree>();
            }
            if (objHierarchyData.LstGeoPartner == null)
            {
                objHierarchyData.LstGeoPartner = new List<MasterListItem>();
            }
            if (objHierarchyData.LstGEOParententity == null)
            {
                objHierarchyData.LstGEOParententity = new List<MasterListItem>();
            }
            if (objHierarchyData.ObjGEOUnitDetails == null)
            {
                objHierarchyData.ObjGEOUnitDetails = new List<GEOUnitDetails>();
            }
            return View("~/Views/Hierarchy/Hierarchy.cshtml", objHierarchyData);
        }
        public ActionResult SearchGeoUnitCode(string CoordinationCode)
        {
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyApi", "GetSearchGeoUnitCode", "CoordinationCode", CoordinationCode);
            return Json(objHierarchy.LstCoordinationGeoUnitCode, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetGeoUnitName(string CoordinationCode)
        {
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyApi", "GetGeoUnitName", "CoordinationCode", CoordinationCode);
            return Json(objHierarchy.LstCoordinationGeoUnitName, JsonRequestBehavior.AllowGet);
        }
        //temporary for geounit
        public ActionResult LoadReportingEntityCodeGeoUnit(HierarchyData objHierarchyData, string ReportingEntityType, string ReportingEntityCode)
        {
            if (ReportingEntityType != null && ReportingEntityCode != null)
            {
                objHierarchyData.ReportEntityType = ReportingEntityType;
                objHierarchyData.SearchTerm = ReportingEntityCode;
            }
            //objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchyData, "HierarchyApi", "GetReportingEntityCode", "ReportingEntityCode", ReportingEntityCode);
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "LoadReportingEntityCodeGeoUnit", "HierarchyApi");
            return Json(objHierarchy.LstReportingEntityCode, JsonRequestBehavior.AllowGet);
        }
        //temporary for geounit
        public ActionResult LoadReportingEntityCodeAndNameGeoUnit(string ReportingEntityCode)
        {
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyApi", "GetReportingEntityCodeAndNameGeoUnit", "ReportingEntityCode", ReportingEntityCode);
            return Json(objHierarchy.LstReportingEntityCodeName, JsonRequestBehavior.AllowGet);
        }
        //temporary for geounit
        public ActionResult LoadReportingEntityNameGeoUnit(HierarchyData objHierarchyData, string ReportingEntityType, string ReportingEntityName)
        {
            if (ReportingEntityType != null && ReportingEntityName != null)
            {
                objHierarchyData.ReportEntityType = ReportingEntityType;
                objHierarchyData.SearchTerm = ReportingEntityName;
            }
            //objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyApi", "GetReportingEntityName", "ReportingEntityName", ReportingEntityName);
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "LoadReportingEntityNameGeoUnit", "HierarchyApi");
            return Json(objHierarchy.LstReportingEntityName, JsonRequestBehavior.AllowGet);
        }
        //////temporary for geounit
        //public ActionResult LoadReportingEntityOnNameGeoUnitCode(string ReportingEntityCode)
        //{
        //    objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyApi", "GetReportingEntityCodeNameGeoUnit", "ReportingEntityCode", ReportingEntityCode);
        //    return Json(objHierarchy.LstReportingEntityCodeName, JsonRequestBehavior.AllowGet);
        //}
    }
}         
       
 
