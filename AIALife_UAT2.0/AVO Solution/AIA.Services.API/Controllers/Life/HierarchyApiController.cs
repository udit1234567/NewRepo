using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AIA.Life.Models.Hierarchy;
using System.Web.Http;
using AIA.Life.Business.Hierarchy;
namespace AIA.Services.API.Controllers.Life
{
    public class HierarchyApiController : ApiController
    {
        HierarchyBusiness objHierarchybusiness = new HierarchyBusiness();
        public HierarchyData Loadentitytypes()
        {
            return objHierarchybusiness.Loadentitytypes();
        }      
        public HierarchyData SaveHierarchy(HierarchyData objHierarchyData)
        {
            return objHierarchybusiness.SaveHierarchy(objHierarchyData);
        }
        public HierarchyData GetHierachyCodes(string EntityID)
        {
            return objHierarchybusiness.GetHierachyCodes(EntityID);
        }
        public HierarchyData GetProspectDistricts(string ProvinceCode)
        {
            return objHierarchybusiness.GetProspectDistricts(ProvinceCode);
        }
        public HierarchyData GetProspectCity(string DistrictCode)
        {
            return objHierarchybusiness.GetProspectCity(DistrictCode);
        }
        public HierarchyData GetPostalCode(string CityCode)
        {
            return objHierarchybusiness.GetPostalCode(CityCode);
        }
        public HierarchyData SaveDocumentDetails(HierarchyData objHierarchyData)
        {
            return objHierarchybusiness.SaveDocumentDetails(objHierarchyData);
        }
        public HierarchyData GetPartnerReporting(string SubChannelID)
        {
            return objHierarchybusiness.GetPartnerReporting(SubChannelID);
        }
        public HierarchyData LoadSearchHierarchyData(HierarchyData objHierarchyData)
        {
            return objHierarchybusiness.LoadSearchHierarchyData(objHierarchyData);
        }
        public HierarchyData LoadCoordinationSearchDetails(HierarchyData objHierarchyData)
        {
            return objHierarchybusiness.LoadCoordinationSearchDetails(objHierarchyData);
        }
        public HierarchyData LoadCoordinationSrchDetails(HierarchyData objHierarchyData)
        {
            return objHierarchybusiness.LoadCoordinationSrchDetails(objHierarchyData);
        }
        public HierarchyData LoadCoordinationChannels()
        {
            return objHierarchybusiness.LoadCoordinationChannels();
        }
        public HierarchyData GetCoordinationDataGrid(string DataSerializer)
        {
            return objHierarchybusiness.GetCoordinationDataGrid(DataSerializer);
        }
        public HierarchyData GetSubChannelGeoPartner(string GeoSubChannelId)
        {
            return objHierarchybusiness.GetSubChannelGeoPartner(GeoSubChannelId);
        }
        public HierarchyData LoadCoordinateSubChannel(HierarchyData objHierarchyData)
        {
            return objHierarchybusiness.LoadCoordinateSubChannel(objHierarchyData);
        }
        public HierarchyData GetGeoUnitTypes(string PartnerTypId)
        {
            return objHierarchybusiness.GetGeoUnitTypes(PartnerTypId);
        }       
        public HierarchyData LoadReportingEntityCode(HierarchyData objHierarchyData)
        {
            return objHierarchybusiness.LoadReportingEntityCode(objHierarchyData);
        }
        public HierarchyData GetReportingEntityCodeAndName(string ReportingEntityCode)
        {
            return objHierarchybusiness.GetReportingEntityCodeAndName(ReportingEntityCode);
        }
        //temp for geo
        public HierarchyData LoadReportingEntityCodeGeoUnit(HierarchyData objHierarchyData)
        {
            return objHierarchybusiness.LoadReportingEntityCodeGeoUnit(objHierarchyData);
        }
        //temp for geo
        public HierarchyData GetReportingEntityCodeAndNameGeoUnit(string ReportingEntityCode)
        {
            return objHierarchybusiness.GetReportingEntityCodeAndNameGeoUnit(ReportingEntityCode);
        }
        public HierarchyData LoadReportingEntityName(HierarchyData objHierarchyData)
        {
            return objHierarchybusiness.LoadReportingEntityName(objHierarchyData);
        }
        //temp for geounit
        public HierarchyData LoadReportingEntityNameGeoUnit(HierarchyData objHierarchyData)
        {
            return objHierarchybusiness.LoadReportingEntityNameGeoUnit(objHierarchyData);
        }
        public HierarchyData FetchHierarchyProcessData(HierarchyData objHierarchyData)
        {
            return objHierarchybusiness.FetchHierarchyProcessData(objHierarchyData);
        }
        public HierarchyData GetPartnerCode(string PartnerTypeId)
        {
            return objHierarchybusiness.GetPartnerCode(PartnerTypeId);
        }
        public HierarchyData GetGeoSubChannelParentEntity(string SubChannelCode)
        {
            return objHierarchybusiness.GetGeoSubChannelParentEntity(SubChannelCode);
        }
        public HierarchyData GetGeoPartnerParentEntity(string PartnerCode)
        {
            return objHierarchybusiness.GetGeoPartnerParentEntity(PartnerCode);
        }
        public HierarchyData FetchEntityTypes()
        {
            return objHierarchybusiness.FetchEntityTypes();
        }
        public HierarchyData FetchStatus()
        {
            return objHierarchybusiness.FetchStatus();
        }
        public HierarchyData GetSearchGeoUnitCode(string CoordinationCode)
        {
            return objHierarchybusiness.GetSearchGeoUnitCode(CoordinationCode);
        }
        public HierarchyData GetGeoUnitName(string CoordinationCode)
        {
            return objHierarchybusiness.GetGeoUnitName(CoordinationCode);
        }
    }
}