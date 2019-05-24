using NexGenIpos.Scheduler.Util;

namespace NexGenIpos.Scheduler
{
    /// <summary>
    /// Implement this interface in the component you would like to schedule
    /// </summary>
    public interface IScheduledComponent
    {
        void Initialize(ILogger logger);

        void Execute(ILogger logger);

        void DeInitialize(ILogger logger);
    }
}