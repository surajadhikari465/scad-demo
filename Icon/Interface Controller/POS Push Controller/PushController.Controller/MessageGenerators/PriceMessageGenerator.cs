using Icon.Framework;
using PushController.Controller.MessageBuilders;
using PushController.Controller.MessageQueueServices;
using System;
using System.Collections.Generic;

namespace PushController.Controller.MessageGenerators
{
    public class PriceMessageGenerator : IMessageGenerator<MessageQueuePrice>
    {
        private IMessageBuilder<MessageQueuePrice> priceMessageBuilder;
        private IMessageQueueService<MessageQueuePrice> priceMessageQueueService;

        public PriceMessageGenerator(
            IMessageBuilder<MessageQueuePrice> priceMessageBuilder,
            IMessageQueueService<MessageQueuePrice> priceMessageQueueService)
        {
            this.priceMessageBuilder = priceMessageBuilder;
            this.priceMessageQueueService = priceMessageQueueService;
        }

        public List<MessageQueuePrice> BuildMessages(List<IRMAPush> posDataReadyForEsb)
        {
            return priceMessageBuilder.BuildMessages(posDataReadyForEsb);
        }

        public void SaveMessages(List<MessageQueuePrice> messagesToSave)
        {
            try
            {
                priceMessageQueueService.SaveMessagesBulk(messagesToSave);
            }
            catch (Exception)
            {
                SaveMessagesRowByRow(messagesToSave);
            }
        }

        private void SaveMessagesRowByRow(List<MessageQueuePrice> messagesToSave)
        {
            priceMessageQueueService.SaveMessagesRowByRow(messagesToSave);
        }
    }
}
