using InventoryProducer.Common;

namespace InventoryProducer.Producer.ProducerBuilders
{
    public interface IProducerBuilder
    {
        InventoryProducerBase ComposeProducer(InventoryProducerSettings inventoryProducerSettings);
    }
}
