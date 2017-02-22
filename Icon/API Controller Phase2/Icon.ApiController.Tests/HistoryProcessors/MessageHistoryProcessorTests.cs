using Icon.ApiController.Controller.HistoryProcessors;
using Icon.ApiController.DataAccess.Commands;

using Icon.ApiController.DataAccess.Queries;
using Icon.Esb.Producer;
using Icon.RenewableContext;
using Icon.Framework;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Icon.Testing.Builders;
using Icon.Common.DataAccess;
using Icon.ApiController.Common;
using Icon.ApiController.Controller.ControllerConstants;

namespace Icon.ApiController.Tests.HistoryProcessors
{
    [TestClass]
    public class MessageHistoryProcessorTests
    {
        private MessageHistoryProcessor historyProcessor;
        private Mock<ILogger<MessageHistoryProcessor>> mockLogger;
        private Mock<ICommandHandler<MarkUnsentMessagesAsInProcessCommand>> mockMarkUnsentMessagesCommand;
        private Mock<IQueryHandler<GetMessageHistoryParameters, List<MessageHistory>>> mockGetMessageHistoryQuery;
        private Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>> mockUpdateMessageHistoryCommand;
        private Mock<ICommandHandler<UpdateSentToEsbHierarchyTraitCommand>> mockUpdateSentToEsbHierarchyTraitCommand;
        private Mock<ICommandHandler<UpdateStagedProductStatusCommand>> mockUpdateStagedProductStatusCommand;
        private Mock<IQueryHandler<IsMessageHistoryANonRetailProductMessageParameters, bool>> mockIsMessageHistoryANonRetailProductMessageQueryHandler;
        private Mock<IEsbProducer> mockProducer;
        private ApiControllerSettings settings;

        // app config key constants
        const string nonReceivingSystemsKey = EsbConstants.NonReceivingSystemsJmsProperty; //"nonReceivingSysName"
        const string nonReceivingSystemsAllKey = "NonReceivingSystemsAll";
        const string nonReceivingSystemsItemLocaleKey = "NonReceivingSystemsItemLocale";
        const string nonReceivingSystemsPriceKey = "NonReceivingSystemsPrice";

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger<MessageHistoryProcessor>>();
            mockMarkUnsentMessagesCommand = new Mock<ICommandHandler<MarkUnsentMessagesAsInProcessCommand>>();
            mockGetMessageHistoryQuery = new Mock<IQueryHandler<GetMessageHistoryParameters, List<MessageHistory>>>();
            mockUpdateMessageHistoryCommand = new Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>>();
            mockUpdateStagedProductStatusCommand = new Mock<ICommandHandler<UpdateStagedProductStatusCommand>>();
            mockUpdateSentToEsbHierarchyTraitCommand = new Mock<ICommandHandler<UpdateSentToEsbHierarchyTraitCommand>>();
            mockIsMessageHistoryANonRetailProductMessageQueryHandler = new Mock<IQueryHandler<IsMessageHistoryANonRetailProductMessageParameters, bool>>();
            mockProducer = new Mock<IEsbProducer>();
            settings = new ApiControllerSettings();

            this.historyProcessor = new MessageHistoryProcessor(
                settings,
                mockLogger.Object,
                mockMarkUnsentMessagesCommand.Object,
                mockGetMessageHistoryQuery.Object,
                mockUpdateMessageHistoryCommand.Object,
                mockUpdateStagedProductStatusCommand.Object,
                mockUpdateSentToEsbHierarchyTraitCommand.Object,
                mockIsMessageHistoryANonRetailProductMessageQueryHandler.Object,
                mockProducer.Object,
                MessageTypes.Product);
        }

        [TestMethod]
        public void HistoryProcessor_NoMessages_SendShouldNotBeCalled()
        {
            // Given.
            mockGetMessageHistoryQuery.Setup(q => q.Search(It.IsAny<GetMessageHistoryParameters>())).Returns(new List<MessageHistory>());

            // When.
            this.historyProcessor.ProcessMessageHistory();

            // Then.
            mockProducer.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
        }

        [TestMethod]
        public void HistoryProcessor_OneMessageInProcess_SendShouldBeCalledOnce()
        {
            // Given.
            var fakeMessageHistory = new List<MessageHistory>
            {
                new TestMessageHistoryBuilder()
            };

            var fakeEmptyMessageHistory = new List<MessageHistory>();

            var fakeMessageHistoryQueue = new Queue<List<MessageHistory>>();
            fakeMessageHistoryQueue.Enqueue(fakeMessageHistory);
            fakeMessageHistoryQueue.Enqueue(fakeEmptyMessageHistory);

            mockGetMessageHistoryQuery.Setup(q => q.Search(It.IsAny<GetMessageHistoryParameters>())).Returns(fakeMessageHistoryQueue.Dequeue);
            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            // When.
            this.historyProcessor.ProcessMessageHistory();

            // Then.
            mockProducer.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        [TestMethod]
        public void HistoryProcessor_ThreeMessagesInProcess_SendShouldBeCalledThreeTimes()
        {
            // Given.
            var fakeMessageHistory = new List<MessageHistory>
            {
                new TestMessageHistoryBuilder(),
                new TestMessageHistoryBuilder(),
                new TestMessageHistoryBuilder()
            };

            var fakeEmptyMessageHistory = new List<MessageHistory>();

            var fakeMessageHistoryQueue = new Queue<List<MessageHistory>>();
            fakeMessageHistoryQueue.Enqueue(fakeMessageHistory);
            fakeMessageHistoryQueue.Enqueue(fakeEmptyMessageHistory);

            mockGetMessageHistoryQuery.Setup(q => q.Search(It.IsAny<GetMessageHistoryParameters>())).Returns(fakeMessageHistoryQueue.Dequeue);
            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            // When.
            this.historyProcessor.ProcessMessageHistory();

            // Then.
            mockProducer.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Exactly(3));
        }

        [TestMethod]
        public void HistoryProcessor_MessageTypeIsHierarchy_ShouldUpdateSentToEsbTraitAndStageProducts()
        {
            //Given
            historyProcessor = new MessageHistoryProcessor(
                settings,
                mockLogger.Object,
                mockMarkUnsentMessagesCommand.Object,
                mockGetMessageHistoryQuery.Object,
                mockUpdateMessageHistoryCommand.Object,
                mockUpdateStagedProductStatusCommand.Object,
                mockUpdateSentToEsbHierarchyTraitCommand.Object,
                mockIsMessageHistoryANonRetailProductMessageQueryHandler.Object,
                mockProducer.Object,
                MessageTypes.Hierarchy);

            var fakeMessageHistory = new List<MessageHistory>
            {
                new TestMessageHistoryBuilder().WithMessageTypeId(MessageTypes.Hierarchy)
            };

            var fakeEmptyMessageHistory = new List<MessageHistory>();
            var fakeMessageHistoryQueue = new Queue<List<MessageHistory>>();

            fakeMessageHistoryQueue.Enqueue(fakeMessageHistory);
            fakeMessageHistoryQueue.Enqueue(fakeEmptyMessageHistory);

            mockGetMessageHistoryQuery.Setup(q => q.Search(It.IsAny<GetMessageHistoryParameters>())).Returns(fakeMessageHistoryQueue.Dequeue);
            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            // When.
            this.historyProcessor.ProcessMessageHistory();

            // Then.
            mockProducer.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            mockUpdateSentToEsbHierarchyTraitCommand.Verify(m => m.Execute(It.IsAny<UpdateSentToEsbHierarchyTraitCommand>()), Times.Once);
            mockUpdateStagedProductStatusCommand.Verify(m => m.Execute(It.IsAny<UpdateStagedProductStatusCommand>()), Times.Once);
        }

        protected void SetupMockMessageHistory(int messageTypeId, List<MessageHistory> messageHistoryList = null)
        {
            const string testMsg = "testMsg";
            //build the message history data, using any provided parameters
            var fakeActualMessageHistory = (null == messageHistoryList)
                ? new List<MessageHistory> { new TestMessageHistoryBuilder().WithMessageTypeId(messageTypeId).WithMessage(testMsg) }
                : messageHistoryList;
            var fakeEmptyMessageHistory = new List<MessageHistory>();
            var fakeQueue = new Queue<List<MessageHistory>>();
            fakeQueue.Enqueue(fakeActualMessageHistory);
            fakeQueue.Enqueue(fakeEmptyMessageHistory);

            mockGetMessageHistoryQuery = new Mock<IQueryHandler<GetMessageHistoryParameters, List<MessageHistory>>>();
            mockGetMessageHistoryQuery.Setup(q => q.Search(It.IsAny<GetMessageHistoryParameters>())).Returns(fakeQueue.Dequeue);
        }

        protected void InitMessageHistoryProcessor(ApiControllerSettings settings, int messageTypeId)
        {
            historyProcessor = new MessageHistoryProcessor(
              settings,
              mockLogger.Object,
              mockMarkUnsentMessagesCommand.Object,
              mockGetMessageHistoryQuery.Object,
              mockUpdateMessageHistoryCommand.Object,
              mockUpdateStagedProductStatusCommand.Object,
              mockUpdateSentToEsbHierarchyTraitCommand.Object,
              mockIsMessageHistoryANonRetailProductMessageQueryHandler.Object,
              mockProducer.Object,
              messageTypeId);
        }

        protected List<MessageHistory> CreateMessageHistoryForProduct(string itemTypeCode, string testMsg = "testMsg")
        {
            return CreateMessageHistoryForProduct(new List<string>() { itemTypeCode }, testMsg);
        }

        protected List<MessageHistory> CreateMessageHistoryForProduct(List<string> itemTypeCodes, string testMsg = "testMsg")
        {
            var messageHistoryList = new List<MessageHistory>();

            foreach (var itemTypeCode in itemTypeCodes)
            {
                MessageQueueProduct productQueue = new MessageQueueProduct() { ItemTypeCode = itemTypeCode };
                List<MessageQueueProduct> messageQueuesForProduct = new List<MessageQueueProduct>() { productQueue };
                messageHistoryList.Add(
                    new TestMessageHistoryBuilder()
                        .WithMessageTypeId(MessageTypes.Product)
                        .WithMessage(testMsg)
                        .WithProductMessageQueue(messageQueuesForProduct));
            }
            return messageHistoryList;
        }

        [TestMethod]
        public void HistoryProcessor_MessageTypeProduct_NonRetail_NonReceivingSystems_ShouldBeR10_EvenWhenNoSettingForNonReceivingSystems()
        {
            //Given
            // setup fake messages & mock queues for test
            var messageHistoryList = CreateMessageHistoryForProduct(ItemTypeCodes.NonRetail);
            SetupMockMessageHistory(MessageTypes.Product, messageHistoryList);
            const string nonReceivingSystemR10 = "R10";
            // simulate loading settings from app.config
            // (create settings with no values for NonReceivingSystemsAll)
            settings = new ApiControllerSettings();
            //setup processor object with settings and mocks
            InitMessageHistoryProcessor(settings, MessageTypes.Product);
            mockIsMessageHistoryANonRetailProductMessageQueryHandler.Setup(m => m.Search(It.IsAny<IsMessageHistoryANonRetailProductMessageParameters>()))
                .Returns(true);

            // When.
            this.historyProcessor.ProcessMessageHistory();

            // Then.
            mockProducer.Verify(p =>
                p.Send(
                    It.IsAny<string>(),
                    It.Is<Dictionary<string, string>>(dict => dict[nonReceivingSystemsKey] == nonReceivingSystemR10)
                ), Times.Once);
        }

        [TestMethod]
        public void HistoryProcessor_MessageTypeProduct_NonRetail_NonReceivingSystems_ShouldAddR10ToAnyOtherSettingForNonReceivingSystems()
        {
            //Given
            // setup fake messages & mock queues for test
            var messageHistoryList = CreateMessageHistoryForProduct(ItemTypeCodes.NonRetail);
            SetupMockMessageHistory(MessageTypes.Product, messageHistoryList);
            //simulate loading settings from app.config
            const string nonReceivingSystemAll = "XYZ";
            const string nonReceivingSystemR10 = "R10";
            settings = new ApiControllerSettings()
            {
                NonReceivingSystemsAll = nonReceivingSystemAll
            };
            //setup processor object with settings and mocks
            InitMessageHistoryProcessor(settings, MessageTypes.Product);
            mockIsMessageHistoryANonRetailProductMessageQueryHandler.Setup(m => m.Search(It.IsAny<IsMessageHistoryANonRetailProductMessageParameters>()))
                .Returns(true);

            // When.
            this.historyProcessor.ProcessMessageHistory();

            // Then.
            //verify that the message was sent with the expected property value
            mockProducer.Verify(p =>
                p.Send(
                    It.IsAny<string>(),
                    It.Is<Dictionary<string, string>>(dict => dict[nonReceivingSystemsKey] == nonReceivingSystemR10 + "," + nonReceivingSystemAll)
                ), Times.Once);
        }

        [TestMethod]
        public void HistoryProcessor_MessageTypeProduct_NonRetail_NonReceivingSystems_ShouldAddR10_IgnoringUnrelatedNonReceivingSystemSettings()
        {
            //Given
            // setup fake messages & mock queues for test
            var messageHistoryList = CreateMessageHistoryForProduct(ItemTypeCodes.NonRetail);
            SetupMockMessageHistory(MessageTypes.Product, messageHistoryList);
            //simulate loading settings from app.config
            const string nonReceivingSystemPrice = "ABCD";
            const string nonReceivingSystemR10 = "R10";
            settings = new ApiControllerSettings()
            {
                NonReceivingSystemsPrice = nonReceivingSystemPrice
            };
            //setup processor object with settings and mocks
            InitMessageHistoryProcessor(settings, MessageTypes.Product);
            mockIsMessageHistoryANonRetailProductMessageQueryHandler.Setup(m => m.Search(It.IsAny<IsMessageHistoryANonRetailProductMessageParameters>()))
                .Returns(true);

            // When.
            this.historyProcessor.ProcessMessageHistory();

            // Then.
            //verify that the message was sent with the expected property value
            mockProducer.Verify(p =>
                p.Send(
                    It.IsAny<string>(),
                    It.Is<Dictionary<string, string>>(dict => dict[nonReceivingSystemsKey] == nonReceivingSystemR10)
                ), Times.Once);
        }

        [TestMethod]
        public void HistoryProcessor_MessageTypeProduct_RetailSale_NonReceivingSystems_ShouldNotAutomaticallyBeSetToR10()
        {
            //Given
            // setup fake messages & mock queues for test
            var messageHistoryList = CreateMessageHistoryForProduct(ItemTypeCodes.RetailSale);
            SetupMockMessageHistory(MessageTypes.Product, messageHistoryList);
            // simulate loading settings from app.config
            // (create settings with no values for NonReceivingSystemsAll)
            settings = new ApiControllerSettings();
            //setup processor object with settings and mocks
            InitMessageHistoryProcessor(settings, MessageTypes.Product);

            // When.
            this.historyProcessor.ProcessMessageHistory();

            // Then.
            mockProducer.Verify(p =>
                p.Send(
                    It.IsAny<string>(),
                    It.Is<Dictionary<string, string>>(dict => dict.ContainsKey(nonReceivingSystemsKey))
                ), Times.Never);
        }

        [TestMethod]
        public void HistoryProcessor_MessageTypeProduct_RetailSale_NonReceivingSystems_ShouldNotAutomaticallyBeR10_ButShouldHaveOtherNonReceivingSystemsFromSettings()
        {
            //Given
            // setup fake messages & mock queues for test
            var messageHistoryList = CreateMessageHistoryForProduct(ItemTypeCodes.RetailSale);
            SetupMockMessageHistory(MessageTypes.Product, messageHistoryList); ;
            //simulate loading settings from app.config
            const string nonReceivingSystemAll = "XYZ";
            settings = new ApiControllerSettings()
            {
                NonReceivingSystemsAll = nonReceivingSystemAll
            };
            //setup processor object with settings and mocks
            InitMessageHistoryProcessor(settings, MessageTypes.Product);

            // When.
            this.historyProcessor.ProcessMessageHistory();

            // Then.
            //verify that the message was sent with the expected property value
            mockProducer.Verify(p =>
                p.Send(
                    It.IsAny<string>(),
                    It.Is<Dictionary<string, string>>(dict => dict[nonReceivingSystemsKey] == nonReceivingSystemAll)
                ), Times.Once);
        }

        [TestMethod]
        public void HistoryProcessor_MessageTypeProduct_RetailSalePlusNonRetail_NonReceivingSystems_ShouldAutomaticallyIncludeR10_OnlyForNonRetailMessages()
        {
            //Given
            // setup fake messages & mock queues for test
            var messageHistoryList = CreateMessageHistoryForProduct(new List<string>() { ItemTypeCodes.NonRetail, ItemTypeCodes.RetailSale });
            SetupMockMessageHistory(MessageTypes.Product, messageHistoryList);
            //simulate loading settings from app.config
            const string nonReceivingSystemAll = "XYZ";
            const string nonReceivingSystemR10 = "R10";
            settings = new ApiControllerSettings()
            {
                NonReceivingSystemsAll = nonReceivingSystemAll
            };
            //setup processor object with settings and mocks
            InitMessageHistoryProcessor(settings, MessageTypes.Product);
            mockIsMessageHistoryANonRetailProductMessageQueryHandler.SetupSequence(m => m.Search(It.IsAny<IsMessageHistoryANonRetailProductMessageParameters>()))
                .Returns(true)
                .Returns(false);

            // When.
            this.historyProcessor.ProcessMessageHistory();

            // Then.
            //verify that the message was sent with the expected property value
            //should send once for non-retail & once for retail products
            mockProducer.Verify(p =>
                p.Send(
                    It.IsAny<string>(),
                    It.Is<Dictionary<string, string>>(dict => dict[nonReceivingSystemsKey] == nonReceivingSystemR10 + "," + nonReceivingSystemAll)
                ), Times.Once);
            mockProducer.Verify(p =>
                p.Send(
                    It.IsAny<string>(),
                    It.Is<Dictionary<string, string>>(dict => dict[nonReceivingSystemsKey] == nonReceivingSystemAll)
                ), Times.Once);
        }

        [TestMethod]
        public void HistoryProcessor_MessageTypePrice_NonReceivingSystems_ShouldUseValueFromSettings_ForNonReceivingSystemsAll()
        {
            //Given
            //setup mock queues for test
            SetupMockMessageHistory(MessageTypes.Price);
            //simulate loading settings from app.config
            const string nonReceivingSystemAll = "XYZ";
            settings = new ApiControllerSettings()
            {
                NonReceivingSystemsAll = nonReceivingSystemAll
            };
            //setup processor object with settings and mocks
            InitMessageHistoryProcessor(settings, MessageTypes.Price);

            // When.
            this.historyProcessor.ProcessMessageHistory();

            // Then.
            //verify that the message was sent with the expected property value
            mockProducer.Verify(p =>
                p.Send(
                    It.IsAny<string>(),
                    It.Is<Dictionary<string, string>>(dict => dict[nonReceivingSystemsKey] == nonReceivingSystemAll)
                ), Times.Once);
        }

        [TestMethod]
        public void HistoryProcessor_MessageTypePrice_NonReceivingSystems_ShouldUseValueFromSettings_ForNonReceivingSystemsPrice()
        {
            //Given
            //setup mock queues for test
            SetupMockMessageHistory(MessageTypes.Price);
            //simulate loading settings from app.config
            const string nonReceivingSystemPrice = "ABCD";
            settings = new ApiControllerSettings()
            {
                NonReceivingSystemsPrice = nonReceivingSystemPrice
            };
            //setup processor object with settings and mocks
            InitMessageHistoryProcessor(settings, MessageTypes.Price);

            // When.
            this.historyProcessor.ProcessMessageHistory();

            // Then.
            //verify that the message was sent with the expected property value
            mockProducer.Verify(p =>
                p.Send(
                    It.IsAny<string>(),
                    It.Is<Dictionary<string, string>>(dict => dict[nonReceivingSystemsKey] == nonReceivingSystemPrice)
                ), Times.Once);
        }

        [TestMethod]
        public void HistoryProcessor_MessageTypePrice_NonReceivingSystems_ShouldSetWithBothValuesFromSettings_ForNonReceivingSystemsAllAndPrice()
        {
            //Given
            //setup mock queues for test
            SetupMockMessageHistory(MessageTypes.Price);
            //simulate loading settings from app.config
            const string nonReceivingSystemAll = "XYZ";
            const string nonReceivingSystemPrice = "ABCD";
            settings = new ApiControllerSettings()
            {
                NonReceivingSystemsAll = nonReceivingSystemAll,
                NonReceivingSystemsPrice = nonReceivingSystemPrice
            };
            //setup processor object with settings and mocks
            InitMessageHistoryProcessor(settings, MessageTypes.Price);

            // When.
            this.historyProcessor.ProcessMessageHistory();

            // Then.
            //verify that the message was sent with the expected property value
            mockProducer.Verify(p =>
                p.Send(
                    It.IsAny<string>(),
                    It.Is<Dictionary<string, string>>(dict => dict[nonReceivingSystemsKey] == nonReceivingSystemPrice + "," + nonReceivingSystemAll)
                ), Times.Once);
        }

        [TestMethod]
        public void HistoryProcessor_MessageTypePrice_NonReceivingSystems_ShouldHaveNoValue_WhenNoSettingForNonReceivingSystems()
        {
            //Given
            //setup mock queues for test
            SetupMockMessageHistory(MessageTypes.Price);
            // simulate loading settings from app.config
            // (create settings with no values for NonReceivingSystemsAll, NonReceivingSystemsPrice)
            settings = new ApiControllerSettings();
            //setup processor object with settings and mocks
            InitMessageHistoryProcessor(settings, MessageTypes.Price);

            // When.
            this.historyProcessor.ProcessMessageHistory();

            // Then.
            mockProducer.Verify(p =>
                p.Send(
                    It.IsAny<string>(),
                    It.Is<Dictionary<string, string>>(dict => dict.ContainsKey(nonReceivingSystemsKey))
                ), Times.Never);
        }

        [TestMethod]
        public void HistoryProcessor_MessageTypeItemLocale_NonReceivingSystems_ShouldUseValueFromSettings_ForNonReceivingSystemsAll()
        {
            //Given
            //setup mock queues for test
            SetupMockMessageHistory(MessageTypes.ItemLocale);
            // simulate loading settings from app.config
            const string nonReceivingSystemAll = "XYZ";
            settings = new ApiControllerSettings()
            {
                NonReceivingSystemsAll = nonReceivingSystemAll
            };
            //setup processor object with settings and mocks
            InitMessageHistoryProcessor(settings, MessageTypes.ItemLocale);

            // When.
            this.historyProcessor.ProcessMessageHistory();

            // Then.
            //verify that the message was sent with the expected property value
            mockProducer.Verify(p =>
                p.Send(
                    It.IsAny<string>(),
                    It.Is<Dictionary<string, string>>(dict => dict[nonReceivingSystemsKey] == nonReceivingSystemAll)
                ), Times.Once);
        }

        [TestMethod]
        public void HistoryProcessor_MessageTypeItemLocale_NonReceivingSystems_ShouldUseValueFromSettings_ForNonReceivingSystemsItemLocale()
        {
            //Given
            //setup mock queues for test
            SetupMockMessageHistory(MessageTypes.ItemLocale);
            // simulate loading settings from app.config
            const string nonReceivingSystemItemLocale = "ABCD";
            settings = new ApiControllerSettings()
            {
                NonReceivingSystemsItemLocale = nonReceivingSystemItemLocale
            };
            //setup processor object with settings and mocks
            InitMessageHistoryProcessor(settings, MessageTypes.ItemLocale);

            // When.
            this.historyProcessor.ProcessMessageHistory();

            // Then.
            //verify that the message was sent with the expected property value
            mockProducer.Verify(p =>
                p.Send(
                    It.IsAny<string>(),
                    It.Is<Dictionary<string, string>>(dict => dict[nonReceivingSystemsKey] == nonReceivingSystemItemLocale)
                ), Times.Once);
        }

        [TestMethod]
        public void HistoryProcessor_MessageTypeItemLocale_NonReceivingSystems_ShouldUseBothValuesFromSettings_ForNonReceivingSystemsAllAndItemLocale()
        {
            //Given
            //setup mock queues for test
            SetupMockMessageHistory(MessageTypes.ItemLocale);
            // simulate loading settings from app.config
            const string nonReceivingSystemAll = "XYZ";
            const string nonReceivingSystemItemLocale = "ABCD";
            settings = new ApiControllerSettings()
            {
                NonReceivingSystemsAll = nonReceivingSystemAll,
                NonReceivingSystemsItemLocale = nonReceivingSystemItemLocale
            };
            //setup processor object with settings and mocks
            InitMessageHistoryProcessor(settings, MessageTypes.ItemLocale);

            // When.
            this.historyProcessor.ProcessMessageHistory();

            // Then.
            //verify that the message was sent with the expected property value
            mockProducer.Verify(p =>
                p.Send(
                    It.IsAny<string>(),
                    It.Is<Dictionary<string, string>>(dict => dict[nonReceivingSystemsKey] == nonReceivingSystemItemLocale + "," + nonReceivingSystemAll)
                ), Times.Once);
        }

        [TestMethod]
        public void HistoryProcessor_MessageTypeItemLocale_NonReceivingSystems_ShouldNotHaveValueWhenNoSetting_ForNonReceivingSystems()
        {
            //Given
            //setup mock queues for test
            SetupMockMessageHistory(MessageTypes.ItemLocale);
            // simulate loading settings from app.config
            // (create settings with no values for NonReceivingSystemsAll, NonReceivingSystemsItemLocale)
            settings = new ApiControllerSettings();
            //setup processor object with settings and mocks
            InitMessageHistoryProcessor(settings, MessageTypes.ItemLocale);

            // When.
            this.historyProcessor.ProcessMessageHistory();

            // Then.
            mockProducer.Verify(p =>
                p.Send(
                    It.IsAny<string>(),
                    It.Is<Dictionary<string, string>>(dict => dict.ContainsKey(nonReceivingSystemsKey))
                ), Times.Never);
        }
    }
}
