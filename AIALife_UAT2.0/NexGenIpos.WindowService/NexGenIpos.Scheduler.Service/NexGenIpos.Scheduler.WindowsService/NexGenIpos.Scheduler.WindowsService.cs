using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using NexGenIpos.Scheduler.Util;

namespace NexGenIpos.Scheduler.WindowsService
{
    public partial class SchedulerEngine : ServiceBase
    {
        private ILogger _logger;
        private List<ISchedule> _schedules = null;
        private Timer _timer;
        private DateTime _lastCheckTime;
        private bool _schedulesStarted = false;

        public SchedulerEngine()
        {
            InitializeComponent();
            _logger = new SimpleLogger(SimpleLogger.SimpleLoggerType.File, "NexGenIpos.Scheduler.NexGenIpos",
                                       Path.Combine(ConfigurationManager.AppSettings["AppLogPath"], "NexGenIpos.Scheduler.NexGenIpos.log"), 51200L);
        }

        #region Service Control

        protected override void OnStart(string[] args)
        {
            _logger.Log("Service Started");

            try
            {
                _schedules = ScheduleConfiguration.GetSchedules();

                if (_schedules == null || _schedules.Count < 1)
                {
                    _logger.Log("No Schedules defined! Stopping Service...");
                    base.Stop();
                }

                _timer = new Timer(2000d);
                _timer.Elapsed += ServiceTimer_Tick;
                _timer.Start();
            }
            catch (Exception ex)
            {
                _logger.Log("Service failed to start schedules! Stopping Service... \r\n" + ex.ToString());
                base.Stop();
            }
        }

        protected override void OnStop()
        {
            _timer.Stop();
            StopSchedules(ScheduleProcessStatus.Stopped);
            _logger.Log("Service Stopped");
        }

        protected override void OnPause()
        {
            _timer.Stop();
            StopSchedules(ScheduleProcessStatus.Paused);
            _logger.Log("Service Paused");
        }

        protected override void OnContinue()
        {
            _timer.Stop();
            StartSchedules();
            _timer.Start();
            _logger.Log("Service Continued");
        }

        #endregion Service Control

        private void ServiceTimer_Tick(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();

            if (!_schedulesStarted)
            {
                StartSchedules();
                _timer.Interval = 60000d;
            }
            else
            {
                _lastCheckTime = DateTime.Now;
                //_logger.Log("Checking for system events...");
            }

            _timer.Start();
        }

        private void StartSchedules()
        {
            try
            {
                _schedules.ForEach(s =>
                {
                    if (s.ScheduleStatus != ScheduleProcessStatus.Running)
                    {
                        IScheduleManager scheduleManager = new ScheduleManager(s);
                        scheduleManager.Start();
                        System.Threading.Thread.Sleep(200);
                    }
                });
                _schedulesStarted = true;
                LogStartupInformation();
            }
            catch (Exception ex)
            {
                _logger.Log("Service failed to start schedules! Stopping Service... \r\n" + ex.ToString());
                base.Stop();
            }
        }

        private void StopSchedules(ScheduleProcessStatus status)
        {
            try
            {
                if (_schedules != null)
                {
                    _schedules.ForEach(s =>
                    {
                        if (s.ScheduleStatus == ScheduleProcessStatus.Running)
                        {
                            s.ScheduleStatus = status;
                        }
                    });
                    _schedulesStarted = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Log("Service failed to stop schedules! Stopping Service... \r\n" + ex.ToString());
                base.Stop();
            }
        }

        private void LogStartupInformation()
        {
            if (_schedules.Count == 0)
            {
                _logger.Log("No schedules have been configured");
                return;
            }

            _logger.Log("The following services have been configured:");
            _schedules.ForEach(s =>
            {
                if (s.RepeatSchedule == 0)
                {
                    _logger.Log(string.Format("{0} Process will run once at {1} everyday", s.LoggerName, s.StartTime.ToShortTimeString()));
                }
                else
                {
                    if (s.EndTime == DateTime.MaxValue)
                    {
                        _logger.Log(string.Format("{0} Process runs every {1} millisecs continuously", s.LoggerName, s.RepeatSchedule));
                    }
                    else
                    {
                        _logger.Log(string.Format("{0} Process starts at {1}, repeating every {2} millisecs till {3}",
                                                  s.LoggerName, s.StartTime.ToShortTimeString(), s.RepeatSchedule, s.EndTime.ToShortTimeString()));
                    }
                }
            });
        }

        public void StartDebug()
        {
            this.OnStart(null);
        }
    }
}