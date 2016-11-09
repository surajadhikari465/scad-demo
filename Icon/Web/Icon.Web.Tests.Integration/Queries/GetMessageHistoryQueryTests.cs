using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetMessageHistoryQueryTests
    {
        private GetMessageHistoryQuery query;
        
        private IconContext context;
        private DbContextTransaction transaction;
        private List<int> testMessageHistoriesById;
        private List<MessageHistory> testMessageHistories;
        private string testMessage;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            query = new GetMessageHistoryQuery(this.context);

            testMessage = "Test";

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void GetMessageHistory_NoMatchingHistoryId_NoResultsShouldBeReturned()
        {
            // Given.
            testMessageHistoriesById = new List<int>();

            var parameters = new GetMessageHistoryParameters { MessageHistoriesById = testMessageHistoriesById };

            // When.
            var results = query.Search(parameters);

            // Then.
            Assert.AreEqual(testMessageHistoriesById.Count, results.Count);
        }

        [TestMethod]
        public void GetMessageHistory_OneMatchingHistoryId_OneResultShouldBeReturned()
        {
            // Given.
            testMessageHistories = new List<MessageHistory> { new TestMessageHistoryBuilder().WithMessageTypeId(MessageTypes.Ewic).WithMessage(testMessage) };

            context.MessageHistory.AddRange(testMessageHistories);
            context.SaveChanges();

            testMessageHistoriesById = new List<int> { testMessageHistories[0].MessageHistoryId };

            var parameters = new GetMessageHistoryParameters { MessageHistoriesById = testMessageHistoriesById };

            // When.
            var results = query.Search(parameters);

            // Then.
            Assert.AreEqual(testMessageHistoriesById.Count, results.Count);
            Assert.AreEqual(testMessage, results[0].Message);
        }

        [TestMethod]
        public void GetMessageHistory_TwoMatchingHistoryId_TwoResultsShouldBeReturned()
        {
            // Given.
            testMessageHistories = new List<MessageHistory>
            { 
                new TestMessageHistoryBuilder().WithMessageTypeId(MessageTypes.Ewic).WithMessage(testMessage),
                new TestMessageHistoryBuilder().WithMessageTypeId(MessageTypes.Ewic).WithMessage(testMessage) 
            };

            context.MessageHistory.AddRange(testMessageHistories);
            context.SaveChanges();

            testMessageHistoriesById = new List<int> 
            { 
                testMessageHistories[0].MessageHistoryId,
                testMessageHistories[1].MessageHistoryId
            };

            var parameters = new GetMessageHistoryParameters { MessageHistoriesById = testMessageHistoriesById };

            // When.
            var results = query.Search(parameters);

            // Then.
            Assert.AreEqual(testMessageHistoriesById.Count, results.Count);
            Assert.AreEqual(testMessage, results[0].Message);
            Assert.AreEqual(testMessage, results[1].Message);
        }
    }
}
