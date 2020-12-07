using LoggerMiddleware.Extensibility.Providers;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace LoggerMiddleware
{
    public class LoggingMiddlewareParameters
    {
        public bool LogToFile { get; set; }
        public string LogFileName { get; set; }
        public string CorrelationIDHeaderName { get; set; }
        public string LogDateFormat { get; set; }
        public LoggingProviders[] Providers { get; set; }

        public LoggingMiddlewareParameters()
        {
        }

        public LoggingMiddlewareParameters(bool logToFile, string logFileName, string correlationIdHeaderName, string logDateFormat, LoggingProviders[] providers)
        {
            LogToFile = logToFile; 
            LogFileName = logFileName;
            CorrelationIDHeaderName = correlationIdHeaderName; 
            LogDateFormat = logDateFormat;
            Providers = providers;
        }

        public static LoggingMiddlewareParameters Default =>
            new LoggingMiddlewareParameters(
                logToFile: true, 
                logFileName: "log.txt", 
                correlationIdHeaderName: "CorrelationID",
                logDateFormat: "dd/MMM/yyyy:hh:mm:ss", 
                providers: new [] { LoggingProviders.NetCore_Console, LoggingProviders.Serilog_File });
    }
}
