using Icon.Common.Xml;
using Icon.Esb.Schemas.Mammoth;
using Icon.Logging;
using JobScheduler.Service.DataAccess;
using JobScheduler.Service.ErrorHandler;
using JobScheduler.Service.Model.DBModel;
using JobScheduler.Service.Publish;
using JobScheduler.Service.Serializer;
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

        public JobSchedulerProcessor(
            IJobScheduerDAL jobSchedulerDAL,
            IMessagePublisher messagePublisher,
            ISerializer<JobSchedule> serializer,
            IErrorEventPublisher errorEventPublisher,
            ILogger<JobSchedulerProcessor> logger
            )
        {
            this.jobSchedulerDAL = jobSchedulerDAL;
            this.messagePublisher = messagePublisher;
            this.serializer = serializer;
            this.errorEventPublisher = errorEventPublisher;
            this.logger = logger;
        }

        public void Process()
        {
            string currentJobName = "UNDEFINED";
            try
            {
                List<GetJobSchedulesQueryModel> jobSchedules = jobSchedulerDAL.GetJobSchedules();
                if (jobSchedules.Count > 0)
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
                logger.Error($"Error occurred in JobScheduler. {e}");
                errorEventPublisher.PublishErrorEvent(
                    "JobScheduler",
                    "JobScheduler",
                    new Dictionary<string, string>()
                    {
                        { "Run Time", DateTimeOffset.Now.ToString("O") }
                    },
                    currentJobName,
                    e.GetType().ToString(),
                    e.Message,
                    "Fatal"
                    );
            }
        }

        private void StartJob(GetJobSchedulesQueryModel jobSchedule)
        {
            logger.Info($@"Starting Job: {jobSchedule.JobName}, Region: {jobSchedule.Region}, DestinationQueueName: {jobSchedule.DestinationQueueName})");
            JobSchedule jobScheduleCanonical = MapToJobScheduleCanonical(jobSchedule);
            messagePublisher.PublishMessage(serializer.Serialize(jobScheduleCanonical, new Utf8StringWriter()), new Dictionary<string, string>());
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
