using Icon.Common;
using SimpleInjector;
using Topshelf;
using Topshelf.Runtime;

namespace AttributePublisher.Infrastructure
{
    public class TopShelfService<TService> where TService : class, IService
    {
        private ServiceFactory<TService> ServiceFactory { get; set; }
        private string ServiceName { get; set; }
        private string ServiceDisplayName { get; set; }
        private string ServiceDescription { get; set; }
        public Container SimpleInjectorContainer { get; private set; }
        public bool ShouldUseAppConfigSettings { get; private set; }

        public TopShelfService() { }

        public TopShelfService(
            ServiceFactory<TService> serviceFactory,
            string serviceName,
            string serviceDisplayName,
            string serviceDescription)
        {
            ServiceFactory = serviceFactory;
            ServiceName = serviceName;
            ServiceDisplayName = serviceDisplayName;
            ServiceDescription = serviceDescription;
        }

        public void Run()
        {
            ServiceFactory<TService> serviceFactory = ServiceFactory;

            if(ShouldUseAppConfigSettings)
            {
                ServiceName = AppSettingsAccessor.GetStringSetting(nameof(ServiceName));
                ServiceDisplayName = AppSettingsAccessor.GetStringSetting(nameof(ServiceDisplayName));
                ServiceDescription = AppSettingsAccessor.GetStringSetting(nameof(ServiceDescription));
            }

            if(SimpleInjectorContainer != null)
            {
                serviceFactory = (c) => SimpleInjectorContainer.GetInstance<TService>();
            }

            HostFactory.Run(
                hc =>
                {
                    hc.Service<TService>(
                        s =>
                        {
                            s.ConstructUsing(serviceFactory);
                            s.WhenStarted(c => c.Start());
                            s.WhenStopped(c => c.Stop());
                        });
                    hc.SetServiceName(ServiceName);
                    hc.SetDisplayName(ServiceDisplayName);
                    hc.SetDescription(ServiceDescription);
                });
        }

        public TopShelfService<TService> UseSimpleInjector(Container container)
        {
            this.SimpleInjectorContainer = container;
            return this;
        }

        public TopShelfService<TService> UseAppConfigSettings(bool useAppConfigSettings = true)
        {
            this.ShouldUseAppConfigSettings = useAppConfigSettings;
            return this;
        }

        public static TopShelfService<TService> Default(Container container)
        {
            return new TopShelfService<TService>()
                .UseAppConfigSettings()
                .UseSimpleInjector(container);
        }
    }
}
