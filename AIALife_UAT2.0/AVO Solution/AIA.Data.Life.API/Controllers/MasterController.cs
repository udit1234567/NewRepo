using AIA.Data.Life.API.ControllerLogic.Common;
using AIA.Life.Models.AgentonBoarding;
using AIA.Life.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AIA.Data.Life.API.Controllers
{
    public class MasterController : ApiController
    {
        RecruitmentAgent obj = new RecruitmentAgent();
        CommonBusiness objCommonBusiness = new CommonBusiness();
        public AddressMaster LoadAddressMaster(AddressMaster objAddressMaster)
        {

            ControllerLogic.Common.CommonBusiness objCommonBusiness = new ControllerLogic.Common.CommonBusiness();
            
            return objCommonBusiness.GetAddresMasters(objAddressMaster.Term);
        }

        public ProductMasters LoadProductMasters(ProductMasters obj)
        {
            ControllerLogic.Common.CommonBusiness objCommonBusiness = new ControllerLogic.Common.CommonBusiness();
            return objCommonBusiness.LoadProductMasters(obj);
        }

        public RecruitmentAgent GetProvinceMaster()
        {
            obj.LstProvince = objCommonBusiness.GetProvinceMaster();
            return obj;

        }
        public RecruitmentAgent GetProspectDistricts(string ProvinceCode)
        {
            if (ProvinceCode != null)
                obj.LstDistrict = objCommonBusiness.GetDistrictMaster(ProvinceCode);
            return obj;
        }
        public RecruitmentAgent GetProspectCity(string DistrictCode)
        {
            if (DistrictCode != null)
                obj.LstCity = objCommonBusiness.GetCityMaster(DistrictCode);
            return obj;

        }
        public RecruitmentAgent GetPostalCode(string CityCode)
        {
            if (CityCode != null)
            {
                obj.Provience = objCommonBusiness.GetProvince(CityCode);
                obj.Distric = objCommonBusiness.GetDistrict(CityCode);
            }
                
            return obj;
        }

        
    }
}