using System;
using System.Collections.Generic;
using Icon.Esb.Schemas.Wfm.Contracts;
using InventoryProducer.Producer.Model.DBModel;
using InventoryProducer.Common;
using InventoryProducer.Common.InstockDequeue.Model;
using InventoryProducer.Common.Serializers;

using TransferOrdersCanonical = Icon.Esb.Schemas.Wfm.Contracts.transferOrders;
using TransferOrdersDeleteCanonical = Icon.Esb.Schemas.Wfm.Contracts.transferOrdersTransferOrderDelete;
using TransferOrdersNonDeleteCanonical = Icon.Esb.Schemas.Wfm.Contracts.transferOrdersTransferOrder;


namespace InventoryProducer.Producer.Mapper
{
    // Transforms TransferOrdersModel (DB model) to XML Canonical model
    public class TransferOrderXmlCanonicalMapper: ITransferOrderCanonicalMapper
    {

        private readonly ISerializer<transferOrders> xmlSerializer;

        public TransferOrderXmlCanonicalMapper(ISerializer<transferOrders> xmlSerializer)
        {
            this.xmlSerializer = xmlSerializer;
        }

        public TransferOrdersCanonical TransformToXmlCanonical(IList<TransferOrdersModel> transferOrdersList, 
            InstockDequeueResult instockDequeueMessage)
        {
            TransferOrdersCanonical transferOrdersCanonical = new TransferOrdersCanonical
            {
                Items = new object[1]
            };

            string eventType = instockDequeueMessage.InstockDequeueModel.EventTypeCode;

            if (eventType.Contains(Constants.EventType.TSF_DEL) || 
                eventType.Contains(Constants.EventType.TSF_LINE_DEL))
            {
                transferOrdersCanonical.Items[0] = this.MapDeleteEvent(transferOrdersList, instockDequeueMessage);

            }
            else
            {
                transferOrdersCanonical.Items[0] = this.MapNonDeleteEvent(transferOrdersList, instockDequeueMessage);
            }
            
            return transferOrdersCanonical;
        }

        private TransferOrdersDeleteCanonical MapDeleteEvent(IList<TransferOrdersModel> transferOrdersList, 
            InstockDequeueResult instockDequeueMessage)
        {
            string eventType = instockDequeueMessage.InstockDequeueModel.EventTypeCode;
            string messageNumber = instockDequeueMessage.Headers[Constants.MessageProperty.MessageNumber];

            TransferOrdersModel transferOrdersItem = transferOrdersList[0];
            TransferOrdersDeleteCanonical deleteCanonical = new TransferOrdersDeleteCanonical
            {
                transferNumber = transferOrdersItem.OrderHeaderId,
                eventType = eventType,
                messageNumber = messageNumber,
                locationNumber = (int) transferOrdersItem.ToLocationNumber,
                locationName = transferOrdersItem.ToLocationName,
                cancelUserInfo = new UserType
                {
                    idNumber = transferOrdersItem.UserId > 0 ? transferOrdersItem.UserId.ToString() : "",
                    name = transferOrdersItem.UserName
                },
                transferOrderDeletionDetail = this.CreateDeletionDetailItemsCanonical(transferOrdersList,
                eventType)
            };
            return deleteCanonical;
        }

        private TransferOrdersNonDeleteCanonical MapNonDeleteEvent(IList<TransferOrdersModel> transferOrdersList, 
            InstockDequeueResult instockDequeueMessage)
        {
            string eventType = instockDequeueMessage.InstockDequeueModel.EventTypeCode;
            string messageNumber = instockDequeueMessage.Headers[Constants.MessageProperty.MessageNumber];

            TransferOrdersModel transferOrdersItem = transferOrdersList[0];
            TransferOrdersNonDeleteCanonical nonDeleteCanonical = new TransferOrdersNonDeleteCanonical
            {
                transferNumber = transferOrdersItem.OrderHeaderId.ToString(),
                eventType = eventType,
                messageNumber = messageNumber,
                locationChange = new LocationChangeType
                {
                    fromLocationNumber = transferOrdersItem.FromLocationNumber.ToString(),
                    fromLocationName = transferOrdersItem.FromLocationName,
                    toLocationNumber = transferOrdersItem.ToLocationNumber.ToString(),
                    toLocationName = transferOrdersItem.ToLocationName
                },
                subTeamChange = new SubTeamChangeType
                {
                    fromSubTeamNumber = transferOrdersItem.FromSubTeamNumber.ToString(),
                    fromSubTeamName = transferOrdersItem.FromSubTeamName,
                    toSubTeamNumber = transferOrdersItem.ToSubTeamNumber.ToString(),
                    toSubTeamName = transferOrdersItem.ToSubTeamName
                },
                userInfo = new UserType
                {
                    idNumber = transferOrdersItem.UserId.ToString(),
                    name = transferOrdersItem.UserName
                },
                createDateTime = this.GetDateTimeOffset(transferOrdersItem.CreateDateTime),
                approveDateTime = this.GetDateTimeOffset(transferOrdersItem.ApproveDateTime),
                approveDateTimeSpecified = transferOrdersItem.ApproveDateTime.HasValue,
                transferOrderDetail = this.CreateTransferOrderDetailItemsCanonical(transferOrdersList)
            };

            return nonDeleteCanonical;
        }

        private TransferOrderDeletionDetailType[] CreateDeletionDetailItemsCanonical(
            IList<TransferOrdersModel> transferOrdersList, string eventType)
        {
            TransferOrderDeletionDetailType[] result = new TransferOrderDeletionDetailType[transferOrdersList.Count];

            for (int i = 0; i < transferOrdersList.Count; i++)
            {
                TransferOrderDeletionDetailType deletionDetailCanonical = new TransferOrderDeletionDetailType();
                TransferOrdersModel transferOrdersItem = transferOrdersList[i];

                if (eventType.Contains(Constants.EventType.TSF_DEL))
                {
                    result[i] = deletionDetailCanonical;
                }
                else if (eventType.Contains(Constants.EventType.TSF_LINE_DEL))
                {
                    deletionDetailCanonical.transferOrderDetailNumber = transferOrdersItem.TransferOrderDetailNumber;
                    deletionDetailCanonical.transferOrderDetailNumberSpecified = transferOrdersItem.TransferOrderDetailNumber.HasValue;
                    deletionDetailCanonical.sourceItemKey = transferOrdersItem.SourceItemKey.ToString();
                    deletionDetailCanonical.defaultScanCode = transferOrdersItem.DefaultScanCode;
                    result[i] = deletionDetailCanonical;
                }
            }
            return result;
        }

        private TransferOrderDetailType[] CreateTransferOrderDetailItemsCanonical(
            IList<TransferOrdersModel> transferOrdersList)
        {
            TransferOrderDetailType[] result = new TransferOrderDetailType[transferOrdersList.Count];
            for(int i = 0; i < transferOrdersList.Count; i++)
            {
                TransferOrdersModel transferOrdersItem = transferOrdersList[i];
                TransferOrderDetailType detailType = new TransferOrderDetailType
                {
                    transferOrderDetailNumber = transferOrdersItem.TransferOrderDetailNumber,
                    transferOrderDetailNumberSpecified = transferOrdersItem.TransferOrderDetailNumber.HasValue, 
                    sourceItemKey = transferOrdersItem.SourceItemKey.ToString(),
                    defaultScanCode = transferOrdersItem.DefaultScanCode,
                    tsfStatus = transferOrdersItem.Status,
                    expectedArrivalDate = this.GetDateTimeOffset(transferOrdersItem.ExpectedArrivalDate),
                    expectedArrivalDateSpecified = transferOrdersItem.ExpectedArrivalDate.HasValue,
                    packSize1 = (int) Decimal.Round((decimal) transferOrdersItem.PackageDesc1),
                    packSize2 = (int?) Decimal.Round((decimal) transferOrdersItem.PackageDesc2),
                    packSize2Specified = transferOrdersItem.PackageDesc2.HasValue,
                    SubTeam = new SubTeamType
                    {
                        subTeamNumber = transferOrdersItem.ToSubTeamNumber.ToString(),
                        subTeamName = transferOrdersItem.ToSubTeamName,
                        hostSubTeamNumber = transferOrdersItem.HostSubTeamNumber.ToString(),
                        hostSubTeamName = transferOrdersItem.HostSubTeamName
                    },
                    transferQuantities = new QuantityType[1]
                };

                bool uomCodeSpecified = Enum.TryParse(
                    transferOrdersItem.OrderedUnitCode.Trim().ToUpper(),
                    out WfmUomCodeEnumType uomCode
                );

                detailType.transferQuantities[0] = new QuantityType
                {
                    value = transferOrdersItem.QuantityOrdered,
                    valueSpecified = transferOrdersItem.QuantityOrdered.HasValue,
                    units = new UnitsType
                    {
                        uom = new UomType
                        {
                            code = uomCode,
                            codeSpecified = uomCodeSpecified
                        }
                    }
                };

                result[i] = detailType;
            }
            return result;
        }

        private string GetDateTimeOffset(DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return new DateTimeOffset((DateTime) dateTime).ToString("yyyy-MM-ddThh:mm:sszzz");
            }
            else
            {
                return null;
            }
        }

        public string SerializeToXml(TransferOrdersCanonical transferOrdersCanonical)
        {
            return xmlSerializer.Serialize(transferOrdersCanonical, new Utf8StringWriter());
        }
    }
}
