using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JanaShakthi.Services.API.Areas.Life.Controllers
{
    public class PolicyController : ApiController
    {

        public JanaShakthi.Life.Models.Policy.Policy LoadMasters(JanaShakthi.Life.Models.Policy.Policy objpolicy)
        {
            JanaShakthi.Life.Business.Policy.PolicyBusiness obj = new JanaShakthi.Life.Business.Policy.PolicyBusiness();
            objpolicy= obj.LoadMasters(objpolicy);
            return objpolicy;
        }
    }
}
