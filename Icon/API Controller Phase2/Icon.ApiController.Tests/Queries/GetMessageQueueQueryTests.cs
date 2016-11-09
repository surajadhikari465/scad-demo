using Icon.ApiController.Common;
using Icon.ApiController.DataAccess.Queries;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.ApiController.Tests.Queries
{
    [TestClass]
    public class GetMessageQueueQueryTests
    {
        private GetMessageQueueQuery<MessageQueueItemLocale> getMessageQueueQuery;

        private IconContext context;
        private GlobalIconContext globalContext;
        private DbContextTransaction transaction;
        private Mock<ILogger<GetMessageQueueQuery<MessageQueueItemLocale>>> mockLogger;
        private List<MessageQueueItemLocale> testMessages;
        private IRMAPush testIrmaPush;
        private int testIrmaPushId;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);

            mockLogger = new Mock<ILogger<GetMessageQueueQuery<MessageQueueItemLocale>>>();
            getMessageQueueQuery = new GetMessageQueueQuery<MessageQueueItemLocale>(mockLogger.Object, globalContext);
            ControllerType.Instance = 99;

            transaction = context.Database.BeginTransaction();

            testIrmaPush = new TestIrmaPushBuilder();
            context.IRMAPush.Add(testIrmaPush);
            context.SaveChanges();

            testIrmaPushId = testIrmaPush.IRMAPushID;

            testMessages = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithInProcessBy(null).WithIrmaPushId(testIrmaPushId),
                new TestItemLocaleMessageBuilder().WithInProcessBy(ControllerType.Instance).WithIrmaPushId(testIrmaPushId)
            };

            context.MessageQueueItemLocale.AddRange(testMessages);
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void GetMessageQueueQuery_MessageIsMarkedByController_MessageShouldBeReturned()
        {
            // Given.
            var parameters = new GetMessageQueueParameters<MessageQueueItemLocale>
            {
                Instance = ControllerType.Instance,
                MessageQueueStatusId = MessageStatusTypes.Ready
            };

            // When.
            var messages = getMessageQueueQuery.Search(parameters);

            // Then.
            messages = messages.Where(m => m.IRMAPushID == testIrmaPushId).ToList();

            Assert.AreEqual(1, messages.Count);
        }

        [TestMethod]
        public void GetMessageQueueQuery_MessageIsNotMarkedByController_MessageShouldNotBeReturned()
        {
            // Given.
            var parameters = new GetMessageQueueParameters<MessageQueueItemLocale>
            {
                Instance = 2,
                MessageQueueStatusId = MessageStatusTypes.Ready
            };

            // When.
            var messages = getMessageQueueQuery.Search(parameters);

            // Then.
            messages = messages.Where(m => m.IRMAPushID == testIrmaPushId).ToList();

            Assert.AreEqual(0, messages.Count);
        }
    }
}
