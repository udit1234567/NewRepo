using NexGenIpos.Scheduler.WindowsService;

namespace FrameworkDebugger
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SchedulerEngine engine = new SchedulerEngine();
            engine.StartDebug();
            System.Threading.Thread.Sleep(12000000);
        }
    }
}