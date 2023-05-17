using Esb.Core.EsbServices;
using Esb.Core.Serializer;
using Icon.Esb;
using Icon.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebSupport.Clients;
using WebSupport.Models;
using Contracts = Icon.Esb.Schemas.Mammoth.ContractTypes;

namespace WebSupport.Services
{
    public class WebSupportJobScheduleMessageService : IEsbService<JobScheduleModel>
    {
        private ISerializer<Contracts.JobSchedule> serializer;
        private ILogger logger;
        private IJobSchedulerBridgeClient jobSchedulerS3Client;

        public EsbConnectionSettings Settings { get; set; }

        public WebSupportJobScheduleMessageService(
            ISerializer<Contracts.JobSchedule> serializer,
            ILogger logger,
            IJobSchedulerBridgeClient jobSchedulerS3Client)
        {
            this.serializer = serializer;
            this.logger = logger;
            this.jobSchedulerS3Client = jobSchedulerS3Client;
        }

        public EsbServiceResponse Send(JobScheduleModel request)
        {
            if (request != null)
            {
                Guid messageId = Guid.NewGuid();
                Dictionary<string, string> messageProperties = new Dictionary<string, string>
                {
                    { EsbConstants.TransactionIdKey, messageId.ToString() },
                    { EsbConstants.SourceKey, EsbConstants.MammothSourceValueName },
                };

                Send(request, messageId, messageProperties);

                logger.Info($"Mammoth Price Push message has been sent for {request.Region}. MessageID: {messageId}.  Message: {JsonConvert.SerializeObject(request)}.");
                var response = new EsbServiceResponse
                {
                    Status = EsbServiceResponseStatus.Sent,
                };

                return response;
            }
            else
            {
                var esbErrorResponse = new EsbServiceResponse
                {
                    Status = EsbServiceResponseStatus.Failed,
                    ErrorCode = ErrorConstants.Codes.NoJobProvided,
                    ErrorDetails = ErrorConstants.Details.NoJobProvided
                };

                this.logger.Info($@"No message was sent to kick off a job because of the following errors -
                    ErrorCode: {esbErrorResponse.ErrorCode}. ErrorDetails: {esbErrorResponse.ErrorDetails}.");

                return esbErrorResponse;
            }
        }

        private void Send(JobScheduleModel request, Guid messageId, Dictionary<string, string> messageProperties)
        {
            var serializer = new Serializer<Contracts.JobSchedule>();
            var message = serializer.Serialize(new Contracts.JobSchedule
            {
                DestinationQueueName = request.DestinationQueueName,
                Enabled = request.Enabled,
                IntervalInSeconds = request.IntervalInSeconds,
                JobName = request.JobName,
                JobScheduleId = request.JobScheduleId,
                LastScheduledDateTimeUtc = request.LastScheduledDateTimeUtc.HasValue ? request.LastScheduledDateTimeUtc.Value.ToString("O") : null,
                LastRunEndDateTimeUtc = request.LastRunEndDateTimeUtc.HasValue ? request.LastRunEndDateTimeUtc.Value.ToString("O") : null,
                NextScheduledDateTimeUtc = request.NextScheduledDateTimeUtc.ToString("O"),
                Status = request.Status,
                Region = request.Region,
                StartDateTimeUtc = request.StartDateTimeUtc.ToString("O"),
                XmlObject = request.XmlObject
            });

            this.jobSchedulerS3Client.Send(request, message, messageId.ToString(), messageProperties);
        }
    }
}