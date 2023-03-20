using Icon.Common.Xml;
using Icon.Esb.Schemas.Mammoth;
using Icon.Logging;
using JobScheduler.Service.DataAccess;
using JobScheduler.Service.ErrorHandler;
using JobScheduler.Service.Model.DBModel;
using JobScheduler.Service.Publish;
using JobScheduler.Service.Serializer;
using JobScheduler.Service.Settings;
using System;
using System.Collections.Generic;

namespace JobScheduler.Service.Processor
{
    internal class JobSchedulerProcessor : IJobSchedulerProcessor
    {
        private readonly IJobScheduerDAL jobSchedulerDAL;
        private readonly IMessagePublisher messagePublisher;
        private readonly ISerializer<JobSchedule> serializer;
        private readonly IErrorEventPublisher errorEventPublisher;
        private readonly ILogger<JobSchedulerProcessor> logger;
        private readonly JobSchedulerServiceSettings jobSchedulerServiceSettings;

        public JobSchedulerProcessor(
            IJobScheduerDAL jobSchedulerDAL,
            IMessagePublisher messagePublisher,
            ISerializer<JobSchedule> serializer,
            IErrorEventPublisher errorEventPublisher,
            ILogger<JobSchedulerProcessor> logger,
            JobSchedulerServiceSettings jobSchedulerServiceSettings
            )
        {
            this.jobSchedulerDAL = jobSchedulerDAL;
            this.messagePublisher = messagePublisher;
            this.serializer = serializer;
            this.errorEventPublisher = errorEventPublisher;
            this.logger = logger;
            this.jobSchedulerServiceSettings = jobSchedulerServiceSettings;
        }

        public void Process()
        {
            string currentJobName = "UNDEFINED";
            try
            {
                logger.Info($@"Started JobScheduler service instance with InstanceId: {jobSchedulerServiceSettings.InstanceId}.");
                jobSchedulerDAL.AcquireLock();
                List<GetJobSchedulesQueryModel> jobSchedules = jobSchedulerDAL.GetJobSchedules();
                int totalJobsFetched = jobSchedules.Count;
                logger.Info($@"Fetched {totalJobsFetched} jobs for JobScheduler service instance with InstanceId: {jobSchedulerServiceSettings.InstanceId}.");
                if (totalJobsFetched > 0)
                {
                    foreach (GetJobSchedulesQueryModel jobSchedule in jobSchedules)
                    {
                        currentJobName = jobSchedule.JobName;
                        StartJob(jobSchedule);
                        jobSchedulerDAL.UpdateLastRunDateTime(jobSchedule.JobScheduleId);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error($"Error occurred in JobScheduler with InstanceId: {jobSchedulerServiceSettings.InstanceId}. {e}");
                errorEventPublisher.PublishErrorEvent(
                    "JobScheduler",
                    "JobScheduler",
                    new Dictionary<string, string>()
                    {
                        { "Run Time", DateTimeOffset.Now.ToString("O") }
                    },
                    currentJobName,
                    e.GetType().ToString(),
                    e.Message
                    );
            }
            finally
            {
                jobSchedulerDAL.ReleaseLock();
                logger.Info($@"Released acquired locks, if any, for JobScheduler service instance with InstanceId: {jobSchedulerServiceSettings.InstanceId}.");
                logger.Info($@"Ended JobScheduler service instance with InstanceId: {jobSchedulerServiceSettings.InstanceId}.");
            }
        }

        private void StartJob(GetJobSchedulesQueryModel jobSchedule)
        {
            logger.Info($@"Starting Job: {jobSchedule.JobName}, Region: {jobSchedule.Region}, DestinationQueueName: {jobSchedule.DestinationQueueName} with InstanceId: {jobSchedulerServiceSettings.InstanceId}.");
            JobSchedule jobScheduleCanonical = MapToJobScheduleCanonical(jobSchedule);
            messagePublisher.PublishMessage(jobSchedule.DestinationQueueName, serializer.Serialize(jobScheduleCanonical, new Utf8StringWriter()), new Dictionary<string, string>());
            logger.Info($@"Completed Job: {jobSchedule.JobName}, Region: {jobSchedule.Region}, DestinationQueueName: {jobSchedule.DestinationQueueName} with InstanceId: {jobSchedulerServiceSettings.InstanceId}.");
        }

        private JobSchedule MapToJobScheduleCanonical(GetJobSchedulesQueryModel jobSchedule)
        {
            JobSchedule jobScheduleCanonical = new JobSchedule()
            {
                JobName = jobSchedule.JobName,
                Region = jobSchedule.Region,
                DestinationQueueName = jobSchedule.DestinationQueueName,
                StartDateTimeUtc = new DateTimeOffset(jobSchedule.StartDateTimeUtc).ToString("O"),
                NextScheduledDateTimeUtc = new DateTimeOffset(jobSchedule.NextScheduledDateTimeUtc).ToString("O"),
                IntervalInSeconds = jobSchedule.IntervalInSeconds,
                Enabled = jobSchedule.Enabled,
                Status = jobSchedule.Status,
                XmlObject = jobSchedule.XmlObject,
                JobScheduleId = jobSchedule.JobScheduleId,
            };
            if (jobSchedule.LastScheduledDateTimeUtc.HasValue)
            {
                jobScheduleCanonical.LastScheduledDateTimeUtc = new DateTimeOffset((DateTime)jobSchedule.LastScheduledDateTimeUtc).ToString("O");
            }
            if (jobSchedule.LastRunEndDateTimeUtc.HasValue)
            {
                jobScheduleCanonical.LastRunEndDateTimeUtc = new DateTimeOffset((DateTime)jobSchedule.LastRunEndDateTimeUtc).ToString("O");
            }
            return jobScheduleCanonical;
        }
    }
}
