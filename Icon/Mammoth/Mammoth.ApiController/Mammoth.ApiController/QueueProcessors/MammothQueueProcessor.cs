using Icon.ApiController.Common;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
using Icon.Common.DataAccess;
using Icon.ActiveMQ.Producer;
using Icon.Esb.Producer;
using Icon.Logging;
using Mammoth.Common.DataAccess;
using Mammoth.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mammoth.ApiController.QueueProcessors
{
    public abstract class MammothQueueProcessor<TMessageQueue, TContract> : QueueProcessorBase<TMessageQueue, TContract, MessageHistory>
    {
        protected Dictionary<string, string> esbMessageProperties;

        public MammothQueueProcessor(ILogger logger,
            IQueueReader<TMessageQueue, TContract> queueReader,
            ISerializer<TContract> serializer,
            ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>> saveToMessageHistoryCommandHandler,
            ICommandHandler<AssociateMessageToQueueCommand<TMessageQueue, MessageHistory>> associateMessageToQueueCommandHandler,
            ICommandHandler<UpdateMessageQueueProcessedDateCommand<TMessageQueue>> setProcessedDateCommandHandler,
            ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>> updateMessageHistoryCommandHandler,
            ICommandHandler<UpdateMessageQueueStatusCommand<TMessageQueue>> updateMessageQueueStatusCommandHandler,
            ICommandHandler<MarkQueuedEntriesAsInProcessCommand<TMessageQueue>> markQueuedEntriesAsInProcessCommandHandler,
            IEsbProducer esbProducer,
            IActiveMQProducer activeMqProducer,
            ApiControllerSettings settings)
            : base(logger, 
                  queueReader, 
                  serializer, 
                  saveToMessageHistoryCommandHandler,
                  associateMessageToQueueCommandHandler,
                  setProcessedDateCommandHandler,
                  updateMessageHistoryCommandHandler,
                  updateMessageQueueStatusCommandHandler,
                  markQueuedEntriesAsInProcessCommandHandler,
                  esbProducer,
                  activeMqProducer)
        {
            this.settings = settings;   
        }

        protected override MessageHistory BuildXmlMessage(string xml)
        {
            // ESB wants the xml in utf-8 encoding, but SQL Server wants it as utf-16.  This will replace the encoding in the xml header so that
            // the database will happily store it.
            xml = new StringBuilder(xml).Replace("utf-8", "utf-16").ToString();

            return new MessageHistory
            {
                MessageStatusId = MessageStatusTypes.Ready,
                MessageTypeId = MessageType,
                Message = xml,
                InsertDate = DateTime.Now,
                InProcessBy = settings.Instance,
                ProcessedDate = null
            };
        }

        protected override string GetMessageHistoryId(MessageHistory messageHistory)
        {
            return messageHistory.MessageHistoryId.ToString();
        }

        protected override int PublishMessage(MessageHistory messageHistory)
        {
            int messageStatus;
            bool sentToEsb = true, sentToActiveMq = true;

            logger.Info(string.Format("Preparing to send message {0}.", messageHistory.MessageHistoryId));
            esbMessageProperties["IconMessageID"] = messageHistory.MessageHistoryId.ToString();

            // Converting utf-16 encoding back to utf-8 before sending to ESB or ActiveMQ
            string xmlMessage = messageHistory.Message.Replace("utf-16", "utf-8");

            if(messageHistory.MessageStatusId != MessageStatusTypes.SentToEsb)
                sentToEsb = PublishMessageToEsb(xmlMessage);
            if(messageHistory.MessageStatusId != MessageStatusTypes.SentToActiveMq)
                sentToActiveMq = PublishMessageToActiveMq(xmlMessage);

            if (sentToEsb && sentToActiveMq)
                messageStatus = messageSentStatusId;
            else if (sentToEsb && !sentToActiveMq)
                messageStatus = messageSentToEsbStatusId;
            else if (!sentToEsb && sentToActiveMq)
                messageStatus = messageSentToActiveMqStatusId;
            else
                messageStatus = messageFailedStatusId;
            return messageStatus;
        }

        private bool PublishMessageToEsb(string xmlMessage)
        {
            try
            {
                esbProducer.Send(xmlMessage, esbMessageProperties);
                return true;
            }
            catch(Exception ex)
            {
                logger.Error(string.Format("Failed to send message {0} to ESB. Error Details: {1}", esbMessageProperties["IconMessageID"], ex.ToString()));
                return false;
            }
        }

        private bool PublishMessageToActiveMq(String xmlMessage)
        {
            try
            {
                activeMqProducer.Send(xmlMessage, esbMessageProperties);
                return true;
            }
            catch(Exception ex)
            {
                logger.Error(String.Format("Failed to send message {0} to ActiveMQ. Error Details: {1}", esbMessageProperties["IconMessageID"], ex.ToString()));
                return false;
            }
        }

        protected override void SetMessageIds()
        {
            messageFailedStatusId = MessageStatusTypes.Failed;
            messageReadyStatusId = MessageStatusTypes.Ready;
            messageSentStatusId = MessageStatusTypes.Sent;
            messageSentToEsbStatusId = MessageStatusTypes.SentToEsb;
            messageSentToActiveMqStatusId = MessageStatusTypes.SentToActiveMq;
        }
    }
}
