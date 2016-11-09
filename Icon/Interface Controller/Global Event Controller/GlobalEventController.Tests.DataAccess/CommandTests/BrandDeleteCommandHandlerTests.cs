using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Irma.Framework;
using Irma.Testing.Builders;
using GlobalEventController.DataAccess.Commands;
using Moq;
using Icon.Logging;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using static GlobalEventController.DataAccess.Commands.BrandDeleteCommand;

namespace GlobalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class BrandDeleteCommandHandlerTests
    {
        private IrmaContext context;
        private BrandDeleteCommand command;
        private BrandDeleteCommandHandler handler;
        private Mock<ILogger<BrandDeleteCommandHandler>> mockLogger;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IrmaContext();
            this.command = new BrandDeleteCommand();
            this.mockLogger = new Mock<ILogger<BrandDeleteCommandHandler>>();
            this.handler = new BrandDeleteCommandHandler(this.context, mockLogger.Object);

            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            if (this.transaction != null)
            {
                this.transaction.Rollback();
                this.transaction.Dispose();
            }
        }

        [TestMethod]
        public void BrandDelete_BrandNotFoundInIrma_LoggerErrorCalled()
        {
            // Given
            this.command.IconBrandId = -1;
            this.command.Region = "FL";
            var expectedResult = BrandDeleteResult.NothingDeleted;

            // When
            this.handler.Handle(this.command);

            // Then
            this.mockLogger.Verify(log => log.Error(It.IsAny<string>()), Times.Once, "Logging Error was not called one time.");
            Assert.AreEqual(expectedResult, command.Result);
        }

        [TestMethod]
        public void BrandDelete_BrandAssociatedToItem_ValidatedBrandDeleted()
        {
            // Given
            var testItem = context.Item.First(i => i.Brand_ID != null);

            ValidatedBrand testValidatedBrand = new ValidatedBrand { IrmaBrandId = testItem.Brand_ID ?? 0, IconBrandId = -1 };
            context.ValidatedBrand.Add(testValidatedBrand);
            context.SaveChanges();

            this.command.IconBrandId = -1;
            this.command.Region = "FL";
            var expectedResult = BrandDeleteResult.ValidatedBrandDeleted;

            // When
            this.handler.Handle(this.command);
            context.SaveChanges();
            // Then
            Assert.AreEqual(expectedResult, command.Result);
            Assert.AreEqual(testItem.ItemBrand, context.ItemBrand.Where(i=>i.Brand_Name==testItem.ItemBrand.Brand_Name).FirstOrDefault());
            Assert.AreEqual(false, context.ValidatedBrand.Where(vb=>vb.Id==testValidatedBrand.Id).Any());
        }

        [TestMethod]
        public void BrandDelete_BrandNotAssociatedToItem_ValidatedAndItemBrandsDeleted()
        {
            // Given
            ItemBrand testItemBrand = new ItemBrand { Brand_Name = "test itemBrand" };
            context.ItemBrand.Add(testItemBrand);

            ValidatedBrand testValidatedBrand = new ValidatedBrand { IrmaBrandId = testItemBrand.Brand_ID, IconBrandId = -1 };
            context.ValidatedBrand.Add(testValidatedBrand);
            context.SaveChanges();

            this.command.IconBrandId = -1;
            this.command.Region = "FL";
            var expectedResult = BrandDeleteResult.ItemBrandAndValidatedBrandDeleted;

            // When
            this.handler.Handle(this.command);
            context.SaveChanges();
            // Then
            bool isValidatedBrandExist = context.ValidatedBrand.Any(vb => vb.IconBrandId == -1);
            bool isItemBrandExist = context.ItemBrand.Any(ib => ib.Brand_Name == "test itemBrand");
            Assert.AreEqual(expectedResult, command.Result);
            Assert.AreEqual(false, isValidatedBrandExist);
            Assert.AreEqual(false, isItemBrandExist);
        }
    }
}

