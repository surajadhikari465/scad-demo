using System;
using System.Collections.Generic;
using Icon.Esb.Schemas.Wfm.Contracts;
using InventoryProducer.Producer.Model.DBModel;
using InventoryProducer.Common;
using InventoryProducer.Common.InstockDequeue.Model;
using InventoryProducer.Common.Serializers;
using OrderReceipts = Icon.Esb.Schemas.Wfm.Contracts.orderReceipts;


namespace InventoryProducer.Producer.Mapper
{
    // Transforms ReceiveModel (DB model) to XML Canonical model
    public class ReceiveXmlCanonicalMapper : ICanonicalMapper<OrderReceipts, ReceiveModel>
    {

        private readonly ISerializer<OrderReceipts> xmlSerializer;

        public ReceiveXmlCanonicalMapper(ISerializer<OrderReceipts> xmlSerializer)
        {
            this.xmlSerializer = xmlSerializer;
        }

        public OrderReceipts TransformToXmlCanonical(IList<ReceiveModel> receiveList,
            InstockDequeueResult instockDequeueMessage)
        {
            OrderReceipts orderReceiptsCanonical = new orderReceipts
            {
                orderReceipt = new orderReceiptsOrderReceipt[1]
            };

            orderReceiptsCanonical.orderReceipt[0] = this.GetReceiveCanonical(receiveList, instockDequeueMessage);
            return orderReceiptsCanonical;
        }

        private orderReceiptsOrderReceipt GetReceiveCanonical(IList<ReceiveModel> receiveList,
            InstockDequeueResult instockDequeueMessage)
        {
            string eventType = instockDequeueMessage.InstockDequeueModel.EventTypeCode;
            string messageNumber = instockDequeueMessage.Headers[Constants.MessageProperty.MessageNumber];
            ReceiveModel receiveItem = receiveList[0];

            orderReceiptsOrderReceipt receiveCanonical = new orderReceiptsOrderReceipt
            {
                receiptNumber = receiveItem.OrderHeaderId,
                purchaseOrderNumber = receiveItem.OrderHeaderId.ToString(),
                locationNumber = receiveItem.StoreNumber,
                locationName = receiveItem.StoreName,
                eventType = eventType,
                messageNumber = messageNumber,
                receiptStatus = receiveItem.ReceiptStatus,
                isPastReceipt = (receiveItem.PastReceiptDate.Equals(null)) ? "false" : "true",
                pastRceiptDate = GetDateTimeOffset(receiveItem.PastReceiptDate),
                pastRceiptDateSpecified = receiveItem.PastReceiptDate.Equals(null) ? false : true,
                purchaseOrderCreateDateTime = GetDateTimeOffset(receiveItem.CreateDateTime),
                purchaseOrderSupplierNumber = receiveItem.SupplierNumber,
                receiptDetail = CreateReceiptDetail(receiveList),
            };
            return receiveCanonical;
        }

        private orderReceiptsOrderReceiptReceiptDetail[] CreateReceiptDetail(IList<ReceiveModel> receiveList)
        {
            int sizeOfReceiveList = receiveList.Count;
            orderReceiptsOrderReceiptReceiptDetail[] result = new orderReceiptsOrderReceiptReceiptDetail[sizeOfReceiveList];
            for (int i = 0; i < sizeOfReceiveList; i++)
            {
                result[i] = new orderReceiptsOrderReceiptReceiptDetail
                {
                    receiptDetailNumber = receiveList[i].OrderItemId,
                    purchaseOrderDetailNumber = receiveList[i].OrderItemId,
                    sourceItemKey = receiveList[i].ItemKey.ToString(),
                    SubTeam = new SubTeamType
                    {
                        subTeamNumber = receiveList[i].SubTeamNumber.ToString(),
                        subTeamName = receiveList[i].SubTeam,
                        hostSubTeamNumber = receiveList[i].HostSubTeamNumber.ToString(),
                        hostSubTeamName = receiveList[i].HostSubTeam,
                    },
                    itemVIN = receiveList[i].VIN,
                    defaultScanCode = receiveList[i].Identifier,
                    quantityOrdered = receiveList[i].CreditPO == 0 ? (int)receiveList[i].QuantityOrdered : (int)(-1 * receiveList[i].QuantityOrdered),
                    quantityReceived = receiveList[i].CreditPO == 0 ? (int)receiveList[i].QuantityReceived : (int)(-1 * receiveList[i].QuantityReceived),
                    receiptStatus = receiveList[i].ReceiptStatus,
                    receiptUoM = receiveList[i].ReceiptUom,
                    cartonNumberSpecified = false,
                    packSize1 = (int)Math.Round(receiveList[i].PackageDesc1),
                    packSize2 = (int)Math.Round(receiveList[i].PackageDesc2),
                    packSize2Specified = true,
                    receiptUserInfo = new UserType
                    {
                        idNumber = receiveList[i].RecvLogUserId.ToString(),
                        name = receiveList[i].RecvUserName
                    },
                    createDateTime = receiveList[i].DateReceived != null ? GetDateTimeOffset(receiveList[i].DateReceived) : DateTimeOffset.Now.ToString("O"),
                    documentNumberSpecified = false
                };
            }
            return result;
        }

        private string GetDateTimeOffset(DateTime? dateTime)
        {
            if (dateTime.HasValue)
                return new DateTimeOffset((DateTime)dateTime, new TimeSpan(-5, 0, 0)).ToString("O");
            else
                return string.Empty;
        }

        public string SerializeToXml(OrderReceipts orderReceiptsCanonical)
        {
            return xmlSerializer.Serialize(orderReceiptsCanonical, new Utf8StringWriter());
        }
    }
}
