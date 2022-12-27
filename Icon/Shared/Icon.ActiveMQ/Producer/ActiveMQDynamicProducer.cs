using System;
using System.Collections.Generic;
using System.Threading;
using Apache.NMS.ActiveMQ;
using Apache.NMS;


namespace Icon.ActiveMQ.Producer
{
    /// <summary>
    /// This producer can be used when the queue will be known only during Send operation
    /// </summary>
    public class ActiveMQDynamicProducer : ActiveMQConnection, IActiveMQDynamicProducer
    {
        private IMessageProducer producer;
        private string lastClientId;

        private const int TIME_TO_LIVE_HOURS = 12;

        public ActiveMQDynamicProducer(ActiveMQConnectionSettings settings) : base(settings)
        {
            lastClientId = "Undefined-" + Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// Verifies the ActiveMQ Connection and sends the message
        /// </summary>
        /// <param name="queueName">queue to send the message</param>
        /// <param name="message">Message to be sent</param>
        /// <param name="messageProperties">Message properties: Default null</param>
        public void Send(string queueName, string message, Dictionary<string, string> messageProperties = null)
        {
            VerifyConnectionAndGracefullyReconnect();
            ITextMessage textMessage = session.CreateTextMessage(message);
            Send(queueName, textMessage, messageProperties);
        }

        private void Send(string queueName, ITextMessage textMessage, Dictionary<string, string> messageProperties = null)
        {
            if (messageProperties != null)
            {
                foreach (var property in messageProperties)
                {
                    textMessage.Properties.SetString(property.Key, property.Value);
                }
            }
            // The messages are retained in the Broker for 12 hours if no consumer consumes them
            textMessage.NMSTimeToLive = TimeSpan.FromHours(TIME_TO_LIVE_HOURS);

            VerifyConnectionAndGracefullyReconnect();
            Retry<Exception>(() => {
                producer.Send(GetDestination(queueName), textMessage);
            });
        }

        /// <summary>
        /// Opens a new connection with the given clientId
        /// </summary>
        /// <param name="clientId">An identifier to identify the specific connection. Using
        /// existing open connection's clientID will throw Exception</param>
        /// <param name="maxRetries">Max times to retry</param>
        public void OpenConnection(string clientId, int maxRetries)
        {
            this.lastClientId = clientId;
            Retry<Exception>(() =>
            {
                OpenConnectionAndCreateProducer(clientId);
            }, maxRetries);
        }

        /// <summary>
        /// Opens a new connection with the given clientId
        /// </summary>
        /// <param name="clientId">An identifier to identify the specific connection. Using
        /// existing open connection's clientID will throw Exception</param>
        public override void OpenConnection(string clientId)
        {
            this.lastClientId = clientId;
            Retry<Exception>(() => {
                OpenConnectionAndCreateProducer(clientId);
            });
        }

        private void OpenConnectionAndCreateProducer(string clientId)
        {
            base.OpenConnection(clientId);
            producer = session.CreateProducer();
            producer.DeliveryMode = MsgDeliveryMode.Persistent;
        }

        private void VerifyConnectionAndGracefullyReconnect(int maxRetries = 10)
        {
            if (!base.IsConnected)
            {
                this.OpenConnection(this.lastClientId, maxRetries);
            }
        }

        /// <summary>
        /// Retries action() 10 times in 30 second intervals for the exception type TException
        /// </summary>
        /// <typeparam name="TException">Type of exception to retry.</typeparam>
        /// <param name="action">Action to execute and retry if needed.</param>
        /// <param name="maxRetries">Max times to retry: Default 10 times</param>
        /// <param name="timeBetweenRetries">Time in milliseconds to wait between retries.: Default 30 seconds.</param>
        private void Retry<TException>(Action action, int maxRetries = 10, int timeBetweenRetries = 30000) where TException : Exception
        {
            int retryCount = 0;
            bool retry = true;
            while (retry)
            {
                try
                {
                    action();
                    retry = false;
                }
                catch (ConnectionClosedException ex)
                {
                    // Removing previous Connection, so that producer will reconnect
                    this.connection = null;
                    retryCount = EvaluateFurtherRetry(ex, retryCount, maxRetries, timeBetweenRetries);

                    // trying to reconnect
                    VerifyConnectionAndGracefullyReconnect();
                }
                catch (TException ex)
                {
                    retryCount = EvaluateFurtherRetry(ex, retryCount, maxRetries, timeBetweenRetries);
                }
            }
        }

        /// <summary>
        /// Handles delay for the next retry
        /// </summary>
        /// <param name="exception">Exception to throw when number of retries exceeds maxRetries</param>
        /// <param name="retryCount">Number of retries performed</param>
        /// <param name="maxRetries">Maximum number of retries allowed</param>
        /// <param name="timeBetweenRetries">Time interval to sleep between retries</param>
        /// <returns></returns>
        private int EvaluateFurtherRetry(Exception exception, int retryCount, int maxRetries, int timeBetweenRetries)
        {
            retryCount++;
            if (retryCount >= maxRetries)
            {
                throw exception;
            }
            Thread.Sleep(timeBetweenRetries);
            return retryCount;
        }

        private IDestination GetDestination(string queueName)
        {
            if (queueName.ToLower().Contains("topic"))
            {
                return session.GetTopic(Settings.QueueName);
            }
            else
            {
                return session.GetQueue(Settings.QueueName);
            }
        }
    }
}
