using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Logging;
using Services.Extract.Infrastructure.Esb;
using SimpleInjector;
using OpsgenieAlert;
using Services.Extract.Credentials;

namespace Services.Extract
{
    public static class SimpleInjectorInitializer
    {
        public static Container Init()
        {

            var container = new Container();
            var esbSettings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("Sb1EmsConnection");
            container.Register(() => ListenerApplicationSettings.CreateDefaultSettings("Extract Service"));
            container.Register(() => esbSettings);
            container.Register<IEmailClient>(EmailClient.CreateFromConfig);
            container.Register(typeof(ILogger<>), typeof(NLogLogger<>));
            container.Register<IEsbSubscriber>(() => new Sb1EsbConsumer(esbSettings), Lifestyle.Singleton);
            container.Register<ExtractServiceListener>();
            container.RegisterSingleton<IOpsgenieAlert, OpsgenieAlert.OpsgenieAlert>();
            container.RegisterSingleton<ISFtpCredentialsCache, SFtpCredentialsCache>();
            container.RegisterSingleton<IS3CredentialsCache, S3CredentialsCache>();
            container.Register<ICredentialsCacheManager, CredentialsCacheManager>();

            container.Register<ExtractService>();

            container.Verify();
            return container;

        }
    }
}
