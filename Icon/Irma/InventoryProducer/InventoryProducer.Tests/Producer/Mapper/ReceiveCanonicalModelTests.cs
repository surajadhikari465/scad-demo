using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using InventoryProducer.Common.InstockDequeue.Model;
using InventoryProducer.Producer.Mapper;
using InventoryProducer.Producer.Model.DBModel;
using InventoryProducer.Common.Serializers;
using InventoryProducer.Tests;

using OrderReceipts = Icon.Esb.Schemas.Wfm.Contracts.orderReceipts;
using OrderReceiptsCanonical = Icon.Esb.Schemas.Wfm.Contracts.orderReceiptsOrderReceipt;
using System;

namespace InventoryProducer.Mapper.Tests
{
    [TestClass]
    public class ReceiveCanonicalModelTests
    {
        private ReceiveXmlCanonicalMapper mapper;
        private Mock<ISerializer<OrderReceipts>> serializer;

        [TestInitialize]
        public void Initialize()
        {
            serializer = new Mock<ISerializer<OrderReceipts>>();
            mapper = new ReceiveXmlCanonicalMapper(serializer.Object);
        }

        [TestMethod]
        public void TransformToXMLCanonical_Test()
        {
            // Given
            IList<ReceiveModel> receiveList = TestResources.GetReceiveList(5);
            InstockDequeueResult instockDequeueResult = TestResources.GetInstockDequeueResult("RCPT_CRE");

            // When
            OrderReceipts receiveCanonical = mapper.TransformToXmlCanonical(receiveList, instockDequeueResult);

            Assert.IsNotNull(receiveCanonical);
            Assert.AreEqual(receiveCanonical.orderReceipt.Length, 1);
            Assert.AreEqual(receiveCanonical.orderReceipt[0].GetType(), typeof(OrderReceiptsCanonical));

            var orderReceiptsCanonical = receiveCanonical.orderReceipt[0];
            ReceiveModel receiveItem = receiveList[0];

            Assert.AreEqual(orderReceiptsCanonical.receiptNumber, 21636715);
            Assert.AreEqual(orderReceiptsCanonical.purchaseOrderNumber, "21636715");
            Assert.AreEqual(orderReceiptsCanonical.locationNumber, 10379);
            Assert.AreEqual(orderReceiptsCanonical.locationName, "CA NOE VALLEY (NOE)");
            Assert.AreEqual(orderReceiptsCanonical.eventType, "RCPT_CRE");
            Assert.AreEqual(orderReceiptsCanonical.messageNumber, "1");
            Assert.AreEqual(orderReceiptsCanonical.receiptStatus, "Received");
            Assert.AreEqual(orderReceiptsCanonical.isPastReceipt, "false");
            Assert.AreEqual(orderReceiptsCanonical.pastRceiptDate, string.Empty);
            Assert.IsFalse(orderReceiptsCanonical.pastRceiptDateSpecified);
            Assert.AreEqual(orderReceiptsCanonical.purchaseOrderCreateDateTime, new DateTimeOffset(receiveItem.CreateDateTime).ToString("O"));
            Assert.AreEqual(orderReceiptsCanonical.purchaseOrderSupplierNumber, "0000210489");

            Assert.AreEqual(orderReceiptsCanonical.receiptDetail.Length, 5);
            int i = 0;
            foreach (var receiptDetailItem in orderReceiptsCanonical.receiptDetail)
            {

                Assert.AreEqual(receiptDetailItem.receiptDetailNumber, i);
                Assert.AreEqual(receiptDetailItem.purchaseOrderDetailNumber, i);
                Assert.AreEqual(receiptDetailItem.sourceItemKey, "346114");
                Assert.AreEqual(receiptDetailItem.SubTeam.subTeamNumber, "1700");
                Assert.AreEqual(receiptDetailItem.SubTeam.subTeamName, "Produce");
                Assert.AreEqual(receiptDetailItem.SubTeam.hostSubTeamNumber, "1700");
                Assert.AreEqual(receiptDetailItem.SubTeam.hostSubTeamName, "Produce");
                Assert.AreEqual(receiptDetailItem.itemVIN, "203569");
                Assert.AreEqual(receiptDetailItem.defaultScanCode, "82676681171");
                Assert.AreEqual(receiptDetailItem.quantityOrdered, 1);
                Assert.AreEqual(receiptDetailItem.quantityReceived, 1);
                Assert.AreEqual(receiptDetailItem.receiptStatus, "Received");
                Assert.AreEqual(receiptDetailItem.receiptUoM, "CS");
                Assert.AreEqual(receiptDetailItem.packSize1, 6);
                Assert.AreEqual(receiptDetailItem.packSize2, 1);
                Assert.IsTrue(receiptDetailItem.packSize2Specified);
                Assert.AreEqual(receiptDetailItem.receiptUserInfo.idNumber, string.Empty);
                Assert.AreEqual(receiptDetailItem.receiptUserInfo.name, "ol_import");
                Assert.AreEqual(receiptDetailItem.createDateTime, new DateTimeOffset((DateTime)receiveList[i].DateReceived).ToString("O"));
                Assert.IsFalse(receiptDetailItem.documentNumberSpecified);
                i++;
            }
        }

        [TestMethod]
        public void SerializeToXml_Test()
        {
            // Given
            IList<ReceiveModel> receiveList = TestResources.GetReceiveList(5);
            InstockDequeueResult instockDequeueResult = TestResources.GetInstockDequeueResult("RCPT_CRE");
            serializer.Setup(s => s.Serialize(It.IsAny<OrderReceipts>(), It.IsAny<System.IO.TextWriter>())).Returns("xml");

            // When
            OrderReceipts orderReceiptsCanonical = mapper.TransformToXmlCanonical(receiveList, instockDequeueResult);
            string xmlMessage = mapper.SerializeToXml(orderReceiptsCanonical);

            Assert.AreEqual(xmlMessage, "xml");
        }

        [TestMethod]
        public void GetEvents_Transformation_Validation() // Without pastRceiptDate
        {
            // Given
            ISerializer<OrderReceipts> serializer = new Serializer<OrderReceipts>();
            ReceiveXmlCanonicalMapper receiveXmlCanonicalMapper = new ReceiveXmlCanonicalMapper(serializer);
            IList<ReceiveModel> receiveList = TestResources.GetReceiveList(2);
            InstockDequeueResult instockDequeueResult = TestResources.GetInstockDequeueResult("RCPT_CRE");

            // When
            OrderReceipts orderReceiptsCanonical = receiveXmlCanonicalMapper.TransformToXmlCanonical(receiveList, instockDequeueResult);
            string xmlMessage = receiveXmlCanonicalMapper.SerializeToXml(orderReceiptsCanonical);

            // Then
            Assert.IsNotNull(xmlMessage);
            string expectedXmlMessage = "<?xml version=\"1.0\" encoding=\"utf-8\"?><ns0:orderReceipts xmlns:ns1=\"http://schemas.wfm.com/Enterprise/InventoryMgmt/CommonRefTypes/V1\" " +
            "xmlns:ns0=\"http://schemas.wfm.com/Enterprise/InventoryMgmt/OrderReceipt/V1\">" +
            "<ns0:orderReceipt>" +
            "<ns0:receiptNumber>21636715</ns0:receiptNumber>" +
            "<ns0:purchaseOrderNumber>21636715</ns0:purchaseOrderNumber>" +
            "<ns0:locationNumber>10379</ns0:locationNumber>" +
            "<ns0:locationName>CA NOE VALLEY (NOE)</ns0:locationName>" +
            "<ns0:eventType>RCPT_CRE</ns0:eventType>" +
            "<ns0:messageNumber>1</ns0:messageNumber>" +
            "<ns0:receiptStatus>Received</ns0:receiptStatus>" +
            "<ns0:isPastReceipt>false</ns0:isPastReceipt>" +
            "<ns0:purchaseOrderCreateDateTime>2022-05-25T10:46:00.0000000-05:00</ns0:purchaseOrderCreateDateTime>" +
            "<ns0:purchaseOrderSupplierNumber>0000210489</ns0:purchaseOrderSupplierNumber>" +
            "<ns0:receiptDetail>" +
            "<ns0:receiptDetailNumber>0</ns0:receiptDetailNumber>" +                    // i = 0
            "<ns0:purchaseOrderDetailNumber>0</ns0:purchaseOrderDetailNumber>" +
            "<ns0:sourceItemKey>346114</ns0:sourceItemKey>" +
            "<ns0:SubTeam>" +
            "<ns1:subTeamNumber>1700</ns1:subTeamNumber>" +
            "<ns1:subTeamName>Produce</ns1:subTeamName>" +
            "<ns1:hostSubTeamNumber>1700</ns1:hostSubTeamNumber>" +
            "<ns1:hostSubTeamName>Produce</ns1:hostSubTeamName>" +
            "</ns0:SubTeam>" +
            "<ns0:itemVIN>203569</ns0:itemVIN>" +
            "<ns0:defaultScanCode>82676681171</ns0:defaultScanCode>" +
            "<ns0:quantityOrdered>1</ns0:quantityOrdered>" +
            "<ns0:quantityReceived>1</ns0:quantityReceived>" +
            "<ns0:receiptStatus>Received</ns0:receiptStatus>" +
            "<ns0:receiptUoM>CS</ns0:receiptUoM>" +
            "<ns0:packSize1>6</ns0:packSize1>" +
            "<ns0:packSize2>1</ns0:packSize2>" +
            "<ns0:receiptUserInfo>" +
            "<ns1:idNumber />" +
            "<ns1:name>ol_import</ns1:name>" +
            "</ns0:receiptUserInfo>" +
            "<ns0:createDateTime>2022-10-28T06:39:36.0000000-05:00</ns0:createDateTime>" +
            "</ns0:receiptDetail>" +
            "<ns0:receiptDetail>" +
            "<ns0:receiptDetailNumber>1</ns0:receiptDetailNumber>" +                    // i = 1
            "<ns0:purchaseOrderDetailNumber>1</ns0:purchaseOrderDetailNumber>" +
            "<ns0:sourceItemKey>346114</ns0:sourceItemKey>" +
            "<ns0:SubTeam>" +
            "<ns1:subTeamNumber>1700</ns1:subTeamNumber>" +
            "<ns1:subTeamName>Produce</ns1:subTeamName>" +
            "<ns1:hostSubTeamNumber>1700</ns1:hostSubTeamNumber>" +
            "<ns1:hostSubTeamName>Produce</ns1:hostSubTeamName>" +
            "</ns0:SubTeam>" +
            "<ns0:itemVIN>203569</ns0:itemVIN>" +
            "<ns0:defaultScanCode>82676681171</ns0:defaultScanCode>" +
            "<ns0:quantityOrdered>1</ns0:quantityOrdered>" +
            "<ns0:quantityReceived>1</ns0:quantityReceived>" +
            "<ns0:receiptStatus>Received</ns0:receiptStatus>" +
            "<ns0:receiptUoM>CS</ns0:receiptUoM>" +
            "<ns0:packSize1>6</ns0:packSize1>" +
            "<ns0:packSize2>1</ns0:packSize2>" +
            "<ns0:receiptUserInfo>" +
            "<ns1:idNumber />" +
            "<ns1:name>ol_import</ns1:name>" +
            "</ns0:receiptUserInfo>" +
            "<ns0:createDateTime>2022-10-28T06:39:36.0000000-05:00</ns0:createDateTime>" +
            "</ns0:receiptDetail>" +
            "</ns0:orderReceipt>" +
            "</ns0:orderReceipts>";
            Assert.AreEqual(xmlMessage, expectedXmlMessage);
        }

        [TestMethod]
        public void GetEvents_Transformation_Validation2() // With pastRceiptDate
        {
            // Given
            ISerializer<OrderReceipts> serializer = new Serializer<OrderReceipts>();
            ReceiveXmlCanonicalMapper receiveXmlCanonicalMapper = new ReceiveXmlCanonicalMapper(serializer);
            IList<ReceiveModel> receiveList = TestResources.GetReceiveList(2);
            receiveList[0].PastReceiptDate = new DateTime(2021, 11, 09, 09, 30, 15);
            InstockDequeueResult instockDequeueResult = TestResources.GetInstockDequeueResult("RCPT_CRE");

            // When
            OrderReceipts orderReceiptsCanonical = receiveXmlCanonicalMapper.TransformToXmlCanonical(receiveList, instockDequeueResult);
            string xmlMessage = receiveXmlCanonicalMapper.SerializeToXml(orderReceiptsCanonical);

            // Then
            Assert.IsNotNull(xmlMessage);
            string expectedXmlMessage = "<?xml version=\"1.0\" encoding=\"utf-8\"?><ns0:orderReceipts xmlns:ns1=\"http://schemas.wfm.com/Enterprise/InventoryMgmt/CommonRefTypes/V1\" " +
            "xmlns:ns0=\"http://schemas.wfm.com/Enterprise/InventoryMgmt/OrderReceipt/V1\">" +
            "<ns0:orderReceipt>" +
            "<ns0:receiptNumber>21636715</ns0:receiptNumber>" +
            "<ns0:purchaseOrderNumber>21636715</ns0:purchaseOrderNumber>" +
            "<ns0:locationNumber>10379</ns0:locationNumber>" +
            "<ns0:locationName>CA NOE VALLEY (NOE)</ns0:locationName>" +
            "<ns0:eventType>RCPT_CRE</ns0:eventType>" +
            "<ns0:messageNumber>1</ns0:messageNumber>" +
            "<ns0:receiptStatus>Received</ns0:receiptStatus>" +
            "<ns0:isPastReceipt>true</ns0:isPastReceipt>" +
            "<ns0:pastRceiptDate>2021-11-09T09:30:15.0000000-05:00</ns0:pastRceiptDate>" +
            "<ns0:purchaseOrderCreateDateTime>2022-05-25T10:46:00.0000000-05:00</ns0:purchaseOrderCreateDateTime>" +
            "<ns0:purchaseOrderSupplierNumber>0000210489</ns0:purchaseOrderSupplierNumber>" +
            "<ns0:receiptDetail>" +
            "<ns0:receiptDetailNumber>0</ns0:receiptDetailNumber>" +                    // i = 0
            "<ns0:purchaseOrderDetailNumber>0</ns0:purchaseOrderDetailNumber>" +
            "<ns0:sourceItemKey>346114</ns0:sourceItemKey>" +
            "<ns0:SubTeam>" +
            "<ns1:subTeamNumber>1700</ns1:subTeamNumber>" +
            "<ns1:subTeamName>Produce</ns1:subTeamName>" +
            "<ns1:hostSubTeamNumber>1700</ns1:hostSubTeamNumber>" +
            "<ns1:hostSubTeamName>Produce</ns1:hostSubTeamName>" +
            "</ns0:SubTeam>" +
            "<ns0:itemVIN>203569</ns0:itemVIN>" +
            "<ns0:defaultScanCode>82676681171</ns0:defaultScanCode>" +
            "<ns0:quantityOrdered>1</ns0:quantityOrdered>" +
            "<ns0:quantityReceived>1</ns0:quantityReceived>" +
            "<ns0:receiptStatus>Received</ns0:receiptStatus>" +
            "<ns0:receiptUoM>CS</ns0:receiptUoM>" +
            "<ns0:packSize1>6</ns0:packSize1>" +
            "<ns0:packSize2>1</ns0:packSize2>" +
            "<ns0:receiptUserInfo>" +
            "<ns1:idNumber />" +
            "<ns1:name>ol_import</ns1:name>" +
            "</ns0:receiptUserInfo>" +
            "<ns0:createDateTime>2022-10-28T06:39:36.0000000-05:00</ns0:createDateTime>" +
            "</ns0:receiptDetail>" +
            "<ns0:receiptDetail>" +
            "<ns0:receiptDetailNumber>1</ns0:receiptDetailNumber>" +                    // i = 1
            "<ns0:purchaseOrderDetailNumber>1</ns0:purchaseOrderDetailNumber>" +
            "<ns0:sourceItemKey>346114</ns0:sourceItemKey>" +
            "<ns0:SubTeam>" +
            "<ns1:subTeamNumber>1700</ns1:subTeamNumber>" +
            "<ns1:subTeamName>Produce</ns1:subTeamName>" +
            "<ns1:hostSubTeamNumber>1700</ns1:hostSubTeamNumber>" +
            "<ns1:hostSubTeamName>Produce</ns1:hostSubTeamName>" +
            "</ns0:SubTeam>" +
            "<ns0:itemVIN>203569</ns0:itemVIN>" +
            "<ns0:defaultScanCode>82676681171</ns0:defaultScanCode>" +
            "<ns0:quantityOrdered>1</ns0:quantityOrdered>" +
            "<ns0:quantityReceived>1</ns0:quantityReceived>" +
            "<ns0:receiptStatus>Received</ns0:receiptStatus>" +
            "<ns0:receiptUoM>CS</ns0:receiptUoM>" +
            "<ns0:packSize1>6</ns0:packSize1>" +
            "<ns0:packSize2>1</ns0:packSize2>" +
            "<ns0:receiptUserInfo>" +
            "<ns1:idNumber />" +
            "<ns1:name>ol_import</ns1:name>" +
            "</ns0:receiptUserInfo>" +
            "<ns0:createDateTime>2022-10-28T06:39:36.0000000-05:00</ns0:createDateTime>" +
            "</ns0:receiptDetail>" +
            "</ns0:orderReceipt>" +
            "</ns0:orderReceipts>";
            Assert.AreEqual(xmlMessage, expectedXmlMessage);
        }
    }
}
