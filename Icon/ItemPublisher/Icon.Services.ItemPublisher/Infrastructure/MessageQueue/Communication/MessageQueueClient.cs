using Icon.Esb.Factory;
using Icon.Esb.Producer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Icon.Services.ItemPublisher.Infrastructure.MessageQueue.Communication;
using Icon.ActiveMQ.Factory;
using Icon.ActiveMQ.Producer;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue
{
    /// <summary>
    /// Class that encapsulates ESB functionality
    /// </summary>
    public class MessageQueueClient : IMessageQueueClient
    {
        private IEsbConnectionFactory esbConnectionFactory;
        private IActiveMQConnectionFactory activeMQConnectionFactory;
        private IMessageHeaderBuilder messageHeaderBuilder;
        private IEsbProducer producer;
        private IActiveMQProducer activeMqProducer;
        private IClientIdManager clientIdManager;

        public MessageQueueClient(IEsbConnectionFactory esbConnectionFactory, IMessageHeaderBuilder esbHeaderBuilder, IClientIdManager clientIdManager, IActiveMQConnectionFactory activeMQConnectionFactory)
        {
            this.esbConnectionFactory = esbConnectionFactory;
            this.messageHeaderBuilder = esbHeaderBuilder;
            this.clientIdManager = clientIdManager;
            this.activeMQConnectionFactory = activeMQConnectionFactory;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        /// <summary>
        /// Sends message to ESB and ActiveMQ.
        /// </summary>
        /// <param name="request">Message request</param>
        /// <returns></returns>
        public async Task<MessageSendResult> SendMessage(string request, List<string> nonReceivingSystems)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            Guid messageId = Guid.NewGuid();
            Dictionary<string, string> headers = new Dictionary<string, string>();
            Exception activeMqSendException;
            Exception esbSendException;
            string errorMessage = String.Empty;

            try
            {
                headers = this.messageHeaderBuilder.BuildMessageHeader(nonReceivingSystems, messageId.ToString());

                esbSendException = PublishMessageToEsb(request, headers);
                activeMqSendException = PublishMessageToActiveMq(request, headers);

                if (esbSendException == null && activeMqSendException == null)
                {
                    return new MessageSendResult(true, errorMessage, request, headers, messageId);
                }
                else if (esbSendException != null && activeMqSendException != null)
                {
                    errorMessage = "Error Occurred while sending to both ESB and ActiveMQ";
                    throw new AggregateException(errorMessage, new Exception[] { esbSendException, activeMqSendException });
                }
                else if (activeMqSendException != null)
                {
                    errorMessage = "Error Occurred while sending to ActiveMQ, message sent to ESB successfully";
                    throw activeMqSendException;
                }
                else
                {
                    errorMessage = "Error Occurred while sending to ESB, message sent to ActiveMQ successfully";
                    throw esbSendException;
                }
            }
            catch (Exception ex)
            {
                // Disposing EsbProducer
                this.producer?.Dispose();
                this.producer = null;

                // Disposing ActiveMQProducer
                this.activeMqProducer?.Dispose();
                this.activeMqProducer = null;

                return new MessageSendResult(false, errorMessage, request, headers, messageId, null, ex);
            }

        }

        private Exception PublishMessageToEsb(string message, Dictionary<string, string> properties)
        {
            try
            {
                if (this.producer == null || !this.producer.IsConnected)
                {
                    this.producer = this.esbConnectionFactory.CreateProducer(this.clientIdManager.GetClientId());
                }
                this.producer.Send(message, properties);
            }
            catch(Exception ex)
            {
                return ex;
            }
            return null;
        }

        private Exception PublishMessageToActiveMq(string message, Dictionary<string, string> properties)
        {
            try
            {
                if (this.activeMqProducer == null || !this.activeMqProducer.IsConnected)
                {
                    this.activeMqProducer = this.activeMQConnectionFactory.CreateProducer(this.clientIdManager.GetClientId());
                }
                this.activeMqProducer.Send(message, properties);
            }
            catch (Exception ex)
            {
                return ex;
            }
            return null;
        }
    }
}