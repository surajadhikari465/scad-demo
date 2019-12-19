using System;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Logging;
using OpsgenieAlert;
using Services.Extract.Credentials;
using Services.Extract.Models;

namespace Services.Extract
{
    public class ExtractServiceListener : ListenerApplication<ExtractServiceListener, ExtractServiceListenerApplicationSettings>
    {
        private readonly ILogger<ExtractJobRunner> ExtractJobLogger;
        private readonly IOpsgenieAlert OpsGenieAlert;
        private readonly ICredentialsCacheManager CredentialsCacheManager;

        public ExtractServiceListener(
            ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<ExtractServiceListener> serviceLogger,
            ILogger<ExtractJobRunner> extractJoblogger,
            ICredentialsCacheManager credentialsCacheManager,
            IOpsgenieAlert opsGenieAlert
        ) : base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, serviceLogger)
        {
            ExtractJobLogger = extractJoblogger;
            OpsGenieAlert = opsGenieAlert;
            CredentialsCacheManager = credentialsCacheManager;
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            var jobSchedule = new JobScheduleMessageParser().ParseMessage(args.Message);
            var extractJobConfig = new ExtractJobConfigurationParser().Parse(jobSchedule.XmlObject);
            logger.Debug($"Executing Job: {jobSchedule.JobName}");

            var runner = new ExtractJobRunner(ExtractJobLogger, OpsGenieAlert, CredentialsCacheManager);
            try
            {
                runner.Run(extractJobConfig);
            }
            catch (Exception ex)
            {
                logger.Error($"Job Failed: {jobSchedule.JobName}");
                logger.Error(ex.Message);
            }

        }
    }
}