using Icon.ApiController.Common;
using Icon.ApiController.Controller.Mappers;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;

using Icon.ApiController.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Framework;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.ApiController.Tests.SerializerTests
{
    [TestClass]
    public class SerializerTests
    {
        [TestMethod]
        public void Serializer_SerializeProductXmlToFile_XmlShouldBeSavedToDisk()
        {
            // Given.
            var mockLoggerSerializer = new Mock<ILogger<Serializer<Contracts.items>>>();
            var mockEmailClient = new Mock<IEmailClient>();
            var mockLoggerQueueReader = new Mock<ILogger<ProductQueueReader>>();
            var mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueProduct>, List<MessageQueueProduct>>>();
            var mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueProduct>>>();
            var mockProductSelectionGroupsMapper = new Mock<IProductSelectionGroupsMapper>();
            var mockUomMapper = new Mock<IUomMapper>();
            
            var productQueueReader = new ProductQueueReader(
                mockLoggerQueueReader.Object,
                mockEmailClient.Object,
                mockGetMessageQueueQuery.Object,
                mockUpdateMessageQueueStatusCommandHandler.Object,
                mockProductSelectionGroupsMapper.Object,
                mockUomMapper.Object);

            var message = new List<MessageQueueProduct> { TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 123, "0", ItemTypeCodes.RetailSale) };

            string path = @"product.xml";
            var serializer = new Serializer<Contracts.items>(mockLoggerSerializer.Object, mockEmailClient.Object);
            var miniBulk = productQueueReader.BuildMiniBulk(message);

            // When.
            serializer.Serialize(miniBulk, new StreamWriter(path));

            // Then.
            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod]
        public void Serializer_SerializeDepartmentSaleXmlToFile_XmlShouldBeSavedToDisk()
        {
            // Given.
            var mockLoggerSerializer = new Mock<ILogger<Serializer<Contracts.items>>>();
            var mockEmailClient = new Mock<IEmailClient>();
            var mockLoggerQueueReader = new Mock<ILogger<ProductQueueReader>>();
            var mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueProduct>, List<MessageQueueProduct>>>();
            var mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueProduct>>>();
            var mockProductSelectionGroupsMapper = new Mock<IProductSelectionGroupsMapper>();
            var mockUomMapper = new Mock<IUomMapper>();

            var productQueueReader = new ProductQueueReader(
                mockLoggerQueueReader.Object,
                mockEmailClient.Object,
                mockGetMessageQueueQuery.Object,
                mockUpdateMessageQueueStatusCommandHandler.Object,
                mockProductSelectionGroupsMapper.Object,
                mockUomMapper.Object);

            var message = new List<MessageQueueProduct> { TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 123, "1", ItemTypeCodes.RetailSale) };

            string path = @"department sale.xml";
            var serializer = new Serializer<Contracts.items>(mockLoggerSerializer.Object, mockEmailClient.Object);
            var miniBulk = productQueueReader.BuildMiniBulk(message);

            // When.
            serializer.Serialize(miniBulk, new StreamWriter(path));

            // Then.
            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod]
        public void Serializer_SerializeItemLocaleXmlToFile_XmlShouldBeSavedToDisk()
        {
            // Given.
            var mockLoggerSerializer = new Mock<ILogger<Serializer<Contracts.items>>>();
            var mockLoggerQueueReader = new Mock<ILogger<ItemLocaleQueueReader>>();
            var mockEmailClient = new Mock<IEmailClient>();
            var mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueItemLocale>, List<MessageQueueItemLocale>>>();
            var getItemsByScanCodeQuery = new Mock<IQueryHandler<GetItemByScanCodeParameters, Item>>();
            var mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueItemLocale>>>();
            var mockProductSelectionGroupsMapper = new Mock<IProductSelectionGroupsMapper>();
            
            var itemLocaleQueueReader = new ItemLocaleQueueReader(
                mockLoggerQueueReader.Object,
                mockEmailClient.Object,
                mockGetMessageQueueQuery.Object,
                getItemsByScanCodeQuery.Object,
                mockUpdateMessageQueueStatusCommandHandler.Object,
                mockProductSelectionGroupsMapper.Object);
                

            var message = new List<MessageQueueItemLocale> { TestHelpers.GetFakeMessageQueueItemLocale(123, 234, ItemTypeCodes.RetailSale) };

            string path = @"itemlocale.xml";
            var serializer = new Serializer<Contracts.items>(mockLoggerSerializer.Object, mockEmailClient.Object);
            var miniBulk = itemLocaleQueueReader.BuildMiniBulk(message);

            // When.
            serializer.Serialize(miniBulk, new StreamWriter(path));

            // Then.
            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod]
        public void Serializer_SerializePriceXmlToFile_XmlShouldBeSavedToDisk()
        {
            // Given.
            var mockLoggerSerializer = new Mock<ILogger<Serializer<Contracts.items>>>();
            var mockLoggerQueueReader = new Mock<ILogger<PriceQueueReader>>();
            var mockEmailClient = new Mock<IEmailClient>();
            var mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueuePrice>, List<MessageQueuePrice>>>();
            var mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueuePrice>>>();

            var priceQueueReader = new PriceQueueReader(
                mockLoggerQueueReader.Object,
                mockEmailClient.Object,
                mockGetMessageQueueQuery.Object,
                mockUpdateMessageQueueStatusCommandHandler.Object);

            var message = new List<MessageQueuePrice> { TestHelpers.GetFakeMessageQueuePrice(123, 234, 1.99m, null, null) };

            string path = @"price.xml";
            var serializer = new Serializer<Contracts.items>(mockLoggerSerializer.Object, mockEmailClient.Object);
            var miniBulk = priceQueueReader.BuildMiniBulk(message);

            // When.
            serializer.Serialize(miniBulk, new StreamWriter(path));

            // Then.
            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod]
        public void Serializer_SerializeHierarchyXmlToFile_XmlShouldBeSavedToDisk()
        {
            // Given.
            var mockLoggerSerializer = new Mock<ILogger<Serializer<Contracts.HierarchyType>>>();
            var mockLoggerQueueReader = new Mock<ILogger<HierarchyQueueReader>>();
            var mockEmailClient = new Mock<IEmailClient>();
            var mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueHierarchy>, List<MessageQueueHierarchy>>>();
            var mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueHierarchy>>>();

            var hierarchyQueueReader = new HierarchyQueueReader(
                mockLoggerQueueReader.Object,
                mockEmailClient.Object,
                mockGetMessageQueueQuery.Object,
                mockUpdateMessageQueueStatusCommandHandler.Object);
            var message = new List<MessageQueueHierarchy> { TestHelpers.GetFakeMessageQueueHierarchy(123, "Level", true) };

            string path = @"hierarchy.xml";
            var serializer = new Serializer<Contracts.HierarchyType>(mockLoggerSerializer.Object, mockEmailClient.Object);
            var miniBulk = hierarchyQueueReader.BuildMiniBulk(message);

            // When.
            serializer.Serialize(miniBulk, new StreamWriter(path));

            // Then.
            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod]
        public void Serializer_SerializeLocaleStoreXmlToFile_XmlShouldBeSavedToDisk()
        {
            // Given.
            var mockLoggerSerializer = new Mock<ILogger<Serializer<Contracts.LocaleType>>>();
            var mockLoggerQueueReader = new Mock<ILogger<LocaleQueueReader>>();
            var mockEmailClient = new Mock<IEmailClient>();
            var mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueLocale>, List<MessageQueueLocale>>>();
            var mockGetLocaleLineageQuery = new Mock<IQueryHandler<GetLocaleLineageParameters, LocaleLineageModel>>();
            var mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueLocale>>>();

            var localeQueueReader = new LocaleQueueReader(
                mockLoggerQueueReader.Object,
                mockEmailClient.Object,
                mockGetMessageQueueQuery.Object,
                mockGetLocaleLineageQuery.Object,
                mockUpdateMessageQueueStatusCommandHandler.Object);
            var message = new List<MessageQueueLocale> { TestHelpers.GetFakeMessageQueueLocale() };

            string path = @"locale_store.xml";
            var serializer = new Serializer<Contracts.LocaleType>(mockLoggerSerializer.Object, mockEmailClient.Object);
            var miniBulk = localeQueueReader.BuildMiniBulk(message);

            // When.
            serializer.Serialize(miniBulk, new StreamWriter(path));

            // Then.
            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod]
        public void Serializer_SerializeLocaleRegionXmlToFile_XmlShouldBeSavedToDisk()
        {
            // Given.
            var mockLoggerSerializer = new Mock<ILogger<Serializer<Contracts.LocaleType>>>();
            var mockLoggerQueueReader = new Mock<ILogger<LocaleQueueReader>>();
            var mockEmailClient = new Mock<IEmailClient>();
            var mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueLocale>, List<MessageQueueLocale>>>();
            var mockGetLocaleLineageQuery = new Mock<IQueryHandler<GetLocaleLineageParameters, LocaleLineageModel>>();
            var mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueLocale>>>();

            var localeQueueReader = new LocaleQueueReader(
                mockLoggerQueueReader.Object,
                mockEmailClient.Object,
                mockGetMessageQueueQuery.Object,
                mockGetLocaleLineageQuery.Object,
                mockUpdateMessageQueueStatusCommandHandler.Object);
            var message = new List<MessageQueueLocale> { TestHelpers.GetFakeMessageQueueLocaleRegion() };

            string path = @"locale_region.xml";
            var serializer = new Serializer<Contracts.LocaleType>(mockLoggerSerializer.Object, mockEmailClient.Object);
            var miniBulk = localeQueueReader.BuildMiniBulk(message);

            // When.
            serializer.Serialize(miniBulk, new StreamWriter(path));

            // Then.
            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod]
        public void Serializer_SerializeProductSelectionGroup_XmlShouldBeSavedToDisk()
        {
            // Given.
            var mockLoggerSerializer = new Mock<ILogger<Serializer<Contracts.SelectionGroupsType>>>();
            var mockLoggerQueueReader = new Mock<ILogger<ProductSelectionGroupQueueReader>>();
            var mockEmailClient = new Mock<IEmailClient>();
            var mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueProductSelectionGroup>, List<MessageQueueProductSelectionGroup>>>();
            var updateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueProductSelectionGroup>>>();

            var productSelectionGroupQueueReader = new ProductSelectionGroupQueueReader(
                mockLoggerQueueReader.Object,
                mockGetMessageQueueQuery.Object,
                updateMessageQueueStatusCommandHandler.Object);
            var message = new List<MessageQueueProductSelectionGroup> { TestHelpers.GetFakeMessageQueueProductSelectionGroup() };

            string path = @"psg.xml";
            var serializer = new Serializer<Contracts.SelectionGroupsType>(mockLoggerSerializer.Object, mockEmailClient.Object);
            var miniBulk = productSelectionGroupQueueReader.BuildMiniBulk(message);

            // When.
            serializer.Serialize(miniBulk, new StreamWriter(path));

            // Then.
            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod]
        public void Serializer_SerializeXmlToString_XmlStringShouldBeReturned()
        {
            // Given.
            var mockLogger = new Mock<ILogger<Serializer<Contracts.items>>>();
            var mockEmailClient = new Mock<IEmailClient>();

            var serializer = new Serializer<Contracts.items>(mockLogger.Object, mockEmailClient.Object);
            
            var miniBulk = TestHelpers.GetFakeProductMiniBulk();

            // When.
            string xml = serializer.Serialize(miniBulk, new Utf8StringWriter());

            // Then.
            Assert.IsNotNull(xml);
        }

        [TestMethod]
        public void Serializer_XmlString_ShouldHaveUtf8Encoding()
        {
            // Given.
            var mockLogger = new Mock<ILogger<Serializer<Contracts.items>>>();
            var mockEmailClient = new Mock<IEmailClient>();

            var serializer = new Serializer<Contracts.items>(mockLogger.Object, mockEmailClient.Object);
            
            var miniBulk = TestHelpers.GetFakeProductMiniBulk();

            // When.
            string xml = serializer.Serialize(miniBulk, new Utf8StringWriter());

            // Then.
            bool utf8 = xml.Contains("utf-8");
            bool utf16 = xml.Contains("utf-16");

            Assert.IsTrue(utf8);
            Assert.IsFalse(utf16);
        }

        [TestMethod]
        public void Serializer_SecondCallToSerialize_TextWriterShouldBeNewEachCall()
        {
            // Given.
            var mockLogger = new Mock<ILogger<Serializer<Contracts.items>>>();
            var mockEmailClient = new Mock<IEmailClient>();

            var serializer = new Serializer<Contracts.items>(mockLogger.Object, mockEmailClient.Object);
            
            var miniBulk = TestHelpers.GetFakeProductMiniBulk();

            // When.
            string xml = serializer.Serialize(miniBulk, new Utf8StringWriter());
            string anotherXml = serializer.Serialize(miniBulk, new Utf8StringWriter());

            // Then.
            Assert.AreEqual(xml.Length, anotherXml.Length);
        }
    }
}
