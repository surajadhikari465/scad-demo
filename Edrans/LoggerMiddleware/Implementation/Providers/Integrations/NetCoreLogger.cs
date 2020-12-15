using System;
using System.Collections.Generic;
using System.Linq;
using LoggerMiddleware.Extensibility;
using Microsoft.Extensions.Logging;

namespace LoggerMiddleware.Implementation.Providers.Integrations
{ 
    public class NetCoreLogger : ILoggingImplementationProvider
    {
        ILogger<RequestResponseLoggingMiddleware> netcoreLogger;

        public NetCoreLogger(Func<ILogger<RequestResponseLoggingMiddleware>> _netcoreLogger)
        {
            netcoreLogger = _netcoreLogger();
        }

        public void LogRequest(RequestLogParams parameters)
        {
            Log(parameters.Values().BetweenConcat(" "));
        }

        public void LogResponse(ResponseLogParams parameters, Dictionary<string, string> Values = null)
        {
            var added = Values?.Select(v => $"{v.Key}={v.Value}") ?? Enumerable.Empty<string>();

            var lineToLog = parameters.Values().Concat(added).BetweenConcat(" ");

            Log(lineToLog);
        }

        private void Log(string line)
        {
            netcoreLogger.Log(LogLevel.Information, $"[INFO]{ line}");
        }

        public void Dispose()
        {
            ((IDisposable)netcoreLogger)?.Dispose();
        }
    }
}
