using AIA.Life.Business.Allocation;
using AIA.Life.Models.Allocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AIA.Services.API.Controllers.Life
{
    public class AllocationApiController : ApiController
    {

        public AllocationModel LoadAllocationDetails(AllocationModel objAllocationModel)
        {
            AllocationBusiness objLogic = new AllocationBusiness();
            return objLogic.LoadAllocationDetails(objAllocationModel);
        }
        public ManualAllocation ManualAllocationDetails(ManualAllocation objAllocationModel)
        {
            AllocationBusiness objLogic = new AllocationBusiness();
            return objLogic.ManualAllocationDetails(objAllocationModel);
        }
        public AllocationModel SaveAllocation(AllocationModel objAllocationModel)
        {
            AllocationBusiness objLogic = new AllocationBusiness();
            return objLogic.SaveAllocation(objAllocationModel);
        }
        public AllocationModel ResetAllocation(AllocationModel objAllocationModel)
        {
            AllocationBusiness objLogic = new AllocationBusiness();
            return objLogic.ResetAllocation(objAllocationModel);
        }

        public ManualAllocation SaveManualAllocation(ManualAllocation objAllocationModel)
        {
            AllocationBusiness objLogic = new AllocationBusiness();
            return objLogic.SaveManualAllocation(objAllocationModel);
        }
        public ManualAllocation ResetManualAllocation(ManualAllocation objAllocationModel)
        {
            AllocationBusiness objLogic = new AllocationBusiness();
            return objLogic.ResetManualAllocation(objAllocationModel);
        }
    }
}
