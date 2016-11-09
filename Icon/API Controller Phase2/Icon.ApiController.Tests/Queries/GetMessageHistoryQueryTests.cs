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

namespace Icon.ApiController.Tests.Queries
{
    [TestClass]
    public class GetMessageHistoryQueryTests
    {
        private GetMessageHistoryQuery getMessageHistoryQuery;
        private IconContext context;
        private GlobalIconContext globalContext;
        private DbContextTransaction transaction;
        private Mock<ILogger<GetMessageHistoryQuery>> mockLogger;
        private List<MessageHistory> testMessages;
        
        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);

            mockLogger = new Mock<ILogger<GetMessageHistoryQuery>>();
            getMessageHistoryQuery = new GetMessageHistoryQuery(mockLogger.Object, globalContext);
            ControllerType.Instance = 99;

            transaction = context.Database.BeginTransaction();

            testMessages = new List<MessageHistory>
            {
                new TestMessageHistoryBuilder().WithInProcessBy(null),
                new TestMessageHistoryBuilder().WithInProcessBy(ControllerType.Instance)
            };

            context.MessageHistory.AddRange(testMessages);
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void GetMessageHistoryQuery_MessageIsMarkedByController_MessageShouldBeReturned()
        {
            // Given.
            var parameters = new GetMessageHistoryParameters
            {
                Instance = ControllerType.Instance,
                MessageTypeId = MessageTypes.Product
            };

            // When.
            var messages = getMessageHistoryQuery.Search(parameters);

            // Then.
            Assert.AreEqual(1, messages.Count);
        }

        [TestMethod]
        public void GetMessageHistoryQuery_MessageIsNotMarkedByController_MessageShouldNotBeReturned()
        {
            // Given.
            var parameters = new GetMessageHistoryParameters
            {
                Instance = 2,
                MessageTypeId = MessageTypes.Product
            };

            // When.
            var messages = getMessageHistoryQuery.Search(parameters);

            // Then.
            Assert.AreEqual(0, messages.Count);
        }
    }
}
