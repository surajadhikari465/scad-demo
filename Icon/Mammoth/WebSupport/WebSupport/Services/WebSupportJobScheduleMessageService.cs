using Esb.Core.EsbServices;
using Esb.Core.Serializer;
using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TIBCO.EMS;
using WebSupport.DataAccess.Models;
using WebSupport.Models;
using Contracts = Icon.Esb.Schemas.Mammoth.ContractTypes;

namespace WebSupport.Services
{
    public class WebSupportJobScheduleMessageService : IEsbService<JobScheduleModel>
    {
        private ISerializer<Contracts.JobSchedule> serializer;
        private ILogger logger;

        public EsbConnectionSettings Settings { get; set; }

        public WebSupportJobScheduleMessageService(
            ISerializer<Contracts.JobSchedule> serializer,
            ILogger logger)
        {
            this.serializer = serializer;
            this.logger = logger;
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

            EMSSSLFileStoreInfo storeInfo = new EMSSSLFileStoreInfo();
            storeInfo.SetSSLPassword(Settings.SslPassword.ToCharArray());
            storeInfo.SetSSLClientIdentity(GetEsbCert());

            ConnectionFactory connectionFactory = new ConnectionFactory(Settings.ServerUrl);
            connectionFactory.SetCertificateStoreType(EMSSSLStoreType.EMSSSL_STORE_TYPE_FILE, storeInfo);
            connectionFactory.SetTargetHostName(Settings.TargetHostName);

            Session session = connectionFactory.CreateConnection(Settings.JmsUsername, Settings.JmsPassword)
                .CreateSession(false, Settings.SessionMode);
            Destination destination = session.CreateQueue(Settings.QueueName);
            MessageProducer producer = session.CreateProducer(destination);

            TextMessage textMessage = session.CreateTextMessage(message);
            textMessage.MessageID = messageId.ToString();
            foreach (var keyValuePair in messageProperties)
            {
                textMessage.SetStringProperty(keyValuePair.Key, keyValuePair.Value);
            }

            producer.Send(textMessage);

            session.Connection.Close();
        }

        private X509Certificate GetEsbCert()
        {
            var store = new X509Store(Settings.CertificateStoreName, Settings.CertificateStoreLocation);
            store.Open(OpenFlags.ReadOnly);
            var cert = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, Settings.CertificateName, true)[0];
            store.Close();
            return cert;
        }
    }
}