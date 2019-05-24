using System;
using System.Diagnostics;
using System.IO;

namespace NexGenIpos.Scheduler.Util
{
    public interface ILogger
    {
        void Log(Exception ex);

        void Log(string[] content);

        void Log(string content);

        void WriteToEventLog(string[] content);

        void WriteToFile(string[] content);
    }

    public class SimpleLogger : ILogger
    {
        #region SimpleLoggerType enum

        public enum SimpleLoggerType
        {
            File,
            EventLog
        }

        #endregion SimpleLoggerType enum

        private readonly string _applicationName;
        private readonly SimpleLoggerType _defaultType;
        private readonly long _logFileMaxSize;
        private readonly string _logFilePath;

        /// <param name="defaultType">Can be File or EventLog</param>
        /// <param name="applicationName">This is the name that appears in the log entry</param>
        /// <param name="logFilePath">The location of the log file</param>
        /// <param name="logFileMaxSize">For file logging we can set a max size, in BYTEs, 1000 is about 20-30 lines to keep, good enough for debugging</param>
        public SimpleLogger(SimpleLoggerType defaultType, string applicationName, string logFilePath,
                            long logFileMaxSize)
        {
            _defaultType = defaultType;
            _applicationName = applicationName;
            _logFilePath = logFilePath;
            _logFileMaxSize = logFileMaxSize;
        }

        #region ILogger Members

        public void Log(Exception ex)
        {
            Log(string.Format("EXCEPTION: {0}", ex));

            for (Exception inner = ex.InnerException; inner != null; inner = inner.InnerException)
            {
                Log(string.Format("INNER EXCEPTION: {0}", inner));
            }
        }

        public void Log(string[] content)
        {
            Log(content, true);
        }

        public void Log(string content)
        {
            Log(content, true);
        }

        public void WriteToEventLog(string[] content)
        {
            string message = string.Empty;

            foreach (string s in content)
            {
                message = message + s;
            }

            EventLog.WriteEntry(_applicationName, message, EventLogEntryType.Information, 0);
        }

        public void WriteToFile(string[] content)
        {
            if (File.Exists(_logFilePath))
            {
                var log = new FileInfo(_logFilePath);

                if (log.Length > _logFileMaxSize)
                {
                    log.Delete();
                }
            }

            StreamWriter logger = File.AppendText(_logFilePath);

            foreach (string item in content)
            {
                logger.Write(item);
            }

            logger.Close();
        }

        #endregion ILogger Members

        private void Log(string content, bool prepareLines)
        {
            var tmp = new[] { content };
            Log(tmp, prepareLines);
        }

        private void Log(string[] content, bool prepareLines)
        {
            if (prepareLines)
            {
                for (int i = 0; i < content.Length; i++)
                {
                    content[i] = prepareLine(content[i]);
                }
            }

            if (_defaultType == SimpleLoggerType.File)
            {
                WriteToFile(content);
            }
            else
            {
                WriteToEventLog(content);
            }
        }

        private string prepareLine(string line)
        {
            return (DateTime.Now.ToString() + ": " + line + "\r\n");
        }
    }
}