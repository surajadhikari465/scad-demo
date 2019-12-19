using Icon.Common.Context;
using Icon.Framework;
using Services.NewItem.Commands;
using Services.NewItem.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Services.NewItem.Tests.Commands
{
    [TestClass]
    public class ArchiveNewItemsCommandHandlerTests
    {
        private ArchiveNewItemsCommandHandler commandHandler;
        private ArchiveNewItemsCommand command;
        private Mock<IRenewableContext<IconContext>> mockContext;
        private IconContext context;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            context = new IconContext();
            context.Database.Connection.Open();
            mockContext = new Mock<IRenewableContext<IconContext>>();
            mockContext.SetupGet(m => m.Context).Returns(context);
            commandHandler = new ArchiveNewItemsCommandHandler(mockContext.Object);
            command = new ArchiveNewItemsCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void ArchiveNewItems_1NewItem_ArchivesItemToArchiveTable()
        {
            //Given
            var newItem = new NewItemModel
            {
                QueueId = 123456789,
                ScanCode = "123456789",
                ErrorCode = "TestErrorCode",
                ErrorDetails = "TestErrorDetails",
                Region = "FL",
                MessageHistoryId = 12345
            };
            command.NewItems = new List<NewItemModel>
            {
                newItem
            };

            //When
            commandHandler.Execute(command);

            //Then
            var archiveModel = context.Database.SqlQuery<NewItemArchiveModel>("select * from MessageArchiveNewItem where ScanCode = '123456789'").Single();
            Assert.AreEqual(newItem.QueueId, archiveModel.QueueId);
            Assert.AreEqual(newItem.ScanCode, archiveModel.ScanCode);
            Assert.AreEqual(newItem.ErrorCode, archiveModel.ErrorCode);
            Assert.AreEqual(newItem.ErrorDetails, archiveModel.ErrorDetails);
            Assert.AreEqual(newItem.Region, archiveModel.Region);
            Assert.AreEqual(newItem.MessageHistoryId, archiveModel.MessageHistoryId);
            Assert.AreEqual(JsonConvert.SerializeObject(newItem), archiveModel.Context);
        }
    }
}
