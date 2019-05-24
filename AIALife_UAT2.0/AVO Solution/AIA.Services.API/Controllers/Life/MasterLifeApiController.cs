using AIA.Life.Business.Common;
using AIA.Life.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace AIA.Services.API.Controllers.Life
{
    public class MasterLifeApiController :ApiController
    {

        public AddressMaster LoadAddressMaster(AddressMaster objAddressMaster )
        {
            CommonBusiness objBusiness = new CommonBusiness();
           return objBusiness.GetAddresMasters(objAddressMaster);         
        }

        public ProductMasters LoadProductMasters(ProductMasters obj)
        {
            CommonBusiness objBusiness = new CommonBusiness();
            return objBusiness.LoadProductMasters(obj);
        }
    }
}