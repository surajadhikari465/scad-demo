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
    }
}
