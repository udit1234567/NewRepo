using System;

namespace NexGenIpos.Scheduler
{
    public enum ScheduleProcessStatus
    {
        Created = 1,
        Running,
        Paused,
        Stopped
    }

    public interface ISchedule
    {
        string LoggerName
        {
            get;
            set;
        }

        IScheduledComponent LoggerComponent
        {
            get;
            set;
        }

        DateTime StartTime
        {
            get;
            set;
        }

        DateTime EndTime
        {
            get;
            set;
        }

        long RepeatSchedule
        {
            get;
            set;
        }

        //Add indicator for Schedule Manager to start normal thread
        bool UseMultiCoreProcess
        {
            get;
            set;
        }

        ScheduleProcessStatus ScheduleStatus
        {
            get;
            set;
        }

        string ToString();
    }

    public class Schedule : ISchedule
    {
        #region Scheduler related private properties

        private DateTime _startTime;
        private DateTime _endTime;
        private long _repeatSchedule;
        private ScheduleProcessStatus _scheduleProcessStatus;
        private bool _useMultiCoreProcess;

        #endregion Scheduler related private properties

        #region Scheduled Service Related Public Properties

        public string LoggerName
        {
            get;
            set;
        }

        public IScheduledComponent LoggerComponent
        {
            get;
            set;
        }

        public DateTime StartTime
        {
            get
            {
                return _startTime;
            }

            set
            {
                _startTime = value;
            }
        }

        public DateTime EndTime
        {
            get
            {
                return _endTime;
            }

            set
            {
                _endTime = value;
            }
        }

        public long RepeatSchedule
        {
            get
            {
                return _repeatSchedule;
            }

            set
            {
                _repeatSchedule = value;
            }
        }

        public bool UseMultiCoreProcess
        {
            get
            {
                return _useMultiCoreProcess;
            }

            set
            {
                _useMultiCoreProcess = value;
            }
        }

        public ScheduleProcessStatus ScheduleStatus
        {
            get
            {
                return _scheduleProcessStatus;
            }

            set
            {
                _scheduleProcessStatus = value;
            }
        }

        #endregion Scheduled Service Related Public Properties

        public Schedule(string name, IScheduledComponent component, DateTime startTime, DateTime endTime, int repeatSchedule, int useMultiCoreProcess)
        {
            //Check if the schedule is configured to run continuously without end time!!
            if (startTime.ToShortTimeString().Equals("00:00") && endTime.ToShortTimeString().Equals("00:00") && repeatSchedule > 0)
            {
                endTime = DateTime.MaxValue;
            }

            if (startTime > endTime)
            {
                endTime = startTime;
            }

            if (startTime == endTime)
            {
                repeatSchedule = 0;
            }
            else
                if (repeatSchedule == 0)
                {
                    endTime = startTime;
                }

            LoggerName = name;
            LoggerComponent = component;
            _startTime = startTime;
            _endTime = endTime;
            _repeatSchedule = repeatSchedule;
            _scheduleProcessStatus = Scheduler.ScheduleProcessStatus.Created;
            _useMultiCoreProcess = Convert.ToBoolean(useMultiCoreProcess);
        }

        public override string ToString()
        {
            return LoggerName;
        }
    }
}