using Icon.Framework;
using PushController.Controller.MessageBuilders;
using PushController.Controller.MessageQueueServices;
using System;
using System.Collections.Generic;

namespace PushController.Controller.MessageGenerators
{
    public class ItemLocaleMessageGenerator : IMessageGenerator<MessageQueueItemLocale>
    {
        private IMessageBuilder<MessageQueueItemLocale> itemLocaleMessageBuilder;
        private IMessageQueueService<MessageQueueItemLocale> itemLocaleMessageQueueService;
        
        public ItemLocaleMessageGenerator(
            IMessageBuilder<MessageQueueItemLocale> itemLocaleMessageBuilder,
            IMessageQueueService<MessageQueueItemLocale> itemLocaleMessageQueueService)
        {
            this.itemLocaleMessageBuilder = itemLocaleMessageBuilder;
            this.itemLocaleMessageQueueService = itemLocaleMessageQueueService;
        }

        public List<MessageQueueItemLocale> BuildMessages(List<IRMAPush> posDataReadyForEsb)
        {
            return itemLocaleMessageBuilder.BuildMessages(posDataReadyForEsb);
        }

        public void SaveMessages(List<MessageQueueItemLocale> messagesToSave)
        {
            try
            {
                itemLocaleMessageQueueService.SaveMessagesBulk(messagesToSave);
            }
            catch (Exception)
            {
                SaveMessagesRowByRow(messagesToSave);
            }
        }

        private void SaveMessagesRowByRow(List<MessageQueueItemLocale> messagesToSave)
        {
            itemLocaleMessageQueueService.SaveMessagesRowByRow(messagesToSave);
        }
    }
}
