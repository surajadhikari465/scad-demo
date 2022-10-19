using System.Collections.Generic;
using InventoryProducer.Producer.Model.DBModel;
using InventoryProducer.Common.InstockDequeue.Model;

using PurchaseOrdersCanonical = Icon.Esb.Schemas.Wfm.Contracts.PurchaseOrders;

namespace InventoryProducer.Producer.Mapper
{
    public interface IPurchaseOrderCanonicalMapper
    {
        PurchaseOrdersCanonical TransformToXmlCanonical(IList<PurchaseOrdersModel> transferOrdersList, 
            InstockDequeueResult instockDequeueResult);

        string SerializeToXml(PurchaseOrdersCanonical xmlCanonical);
    }
}
