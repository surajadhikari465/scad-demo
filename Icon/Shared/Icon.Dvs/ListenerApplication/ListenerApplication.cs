using System;
using System.Collections.Generic;
using System.Threading;
using Icon.Common.Email;
using Icon.Dvs.Subscriber;
using Icon.Dvs.Model;
using Icon.Logging;

namespace Icon.Dvs.ListenerApplication
{
    public abstract class ListenerApplication<TListener> : IListenerApplication
        where TListener : class
    {
        private readonly DvsListenerSettings settings;
        private readonly IEmailClient emailClient;
        private readonly IDvsSubscriber subscriber;
        private readonly ILogger<TListener> logger;
        private bool pollMessages = false;
        private bool processingMessage = false;

        public ListenerApplication(
            DvsListenerSettings settings,
            IDvsSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<TListener> logger)
        {
            this.settings = settings;
            this.subscriber = subscriber;
            this.emailClient = emailClient;
            this.logger = logger;
        }

        /// <summary>
        /// Polls and processes messages if pollMessages is true
        /// Sleeps if no message was processed in the previous execution
        /// </summary>
        public void Start()
        {
            // TODO: Implement using Timer if applicable
            pollMessages = true;
            while (pollMessages)
            {
                processingMessage = true;
                bool messageReceived = ProcessDvsMessage();
                processingMessage = false;
                if (!messageReceived)
                {
                    Thread.Sleep(settings.PollInterval);
                }
            }
        }

        public void Stop()
        {
            pollMessages = false;
            // Wait till processing is complete
            while (processingMessage)
            {
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Receives DvsMessage, handles the message and deletes the message if there's no error
        /// </summary>
        /// <returns>true, if message was received</returns>
        private bool ProcessDvsMessage()
        {
            DvsMessage message = null;
            bool messageReceived = false;

            try
            {
                message = subscriber.ReceiveDvsMessage();
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred in the {settings.ListenerApplicationName} while receiving Dvs messages";
                LogAndNotifyError(errorMessage, ex);
            }

            if (message != null)
            {
                messageReceived = true;
                try
                {
                    HandleMessage(message);
                    subscriber.DeleteSqsMessage(message.SqsMessage.SqsReceiptHandle);
                }
                catch (Exception ex)
                {
                    HandleException(ex, message);
                }
            }
            else
            {
                logger.Info("Message is null, no message received");
            }
            return messageReceived;
        }

        /// <summary>
        /// Handles the DvsMessage.
        /// Override this method to implement the message handling logic
        /// Throw the Exception to not delete the SQS message (used to handle retries and DLQ)
        /// </summary>
        /// <param name="message">message received from DVS</param>
        public abstract void HandleMessage(DvsMessage message);

        /// <summary>
        /// Handles exception caused during HandleMessage method.
        /// This method can be overrided for any custom error handling logic.
        /// </summary>
        /// <param name="ex">Exception during HandleMessage() method</param>
        /// <param name="message">DvsMessage that was being processed during the Exception</param>
        public void HandleException(Exception ex, DvsMessage message)
        {
            string errorMessage = $"An error occurred in the {settings.ListenerApplicationName} while processing DVS message with MessageId: {message.SqsMessage.MessageId}";
            LogAndNotifyError(errorMessage, ex);
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
                string subject = $"{settings.ListenerApplicationName}: Error Occurred";
                emailClient.Send(string.Join("<br /><br />", errorMessageSections), subject);
            }
            catch (Exception emailException)
            {
                logger.Error(emailException.ToString());
            }
        }
    }
}
