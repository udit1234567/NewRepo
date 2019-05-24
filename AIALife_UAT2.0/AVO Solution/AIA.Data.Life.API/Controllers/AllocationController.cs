using AIA.Data.Life.API.ControllerLogic.Allocation;
using AIA.Life.Models.Allocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AIA.Data.Life.API.Controllers
{
    public class AllocationController : ApiController
    {

        public AllocationModel LoadAllocationDetails(AllocationModel objAllocationModel)
        {
            AllocationLogic objLogic = new AllocationLogic();
           return objLogic.LoadAllocationDetails(objAllocationModel);
        }
        
        public ManualAllocation ManualAllocationDetails(ManualAllocation objAllocationModel)
        {
            AllocationLogic objLogic = new AllocationLogic();
            return objLogic.ManualAllocationDetails(objAllocationModel);
        }
        
        public AllocationModel SaveAllocation(AllocationModel objAllocationModel)
        {
            AllocationLogic objLogic = new AllocationLogic();
            return objLogic.SaveAllocation(objAllocationModel);
        }
        public AllocationModel ResetAllocation(AllocationModel objAllocationModel)
        {
            AllocationLogic objLogic = new AllocationLogic();
            return objLogic.ResetAllocation(objAllocationModel);
        }


        public ManualAllocation SaveManualAllocation(ManualAllocation objAllocationModel)
        {
            AllocationLogic objLogic = new AllocationLogic();
            return objLogic.SaveManualAllocation(objAllocationModel);
        }
    

        public ManualAllocation ResetManualAllocation(ManualAllocation objAllocationModel)
        {
        AllocationLogic objLogic = new AllocationLogic();
        return objLogic.ResetManualAllocation(objAllocationModel);
        }
    }
}
