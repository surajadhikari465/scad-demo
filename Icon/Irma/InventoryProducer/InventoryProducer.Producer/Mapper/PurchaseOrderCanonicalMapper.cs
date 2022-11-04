using System;
using System.Collections.Generic;
using Icon.Esb.Schemas.Wfm.Contracts;
using InventoryProducer.Producer.Model.DBModel;
using InventoryProducer.Common;
using InventoryProducer.Common.InstockDequeue.Model;
using InventoryProducer.Common.Serializers;
using System.Linq;

namespace InventoryProducer.Producer.Mapper
{
    // Transforms PurchaseOrdersModel (DB model) to XML Canonical model
    public class PurchaseOrderXmlCanonicalMapper : ICanonicalMapper<PurchaseOrders, PurchaseOrdersModel>
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

        private PurchaseOrdersPurchaseOrderDelete MapDeleteEvent(IList<PurchaseOrdersModel> purchaseOrderList,
            InstockDequeueResult instockDequeueMessage)
        {
            string eventType = instockDequeueMessage.InstockDequeueModel.EventTypeCode;
            string messageNumber = instockDequeueMessage.Headers[Constants.MessageProperty.MessageNumber];

            PurchaseOrdersModel purchaseOrder = purchaseOrderList[0];
            PurchaseOrdersPurchaseOrderDelete canonicalDelete = new PurchaseOrdersPurchaseOrderDelete()
            {
                purchaseOrderNumber = purchaseOrder.OrderHeaderId.ToString(),
                eventType = eventType,
                messageNumber = messageNumber,
                locationName = purchaseOrder.LocationName,
                locationNumber = purchaseOrder.LocationNumber,
                cancelUserInfo = new UserType()
                {
                    idNumber = purchaseOrder.UserId > 0 ? purchaseOrder.UserId.ToString() : "",
                    name = purchaseOrder.Username
                }

            };

            if (purchaseOrder.ExternalOrderId.HasValue)
            {
                canonicalDelete.externalSource = purchaseOrder.ExternalSource;
                canonicalDelete.externalPurchaseOrderNumber = purchaseOrder.ExternalOrderId.Value.ToString();
            }

            if (purchaseOrder.OtherOrderExternalSourceOrderID.HasValue)
            {
                ExternalPurchaseOrderReference externalPurchaseOrderRef = null;
                if (purchaseOrder.ExternalOrderId.HasValue)
                {
                    externalPurchaseOrderRef = new ExternalPurchaseOrderReference()
                    {
                        externalPurchaseOrderNumber = purchaseOrder.ExternalOrderId.Value.ToString(),
                        externalSource = purchaseOrder.ExternalSource
                    };
                }
                if (purchaseOrder.OtherOrderExternalSourceOrderID.HasValue)
                {
                    externalPurchaseOrderRef = new ExternalPurchaseOrderReference()
                    {
                        externalPurchaseOrderNumber = purchaseOrder.OtherOrderExternalSourceOrderID.Value.ToString(),
                        externalSource = purchaseOrder.OtherExternalSourceDescription
                    };
                }
                canonicalDelete.externalPurchaseOrderReferences = new ExternalPurchaseOrderReferences()
                {
                    externalPurchaseOrderReference = externalPurchaseOrderRef
                };
            }

            canonicalDelete.purchaseOrderDeletionDetail = CreatePurchaseOrderDeletionDetail(purchaseOrderList, eventType);

            return canonicalDelete;
        }

        private PurchaseOrdersPurchaseOrder MapNonDeleteEvent(IList<PurchaseOrdersModel> purchaseOrderList,
            InstockDequeueResult instockDequeueMessage)
        {
            string eventType = instockDequeueMessage.InstockDequeueModel.EventTypeCode;
            string messageNumber = instockDequeueMessage.Headers[Constants.MessageProperty.MessageNumber];

            PurchaseOrdersModel purchaseOrder = purchaseOrderList[0];
            PurchaseOrdersPurchaseOrder canonicalNonDelete = new PurchaseOrdersPurchaseOrder()
            {
                purchaseOrderNumber = purchaseOrder.OrderHeaderId.ToString(),
                externalPurchaseOrderNumber = Convert.ToString(purchaseOrder.ExternalOrderId),
                eventType = eventType,
                messageNumber = messageNumber,
                purchaseType = purchaseOrder.PurchaseType,
                supplierNumber = purchaseOrder.SupplierNumber,
                locationNumber = purchaseOrder.LocationNumber,
                locationName = purchaseOrder.LocationName,
                OrderSubTeam = new OrderSubTeamType()
                {
                    orderSubTeamName = purchaseOrder.OrderSubTeamName,
                    orderSubTeamNumber = Convert.ToString(purchaseOrder.OrderSubTeamNo),
                    orderTeamName = purchaseOrder.OrderTeamName,
                    orderTeamNumber = Convert.ToString(purchaseOrder.OrderTeamNo),
                },
                status = purchaseOrder.Status,
                userInfo = new UserType()
                {
                    idNumber = purchaseOrder.UserId.ToString(),
                    name = purchaseOrder.Username
                },
                createDateTime = purchaseOrder.CreateDateTime.GetValueOrDefault(),
                approveDateTimeSpecified = purchaseOrder.ApproveDateTime.HasValue,
                closeDateTimeSpecified = purchaseOrder.CloseDateTime.HasValue,
                approveDateTime = purchaseOrder.ApproveDateTime.GetValueOrDefault(),
                closeDateTime = purchaseOrder.CloseDateTime.GetValueOrDefault(),
                purchaseOrderComments = purchaseOrder.PurchaseOrderComments,
            };

            if (purchaseOrder.ExternalOrderId.HasValue)
            {
                canonicalNonDelete.externalSource = purchaseOrder.ExternalSource;
            }

            canonicalNonDelete.PurchaseOrderDetail = CreatePurchaseOrderDetail(purchaseOrderList);

            return canonicalNonDelete;
        }

        private PurchaseOrdersPurchaseOrderPurchaseOrderDetail[] CreatePurchaseOrderDetail(IList<PurchaseOrdersModel> purchaseOrderList)
        {
            IList<PurchaseOrdersPurchaseOrderPurchaseOrderDetail> detailList = new List<PurchaseOrdersPurchaseOrderPurchaseOrderDetail>();

            foreach (var purchaseOrder in purchaseOrderList)
            {
                bool uomCodeSpecified = Enum.TryParse(
                    purchaseOrder.OrderedUnitCode.Trim().ToUpper(),
                    out WfmUomCodeEnumType uomCode
                );
                detailList.Add(new PurchaseOrdersPurchaseOrderPurchaseOrderDetail()
                {
                    PurchaseOrderDetailNumber = purchaseOrder.PurchaseOrderDetailNumber.GetValueOrDefault(),
                    sourceItemKey = Convert.ToString(purchaseOrder.SourceItemKey),
                    locationId = Convert.ToString(purchaseOrder.LocationNumber),
                    SubTeam = new SubTeamType()
                    {
                        subTeamName = purchaseOrder.OrderSubTeamName,
                        subTeamNumber = Convert.ToString(purchaseOrder.OrderSubTeamNo),
                        hostSubTeamName = purchaseOrder.HostSubTeamName,
                        hostSubTeamNumber = Convert.ToString(purchaseOrder.HostSubTeamNumber)
                    },
                    defaultScanCode = purchaseOrder.DefaultScanCode,
                    quantities = new QuantityType[]
                    {
                        new QuantityType(){
                            value = purchaseOrder.QuantityOrdered.GetValueOrDefault(),
                            units = new UnitsType()
                            {
                                uom = new UomType()
                                {
                                    code = uomCode,
                                    codeSpecified = uomCodeSpecified
                                }
                            }
                        }
                    },
                    eInvoiceASNQuantities = new QuantityType[]
                    {
                        new QuantityType()
                        {
                            value = purchaseOrder.EInvoiceQuantity.GetValueOrDefault(),
                            units = new UnitsType()
                            {
                                uom = new UomType()
                                {
                                    code = uomCode,
                                    codeSpecified = purchaseOrder.EInvoiceQuantity.HasValue
                                }
                            }
                        }
                    },
                    eInvoiceASNWeights = new QuantityType[]
                    {
                        new QuantityType()
                        {
                            value = purchaseOrder.EInvoiceWeight.GetValueOrDefault(),
                            units = new UnitsType()
                            {
                                uom = new UomType()
                                {
                                    code = uomCode,
                                    codeSpecified = purchaseOrder.EInvoiceQuantity.HasValue
                                }
                            }
                        }
                    },
                    packSize1 = purchaseOrder.PackSize1.GetValueOrDefault(),
                    packSize2 = purchaseOrder.PackSize2.GetValueOrDefault(),
                    itemCost = purchaseOrder.ItemCost.ToString(),
                    earliestArrivalDate = purchaseOrder.EarliestArrivalDate.GetValueOrDefault(),
                    expectedArrivalDate = purchaseOrder.ExpectedArrivalDate.GetValueOrDefault(),
                    earliestArrivalDateSpecified = purchaseOrder.EarliestArrivalDate.HasValue,
                    expectedArrivalDateSpecified = purchaseOrder.ExpectedArrivalDate.HasValue,
                });
            }

            return detailList.ToArray();
        }

        private PurchaseOrderDeletionDetailType[] CreatePurchaseOrderDeletionDetail(IList<PurchaseOrdersModel> purchaseOrderList, string eventType)
        {
            IList<PurchaseOrderDeletionDetailType> deletionDetailList = new List<PurchaseOrderDeletionDetailType>();

            foreach (var purchaseOrder in purchaseOrderList)
            {
                PurchaseOrderDeletionDetailType purchaseOrderDeletionDetailType = new PurchaseOrderDeletionDetailType();
                if (Constants.EventType.PO_LINE_DEL.Equals(eventType))
                {
                    purchaseOrderDeletionDetailType.PurchaseOrderDetailNumber = purchaseOrder.PurchaseOrderDetailNumber.GetValueOrDefault();
                    purchaseOrderDeletionDetailType.PurchaseOrderDetailNumberSpecified = purchaseOrder.PurchaseOrderDetailNumber.HasValue;
                    purchaseOrderDeletionDetailType.sourceItemKey = purchaseOrder.SourceItemKey.ToString();
                    purchaseOrderDeletionDetailType.defaultScanCode = purchaseOrder.DefaultScanCode;
                }
                deletionDetailList.Add(purchaseOrderDeletionDetailType);
            }

            return deletionDetailList.ToArray();
        }

        public string SerializeToXml(PurchaseOrders purchaseOrdersCanonical)
        {
            return xmlSerializer.Serialize(purchaseOrdersCanonical, new Utf8StringWriter());
        }
    }
}
