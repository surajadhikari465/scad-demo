using System;
using Icon.Common;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using ServiceStack.Text;
using Topshelf;
using SimpleInjector;
using NLog;

namespace KitBuilder.ESB.Listeners.Item.Service
{
    internal class Program
    {

        private static string serviceDescription;
        private static string serviceDisplayName;
        private static string serviceName;
        private static Container container;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
           container = SimpleInjectorInitializer.InitializeContainer();
           serviceDescription = AppSettingsAccessor.GetStringSetting("ServiceDescription", string.Empty);
           serviceDisplayName = AppSettingsAccessor.GetStringSetting("ServiceDisplayName", string.Empty);
           serviceName = AppSettingsAccessor.GetStringSetting("ServiceName", string.Empty);

            ValidateSettings();

            HostFactory.Run(r =>
            {
                r.Service<IListenerApplication>(s =>
                {
                    s.ConstructUsing(c => container.GetInstance<IListenerApplication>());
                    s.WhenStarted(c => c.Run());
                    s.WhenStopped(c => c.Close());
                });

                r.SetDescription(serviceDescription);
                r.SetDisplayName(serviceDisplayName);
                r.SetServiceName(serviceName);
            });
        }

        private static void ValidateSettings()
        {
            if (serviceDescription == string.Empty) throw new ApplicationException("ServiceDescription must be configured in app.settings before the service can run.");
            if (serviceDisplayName == string.Empty) throw new ApplicationException("ServiceDisplayName must be configured in app.settings before the service can run.");
            if (serviceName == string.Empty) throw new ApplicationException("ServiceName must be configured in app.settings before the service can run.");
           
        }
    }
}