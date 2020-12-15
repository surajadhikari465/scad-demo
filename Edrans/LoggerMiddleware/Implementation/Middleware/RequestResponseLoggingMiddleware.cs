using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using LoggerMiddleware.Implementation.Providers;
using LoggerMiddleware.Extensibility.Providers;
using LoggerMiddleware.Extensibility;

namespace LoggerMiddleware
{
    public class RequestResponseLoggingMiddleware : IDisposable
    {
        #region Fields

        private readonly RequestDelegate next;
        
        LoggingMiddlewareParameters parameters;

        Dictionary<LoggingProviders, ILoggingImplementationProvider> providers;

        #endregion

        #region Constructor

        public RequestResponseLoggingMiddleware(RequestDelegate next, LoggingMiddlewareParameters parameters, Dictionary<LoggingProviders, ILoggingImplementationProvider> providers)
        {
            this.next = next;
            this.parameters = parameters;
            this.providers = providers;
        }

        #endregion

        /// <summary>
        /// Generates a new CorrelationID from System.Guid.NewGuid()
        /// </summary>
        /// <returns></returns>
        string NewCorrelationID() => Guid.NewGuid().ToString();
        
        /// <summary>
        /// Returns DateTime.Now formatted like 21/Oct/2020:03:19:09
        /// </summary>
        /// <returns></returns>
        string Now() => DateTime.Now.ToString(parameters.LogDateFormat);

        #region Invoke

        public async Task Invoke(HttpContext context, ILoggerAccessor loggerAccessor)
        {
            var request = context.Request;

            var ip = request.HttpContext.Connection.RemoteIpAddress.ToString();

            if (!request.Headers.TryGetValue(parameters.CorrelationIDHeaderName, out var id))
                id = NewCorrelationID();

            request.HttpContext.TraceIdentifier = id;

            var watch = new Stopwatch();

            watch.Start();

            var requestparams = new RequestLogParams(ip, id, Now(), request.Path);

            //Request
            foreach(var key in parameters.Providers)
            {
                if (providers.TryGetValue(key, out var provider))
                    provider.LogRequest(requestparams);
            }

            context.Response.OnStarting(state =>
            {
                var response = ((HttpContext)state).Response;

                response.Headers.Add(parameters.CorrelationIDHeaderName, id);

                var responseparams = new ResponseLogParams(ip, id, Now(), request.Path, response.StatusCode.ToString(), $"{watch.ElapsedMilliseconds}ms");

                //Response
                foreach (var key in parameters.Providers)
                {
                    if (providers.TryGetValue(key, out var provider))
                        provider.LogResponse(responseparams, loggerAccessor.GetValues());
                }

                watch.Stop();

                return Task.CompletedTask;
            }, context);

            await next(context);
        }

        public void Dispose()
        {
            foreach (var provider in providers)
                provider.Value.Dispose();
        }

        #endregion
    }
}

