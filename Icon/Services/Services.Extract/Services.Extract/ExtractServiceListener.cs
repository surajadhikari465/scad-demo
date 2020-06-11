﻿using System;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Logging;
using OpsgenieAlert;
using Services.Extract.Credentials;
using Services.Extract.DataAccess.Commands;
using Services.Extract.Models;

namespace Services.Extract
{
    public class ExtractServiceListener : ListenerApplication<ExtractServiceListener, ExtractServiceListenerApplicationSettings>
    {
        private readonly ILogger<ExtractJobRunner> ExtractJobLogger;
        private readonly IOpsgenieAlert OpsGenieAlert;
        private readonly ICommandHandler<UpdateJobLastRunEndCommand> UpdateJobLastRunEndCommandHandler;
        private readonly ICredentialsCacheManager CredentialsCacheManager;
        private readonly IFileDestinationCache FileDestinationCache;

        public ExtractServiceListener(
            ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<ExtractServiceListener> serviceLogger,
            ILogger<ExtractJobRunner> extractJoblogger,
            ICredentialsCacheManager credentialsCacheManager,
            IFileDestinationCache fileDestinationCache,
            IOpsgenieAlert opsGenieAlert,
            ICommandHandler<UpdateJobLastRunEndCommand> updateJobLastRunEndCommandHandler
        ) : base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, serviceLogger)
        {
            ExtractJobLogger = extractJoblogger;
            OpsGenieAlert = opsGenieAlert;
            UpdateJobLastRunEndCommandHandler = updateJobLastRunEndCommandHandler;
            CredentialsCacheManager = credentialsCacheManager;
            FileDestinationCache = fileDestinationCache;
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            var jobSchedule = new JobScheduleMessageParser().ParseMessage(args.Message);
            var extractJobConfig = new ExtractJobConfigurationParser().Parse(jobSchedule.XmlObject);
            logger.Debug($"Executing Job: {jobSchedule.JobName}");

            var runner = new ExtractJobRunner(ExtractJobLogger, OpsGenieAlert, CredentialsCacheManager, FileDestinationCache);
            try
            {
                runner.Run(extractJobConfig);
            }
            catch (Exception ex)
            {
                logger.Error($"Job Failed: {jobSchedule.JobName}");
                logger.Error(ex.Message);
            }
            finally
            {
                try
                {
                    UpdateJobLastRunEndCommandHandler.Execute(new UpdateJobLastRunEndCommand
                    {
                        JobScheduleId = jobSchedule.JobScheduleId,
                        LastRunEndDateTime = DateTime.UtcNow
                    });
                }
                catch(Exception ex)
                {
                    logger.Error($"Failed to update LastRunEndDateTimeUtc: {jobSchedule.JobName}");
                    logger.Error(ex.Message);
                }
            }
        }
    }
}