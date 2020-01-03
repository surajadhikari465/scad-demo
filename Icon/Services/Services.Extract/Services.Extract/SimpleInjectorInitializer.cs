using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Logging;
using Services.Extract.Infrastructure.Esb;
using SimpleInjector;
using OpsgenieAlert;
using Services.Extract.Credentials;
using Icon.Common.DataAccess;
using Services.Extract.DataAccess.Commands;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SimpleInjector.Diagnostics;

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
            container.Register(typeof(ICommandHandler<>), typeof(UpdateJobLastRunEndCommand).Assembly);
            container.Register<IDbConnection>(() => new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString));
            
            
            container.Register<ExtractService>();

            container.GetRegistration(typeof(IDbConnection))
                .Registration
                .SuppressDiagnosticWarning(
                    DiagnosticType.DisposableTransientComponent,
                    "Dispose not needed");

            container.Verify();
            return container;

        }
    }
}
