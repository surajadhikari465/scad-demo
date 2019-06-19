using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Icon.Esb.Subscriber;
using Icon.Framework;
using Icon.Logging;
using KitBuilder.ESB.Listeners.Item.Service;
using KitBuilder.ESB.Listeners.Item.Service.MessageParsers;
using KitBuilder.ESB.Listeners.Item.Service.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace KitBuilder.ESB.Listeners.Item.Tests.MessageParsers
{
    [TestClass]
    public class ItemMessageParserTests
    {
        private ItemMessageParser itemMessageParser;
        private ItemListenerSettings settings;
        private Mock<ILogger<ItemMessageParser>> mockLogger;

        private Mock<IEsbMessage> mockEsbMessage;
        private string message;

        [TestInitialize]
        public void Initialize()
        {
            settings = new ItemListenerSettings();
            mockLogger = new Mock<ILogger<ItemMessageParser>>();

            itemMessageParser = new ItemMessageParser(settings, mockLogger.Object);
            mockEsbMessage = new Mock<IEsbMessage>();
            mockEsbMessage.Setup(m => m.GetProperty("IconMessageID")).Returns("Icon_" + Guid.NewGuid().ToString());
        }

        [TestMethod]
        public void ParseMessage_ProductMessageWith3Products_ShouldReturn3ItemModels()
        {
            //Given
            message = File.ReadAllText(@"TestMessages\ProductMessageWith3Items.xml");

            mockEsbMessage.SetupGet(m => m.MessageText)
                .Returns(message);

            //When
            var items = itemMessageParser.ParseMessage(mockEsbMessage.Object).ToList();

            //Then
            Assert.AreEqual(3, items.Count());
            AssertItemsAreEqualToXml(items);
        }

        [TestMethod]
        public void ParseMessage_ProductMessageWithNoHospitalityData_ShouldReturnNoHospitalityData()
        {
            //Given
            message = File.ReadAllText(@"TestMessages\ProductMessageWithNoHospitality.xml");

            mockEsbMessage.SetupGet(m => m.MessageText)
                .Returns(message);

            //When
            var items = itemMessageParser.ParseMessage(mockEsbMessage.Object).ToList();

            //Then
            Assert.AreEqual(1, items.Count());
            Assert.IsNull(items[0].KitchenDescription);
            Assert.IsNull(items[0].KitchenItem);
            Assert.IsNull(items[0].HospitalityItem);
            Assert.IsNull(items[0].ImageUrl);
        }

        [TestMethod]
        public void ParseMessage_ExceptionIsThrown_ShouldThrowExceptionWithParseMessageExceptionErrorMessage()
        {
            //Given
            IEnumerable<ItemModel> items = null;
            mockEsbMessage.SetupGet(m => m.MessageText)
                .Returns("Bad Message");
            Exception expectedException = null;

            //When
            try
            {
                items = itemMessageParser.ParseMessage(mockEsbMessage.Object);
            }
            catch (Exception e)
            {
                expectedException = e;
            }

            //Then
            Assert.IsTrue(expectedException != null && expectedException.Message == "Failed to parse Infor Item message.");
        }

        [TestMethod]
        public void ParseMessage_ValidateSequenceIdIsTurnedOn_ShouldParseSequenceId()
        {
            //Given
            settings.ValidateSequenceId = true;
            message = File.ReadAllText(@"TestMessages\ProductMessageWith3Items.xml");

            mockEsbMessage.SetupGet(m => m.MessageText)
                .Returns(message);
            mockEsbMessage.Setup(m => m.GetProperty("SequenceID"))
                .Returns("1234");

            //When
            var items = itemMessageParser.ParseMessage(mockEsbMessage.Object).ToList();

            //Then
            Assert.AreEqual(3, items.Count);
            AssertItemsAreEqualToXml(items, 1234);
        }


		[TestMethod]
		private void AssertItemsAreEqualToXml(IEnumerable<ItemModel> items, decimal? sequenceId = null)
        {
            //Copied these values directly from ProductMessageWith3Items.xml. Updating that test message will require updating
            //this code.
            var messageItems = new List<ItemModel>
            {
                new ItemModel
                {
                    ItemId = 198757,
                    ScanCode = "9948243625",
                    MerchandiseHierarchyClassId = "84305",
                    CustomerFriendlyDescription = "Test Customer Friendly Description",
                    BrandsHierarchyName = "365",
					FlexibleText = "Test Item FlexibleText 1"
				},
                new ItemModel
                {
                    ItemId = 198759,
                    ScanCode = "29456600000",
                    MerchandiseHierarchyClassId = "83876",
                    CustomerFriendlyDescription = "Test Customer Friendly Description",
                    BrandsHierarchyName = "365",
					FlexibleText = "Test Item FlexibleText 2"
				},
                new ItemModel
                {
                    ItemId = 198760,
                    ScanCode = "223344",
                    MerchandiseHierarchyClassId = "83871",
                    CustomerFriendlyDescription = "",
                    BrandsHierarchyName = "Test",
					FlexibleText = "Test Item FlexibleText 3"
				}
            };
            var listItems = items.OrderBy(i => i.ItemId).ToList();

            for (int i = 0; i < listItems.Count(); i++)
            {
                Assert.AreEqual(messageItems[i].ItemId, listItems[i].ItemId);
               Assert.AreEqual(messageItems[i].ScanCode, listItems[i].ScanCode);
                Assert.AreEqual(messageItems[i].MerchandiseHierarchyClassId, listItems[i].MerchandiseHierarchyClassId);
                Assert.AreEqual(messageItems[i].CustomerFriendlyDescription, listItems[i].CustomerFriendlyDescription);
                Assert.AreEqual(messageItems[i].BrandsHierarchyName, listItems[i].BrandsHierarchyName);
            }
        }

    }
}
