using LoggerMiddleware.Extensibility.Providers;
using LoggerMiddleware.Implementation.EndpointBehavior;
using LoggerMiddleware.Implementation.Providers;
using LoggerMiddleware.Implementation.Providers.Integrations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System.Collections.Generic;
using System.ServiceModel.Description;

namespace LoggerMiddleware.Extensibility
{
    public static class Extensions
    {
        /// <summary>
        /// Adds an scoped instance of ILoggerAccessor to Service Collection
        /// </summary>
        /// <param name="services"></param>
        public static void AddLoggerAccessor(this IServiceCollection services)
        {
            services.AddScoped<ILoggerAccessor, LoggerAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// Pulls the CorrelationID fron the HttpContext and adds an EndpointBehavior that includes it as a header
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="accessor"></param>
        /// <param name="correlationIDHeaderName"></param>
        public static void AddCorrelationIDBehavior(this ServiceEndpoint endpoint, IHttpContextAccessor accessor, string correlationIDHeaderName = null)
        {
            correlationIDHeaderName = correlationIDHeaderName ?? "CorrelationID";

            var correlationId = accessor.HttpContext.TraceIdentifier;

            endpoint.EndpointBehaviors.Add(new CorrelationIDEndpointBehavior(correlationId, correlationIDHeaderName));
        }

        /// <summary>
        /// Adds a middleware that provides telemetry by logging stats about the execution of each request and response with an unique CorrelationID
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        public static void AddRequestResponseLoggingMiddleware(this IApplicationBuilder app, IConfiguration configuration)
        {
            var parameters = configuration.GetSection(nameof(LoggingMiddlewareParameters))?.Get<LoggingMiddlewareParameters>() ?? LoggingMiddlewareParameters.Default;            
            
            var providers = new Dictionary<LoggingProviders, ILoggingImplementationProvider>()
            {
                { LoggingProviders.NetCore_Console, new NetCoreLogger(() => (ILogger<RequestResponseLoggingMiddleware >)app.ApplicationServices.GetService(typeof(ILogger<RequestResponseLoggingMiddleware>)))},
                { LoggingProviders.Serilog_File, new SerilogFileLogger(() => new LoggerConfiguration().WriteTo.File(parameters.LogFileName).CreateLogger(), parameters.LogToFile) },
                { LoggingProviders.Serilog_Console, new SerilogConsoleLogger(() => new LoggerConfiguration().Enrich.FromLogContext().MinimumLevel.Override("Microsoft", LogEventLevel.Information).WriteTo.Console().CreateLogger())}
            };

            app.UseMiddleware<RequestResponseLoggingMiddleware>(parameters, providers);                          
        }
    }
}
