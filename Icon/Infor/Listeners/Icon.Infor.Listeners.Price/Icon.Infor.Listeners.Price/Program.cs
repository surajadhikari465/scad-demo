using Icon.Common;
using Icon.Esb.ListenerApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Icon.Infor.Listeners.Price
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(r =>
            {
                r.Service<IListenerApplication>(s =>
                {
                    s.ConstructUsing(() => SimpleInjectorInitializer.CreateContainer().GetInstance<IListenerApplication>());
                    s.WhenStarted(cm => cm.Run());
                    s.WhenStopped(cm => cm.Close());
                });
                r.SetServiceName(AppSettingsAccessor.GetStringSetting("ServiceName"));
                r.SetDisplayName(AppSettingsAccessor.GetStringSetting("ServiceDisplayName"));
                r.SetDescription(AppSettingsAccessor.GetStringSetting("ServiceDescription"));
            });
        }
    }
}
