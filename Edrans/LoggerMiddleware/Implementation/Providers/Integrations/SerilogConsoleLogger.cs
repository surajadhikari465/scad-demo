using LoggerMiddleware.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoggerMiddleware.Implementation.Providers.Integrations
{
    public class SerilogConsoleLogger : ILoggingImplementationProvider
    {
        Lazy<Serilog.ILogger> logger;

        public SerilogConsoleLogger(Func<Serilog.ILogger> _logger)
        {
            logger = new Lazy<Serilog.ILogger>(_logger);
        }

        public void LogRequest(RequestLogParams parameters)
        {            
            Log(parameters.Values().BetweenConcat(" "));
        }

        public void LogResponse(ResponseLogParams parameters, Dictionary<string, string> Values)
        {
            var added = Values?.Select(v => $"{v.Key}={v.Value}") ?? Enumerable.Empty<string>();

            var lineToLog = parameters.Values().Concat(added).BetweenConcat(" ");

            Log(lineToLog);
        }

        private void Log(string line)
        {
            logger.Value.Information(line);
        }
        public void Dispose()
        {
            var dis = ((IDisposable)logger.Value);
            System.Diagnostics.Debug.WriteLine(dis);
            dis.Dispose();
        }
    }
}
