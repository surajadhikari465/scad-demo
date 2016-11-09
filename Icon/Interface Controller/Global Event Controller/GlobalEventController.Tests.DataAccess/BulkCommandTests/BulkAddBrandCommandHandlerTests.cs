using GlobalEventController.Common;
using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.Testing.Common;
using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GlobalEventController.Tests.DataAccess.BulkCommandTests
{
    [TestClass]
    public class BulkAddBrandCommandHandlerTests
    {
        private IrmaContext context;
        private BulkAddBrandCommand command;
        private BulkAddBrandCommandHandler handler;
        private List<ValidatedItemModel> validatedItems;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IrmaContext(); // this is the FL ItemCatalog_Test database
            this.command = new BulkAddBrandCommand();
            this.handler = new BulkAddBrandCommandHandler(this.context);
            this.validatedItems = new List<ValidatedItemModel>();

            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            if (transaction != null)
            {
                this.transaction.Rollback();
            }
        }

        [TestMethod]
        public void BulkAddBrand_BrandDoesNotExistInItemBrand_BrandIsAddedToItemBrand()
        {
            // Given
            this.validatedItems = BuildValidatedItems();
            this.command.ValidatedItems = this.validatedItems;

            string brandOne = this.validatedItems.First().BrandName;
            string brandTwo = this.validatedItems.Last().BrandName;

            // When
            this.handler.Handle(command);

            // Then
            List<ItemBrand> actual = this.context.ItemBrand
                .Where(ib => ib.Brand_Name == brandOne || ib.Brand_Name == brandTwo)
                .ToList();

            Assert.AreEqual(this.command.ValidatedItems.Count, actual.Count);
        }

        [TestMethod]
        public void BulkAddBrand_BrandDoesNotExistInItemBrand_BrandIsAddedToValidatedBrand()
        {
            // Given
            this.validatedItems = BuildValidatedItems();
            this.command.ValidatedItems = this.validatedItems;

            ValidatedItemModel itemOne = this.validatedItems.First();
            ValidatedItemModel itemTwo = this.validatedItems.Last();

            // When
            this.handler.Handle(command);

            // Then
            ValidatedBrand validatedBrandOne = this.context.ValidatedBrand.SingleOrDefault(ib => ib.ItemBrand.Brand_Name == itemOne.BrandName);
            ValidatedBrand validatedBrandTwo = this.context.ValidatedBrand.SingleOrDefault(ib => ib.ItemBrand.Brand_Name == itemTwo.BrandName);

            Assert.IsNotNull(validatedBrandOne);
            Assert.IsNotNull(validatedBrandTwo);
            Assert.AreEqual(itemOne.BrandId, validatedBrandOne.IconBrandId, "Actual IconBrandId does not match the expected IconBrandId.");
            Assert.AreEqual(itemTwo.BrandId, validatedBrandTwo.IconBrandId, "Actual IconBrandId does not match the expected IconBrandId.");
        }

        [TestMethod]
        public void BulkAddBrand_BrandAlreadyExistsInItemBrand_BrandIsNotAddedAgain()
        {
            // Given
            string existingBrand = this.context.ItemBrand.First().Brand_Name;
            this.validatedItems.Add(new TestValidatedItemModelBuilder().WithBrandId(0).WithItemId(0).WithBrandName(existingBrand).Build());
            this.command.ValidatedItems = this.validatedItems;

            // When
            this.handler.Handle(command);

            // Then
            List<ItemBrand> actualBrands = this.context.ItemBrand.Where(ib => ib.Brand_Name == existingBrand).ToList();
            Assert.IsTrue(actualBrands.Count == 1);
        }

        [TestMethod]
        public void BulkAddBrand_BrandAlreadyExistsInValidatedBrand_BrandIsNotAddedAgain()
        {
            // Given
            ValidatedBrand expectedValidatedBrand = this.context.ValidatedBrand.First();
            this.validatedItems.Add(new TestValidatedItemModelBuilder().WithItemId(0).WithBrandName(expectedValidatedBrand.ItemBrand.Brand_Name).Build());
            this.command.ValidatedItems = this.validatedItems;

            // When
            this.handler.Handle(command);

            // Then
            ValidatedBrand actualValidatedBrand = this.context.ValidatedBrand
                .SingleOrDefault(vb => vb.ItemBrand.Brand_Name == expectedValidatedBrand.ItemBrand.Brand_Name);
            Assert.IsNotNull(actualValidatedBrand);
            Assert.AreEqual(expectedValidatedBrand.Id, actualValidatedBrand.Id);
            Assert.AreEqual(expectedValidatedBrand.IconBrandId, actualValidatedBrand.IconBrandId);
            Assert.AreEqual(expectedValidatedBrand.IrmaBrandId, actualValidatedBrand.IrmaBrandId);
        }

        [TestMethod]
        public void BulkAddBrand_BrandAlreadyExistsInItemBrandButNotValidatedBrand_BrandIsAddedToValidatedBrand()
        {
            // Given
            ItemBrand brandNotValidated = this.context.ItemBrand.First(ib => !ib.ValidatedBrand.Any());
            string identifier = this.context.ItemIdentifier.First(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item.Retail_Sale == true).Identifier;
            this.validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode(identifier).WithBrandId(5).WithBrandName(brandNotValidated.Brand_Name).Build());
            this.command.ValidatedItems = this.validatedItems;

            if (this.context.ValidatedBrand.Any(vb => vb.IrmaBrandId == brandNotValidated.Brand_ID))
	        {
		        Assert.Fail("The brand used for this test already exists in the Validated Brand table.");
	        }

            int expectedIconBrandId = this.validatedItems.First().BrandId;

            // When
            handler.Handle(command);

            // Then
            Assert.IsTrue(this.context.ValidatedBrand.Any(vb => vb.IconBrandId == expectedIconBrandId));
            Assert.IsTrue(this.context.ValidatedBrand.Any(vb => vb.ItemBrand.Brand_Name == brandNotValidated.Brand_Name));
        }

        [TestMethod]
        public void BulkAddBrand_BrandNotInIrmaButIdentifierIsNotDefault_BrandIsNotAddedToItemBrandOrValidatedBrandTables()
        {
            // Given
            // get identifier that is not default
            string identifier = this.context.ItemIdentifier.First(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 0 && ii.Item.Retail_Sale == true).Identifier;
            this.validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode(identifier).WithBrandId(1).WithBrandName("i am new brand").Build());
            this.command.ValidatedItems = this.validatedItems;

            string expectedBrandName = this.validatedItems.First().BrandName;

            // When
            handler.Handle(this.command);

            // Then
            ItemBrand actualBrand = this.context.ItemBrand.FirstOrDefault(ib => ib.Brand_Name == expectedBrandName);
            ValidatedBrand actualValidatedBrand = this.context.ValidatedBrand.FirstOrDefault(vb => vb.ItemBrand.Brand_Name == expectedBrandName);

            Assert.IsNull(actualBrand);
            Assert.IsNull(actualValidatedBrand);
        }

        [TestMethod]
        public void BulkAddBrand_TwoDefaultIdentifiersWithSameBrand_BrandIsAddedOnlyOnce()
        {
            // Given
            string identifierOne = this.context.ItemIdentifier.First(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item.Retail_Sale == true).Identifier;
            string identifierTwo = this.context.ItemIdentifier
                .First(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item.Retail_Sale == true && ii.Identifier != identifierOne).Identifier;

            this.validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode(identifierOne).WithBrandId(1).WithBrandName("automated test brand").Build());
            this.validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode(identifierTwo).WithBrandId(1).WithBrandName("automated test brand").Build());
            this.command.ValidatedItems = this.validatedItems;

            string expectedBrandName = this.validatedItems.First().BrandName;

            // When
            handler.Handle(this.command);

            // Then
            IEnumerable<ItemBrand> actualBrand = this.context.ItemBrand.Where(ib => ib.Brand_Name == expectedBrandName);
            IEnumerable<ValidatedBrand> actualValidatedBrand = this.context.ValidatedBrand.Where(vb => vb.ItemBrand.Brand_Name == expectedBrandName);

            Assert.IsTrue(actualBrand.Count() == 1);
            Assert.IsTrue(actualValidatedBrand.Count() == 1);
        }

        [TestMethod]
        public void BulkAddBrand_BrandDoesNotExist_BrandUserPopulatedWithInterfaceControllerUserId()
        {
            // Given
            int expectedUserId = this.context.Users.First(u => u.UserName == "iconcontrolleruser").User_ID;
            this.validatedItems = BuildValidatedItems();
            this.command.ValidatedItems = this.validatedItems;

            string expectedBrandNameOne = this.validatedItems.First().BrandName;
            string expectedBrandNameTwo = this.validatedItems.Last().BrandName;

            // When
            handler.Handle(this.command);

            // Then
            int actualUserIdOne = this.context.ItemBrand.First(ib => ib.Brand_Name == expectedBrandNameOne).User_ID.Value;
            int actualUserIdTwo = this.context.ItemBrand.First(ib => ib.Brand_Name == expectedBrandNameTwo).User_ID.Value;

            Assert.AreEqual(expectedUserId, actualUserIdOne, "The expected User_ID does not match the actual User_ID in ItemBrand");
            Assert.AreEqual(expectedUserId, actualUserIdTwo, "The expected User_ID does not match the actual User_ID in ItemBrand");
        }

        private List<ValidatedItemModel> BuildValidatedItems()
        {
            List<ValidatedItemModel> validatedItems = new List<ValidatedItemModel>();
            List<string> identifiers = new List<string>();
            identifiers = this.context.ItemIdentifier
                .Where(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item.Retail_Sale == true)
                .Select(ii => ii.Identifier).Take(2).ToList();

            validatedItems.Add(new TestValidatedItemModelBuilder().WithBrandId(-1).WithItemId(-1).WithBrandName("Add Brand Command Test1").WithScanCode(identifiers.First()).Build());
            validatedItems.Add(new TestValidatedItemModelBuilder().WithBrandId(-2).WithItemId(-2).WithBrandName("Add Brand Command Test2").WithScanCode(identifiers.Last()).Build());

            return validatedItems;
        }
    }
}
