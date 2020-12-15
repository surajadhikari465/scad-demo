using LoggerMiddleware.Extensibility;
using System;
using System.Collections.Generic;

namespace LoggerMiddleware.Implementation.Providers
{
    public interface ILoggingImplementationProvider : IDisposable
    {
        void LogRequest(RequestLogParams parameters);
        void LogResponse(ResponseLogParams parameters, Dictionary<string, string> Values = default);
    }
}
