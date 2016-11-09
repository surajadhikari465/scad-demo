using NLog;
using System;
using System.Collections.Generic;

namespace Vim.Logging
{
    public class NLogLogger : ILogger
    {
        private Logger logger;

        public NLogLogger(Type t)
        {
            this.logger = LogManager.GetLogger(t.FullName);
        }

        public void Info(string message)
        {
            if (!logger.IsInfoEnabled)
            {
                return;
            }

            if (String.IsNullOrEmpty(message))
            {
                LogMessage(LogLevel.Warn, "NullOrEmpty message.");
                return;
            }

            // Size of AppLog.Message field in DB is 4000.
            IEnumerable<string> messageSplit = message.SplitByLength(4000);

            foreach (string msg in messageSplit)
            {
                LogMessage(LogLevel.Info, msg);
            }
        }

        public void Warn(string message)
        {
            if (!logger.IsWarnEnabled)
            {
                return;
            }

            if (String.IsNullOrEmpty(message))
            {
                LogMessage(LogLevel.Warn, "NullOrEmpty message.");
                return;
            }

            // Size of AppLog.Message field in DB is 4000.
            IEnumerable<string> messageSplit = message.SplitByLength(4000);

            foreach (string msg in messageSplit)
            {
                LogMessage(LogLevel.Warn, msg);
            }
        }

        public void Error(string message)
        {
            if (!logger.IsErrorEnabled)
            {
                return;
            }

            if (String.IsNullOrEmpty(message))
            {
                LogMessage(LogLevel.Warn, "NullOrEmpty message.");
                return;
            }

            // Size of AppLog.Message field in DB is 4000.
            IEnumerable<string> messageSplit = message.SplitByLength(4000);

            foreach (string msg in messageSplit)
            {
                LogMessage(LogLevel.Error, msg);
            }
        }

        public void Error(string message, Exception exception)
        {
            if (!logger.IsErrorEnabled)
            {
                return;
            }

            if (String.IsNullOrEmpty(message))
            {
                LogMessage(LogLevel.Warn, "NullOrEmpty message.");
                return;
            }

            // Size of AppLog.Message field in DB is 4000.
            IEnumerable<string> messageSplit = message.SplitByLength(4000);
            IEnumerable<string> exceptionSplit = message.SplitByLength(4000);

            foreach (string msg in messageSplit)
            {
                LogMessage(LogLevel.Error, msg, exception);
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

            // Size of AppLog.Message field in DB is 4000.
            IEnumerable<string> messageSplit = message.SplitByLength(4000);

            foreach (string msg in messageSplit)
            {
                LogMessage(LogLevel.Debug, msg);
            }
        }

        private void LogMessage(LogLevel level, string message)
        {
            LogEventInfo info = new LogEventInfo(level, logger.Name, message);
            logger.Log(typeof(NLogLogger), info);
        }

        private void LogMessage(LogLevel level, string message, Exception exception)
        {
            LogEventInfo info = new LogEventInfo(level, logger.Name, message);
            info.Exception = exception;
            logger.Log(typeof(NLogLogger), info);
        }
    }
}
