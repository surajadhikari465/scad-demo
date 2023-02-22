using Amazon.SQS.Model;
using Icon.Common.Email;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Timers;
using Wfm.Aws.ExtendedClient.Listener.SQS.Settings;
using Wfm.Aws.ExtendedClient.SQS;
using Wfm.Aws.ExtendedClient.SQS.Model;

namespace Wfm.Aws.ExtendedClient.Listener.SQS
{
    public abstract class SQSExtendedClientListener<TListener> : ISQSExtendedClientListener
        where TListener : class
    {
        protected readonly SQSExtendedClientListenerSettings settings;
        protected readonly IEmailClient emailClient;
        protected readonly ISQSExtendedClient sqsExtendedClient;
        protected readonly ILogger<TListener> logger;
        private readonly Timer listenerTimer;
        // keeping default as false because in ESB we had to explicitly call Acknowledge().
        protected bool AcknowledgeMessage { get; set; } = false;
        private bool isServiceRunning = false;

        public SQSExtendedClientListener(SQSExtendedClientListenerSettings settings, IEmailClient emailClient, ISQSExtendedClient sqsExtendedClient, ILogger<TListener> logger)
        {
            this.settings = settings;
            this.emailClient = emailClient;
            this.sqsExtendedClient = sqsExtendedClient;
            this.logger = logger;
            this.listenerTimer = new Timer(settings.SQSListenerPollIntervalInSeconds * 1000);
        }

        public abstract void HandleMessage(SQSExtendedClientReceiveModel message);

        public void Start()
        {
            logger.Info($"Starting the listener - {settings.SQSListenerApplicationName}");
            listenerTimer.Elapsed += RunService;
            listenerTimer.Start();
            logger.Info($"Started the listener - {settings.SQSListenerApplicationName}");
        }

        private void RunService(object sender, ElapsedEventArgs e)
        {
            listenerTimer.Stop();
            try
            {
                isServiceRunning = true;
                ProcessMessagesTillQueueIsEmpty();
            }
            finally
            {
                listenerTimer.Start();
                isServiceRunning = false;
            }
        }

        private void ProcessMessagesTillQueueIsEmpty()
        {
            // Processes till Queue is empty
            while (ProcessMessage())
                ;
        }

        private bool ProcessMessage()
        {
            IList<SQSExtendedClientReceiveModel> messages = null;
            bool messageReceived = false;

            try
            {
                messages = sqsExtendedClient.ReceiveMessage(settings.SQSListenerQueueUrl, 1, settings.SQSListenerTimeoutInSeconds);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred in the {settings.SQSListenerApplicationName} while receiving messages";
                LogAndNotifyError(errorMessage, ex);
            }

            if (messages != null && messages.Count > 0)
            {
                messageReceived = true;
                try
                {
                    HandleMessage(messages[0]);
                }
                catch (Exception ex)
                {
                    HandleException(ex, messages[0]);
                }
                finally
                {
                    if (AcknowledgeMessage)
                    {
                        Acknowledge(messages[0]);
                    }
                }
            }
            else
            {
                logger.Info("Message is null, no message received");
            }
            return messageReceived;
        }
        public void HandleException(Exception ex, SQSExtendedClientReceiveModel message)
        {
            string errorMessage = $"An error occurred in the {settings.SQSListenerApplicationName} while processing message with MessageId: {message.SQSMessageID}";
            LogAndNotifyError(errorMessage, ex);
        }

        protected DeleteMessageResponse Acknowledge(SQSExtendedClientReceiveModel message)
        {
            return sqsExtendedClient.DeleteMessage(settings.SQSListenerQueueUrl, message.SQSReceiptHandle);
        }

        protected void LogAndNotifyError(string errorMessage, Exception ex)
        {
            logger.Error(errorMessage);
            logger.Error(ex.ToString());
            try
            {
                List<string> errorMessageSections = new List<string>
                {
                    errorMessage,
                    $"Error Details: {ex}"
                };
                string subject = $"{settings.SQSListenerApplicationName}: Error Occurred";
                emailClient.Send(string.Join("<br /><br />", errorMessageSections), subject);
            }
            catch (Exception emailException)
            {
                logger.Error(emailException.ToString());
            }
        }

        public void Stop()
        {
            logger.Info($"Stopping the listener - {settings.SQSListenerApplicationName}");
            if (settings.SQSListenerSafeStopCheckEnabled)
            {
                while (isServiceRunning)
                {
                    logger.Info($"Waiting {settings.SQSListenerSafeStopCheckInSeconds} seconds for the listener - {settings.SQSListenerApplicationName} to complete processing before stopping.");
                    System.Threading.Thread.Sleep(settings.SQSListenerSafeStopCheckInSeconds * 1000);
                }
            }
            listenerTimer.Stop();
            listenerTimer.Elapsed -= RunService;
            logger.Info($"Stopped the listener - {settings.SQSListenerApplicationName}");
        }
    }
}
