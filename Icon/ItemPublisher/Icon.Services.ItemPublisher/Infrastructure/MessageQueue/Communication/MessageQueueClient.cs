using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Icon.Services.ItemPublisher.Infrastructure.MessageQueue.Communication;
using Icon.ActiveMQ.Factory;
using Icon.ActiveMQ.Producer;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue
{
    /// <summary>
    /// Class that encapsulates the functionality to send message to DVS
    /// </summary>
    public class MessageQueueClient : IMessageQueueClient
    {
        private IActiveMQConnectionFactory activeMQConnectionFactory;
        private IMessageHeaderBuilder messageHeaderBuilder;
        private IActiveMQProducer activeMqProducer;
        private IClientIdManager clientIdManager;

        public MessageQueueClient(IMessageHeaderBuilder messageHeaderBuilder, IClientIdManager clientIdManager, IActiveMQConnectionFactory activeMQConnectionFactory)
        {
            this.messageHeaderBuilder = messageHeaderBuilder;
            this.clientIdManager = clientIdManager;
            this.activeMQConnectionFactory = activeMQConnectionFactory;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        /// <summary>
        /// Sends message to ActiveMQ (DVS).
        /// </summary>
        /// <param name="request">Message request</param>
        /// <returns></returns>
        public async Task<MessageSendResult> SendMessage(string request, List<string> nonReceivingSystems)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            Guid messageId = Guid.NewGuid();
            Dictionary<string, string> headers = new Dictionary<string, string>();
            Exception activeMqSendException;
            string errorMessage = String.Empty;

            try
            {
                headers = this.messageHeaderBuilder.BuildMessageHeader(nonReceivingSystems, messageId.ToString());

                activeMqSendException = PublishMessageToActiveMq(request, headers);

                if (activeMqSendException == null)
                {
                    return new MessageSendResult(true, errorMessage, request, headers, messageId);
                }
                else
                {
                    errorMessage = "Error Occurred while sending to ActiveMQ";
                    throw activeMqSendException;
                }
            }
            catch (Exception ex)
            {
                // Disposing ActiveMQProducer
                this.activeMqProducer?.Dispose();
                this.activeMqProducer = null;

                return new MessageSendResult(false, errorMessage, request, headers, messageId, null, ex);
            }

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