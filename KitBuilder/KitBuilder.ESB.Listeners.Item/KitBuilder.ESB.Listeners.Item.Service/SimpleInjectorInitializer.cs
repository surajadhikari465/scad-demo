using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Logging;
using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Repository;
using KitBuilder.DataAccess.UnitOfWork;
using KitBuilder.ESB.Listeners.Item.Service.Commands;
using KitBuilder.ESB.Listeners.Item.Service.MessageParsers;
using KitBuilder.ESB.Listeners.Item.Service.Models;
using KitBuilder.ESB.Listeners.Item.Service.Notifiers;
using KitBuilder.ESB.Listeners.Item.Service.Services;
using Microsoft.EntityFrameworkCore;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace KitBuilder.ESB.Listeners.Item.Service
{
    public static class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<KitBuilderContext>();
            dbContextOptionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["KitBuliderDb"].ConnectionString);

            ItemListenerSettings itemListenerSettings = ItemListenerSettings.CreateFromConfig();

            var container = new Container();
            container.Options.DefaultLifestyle = Lifestyle.Singleton;


            container.Register(typeof(IRepository<>), typeof(Repository<>));
            container.Register<ICommandHandler<ItemAddOrUpdateOrRemoveCommand>, ItemAddOrUpdateOrRemoveCommandHandler>();
            container.Register<ICommandHandler<ArchiveMessageCommand>, ArchiveMessageCommandHandler>();
            container.Register(() => ListenerApplicationSettings.CreateDefaultSettings("KitBuidler Item Listener"));
            container.Register(EsbConnectionSettings.CreateSettingsFromConfig);
            container.Register<IEsbSubscriber>(() => new EsbSubscriber(EsbConnectionSettings.CreateSettingsFromConfig()));
            container.Register<IEmailClient>(EmailClient.CreateFromConfig);
            container.Register(ItemListenerSettings.CreateFromConfig);
            container.Register<IItemListenerNotifier, ItemListenerNotifier>();
            container.Register<ILogger<ItemListener>, NLogLogger<ItemListener>>();
            container.Register<IItemService, ItemService>();
            container.Register<IListenerApplication, ItemListener>(Lifestyle.Singleton);
            container.Register<IUnitOfWork>(() => new UnitOfWork(new KitBuilderContext(dbContextOptionsBuilder.Options)) );
            
            container.Register<IMessageParser<IEnumerable<ItemModel>>>(() => new ItemMessageParser(itemListenerSettings, new NLogLogger<ItemMessageParser>()));

            container.Verify();
            return container;
        }
    }
}