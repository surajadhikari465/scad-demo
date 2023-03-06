using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using InventoryProducer.Common.InstockDequeue.Model;
using InventoryProducer.Producer.Mapper;
using InventoryProducer.Producer.Model.DBModel;
using InventoryProducer.Common.Serializers;
using InventoryProducer.Tests;

using TransferOrdersCanonical = Icon.Esb.Schemas.Wfm.Contracts.transferOrders;
using TransferOrdersDeleteCanonical = Icon.Esb.Schemas.Wfm.Contracts.transferOrdersTransferOrderDelete;
using TransferOrdersNonDeleteCanonical = Icon.Esb.Schemas.Wfm.Contracts.transferOrdersTransferOrder;

namespace InventoryProducer.Mapper.Tests
{
    [TestClass]
    public class TransferOrderCanonicalModelTests
    {
        private TransferOrderXmlCanonicalMapper mapper;
        private Mock<ISerializer<TransferOrdersCanonical>> serializer;

        [TestInitialize]
        public void Initialize()
        {
            serializer = new Mock<ISerializer<TransferOrdersCanonical>>();
            mapper = new TransferOrderXmlCanonicalMapper(serializer.Object);
        }

        [TestMethod]
        public void TransformToXMLCanonical_NonDeleteEvent_Test()
        {
            // Given
            IList<TransferOrdersModel> transferOrdersDbModels = TestResources.GetTransferOrdersList(5);
            InstockDequeueResult instockDequeueResult = TestResources.GetInstockDequeueResult("TSF_CRE");
            
            // When
            TransferOrdersCanonical canonicalModel = mapper.TransformToXmlCanonical(transferOrdersDbModels, instockDequeueResult);

            Assert.IsNotNull(canonicalModel);
            Assert.AreEqual(canonicalModel.Items.Length, 1);
            Assert.AreEqual(canonicalModel.Items[0].GetType(), typeof(TransferOrdersNonDeleteCanonical));

            var transferOrderNonDeleteCanonical = (TransferOrdersNonDeleteCanonical) canonicalModel.Items[0];
            Assert.AreEqual(transferOrderNonDeleteCanonical.eventType, "TSF_CRE");
            Assert.IsNotNull(transferOrderNonDeleteCanonical.approveDateTime);
            Assert.IsNotNull(transferOrderNonDeleteCanonical.createDateTime);
            Assert.AreEqual(transferOrderNonDeleteCanonical.messageNumber, "1");
            Assert.AreEqual(transferOrderNonDeleteCanonical.transferNumber, "1");
            Assert.AreEqual(transferOrderNonDeleteCanonical.locationChange.fromLocationNumber, "1");
            Assert.AreEqual(transferOrderNonDeleteCanonical.locationChange.fromLocationName, "from");
            Assert.AreEqual(transferOrderNonDeleteCanonical.locationChange.toLocationNumber, "2");
            Assert.AreEqual(transferOrderNonDeleteCanonical.locationChange.toLocationName, "to");
            Assert.AreEqual(transferOrderNonDeleteCanonical.subTeamChange.fromSubTeamName, "from_subteam");
            Assert.AreEqual(transferOrderNonDeleteCanonical.subTeamChange.fromSubTeamNumber, "1");
            Assert.AreEqual(transferOrderNonDeleteCanonical.subTeamChange.toSubTeamName, "to_subteam");
            Assert.AreEqual(transferOrderNonDeleteCanonical.subTeamChange.toSubTeamNumber, "2");
            Assert.AreEqual(transferOrderNonDeleteCanonical.transferOrderDetail.Length, 5);

            foreach(var transferOrderDetailItem in transferOrderNonDeleteCanonical.transferOrderDetail){
                Assert.IsNotNull(transferOrderDetailItem);
                Assert.AreEqual(transferOrderDetailItem.defaultScanCode, "1");
                Assert.AreEqual(transferOrderDetailItem.packSize1, 1);
                Assert.AreEqual(transferOrderDetailItem.packSize2, 2);
                Assert.IsTrue(transferOrderDetailItem.packSize2Specified);
                Assert.IsNotNull(transferOrderDetailItem.expectedArrivalDate);
                Assert.IsTrue(transferOrderDetailItem.expectedArrivalDateSpecified);
                Assert.AreEqual(transferOrderDetailItem.sourceItemKey, "1");

                Assert.AreEqual(transferOrderDetailItem.SubTeam.hostSubTeamName, "host_subteam");
                Assert.AreEqual(transferOrderDetailItem.SubTeam.hostSubTeamNumber, "1");
                Assert.AreEqual(transferOrderDetailItem.SubTeam.subTeamName, "to_subteam");
                Assert.AreEqual(transferOrderDetailItem.SubTeam.subTeamNumber, "2");

                Assert.AreEqual(transferOrderDetailItem.transferQuantities.Length, 1);
                var transferQuantity = transferOrderDetailItem.transferQuantities[0];

                Assert.AreEqual(transferQuantity.value, 1);
                Assert.IsTrue(transferQuantity.valueSpecified);
                Assert.AreEqual(transferQuantity.units.uom.code.ToString(), "EA");
                Assert.IsTrue(transferQuantity.units.uom.codeSpecified);
                Assert.IsFalse(transferQuantity.units.uom.nameSpecified);
                Assert.IsFalse(transferQuantity.units.valueSpecified);
            }
        }

        [TestMethod]
        public void TransformToXMLCanonical_LineDeleteEvent_Test()
        {
            // Given
            IList<TransferOrdersModel> transferOrdersDbModels = TestResources.GetTransferOrdersList(5);
            InstockDequeueResult instockDequeueResult = TestResources.GetInstockDequeueResult("TSF_LINE_DEL");

            // When
            TransferOrdersCanonical canonicalModel = mapper.TransformToXmlCanonical(transferOrdersDbModels, instockDequeueResult);

            Assert.IsNotNull(canonicalModel);
            Assert.AreEqual(canonicalModel.Items.Length, 1);
            Assert.AreEqual(canonicalModel.Items[0].GetType(), typeof(TransferOrdersDeleteCanonical));

            var transferOrdersDeleteCanonical = (TransferOrdersDeleteCanonical) canonicalModel.Items[0];
            Assert.IsNotNull(transferOrdersDeleteCanonical);
            Assert.IsFalse(transferOrdersDeleteCanonical.cancelDateTimeSpecified);
            Assert.AreEqual(transferOrdersDeleteCanonical.eventType, "TSF_LINE_DEL");
            Assert.AreEqual(transferOrdersDeleteCanonical.locationName, "to");
            Assert.AreEqual(transferOrdersDeleteCanonical.locationNumber, 2);
            Assert.AreEqual(transferOrdersDeleteCanonical.messageNumber, "1");
            Assert.AreEqual(transferOrdersDeleteCanonical.transferNumber, 1);
            Assert.AreEqual(transferOrdersDeleteCanonical.cancelUserInfo.idNumber, "1");
            Assert.AreEqual(transferOrdersDeleteCanonical.cancelUserInfo.name, "user");

            Assert.AreEqual(transferOrdersDeleteCanonical.transferOrderDeletionDetail.Length, 5);

            foreach(var item in transferOrdersDeleteCanonical.transferOrderDeletionDetail)
            {
                Assert.IsNotNull(item);
                Assert.IsFalse(item.batchNumberSpecified);
                Assert.AreEqual(item.defaultScanCode, "1");
                Assert.AreEqual(item.sourceItemKey, "1");
                Assert.IsTrue(item.transferOrderDetailNumberSpecified);
            }
        }

        [TestMethod]
        public void TransformToXMLCanonical_DeleteEvent_Test()
        {
            // Given
            IList<TransferOrdersModel> transferOrdersDbModels = TestResources.GetTransferOrdersList(5);
            InstockDequeueResult instockDequeueResult = TestResources.GetInstockDequeueResult("TSF_DEL");

            // When
            TransferOrdersCanonical canonicalModel = mapper.TransformToXmlCanonical(transferOrdersDbModels, instockDequeueResult);

            Assert.IsNotNull(canonicalModel);
            Assert.AreEqual(canonicalModel.Items.Length, 1);
            Assert.AreEqual(canonicalModel.Items[0].GetType(), typeof(TransferOrdersDeleteCanonical));

            var transferOrdersDeleteCanonical = (TransferOrdersDeleteCanonical)canonicalModel.Items[0];
            Assert.IsNotNull(transferOrdersDeleteCanonical);
            Assert.IsFalse(transferOrdersDeleteCanonical.cancelDateTimeSpecified);
            Assert.AreEqual(transferOrdersDeleteCanonical.eventType, "TSF_DEL");
            Assert.AreEqual(transferOrdersDeleteCanonical.locationName, "to");
            Assert.AreEqual(transferOrdersDeleteCanonical.locationNumber, 2);
            Assert.AreEqual(transferOrdersDeleteCanonical.messageNumber, "1");
            Assert.AreEqual(transferOrdersDeleteCanonical.transferNumber, 1);
            Assert.AreEqual(transferOrdersDeleteCanonical.cancelUserInfo.idNumber, "1");
            Assert.AreEqual(transferOrdersDeleteCanonical.cancelUserInfo.name, "user");

            Assert.AreEqual(transferOrdersDeleteCanonical.transferOrderDeletionDetail.Length, 5);

            foreach (var item in transferOrdersDeleteCanonical.transferOrderDeletionDetail)
            {
                Assert.IsNotNull(item);
                Assert.IsFalse(item.batchNumberSpecified);
                Assert.IsNull(item.defaultScanCode);
                Assert.IsNull(item.sourceItemKey);
                Assert.IsFalse(item.transferOrderDetailNumberSpecified);
            }
        }

        [TestMethod]
        public void SerializeToXml_Test()
        {
            // Given
            IList<TransferOrdersModel> transferOrdersDbModels = TestResources.GetTransferOrdersList(5);
            InstockDequeueResult instockDequeueResult = TestResources.GetInstockDequeueResult("TSF_CRE");
            serializer.Setup(s => s.Serialize(It.IsAny<TransferOrdersCanonical>(), It.IsAny<System.IO.TextWriter>())).Returns("xml");

            // When
            TransferOrdersCanonical canonicalModel = mapper.TransformToXmlCanonical(transferOrdersDbModels, instockDequeueResult);
            string xmlMessage = mapper.SerializeToXml(canonicalModel);

            Assert.AreEqual(xmlMessage, "xml");
        }

        [TestMethod]
        public void NonDeleteEvent_Transformation_Validation()
        {
            // Given
            ISerializer<TransferOrdersCanonical> serializer = new Serializer<TransferOrdersCanonical>();
            TransferOrderXmlCanonicalMapper canonicalMapper = new TransferOrderXmlCanonicalMapper(serializer);
            IList<TransferOrdersModel> transferOrdersDbModels = TestResources.GetTransferOrdersList(2);
            InstockDequeueResult instockDequeueResult = TestResources.GetInstockDequeueResult("TSF_CRE");

            // When
            TransferOrdersCanonical canonicalModel = canonicalMapper.TransformToXmlCanonical(transferOrdersDbModels, instockDequeueResult);
            string xmlMessage = canonicalMapper.SerializeToXml(canonicalModel);

            // Then
            Assert.IsNotNull(xmlMessage);
            string expectedXmlMessage = "<?xml version=\"1.0\" encoding=\"utf-8\"?><transferOrders xmlns:ns0=\"http://schemas.wfm.com/Enterprise/InventoryMgmt/CommonRefTypes/V1\" " +
            "xmlns:ns1=\"http://schemas.wfm.com/Enterprise/UnitOfMeasureMgmt/UnitOfMeasure/V2\" xmlns:ns2=\"http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1\" " +
            "xmlns=\"http://schemas.wfm.com/Enterprise/InventoryMgmt/TransferOrder/V1\"><transferOrder><transferNumber>1</transferNumber><eventType>TSF_CRE</eventType>" +
            "<messageNumber>1</messageNumber><locationChange><fromLocationNumber>1</fromLocationNumber><fromLocationName>from</fromLocationName><toLocationNumber>2</toLocationNumber>" + 
            "<toLocationName>to</toLocationName></locationChange><subTeamChange><fromSubTeamNumber>1</fromSubTeamNumber><fromSubTeamName>from_subteam</fromSubTeamName>" +
            "<toSubTeamNumber>2</toSubTeamNumber><toSubTeamName>to_subteam</toSubTeamName></subTeamChange><userInfo><ns0:idNumber>1</ns0:idNumber><ns0:name>user</ns0:name></userInfo>" +
            "<createDateTime>2022-10-11T01:30:30-05:00</createDateTime><approveDateTime>2022-10-11T01:30:30-05:00</approveDateTime><transferOrderDetail>" + 
            "<transferOrderDetailNumber>0</transferOrderDetailNumber><sourceItemKey>1</sourceItemKey><SubTeam><ns0:subTeamNumber>2</ns0:subTeamNumber><ns0:subTeamName>to_subteam</ns0:subTeamName>" +
            "<ns0:hostSubTeamNumber>1</ns0:hostSubTeamNumber><ns0:hostSubTeamName>host_subteam</ns0:hostSubTeamName></SubTeam><defaultScanCode>1</defaultScanCode><transferQuantities><ns2:quantity>" + 
            "<ns2:value>1</ns2:value><ns2:units><ns2:uom><ns1:code>EA</ns1:code></ns2:uom></ns2:units></ns2:quantity></transferQuantities><tsfStatus>status</tsfStatus>" + 
            "<expectedArrivalDate>2022-10-11T01:30:30-05:00</expectedArrivalDate><packSize1>1</packSize1><packSize2>2</packSize2></transferOrderDetail><transferOrderDetail>" + 
            "<transferOrderDetailNumber>1</transferOrderDetailNumber><sourceItemKey>1</sourceItemKey><SubTeam><ns0:subTeamNumber>2</ns0:subTeamNumber><ns0:subTeamName>to_subteam</ns0:subTeamName>" +
            "<ns0:hostSubTeamNumber>1</ns0:hostSubTeamNumber><ns0:hostSubTeamName>host_subteam</ns0:hostSubTeamName></SubTeam><defaultScanCode>1</defaultScanCode><transferQuantities><ns2:quantity>" +
            "<ns2:value>1</ns2:value><ns2:units><ns2:uom><ns1:code>EA</ns1:code></ns2:uom></ns2:units></ns2:quantity></transferQuantities><tsfStatus>status</tsfStatus>" +
            "<expectedArrivalDate>2022-10-11T01:30:30-05:00</expectedArrivalDate><packSize1>1</packSize1><packSize2>2</packSize2></transferOrderDetail></transferOrder></transferOrders>";
            Assert.AreEqual(xmlMessage, expectedXmlMessage);
        }

        [TestMethod]
        public void LineDeleteEvent_Transformation_Validation()
        {
            // Given
            ISerializer<TransferOrdersCanonical> serializer = new Serializer<TransferOrdersCanonical>();
            TransferOrderXmlCanonicalMapper canonicalMapper = new TransferOrderXmlCanonicalMapper(serializer);
            IList<TransferOrdersModel> transferOrdersDbModels = TestResources.GetTransferOrdersList(2);
            InstockDequeueResult instockDequeueResult = TestResources.GetInstockDequeueResult("TSF_LINE_DEL");

            // When
            TransferOrdersCanonical canonicalModel = canonicalMapper.TransformToXmlCanonical(transferOrdersDbModels, instockDequeueResult);
            string xmlMessage = canonicalMapper.SerializeToXml(canonicalModel);

            // Then
            Assert.IsNotNull(xmlMessage);
            string expectedXmlMessage = "<?xml version=\"1.0\" encoding=\"utf-8\"?><transferOrders xmlns:ns0=\"http://schemas.wfm.com/Enterprise/InventoryMgmt/CommonRefTypes/V1\" " +
            "xmlns:ns1=\"http://schemas.wfm.com/Enterprise/UnitOfMeasureMgmt/UnitOfMeasure/V2\" xmlns:ns2=\"http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1\" " +
            "xmlns=\"http://schemas.wfm.com/Enterprise/InventoryMgmt/TransferOrder/V1\"><transferOrderDelete><transferNumber>1</transferNumber><eventType>TSF_LINE_DEL</eventType>" + 
            "<messageNumber>1</messageNumber><locationNumber>2</locationNumber><locationName>to</locationName><cancelUserInfo><ns0:idNumber>1</ns0:idNumber><ns0:name>user</ns0:name>" +
            "</cancelUserInfo><transferOrderDeletionDetail><transferOrderDetailNumber>0</transferOrderDetailNumber><sourceItemKey>1</sourceItemKey><defaultScanCode>1</defaultScanCode>" + 
            "</transferOrderDeletionDetail><transferOrderDeletionDetail><transferOrderDetailNumber>1</transferOrderDetailNumber><sourceItemKey>1</sourceItemKey>" + 
            "<defaultScanCode>1</defaultScanCode></transferOrderDeletionDetail></transferOrderDelete></transferOrders>";
            Assert.AreEqual(xmlMessage, expectedXmlMessage);
        }

        [TestMethod]
        public void PODeleteEvent_Transformation_Validation()
        {
            // Given
            ISerializer<TransferOrdersCanonical> serializer = new Serializer<TransferOrdersCanonical>();
            TransferOrderXmlCanonicalMapper canonicalMapper = new TransferOrderXmlCanonicalMapper(serializer);
            IList<TransferOrdersModel> transferOrdersDbModels = TestResources.GetTransferOrdersList(2);
            InstockDequeueResult instockDequeueResult = TestResources.GetInstockDequeueResult("TSF_DEL");

            // When
            TransferOrdersCanonical canonicalModel = canonicalMapper.TransformToXmlCanonical(transferOrdersDbModels, instockDequeueResult);
            string xmlMessage = canonicalMapper.SerializeToXml(canonicalModel);

            // Then
            Assert.IsNotNull(xmlMessage);
            string expectedXmlMessage = "<?xml version=\"1.0\" encoding=\"utf-8\"?><transferOrders xmlns:ns0=\"http://schemas.wfm.com/Enterprise/InventoryMgmt/CommonRefTypes/V1\" " +
            "xmlns:ns1=\"http://schemas.wfm.com/Enterprise/UnitOfMeasureMgmt/UnitOfMeasure/V2\" xmlns:ns2=\"http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1\" " +
            "xmlns=\"http://schemas.wfm.com/Enterprise/InventoryMgmt/TransferOrder/V1\"><transferOrderDelete><transferNumber>1</transferNumber><eventType>TSF_DEL</eventType>" +
            "<messageNumber>1</messageNumber><locationNumber>2</locationNumber><locationName>to</locationName><cancelUserInfo><ns0:idNumber>1</ns0:idNumber><ns0:name>user</ns0:name></cancelUserInfo>" +
            "<transferOrderDeletionDetail /><transferOrderDeletionDetail /></transferOrderDelete></transferOrders>";
            Assert.AreEqual(xmlMessage, expectedXmlMessage);
        }
    }
}
