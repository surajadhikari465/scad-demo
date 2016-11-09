using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Decorators;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Unit.Decorators
{
    [TestClass]
    public class UpdateProductMammothEventDecoratorTests
    {
        private IconContext context;
        private DbContextTransaction transaction;
        private UpdateProductMammothEventDecorator updateProductMammothEventDecorator;
        private BulkImportCommand<BulkImportItemModel> bulkImportCommand;
        private Mock<ICommandHandler<BulkImportCommand<BulkImportItemModel>>> mockBulkImportCommandHandler;
        private Mock<ILogger> mockLogger;

        private int maxQueueId = 0;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>();

            mockBulkImportCommandHandler = new Mock<ICommandHandler<BulkImportCommand<BulkImportItemModel>>>();
            mockLogger = new Mock<ILogger>();
            updateProductMammothEventDecorator = new UpdateProductMammothEventDecorator(mockBulkImportCommandHandler.Object, context, mockLogger.Object);

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (transaction != null)
            {
                transaction.Rollback();
            }
        }

        [TestMethod]
        public void UpdateProductInIcon_UpdateProductMammothEventDecoratorCalled_ShouldCallBulkImportCommandHandler()
        {
            //Given
            bulkImportCommand.BulkImportData = new List<BulkImportItemModel>
            {
                new BulkImportItemModel(){ScanCode = "1111111111"},
                new BulkImportItemModel(){ScanCode = "2222222222"}
            };

            //When
            updateProductMammothEventDecorator.Execute(bulkImportCommand);

            //Then
            mockBulkImportCommandHandler.Verify(v => v.Execute(It.IsAny<BulkImportCommand<BulkImportItemModel>>()), Times.Once);
        }

        [TestMethod]
        public void UpdateProductInIcon_UpdateProductMammothEventDecoratorCalled_MammothProductUpdateEventsCreated()
        {
            //Given
            var scanCodes = context.ScanCode.Take(2).ToList();

            bulkImportCommand.BulkImportData = new List<BulkImportItemModel>
            {
                new BulkImportItemModel(){ScanCode = scanCodes[0].scanCode},
                new BulkImportItemModel(){ScanCode = scanCodes[1].scanCode}
            };

            maxQueueId = context.Database.SqlQuery<int>("select count(QueueId) from mammoth.EventQueue").First();

            if (maxQueueId > 0)
                maxQueueId = context.Database.SqlQuery<int>("select max(QueueId) from mammoth.EventQueue").First();

            //When
            updateProductMammothEventDecorator.Execute(bulkImportCommand);

            //Then
            var actualEventMessage = context.Database.SqlQuery<string>("select EventMessage from mammoth.EventQueue where QueueId > " + this.maxQueueId.ToString()).ToList();
            var actualEventTypeId = context.Database.SqlQuery<int>("select EventTypeId from mammoth.EventQueue where QueueId > " + this.maxQueueId.ToString()).ToList();

            for (int i = 0; i < scanCodes.Count(); i++)
            {
                Assert.AreEqual(scanCodes[i].scanCode, actualEventMessage[i].ToString(), "The inserted event's EventMessage did not match the expected value.");
                Assert.AreEqual(MommothEventTypes.Productupdate, actualEventTypeId[i], "The inserted event's EventTypeId did not match the expected value.");
            }
        }
    }
}
