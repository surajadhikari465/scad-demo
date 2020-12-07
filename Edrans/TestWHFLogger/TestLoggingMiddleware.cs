using NUnit.Framework;
using LoggerMiddleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using Moq;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Collections.Generic;
using LoggerMiddleware.Extensibility.Providers;
using LoggerMiddleware.Implementation.Providers;
using LoggerMiddleware.Implementation.Providers.Integrations;
using Serilog;
using Serilog.Events;

namespace TestWHFLogger
{
    public class Tests
    {
        private Mock<HttpContext> contextMock;
        private Mock<ILogger<RequestResponseLoggingMiddleware>> loggerMock;

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();

            return config;
        }

        [SetUp]
        public void Setup()
        {
            loggerMock = new Mock<ILogger<RequestResponseLoggingMiddleware>>();
            var requestMessage = new Mock<HttpRequestMessage>(HttpMethod.Get, "http://stackoverflow");
            requestMessage.Object.Headers.Add("deviceId", "1234");
            requestMessage.Object.Headers.Add("CorrelationID", "52121234");


            var responseMock = new Mock<HttpResponse>();
            var requestMock = new Mock<HttpRequest>();
            requestMock.Setup(x => x.Scheme).Returns("http");
            requestMock.Setup(x => x.Host).Returns(new HostString("localhost"));
            requestMock.Setup(x => x.Path).Returns(new PathString("/test"));
            requestMock.Setup(x => x.PathBase).Returns(new PathString("/"));
            requestMock.Setup(x => x.Method).Returns("GET");
            requestMock.Setup(x => x.Body).Returns(new MemoryStream());
            requestMock.Setup(x => x.QueryString).Returns(new QueryString("?param1=2"));
            requestMock.Setup(x => x.HttpContext).Returns((new Mock<HttpContext>()).Object);
            requestMock.Setup(x => x.HttpContext.Connection).Returns((new Mock<ConnectionInfo>()).Object);
            requestMock.Setup(x => x.HttpContext.Connection.RemoteIpAddress).Returns(System.Net.IPAddress.Parse("0.0.0.0"));
            requestMock.SetupGet(req => req.Headers).Returns((new Mock<IHeaderDictionary>()).Object);

            contextMock = new Mock<HttpContext>();
            contextMock.Setup(x => x.Request).Returns(requestMock.Object);
            contextMock.Setup(x => x.Response).Returns(responseMock.Object);
        }

        [Test]
        public async Task Is_Logging_Request_On_File()
        {
            LoggingMiddlewareParameters parameters = InitConfiguration().GetSection("LoggingMiddlewareParameters").Get<LoggingMiddlewareParameters>();

            var providers = new Dictionary<LoggingProviders, ILoggingImplementationProvider>()
            {
                { LoggingProviders.Serilog_File, new SerilogFileLogger(() => new LoggerConfiguration().WriteTo.File(parameters.LogFileName).CreateLogger(), parameters.LogToFile) },                
            };

            var logRequestMiddleware = new RequestResponseLoggingMiddleware(next: (innerHttpContext) => Task.FromResult(0),
                parameters: parameters,
                providers: providers);

            await logRequestMiddleware.Invoke(contextMock.Object);
            var fs = new FileStream(parameters.LogFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var fr = new StreamReader(fs);
            var lines = fr.ReadToEnd().Split('\n');
            var lastLine = lines[lines.Length - 2];

            Assert.IsTrue(lastLine.StartsWith(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm")) && lastLine.Contains(" /test"));
        }


        [Test]
        public void Is_Logging_Parameterizable()
        {
            LoggingMiddlewareParameters parameters = LoggingMiddlewareParameters.Default;

            Assert.IsTrue(parameters.LogFileName == "log.txt"
                && parameters.LogToFile == true
                && parameters.CorrelationIDHeaderName == "CorrelationID"
                && parameters.LogDateFormat == "dd/MMM/yyyy:hh:mm:ss"
                && parameters.Providers.SequenceEqual(new[] { LoggingProviders.NetCore_Console, LoggingProviders.Serilog_File }));
        } 

    }
}