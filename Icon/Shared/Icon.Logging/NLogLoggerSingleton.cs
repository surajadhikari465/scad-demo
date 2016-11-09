using NLog;
using System;
using System.Collections.Generic;

namespace Icon.Logging
{
    public class NLogLoggerSingleton : ILogger
    {
        private Logger logger;

        public NLogLoggerSingleton(Type t)
        {
            logger = LogManager.GetLogger(t.FullName);
        }

        public void Info(string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                LogMessage(LogLevel.Warn, "NullOrEmpty message.");
                return;
            }

            IEnumerable<string> messageSplit = SplitMessage(message);

            foreach (string msg in messageSplit)
            {
                LogMessage(LogLevel.Info, msg);
            }
        }

        public void Warn(string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                LogMessage(LogLevel.Warn, "NullOrEmpty message.");
                return;
            }

            IEnumerable<string> messageSplit = SplitMessage(message);

            foreach (string msg in messageSplit)
            {
                LogMessage(LogLevel.Warn, msg);
            }
        }

        public void Error(string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                LogMessage(LogLevel.Warn, "NullOrEmpty message.");
                return;
            }

            IEnumerable<string> messageSplit = SplitMessage(message);

            foreach (string msg in messageSplit)
            {
                LogMessage(LogLevel.Error, msg);
            }
        }

        public void Debug(string message)
        {
            if (!logger.IsDebugEnabled)
            {
                return;
            }

            if (String.IsNullOrEmpty(message))
            {
                LogMessage(LogLevel.Warn, "NullOrEmpty message.");
                return;
            }

            IEnumerable<string> messageSplit = SplitMessage(message);

            foreach (string msg in messageSplit)
            {
                LogMessage(LogLevel.Debug, msg);
            }
        }

        private IEnumerable<string> SplitMessage(string message)
        {
            // Size of AppLog.Message field in DB is 4000.
            IEnumerable<string> messageSplit = message.SplitByLength(4000);
            return messageSplit;
        }

        private void LogMessage(LogLevel level, string message)
        {
            LogEventInfo info = new LogEventInfo(level, logger.Name, message);
            logger.Log(typeof(NLogLoggerSingleton), info);
        }
    }
}
