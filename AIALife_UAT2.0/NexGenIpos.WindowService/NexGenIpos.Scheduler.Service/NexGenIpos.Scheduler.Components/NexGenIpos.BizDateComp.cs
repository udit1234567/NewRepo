using NexGenIpos.Scheduler.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexGenIpos.Scheduler.Components
{
    public class BizDateComponent : IScheduledComponent
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
            WebApiComp.FireForgetAPI(new object(), "CallBizDate", "AIAService");
            _logger.Log("Biz date Component...");
        }

        void IScheduledComponent.DeInitialize(ILogger logger)
        {
            _logger = logger;
            //TODO: Add logic to dispose any components, objects created in Initialize
        }
    }
}
