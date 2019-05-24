using System.ServiceProcess;

namespace NexGenIpos.Scheduler.WindowsService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new SchedulerEngine()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}