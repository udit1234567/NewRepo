using AIA.Life.Models.Allocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Business.Allocation
{
     public class AllocationBusiness
    {

        public AllocationModel LoadAllocationDetails(AllocationModel objAllocationModel)
        {
            #region Call API
            objAllocationModel = WebApiLogic.GetPostComplexTypeToAPI<AllocationModel>(objAllocationModel, "LoadAllocationDetails", "Allocation");
            #endregion
            return objAllocationModel;
           
        }
        public ManualAllocation ManualAllocationDetails(ManualAllocation objAllocationModel)
        {
            #region Call API
            objAllocationModel = WebApiLogic.GetPostComplexTypeToAPI<ManualAllocation>(objAllocationModel, "ManualAllocationDetails", "Allocation");
            #endregion
            return objAllocationModel;
        }

        public AllocationModel SaveAllocation(AllocationModel objAllocationModel)
        {
            #region Call API
            objAllocationModel = WebApiLogic.GetPostComplexTypeToAPI<AllocationModel>(objAllocationModel, "SaveAllocation", "Allocation");
            #endregion
            return objAllocationModel;
        }

        public AllocationModel ResetAllocation(AllocationModel objAllocationModel)
        {
            #region Call API
            objAllocationModel = WebApiLogic.GetPostComplexTypeToAPI<AllocationModel>(objAllocationModel, "ResetAllocation", "Allocation");
            #endregion
            return objAllocationModel;
        }

        public ManualAllocation SaveManualAllocation(ManualAllocation objAllocationModel)
        {
            #region Call API
            objAllocationModel = WebApiLogic.GetPostComplexTypeToAPI<ManualAllocation>(objAllocationModel, "SaveManualAllocation", "Allocation");
            #endregion
            return objAllocationModel;
        }

        public ManualAllocation ResetManualAllocation(ManualAllocation objAllocationModel)
        {
            #region Call API
            objAllocationModel = WebApiLogic.GetPostComplexTypeToAPI<ManualAllocation>(objAllocationModel, "ResetManualAllocation", "Allocation");
            #endregion
            return objAllocationModel;
        }


    }
}
