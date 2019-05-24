using AIA.Life.Models.Hierarchy;
namespace AIA.Life.Business.Hierarchy
{
    public class HierarchyBusiness
    {
         HierarchyData objHierarchy= new HierarchyData();
        
        public HierarchyData Loadentitytypes()
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchy, "Loadentitytypes", "HierarchyData");
            #endregion
            return objHierarchy;
        }       
        public HierarchyData SaveHierarchy(HierarchyData objHierarchyData)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "SaveHierarchy", "HierarchyData");
            #endregion
            return objHierarchy;
        }
        public HierarchyData GetHierachyCodes(string EntityID)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyData", "GetHierachyCodes", "EntityID", EntityID);
            #endregion
            return objHierarchy;          
        }
        public HierarchyData GetPartnerReporting(string SubChannelID)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyData", "GetPartnerReporting", "SubChannelID", SubChannelID);
            #endregion
            return objHierarchy;
        }        
        public HierarchyData GetProspectDistricts(string ProvinceCode)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyData", "GetProspectDistricts", "ProvinceCode", ProvinceCode);
            #endregion
            return objHierarchy;
        }
        public HierarchyData GetProspectCity(string DistrictCode)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyData", "GetProspectCity", "DistrictCode", DistrictCode);
            #endregion
            return objHierarchy;
        }
        public HierarchyData GetPostalCode(string CityCode)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyData", "GetPostalCode", "CityCode", CityCode);
            #endregion
            return objHierarchy;
        }
        public HierarchyData SaveDocumentDetails(HierarchyData objHierarchyData)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "SaveDocumentDetails", "HierarchyData");
            #endregion
            return objHierarchy;
        }
        public HierarchyData LoadCoordinationChannels()
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchy, "LoadCoordinationChannels", "HierarchyData");
            #endregion
            return objHierarchy;
        }
        public HierarchyData GetCoordinationDataGrid(string DataSerializer)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyData", "GetCoordinationDataGrid", "DataSerializer", DataSerializer);
            #endregion
            return objHierarchy;
        }
        public HierarchyData LoadSearchHierarchyData(HierarchyData objHierarchyData)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "LoadSearchHierarchyData", "HierarchyData");
            #endregion
            return objHierarchy;
        }
        public HierarchyData LoadCoordinationSearchDetails(HierarchyData objHierarchyData)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "LoadCoordinationSearchDetails", "HierarchyData");
            #endregion
            return objHierarchy;
        }
        public HierarchyData LoadCoordinationSrchDetails(HierarchyData objHierarchyData)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "LoadCoordinationSrchDetails", "HierarchyData");
            #endregion
            return objHierarchy;
        }
        public HierarchyData GetSubChannelGeoPartner(string GeoSubChannelId)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyData", "GetSubChannelGeoPartner", "GeoSubChannelId", GeoSubChannelId);
            #endregion
            return objHierarchy;
        }
        public HierarchyData LoadCoordinateSubChannel(HierarchyData objHierarchyData)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "LoadCoordinateSubChannel", "HierarchyData");
            #endregion
            return objHierarchy;
        }
        public HierarchyData LoadReportingEntityCode(HierarchyData objHierarchyData)
        {
            #region Call API
            //objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchyData, "HierarchyData", "GetReportingEntityCode", "ReportingEntityCode", ReportingEntityCode);
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "LoadReportingEntityCode", "HierarchyData");
            #endregion
            return objHierarchy;
        }
        public HierarchyData GetReportingEntityCodeAndName(string ReportingEntityCode)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyData", "GetReportingEntityCodeAndName", "ReportingEntityCode", ReportingEntityCode);
            #endregion
            return objHierarchy;
        }
        //temp for geounit
        public HierarchyData LoadReportingEntityCodeGeoUnit(HierarchyData objHierarchyData)
        {
            #region Call API
            //objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchyData, "HierarchyData", "GetReportingEntityCode", "ReportingEntityCode", ReportingEntityCode);
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "LoadReportingEntityCodeGeoUnit", "HierarchyData");
            #endregion
            return objHierarchy;
        }
        //temp for geounit
        public HierarchyData GetReportingEntityCodeAndNameGeoUnit(string ReportingEntityCode)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyData", "GetReportingEntityCodeAndNameGeoUnit", "ReportingEntityCode", ReportingEntityCode);
            #endregion
            return objHierarchy;
        }
        public HierarchyData LoadReportingEntityName(HierarchyData objHierarchyData)
        {
            #region Call API
           // objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyData", "GetReportingEntityName", "ReportingEntityName", ReportingEntityName);
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "LoadReportingEntityName", "HierarchyData");
            #endregion
            return objHierarchy;
        }
        //tempfor geounit
        public HierarchyData LoadReportingEntityNameGeoUnit(HierarchyData objHierarchyData)
        {
            #region Call API
            // objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyData", "GetReportingEntityName", "ReportingEntityName", ReportingEntityName);
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "LoadReportingEntityNameGeoUnit", "HierarchyData");
            #endregion
            return objHierarchy;
        }
        public HierarchyData GetGeoUnitTypes(string PartnerTypId)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyData", "GetGeoUnitTypes", "PartnerTypId", PartnerTypId);
            #endregion
            return objHierarchy;
        }       
        public HierarchyData FetchHierarchyProcessData(HierarchyData objHierarchyData)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchyData, "FetchHierarchyProcessData", "HierarchyData");
            #endregion
            return objHierarchy;
        }
        public HierarchyData GetPartnerCode(string PartnerTypeId)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyData", "GetPartnerCode", "PartnerTypeId", PartnerTypeId);
            #endregion
            return objHierarchy;
        }
        public HierarchyData GetGeoSubChannelParentEntity(string SubChannelCode)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyData", "GetGeoSubChannelParentEntity", "SubChannelCode", SubChannelCode);
            #endregion
            return objHierarchy;
        }
        public HierarchyData GetGeoPartnerParentEntity(string PartnerCode)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyData", "GetGeoPartnerParentEntity", "PartnerCode", PartnerCode);
            #endregion
            return objHierarchy;
        }
        public HierarchyData FetchEntityTypes()
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchy, "FetchEntityTypes", "HierarchyData");
            #endregion
            return objHierarchy;
        }
        public HierarchyData FetchStatus()
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostComplexTypeToAPI<HierarchyData>(objHierarchy, "FetchStatus", "HierarchyData");
            #endregion
            return objHierarchy;
        }
        public HierarchyData GetSearchGeoUnitCode(string CoordinationCode)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyData", "GetSearchGeoUnitCode", "CoordinationCode", CoordinationCode);
            #endregion
            return objHierarchy;
        }
        public HierarchyData GetGeoUnitName(string CoordinationCode)
        {
            #region Call API
            objHierarchy = WebApiLogic.GetPostParametersToAPI<HierarchyData>(objHierarchy, "HierarchyData", "GetGeoUnitName", "CoordinationCode", CoordinationCode);
            #endregion
            return objHierarchy;
        }
    }
}
