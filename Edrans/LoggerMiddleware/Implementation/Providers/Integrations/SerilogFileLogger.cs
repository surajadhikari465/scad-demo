using System;
using System.Collections.Generic;
using System.Linq;
using LoggerMiddleware.Extensibility;

namespace LoggerMiddleware.Implementation.Providers.Integrations
{
    public class SerilogFileLogger : ILoggingImplementationProvider
    {
        Lazy<Serilog.ILogger> logger;
        bool logToFile;

        public SerilogFileLogger(Func<Serilog.ILogger> _logger, bool _logToFile)
        {
            logger = new Lazy<Serilog.ILogger>(_logger);
            logToFile = _logToFile;
        }

        public void Dispose()
        {
            ((IDisposable)logger.Value)?.Dispose();
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
            if(logToFile)
                logger.Value.Information(line);
        }
    }
}
