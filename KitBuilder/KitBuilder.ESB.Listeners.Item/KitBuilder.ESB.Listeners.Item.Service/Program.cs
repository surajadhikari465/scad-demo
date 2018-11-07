using Icon.Common;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using ServiceStack.Text;
using Topshelf;

namespace KitBuilder.ESB.Listeners.Item.Service
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var container = SimpleInjectorInitializer.InitializeContainer();
            container.GetInstance<EsbConnectionSettings>().PrintDump();

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