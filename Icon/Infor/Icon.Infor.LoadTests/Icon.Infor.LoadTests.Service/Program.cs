using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;
using System.Web.Http;
using Topshelf;
using Icon.Common;
using System.Security.Principal;

namespace Icon.Infor.LoadTests.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(r =>
            {
                r.Service<LoadTestServer>(s =>
                {
                    s.ConstructUsing(c => new LoadTestServer());
                    s.WhenStarted(cm => cm.Start());
                    s.WhenStopped(cm => cm.Stop());
                });
                r.SetDescription(AppSettingsAccessor.GetStringSetting("ServiceDescription"));
                r.SetDisplayName(AppSettingsAccessor.GetStringSetting("ServiceDisplayName"));
                r.SetServiceName(AppSettingsAccessor.GetStringSetting("ServiceName"));
            });
        }
    }
}
