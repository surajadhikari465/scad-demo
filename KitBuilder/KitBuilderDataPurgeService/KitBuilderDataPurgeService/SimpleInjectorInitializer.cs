using System.Configuration;
using Icon.Common.DataAccess;
using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Repository;
using KitBuilder.DataAccess.UnitOfWork;
using KitBuilder.DataPurge.Service.Commands;
using KitBuilder.DataPurge.Service.Services;
using Microsoft.EntityFrameworkCore;
using SimpleInjector;

namespace KitBuilder.DataPurge.Service
{
    public static class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<KitBuilderContext>();
            dbContextOptionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["KitBuliderDb"].ConnectionString);
            var container = new Container();
            container.Options.DefaultLifestyle = Lifestyle.Singleton;

            container.Register(typeof(IRepository<>), typeof(Repository<>));
			container.Register<ICommandHandler<PurgeDataCommand>, PurgeDataCommandHandler>();
            container.Register<IUnitOfWork>(() => new UnitOfWork(new KitBuilderContext(dbContextOptionsBuilder.Options)) );
			container.Register<IPurgeApplicationService, PurgeApplicationService>();
			container.Register<IDataPurgeService, DataPurgeService>();

			container.Verify();
            return container;
        }
    }
}