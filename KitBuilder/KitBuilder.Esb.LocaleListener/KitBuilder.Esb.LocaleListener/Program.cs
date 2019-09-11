using Icon.Common;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using KitBuilder.Esb.LocaleListener;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Mammoth.Esb.LocaleListener
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var container = SimpleInjectorInitializer.InitializeContainer();

            HostFactory.Run(r =>
            {
                r.Service<IListenerApplication>(s =>
                {
                    s.ConstructUsing(c => container.GetInstance<IListenerApplication>());
                    s.WhenStarted(c => c.Run());
                    s.WhenStopped(c => c.Close());
                });

                r.SetDescription(AppSettingsAccessor.GetStringSetting("ServiceDescription"));
                r.SetDisplayName(AppSettingsAccessor.GetStringSetting("ServiceDisplayName"));
                r.SetServiceName(AppSettingsAccessor.GetStringSetting("ServiceName"));
            });
        }
    }
}
