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
        private const string DATETIME_FORMAT = "yyyy-MM-ddTHH:mm:sszzz";
        private const string DATETIME_FORMAT_WITH_MS = "yyyy-MM-ddTHH:mm:ss{}zzz";  //Milliseconds will be filled using FormatDateWithMS function
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
                locationName = purchaseOrder.LocationName != null ? purchaseOrder.LocationName : "",
                locationNumber = purchaseOrder.LocationNumber,
                cancelUserInfo = new UserType()
                {
                    idNumber = purchaseOrder.UserId > 0 ? purchaseOrder.UserId.ToString() : "",
                    name = purchaseOrder.Username != null ? purchaseOrder.Username : ""
                }
            };

            if (purchaseOrder.ExternalOrderId.HasValue)
            {
                canonicalDelete.externalSource = purchaseOrder.ExternalSource;
                canonicalDelete.externalPurchaseOrderNumber = purchaseOrder.ExternalOrderId.Value.ToString();
            }

            if (purchaseOrder.OtherOrderExternalSourceOrderID.HasValue)
            {
                List<ExternalPurchaseOrderReference> externalPurchaseOrderReferenceList = new List<ExternalPurchaseOrderReference>();
                if (purchaseOrder.ExternalOrderId.HasValue)
                {
                    externalPurchaseOrderReferenceList.Add(new ExternalPurchaseOrderReference()
                    {
                        externalPurchaseOrderNumber = purchaseOrder.ExternalOrderId.Value.ToString(),
                        externalSource = purchaseOrder.ExternalSource
                    });
                }
                if (purchaseOrder.OtherOrderExternalSourceOrderID.HasValue)
                {
                    externalPurchaseOrderReferenceList.Add(new ExternalPurchaseOrderReference()
                    {
                        externalPurchaseOrderNumber = purchaseOrder.OtherOrderExternalSourceOrderID.Value.ToString(),
                        externalSource = purchaseOrder.OtherExternalSourceDescription
                    });
                }
                canonicalDelete.externalPurchaseOrderReferences = externalPurchaseOrderReferenceList.ToArray();
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
                eventType = eventType,
                messageNumber = messageNumber,
                purchaseType = purchaseOrder.PurchaseType,
                supplierNumber = purchaseOrder.SupplierNumber,
                supplierName = purchaseOrder.SupplierName,
                invoiceNumber = purchaseOrder.InvoiceNumber,
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
                createDateTime = purchaseOrder.CreateDateTime.GetValueOrDefault().ToString(DATETIME_FORMAT),
                approveDateTimeSpecified = purchaseOrder.ApproveDateTime.HasValue,
                closeDateTimeSpecified = purchaseOrder.CloseDateTime.HasValue,
                approveDateTime = purchaseOrder.ApproveDateTime.GetValueOrDefault().ToString(DATETIME_FORMAT),
                closeDateTime = FormatDateWithMS(purchaseOrder.CloseDateTime.GetValueOrDefault()),
                purchaseOrderComments = purchaseOrder.PurchaseOrderComments,
            };

            if (purchaseOrder.ExternalOrderId.HasValue)
            {
                canonicalNonDelete.externalPurchaseOrderNumber = Convert.ToString(purchaseOrder.ExternalOrderId);
                canonicalNonDelete.externalSource = purchaseOrder.ExternalSource;
            }

            if (purchaseOrder.OtherOrderExternalSourceOrderID.HasValue)
            {
                List<ExternalPurchaseOrderReference> externalPurchaseOrderReferenceList = new List<ExternalPurchaseOrderReference>();
                if (purchaseOrder.ExternalOrderId.HasValue)
                {
                    externalPurchaseOrderReferenceList.Add(new ExternalPurchaseOrderReference()
                    {
                        externalPurchaseOrderNumber = purchaseOrder.ExternalOrderId.Value.ToString(),
                        externalSource = purchaseOrder.ExternalSource
                    });
                }
                if (purchaseOrder.OtherOrderExternalSourceOrderID.HasValue)
                {
                    externalPurchaseOrderReferenceList.Add(new ExternalPurchaseOrderReference()
                    {
                        externalPurchaseOrderNumber = purchaseOrder.OtherOrderExternalSourceOrderID.Value.ToString(),
                        externalSource = purchaseOrder.OtherExternalSourceDescription
                    });
                }
                canonicalNonDelete.externalPurchaseOrderReferences = externalPurchaseOrderReferenceList.ToArray();
            }

            canonicalNonDelete.PurchaseOrderDetail = CreatePurchaseOrderDetail(purchaseOrderList);

            return canonicalNonDelete;
        }

        private string FormatDateWithMS(DateTime rawDateTime)
        {
            string dateTime = rawDateTime.ToString(DATETIME_FORMAT_WITH_MS);
            string ms = Helper.ConvertToG29String(rawDateTime.Millisecond * 0.001M);
            return dateTime.Replace("{}", ms.TrimStart('0'));
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
                bool eInvoiceWeightUomCodeSpecified = Enum.TryParse(
                    purchaseOrder.RetailUnit.Trim().ToUpper(),
                    out WfmUomCodeEnumType weightUomCode
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
                    vendorItemNumber = purchaseOrder.VendorItemNumber,
                    costedByWeight = purchaseOrder.CostedByWeight,
                    quantities = new QuantityType[]
                    {
                        new QuantityType(){
                            value = purchaseOrder.QuantityOrdered,
                            valueSpecified = purchaseOrder.QuantityOrdered.HasValue,
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
                            value = purchaseOrder.EInvoiceQuantity,
                            valueSpecified = purchaseOrder.EInvoiceQuantity.HasValue,
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
                            value = purchaseOrder.EInvoiceWeight,
                            valueSpecified = purchaseOrder.EInvoiceWeight.HasValue,
                            units = new UnitsType()
                            {
                                uom = new UomType()
                                {
                                    code = weightUomCode,
                                    codeSpecified = eInvoiceWeightUomCodeSpecified
                                }
                            }
                        }
                    },
                    packSize1 = purchaseOrder.PackSize1.GetValueOrDefault(),
                    packSize2 = purchaseOrder.PackSize2.GetValueOrDefault(),
                    itemCost = Helper.ConvertToG29String(purchaseOrder.ItemCost),
                    earliestArrivalDate = FormatDateWithMS(purchaseOrder.EarliestArrivalDate.GetValueOrDefault()),
                    expectedArrivalDate = purchaseOrder.ExpectedArrivalDate.GetValueOrDefault().ToString(DATETIME_FORMAT),
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
