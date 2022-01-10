using System;
using System.Collections.Generic;
using System.Threading;
using Apache.NMS.ActiveMQ;
using Apache.NMS;


namespace Icon.ActiveMQ.Producer
{
    public class ActiveMQProducer : ActiveMQConnection, IActiveMQProducer
    {
        private IMessageProducer producer;
        private string lastClientId;

        public ActiveMQProducer(ActiveMQConnectionSettings settings): base(settings)
        {
            lastClientId = "Undefined-" + Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// Verifies the ActiveMQ Connection and sends the message
        /// </summary>
        /// <param name="message">Message to be sent</param>
        /// <param name="messageProperties">Message properties: Default null</param>
        public void Send(string message, Dictionary<string, string> messageProperties = null)
        {
            VerifyConnectionAndGracefullyReconnect();
            ITextMessage textMessage = session.CreateTextMessage(message);
            Send(textMessage, messageProperties);   
        }

        /// <summary>
        /// Verifies the ActiveMQ Connection and sends the message
        /// </summary>
        /// <param name="message">Message to be sent</param>
        /// <param name="messageId">To give message ID to the message</param>
        /// <param name="messageProperties">Message properties: Default null</param>
        public void Send(string message, string messageId, Dictionary<string, string> messageProperties = null)
        {
            VerifyConnectionAndGracefullyReconnect();
            ITextMessage textMessage = session.CreateTextMessage(message);
            textMessage.NMSMessageId = messageId;
            Send(textMessage, messageProperties);
        }

        private void Send(ITextMessage textMessage, Dictionary<string, string> messageProperties = null)
        {
            if(messageProperties != null)
            {
                foreach(var property in messageProperties)
                {
                    textMessage.Properties.SetString(property.Key, property.Value);
                }
            }
            // The messages are retained in the Broker for 30 days if no consumer consumes them
            textMessage.NMSTimeToLive = TimeSpan.FromDays(30);

            VerifyConnectionAndGracefullyReconnect();
            Retry<Exception>(() => {
                producer.Send(textMessage);
            });
        }

        /// <summary>
        /// Opens a new connection with the given clientId
        /// </summary>
        /// <param name="clientId">An identifier to identify the specific connection. Using
        /// existing open connection's clientID will throw Exception</param>
        /// <param name="maxRetries">Max times to retry: Default 10 times</param>
        public void OpenConnection(string clientId, int maxRetries = 10)
        {
            this.lastClientId = clientId;
            Retry<Exception>(() => {
                base.OpenConnection(clientId);
                producer = session.CreateProducer(destination);
                producer.DeliveryMode = MsgDeliveryMode.Persistent;
            }, maxRetries);
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
        private void Retry<TException>(Action action, int maxRetries = 10, int timeBetweenRetries = 30000) where TException: Exception
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
            if(retryCount >= maxRetries)
            {
                throw exception;
            }
            Thread.Sleep(timeBetweenRetries);
            return retryCount;
        }
    }
}
