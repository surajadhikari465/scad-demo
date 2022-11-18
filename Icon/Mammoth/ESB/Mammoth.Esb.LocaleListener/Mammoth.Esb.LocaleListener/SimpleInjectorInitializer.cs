using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Dvs;
using Icon.Dvs.MessageParser;
using Amazon.S3;
using Amazon.SQS;
using Icon.Dvs.Subscriber;
using Icon.Logging;
using Icon.Shared.DataAccess.Dapper.ConnectionBuilders;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Shared.DataAccess.Dapper.Decorators;
using Mammoth.Esb.LocaleListener.Commands;
using Mammoth.Esb.LocaleListener.MessageParsers;
using Mammoth.Esb.LocaleListener.Models;
using SimpleInjector;
using SimpleInjector.Diagnostics;
using System.Collections.Generic;

namespace Mammoth.Esb.LocaleListener
{
    public static class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {
            var container = new Container();
            var listenerSettings = DvsListenerSettings.CreateSettingsFromConfig();

            container.RegisterSingleton<MammothLocaleListener>();
            container.RegisterSingleton<DvsListenerSettings>(() => listenerSettings);
            container.RegisterSingleton(() => DvsClientUtil.GetS3Client(listenerSettings));
            container.RegisterSingleton(() => DvsClientUtil.GetSqsClient(listenerSettings));
            container.RegisterSingleton<IDvsSubscriber, DvsSqsSubscriber>();
            container.RegisterSingleton<IEmailClient>(() => EmailClient.CreateFromConfig());
            container.RegisterSingleton<ILogger<MammothLocaleListener>, NLogLogger<MammothLocaleListener>>();
            container.RegisterSingleton<IMessageParser<List<LocaleModel>>, LocaleMessageParser>();
            container.RegisterSingleton<ICommandHandler<AddOrUpdateLocalesCommand>, AddOrUpdateLocalesCommandHandler>();
            container.RegisterSingleton<IConnectionBuilder>(() => new ConnectionBuilder("Mammoth"));
            container.RegisterSingleton<IDbProvider, SqlDbProvider>();

            container.RegisterDecorator<ICommandHandler<AddOrUpdateLocalesCommand>, DbProviderCommandHandlerDecorator<AddOrUpdateLocalesCommand>>(Lifestyle.Singleton);
            Registration amazonS3Registration = container.GetRegistration(typeof(IAmazonS3)).Registration;
            amazonS3Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing IAmazonS3 is taken care of by the application.");
            Registration amazonSqsRegistration = container.GetRegistration(typeof(IAmazonSQS)).Registration;
            amazonSqsRegistration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing IAmazonSQS is taken care of by the application.");

            return container;
        }
    }
}
