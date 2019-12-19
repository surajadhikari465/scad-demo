using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon.DataContext
{
    public class EntityFactory : ICreateDisposableEntities
    {
        private IConfigure config;

        public EntityFactory(IConfigure config)
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
