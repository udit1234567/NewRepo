namespace NexGenIpos.Scheduler.WindowsService
{
    partial class SchedulerInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SchedulerServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.SchedulerServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            //
            // NexGenIposSchedulerServiceProcessInstaller
            //
            this.SchedulerServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.SchedulerServiceProcessInstaller.Password = null;
            this.SchedulerServiceProcessInstaller.Username = null;
            //
            // NexGenIposSchedulerServiceInstaller
            //
            this.SchedulerServiceInstaller.Description = "This service executes the NexGenIpos scheduled job processing.";
            this.SchedulerServiceInstaller.DisplayName = "NexGenIpos Windows scheduler";
            this.SchedulerServiceInstaller.ServiceName = "NexGenIpos.Scheduler.Service";
            this.SchedulerServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            //
            // NexGenIposSchedulerInstaller
            //
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
                this.SchedulerServiceProcessInstaller,
                this.SchedulerServiceInstaller
            });
            this.BeforeUninstall += new System.Configuration.Install.InstallEventHandler(this.ProjectInstaller_BeforeUninstall);

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller SchedulerServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller SchedulerServiceInstaller;
    }
}