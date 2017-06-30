using GlobalEventController.DataAccess.Commands;
using GlobalEventController.Testing.Common;
using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Transactions;
using static GlobalEventController.DataAccess.Commands.BrandDeleteCommand;

namespace GlobalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class BrandDeleteCommandHandlerTests
    {
        private BrandDeleteCommandHandler brandDeleteCommandHandler;
        private BrandDeleteCommand command;
        private TransactionScope transaction;
        private IrmaContext context;
        private IrmaDbContextFactory contextFactory;
        private const string region = "FL";
        private TestIrmaDataHelper helper = new TestIrmaDataHelper();

        [TestInitialize]
        public void InitializeData()
        {
            transaction = new TransactionScope();
            context = new IrmaContext();
            command = new BrandDeleteCommand();

            contextFactory = new IrmaDbContextFactory();
            brandDeleteCommandHandler = new BrandDeleteCommandHandler(contextFactory);
        }

        [TestCleanup]
        public void CleanupData()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void BrandDeleteCommandHandlerHandle_WhenBrandNotFoundInIrma_NothingIsDeleted()
        {
            // Given
            command.IconBrandId = TestingConstants.IconBrandId_Negative;
            command.Region = region;
            int validatedBrandsBefore = context.ValidatedBrand.Count();
            int itemBrandsBefore = context.ItemBrand.Count();

            // When
            brandDeleteCommandHandler.Handle(command);

            // Then
            int validatedBrandsAfter = context.ValidatedBrand.Count();
            int itemBrandsAfter = context.ItemBrand.Count();
            Assert.AreEqual(validatedBrandsBefore, validatedBrandsAfter);
            Assert.AreEqual(itemBrandsBefore, itemBrandsAfter);
        }

        [TestMethod]
        public void BrandDeleteCommandHandlerHandle_WhenBrandNotFoundInIrma_ResultMatchesExpected()
        {
            // Given
            command.IconBrandId = TestingConstants.IconBrandId_Negative;
            command.Region = region;
            var expectedResult = BrandDeleteResult.NothingDeleted;

            // When
            brandDeleteCommandHandler.Handle(command);

            // Then
            Assert.AreEqual(expectedResult, command.Result);
        }

        [TestMethod]
        public void BrandDeleteCommandHandlerHandle_WhenBrandNotAssociatedToItem_ValidatedAndItemBrandsBothDeleted()
        {
            // Given
            ItemBrand testBrand = helper.CreateAndSaveItemBrandForTest(context);
            ValidatedBrand testValidatedBrand = helper.CreateAndSaveValidatedBrandForTest(context,
                testBrand.Brand_ID, TestingConstants.IconBrandId_Negative);

            int validatedBrandsBefore = context.ValidatedBrand.Count();
            int itemBrandsBefore = context.ItemBrand.Count();

            command.IconBrandId = TestingConstants.IconBrandId_Negative;
            command.Region = region;

            // When
            brandDeleteCommandHandler.Handle(command);

            // Then
            Assert.IsFalse(context.ValidatedBrand.AsNoTracking().Any(vb => vb.IconBrandId == TestingConstants.IconBrandId_Negative));
            Assert.IsFalse(context.ItemBrand.AsNoTracking().Any(ib => ib.Brand_Name == testBrand.Brand_Name));
            int validatedBrandsAfter = context.ValidatedBrand.AsNoTracking().Count();
            int itemBrandsAfter = context.ItemBrand.AsNoTracking().Count();
            Assert.AreEqual(validatedBrandsBefore, validatedBrandsAfter + 1);
            Assert.AreEqual(itemBrandsBefore, itemBrandsAfter + 1);
        }

        [TestMethod]
        public void BrandDeleteCommandHandlerHandle_WhenBrandNotAssociatedToItem_ResultMatchesExpected()
        {
            // Given
            ItemBrand testBrand = helper
                .CreateAndSaveItemBrandForTest(context);
            ValidatedBrand testValidatedBrand = helper.CreateAndSaveValidatedBrandForTest(context,
                testBrand.Brand_ID, TestingConstants.IconBrandId_Negative);

            command.IconBrandId = TestingConstants.IconBrandId_Negative;
            command.Region = region;
            var expectedResult = BrandDeleteResult.ValidatedAndItemBrandsDeleted;

            // When
            brandDeleteCommandHandler.Handle(command);
            context.SaveChanges();

            // Then
            Assert.AreEqual(expectedResult, command.Result);
            //confirm that combined bitwise result values work as expectedd
            Assert.IsTrue((command.Result & BrandDeleteResult.ValidatedBrandDeleted)
                == BrandDeleteResult.ValidatedBrandDeleted);
            Assert.IsTrue((command.Result & BrandDeleteResult.ItemBrandDeleted)
                == BrandDeleteResult.ItemBrandDeleted);
        }

        [TestMethod]
        public void BrandDeleteCommandHandlerHandle_WhenBrandAssociatedToItem_ItemBrandIsNotDeleted()
        {
            // Given
            ItemBrand testBrand = helper
                .CreateAndSaveItemBrandForTest(context);
            Item testItem = helper
                .CreateAndSaveItemAndSubteamForTest(context, testBrand.Brand_ID);
            ValidatedBrand testValidatedBrand = helper
                .CreateAndSaveValidatedBrandForTest(context, testItem.Brand_ID);

            int itemBrandsBefore = context.ItemBrand.Count();
            command.IconBrandId = testValidatedBrand.IconBrandId;
            command.Region = region;

            // When
            brandDeleteCommandHandler.Handle(command);
            context.SaveChanges();

            // Then
            int itemBrandsAfter = context.ItemBrand.Count();
            Assert.AreEqual(itemBrandsBefore, itemBrandsAfter);
            Assert.IsTrue(context.ItemBrand.Any(ib => ib.Brand_Name == testBrand.Brand_Name));
        }

        [TestMethod]
        public void BrandDeleteCommandHandlerHandle_WhenBrandAssociatedToItem_ValidatedBrandIsDeleted()
        {
            // Given
            ItemBrand testBrand = helper
                .CreateAndSaveItemBrandForTest(context);
            Item testItem = helper
                .CreateAndSaveItemAndSubteamForTest(context, testBrand.Brand_ID);
            ValidatedBrand testValidatedBrand = helper
                .CreateAndSaveValidatedBrandForTest(context, testItem.Brand_ID);

            int validatedBrandsBefore = context.ValidatedBrand.Count();
            command.IconBrandId = testValidatedBrand.IconBrandId;
            command.Region = region;

            // When
            brandDeleteCommandHandler.Handle(command);

            // Then
            Assert.IsFalse(context.ValidatedBrand.AsNoTracking().Any(vb => vb.IconBrandId == TestingConstants.IconBrandId_Negative));
            int validatedBrandsAfter = context.ValidatedBrand.Count();
            Assert.AreEqual(validatedBrandsBefore, validatedBrandsAfter + 1);
        }


        [TestMethod]
        public void BrandDeleteCommandHandlerHandle_WhenBrandAssociatedToItem_ResultMatchesExpected()
        {
            // Given
            ItemBrand testBrand = helper
                .CreateAndSaveItemBrandForTest(context);
            Item testItem = helper
                .CreateAndSaveItemAndSubteamForTest(context, testBrand.Brand_ID);
            ValidatedBrand testValidatedBrand = helper
                .CreateAndSaveValidatedBrandForTest(context, testItem.Brand_ID);

            command.IconBrandId = testValidatedBrand.IconBrandId;
            command.Region = region;
            var expectedResult = BrandDeleteResult.ValidatedBrandDeletedButItemBrandAssociatedWithItems;

            // When
            brandDeleteCommandHandler.Handle(command);
            context.SaveChanges();

            // Then
            Assert.AreEqual(expectedResult, command.Result);
            //confirm that combined bitwise result values work as expected
            Assert.IsTrue((command.Result & BrandDeleteResult.ValidatedBrandDeleted)
                == BrandDeleteResult.ValidatedBrandDeleted);
            Assert.IsTrue((command.Result & BrandDeleteResult.ItemBrandAssociatedWithItems)
                == BrandDeleteResult.ItemBrandAssociatedWithItems);
        }
    }
}

