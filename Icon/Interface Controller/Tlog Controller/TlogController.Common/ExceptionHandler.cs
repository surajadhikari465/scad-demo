using Icon.Logging;
using System;
using System.Reflection;


namespace TlogController.Common
{
    public static class ExceptionHandler<T> where T : class
    {
        public static ILogger<T> logger { get; set; }

        public static void HandleException(string message, Exception ex, Type callingClass, MethodBase callingMethod)
        {
            logger.Error(String.Format("{0}  Exception details - Method: {1}  Class: {2}  Exception: {3}",
                message,
                callingMethod.Name,
                callingClass.Name,
                ex.ToString()
            ));
        }

        public static bool IsTransientError(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
