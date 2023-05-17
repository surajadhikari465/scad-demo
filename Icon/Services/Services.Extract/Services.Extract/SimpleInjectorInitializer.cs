using Icon.Common.Email;
using Icon.Logging;
using SimpleInjector;
using OpsgenieAlert;
using Services.Extract.Credentials;
using Icon.Common.DataAccess;
using Services.Extract.DataAccess.Commands;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SimpleInjector.Diagnostics;
using Services.Extract.Models;
using Services.Extract.Message.Parser;
using Wfm.Aws.ExtendedClient.Listener.SQS.Settings;
using Wfm.Aws.ExtendedClient.SQS;
using Wfm.Aws.S3.Settings;
using Wfm.Aws.SQS.Settings;
using Wfm.Aws.S3;
using Wfm.Aws.SQS;
using Wfm.Aws.ExtendedClient.Serializer;
using Wfm.Aws.ExtendedClient.Listener.SQS;

namespace Services.Extract
{
    public static class SimpleInjectorInitializer
    {
        public static Container Init()
        {
            var container = new Container();
            container.RegisterSingleton(() => S3FacadeSettings.CreateSettingsFromConfig());
            container.RegisterSingleton(() => SQSFacadeSettings.CreateSettingsFromConfig());
            container.RegisterSingleton(() => SQSExtendedClientListenerSettings.CreateSettingsFromConfig());
            container.Register<IEmailClient>(EmailClient.CreateFromConfig);
            container.Register(typeof(ILogger<>), typeof(NLogLogger<>));
            container.Register<ExtractServiceListener>();
            container.RegisterSingleton<IS3Facade, S3Facade>();
            container.RegisterSingleton<ISQSFacade, SQSFacade>();
            container.RegisterSingleton<IExtendedClientMessageSerializer, S3EventMessageSerializer>();
            container.Register<SQSExtendedClientListener<ExtractServiceListener>, ExtractServiceListener>();
            container.RegisterSingleton<ISQSExtendedClient, SQSExtendedClient>();
            container.RegisterSingleton<IOpsgenieAlert, OpsgenieAlert.OpsgenieAlert>();
            container.RegisterSingleton<ISFtpCredentialsCache, SFtpCredentialsCache>();
            container.RegisterSingleton<IS3CredentialsCache, S3CredentialsCache>();
            container.RegisterSingleton<IActiveMqCredentialsCache, ActiveMqCredentialCache>();
            container.Register<ICredentialsCacheManager, CredentialsCacheManager>();
            container.Register<IFileDestinationCache, FileDestinationsCache>();
            container.Register(typeof(ICommandHandler<>), typeof(UpdateJobLastRunEndCommand).Assembly);
            container.Register<IDbConnection>(() => new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString));
            container.Register<IMessageParser<JobSchedule>, JobScheduleMessageParser>();
            container.Register<IExtractJobConfigurationParser, ExtractJobConfigurationParser>();
            container.Register<IExtractJobRunnerFactory, ExtractJobRunnerFactory>();
            
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
