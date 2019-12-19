using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Irma.Framework;
using Services.NewItem.Cache;
using Services.NewItem.Infrastructure;
using Services.NewItem.Processor;
using Services.NewItem.Services;
using SimpleInjector;
using SimpleInjector.Diagnostics;

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
