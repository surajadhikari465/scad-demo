using Icon.Common;
using Icon.Logging;
using ServiceStack.Text;
using System;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace Icon.Infor.LoadTests.Service
{
    public class LoadTestServer
    {
        private ILogger logger;
        private HttpSelfHostServer server;

        public LoadTestServer()
        {
            logger = new NLogLogger(this.GetType());
        }

        public void Start()
        {
            string baseAddress = GetBaseAddress();

            var config = new HttpSelfHostConfiguration(baseAddress);

            config.Routes.MapHttpRoute(
                "action",
                "api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional });

            server = new HttpSelfHostServer(config);

            logger.Info(new { Message = "Starting server", BaseAddress = baseAddress, UserName = WindowsIdentity.GetCurrent().Name }.Dump());

            try
            {
                var task = server.OpenAsync();
                task.Wait();
            }
            catch(Exception ex)
            {
                logger.Error(new { Message = "Unexpected error occurred when starting server", Exception = ex.StackTrace }.Dump());
                throw;
            }

            logger.Info(new { Message = "Server started" }.Dump());
        }

        public string GetBaseAddress()
        {
            try
            {
                return AppSettingsAccessor.GetStringSetting("BaseAddress");
            }
            catch (Exception ex)
            {
                logger.Error(new { Message = "Error reading BaseAddess config", Exception = ex.ToString() }.Dump());
                throw;
            }
        }

        public bool Stop()
        {
            server.CloseAsync().Wait();

            return true;
        }
    }
}
