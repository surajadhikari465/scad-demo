using OOSCommon;

namespace OOS.Model
{
    public class LogService : ILogService
    {
        private IOOSLog logger;
        private IConfigurator config;

        public LogService(IConfigurator config)
        {
            this.config = config;
        }

        public IOOSLog GetLogger()
        {
            if (logger == null)
            {
                var basePath = config.GetLoggerBasePath();
                var loggerName = config.GetLoggerName();

                logger = new OOSLog(loggerName, basePath, config.GetSessionID, null);
                               
            }
            return logger;
        }

    }
}
