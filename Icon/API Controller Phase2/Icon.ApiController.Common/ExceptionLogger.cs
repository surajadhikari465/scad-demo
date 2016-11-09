using Icon.Logging;
using System;
using System.Reflection;

namespace Icon.ApiController.Common
{
    public class ExceptionLogger<T> where T : class
    {
        private ILogger<T> logger;

        public ExceptionLogger(ILogger<T> logger)
        {
            this.logger = logger;
        }

        public void LogException(Exception ex, Type callingClass, MethodBase callingMethod)
        {
            logger.Error(String.Format("Exception in Method: {0}  Class: {1}.  Exception: {2}",
                callingMethod.Name,
                callingClass.Name,
                ex.ToString()
            ));
        }

        public void LogException(string message, Exception ex, Type callingClass, MethodBase callingMethod)
        {
            logger.Error(String.Format("{0}  Method: {1}.  Class: {2}.  Exception: {3}.",
                message,
                callingMethod.Name,
                callingClass.Name,
                ex.ToString()
            ));
        }
    }
}
