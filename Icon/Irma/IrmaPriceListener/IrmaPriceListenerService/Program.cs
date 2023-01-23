using IrmaPriceListenerService.Listener;
using Topshelf;

namespace IrmaPriceListenerService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var container = SimpleInjectorInitializer.InitializeContainer();
            var listener = container.GetInstance<IrmaPriceListener>();
            var serviceSettings = container.GetInstance<IrmaPriceListenerServiceSettings>();

            HostFactory.Run(r =>
            {
                r.Service<IrmaPriceListener>(s =>
                {
                    s.ConstructUsing(c => container.GetInstance<IrmaPriceListener>());
                    s.WhenStarted(cm => cm.Start());
                    s.WhenStopped(cm => cm.Stop());
                });
                r.SetDescription($"Listens to Mammoth Prices and Updates the prices in IRMA DB-{serviceSettings.IrmaRegionCode} Region");
                r.SetDisplayName($"Irma Price Listener Service - {serviceSettings.IrmaRegionCode}");
                r.SetServiceName(serviceSettings.ApplicationName);
            });
        }
    }
}
