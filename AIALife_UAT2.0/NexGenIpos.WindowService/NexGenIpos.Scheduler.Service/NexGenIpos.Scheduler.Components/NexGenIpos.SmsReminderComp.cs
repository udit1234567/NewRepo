using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexGenIpos.Scheduler;
using NexGenIpos.Scheduler.Util;

namespace NexGenIpos.Scheduler.Components
{
    public class SmsReminderComponent : IScheduledComponent
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
            WebApiComp.FireForgetAPI(null, "SMSReminder", "AIAService");
            _logger.Log("Sms Reminder Component...");
        }

        void IScheduledComponent.DeInitialize(ILogger logger)
        {
            _logger = logger;
            //TODO: Add logic to dispose any components, objects created in Initialize
        }
    }
}
