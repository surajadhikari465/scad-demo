using System.Collections.Generic;
using InventoryProducer.Producer.Model.DBModel;
using InventoryProducer.Common.InstockDequeue.Model;

using TransferOrdersCanonical = Icon.Esb.Schemas.Wfm.Contracts.transferOrders;

namespace InventoryProducer.Producer.Mapper
{
    public interface ITransferOrderCanonicalMapper
    {
        TransferOrdersCanonical TransformToXmlCanonical(IList<TransferOrdersModel> transferOrdersList, 
            InstockDequeueResult instockDequeueResult);

        string SerializeToXml(TransferOrdersCanonical xmlCanonical);
    }
}
