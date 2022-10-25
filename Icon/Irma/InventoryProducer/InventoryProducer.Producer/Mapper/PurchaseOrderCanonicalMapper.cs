using System;
using System.Collections.Generic;
using Icon.Esb.Schemas.Wfm.Contracts;
using InventoryProducer.Producer.Model.DBModel;
using InventoryProducer.Common;
using InventoryProducer.Common.InstockDequeue.Model;
using InventoryProducer.Common.Serializers;


namespace InventoryProducer.Producer.Mapper
{
    // Transforms PurchaseOrdersModel (DB model) to XML Canonical model
    public class PurchaseOrderXmlCanonicalMapper: ICanonicalMapper<PurchaseOrders, PurchaseOrdersModel>
    {

        private readonly ISerializer<PurchaseOrders> xmlSerializer;

        public PurchaseOrderXmlCanonicalMapper(ISerializer<PurchaseOrders> xmlSerializer)
        {
            this.xmlSerializer = xmlSerializer;
        }

        public PurchaseOrders TransformToXmlCanonical(IList<PurchaseOrdersModel> purchaseOrdersList, 
            InstockDequeueResult instockDequeueMessage)
        {
            PurchaseOrders purchaseOrderCanonical = new PurchaseOrders
            {
                Items = new object[1]
            };

            string eventType = instockDequeueMessage.InstockDequeueModel.EventTypeCode;

            if (eventType.Contains(Constants.EventType.PO_DEL) || 
                eventType.Contains(Constants.EventType.PO_LINE_DEL))
            {
                purchaseOrderCanonical.Items[0] = this.MapDeleteEvent(purchaseOrdersList, instockDequeueMessage);

            }
            else
            {
                purchaseOrderCanonical.Items[0] = this.MapNonDeleteEvent(purchaseOrdersList, instockDequeueMessage);
            }
            
            return purchaseOrderCanonical;
        }

        private PurchaseOrdersPurchaseOrderDelete MapDeleteEvent(IList<PurchaseOrdersModel> transferOrdersList, 
            InstockDequeueResult instockDequeueMessage)
        {
            throw new NotImplementedException();
        }

        private PurchaseOrdersPurchaseOrder MapNonDeleteEvent(IList<PurchaseOrdersModel> purchaseOrderList, 
            InstockDequeueResult instockDequeueMessage)
        {
            throw new NotImplementedException();
        }

        public string SerializeToXml(PurchaseOrders purchaseOrdersCanonical)
        {
            return xmlSerializer.Serialize(purchaseOrdersCanonical, new Utf8StringWriter());
        }
    }
}
