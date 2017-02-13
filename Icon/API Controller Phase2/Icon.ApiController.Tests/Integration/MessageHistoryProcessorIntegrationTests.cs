using Icon.ApiController.Common;
using Icon.ApiController.Controller.HistoryProcessors;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Esb.Producer;
using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;

namespace Icon.ApiController.Tests.Integration
{
    [TestClass]
    public class MessageHistoryProcessorIntegrationTests
    {
        private MessageHistoryProcessor historyProcessor;
        private IconContext context;
        private TransactionScope transaction;
        private Mock<ILogger<MessageHistoryProcessor>> mockLogger;
        private Mock<ICommandHandler<MarkUnsentMessagesAsInProcessCommand>> mockMarkUnsentMessagesAsInProcessCommandHandler;
        private Mock<IQueryHandler<GetMessageHistoryParameters, List<MessageHistory>>> mockGetMessageHistoryQuery;
        private UpdateMessageHistoryStatusCommandHandler updateMessageHistoryCommandHandler;
        private Mock<ICommandHandler<UpdateStagedProductStatusCommand>> mockUpdateStagedProductCommandHandler;
        private Mock<ICommandHandler<UpdateSentToEsbHierarchyTraitCommand>> mockUpdateSentToEsbHierarchyTraitCommandHandler;
        private Mock<IEsbProducer> mockProducer;
        private ApiControllerSettings settings;
        
        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();

            context = new IconContext();

            mockLogger = new Mock<ILogger<MessageHistoryProcessor>>();
            mockMarkUnsentMessagesAsInProcessCommandHandler = new Mock<ICommandHandler<MarkUnsentMessagesAsInProcessCommand>>();
            mockGetMessageHistoryQuery = new Mock<IQueryHandler<GetMessageHistoryParameters, List<MessageHistory>>>();
            updateMessageHistoryCommandHandler = new UpdateMessageHistoryStatusCommandHandler(new Mock<ILogger<UpdateMessageHistoryStatusCommandHandler>>().Object, new IconDbContextFactory());
            mockUpdateSentToEsbHierarchyTraitCommandHandler = new Mock<ICommandHandler<UpdateSentToEsbHierarchyTraitCommand>>();
            mockUpdateStagedProductCommandHandler = new Mock<ICommandHandler<UpdateStagedProductStatusCommand>>();
            mockProducer = new Mock<IEsbProducer>();
            settings = new ApiControllerSettings();

            this.historyProcessor = new MessageHistoryProcessor(
                settings,
                mockLogger.Object,
                mockMarkUnsentMessagesAsInProcessCommandHandler.Object,
                mockGetMessageHistoryQuery.Object,
                updateMessageHistoryCommandHandler,
                mockUpdateStagedProductCommandHandler.Object,
                mockUpdateSentToEsbHierarchyTraitCommandHandler.Object,
                mockProducer.Object,
                MessageTypes.Product);
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void ProcessMessageHistory_MessageSentToEsb_MessageStatusShouldBeUpdated()
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

            context.MessageHistory.AddRange(fakeMessageHistory);
            context.SaveChanges();

            mockGetMessageHistoryQuery.Setup(q => q.Search(It.IsAny<GetMessageHistoryParameters>())).Returns(fakeMessageHistoryQueue.Dequeue);

            // When.
            this.historyProcessor.ProcessMessageHistory();

            // Then.
            int messageHistoryId = fakeMessageHistory[0].MessageHistoryId;
            var message = context.MessageHistory.Single(mh => mh.MessageHistoryId == messageHistoryId);

            Assert.AreEqual(MessageStatusTypes.Sent, message.MessageStatusId);
        }
    }
}
