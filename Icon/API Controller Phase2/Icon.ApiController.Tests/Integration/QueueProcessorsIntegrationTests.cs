using Icon.ApiController.Common;
using Icon.ApiController.Controller.Mappers;
using Icon.ApiController.Controller.Monitoring;
using Icon.ApiController.Controller.QueueProcessors;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb.Producer;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;
using System.Transactions;

namespace Icon.ApiController.Tests.Integration
{
    [TestClass]
    public class QueueProcessorsIntegrationTests
    {
        private ApiControllerSettings settings;
        private IconContext context;
        private TransactionScope transaction;
        private IconDbContextFactory iconContextFactory;
        private Mock<IEsbProducer> mockProducer;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });

            context = new IconContext();
            iconContextFactory = new IconDbContextFactory();

            mockProducer = new Mock<IEsbProducer>();
            settings = new ApiControllerSettings();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void ProductQueueProcessor_ValidProductRecords_ShouldBeProcessedSuccessfully()
        {
            // Given.
            var fakeMessageQueueProducts = new List<MessageQueueProduct> 
            { 
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 123, "0", ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 234, "0", ItemTypeCodes.RetailSale)
            };

            var fakeEmptyMessageQueueProducts = new List<MessageQueueProduct>();

            var fakeMessageQueue = new Queue<List<MessageQueueProduct>>();
            fakeMessageQueue.Enqueue(fakeMessageQueueProducts);
            fakeMessageQueue.Enqueue(fakeEmptyMessageQueueProducts);

            context.MessageQueueProduct.AddRange(fakeMessageQueueProducts);
            context.SaveChanges();

            ControllerType.Instance = 1;

            var mockQueueProcessorLogger = new Mock<ILogger<ProductQueueProcessor>>();
            var mockContext = new Mock<IRenewableContext<IconContext>>();
            var mockSerializerLogger = new Mock<ILogger<Serializer<Contracts.items>>>();
            var mockUpdateMessageQueueStatusLogger = new Mock<ILogger<UpdateMessageQueueStatusCommandHandler<MessageQueueProduct>>>();
            var mockQueueReaderLogger = new Mock<ILogger<ProductQueueReader>>();
            var mockEmailClient = new Mock<IEmailClient>();
            var mockAssociateMessageToQueueLogger = new Mock<ILogger<AssociateMessageToQueueCommandHandler<MessageQueueProduct>>>();
            var mockSaveToMessageHistoryLogger = new Mock<ILogger<SaveToMessageHistoryCommandHandler>>();
            var mockSetProcessedDateLogger = new Mock<ILogger<UpdateMessageQueueProcessedDateCommandHandler<MessageQueueProduct>>>();
            var mockUpdateMessageHistoryLogger = new Mock<ILogger<UpdateMessageHistoryStatusCommandHandler>>();
            var mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueProduct>, List<MessageQueueProduct>>>();
            var mockMarkMessagesAsInProcessCommand = new Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueProduct>>>();
            var mockProductSelectionGroupsMapper = new Mock<IProductSelectionGroupsMapper>();
            var mockUomMapper = new Mock<IUomMapper>();

            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            mockGetMessageQueueQuery.Setup(q => q.Search(It.IsAny<GetMessageQueueParameters<MessageQueueProduct>>())).Returns(fakeMessageQueue.Dequeue);

            var serializer = new Serializer<Contracts.items>(mockSerializerLogger.Object, mockEmailClient.Object);
            var queueReader = new ProductQueueReader(
                mockQueueReaderLogger.Object,
                mockEmailClient.Object,
                mockGetMessageQueueQuery.Object,
                new UpdateMessageQueueStatusCommandHandler<MessageQueueProduct>(mockUpdateMessageQueueStatusLogger.Object, iconContextFactory),
                mockProductSelectionGroupsMapper.Object,
                mockUomMapper.Object,
                settings);
            var saveXmlMessageCommandHandler = new SaveToMessageHistoryCommandHandler(mockSaveToMessageHistoryLogger.Object, iconContextFactory);
            var associateMessageToMessageQueueCommandHandler = new AssociateMessageToQueueCommandHandler<MessageQueueProduct>(mockAssociateMessageToQueueLogger.Object, iconContextFactory);
            var setProcessedDateCommandHandler = new UpdateMessageQueueProcessedDateCommandHandler<MessageQueueProduct>(mockSetProcessedDateLogger.Object, iconContextFactory);
            var updateMessageHistoryCommandHandler = new UpdateMessageHistoryStatusCommandHandler(mockUpdateMessageHistoryLogger.Object, iconContextFactory);
            var updateMessageQueueStatusCommandHandler = new UpdateMessageQueueStatusCommandHandler<MessageQueueProduct>(mockUpdateMessageQueueStatusLogger.Object, iconContextFactory);
            var mockMonitor = new Mock<IMessageProcessorMonitor>();

            var productQueueProcessor = new ProductQueueProcessor(
                settings,
                mockQueueProcessorLogger.Object,
                queueReader,
                serializer,
                saveXmlMessageCommandHandler,
                associateMessageToMessageQueueCommandHandler,
                setProcessedDateCommandHandler,
                updateMessageHistoryCommandHandler,
                updateMessageQueueStatusCommandHandler,
                mockMarkMessagesAsInProcessCommand.Object,
                mockProducer.Object,
                mockMonitor.Object);

            // When.
            productQueueProcessor.ProcessMessageQueue();

            // Then.
            var originalQueuedMessageId = fakeMessageQueueProducts.Select(p => p.MessageQueueId).ToList();
            var queuedMessages = context.MessageQueueProduct.Where(mq => originalQueuedMessageId.Contains(mq.MessageQueueId)).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var message in queuedMessages)
            {
                context.Entry(message).Reload();
            }

            int savedMessageId = queuedMessages.First().MessageHistoryId.Value;
            var savedMessage = context.MessageHistory.Single(mh => savedMessageId == mh.MessageHistoryId);

            bool allQueuedMessagesAreUpdated = queuedMessages.TrueForAll(m =>
                !m.InProcessBy.HasValue &&
                m.ProcessedDate.Value.Date == DateTime.Today &&
                m.MessageStatusId == MessageStatusTypes.Associated &&
                m.MessageHistoryId != null);

            Assert.IsTrue(allQueuedMessagesAreUpdated);
            Assert.AreEqual(MessageStatusTypes.Sent, savedMessage.MessageStatusId);
        }

        [TestMethod]
        public void ItemLocaleQueueProcessor_ValidItemLocaleRecords_ShouldBeProcessedSuccessfully()
        {
            // Given.
            IRMAPush irmaPushEntry = new TestIrmaPushBuilder();

            context.IRMAPush.Add(irmaPushEntry);
            context.SaveChanges();

            var fakeNextBusinessUnits = new Queue<int?>();
            fakeNextBusinessUnits.Enqueue(irmaPushEntry.BusinessUnit_ID);
            fakeNextBusinessUnits.Enqueue(null);

            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale> 
            { 
                new TestItemLocaleMessageBuilder().WithItemId(123).WithIrmaPushId(irmaPushEntry.IRMAPushID),
                new TestItemLocaleMessageBuilder().WithItemId(456).WithIrmaPushId(irmaPushEntry.IRMAPushID)
            };

            context.MessageQueueItemLocale.AddRange(fakeMessageQueueItemLocales);
            context.SaveChanges();

            var fakeEmptyMessageQueueItemLocales = new List<MessageQueueItemLocale>();

            var fakeMessageQueue = new Queue<List<MessageQueueItemLocale>>();
            fakeMessageQueue.Enqueue(fakeMessageQueueItemLocales);
            fakeMessageQueue.Enqueue(fakeEmptyMessageQueueItemLocales);

            ControllerType.Instance = 1;

            var mockQueueProcessorLogger = new Mock<ILogger<ItemLocaleQueueProcessor>>();
            var mockContext = new Mock<IRenewableContext<IconContext>>();
            var mockSerializerLogger = new Mock<ILogger<Serializer<Contracts.items>>>();
            var mockUpdateMessageQueueStatusLogger = new Mock<ILogger<UpdateMessageQueueStatusCommandHandler<MessageQueueItemLocale>>>();
            var mockQueueReaderLogger = new Mock<ILogger<ItemLocaleQueueReader>>();
            var mockEmailClient = new Mock<IEmailClient>();
            var mockAssociateMessageToQueueLogger = new Mock<ILogger<AssociateMessageToQueueCommandHandler<MessageQueueItemLocale>>>();
            var mockUpdateProcessedDateLogger = new Mock<ILogger<UpdateMessageQueueProcessedDateCommandHandler<MessageQueueItemLocale>>>();
            var mockSaveToMessageHistoryLogger = new Mock<ILogger<SaveToMessageHistoryCommandHandler>>();
            var mockUpdateMessageHistoryLogger = new Mock<ILogger<UpdateMessageHistoryStatusCommandHandler>>();
            var mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueItemLocale>, List<MessageQueueItemLocale>>>();
            var mockGetItemByScanCodeQuery = new Mock<IQueryHandler<GetItemByScanCodeParameters, Item>>();
            var mockMarkQueuedEntriesCommandHandler = new Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>>>();
            var mockProductSelectionGroupsMapper = new Mock<IProductSelectionGroupsMapper>();
            var mockGetNextAvailableBusinessUnitQueryHandler = new Mock<IQueryHandler<GetNextAvailableBusinessUnitParameters, int?>>();

            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            mockGetMessageQueueQuery.Setup(q => q.Search(It.IsAny<GetMessageQueueParameters<MessageQueueItemLocale>>())).Returns(fakeMessageQueue.Dequeue);
            mockGetNextAvailableBusinessUnitQueryHandler.Setup(q => q.Search(It.IsAny<GetNextAvailableBusinessUnitParameters>())).Returns(fakeNextBusinessUnits.Dequeue);

            var serializer = new Serializer<Contracts.items>(mockSerializerLogger.Object, mockEmailClient.Object);
            var queueReader = new ItemLocaleQueueReader(
                mockQueueReaderLogger.Object,
                mockEmailClient.Object,
                mockGetMessageQueueQuery.Object,
                mockGetItemByScanCodeQuery.Object,
                new UpdateMessageQueueStatusCommandHandler<MessageQueueItemLocale>(mockUpdateMessageQueueStatusLogger.Object, iconContextFactory),
                mockProductSelectionGroupsMapper.Object);
            var saveToMessageHistoryCommandHandler = new SaveToMessageHistoryCommandHandler(mockSaveToMessageHistoryLogger.Object, iconContextFactory);
            var associateMessageToQueueCommandHandler = new AssociateMessageToQueueCommandHandler<MessageQueueItemLocale>(mockAssociateMessageToQueueLogger.Object, iconContextFactory);
            var updateProcessedDateCommandHandler = new UpdateMessageQueueProcessedDateCommandHandler<MessageQueueItemLocale>(mockUpdateProcessedDateLogger.Object, iconContextFactory);
            var updateMessageHistoryCommandHandler = new UpdateMessageHistoryStatusCommandHandler(mockUpdateMessageHistoryLogger.Object, iconContextFactory);
            var updateMessageQueueStatusCommandHandler = new UpdateMessageQueueStatusCommandHandler<MessageQueueItemLocale>(mockUpdateMessageQueueStatusLogger.Object, iconContextFactory);
            var updateInProcessBusinessUnitCommandHandler = new UpdateInProcessBusinessUnitCommandHandler(iconContextFactory);
            var clearBusinessUnitInProcessCommandHandler = new ClearBusinessUnitInProcessCommandHandler(iconContextFactory);
            var mockMonitor = new Mock<IMessageProcessorMonitor>();

            var itemLocaleQueueProcessor = new ItemLocaleQueueProcessor(
                settings,
                mockQueueProcessorLogger.Object,
                queueReader,
                serializer,
                saveToMessageHistoryCommandHandler,
                associateMessageToQueueCommandHandler,
                updateProcessedDateCommandHandler,
                updateMessageHistoryCommandHandler,
                updateMessageQueueStatusCommandHandler,
                updateInProcessBusinessUnitCommandHandler,
                clearBusinessUnitInProcessCommandHandler,
                mockGetNextAvailableBusinessUnitQueryHandler.Object,
                mockMarkQueuedEntriesCommandHandler.Object,
                mockProducer.Object,
                mockMonitor.Object);

            // When.
            itemLocaleQueueProcessor.ProcessMessageQueue();

            // Then.
            var queuedMessages = context.MessageQueueItemLocale.Where(mq => mq.IRMAPushID == irmaPushEntry.IRMAPushID).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var message in queuedMessages)
            {
                context.Entry(message).Reload();
            }

            int savedMessageId = queuedMessages.First().MessageHistoryId.Value;
            var savedMessage = context.MessageHistory.Single(mh => savedMessageId == mh.MessageHistoryId);

            bool allQueuedMessagesAreUpdated = queuedMessages.TrueForAll(m =>
                !m.InProcessBy.HasValue &&
                m.ProcessedDate.Value.Date == DateTime.Today.Date &&
                m.MessageStatusId == MessageStatusTypes.Associated &&
                m.MessageHistoryId == savedMessage.MessageHistoryId);

            Assert.IsTrue(allQueuedMessagesAreUpdated);
            Assert.AreEqual(MessageStatusTypes.Sent, savedMessage.MessageStatusId);
        }

        [TestMethod]
        public void ItemLocaleQueueProcessor_ItemCannotBeAddedToMiniBulk_ItemShouldBeMarkedAsFailedAndNotAssociatedToAnyMessage()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale> 
            { 
                new TestItemLocaleMessageBuilder().WithItemId(123).WithLinkedItem("123"),
                new TestItemLocaleMessageBuilder().WithItemId(456).WithLinkedItem("123")
            };

            var fakeEmptyMessageQueueItemLocales = new List<MessageQueueItemLocale>();

            var fakeMessageQueue = new Queue<List<MessageQueueItemLocale>>();
            fakeMessageQueue.Enqueue(fakeMessageQueueItemLocales);
            fakeMessageQueue.Enqueue(fakeEmptyMessageQueueItemLocales);

            IRMAPush irmaPushEntry = new TestIrmaPushBuilder();

            context.IRMAPush.Add(irmaPushEntry);
            context.SaveChanges();

            var fakeNextBusinessUnits = new Queue<int?>();
            fakeNextBusinessUnits.Enqueue(irmaPushEntry.BusinessUnit_ID);
            fakeNextBusinessUnits.Enqueue(null);

            fakeMessageQueueItemLocales[0].IRMAPushID = irmaPushEntry.IRMAPushID;
            fakeMessageQueueItemLocales[1].IRMAPushID = irmaPushEntry.IRMAPushID;

            context.MessageQueueItemLocale.AddRange(fakeMessageQueueItemLocales);
            context.SaveChanges();

            ControllerType.Instance = 1;

            var mockQueueProcessorLogger = new Mock<ILogger<ItemLocaleQueueProcessor>>();
            var mockContext = new Mock<IRenewableContext<IconContext>>();
            var mockSerializerLogger = new Mock<ILogger<Serializer<Contracts.items>>>();
            var mockUpdateMessageQueueStatusLogger = new Mock<ILogger<UpdateMessageQueueStatusCommandHandler<MessageQueueItemLocale>>>();
            var mockQueueReaderLogger = new Mock<ILogger<ItemLocaleQueueReader>>();
            var mockEmailClient = new Mock<IEmailClient>();
            var mockAssociateMessageToQueueLogger = new Mock<ILogger<AssociateMessageToQueueCommandHandler<MessageQueueItemLocale>>>();
            var mockUpdateProcessedDateLogger = new Mock<ILogger<UpdateMessageQueueProcessedDateCommandHandler<MessageQueueItemLocale>>>();
            var mockSaveToMessageHistoryLogger = new Mock<ILogger<SaveToMessageHistoryCommandHandler>>();
            var mockUpdateMessageHistoryLogger = new Mock<ILogger<UpdateMessageHistoryStatusCommandHandler>>();
            var mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueItemLocale>, List<MessageQueueItemLocale>>>();
            var mockGetItemByScanCodeQuery = new Mock<IQueryHandler<GetItemByScanCodeParameters, Item>>();
            var mockMarkQueuedEntriesCommandHandler = new Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>>>();
            var mockProductSelectionGroupsMapper = new Mock<IProductSelectionGroupsMapper>();
            var mockGetNextAvailableBusinessUnitQueryHandler = new Mock<IQueryHandler<GetNextAvailableBusinessUnitParameters, int?>>();
            var mockMonitor = new Mock<IMessageProcessorMonitor>();

            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            mockGetMessageQueueQuery.Setup(q => q.Search(It.IsAny<GetMessageQueueParameters<MessageQueueItemLocale>>())).Returns(fakeMessageQueue.Dequeue);
            mockGetItemByScanCodeQuery.Setup(q => q.Search(It.IsAny<GetItemByScanCodeParameters>())).Throws(new Exception());
            mockGetNextAvailableBusinessUnitQueryHandler.Setup(q => q.Search(It.IsAny<GetNextAvailableBusinessUnitParameters>())).Returns(fakeNextBusinessUnits.Dequeue);
            
            var serializer = new Serializer<Contracts.items>(mockSerializerLogger.Object, mockEmailClient.Object);
            var queueReader = new ItemLocaleQueueReader(
                mockQueueReaderLogger.Object,
                mockEmailClient.Object,
                mockGetMessageQueueQuery.Object,
                mockGetItemByScanCodeQuery.Object,
                new UpdateMessageQueueStatusCommandHandler<MessageQueueItemLocale>(mockUpdateMessageQueueStatusLogger.Object, iconContextFactory),
                mockProductSelectionGroupsMapper.Object);
            var saveToMessageHistoryCommandHandler = new SaveToMessageHistoryCommandHandler(mockSaveToMessageHistoryLogger.Object, iconContextFactory);
            var associateMessageToMessageQueueCommandHandler = new AssociateMessageToQueueCommandHandler<MessageQueueItemLocale>(mockAssociateMessageToQueueLogger.Object, iconContextFactory);
            var updateProcessedDateCommandHandler = new UpdateMessageQueueProcessedDateCommandHandler<MessageQueueItemLocale>(mockUpdateProcessedDateLogger.Object, iconContextFactory);
            var updateMessageHistoryCommandHandler = new UpdateMessageHistoryStatusCommandHandler(mockUpdateMessageHistoryLogger.Object, iconContextFactory);
            var updateMessageQueueStatusCommandHandler = new UpdateMessageQueueStatusCommandHandler<MessageQueueItemLocale>(mockUpdateMessageQueueStatusLogger.Object, iconContextFactory);
            var updateInProcessBusinessUnitCommandHandler = new UpdateInProcessBusinessUnitCommandHandler(iconContextFactory);
            var clearBusinessUnitInProcessCommandHandler = new ClearBusinessUnitInProcessCommandHandler(iconContextFactory);
            
            var itemLocaleQueueProcessor = new ItemLocaleQueueProcessor(
                settings,
                mockQueueProcessorLogger.Object,
                queueReader,
                serializer,
                saveToMessageHistoryCommandHandler,
                associateMessageToMessageQueueCommandHandler,
                updateProcessedDateCommandHandler,
                updateMessageHistoryCommandHandler,
                updateMessageQueueStatusCommandHandler,
                updateInProcessBusinessUnitCommandHandler,
                clearBusinessUnitInProcessCommandHandler,
                mockGetNextAvailableBusinessUnitQueryHandler.Object,
                mockMarkQueuedEntriesCommandHandler.Object,
                mockProducer.Object,
                mockMonitor.Object);

            // When.
            itemLocaleQueueProcessor.ProcessMessageQueue();

            // Then.
            var originalQueuedMessageId = fakeMessageQueueItemLocales.Select(p => p.MessageQueueId).ToList();
            var queuedMessages = context.MessageQueueItemLocale.Where(mq => originalQueuedMessageId.Contains(mq.MessageQueueId)).ToList();

            Assert.AreEqual(MessageStatusTypes.Failed, queuedMessages[0].MessageStatusId);
            Assert.AreEqual(MessageStatusTypes.Failed, queuedMessages[1].MessageStatusId);
            Assert.IsNull(queuedMessages[0].MessageHistoryId);
            Assert.IsNull(queuedMessages[1].MessageHistoryId);
        }

        [TestMethod]
        public void PriceQueueProcessor_ValidPriceRecords_ShouldBeProcessedSuccessfully()
        {
            // Given.
            IRMAPush irmaPushEntry = new TestIrmaPushBuilder();

            context.IRMAPush.Add(irmaPushEntry);
            context.SaveChanges();

            var fakeNextBusinessUnits = new Queue<int?>();
            fakeNextBusinessUnits.Enqueue(irmaPushEntry.BusinessUnit_ID);
            fakeNextBusinessUnits.Enqueue(null);

            var fakeMessageQueuePrices = new List<MessageQueuePrice> 
            { 
                new TestPriceMessageBuilder().WithItemId(123).WithIrmaPushId(irmaPushEntry.IRMAPushID),
                new TestPriceMessageBuilder().WithItemId(456).WithIrmaPushId(irmaPushEntry.IRMAPushID)
            };

            var fakeEmptyMessageQueuePrices = new List<MessageQueuePrice>();

            context.MessageQueuePrice.AddRange(fakeMessageQueuePrices);
            context.SaveChanges();

            var fakeMessageQueue = new Queue<List<MessageQueuePrice>>();
            fakeMessageQueue.Enqueue(fakeMessageQueuePrices);
            fakeMessageQueue.Enqueue(fakeEmptyMessageQueuePrices);

            ControllerType.Instance = 1;

            var mockQueueProcessorLogger = new Mock<ILogger<PriceQueueProcessor>>();
            var mockContext = new Mock<IRenewableContext<IconContext>>();
            var mockSerializerLogger = new Mock<ILogger<Serializer<Contracts.items>>>();
            var mockUpdateMessageQueueStatusLogger = new Mock<ILogger<UpdateMessageQueueStatusCommandHandler<MessageQueuePrice>>>();
            var mockQueueReaderLogger = new Mock<ILogger<PriceQueueReader>>();
            var mockEmailClient = new Mock<IEmailClient>();
            var mockAssociateMessageToQueueLogger = new Mock<ILogger<AssociateMessageToQueueCommandHandler<MessageQueuePrice>>>();
            var mockSetProcessedDateLogger = new Mock<ILogger<UpdateMessageQueueProcessedDateCommandHandler<MessageQueuePrice>>>();
            var mockSaveToMessageHistoryLogger = new Mock<ILogger<SaveToMessageHistoryCommandHandler>>();
            var mockUpdateMessageHistoryLogger = new Mock<ILogger<UpdateMessageHistoryStatusCommandHandler>>();
            var mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueuePrice>, List<MessageQueuePrice>>>();
            var mockMarkMessagesAsInProcessCommand = new Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueuePrice>>>();
            var mockGetNextAvailableBusinessUnitQueryHandler = new Mock<IQueryHandler<GetNextAvailableBusinessUnitParameters, int?>>();
            mockGetNextAvailableBusinessUnitQueryHandler.Setup(q => q.Search(It.IsAny<GetNextAvailableBusinessUnitParameters>())).Returns(fakeNextBusinessUnits.Dequeue);

            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            mockGetMessageQueueQuery.Setup(q => q.Search(It.IsAny<GetMessageQueueParameters<MessageQueuePrice>>())).Returns(fakeMessageQueue.Dequeue);

            var serializer = new Serializer<Contracts.items>(mockSerializerLogger.Object, mockEmailClient.Object);
            var queueReader = new PriceQueueReader(
                mockQueueReaderLogger.Object,
                mockEmailClient.Object,
                mockGetMessageQueueQuery.Object,
                new UpdateMessageQueueStatusCommandHandler<MessageQueuePrice>(mockUpdateMessageQueueStatusLogger.Object, iconContextFactory));
            var saveToMessageHistoryCommandHandler = new SaveToMessageHistoryCommandHandler(mockSaveToMessageHistoryLogger.Object, iconContextFactory);
            var associateMessageToMessageQueueCommandHandler = new AssociateMessageToQueueCommandHandler<MessageQueuePrice>(mockAssociateMessageToQueueLogger.Object, iconContextFactory);
            var setProcessedDateCommandHandler = new UpdateMessageQueueProcessedDateCommandHandler<MessageQueuePrice>(mockSetProcessedDateLogger.Object, iconContextFactory);
            var updateMessageHistoryCommandHandler = new UpdateMessageHistoryStatusCommandHandler(mockUpdateMessageHistoryLogger.Object, iconContextFactory);
            var updateMessageQueueStatusCommandHandler = new UpdateMessageQueueStatusCommandHandler<MessageQueuePrice>(mockUpdateMessageQueueStatusLogger.Object, iconContextFactory);
            var updateInProcessBusinessUnitCommandHandler = new UpdateInProcessBusinessUnitCommandHandler(iconContextFactory);
            var clearBusinessUnitInProcessCommandHandler = new ClearBusinessUnitInProcessCommandHandler(iconContextFactory);
            var mockMonitor = new Mock<IMessageProcessorMonitor>();

            var productQueueProcessor = new PriceQueueProcessor(
                settings,
                mockQueueProcessorLogger.Object,
                queueReader,
                serializer,
                saveToMessageHistoryCommandHandler,
                associateMessageToMessageQueueCommandHandler,
                setProcessedDateCommandHandler,
                updateMessageHistoryCommandHandler,
                updateMessageQueueStatusCommandHandler,
                updateInProcessBusinessUnitCommandHandler,
                clearBusinessUnitInProcessCommandHandler,
                mockGetNextAvailableBusinessUnitQueryHandler.Object,
                mockMarkMessagesAsInProcessCommand.Object,
                mockProducer.Object,
                mockMonitor.Object);

            // When.
            productQueueProcessor.ProcessMessageQueue();

            // Then.
            var queuedMessages = context.MessageQueuePrice.Where(mq => mq.IRMAPushID == irmaPushEntry.IRMAPushID).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var message in queuedMessages)
            {
                context.Entry(message).Reload();
            }

            int savedMessageId = queuedMessages.First().MessageHistoryId.Value;
            var savedMessage = context.MessageHistory.Single(mh => savedMessageId == mh.MessageHistoryId);

            bool allQueuedMessagesAreUpdated = queuedMessages.TrueForAll(m =>
                !m.InProcessBy.HasValue &&
                m.ProcessedDate.Value.Date == DateTime.Today.Date &&
                m.MessageStatusId == MessageStatusTypes.Associated &&
                m.MessageHistoryId == savedMessage.MessageHistoryId);

            Assert.IsTrue(allQueuedMessagesAreUpdated);
            Assert.AreEqual(MessageStatusTypes.Sent, savedMessage.MessageStatusId);
        }

        [TestMethod]
        public void LocaleQueueProcessor_ValidLocaleRecords_ShouldBeProcessedSuccessfully()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale> 
            { 
                TestHelpers.GetFakeMessageQueueLocale()
            };

            var fakeEmptyMessageQueueLocales = new List<MessageQueueLocale>();

            var fakeMessageQueue = new Queue<List<MessageQueueLocale>>();
            fakeMessageQueue.Enqueue(fakeMessageQueueLocales);
            fakeMessageQueue.Enqueue(fakeEmptyMessageQueueLocales);

            context.MessageQueueLocale.AddRange(fakeMessageQueueLocales);
            context.SaveChanges();

            ControllerType.Instance = 88;

            var mockQueueProcessorLogger = new Mock<ILogger<LocaleQueueProcessor>>();
            var mockContext = new Mock<IRenewableContext<IconContext>>();
            var mockSerializerLogger = new Mock<ILogger<Serializer<Contracts.LocaleType>>>();
            var mockUpdateMessageQueueStatusLogger = new Mock<ILogger<UpdateMessageQueueStatusCommandHandler<MessageQueueLocale>>>();
            var mockQueueReaderLogger = new Mock<ILogger<LocaleQueueReader>>();
            var mockEmailClient = new Mock<IEmailClient>();
            var mockAssociateMessageToQueueLogger = new Mock<ILogger<AssociateMessageToQueueCommandHandler<MessageQueueLocale>>>();
            var mockMarkQueuedEntriesAsInProcessLogger = new Mock<ILogger<MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueLocale>>>();
            var mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueLocale>, List<MessageQueueLocale>>>();
            var mockSaveToMessageHistoryLogger = new Mock<ILogger<SaveToMessageHistoryCommandHandler>>();
            var mockSetProcessedDateLogger = new Mock<ILogger<UpdateMessageQueueProcessedDateCommandHandler<MessageQueueLocale>>>();
            var mockUpdateMessageHistoryStatusLogger = new Mock<ILogger<UpdateMessageHistoryStatusCommandHandler>>();
            var mockGetLocaleLineageQuery = new Mock<IQueryHandler<GetLocaleLineageParameters, LocaleLineageModel>>();

            var localeLineage = new List<LocaleLineageModel>
            {
                new LocaleLineageModel
                {
                    LocaleId = 3,
                    LocaleName = "SW",
                    DescendantLocales = new List<LocaleLineageModel>()
                }
            };

            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            mockGetMessageQueueQuery.Setup(q => q.Search(It.IsAny<GetMessageQueueParameters<MessageQueueLocale>>())).Returns(fakeMessageQueue.Dequeue);
            mockGetLocaleLineageQuery.Setup(q => q.Search(It.IsAny<GetLocaleLineageParameters>())).Returns(new LocaleLineageModel { DescendantLocales = localeLineage });

            var serializer = new Serializer<Contracts.LocaleType>(mockSerializerLogger.Object, mockEmailClient.Object);
            var queueReader = new LocaleQueueReader(
                mockQueueReaderLogger.Object,
                mockEmailClient.Object,
                mockGetMessageQueueQuery.Object,
                mockGetLocaleLineageQuery.Object,
                new UpdateMessageQueueStatusCommandHandler<MessageQueueLocale>(mockUpdateMessageQueueStatusLogger.Object, iconContextFactory));
            var saveXmlMessageCommandHandler = new SaveToMessageHistoryCommandHandler(mockSaveToMessageHistoryLogger.Object, iconContextFactory);
            var associateMessageToMessageQueueCommandHandler = new AssociateMessageToQueueCommandHandler<MessageQueueLocale>(mockAssociateMessageToQueueLogger.Object, iconContextFactory);
            var setProcessedDateCommandHandler = new UpdateMessageQueueProcessedDateCommandHandler<MessageQueueLocale>(mockSetProcessedDateLogger.Object, iconContextFactory);
            var updateMessageHistoryCommandHandler = new UpdateMessageHistoryStatusCommandHandler(mockUpdateMessageHistoryStatusLogger.Object, iconContextFactory);
            var updateMessageQueueStatusCommandHandler = new UpdateMessageQueueStatusCommandHandler<MessageQueueLocale>(mockUpdateMessageQueueStatusLogger.Object, iconContextFactory);
            var mockMonitor = new Mock<IMessageProcessorMonitor>();

            var localeQueueProcessor = new LocaleQueueProcessor(
                settings,
                mockQueueProcessorLogger.Object,
                queueReader,
                serializer,
                saveXmlMessageCommandHandler,
                associateMessageToMessageQueueCommandHandler,
                setProcessedDateCommandHandler,
                updateMessageHistoryCommandHandler,
                updateMessageQueueStatusCommandHandler,
                new MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueLocale>(mockMarkQueuedEntriesAsInProcessLogger.Object, iconContextFactory),
                mockProducer.Object,
                mockMonitor.Object);

            // When.
            localeQueueProcessor.ProcessMessageQueue();

            // Then.
            var originalQueuedMessageId = fakeMessageQueueLocales[0].MessageQueueId;
            var queuedMessage = context.MessageQueueLocale.Single(mq => mq.MessageQueueId == originalQueuedMessageId);

            // Have to reload the entity since the update was done via stored procedure.
            context.Entry(queuedMessage).Reload();

            int savedMessageId = queuedMessage.MessageHistoryId.Value;
            var savedMessage = context.MessageHistory.Single(mh => savedMessageId == mh.MessageHistoryId);

            bool queuedMessageIsUpdated =
                (!queuedMessage.InProcessBy.HasValue) &&
                (queuedMessage.ProcessedDate.Value.Date == DateTime.Today) &&
                (queuedMessage.MessageStatusId == MessageStatusTypes.Associated) &&
                (queuedMessage.MessageHistoryId != null);

            Assert.IsTrue(queuedMessageIsUpdated);
            Assert.AreEqual(MessageStatusTypes.Sent, savedMessage.MessageStatusId);
        }

        [TestMethod]
        public void HierarchyQueueProcessor_ValidHierarchyRecords_ShouldBeProcessedSuccessfully()
        {
            // Given.
            var fakeMessageQueueHierarchies = new List<MessageQueueHierarchy> 
            { 
                TestHelpers.GetFakeMessageQueueHierarchy(2, "Segment", true),
                TestHelpers.GetFakeMessageQueueHierarchy(2, "Segment", true)
            };

            var fakeEmptyMessageQueueHierarchies = new List<MessageQueueHierarchy>();

            var fakeMessageQueue = new Queue<List<MessageQueueHierarchy>>();
            fakeMessageQueue.Enqueue(fakeMessageQueueHierarchies);
            fakeMessageQueue.Enqueue(fakeEmptyMessageQueueHierarchies);

            context.MessageQueueHierarchy.AddRange(fakeMessageQueueHierarchies);
            context.SaveChanges();

            ControllerType.Instance = 1;

            var mockQueueProcessorLogger = new Mock<ILogger<HierarchyQueueProcessor>>();
            var mockContext = new Mock<IRenewableContext<IconContext>>();
            var mockSerializerLogger = new Mock<ILogger<Serializer<Contracts.HierarchyType>>>();
            var mockUpdateMessageQueueStatusLogger = new Mock<ILogger<UpdateMessageQueueStatusCommandHandler<MessageQueueHierarchy>>>();
            var mockSaveToMessageHistoryLogger = new Mock<ILogger<SaveToMessageHistoryCommandHandler>>();
            var mockQueueReaderLogger = new Mock<ILogger<HierarchyQueueReader>>();
            var mockEmailClient = new Mock<IEmailClient>();
            var mockAssociateMessageToQueueLogger = new Mock<ILogger<AssociateMessageToQueueCommandHandler<MessageQueueHierarchy>>>();
            var mockMarkQueuedEntriesAsInProcess = new Mock<ILogger<MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueHierarchy>>>();
            var mockSetProcessedDateLogger = new Mock<ILogger<UpdateMessageQueueProcessedDateCommandHandler<MessageQueueHierarchy>>>();
            var mockUpdateMessageHistoryStatusLogger = new Mock<ILogger<UpdateMessageHistoryStatusCommandHandler>>();
            var mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueHierarchy>, List<MessageQueueHierarchy>>>();

            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            mockGetMessageQueueQuery.Setup(q => q.Search(It.IsAny<GetMessageQueueParameters<MessageQueueHierarchy>>())).Returns(fakeMessageQueue.Dequeue);

            var queueReader = new HierarchyQueueReader(
                mockQueueReaderLogger.Object,
                mockEmailClient.Object,
                mockGetMessageQueueQuery.Object,
                new UpdateMessageQueueStatusCommandHandler<MessageQueueHierarchy>(mockUpdateMessageQueueStatusLogger.Object, iconContextFactory));
            var serializer = new Serializer<Contracts.HierarchyType>(mockSerializerLogger.Object, mockEmailClient.Object);
            var getFinancialClassesQueryHandler = new GetFinancialHierarchyClassesQuery(iconContextFactory);
            var saveXmlMessageCommandHandler = new SaveToMessageHistoryCommandHandler(mockSaveToMessageHistoryLogger.Object, iconContextFactory);
            var associateMessageToMessageQueueCommandHandler = new AssociateMessageToQueueCommandHandler<MessageQueueHierarchy>(mockAssociateMessageToQueueLogger.Object, iconContextFactory);
            var setProcessedDateCommandHandler = new UpdateMessageQueueProcessedDateCommandHandler<MessageQueueHierarchy>(mockSetProcessedDateLogger.Object, iconContextFactory);
            var updateMessageHistoryCommandHandler = new UpdateMessageHistoryStatusCommandHandler(mockUpdateMessageHistoryStatusLogger.Object, iconContextFactory);
            var updateMessageQueueStatusCommandHandler = new UpdateMessageQueueStatusCommandHandler<MessageQueueHierarchy>(mockUpdateMessageQueueStatusLogger.Object, iconContextFactory);
            var mockUpdateStagedProductStatusCommandHandler = new Mock<ICommandHandler<UpdateStagedProductStatusCommand>>();
            var mockUpdateSentToEsbHierarchyTraitCommandHandler = new Mock<ICommandHandler<UpdateSentToEsbHierarchyTraitCommand>>();
            var mockMonitor = new Mock<IMessageProcessorMonitor>();

            var hierarchyQueueProcessor = new HierarchyQueueProcessor(
                settings,
                mockQueueProcessorLogger.Object,
                queueReader,
                serializer,
                getFinancialClassesQueryHandler,
                saveXmlMessageCommandHandler,
                associateMessageToMessageQueueCommandHandler,
                setProcessedDateCommandHandler,
                updateMessageHistoryCommandHandler,
                updateMessageQueueStatusCommandHandler,
                mockUpdateStagedProductStatusCommandHandler.Object,
                mockUpdateSentToEsbHierarchyTraitCommandHandler.Object,
                new MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueHierarchy>(mockMarkQueuedEntriesAsInProcess.Object, iconContextFactory),
                mockProducer.Object,
                mockMonitor.Object);

            // When.
            hierarchyQueueProcessor.ProcessMessageQueue();

            // Then.
            var originalQueuedMessageId = fakeMessageQueueHierarchies.Select(p => p.MessageQueueId).ToList();
            var queuedMessages = context.MessageQueueHierarchy.Where(mq => originalQueuedMessageId.Contains(mq.MessageQueueId)).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var message in queuedMessages)
            {
                context.Entry(message).Reload();
            }

            int savedMessageId = queuedMessages.First().MessageHistoryId.Value;
            var savedMessage = context.MessageHistory.Single(mh => savedMessageId == mh.MessageHistoryId);

            bool allQueuedMessagesAreUpdated = queuedMessages.TrueForAll(m =>
                !m.InProcessBy.HasValue &&
                m.ProcessedDate.Value.Date == DateTime.Today &&
                m.MessageStatusId == MessageStatusTypes.Associated &&
                m.MessageHistoryId != null);

            Assert.IsTrue(allQueuedMessagesAreUpdated);
            Assert.AreEqual(MessageStatusTypes.Sent, savedMessage.MessageStatusId);
        }

        [TestMethod]
        public void ProductSelectionGroupProcessor_ValidPsgRecords_ShouldBeProcessedSuccessfully()
        {
            // Given.
            var fakeMessageQueuePsgs = new List<MessageQueueProductSelectionGroup> 
            { 
                TestHelpers.GetFakeMessageQueueProductSelectionGroup(1),
                TestHelpers.GetFakeMessageQueueProductSelectionGroup(2)
            };

            var fakeEmptyMessageQueuePsgs = new List<MessageQueueProductSelectionGroup>();

            var fakeMessageQueue = new Queue<List<MessageQueueProductSelectionGroup>>();
            fakeMessageQueue.Enqueue(fakeMessageQueuePsgs);
            fakeMessageQueue.Enqueue(fakeEmptyMessageQueuePsgs);

            context.MessageQueueProductSelectionGroup.AddRange(fakeMessageQueuePsgs);
            context.SaveChanges();

            ControllerType.Instance = 1;

            var mockQueueProcessorLogger = new Mock<ILogger<ProductSelectionGroupQueueProcessor>>();
            var mockContext = new Mock<IRenewableContext<IconContext>>();
            var mockSerializerLogger = new Mock<ILogger<Serializer<Contracts.SelectionGroupsType>>>();
            var mockUpdateMessageQueueStatusLogger = new Mock<ILogger<UpdateMessageQueueStatusCommandHandler<MessageQueueProductSelectionGroup>>>();
            var mockQueueReaderLogger = new Mock<ILogger<ProductSelectionGroupQueueReader>>();
            var mockEmailClient = new Mock<IEmailClient>();
            var mockAssociateMessageToQueueLogger = new Mock<ILogger<AssociateMessageToQueueCommandHandler<MessageQueueProductSelectionGroup>>>();
            var mockSaveToMessageHistoryLogger = new Mock<ILogger<SaveToMessageHistoryCommandHandler>>();
            var mockSetProcessedDateLogger = new Mock<ILogger<UpdateMessageQueueProcessedDateCommandHandler<MessageQueueProductSelectionGroup>>>();
            var mockUpdateMessageHistoryLogger = new Mock<ILogger<UpdateMessageHistoryStatusCommandHandler>>();

            var mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueProductSelectionGroup>, List<MessageQueueProductSelectionGroup>>>();
            var mockMarkMessagesAsInProcessCommand = new Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueProductSelectionGroup>>>();

            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            mockGetMessageQueueQuery.Setup(q => q.Search(It.IsAny<GetMessageQueueParameters<MessageQueueProductSelectionGroup>>())).Returns(fakeMessageQueue.Dequeue);

            var serializer = new Serializer<Contracts.SelectionGroupsType>(mockSerializerLogger.Object, mockEmailClient.Object);
            var queueReader = new ProductSelectionGroupQueueReader(
                mockQueueReaderLogger.Object,
                mockGetMessageQueueQuery.Object,
                new UpdateMessageQueueStatusCommandHandler<MessageQueueProductSelectionGroup>(mockUpdateMessageQueueStatusLogger.Object, iconContextFactory));
            var saveXmlMessageCommandHandler = new SaveToMessageHistoryCommandHandler(mockSaveToMessageHistoryLogger.Object, iconContextFactory);
            var associateMessageToMessageQueueCommandHandler = new AssociateMessageToQueueCommandHandler<MessageQueueProductSelectionGroup>(mockAssociateMessageToQueueLogger.Object, iconContextFactory);
            var setProcessedDateCommandHandler = new UpdateMessageQueueProcessedDateCommandHandler<MessageQueueProductSelectionGroup>(mockSetProcessedDateLogger.Object, iconContextFactory);
            var updateMessageHistoryCommandHandler = new UpdateMessageHistoryStatusCommandHandler(mockUpdateMessageHistoryLogger.Object, iconContextFactory);
            var updateMessageQueueStatusCommandHandler = new UpdateMessageQueueStatusCommandHandler<MessageQueueProductSelectionGroup>(mockUpdateMessageQueueStatusLogger.Object, iconContextFactory);
            var mockMonitor = new Mock<IMessageProcessorMonitor>();

            var queueProcessor = new ProductSelectionGroupQueueProcessor(
                settings,
                mockQueueProcessorLogger.Object,
                queueReader,
                serializer,
                saveXmlMessageCommandHandler,
                associateMessageToMessageQueueCommandHandler,
                setProcessedDateCommandHandler,
                updateMessageHistoryCommandHandler,
                updateMessageQueueStatusCommandHandler,
                mockMarkMessagesAsInProcessCommand.Object,
                mockProducer.Object,
                mockMonitor.Object);

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            var originalQueuedMessageId = fakeMessageQueuePsgs.Select(p => p.MessageQueueId).ToList();
            var queuedMessages = context.MessageQueueProductSelectionGroup.Where(mq => originalQueuedMessageId.Contains(mq.MessageQueueId)).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var message in queuedMessages)
            {
                context.Entry(message).Reload();
            }

            int savedMessageId = queuedMessages.First().MessageHistoryId.Value;
            var savedMessage = context.MessageHistory.Single(mh => savedMessageId == mh.MessageHistoryId);

            bool allQueuedMessagesAreUpdated = queuedMessages.TrueForAll(m =>
                !m.InProcessBy.HasValue &&
                m.ProcessedDate.Value.Date == DateTime.Today &&
                m.MessageStatusId == MessageStatusTypes.Associated &&
                m.MessageHistoryId != null);

            Assert.IsTrue(allQueuedMessagesAreUpdated);
            Assert.AreEqual(MessageStatusTypes.Sent, savedMessage.MessageStatusId);
        }
    }
}
