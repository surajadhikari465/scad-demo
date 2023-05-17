using Icon.Common;
using Topshelf;

namespace Icon.Services.ItemPublisher.Application
{
    /// <summary>
    /// Enacapulates the TopShelf logic for hosting the Windows service
    /// </summary>
    public class ApplicationRunner
    {
        public void Run()
        {
            Host host = HostFactory.New(r =>
            {
                r.Service<IItemPublisherApplication>(s =>
                {
                    s.ConstructUsing(c => SimpleInjectorInitializer.InitializeContainer().GetInstance<IItemPublisherApplication>());

                    s.WhenStarted(cm => cm.Start());
                    s.WhenStopped(cm => cm.Stop());
                });
                r.SetServiceName(AppSettingsAccessor.GetStringSetting("ServiceName", "IconItemPublisherService"));
                r.SetDisplayName(AppSettingsAccessor.GetStringSetting("ServiceDisplayName", "Icon Item Publisher Service"));
                r.SetDescription(AppSettingsAccessor.GetStringSetting("ServiceDescription", "Service that publishes Icon item changes to DVS"));
                r.UseNLog();
            });

            host.Run();
        }
    }
}