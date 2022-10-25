using InventoryProducer.Common.InstockDequeue.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryProducer.Producer.Mapper
{
    public interface ICanonicalMapper<CanonicalType, InventoryModel>
    {
        CanonicalType TransformToXmlCanonical(IList<InventoryModel> transferOrdersList,
            InstockDequeueResult instockDequeueResult);

        string SerializeToXml(CanonicalType xmlCanonical);
    }
}
