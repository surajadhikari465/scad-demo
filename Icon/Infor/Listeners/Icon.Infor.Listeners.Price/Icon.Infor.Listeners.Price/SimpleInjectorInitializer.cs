using Esb.Core.MessageBuilders;
using Icon.Caching;
using Icon.Common;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Price.DataAccess.Commands;
using Icon.Infor.Listeners.Price.DataAccess.Decorators;
using Icon.Infor.Listeners.Price.DataAccess.Models;
using Icon.Infor.Listeners.Price.DataAccess.Queries;
using Icon.Infor.Listeners.Price.Decorators;
using Icon.Infor.Listeners.Price.EsbFactory;
using Icon.Infor.Listeners.Price.MessageParsers;
using Icon.Infor.Listeners.Price.Models;
using Icon.Infor.Listeners.Price.Services;
using Icon.Logging;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.ConnectionBuilders;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Common.DataAccess.Decorators;
using SimpleInjector;
using SimpleInjector.Diagnostics;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Icon.Infor.Listeners.Price
{
    public static class SimpleInjectorInitializer
    {
        public static Container CreateContainer()
        {
            Container container = new Container();

            container.Register<IListenerApplication, PriceListener>();

            //var types = GetPriceServices();
            //container.RegisterCollection<IService<PriceModel>>(types);
            container.Register<IMessageParser<IEnumerable<PriceModel>>, PriceMessageParser>();
            container.Register(() => EsbConnectionSettings.CreateSettingsFromConfig());
            container.Register(() => ListenerApplicationSettings.CreateDefaultSettings("Infor Price Listener"));
            container.Register<IEsbSubscriber, EsbSubscriber>();
            container.Register<IEmailClient, EmailClient>();
            container.Register(() => EmailClientSettings.CreateFromConfig());
            container.Register(typeof(ILogger<>), typeof(NLogLogger<>));
            container.Register(typeof(ICache), typeof(Cache), Lifestyle.Singleton);
            container.Register(typeof(ICachePolicy<>), new[] { typeof(ICachePolicy<>).Assembly }, Lifestyle.Singleton);
            container.Register<IEsbConnectionSettingsFactory, EsbConnectionSettingsFactory>();
            container.Register<IEsbConnectionFactory, EsbConnectionFactory>();
            container.Register(typeof(IMessageBuilder<>), new[] { typeof(PriceListener).Assembly });
            container.Register(typeof(IService<>), typeof(PriceServiceHandler));
            
            // price service decorators
            container.RegisterDecorator(typeof(IService<>), typeof(AddValidationPriceServiceDecorator));
            container.RegisterDecorator(typeof(IService<>), typeof(DeleteValidationPricesServiceDecorator));
            container.RegisterDecorator(typeof(IService<>), typeof(ReplaceValidationPriceServiceDecorator));
            container.RegisterDecorator(typeof(IService<>), typeof(ErrorExceptionServiceDecorator<>));
            container.RegisterDecorator(typeof(IService<>), typeof(ArchivePriceServiceDecorator));

            // Data Access Registration
            var dataAccessAssembly = Assembly.Load("Icon.Infor.Listeners.Price.DataAccess");
            container.Register(typeof(ICommandHandler<>), new[] { dataAccessAssembly });
            container.Register(typeof(IQueryHandler<,>), new[] { dataAccessAssembly });
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(DbProviderCommandHandlerDecorator<>));
            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(CachingQueryHandlerDecorator<GetCurrenciesParameters, IEnumerable<Currency>>));
            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(DbProviderQueryHandlerDecorator<,>));
            container.RegisterDecorator(typeof(ICommandHandler<ReplacePricesCommand>), typeof(TransactionCommandHandlerDecorator<ReplacePricesCommand>)); // delete + add need a transaction
            container.Register<IDbProvider, SqlDbProvider>(Lifestyle.Singleton);
            container.Register<IConnectionBuilder>(() => new ConnectionBuilder("Mammoth"), Lifestyle.Singleton);

            // Set Cache Policy based on AppSetting
            var currencyPolicy = SetupCachePolicy<GetCurrenciesParameters>(appSetting: "CurrencyQueryCacheExpirationInMinutes", defaultExpiry: 720);
            container.RegisterSingleton<ICachePolicy<GetCurrenciesParameters>>(currencyPolicy);
            var localePolicy = SetupCachePolicy<GetLocalesByBusinessUnitsParameters>(appSetting: "LocaleQueryCacheExpirationInMinutes", defaultExpiry: 30);
            container.RegisterSingleton<ICachePolicy<GetLocalesByBusinessUnitsParameters>>(localePolicy);

            // Disable diagnostic warnings
            Registration registration = container.GetRegistration(typeof(IEsbSubscriber)).Registration;
            registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent,
                "Code calls dispose");

            return container;
        }

        private static IEnumerable<Type> GetPriceServices()
        {
            return new List<Type>
            {
                //typeof(AddPricesServiceHandler),
                //typeof(DeletePricesService),
                //typeof(ReplacePricesService),
                //typeof(SendFailedPricesToEsbService)
                //typeof(SendPricesToEsbService),
                //typeof(ArchivePriceService)
            };
        }

        private static CachePolicy<T> SetupCachePolicy<T>(string appSetting, double defaultExpiry)
        {
            double timeExpiryInMinutes;
            if (!Double.TryParse(AppSettingsAccessor.GetStringSetting(appSetting, false), out timeExpiryInMinutes))
            {
                timeExpiryInMinutes = defaultExpiry;
            }
            var cachePolicy = new CachePolicy<T> { AbsoluteExpiration = DateTime.Now.AddMinutes(timeExpiryInMinutes) };
            return cachePolicy;
        }
    }
}
