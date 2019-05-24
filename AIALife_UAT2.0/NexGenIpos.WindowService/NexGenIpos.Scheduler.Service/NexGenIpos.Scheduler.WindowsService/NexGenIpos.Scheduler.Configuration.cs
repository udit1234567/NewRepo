using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NexGenIpos.Scheduler.Components;
using NexGenIpos.Scheduler.Util;

namespace NexGenIpos.Scheduler.WindowsService
{
    public class ScheduleConfiguration
    {
        public static List<ISchedule> GetSchedules()
        {
            SchedulerHelper.SetDefaultDateTimeFormat();
            var schedules = new List<ISchedule>();
            string scheduledComponentsFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"NexGenIpos.Scheduler.Schedules.csv");
            var parser = new DelimitedTextFileParser(scheduledComponentsFile, ',', true);

            foreach (var line in parser)
            {
                string componentName = line[0].Trim().ToUpper();

                if (componentName.StartsWith("--"))
                {
                    continue;
                }

                IScheduledComponent component = null;
                var startTime = DateTime.Parse(line[1].Trim());
                var endTime = DateTime.Parse(line[2].Trim());
                var repeatSchedule = line.Length > 3 ? int.Parse(line[3].Trim()) : 0;
                var useMultiCoreProcess = line.Length > 4 ? int.Parse(line[4].Trim()) : 0;

                string compFullName = componentName;
                int compNameLen = componentName.IndexOf("_");
                compNameLen = (compNameLen > 0 ? compNameLen - 1 : componentName.Length);

                componentName = componentName.Substring(0, compNameLen);

                //when new schedule component is created, add to switch case here
                switch (componentName)
                {
                    case "RECIEPT":
                        component = new ReceiptCompComponent();
                        break;
                    case "MASTERSBAT":
                        component = new EtlILDataComponent();
                        break;
                    case "SMSREMINDER":
                        component = new SmsReminderComponent();
                        break;
                    case "PROPOSAlWITHDRAW":
                        component = new SmsReminderComponent();
                        break;
                    case "BIZDATE":
                        component = new BizDateComponent();
                        break;
                    default:
                        throw new ApplicationException(string.Format("Specified scheduler component is not created or added to component library: {0}", componentName));
                }

                ISchedule schedule = new Schedule(compFullName, component, startTime, endTime, repeatSchedule, useMultiCoreProcess);
                schedules.Add(schedule);
            }

            return schedules;
        }
    }
}