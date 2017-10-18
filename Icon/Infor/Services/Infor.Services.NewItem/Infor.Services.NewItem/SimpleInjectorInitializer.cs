using Esb.Core.Mappers;
using Esb.Core.Serializer;
using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Framework;
using Icon.Logging;
using Infor.Services.NewItem.Cache;
using Infor.Services.NewItem.Infrastructure;
using Infor.Services.NewItem.MessageBuilders;
using Infor.Services.NewItem.Models;
using Infor.Services.NewItem.Notifiers;
using Infor.Services.NewItem.Processor;
using Infor.Services.NewItem.Services;
using Infor.Services.NewItem.Validators;
using Irma.Framework;
using SimpleInjector;
using SimpleInjector.Diagnostics;
using System.Collections.Generic;

namespace Infor.Services.NewItem
{
    public static class SimpleInjectorInitializer
    {
        public static Container InitializeContainer(bool suppressWarnings = true)
        {
            Container container = new Container();

            container.Register(typeof(ILogger<>), typeof(NLogLogger<>));
            container.Register<IRenewableContext<IconContext>>(() => new GlobalContext<IconContext>(), Lifestyle.Singleton);
            container.Register<IRenewableContext<IrmaContext>>(() => new RegionalRenewableContext(), Lifestyle.Singleton);
            container.Register<IIconCache, IconCache>();
            container.Register<INewItemProcessor, NewItemProcessor>();
            container.Register<IInforItemService, InforItemService>();
            container.Register<IEsbConnectionFactory>(() => new EsbConnectionFactory { Settings = EsbConnectionSettings.CreateSettingsFromConfig("QueueName") });
            container.Register<IMessageBuilder<IEnumerable<NewItemModel>>, NewItemMessageBuilder>();
            container.Register<IUomMapper, UomMapper>();
            container.Register<ISerializer<items>, SerializerWithoutEncodingType<items>>();
            container.Register<ICollectionValidator<NewItemModel>, NewItemModelCollectionValidator>();
            container.Register(typeof(ICommandHandler<>), new[] { typeof(InforNewItemApplication).Assembly });
            container.Register(typeof(IQueryHandler<,>), new[] { typeof(InforNewItemApplication).Assembly });
            container.Register(() => InforNewItemApplicationSettings.CreateFromConfig(), Lifestyle.Singleton);
            container.Register<INewItemNotifier, NewItemNotifier>();
            container.Register<NewItemNotifierSettings>(() => NewItemNotifierSettings.CreateFromConfig());
            container.Register<IRegionalEmailClientFactory, RegionalEmailClientFactory>();
            container.Register<IIconItemService, IconItemService>();

            container.Register<IInforNewItemApplication, InforNewItemApplication>();

            if (suppressWarnings)
            {
                container.GetRegistration(typeof(IconContext)).Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Dispose is called in the decorator.");
            }

            return container;
        }
    }
}
