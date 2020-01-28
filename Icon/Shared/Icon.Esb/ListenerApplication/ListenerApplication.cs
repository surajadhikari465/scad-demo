using Icon.Common.Email;
using Icon.Esb.Subscriber;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using TIBCO.EMS;

namespace Icon.Esb.ListenerApplication
{
    public abstract class ListenerApplication<TListener, TListenerSettings> : IListenerApplication
        where TListener : class
        where TListenerSettings : IListenerApplicationSettings
    {
        protected ListenerApplicationSettings listenerApplicationSettings;
        protected EsbConnectionSettings esbConnectionSettings;
        protected IEsbSubscriber subscriber;
        protected IEmailClient emailClient;
        protected ILogger<TListener> logger;
        protected Timer listeningTimer;

        public bool IsConnected { get { return subscriber.IsConnected; } }
        public string ListenerApplicationName { get; protected set; }

        public ListenerApplication(
            ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<TListener> logger)
        {
            this.listenerApplicationSettings = listenerApplicationSettings;
            this.ListenerApplicationName = listenerApplicationSettings.ListenerApplicationName;
            this.esbConnectionSettings = esbConnectionSettings;
            this.subscriber = subscriber;
            this.emailClient = emailClient;
            this.logger = logger;
        }

        /// <summary>
        /// Starts the Listener application. Begins a infinite polling mechanism that will attempt to reconnect the
        /// Listener if it has been disconnected from the ESB.
        /// </summary>
        public void Run()
        {

            var computedClientId = $"{ListenerApplicationName}.{Environment.MachineName}.{Guid.NewGuid().ToString()}";
            var clientId = computedClientId.Substring(Math.Min(computedClientId.Length, 255));

            listeningTimer = new Timer(
                (x) => BeginListening(clientId),
                null,
                TimeSpan.FromSeconds(0),
                TimeSpan.FromSeconds(esbConnectionSettings.ReconnectDelay));
        }

        /// <summary>
        /// Opens a connection to the ESB, sets up MessageHandlers and ExceptionHandlers, and also 
        /// logs and notifies a successful connection was made.
        /// </summary>
        private void BeginListening(string clientId)
        {
            if (!IsConnected)
            {
                try
                {
                    subscriber.MessageReceived -= HandleMessage;
                    subscriber.ExceptionHandlers -= HandleEsbException;
                    subscriber.Dispose();
                    subscriber.OpenConnection(clientId);
                    subscriber.MessageReceived += HandleMessage;
                    subscriber.ExceptionHandlers += HandleEsbException;
                    subscriber.BeginListening();

                    var connectionSuccessMessage =
                        $"App: {ListenerApplicationName} Queue: {esbConnectionSettings.QueueName} ClientId: {clientId} connected successfully.";
                    logger.Info(connectionSuccessMessage);

                    try
                    {
                        emailClient.Send(connectionSuccessMessage, listenerApplicationSettings.EmailSubjectConnectionSuccess);
                    }
                    catch(Exception ex)
                    {
                         logger.Error($"Email alert fail: {connectionSuccessMessage}: {ex.Message}");
                    }
                }
                catch (Exception ex)
                {
                    string connectionFailueMessage =
                        $"An error occurred in {ListenerApplicationName} when trying to connect to {esbConnectionSettings.QueueName}. ClientId: {clientId}  Exception details: {ex.ToString()}";
                    LogAndNotifyError(connectionFailueMessage);
                }
            }
        }

        /// <summary>
        /// Closes and cleans up the Listener application.
        /// </summary>
        public virtual void Close()
        {
            if (subscriber != null)
            {
                subscriber.Dispose();
            }

            if (listeningTimer != null)
            {
                listeningTimer.Dispose();
            }
        }

        /// <summary>
        /// Base handler for ESB exceptions.
        /// </summary>
        /// <param name="sender">The object that sent the Exception.</param>
        /// <param name="exception">The Exception that was encountered.</param>
        public virtual void HandleEsbException(object sender, EMSException exception)
        {
            if (exception.ErrorCode == "Disconnected")
            {
                string errorMessage = string.Format("Icon ESB listener {0} was disconnected from {1}. Exception details: {2}", ListenerApplicationName, esbConnectionSettings.QueueName, exception);
                LogAndNotifyError(errorMessage);
            }
            else
            {
                LogAndNotifyError(exception);
            }
        }

        /// <summary>
        /// Logs and notifies the recipients of the exception via email. The error message has a specific structure.
        /// </summary>
        /// <param name="errorMessage">The Exception to log and notify.</param>
        protected virtual void LogAndNotifyError(Exception exception)
        {
            string errorMessage = string.Format("An error occurred in the {0} while connected to {1}.  Exception details: {2}", ListenerApplicationName, esbConnectionSettings.QueueName, exception.ToString());
            LogAndNotifyError(errorMessage);
        }

        /// <summary>
        /// Logs and notifies the recipients of the error via email.
        /// </summary>
        /// <param name="errorMessage">The error message to log and notify.</param>
        protected virtual void LogAndNotifyError(string errorMessage)
        {
            logger.Error(errorMessage);
            try
            {
                emailClient.Send(errorMessage, listenerApplicationSettings.EmailSubjectError);
            }
            catch(Exception ex)
            {
                logger.Error($"Email alert fail: {ex.Message}");
            }
        }

        /// <summary>
        /// Logs and notifies the recipients of the error via email.
        /// </summary>
        /// <param name="message">A message to be added to the log and email.</param>
        /// <param name="exception">The exception that occurred that will be added to the log and email.</param>
        protected virtual void LogAndNotifyError(string message, Exception exception)
        {
            var errorMessage = string.Format("An error occurred in the {0} while connected to {1}. {2} Exception details: {2}", ListenerApplicationName, esbConnectionSettings.QueueName, message, exception.ToString());
            LogAndNotifyError(errorMessage);
        }

        /// <summary>
        /// Logs and notifies recipients of the error via email. The error message is created by joining the errorMessageSections with line seperator strings.
        /// For the logged string, each section is separated by spaces. For the email notification, each section is separated by double line breaks.
        /// </summary>
        /// <param name="errorMessageSections">The error message split into multiple sections.</param>
        protected virtual void LogAndNotifyError(List<string> errorMessageSections)
        {
            logger.Error(string.Join(" ", errorMessageSections));
            try
            {
                emailClient.Send(string.Join("<br /><br />", errorMessageSections), listenerApplicationSettings.EmailSubjectError);
            }
            catch(Exception ex)
            {
                logger.Error($"Email alert fail: {ex.Message}");
            }
        }

        protected virtual void LogAndNotifyErrorWithMessage(Exception exception, EsbMessageEventArgs args)
        {
            List<string> errorMessageSections = new List<string>
                {
                    string.Format("An error occurred during message processing in the {0} while connected to {1}.", ListenerApplicationName, esbConnectionSettings.QueueName),
                    string.Format("Error details: {0}", exception.ToString()),
                    string.Format("Message: {0}", WebUtility.HtmlEncode(args.Message.ToString()))
                };

            LogAndNotifyError(errorMessageSections);
        }

        protected virtual void AcknowledgeMessage(EsbMessageEventArgs args)
        {
            if ((esbConnectionSettings.SessionMode == SessionMode.ClientAcknowledge 
                || esbConnectionSettings.SessionMode == SessionMode.ExplicitClientAcknowledge 
                || esbConnectionSettings.SessionMode == SessionMode.ExplicitClientDupsOkAcknowledge))
            {
                args.Message.Acknowledge();
            }
        }

        public abstract void HandleMessage(object sender, EsbMessageEventArgs args);
    }
}
