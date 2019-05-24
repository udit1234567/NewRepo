using NexGenIpos.Scheduler.Util;
using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NexGenIpos.Scheduler
{
    public interface IScheduleManager
    {
        ///// <summary>
        ///// The schedule that this manager instance will execute
        ///// </summary>
        //static ISchedule Schedule { get; }

        /// <summary>
        /// Checks the Schedule to determine if it is due and starts the execution of scheduled component
        /// </summary>
        /// <returns>A list of messages (error or information) to write to the log</returns>
        void Start();
    }

    public class ScheduleManager : IScheduleManager
    {
        private ILogger _logger;
        private const int PROCESS_STATUS_CHECK_FREQUENCY = 60000;
        private const double SCHEDULE_OVERRUN_TIME = 60000;
        private const int REINIT_COUNT = 5;

        public ISchedule _schedule;

        public ScheduleManager(ISchedule schedule)
        {
            _schedule = schedule;
        }

        public void Start()
        {
            _logger = new SimpleLogger(SimpleLogger.SimpleLoggerType.File, "NexGenIpos.Scheduler." +
                                       _schedule.LoggerName, Path.Combine(ConfigurationManager.AppSettings["AppLogPath"], "NexGenIpos.Scheduler." + _schedule.LoggerName + ".log"), 10240000L);

            //If schedule supports parallel processing then use TPL else normal thread processing
            //Note components calling in to COM components or calling components that are already multi threaded must not use TPL
            if (_schedule.UseMultiCoreProcess)
            {
                Task scheduleTask = Task.Factory.StartNew(ExecuteSchedule);
            }
            else
            {
                ThreadStart scheduleTask = new ThreadStart(ExecuteSchedule);
                Thread scheduleThread = new Thread(scheduleTask);
                scheduleThread.Start();
            }

            _schedule.ScheduleStatus = ScheduleProcessStatus.Running;
        }

        public void ExecuteSchedule()
        {
            SchedulerHelper.SetDefaultDateTimeFormat();
            _logger.Log("Schedule Started...");
            bool canContinueExecution = true;
            string startTimeString = _schedule.StartTime.ToLongTimeString();
            string endTimeString = _schedule.EndTime.ToLongTimeString();
            DateTime scheduleStartTime = DateTime.Parse(startTimeString);
            DateTime scheduleEndTime = DateTime.Parse(endTimeString);
            DateTime nextRepeatTime = DateTime.MaxValue;

            #region Initial wait time check before execution starts

            if (scheduleStartTime > DateTime.Now)
            {
                //Still there is time for the schedule to start. Wait for start time
                TimeSpan initialWaitTime = _schedule.StartTime - DateTime.Now;
                nextRepeatTime = DateTime.Now.AddMinutes(initialWaitTime.TotalMilliseconds);
                _logger.Log(string.Format("Waiting to start execution at-{0}", _schedule.StartTime.ToLongTimeString()));
            }
            else
                if (scheduleEndTime < DateTime.Now)
                {
                    //Scheduled processing time for today is over. Reschedule for next day and wait!
                    //One exception - if the schedule is configured to run once daily then continue
                    //                cos, we don't know if the scheduled component has processed!
                    if (_schedule.RepeatSchedule > 0)
                    {
                        nextRepeatTime = DateTime.Parse(startTimeString).AddDays(1);
                        _logger.Log(string.Format("Scheduling to next day: starts at-{0}, Repeat Schedule-{1}min", nextRepeatTime, _schedule.RepeatSchedule));
                    }
                }

            //Schedule start time is deferred! so wait
            if (nextRepeatTime < DateTime.MaxValue)
            {
                canContinueExecution = WaitForNextScheduleTime(nextRepeatTime);
            }

            #endregion Initial wait time check before execution starts

            int runCount = REINIT_COUNT;

            //Time to start the schedule
            while (canContinueExecution)
            {
                nextRepeatTime = DateTime.MaxValue;

                #region Initialize Component

                if (runCount >= REINIT_COUNT)
                {
                    try
                    {
                        //Initialize the component
                        _schedule.LoggerComponent.Initialize(_logger);

                        runCount = 0;
                    }
                    catch (Exception ex)
                    {
                        canContinueExecution = false;
                        _logger.Log("Component failed to initialize \r\n " + ex.ToString());
                    }
                }

                #endregion Initialize Component

                if (canContinueExecution)
                {
                    #region Execute component

                    try
                    {
                        //Execute the scheduled component
                        _schedule.LoggerComponent.Execute(_logger);
                    }
                    catch (Exception ex)
                    {
                        _logger.Log("Component failed to execute \r\n " + ex.ToString());
                    }

                    #endregion Execute component

                    #region Check schedule end time and reschedule

                    if (_schedule.RepeatSchedule > 0)
                    {
                        //Component is scheduled to repeat every RepeatSchedule (minutes) till End Time
                        nextRepeatTime = DateTime.Now.AddMinutes(_schedule.RepeatSchedule);
                        TimeSpan checkForEndTime = nextRepeatTime - scheduleEndTime;

                        //schedule repeat time has crossed schedule end time + the allowed overrun
                        //reschedule the processing to next day
                        if (checkForEndTime.TotalMilliseconds > SCHEDULE_OVERRUN_TIME)
                        {
                            //Check if schedule crossed the days end and moved to next day!!!
                            //This happens for long running schedules
                            if ((nextRepeatTime.Date > scheduleEndTime.Date) && ((nextRepeatTime.Date - scheduleEndTime.Date).TotalDays == 1) && nextRepeatTime.Date.Equals(DateTime.Now.Date))
                            {
                                _logger.Log(string.Format("Scheduling overrun to next day: Continuing the Schedule for today, repeating at-{0} minutes", _schedule.RepeatSchedule));
                            }
                            else
                            {
                                //set next schedule run time
                                nextRepeatTime = DateTime.Parse(startTimeString).AddDays(1);
                                _logger.Log(string.Format("Scheduling to next day: Runs at-{0}, Repeat Schedule-{1} Nimutes", nextRepeatTime, _schedule.RepeatSchedule));
                            }

                            //set next schdule end time
                            scheduleEndTime = scheduleEndTime.AddDays(1);
                        }
                    }
                    else
                    {
                        //Component is configured to run only once daily

                        //Check if schedule crossed the days end and moved to next day!!!
                        //This happens for long running schedules
                        if ((nextRepeatTime.Date > scheduleEndTime.Date) && ((nextRepeatTime.Date - scheduleEndTime.Date).TotalDays == 1) && nextRepeatTime.Date.Equals(DateTime.Now.Date))
                        {
                            _logger.Log(string.Format("Scheduling overrun to next day: Continuing the Schedule to run once today at {0}", nextRepeatTime));
                        }
                        else
                        {
                            //reschedule the processing to next day
                            nextRepeatTime = DateTime.Parse(startTimeString).AddDays(1);
                            _logger.Log(string.Format("Scheduling to next day: Runs at-{0}, runs only once", nextRepeatTime));
                        }

                        //set next schdule end time
                        scheduleEndTime = scheduleEndTime.AddDays(1);
                    }

                    #endregion Check schedule end time and reschedule

                    runCount++;

                    if (runCount >= REINIT_COUNT) {
                        _schedule.LoggerComponent.DeInitialize(_logger);
                    }

                    //Check if schedule can continue - service may have stopped or paused!
                    canContinueExecution = WaitForNextScheduleTime(nextRepeatTime);
                }
            }

            //DeInitialize the component
            _schedule.LoggerComponent.DeInitialize(_logger);
            _logger.Log("Schedule Stopped.");
        }

        private bool WaitForNextScheduleTime(DateTime nextRunTime)
        {
            bool canContinue = true;
            TimeSpan timeToWait = nextRunTime - DateTime.Now;
            int threadWait = (timeToWait.Milliseconds > PROCESS_STATUS_CHECK_FREQUENCY ? PROCESS_STATUS_CHECK_FREQUENCY : timeToWait.Milliseconds);

            while (canContinue && DateTime.Now < nextRunTime)
            {
                Thread.Sleep(threadWait);
                canContinue = (_schedule.ScheduleStatus == ScheduleProcessStatus.Running);
            }

            canContinue = (_schedule.ScheduleStatus == ScheduleProcessStatus.Running);
            return canContinue;
        }
    }
}