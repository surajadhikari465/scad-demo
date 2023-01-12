using IrmaPriceListenerService.Archive;
using IrmaPriceListenerService.DataAccess;
using IrmaPriceListenerService.Listener;
using SimpleInjector;

namespace IrmaPriceListenerService
{
    public static class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {
            var container = new Container();
            container.RegisterSingleton<IMessageArchiver, MessageArchiver>();
            container.RegisterSingleton<IIrmaPriceDAL, IrmaPriceDAL>();
            container.RegisterSingleton<IrmaPriceListener>();
            return container;
        }
    }
}
