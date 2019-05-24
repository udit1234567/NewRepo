using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.ServiceProcess;

namespace NexGenIpos.Scheduler.WindowsService
{
    [RunInstaller(true)]
    public partial class SchedulerInstaller : System.Configuration.Install.Installer
    {
        public SchedulerInstaller()
        {
            InitializeComponent();
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);
            ServiceController controller = new ServiceController(SchedulerServiceInstaller.ServiceName);

            try
            {
                controller.Start();
            }
            catch (Exception ex)
            {
                String source = "NexGenIpos Schedule Service Installer";
                String log = "Application";

                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, log);
                }

                EventLog eLog = new EventLog();
                eLog.Source = source;
                eLog.WriteEntry(@"The service could not be started. Please start the service manually. Error: " + ex.Message, EventLogEntryType.Error);
            }
        }

        private void ProjectInstaller_BeforeUninstall(object sender, InstallEventArgs e)
        {
            ServiceController controller = new ServiceController(SchedulerServiceInstaller.ServiceName);

            try
            {
                controller.Stop();
            }
            catch (Exception ex)
            {
                string source = "NexGenIpos Schedule Service Installer";
                string log = "Application";

                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, log);
                }

                EventLog eLog = new EventLog();
                eLog.Source = source;
                eLog.WriteEntry(string.Concat(@"The service could not be stopped. Please stop the service manually. Error: ", ex.Message), EventLogEntryType.Error);
            }
        }
    }
}