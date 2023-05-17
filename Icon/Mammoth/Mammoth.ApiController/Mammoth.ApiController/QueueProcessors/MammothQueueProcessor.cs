using Icon.ApiController.Common;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
using Icon.Common.DataAccess;
using Icon.ActiveMQ.Producer;
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
                  activeMqProducer)
        {
            this.settings = settings;   
        }

        protected override MessageHistory BuildXmlMessage(string xml)
        {
            // DVS wants the xml in utf-8 encoding, but SQL Server wants it as utf-16.  This will replace the encoding in the xml header so that
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

        protected override bool PublishMessage(MessageHistory messageHistory)
        {
            logger.Info(string.Format("Preparing to send message {0}.", messageHistory.MessageHistoryId));
            esbMessageProperties["IconMessageID"] = messageHistory.MessageHistoryId.ToString();

            // Converting utf-16 encoding back to utf-8 before sending to ActiveMQ
            string xmlMessage = messageHistory.Message.Replace("utf-16", "utf-8");
            try
            {
                activeMqProducer.Send(xmlMessage, esbMessageProperties);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(String.Format("Failed to send message {0} to ActiveMQ (DVS). Error Details: {1}", esbMessageProperties["IconMessageID"], ex.ToString()));
                return false;
            }
        }

        protected override void SetMessageIds()
        {
            messageFailedStatusId = MessageStatusTypes.Failed;
            messageReadyStatusId = MessageStatusTypes.Ready;
            messageSentStatusId = MessageStatusTypes.Sent;
        }
    }
}
