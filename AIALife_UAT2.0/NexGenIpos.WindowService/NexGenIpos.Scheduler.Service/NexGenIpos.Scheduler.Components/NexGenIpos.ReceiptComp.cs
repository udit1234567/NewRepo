using System;
using System.Data;
using System.Data.SqlClient;
using System.Messaging;
using System.Threading;
using NexGenIpos.Scheduler;
using NexGenIpos.Scheduler.Util;

namespace NexGenIpos.Scheduler.Components
{
    public class ReceiptCompComponent : IScheduledComponent
    {
        ILogger _logger;

        void IScheduledComponent.Initialize(ILogger logger)
        {
            _logger = logger;
            //TODO: Add logic to dispose any components, objects created in Initialize
        }

        void IScheduledComponent.Execute(ILogger logger)
        {
            _logger = logger;
            WebApiComp.FireForgetAPI(new object(), "InvokeReceiptCheck", "AIAService");
            _logger.Log("Receipt check Component...");
        }

        void IScheduledComponent.DeInitialize(ILogger logger)
        {
            _logger = logger;
            //TODO: Add logic to dispose any components, objects created in Initialize
        }
    }
}