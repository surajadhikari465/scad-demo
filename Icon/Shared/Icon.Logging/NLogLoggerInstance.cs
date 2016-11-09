using NLog;
using System;
using System.Collections.Generic;

namespace Icon.Logging
{

    public class NLogLoggerInstance<T> : ILogger<T> where T : class
    {
        private Logger logger;
        private string source = typeof(T).ToString();

        public NLogLoggerInstance(string instance)
        {
            logger = LogManager.GetLogger(source + "." + instance);
        }

        public void Info(string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                logger.Warn("NullOrEmpty message.");
                return;
            }

            // Size of AppLog.Message field in DB is 4000.
            IEnumerable<string> messageSplit = message.SplitByLength(4000);

            foreach (string msg in messageSplit)
            {
                logger.Info(msg);
            }
        }

        public void Warn(string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                logger.Warn("NullOrEmpty message.");
                return;
            }

            // Size of AppLog.Message field in DB is 4000.
            IEnumerable<string> messageSplit = message.SplitByLength(4000);

            foreach (string msg in messageSplit)
            {
                logger.Warn(msg);
            }
        }

        public void Error(string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                logger.Warn("NullOrEmpty message.");
                return;
            }

            // Size of AppLog.Message field in DB is 4000.
            IEnumerable<string> messageSplit = message.SplitByLength(4000);

            foreach (string msg in messageSplit)
            {
                logger.Error(msg);
            }
        }

        public void Debug(string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                logger.Warn("NullOrEmpty message.");
                return;
            }

            // Size of AppLog.Message field in DB is 4000.
            IEnumerable<string> messageSplit = message.SplitByLength(4000);

            foreach (string msg in messageSplit)
            {
                logger.Debug(msg);
            }
        }
    }

    // For classes that need to instantiate a logger directly.
    public class NLogLoggerInstance
    {
        private Logger logger;
        
        public NLogLoggerInstance(Type type, string instance)
        {
            logger = LogManager.GetLogger(type.Name + "." + instance);
        }

        public void Info(string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                logger.Warn("NullOrEmpty message.");
                return;
            }

            // Size of AppLog.Message field in DB is 4000.
            IEnumerable<string> messageSplit = message.SplitByLength(4000);

            foreach (string msg in messageSplit)
            {
                logger.Info(msg);
            }
        }

        public void Warn(string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                logger.Warn("NullOrEmpty message.");
                return;
            }

            // Size of AppLog.Message field in DB is 4000.
            IEnumerable<string> messageSplit = message.SplitByLength(4000);

            foreach (string msg in messageSplit)
            {
                logger.Warn(msg);
            }
        }

        public void Error(string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                logger.Warn("NullOrEmpty message.");
                return;
            }

            // Size of AppLog.Message field in DB is 4000.
            IEnumerable<string> messageSplit = message.SplitByLength(4000);

            foreach (string msg in messageSplit)
            {
                logger.Error(msg);
            }
        }

        public void Debug(string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                logger.Warn("NullOrEmpty message.");
                return;
            }

            // Size of AppLog.Message field in DB is 4000.
            IEnumerable<string> messageSplit = message.SplitByLength(4000);

            foreach (string msg in messageSplit)
            {
                logger.Debug(msg);
            }
        }
    }
}
