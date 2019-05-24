using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using AIA.Life.Models.Hierarchy;
using AIA.Data.Life.API.ControllerLogic.Hierarchy;
using AIA.Data.Life.API.ControllerLogic.Common;

namespace AIA.Data.Life.API.Controllers
{
    public class HierarchyDataController : ApiController
    {
        HierarchyData objHierarchy = new HierarchyData();
        HierarchyDataBusiness objHierarchyBusiness = new HierarchyDataBusiness();
        CommonBusiness objCommonBusiness = new CommonBusiness();
        public HierarchyData Loadentitytypes()
        {
            objHierarchy.LstEntityTypes = objHierarchyBusiness.GetHierarchyTypes();
            objHierarchy.LstReportEntityType = objHierarchyBusiness.GetHierarchyTypes();
            objHierarchy.LstHistoryDetails = objHierarchyBusiness.GetHierarchyHistoryDetails();
            //objHierarchy.LstGEOParententity = objHierarchyBusiness.GetGEOUnitsParentEntity();
            objHierarchy.ObjManpowerDetails = objHierarchyBusiness.GetManpowerDetails();
            //objHierarchy.ObjCoordination = objHierarchyBusiness.GetCoordination();
            objHierarchy.LstPartnertypes = objHierarchyBusiness.GetPartnerTypes();
            objHierarchy.ObjHierarchyTarget = objHierarchyBusiness.GetHierarchyTargetData();
            objHierarchy.LstdocumentName = objHierarchyBusiness.GetDocumentDetails();
            //objHierarchy.ObjCoordinationSearchData = objHierarchyBusiness.GetCoordinationSearchDetails();
            objHierarchy.LstSalutation = objHierarchyBusiness.GetHierarchySalutation();
            objHierarchy.LstPosition = objHierarchyBusiness.GetHierarchyPosition();
            //objHierarchy.LstViewHierarchyTree = objHierarchyBusiness.GetHierarchyTree();
            //objHierarchy.LstOrgStructureTree= objHierarchyBusiness.GetHierarchyTree(" ");
            objHierarchy.LstStatus = objHierarchyBusiness.GetStatus();
            objHierarchy.LstSearchAction = objHierarchyBusiness.LoadQuickActions();
            return objHierarchy;
        }
        public HierarchyData SaveHierarchy(HierarchyData objHierarchyData)
        {
            objHierarchy = objHierarchyBusiness.SaveHierarchy(objHierarchyData);
            return objHierarchy;
        }
        public HierarchyData GetHierachyCodes(string EntityID)
        {
            if (EntityID == "483")
            {
                objHierarchy.CorporateCode = objHierarchyBusiness.GetCorporateCode();
            }
            else if (EntityID == "1254")
            {
                objHierarchy.ChannelCode = objHierarchyBusiness.GetChannelCode();
                objHierarchy = objHierarchyBusiness.FetchReportingdata(objHierarchy, "483");
            }
            else if (EntityID == "1255")
            {
                objHierarchy.SubChannelCode = objHierarchyBusiness.GetSubChannelCode();
                objHierarchy = objHierarchyBusiness.FetchReportingdata(objHierarchy, "1254");
            }
            else if (EntityID == "1256")
            {
                //objHierarchy.PartnerCode = objHierarchyBusiness.GetPartnerCode();
                objHierarchy = objHierarchyBusiness.FetchReportingdata(objHierarchy, "1255");
            }
            else if (EntityID == "1257")
            {
                objHierarchy.GeoUnitCode = objHierarchyBusiness.GetGeoUnitCode();
                objHierarchy = objHierarchyBusiness.FetchReportingdata(objHierarchy, "1256");
            }
            return objHierarchy;
        }
        public HierarchyData GetProspectDistricts(string ProvinceCode)
        {
            if (ProvinceCode != null)
            {
                objHierarchy.LstDistrict = objCommonBusiness.GetDistrictMaster(ProvinceCode);
                return objHierarchy;
            }
            else
            {
                return objHierarchy;
            }
        }
        public HierarchyData GetProspectCity(string DistrictCode)
        {
            if (DistrictCode != null)
            {
                objHierarchy.LstCity = objCommonBusiness.GetCityMaster(DistrictCode);
                return objHierarchy;
            }
            else
            {
                return objHierarchy;
            }
        }
        public HierarchyData GetPostalCode(string CityCode)
        {
            if (CityCode != null)
            {
                objHierarchy.Pincode = objCommonBusiness.GetPostalCodeMaster(CityCode);
                return objHierarchy;
            }
            else
            {
                return objHierarchy;
            }
        }
        public HierarchyData SaveDocumentDetails(HierarchyData objHierarchyData)
        {
            objHierarchy = objHierarchyBusiness.SaveDocumentDetails(objHierarchyData);
            return objHierarchy;
        }
        public HierarchyData GetPartnerReporting(string SubChannelID)
        {
            objHierarchy = objHierarchyBusiness.FetchPartnerReporting(SubChannelID);
            return objHierarchy;
        }
        public HierarchyData LoadSearchHierarchyData(HierarchyData objHierarchyData)
        {
            objHierarchy.LstSearchHierarchy = objHierarchyBusiness.LoadSearchHierarchyData(objHierarchyData);
            objHierarchy.LstSearchAction = objHierarchyBusiness.LoadQuickActions();
            return objHierarchy;
        }
        public HierarchyData LoadCoordinationSearchDetails(HierarchyData objHierarchyData)
        {            
            objHierarchy.ObjCoordinationSearchData = objHierarchyBusiness.GetCoordinationSearchDetails(objHierarchyData);            
            return objHierarchy;
        }
        public HierarchyData LoadCoordinationSrchDetails(HierarchyData objHierarchyData)
        {            
            objHierarchy.ObjCoordination = objHierarchyBusiness.GetCoordinationDetails(objHierarchyData);
            return objHierarchy;
        }
        public HierarchyData LoadCoordinationChannels()
        {
            objHierarchy.LstCoordinationChannel = objHierarchyBusiness.GetHierarchyChannel();
            //objHierarchy.LstCoordinationSubChannel = objHierarchyBusiness.GetHierarchySubChannel();
            return objHierarchy;
        }
        public HierarchyData GetCoordinationDataGrid(string DataSerializer)
        {
            //objHierarchy.ObjCoordination = objHierarchyBusiness.GetCoordinationDataGrid(DataSerializer);
            return objHierarchy;
        }
        public HierarchyData GetSubChannelGeoPartner(string GeoSubChannelId)
        {
            objHierarchy.LstGeoPartner = objHierarchyBusiness.GetPartner(GeoSubChannelId);
            return objHierarchy;
        }
        public HierarchyData LoadCoordinateSubChannel(HierarchyData objHierarchyData)
        {
            objHierarchy.LstCoordinationSubChannel = objHierarchyBusiness.GetHierarchySubChannel(objHierarchyData);
            return objHierarchy;
        }
        public HierarchyData GetGeoUnitTypes(string PartnerTypId)
        {
            objHierarchy.LstGeoUnitsTypes = objHierarchyBusiness.GetGeoUnit(PartnerTypId);
            return objHierarchy;
        }    
        public HierarchyData LoadReportingEntityCode(HierarchyData objHierarchyData)
        {           
            objHierarchy.LstReportingEntityCode = objHierarchyBusiness.GetReportingEntityCode(objHierarchyData);
            return objHierarchy;
        }
        public HierarchyData GetReportingEntityCodeAndName(string ReportingEntityCode)
        {
            if (ReportingEntityCode != null)
                objHierarchy.LstReportingEntityCodeName = objHierarchyBusiness.GetReportingEntityCodeAndName(ReportingEntityCode);
            return objHierarchy;
        }
        //temp for GeoUnit
        public HierarchyData LoadReportingEntityCodeGeoUnit(HierarchyData objHierarchyData)
        {
            objHierarchy.LstReportingEntityCode = objHierarchyBusiness.GetReportingEntityCodeGeoUnit(objHierarchyData);
            return objHierarchy;
        }
        //temp for GeoUnit
        public HierarchyData GetReportingEntityCodeAndNameGeoUnit(string ReportingEntityCode)
        {
            if (ReportingEntityCode != null)
                objHierarchy.LstReportingEntityCodeName = objHierarchyBusiness.GetReportingEntityCodeAndNameGeoUnit(ReportingEntityCode);
            return objHierarchy;
        }
        public HierarchyData LoadReportingEntityName(HierarchyData objHierarchyData)
        {
            objHierarchy.LstReportingEntityName = objHierarchyBusiness.GetReportingEntityName(objHierarchyData);
            return objHierarchy;
        }
        //temp for GeoUnit
        public HierarchyData LoadReportingEntityNameGeoUnit(HierarchyData objHierarchyData)
        {
            objHierarchy.LstReportingEntityName = objHierarchyBusiness.GetReportingEntityNameGeoUnit(objHierarchyData);
            return objHierarchy;
        }
        public HierarchyData FetchHierarchyProcessData(HierarchyData objHierarchyData)
        {
            objHierarchy = objHierarchyBusiness.FetchHierarchy(objHierarchyData);
            objHierarchy.LstEntityTypes = objHierarchyBusiness.GetHierarchyTypes();
            objHierarchy.LstStatus = objHierarchyBusiness.GetStatus();
            objHierarchy.LstReportEntityType = objHierarchyBusiness.GetHierarchyTypes();
            objHierarchy.LstHistoryDetails = objHierarchyBusiness.GetHierarchyHistoryDetails();
            objHierarchy.ObjHeirarchyHistroyDetails = objHierarchyBusiness.LoadHierarchyHistoryData(objHierarchyData);
            objHierarchy.ObjHierarchyStatusDetails = objHierarchyBusiness.LoadHierarchyStatusData(objHierarchyData);
            objHierarchy.LstSalutation = objHierarchyBusiness.GetHierarchySalutation();
            objHierarchy.LstPartnertypes = objHierarchyBusiness.GetPartnerTypes();            
            //objHierarchy = objHierarchyBusiness.FetchReportingdata(objHierarchy, "1255");
            objHierarchy.LstSubChannel = objHierarchyBusiness.GetHierarchySubChannel();
            objHierarchy.LstGeoUnitsSubChannel= objHierarchyBusiness.GetHierarchySubChannel();
            objHierarchy.LstGeoPartner= objHierarchyBusiness.GetHierarchyPartners();
            objHierarchy.LstGeoUnitsTypes = objHierarchyBusiness.GetHierarchyGeoUnits();
            objHierarchy.LstOrgStructureTree = objHierarchyBusiness.GetHierarchyTree(objHierarchyData);
            //objHierarchy.Code = objHierarchyData.SearchCode;
            objHierarchy.LstGEOParententity = objHierarchyBusiness.GetGeoParentEntity();
            return objHierarchy;
        }
        public HierarchyData GetPartnerCode(string PartnerTypeId)
        {
            objHierarchy.Code = objHierarchyBusiness.GetPartnerCode(PartnerTypeId);
            //objHierarchy.PartnerCode = objHierarchyBusiness.GetPartnerCode(PartnerTypeId);
            return objHierarchy;
        }
        public HierarchyData GetGeoSubChannelParentEntity(string SubChannelCode)
        {
            objHierarchy.LstGEOParententity = objHierarchyBusiness.GetGeoSubChannelParentEntity(SubChannelCode);
            return objHierarchy;
        }
        public HierarchyData GetGeoPartnerParentEntity(string PartnerCode)
        {
            objHierarchy.LstGEOParententity = objHierarchyBusiness.GetGeoPartnerParentEntity(PartnerCode);
            return objHierarchy;
        }
        public HierarchyData FetchEntityTypes()
        {
            objHierarchy.LstEntityTypes = objHierarchyBusiness.GetHierarchyTypes();           
            return objHierarchy;
        }
        public HierarchyData FetchStatus()
        {
            objHierarchy.LstStatus = objHierarchyBusiness.GetStatus();
            return objHierarchy;
        }
        public HierarchyData GetSearchGeoUnitCode(string CoordinationCode)
        {
            if (CoordinationCode != null)
                objHierarchy.LstCoordinationGeoUnitCode= objHierarchyBusiness.SearchGeoUnitCode(CoordinationCode);
            return objHierarchy;
        }
        public HierarchyData GetGeoUnitName(string CoordinationCode)
        {
            if (CoordinationCode != null)
                objHierarchy.LstCoordinationGeoUnitName = objHierarchyBusiness.GetGeoUnitName(CoordinationCode);
            return objHierarchy;
        }
    }
}