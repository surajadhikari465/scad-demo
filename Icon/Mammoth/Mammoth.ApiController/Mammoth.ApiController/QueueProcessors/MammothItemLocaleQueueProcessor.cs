﻿using Icon.ApiController.Common;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
using Icon.Common.DataAccess;
using Icon.ActiveMQ.Producer;
using Icon.Logging;
using Mammoth.Common.DataAccess;
using Mammoth.Framework;
using System.Collections.Generic;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.PreGpm.Contracts;

namespace Mammoth.ApiController.QueueProcessors
{
    public class MammothItemLocaleQueueProcessor : MammothQueueProcessor<MessageQueueItemLocale, Contracts.items>
    {
        public MammothItemLocaleQueueProcessor(ILogger logger,
            IQueueReader<MessageQueueItemLocale, Contracts.items> queueReader,
            ISerializer<Contracts.items> serializer,
            ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>> saveToMessageHistoryCommandHandler,
            ICommandHandler<AssociateMessageToQueueCommand<MessageQueueItemLocale, MessageHistory>> associateMessageToQueueCommandHandler,
            ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueItemLocale>> setProcessedDateCommandHandler,
            ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>> updateMessageHistoryCommandHandler,
            ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueItemLocale>> updateMessageQueueStatusCommandHandler,
            ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>> markQueuedEntriesAsInProcessCommandHandler,
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
                  activeMqProducer,
                  settings)
        {
            esbMessageProperties = new Dictionary<string, string>
            {
                { "IconMessageID", "" },
                { "Source", settings.Source },
                { "nonReceivingSysName", settings.NonReceivingSystemsItemLocale },
				{"TransactionType" , "Item/Locale" }
			};
        }

        internal override int MessageType => MessageTypes.ItemLocale;

        protected override List<MessageQueueItemLocale> GetMessagesReadyToSerialize(List<MessageQueueItemLocale> messagesReadyForMiniBulk, Contracts.items miniBulk)
        {
            var itemsInMiniBulk = miniBulk.item.Select(i => i.id).ToList();
            return messagesReadyForMiniBulk.Where(m => itemsInMiniBulk.Contains(m.ItemId)).ToList();
        }

        protected override bool ShouldSendMessage(Contracts.items miniBulk)
        {
            return miniBulk.item.Length > 0;
        }
    }
}
