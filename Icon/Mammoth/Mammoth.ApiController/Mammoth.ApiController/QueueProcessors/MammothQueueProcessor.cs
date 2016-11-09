using Icon.ApiController.Common;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
using Icon.Common.Context;
using Icon.Common.DataAccess;
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
            IRenewableContext globalContext,
            IQueueReader<TMessageQueue, TContract> queueReader,
            ISerializer<TContract> serializer,
            ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>> saveToMessageHistoryCommandHandler,
            ICommandHandler<AssociateMessageToQueueCommand<TMessageQueue, MessageHistory>> associateMessageToQueueCommandHandler,
            ICommandHandler<UpdateMessageQueueProcessedDateCommand<TMessageQueue>> setProcessedDateCommandHandler,
            ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>> updateMessageHistoryCommandHandler,
            ICommandHandler<UpdateMessageQueueStatusCommand<TMessageQueue>> updateMessageQueueStatusCommandHandler,
            ICommandHandler<MarkQueuedEntriesAsInProcessCommand<TMessageQueue>> markQueuedEntriesAsInProcessCommandHandler,
            IEsbProducer producer,
            ApiControllerSettings settings)
            : base(logger, 
                  globalContext, 
                  queueReader, 
                  serializer, 
                  saveToMessageHistoryCommandHandler,
                  associateMessageToQueueCommandHandler,
                  setProcessedDateCommandHandler,
                  updateMessageHistoryCommandHandler,
                  updateMessageQueueStatusCommandHandler,
                  markQueuedEntriesAsInProcessCommandHandler,
                  producer)
        {
            this.settings = settings;
            esbMessageProperties = new Dictionary<string, string>
            {
                { "IconMessageID", "" },
                { "Source", settings.Source },
                { "nonReceivingSysName", settings.NonReceivingSystemsAll }
            };
        }

        protected override MessageHistory BuildXmlMessage(string xml)
        {
            // ESB wants the xml in utf-8 encoding, but SQL Server wants it as utf-16.  This will replace the encoding in the xml header so that
            // the database will happily store it.
            xml = new StringBuilder(xml).Replace("utf-8", "utf-16").ToString();

            return new MessageHistory
            {
                MessageStatusId = MessageStatusTypes.Ready,
                MessageTypeId = MessageTypes.ItemLocale,
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

            try
            {
                esbMessageProperties["IconMessageID"] = messageHistory.MessageHistoryId.ToString();
                producer.Send(messageHistory.Message, esbMessageProperties);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Failed to send message {0}.  Error details: {1}", messageHistory.MessageHistoryId, ex.ToString()));
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
