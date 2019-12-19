using Esb.Core.Mappers;
using Esb.Core.Serializer;
using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Framework;
using Icon.Logging;
using Services.NewItem.Cache;
using Services.NewItem.Infrastructure;
using Services.NewItem.MessageBuilders;
using Services.NewItem.Models;
using Services.NewItem.Processor;
using Services.NewItem.Services;
using Irma.Framework;
using SimpleInjector;
using SimpleInjector.Diagnostics;
using System.Collections.Generic;

namespace Services.NewItem
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
            container.Register<IEsbConnectionFactory>(() => new EsbConnectionFactory { Settings = EsbConnectionSettings.CreateSettingsFromConfig("QueueName") });
            container.Register<IMessageBuilder<IEnumerable<NewItemModel>>, NewItemMessageBuilder>();
            container.Register<IUomMapper, UomMapper>();
            container.Register<ISerializer<items>, SerializerWithoutEncodingType<items>>();
            container.Register(typeof(ICommandHandler<>), new[] { typeof(NewItemApplication).Assembly });
            container.Register(typeof(IQueryHandler<,>), new[] { typeof(NewItemApplication).Assembly });
            container.Register(() => NewItemApplicationSettings.CreateFromConfig(), Lifestyle.Singleton);
            container.Register<IRegionalEmailClientFactory, RegionalEmailClientFactory>();
            container.Register<IIconItemService, IconItemService>();

            container.Register<INewItemApplication, NewItemApplication>();

            if (suppressWarnings)
            {
                container.GetRegistration(typeof(IconContext)).Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Dispose is called in the decorator.");
            }

            return container;
        }
    }
}
