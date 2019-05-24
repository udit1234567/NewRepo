using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexGenIpos.Scheduler;
using NexGenIpos.Scheduler.Util;

namespace NexGenIpos.Scheduler.Components
{
    public class EtlILDataComponent : IScheduledComponent
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
            try
            {
                string batFilePath = System.Configuration.ConfigurationManager.AppSettings["BatFilePath"].ToString();
                //var processInfo = new ProcessStartInfo("cmd.exe", "cd /d " + batFilePath); //"\"C:\\Program Files (x86)\AssaultCube_v1.1.0.4\assaultcube.bat\"");

                var processInfo = new ProcessStartInfo("cmd.exe", "c:\\run_AllMasters.bat");
                processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = false;
                processInfo.RedirectStandardError = true;
                processInfo.RedirectStandardOutput = true;

                var process = Process.Start(processInfo);

                process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                    Console.WriteLine("output>>" + e.Data);
                process.BeginOutputReadLine();

                process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                    Console.WriteLine("error>>" + e.Data);
                process.BeginErrorReadLine();

                process.WaitForExit();

                Console.WriteLine("ExitCode: {0}", process.ExitCode);
                process.Close();
            }
            catch(Exception ex)
            {
                _logger.Log(ex.InnerException.ToString());
            }
            _logger.Log("Etl data Component...");
        }

        void IScheduledComponent.DeInitialize(ILogger logger)
        {
            _logger = logger;
            //TODO: Add logic to dispose any components, objects created in Initialize
        }
    }
}
