using AIA.Life.Models.Common;
using AIA.Life.Repository.AIAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIA.Data.Life.API.ControllerLogic.Common
{
    public static class ServiceLog
    {
        
        public static void CreateServiceLog(TpServiceLog tpServiceLog)
        {
            using (AVOAIALifeEntities context = new AVOAIALifeEntities())
            {
                tblLogTPService logTPService = new tblLogTPService();
                logTPService.CreatedDate = DateTime.Now;
                logTPService.ServiceRequest = tpServiceLog.ServiceRequest;
                logTPService.ServiceTraceID = tpServiceLog.ServiceTraceID;
                logTPService.UpdatedDate = DateTime.Now;
                logTPService.ServiceResponse = tpServiceLog.ServiceResponse;
                context.tblLogTPServices.Add(logTPService);
                context.SaveChanges();
            }
        }
        public static void UpdateILStatusLog(ILStatus iLStatus)
        {
            using (AVOAIALifeEntities context = new AVOAIALifeEntities())
            {
                tblLogILUpdate logILUpdate = new tblLogILUpdate();
                logILUpdate.CreatedDate = DateTime.Now;
                logILUpdate.ProposalNo = iLStatus.ProposalNo;
                logILUpdate.ServiceName = iLStatus.ServiceName;
                logILUpdate.ServiceStatus = iLStatus.ServiceStatus;
                logILUpdate.UpdatedDate = DateTime.Now;
                context.tblLogILUpdates.Add(logILUpdate);
                context.SaveChanges();
            }
        }
    }
}