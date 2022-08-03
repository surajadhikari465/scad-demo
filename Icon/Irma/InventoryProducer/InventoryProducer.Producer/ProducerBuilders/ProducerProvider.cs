using InventoryProducer.Common;
using System;
using System.Collections.Generic;

namespace InventoryProducer.Producer.ProducerBuilders
{
    public class ProducerProvider
    {
        private static IDictionary<string, Func<IProducerBuilder>> producerMap = new Dictionary<string, Func<IProducerBuilder>>(StringComparer.InvariantCultureIgnoreCase)
        {
            {
                "spoilage", () => new InventorySpoilageProducerBuilder()
            }
        };

        public static InventoryProducerBase GetProducer(InventoryProducerSettings settings)
        {
            try
            {
                return producerMap[settings.ProducerType]().ComposeProducer(settings);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
    }
}
