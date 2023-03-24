using Icon.ActiveMQ.Producer;
using Icon.Common;
using InvConstants = InventoryProducer.Common.Constants;
using TransactionType = InventoryProducer.Common.Constants.TransactionType;
using System;
using System.Collections.Generic;

namespace InventoryProducer.Producer.Publish
{
    internal class RepublishInventoryMessagePublisher : IMessagePublisher
    {
        IActiveMQDynamicProducer activeMqProducer;

        public RepublishInventoryMessagePublisher(IActiveMQDynamicProducer activeMqProducer)
        {
            this.activeMqProducer = activeMqProducer;
        }

        public void PublishMessage(string message, Dictionary<string, string> messageProperties, Action onSuccess, Action<Exception> onFailure)
        {
            activeMqProducer.Send(
                GetActiveMqQueueName(messageProperties[InvConstants.MessageProperty.TransactionType]),
                message,
                messageProperties
            );
        }

        private string GetActiveMqQueueName(string transactionType)
        {
            if (transactionType == TransactionType.InventorySpoilage) 
            { 
                return AppSettingsAccessor.GetStringSetting("ActiveMqInventorySpoilageQueueName"); 
            }
            else if (transactionType == TransactionType.PurchaseOrders)
            {
                return AppSettingsAccessor.GetStringSetting("ActiveMqInventoryPurchaseOrderQueueName");
            }
            else if (transactionType == TransactionType.ReceiptOrders)
            {
                return AppSettingsAccessor.GetStringSetting("ActiveMqInventoryReceiveQueueName");
            }
            else if (transactionType == TransactionType.TransferOrders)
            {
                return AppSettingsAccessor.GetStringSetting("ActiveMqInventoryTransferQueueName");
            }
            else
            {
                throw new NotImplementedException($"Queue for TransactionType: {transactionType} is not present.");
            }
        }
    }
}
