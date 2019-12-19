using OOSCommon.DataContext;

namespace OOS.Model
{
    public class OOSEntitiesFactory : IOOSEntitiesFactory
    {
        private IConfigurator config;

        public OOSEntitiesFactory(IConfigurator config)
        {
            this.config = config;
        }

        public IDisposableOOSEntities New()
        {
            var oosRepositoryConnectionString = config.GetEFConnectionString();
            return new DisposableOOSEntities(oosRepositoryConnectionString);
        }

    }


}
