using Icon.Common;
using Topshelf;

namespace Services.NewItem
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(r =>
            {
                r.Service<INewItemApplication>(s =>
                {
                    s.ConstructUsing(c => SimpleInjectorInitializer.InitializeContainer().GetInstance<INewItemApplication>());
                    s.WhenStarted(cm => cm.Start());
                    s.WhenStopped(cm => cm.Stop());
                });
                r.SetServiceName(AppSettingsAccessor.GetStringSetting("ServiceName"));
                r.SetDisplayName(AppSettingsAccessor.GetStringSetting("ServiceDisplayName"));
                r.SetDescription(AppSettingsAccessor.GetStringSetting("ServiceDescription"));
            });
        }
    }
}
