using System;
using System.Collections.Generic;
using InventoryProducer.Common;
using InventoryProducer.Common.InstockDequeue.Model;
using InventoryProducer.Common.InstockDequeue.Model.DBModel;
using InventoryProducer.Producer.Model.DBModel;

namespace InventoryProducer.Tests
{
    public class TestResources
    {
        public static TransferOrdersModel GetTransferOrderDbObject(int transferOrderDetailNumber = 1)
        {
            return new TransferOrdersModel()
            {
                OrderHeaderId = 1,
                FromLocationNumber = 1,
                FromLocationName = "from",
                ToLocationNumber = 2,
                ToLocationName = "to",
                FromSubTeamName = "from_subteam",
                FromSubTeamNumber = 1,
                ToSubTeamName = "to_subteam",
                ToSubTeamNumber = 2,
                Status = "status",
                UserId = 1,
                UserName = "user",
                ApproverId = 1,
                ApproverName = "approver",
                CreateDateTime = new DateTime(2022, 10, 11, 1, 30, 30),
                ApproveDateTime = new DateTime(2022, 10, 11, 1, 30, 30),
                TransferOrderDetailNumber = transferOrderDetailNumber,
                SourceItemKey = 1,
                DefaultScanCode = "1",
                HostSubTeamNumber = 1,
                HostSubTeamName = "host_subteam",
                QuantityOrdered = 1,
                OrderedUnit = "",
                OrderedUnitCode = "EA",
                ExpectedArrivalDate = new DateTime(2022, 10, 11, 1, 30, 30),
                PackageDesc1 = 1,
                PackageDesc2 = 2
            };
        }

        public static IList<TransferOrdersModel> GetTransferOrdersList(int count = 0)
        {
            IList<TransferOrdersModel> list = new List<TransferOrdersModel>();
            for(int i = 0; i < count; i++)
            {
                list.Add(GetTransferOrderDbObject(i));
            }
            return list;
        }

        public static InstockDequeueResult GetInstockDequeueResult(string eventTypeCode)
        {
            InstockDequeueModel instockDequeueModel = GetInstockDequeueModel(eventTypeCode);
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers[Constants.MessageProperty.MessageNumber] = "1";
            headers[Constants.MessageProperty.NonReceivingSystems] = "";

            InstockDequeueResult instockDequeueResult = new InstockDequeueResult(instockDequeueModel, headers);
            return instockDequeueResult;
        }

        public static InstockDequeueModel GetInstockDequeueModel(string eventTypeCode)
        {
            InstockDequeueModel instockDequeueModel = new InstockDequeueModel()
            {
                QueueID = 1,
                EventTypeCode = eventTypeCode,
                InsertDate = new DateTime(2022, 10, 11, 1, 30, 30),
                KeyID = 1,
                SecondaryKeyID = 1,
                MessageTimestampUtc = new DateTime(2022, 10, 11, 1, 30, 30),
                MessageType = "Text"
            };
            return instockDequeueModel;
        }
        public static ReceiveModel GetReceiveDbObject(int orderItemID = 1)
        {
            return new ReceiveModel()
            {
                OrderHeaderId = 21636715,
                OrderItemId = orderItemID,
                Identifier = "82676681171",
                ItemKey = 346114,
                HostSubTeam = "Produce",
                HostSubTeamNumber = 1700,
                SubTeam = "Produce",
                SubTeamNumber = 1700,
                DateReceived = new DateTime(2022, 10, 28, 6, 39, 36),
                CreditPO = 0,
                QuantityReceived = 1,
                QuantityOrdered = 1,
                PackageDesc1 = 6,
                PackageDesc2 = 1,
                RecvLogUserId = null,
                RecvUserName = "ol_import",
                OrderUom = "CS",
                ReceiptUom = "CS",
                ReceiptStatus = "Received",
                VIN = "203569",
                StoreNumber = 10379,
                StoreName = "CA NOE VALLEY (NOE)",
                PastReceiptDate = null,
                SupplierNumber = "0000210489",
                CreateDateTime = new DateTime(2022, 05, 25, 10, 46, 00)
            };
        }
        public static PurchaseOrdersModel GetPurchaseOrdersDbObject(int orderItemID = 1)
        {
            return new PurchaseOrdersModel()
            {
                OrderHeaderId = orderItemID,
                ExternalSource = "AMAZON",
                PurchaseType = "Purchase Order",
                SupplierNumber = "11161777",
                LocationNumber = 10268,
                LocationName = "ON SQUARE ONE (SQO)",
                OrderSubTeamNo = 4900,
                OrderSubTeamName = "Prepared Foods",
                OrderTeamNo = 70,
                OrderTeamName = "Prepared Foods",
                Status = "Sent",
                UserId = 16037,
                Username = "Sarah.Roberts",
                CreateDateTime = DateTime.Now,
                PurchaseOrderDetailNumber = 289246108,
                SourceItemKey = 736066,
                DefaultScanCode = "1432151031",
                HostSubTeamNumber = 1400,
                HostSubTeamName = "Bin Bulk",
                QuantityOrdered = 1,
                OrderedUnitCode = "CS",
                OrderedUnit = "CASE",
                PackSize1 = 1,
                PackSize2 = 1,
                RetailUnit = "EACH",
                ItemCost = 19.2244m,
                ExpectedArrivalDate = DateTime.Now
            };
        }

        public static IList<ReceiveModel> GetReceiveList(int count = 0)
        {
            IList<ReceiveModel> list = new List<ReceiveModel>();
            for (int i = 0; i < count; i++)
            {
                list.Add(GetReceiveDbObject(i));
            }
            return list;
        }
        public static IList<PurchaseOrdersModel> GetPurchaseOrderList(int count = 0)
        {
            IList<PurchaseOrdersModel> list = new List<PurchaseOrdersModel>();
            for (int i = 0; i < count; i++)
            {
                list.Add(GetPurchaseOrdersDbObject(i));
            }
            return list;
        }
    }
}
