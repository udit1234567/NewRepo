using System.Globalization;
using System.Threading;

namespace NexGenIpos.Scheduler.Util
{
    public class SchedulerHelper
    {
        public static void SetDefaultDateTimeFormat()
        {
            //Set default standard date time format. this enables uniform comparison across cultures
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            culture.DateTimeFormat.ShortTimePattern = "HH:mm";
            culture.DateTimeFormat.LongTimePattern = "HH:mm:ss";
            Thread.CurrentThread.CurrentCulture = culture;
        }
    }
}