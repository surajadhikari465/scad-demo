using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TIBCO.EMS;

namespace Icon.Esb.Producer
{
    public class EsbProducer : EsbConnection, IEsbProducer
    {
        private MessageProducer producer;
        private string lastClientId = null;

        public EsbProducer(EsbConnectionSettings settings) : base(settings) 
        {
            lastClientId = "Undefined-" + Guid.NewGuid().ToString("N");
         }
        
        public void Send(string message, Dictionary<string, string> messageProperties = null)
        {
            Retry<Exception>(() =>
            {
                // Verify Connection
                VerifyConnectionAndGracefullyReconnect();

                TextMessage textMessage = session.CreateTextMessage(message);

                Send(textMessage, messageProperties);
            });
        }

        public void Send(string message, string messageId, Dictionary<string, string> messageProperties = null)
        {
            Retry<Exception>(() =>
            {
                // Verify Connection
                VerifyConnectionAndGracefullyReconnect();

                TextMessage textMessage = session.CreateTextMessage(message);
                textMessage.MessageID = messageId;

                Send(textMessage, messageProperties);
            });
        }

        public void Send(byte[] bytes, string messageId, Dictionary<string, string> messageProperties = null)
        {
            Retry<Exception>(() =>
            {
                // Verify Connection
                VerifyConnectionAndGracefullyReconnect();

                BytesMessage bytesMessage = session.CreateBytesMessage();
                bytesMessage.WriteBytes(bytes);

                bytesMessage.MessageID = messageId;

                Send(bytesMessage, messageProperties);
            });
        }

        public override void OpenConnection(string clientId)
        {
            this.lastClientId = clientId;
            Retry<EMSException>(() => { 
                base.OpenConnection(clientId);
                producer = session.CreateProducer(destination);
                producer.DeliveryMode = DeliveryMode.PERSISTENT;
            });
        }

        public override void Dispose()
        {
            if (producer != null)
            {
                producer.Close();
            }

            base.Dispose();
        }

        private void Send(TextMessage textMessage, Dictionary<string, string> messageProperties = null)
        {
            // Set Properties
            if (messageProperties != null)
            {
                foreach (var property in messageProperties)
                {
                    textMessage.SetStringProperty(property.Key, property.Value);
                }
            }

            // Verify Connection
            VerifyConnectionAndGracefullyReconnect();

            // Send Message, Retry on TIBCO.EMS.IllegalStateException 10 times with 30s between tries
            Retry<IllegalStateException>(() => { producer.Send(textMessage); });
        }

        private void Send(BytesMessage byteMessage, Dictionary<string, string> messageProperties = null)
        {
            // Set Properties

            if (messageProperties != null)
            {
                foreach (var property in messageProperties)
                {
                    byteMessage.SetStringProperty(property.Key, property.Value);
                }
            }

            // Verify Connection
            VerifyConnectionAndGracefullyReconnect();

            // Send Message, Retry on TIBCO.EMS.IllegalStateException 10 times with 30s between tries
            Retry<IllegalStateException>(() => { producer.Send(byteMessage); });
        }

        private void VerifyConnectionAndGracefullyReconnect()
        {
            Retry<Exception>(() => {
                if (this.IsConnected == false)
                {
                    this.OpenConnection(this.lastClientId);
                }
            });
        }

        /// <summary>
        /// Retry an action maxRetries times, and waits timeBetweenRetries milliseconds between retries.
        /// </summary>
        /// <typeparam name="TException">Type of exception to retry.</typeparam>
        /// <param name="action">Action to execute and retry if needed.</param>
        /// <param name="maxRetries">Max times to retry: Default 10 times</param>
        /// <param name="timeBetweenRetries">Time in milliseconds to wait between retries.: Default 30 seconds.</param>
        private void Retry<TException>(Action action, int maxRetries = 10, int timeBetweenRetries = 30000) where TException: Exception
        {
            int retryCount = 0;
            bool retry = true;
            while (retry == true)
            {
                try
                {
                    action();
                    retry = false;
                }
                catch (TException)
                {
                    retryCount++;
                    if (retryCount >= maxRetries)
                    {
                        throw;
                    }
                    Thread.Sleep(timeBetweenRetries);
                }
            }
        }
    }
}
