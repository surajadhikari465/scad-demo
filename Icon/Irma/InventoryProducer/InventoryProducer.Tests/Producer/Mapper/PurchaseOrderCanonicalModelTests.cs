using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using InventoryProducer.Common.InstockDequeue.Model;
using InventoryProducer.Producer.Mapper;
using InventoryProducer.Producer.Model.DBModel;
using InventoryProducer.Common.Serializers;
using InventoryProducer.Tests;

using PurchaserOrderCanonical = Icon.Esb.Schemas.Wfm.Contracts.PurchaseOrders;
using PurchaseOrderSingleCanonical = Icon.Esb.Schemas.Wfm.Contracts.PurchaseOrdersPurchaseOrder;
using System;
using Icon.Esb.Schemas.Wfm.ContractTypes;

namespace InventoryProducer.Mapper.Tests
{
    [TestClass]
    public class PurchaseOrderCanonicalModelTests
    {
        private PurchaseOrderXmlCanonicalMapper mapper;
        private Mock<ISerializer<PurchaserOrderCanonical>> serializer;

        [TestInitialize]
        public void Initialize()
        {
            serializer = new Mock<ISerializer<PurchaserOrderCanonical>>();
            mapper = new PurchaseOrderXmlCanonicalMapper(serializer.Object);
        }

        [TestMethod]
        public void TransformToXMLCanonical_Test()
        {
            // Given
            IList<PurchaseOrdersModel> purchaseOrderList = TestResources.GetPurchaseOrderList(5);
            InstockDequeueResult instockDequeueResult = TestResources.GetInstockDequeueResult("PO_CRE");

            // When
            PurchaserOrderCanonical purchaseCanonical = mapper.TransformToXmlCanonical(purchaseOrderList, instockDequeueResult);

            Assert.IsNotNull(purchaseCanonical);
            Assert.AreEqual(1, purchaseCanonical.Items.Length);
            Assert.AreEqual(typeof(PurchaseOrderSingleCanonical), purchaseCanonical.Items[0].GetType());

            var purchaseOrderCanonical = (PurchaseOrderSingleCanonical) purchaseCanonical.Items[0];
            PurchaseOrdersModel purchaseOrderItem = purchaseOrderList[0];

            Assert.AreEqual(purchaseOrderItem.Status, purchaseOrderCanonical.status);
            Assert.AreEqual(purchaseOrderItem.LocationNumber, purchaseOrderCanonical.locationNumber);
            Assert.AreEqual(purchaseOrderItem.LocationName, purchaseOrderCanonical.locationName);
        }

        [TestMethod]
        public void SerializeToXml_Test()
        {
            // Given
            IList<PurchaseOrdersModel> purchaseOrderList = TestResources.GetPurchaseOrderList(5);
            InstockDequeueResult instockDequeueResult = TestResources.GetInstockDequeueResult("PO_CRE");
            serializer.Setup(s => s.Serialize(It.IsAny<PurchaserOrderCanonical>(), It.IsAny<System.IO.TextWriter>())).Returns("xml");

            // When
            PurchaserOrderCanonical poCanonical = mapper.TransformToXmlCanonical(purchaseOrderList, instockDequeueResult);
            string xmlMessage = mapper.SerializeToXml(poCanonical);

            Assert.AreEqual(xmlMessage, "xml");
        }

        [TestMethod]
        public void GetEvents_Transformation_Validation()
        {
            // Given
            ISerializer<PurchaserOrderCanonical> serializer = new Serializer<PurchaserOrderCanonical>();
            PurchaseOrderXmlCanonicalMapper poXmlCanonicalMapper = new PurchaseOrderXmlCanonicalMapper(serializer);
            IList<PurchaseOrdersModel> receiveList = TestResources.GetPurchaseOrderList(2);
            InstockDequeueResult instockDequeueResult = TestResources.GetInstockDequeueResult("PO_CRE");

            // When
            PurchaserOrderCanonical poReceiptsCanonical = poXmlCanonicalMapper.TransformToXmlCanonical(receiveList, instockDequeueResult);
            string xmlMessage = poXmlCanonicalMapper.SerializeToXml(poReceiptsCanonical);
            
            // Then
            Assert.IsNotNull(xmlMessage);
            string expectedXmlMessage = @"<?xml version=""1.0"" encoding=""utf-8""?><PurchaseOrders xmlns:ns0=""http://schemas.wfm.com/Enterprise/InventoryMgmt/CommonRefTypes/V1"" xmlns:ns1=""http://schemas.wfm.com/Enterprise/UnitOfMeasureMgmt/UnitOfMeasure/V2"" xmlns:ns2=""http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1"" xmlns=""http://schemas.wfm.com/Enterprise/InventoryMgmt/PurchaseOrder/V1""><PurchaseOrder><purchaseOrderNumber>0</purchaseOrderNumber><invoiceNumber>123456</invoiceNumber><eventType>PO_CRE</eventType><messageNumber>1</messageNumber><purchaseType>Purchase Order</purchaseType><supplierNumber>11161777</supplierNumber><supplierName>TestSupplierName</supplierName><locationNumber>10268</locationNumber><locationName>ON SQUARE ONE (SQO)</locationName><OrderSubTeam><orderSubTeamNumber>4900</orderSubTeamNumber><orderSubTeamName>Prepared Foods</orderSubTeamName><orderTeamNumber>70</orderTeamNumber><orderTeamName>Prepared Foods</orderTeamName></OrderSubTeam><status>Sent</status><userInfo><ns0:idNumber>16037</ns0:idNumber><ns0:name>Sarah.Roberts</ns0:name></userInfo><createDateTime>2022-05-25T10:46:00-05:00</createDateTime><purchaseOrderNotes>test</purchaseOrderNotes><PurchaseOrderDetail><PurchaseOrderDetailNumber>289246108</PurchaseOrderDetailNumber><sourceItemKey>736066</sourceItemKey><itemName>bread</itemName><itemBrand>breadbrand</itemBrand><vendorItemNumber>39283</vendorItemNumber><locationId>10268</locationId><SubTeam><ns0:subTeamNumber>4900</ns0:subTeamNumber><ns0:subTeamName>Prepared Foods</ns0:subTeamName><ns0:hostSubTeamNumber>1400</ns0:hostSubTeamNumber><ns0:hostSubTeamName>Bin Bulk</ns0:hostSubTeamName></SubTeam><defaultScanCode>1432151031</defaultScanCode><quantities><ns2:quantity><ns2:value>4</ns2:value><ns2:units><ns2:uom><ns1:code>CS</ns1:code></ns2:uom></ns2:units></ns2:quantity></quantities><eInvoiceASNQuantities><ns2:quantity><ns2:value>4</ns2:value><ns2:units><ns2:uom><ns1:code>CS</ns1:code></ns2:uom></ns2:units></ns2:quantity></eInvoiceASNQuantities><eInvoiceASNWeights><ns2:quantity><ns2:value>120</ns2:value><ns2:units><ns2:uom><ns1:code>LB</ns1:code></ns2:uom></ns2:units></ns2:quantity></eInvoiceASNWeights><packSize1>30</packSize1><packSize2>1</packSize2><uomConvRetailUom>LB</uomConvRetailUom><costedByWeight>true</costedByWeight><catchweightRequired>false</catchweightRequired><itemCost>19.2244</itemCost><expectedArrivalDate>2022-05-25T10:46:00-05:00</expectedArrivalDate></PurchaseOrderDetail><PurchaseOrderDetail><PurchaseOrderDetailNumber>289246108</PurchaseOrderDetailNumber><sourceItemKey>736066</sourceItemKey><itemName>bread</itemName><itemBrand>breadbrand</itemBrand><vendorItemNumber>39283</vendorItemNumber><locationId>10268</locationId><SubTeam><ns0:subTeamNumber>4900</ns0:subTeamNumber><ns0:subTeamName>Prepared Foods</ns0:subTeamName><ns0:hostSubTeamNumber>1400</ns0:hostSubTeamNumber><ns0:hostSubTeamName>Bin Bulk</ns0:hostSubTeamName></SubTeam><defaultScanCode>1432151031</defaultScanCode><quantities><ns2:quantity><ns2:value>4</ns2:value><ns2:units><ns2:uom><ns1:code>CS</ns1:code></ns2:uom></ns2:units></ns2:quantity></quantities><eInvoiceASNQuantities><ns2:quantity><ns2:value>4</ns2:value><ns2:units><ns2:uom><ns1:code>CS</ns1:code></ns2:uom></ns2:units></ns2:quantity></eInvoiceASNQuantities><eInvoiceASNWeights><ns2:quantity><ns2:value>120</ns2:value><ns2:units><ns2:uom><ns1:code>LB</ns1:code></ns2:uom></ns2:units></ns2:quantity></eInvoiceASNWeights><packSize1>30</packSize1><packSize2>1</packSize2><uomConvRetailUom>LB</uomConvRetailUom><costedByWeight>true</costedByWeight><catchweightRequired>false</catchweightRequired><itemCost>19.2244</itemCost><expectedArrivalDate>2022-05-25T10:46:00-05:00</expectedArrivalDate></PurchaseOrderDetail></PurchaseOrder></PurchaseOrders>";
            Assert.AreEqual(expectedXmlMessage, xmlMessage, true);
        }
    }
}
