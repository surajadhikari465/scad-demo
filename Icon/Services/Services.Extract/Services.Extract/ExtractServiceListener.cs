using System;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
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
            ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<ExtractServiceListener> serviceLogger,
            ILogger<ExtractJobRunner> extractJoblogger,
            ICredentialsCacheManager credentialsCacheManager,
            IFileDestinationCache fileDestinationCache,
            IOpsgenieAlert opsGenieAlert,
            ICommandHandler<UpdateJobLastRunEndCommand> updateJobLastRunEndCommandHandler,
            ICommandHandler<UpdateJobStatusCommand> updateJobStatusCommandHandler,
            IMessageParser<JobSchedule> messageParser,
            IExtractJobConfigurationParser extractJobConfigurationParser,
            IExtractJobRunnerFactory extractJobRunnerFactory
        ) : base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, serviceLogger)
        {
            this.extractJobLogger = extractJoblogger;
            this.opsGenieAlert = opsGenieAlert;
            this.updateJobLastRunEndCommandHandler = updateJobLastRunEndCommandHandler;
            this.updateJobStatusCommandHandler = updateJobStatusCommandHandler;
            this.credentialsCacheManager = credentialsCacheManager;
            this.fileDestinationCache = fileDestinationCache;
            this.messageParser = messageParser;
            this.extractJobConfigurationParser = extractJobConfigurationParser;
            this.extractJobRunnerFactory = extractJobRunnerFactory; 
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            var jobSchedule = messageParser.ParseMessage(args.Message);
            var extractJobConfig = extractJobConfigurationParser.Parse(jobSchedule.XmlObject);
            logger.Info($"Executing Job: {jobSchedule.JobName}");

            var runner = extractJobRunnerFactory.Create(extractJobLogger, opsGenieAlert, credentialsCacheManager, fileDestinationCache);
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
                logger.Error($"Job Failed: {jobSchedule.JobName}");
                logger.Error(ex.Message);
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