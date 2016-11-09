using Icon.RenewableContext;
using Icon.Esb.EwicErrorResponseListener.Common.Models;
using Icon.Esb.EwicErrorResponseListener.DataAccess.Commands;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;

namespace Icon.Esb.EwicErrorResponseListener.Tests.Integration.Commands
{
    [TestClass]
    public class SaveToMessageResponseCommandTests
    {
        private SaveToMessageResponseCommand command;
        private IRenewableContext<IconContext> globalContext;
        private IconContext context;
        private DbContextTransaction transaction;
        private MessageHistory testMessage;
        private R10MessageResponse testResponse;
        
        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);
            command = new SaveToMessageResponseCommand(globalContext);

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void SaveToMessageResponse_NewResponse_ResponseShouldBeSavedToDatabase()
        {
            // Given.
            testMessage = new TestMessageHistoryBuilder();

            context.MessageHistory.Add(testMessage);
            context.SaveChanges();

            testResponse = new R10MessageResponse 
            { 
                MessageHistoryId = testMessage.MessageHistoryId,
                ResponseText = String.Empty
            };

            context.R10MessageResponse.Add(testResponse);
            context.SaveChanges();

            var parameters = new SaveToMessageResponseParameters
            {
                ErrorResponse = new EwicErrorResponseModel { MessageHistoryId = testMessage.MessageHistoryId }
            };

            // When.
            command.Execute(parameters);

            // Then.
            var newResponse = context.R10MessageResponse.SingleOrDefault(r => 
                r.R10MessageResponseId == testResponse.R10MessageResponseId &&
                r.MessageHistoryId == testMessage.MessageHistoryId);

            Assert.IsNotNull(newResponse);
            Assert.AreEqual(testMessage.MessageHistoryId, newResponse.MessageHistoryId);
        }
    }
}
