using Icon.Logging;
using Icon.Services.ItemPublisher.Infrastructure;
using Icon.Services.ItemPublisher.Infrastructure.Filters;
using Icon.Services.ItemPublisher.Infrastructure.MessageQueue;
using Icon.Services.ItemPublisher.Infrastructure.Models;
using Icon.Services.Newitem.Test.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Services.Tests
{
    [TestClass()]
    public class ItemProcessorTests
    {
        [TestMethod]
        public async Task ReadyForProcessing_ESBIsReady_ShouldReturnTrue()
        {
            Mock<IMessageQueueService> mockEsbService = new Mock<IMessageQueueService>();
            Mock<ILogger<ItemProcessor>> loggerMock = new Mock<ILogger<ItemProcessor>>();
            Mock<ISystemListBuilder> mockSystemListBuilder = new Mock<ISystemListBuilder>();
            Mock<IFilter> ukItemFilterMock = new Mock<IFilter>();
            mockEsbService.Setup(x => x.ReadyForProcessing).Returns(Task.FromResult(true));

            var itemProcessor = new ItemProcessor(mockEsbService.Object, loggerMock.Object, new ServiceSettings(), 
                mockSystemListBuilder.Object, ukItemFilterMock.Object);

            Assert.IsTrue(await itemProcessor.ReadyForProcessing);
        }

        [TestMethod]
        public async Task ReadyForProcessing_ESBIsNotReady_ShouldReturnFalse()
        {
            Mock<IMessageQueueService> mockEsbService = new Mock<IMessageQueueService>();
            Mock<ILogger<ItemProcessor>> loggerMock = new Mock<ILogger<ItemProcessor>>();
            Mock<ISystemListBuilder> mockSystemListBuilder = new Mock<ISystemListBuilder>();
            Mock<IFilter> ukItemFilterMock = new Mock<IFilter>();
            mockEsbService.Setup(x => x.ReadyForProcessing).Returns(Task.FromResult(false));

            var itemProcessor = new ItemProcessor(mockEsbService.Object, loggerMock.Object, new ServiceSettings(), 
                mockSystemListBuilder.Object, ukItemFilterMock.Object);

            Assert.IsFalse(await itemProcessor.ReadyForProcessing);
        }

        [TestMethod]
        public async Task ProcessRetail_RecordsAreFilteredAndProcessed()
        {
            // Given.
            Mock<IMessageQueueService> mockEsbService = new Mock<IMessageQueueService>();
            Mock<ILogger<ItemProcessor>> loggerMock = new Mock<ILogger<ItemProcessor>>();
            Mock<ISystemListBuilder> mockSystemListBuilder = new Mock<ISystemListBuilder>();
            Mock<IFilter> ukItemFilterMock = new Mock<IFilter>();
            mockEsbService.Setup(x => x.ReadyForProcessing).Returns(Task.FromResult(false));
            mockSystemListBuilder.Setup(x => x.BuildRetailNonReceivingSystemsList()).Returns(new List<string>()
            {
                "Test"
            });
            TestDataFactory testDataFactory = new TestDataFactory();
            var itemProcessor = new ItemProcessor(mockEsbService.Object, loggerMock.Object, new ServiceSettings()
            {
                NonReceivingSystemsProduct = new List<string>()
                {
                    "Test"
                }
            }, mockSystemListBuilder.Object, ukItemFilterMock.Object);

            MessageQueueItemModel modelRetailSale = testDataFactory.MessageQueueItemModel;
            modelRetailSale.Item.ItemTypeCode = ItemPublisherConstants.RetailSaleTypeCode;

            MessageQueueItemModel modelNonRetailSale = testDataFactory.MessageQueueItemModel;
            modelNonRetailSale.Item.ItemTypeCode = ItemPublisherConstants.NonRetailSaleTypeCode;

            MessageQueueItemModel modelDepartmentSale = testDataFactory.MessageQueueItemModel;
            modelDepartmentSale.Item.ItemAttributes[ItemPublisherConstants.Attributes.DepartmentSale] = "Yes";

            // When.
            List<MessageSendResult> result = await itemProcessor.ProcessRetailRecords(new List<MessageQueueItemModel>()
            {
               modelRetailSale,
               modelNonRetailSale,
               modelDepartmentSale
            });

            // Then.
            mockSystemListBuilder.Verify(x => x.BuildRetailNonReceivingSystemsList(), Times.Once);

            mockEsbService.Verify(x => x.Process(It.Is<List<MessageQueueItemModel>>(a =>
                a.Count == 2 && a[0].Item.ItemTypeCode == ItemPublisherConstants.RetailSaleTypeCode
            ),
            It.Is<List<string>>(b =>
                b[0] == "Test"
            )), "Give a list of three records only the retail item should have been processed.");
        }

        [TestMethod]
        public async Task ProcessRetail_UKRecordsAreFilteredAndRemoved()
        {
            // Given.
            Mock<IMessageQueueService> mockEsbService = new Mock<IMessageQueueService>();
            Mock<ILogger<ItemProcessor>> loggerMock = new Mock<ILogger<ItemProcessor>>();
            Mock<ISystemListBuilder> mockSystemListBuilder = new Mock<ISystemListBuilder>();
            Mock<ILogger<UKItemFilter>> ukItemFilterLogger = new Mock<ILogger<UKItemFilter>>();
            IFilter ukItemFilter = new UKItemFilter(ukItemFilterLogger.Object);
            mockEsbService.Setup(x => x.ReadyForProcessing).Returns(Task.FromResult(false));
            mockSystemListBuilder.Setup(x => x.BuildRetailNonReceivingSystemsList()).Returns(new List<string>()
            {
                "Test"
            });
            TestDataFactory testDataFactory = new TestDataFactory();
            var itemProcessor = new ItemProcessor(mockEsbService.Object, loggerMock.Object, new ServiceSettings()
            {
                NonReceivingSystemsProduct = new List<string>()
                {
                    "Test"
                }
            }, mockSystemListBuilder.Object, ukItemFilter);

            MessageQueueItemModel modelRetailSale = testDataFactory.MessageQueueItemModel;
            modelRetailSale.Item.ItemTypeCode = ItemPublisherConstants.RetailSaleTypeCode;

            MessageQueueItemModel modelNonRetailSale = testDataFactory.MessageQueueItemModel;
            modelNonRetailSale.Item.ItemTypeCode = ItemPublisherConstants.NonRetailSaleTypeCode;

            MessageQueueItemModel modelDepartmentSale = testDataFactory.MessageQueueItemModel;
            modelDepartmentSale.Item.ItemAttributes[ItemPublisherConstants.Attributes.DepartmentSale] = "Yes";

            MessageQueueItemModel modelUKItem = testDataFactory.MessageQueueItemModel;
            modelUKItem.Item.ItemTypeCode = ItemPublisherConstants.RetailSaleTypeCode;
            modelUKItem.Item.ItemAttributes.Add(ItemPublisherConstants.Attributes.UKItem, "Yes");


            // When.
            List<MessageSendResult> result = await itemProcessor.ProcessRetailRecords(new List<MessageQueueItemModel>()
            {
               modelRetailSale,
               modelNonRetailSale,
               modelDepartmentSale,
               modelUKItem
            });

            // Then.
            mockSystemListBuilder.Verify(x => x.BuildRetailNonReceivingSystemsList(), Times.Once);

            mockEsbService.Verify(x => x.Process(It.Is<List<MessageQueueItemModel>>(a =>
                a.Count == 2 && a[0].Item.ItemTypeCode == ItemPublisherConstants.RetailSaleTypeCode &&
                !a[0].Item.ItemAttributes.ContainsKey(ItemPublisherConstants.Attributes.UKItem) &&
                !a[1].Item.ItemAttributes.ContainsKey(ItemPublisherConstants.Attributes.UKItem)
            ),
            It.Is<List<string>>(b =>
                b[0] == "Test"
            )), "Give a list of four records only the retail item and non-UK Items should have been processed.");
        }

        [TestMethod]
        public async Task ProcessNonRetail_RecordsAreFilteredAndProcessed()
        {
            // Given.
            Mock<IMessageQueueService> mockEsbService = new Mock<IMessageQueueService>();
            Mock<ILogger<ItemProcessor>> loggerMock = new Mock<ILogger<ItemProcessor>>();
            Mock<ISystemListBuilder> mockSystemListBuilder = new Mock<ISystemListBuilder>();
            Mock<IFilter> ukItemFilterMock = new Mock<IFilter>();
            mockEsbService.Setup(x => x.ReadyForProcessing).Returns(Task.FromResult(false));
            mockSystemListBuilder.Setup(x => x.BuildNonRetailReceivingSystemsList()).Returns(new List<string>()
            {
                "Test"
            });
            TestDataFactory testDataFactory = new TestDataFactory();
            var itemProcessor = new ItemProcessor(mockEsbService.Object, loggerMock.Object, new ServiceSettings()
            {
                NonReceivingSystemsProduct = new List<string>()
                {
                    "Test"
                }
            }, mockSystemListBuilder.Object, ukItemFilterMock.Object);

            MessageQueueItemModel modelRetailSale = testDataFactory.MessageQueueItemModel;
            modelRetailSale.Item.ItemTypeCode = ItemPublisherConstants.RetailSaleTypeCode;

            MessageQueueItemModel modelNonRetailSale = testDataFactory.MessageQueueItemModel;
            modelNonRetailSale.Item.ItemTypeCode = ItemPublisherConstants.NonRetailSaleTypeCode;

            MessageQueueItemModel modelDepartmentSale = testDataFactory.MessageQueueItemModel;
            modelDepartmentSale.Item.ItemAttributes[ItemPublisherConstants.Attributes.DepartmentSale] = "Yes";

            // When.
            List<MessageSendResult> result = await itemProcessor.ProcessNonRetailRecords(new List<MessageQueueItemModel>()
            {
               modelRetailSale,
               modelNonRetailSale,
               modelDepartmentSale
            });

            // Then.
            mockSystemListBuilder.Verify(x => x.BuildNonRetailReceivingSystemsList(), Times.Once);

            mockEsbService.Verify(x => x.Process(It.Is<List<MessageQueueItemModel>>(a =>
                a.Count == 1 && a[0].Item.ItemTypeCode == ItemPublisherConstants.NonRetailSaleTypeCode
            ),
            It.Is<List<string>>(b =>
                b[0] == "Test"
            )), "Give a list of three records only the non retail item should have been processed.");
        }

        [TestMethod]
        public async Task ProcessNonRetail_UKRecordsAreFilteredAndRemoved()
        {
            // Given.
            Mock<IMessageQueueService> mockEsbService = new Mock<IMessageQueueService>();
            Mock<ILogger<ItemProcessor>> loggerMock = new Mock<ILogger<ItemProcessor>>();
            Mock<ISystemListBuilder> mockSystemListBuilder = new Mock<ISystemListBuilder>();
            Mock<ILogger<UKItemFilter>> ukItemFilterLogger = new Mock<ILogger<UKItemFilter>>();
            IFilter ukItemFilter = new UKItemFilter(ukItemFilterLogger.Object);
            mockEsbService.Setup(x => x.ReadyForProcessing).Returns(Task.FromResult(false));
            mockSystemListBuilder.Setup(x => x.BuildNonRetailReceivingSystemsList()).Returns(new List<string>()
            {
                "Test"
            });
            TestDataFactory testDataFactory = new TestDataFactory();
            var itemProcessor = new ItemProcessor(mockEsbService.Object, loggerMock.Object, new ServiceSettings()
            {
                NonReceivingSystemsProduct = new List<string>()
                {
                    "Test"
                }
            }, mockSystemListBuilder.Object, ukItemFilter);

            MessageQueueItemModel modelRetailSale = testDataFactory.MessageQueueItemModel;
            modelRetailSale.Item.ItemTypeCode = ItemPublisherConstants.RetailSaleTypeCode;

            MessageQueueItemModel modelNonRetailSale = testDataFactory.MessageQueueItemModel;
            modelNonRetailSale.Item.ItemTypeCode = ItemPublisherConstants.NonRetailSaleTypeCode;

            MessageQueueItemModel modelDepartmentSale = testDataFactory.MessageQueueItemModel;
            modelDepartmentSale.Item.ItemAttributes[ItemPublisherConstants.Attributes.DepartmentSale] = "Yes";

            MessageQueueItemModel modelUKItem = testDataFactory.MessageQueueItemModel;
            modelUKItem.Item.ItemTypeCode = ItemPublisherConstants.NonRetailSaleTypeCode;
            modelUKItem.Item.ItemAttributes.Add(ItemPublisherConstants.Attributes.UKItem, "Yes");

            // When.
            List<MessageSendResult> result = await itemProcessor.ProcessNonRetailRecords(new List<MessageQueueItemModel>()
            {
               modelRetailSale,
               modelNonRetailSale,
               modelDepartmentSale,
               modelUKItem
            });

            // Then.
            mockSystemListBuilder.Verify(x => x.BuildNonRetailReceivingSystemsList(), Times.Once);

            mockEsbService.Verify(x => x.Process(It.Is<List<MessageQueueItemModel>>(a =>
                a.Count == 1 && a[0].Item.ItemTypeCode == ItemPublisherConstants.NonRetailSaleTypeCode &&
                !a[0].Item.ItemAttributes.ContainsKey(ItemPublisherConstants.Attributes.UKItem)
            ),
            It.Is<List<string>>(b =>
                b[0] == "Test"
            )), "Give a list of four records only the non retail item and non-UK items should have been processed.");
        }

        [TestMethod]
        public async Task ProcessRetailWithDepartmentSale_RecordsAreFilteredAndProcessed()
        {
            // Given.
            Mock<IMessageQueueService> mockEsbService = new Mock<IMessageQueueService>();
            Mock<ILogger<ItemProcessor>> loggerMock = new Mock<ILogger<ItemProcessor>>();
            Mock<ISystemListBuilder> mockSystemListBuilder = new Mock<ISystemListBuilder>();
            Mock<IFilter> ukItemFilterMock = new Mock<IFilter>();
            mockEsbService.Setup(x => x.ReadyForProcessing).Returns(Task.FromResult(false));
            mockSystemListBuilder.Setup(x => x.BuildRetailNonReceivingSystemsList()).Returns(new List<string>()
            {
                "Test"
            });
            TestDataFactory testDataFactory = new TestDataFactory();
            var itemProcessor = new ItemProcessor(mockEsbService.Object, loggerMock.Object, new ServiceSettings()
            {
                NonReceivingSystemsProduct = new List<string>()
                {
                    "Test"
                }
            }, mockSystemListBuilder.Object, ukItemFilterMock.Object);

            MessageQueueItemModel modelRetailSale = testDataFactory.MessageQueueItemModel;
            modelRetailSale.Item.ItemAttributes[ItemPublisherConstants.Attributes.DepartmentSale] = "Yes";

            MessageQueueItemModel modelNonRetailSale = testDataFactory.MessageQueueItemModel;
            modelNonRetailSale.Item.ItemTypeCode = ItemPublisherConstants.NonRetailSaleTypeCode;

            // When.
            List<MessageSendResult> result = await itemProcessor.ProcessRetailRecords(new List<MessageQueueItemModel>()
            {
               modelRetailSale,
               modelNonRetailSale
            });

            // Then.
            mockSystemListBuilder.Verify(x => x.BuildRetailNonReceivingSystemsList(), Times.Once);

            mockEsbService.Verify(x => x.Process(It.Is<List<MessageQueueItemModel>>(a =>
                a.Count == 1 && a[0].Item.ItemAttributes[ItemPublisherConstants.Attributes.DepartmentSale] == "Yes"
            ),
            It.Is<List<string>>(b =>
                b[0] == "Test"
            )), "Give a list of three records only the department sale item should have been processed.");
        }
    }
}