using LoggerMiddleware.Extensibility;
using System.Collections.Generic;

namespace LoggerMiddleware.Implementation.Providers
{
    public interface ILoggingImplementationProvider
    {
        void LogRequest(RequestLogParams parameters);
        void LogResponse(ResponseLogParams parameters, Dictionary<string, string> Values = default);
    }
}
