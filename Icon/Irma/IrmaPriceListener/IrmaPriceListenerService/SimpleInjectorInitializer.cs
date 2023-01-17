using Icon.DbContextFactory;
using Irma.Framework;
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
            container.RegisterSingleton<IDbContextFactory<IrmaContext>, IrmaDbContextFactory>();
            container.RegisterSingleton<IMessageArchiver, MessageArchiver>();
            container.RegisterSingleton<IIrmaPriceDAL, IrmaPriceDAL>();
            container.RegisterSingleton<IrmaPriceListener>();
            return container;
        }
    }
}
