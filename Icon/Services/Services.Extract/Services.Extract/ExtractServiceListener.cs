using System;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Logging;
using OpsgenieAlert;
using Services.Extract.Credentials;
using Services.Extract.DataAccess.Commands;
using Services.Extract.Message.Parser;
using Services.Extract.Models;
using Wfm.Aws.ExtendedClient.Listener.SQS;
using Wfm.Aws.ExtendedClient.Listener.SQS.Settings;
using Wfm.Aws.ExtendedClient.SQS;
using Wfm.Aws.ExtendedClient.SQS.Model;

namespace Services.Extract
{
    public class ExtractServiceListener : SQSExtendedClientListener<ExtractServiceListener>
    {
        private readonly SQSExtendedClientListenerSettings listenerApplicationSettings;
        private readonly ILogger<ExtractJobRunner> extractJobLogger;
        private readonly IOpsgenieAlert opsGenieAlert;
        private readonly ICommandHandler<UpdateJobLastRunEndCommand> updateJobLastRunEndCommandHandler;
        private readonly ICommandHandler<UpdateJobStatusCommand> updateJobStatusCommandHandler;
        private readonly ICredentialsCacheManager credentialsCacheManager;
        private readonly IFileDestinationCache fileDestinationCache;
        private readonly IMessageParser<JobSchedule> messageParser; 
        private readonly IExtractJobConfigurationParser extractJobConfigurationParser;
        private readonly IExtractJobRunnerFactory extractJobRunnerFactory;

        public ExtractServiceListener(
            SQSExtendedClientListenerSettings listenerApplicationSettings,
            ISQSExtendedClient sqsExtendedClient,
            IEmailClient emailClient,
            ILogger<ExtractServiceListener> serviceLogger,
            ILogger<ExtractJobRunner> extractJobLogger,
            ICredentialsCacheManager credentialsCacheManager,
            IFileDestinationCache fileDestinationCache,
            IOpsgenieAlert opsGenieAlert,
            ICommandHandler<UpdateJobLastRunEndCommand> updateJobLastRunEndCommandHandler,
            ICommandHandler<UpdateJobStatusCommand> updateJobStatusCommandHandler,
            IMessageParser<JobSchedule> messageParser,
            IExtractJobConfigurationParser extractJobConfigurationParser,
            IExtractJobRunnerFactory extractJobRunnerFactory
        ) : base(listenerApplicationSettings, emailClient, sqsExtendedClient, serviceLogger)
        {
            this.listenerApplicationSettings = listenerApplicationSettings;
            this.extractJobLogger = extractJobLogger;
            this.opsGenieAlert = opsGenieAlert;
            this.updateJobLastRunEndCommandHandler = updateJobLastRunEndCommandHandler;
            this.updateJobStatusCommandHandler = updateJobStatusCommandHandler;
            this.credentialsCacheManager = credentialsCacheManager;
            this.fileDestinationCache = fileDestinationCache;
            this.messageParser = messageParser;
            this.extractJobConfigurationParser = extractJobConfigurationParser;
            this.extractJobRunnerFactory = extractJobRunnerFactory; 
        }

        public override void HandleMessage(SQSExtendedClientReceiveModel message)
        {
            string receivedMessage = message.S3Details[0].Data;
            Acknowledge(message);
            var jobSchedule = messageParser.ParseMessage(receivedMessage);
            var extractJobConfig = extractJobConfigurationParser.Parse(jobSchedule.XmlObject);
            logger.Info($"Executing Job: {jobSchedule.JobName}");

            var runner = extractJobRunnerFactory.Create(jobSchedule.JobName, extractJobLogger, opsGenieAlert, credentialsCacheManager, fileDestinationCache);
            try
            {
                updateJobStatusCommandHandler.Execute(new UpdateJobStatusCommand
                {
                    JobScheduleId = jobSchedule.JobScheduleId,
                    Status = Constants.RunningJobStatus
                });
                runner.Run(extractJobConfig);
                logger.Info($"Job Complete: {jobSchedule.JobName}");
            }
            catch (Exception ex)
            {
                logger.Error($"Job Failed: {jobSchedule.JobName}. {ex}");
            }
            finally
            {
                try
                {
                    updateJobStatusCommandHandler.Execute(new UpdateJobStatusCommand
                    {
                        JobScheduleId = jobSchedule.JobScheduleId,
                        Status = Constants.ReadyJobStatus
                    });
                    updateJobLastRunEndCommandHandler.Execute(new UpdateJobLastRunEndCommand
                    {
                        JobScheduleId = jobSchedule.JobScheduleId,
                        LastRunEndDateTime = DateTime.UtcNow
                    });
                }
                catch (Exception ex)
                {
                    logger.Error($"Failed to update LastRunEndDateTimeUtc: {jobSchedule.JobName}");
                    logger.Error(ex.Message);
                }
            }
        }
    }
}