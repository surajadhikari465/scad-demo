using InventoryProducer.Common;
using InventoryProducer.Producer.QueueProcessors;
using System;
using System.Reflection;
using System.Threading;

namespace InventoryProducer.Producer
{
    public class InventoryProducerBase
    {
        private readonly InventoryLogger<InventoryProducerBase> inventoryLogger;
        private readonly IQueueProcessor queueProcessor;

        public InventoryProducerBase(
            InventoryLogger<InventoryProducerBase> inventoryLogger,
            IQueueProcessor queueProcessor)
        {
            this.inventoryLogger = inventoryLogger;
            this.queueProcessor = queueProcessor;
        }

        public void Execute()
        {
            try
            {
                queueProcessor.ProcessMessageQueue();
            }
            catch (Exception ex)
            {
                inventoryLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());

                // Sleep on unhandled exception to prevent logging floods.
                Thread.Sleep(30000);

                return;
            }
        }
    }
}
