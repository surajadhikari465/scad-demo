using System;
using System.Collections.Generic;
using InventoryProducer.Common;
using InventoryProducer.Common.InstockDequeue.Model;
using InventoryProducer.Common.InstockDequeue.Model.DBModel;
using InventoryProducer.Producer.Model.DBModel;

namespace InventoryProducer.Tests
{
    public class InventoryReceiveTestResources
    {
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

        public static IList<ReceiveModel> GetReceiveList(int count = 0)
        {
            IList<ReceiveModel> list = new List<ReceiveModel>();
            for (int i = 0; i < count; i++)
            {
                list.Add(GetReceiveDbObject(i));
            }
            return list;
        }

        public static InstockDequeueResult GetInstockDequeueResult(string eventTypeCode)
        {
            InstockDequeueModel instockDequeueModel = GetInstockDequeueModel(eventTypeCode);
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                [Constants.MessageProperty.MessageNumber] = "1",
                [Constants.MessageProperty.NonReceivingSystems] = ""
            };

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
    }
}
