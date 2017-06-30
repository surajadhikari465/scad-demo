using GlobalEventController.Common;
using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.Testing.Common;
using Irma.Framework;
using Irma.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace GlobalEventController.Tests.DataAccess.BulkCommandTests
{
    [TestClass]
    public class BulkUpdateItemCommandHandlerTests
    {
        private BulkUpdateItemCommandHandler handler;
        private BulkUpdateItemCommand command;
        private IrmaDbContextFactory contextFactory;
        private IrmaContext context;
        private List<ValidatedItemModel> validatedItems;
        private TransactionScope transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.transaction = new TransactionScope();
            this.context = new IrmaContext();
            this.command = new BulkUpdateItemCommand();
            this.contextFactory = new IrmaDbContextFactory();
            this.handler = new BulkUpdateItemCommandHandler(this.contextFactory);
            this.validatedItems = new List<ValidatedItemModel>();
        }

        [TestCleanup]
        public void CleanupData()
        {
            this.transaction.Dispose();
        }

        [TestMethod]
        public void BulkUpdateItem_ValidatedItemsOfDefaultIdentifiers_ItemUpdatedInIrmaWithValidatedItemData()
        {
            // Given
            string identifierOne = this.context.ItemIdentifier
                .First(ii => ii.Default_Identifier == 1 && ii.Deleted_Identifier == 0 && ii.Item.Retail_Sale == true).Identifier;
            string identifierTwo = this.context.ItemIdentifier
                .First(ii => ii.Default_Identifier == 1 && ii.Deleted_Identifier == 0 && ii.Item.Retail_Sale == true && ii.Identifier != identifierOne).Identifier;
            ValidatedBrand brand = this.context.ValidatedBrand.First();
            NatItemClass natClass = this.context.NatItemClass.First();
            TaxClass taxClass = this.context.TaxClass.First(tc => !tc.TaxClassDesc.ToLower().Contains("do not use"));

            this.validatedItems.Add(new TestValidatedItemModelBuilder()
                .WithScanCode(identifierOne).WithBrandId(brand.IconBrandId).WithTaxClass(taxClass.TaxClassDesc).WithNationalClassCode(natClass.ClassID.ToString()).WithItemId(1).Build());
            this.validatedItems.Add(new TestValidatedItemModelBuilder()
                .WithScanCode(identifierTwo).WithBrandId(brand.IconBrandId).WithTaxClass(taxClass.TaxClassDesc).WithProductDesccription("Test Validated Item2").WithNationalClassCode(natClass.ClassID.ToString()).WithItemId(2).Build());
            this.command.ValidatedItems = this.validatedItems;

            ValidatedItemModel expectedItemOne = this.validatedItems[0];
            ValidatedItemModel expectedItemTwo = this.validatedItems[1];
            DateTime now = DateTime.Now.Subtract(TimeSpan.FromMilliseconds(1000));

            // When
            this.handler.Handle(this.command);

            // Then
            Item actualItemOne = this.context.Item.First(i => i.ItemIdentifier.Any(ii => ii.Identifier == identifierOne));
            Item actualItemTwo = this.context.Item.First(i => i.ItemIdentifier.Any(ii => ii.Identifier == identifierTwo));
            context.Entry<Item>(actualItemOne).Reload();
            context.Entry<Item>(actualItemTwo).Reload();

            // first item
            Assert.AreEqual(expectedItemOne.ProductDescription, actualItemOne.Item_Description, "The Item_Description does not match the expected value.");
            Assert.AreEqual(expectedItemOne.PosDescription.ToUpper(), actualItemOne.POS_Description, "The POS_Description does not match the expected value.");
            Assert.AreEqual(expectedItemOne.FoodStampEligible == "1", actualItemOne.Food_Stamps, "The Food_Stamps does not match the expected value.");
            Assert.AreEqual(Convert.ToDecimal(expectedItemOne.PackageUnit), actualItemOne.Package_Desc1, "The Package_Desc1 does not match the expected value.");
            Assert.AreEqual(taxClass.TaxClassID, actualItemOne.TaxClassID, "The TaxClassID does not match the expected value.");
            Assert.AreEqual(natClass.ClassID, actualItemOne.ClassID, "The National Class code does not match the expected value.");
            Assert.AreEqual(brand.IrmaBrandId, actualItemOne.Brand_ID, "The Brand_ID does not match the expected value.");
            Assert.AreEqual(null, actualItemOne.LastModifiedUser_ID, "The LastModifiedUser_ID was not null. The Item Update trigger reverts it back to null if user is 'iconcontrolleruser'.");
            Assert.IsTrue(actualItemOne.LastModifiedDate > now);

            // second item
            Assert.AreEqual(expectedItemTwo.ProductDescription, actualItemTwo.Item_Description, "The Item_Description does not match the expected value.");
            Assert.AreEqual(expectedItemTwo.PosDescription.ToUpper(), actualItemTwo.POS_Description, "The POS_Description does not match the expected value.");
            Assert.AreEqual(expectedItemTwo.FoodStampEligible == "1", actualItemTwo.Food_Stamps, "The Food_Stamps does not match the expected value.");
            Assert.AreEqual(Convert.ToDecimal(expectedItemTwo.PackageUnit), actualItemTwo.Package_Desc1, "The Package_Desc1 does not match the expected value.");
            Assert.AreEqual(natClass.ClassID, actualItemTwo.ClassID, "The National Class code does not match the expected value.");
            Assert.AreEqual(taxClass.TaxClassID, actualItemTwo.TaxClassID, "The TaxClassID does not match the expected value.");
            Assert.AreEqual(brand.IrmaBrandId, actualItemTwo.Brand_ID, "The Brand_ID does not match the expected value.");
            Assert.AreEqual(null, actualItemTwo.LastModifiedUser_ID, "The LastModifiedUser_ID was not null. The Item Update trigger reverts it back to null if user is 'iconcontrolleruser'.");
            Assert.IsTrue(actualItemTwo.LastModifiedDate > now);
        }

        [TestMethod]
        public void BulkUpdateItem_ValidatedItemsOfNonDefaultIdentifiers_ItemsAreNotUpdated()
        {
            // Given
            string identifierOne = this.context.ItemIdentifier
                .First(ii => ii.Default_Identifier == 0 && ii.Deleted_Identifier == 0 && ii.Item.Retail_Sale == true).Identifier;
            string identifierTwo = this.context.ItemIdentifier
                .First(ii => ii.Default_Identifier == 0 && ii.Deleted_Identifier == 0 && ii.Item.Retail_Sale == true && ii.Identifier != identifierOne).Identifier;

            ValidatedBrand brand = this.context.ValidatedBrand.First();
            TaxClass taxClass = this.context.TaxClass.First(tc => !tc.TaxClassDesc.ToLower().Contains("do not use"));

            this.validatedItems.Add(new TestValidatedItemModelBuilder()
                .WithScanCode(identifierOne).WithBrandId(brand.IconBrandId).WithTaxClass(taxClass.TaxClassDesc).WithItemId(1).Build());
            this.validatedItems.Add(new TestValidatedItemModelBuilder()
                .WithScanCode(identifierTwo).WithBrandId(brand.IconBrandId).WithTaxClass(taxClass.TaxClassDesc).WithProductDesccription("Test Validated Item2").WithItemId(2).Build());
            this.command.ValidatedItems = this.validatedItems;

            ValidatedItemModel expectedItemOne = this.validatedItems[0];
            ValidatedItemModel expectedItemTwo = this.validatedItems[1];

            Item itemOne = this.context.Item.First(i => i.ItemIdentifier.Any(ii => ii.Identifier == identifierOne));
            Item itemTwo = this.context.Item.First(i => i.ItemIdentifier.Any(ii => ii.Identifier == identifierTwo));

            // When
            this.handler.Handle(this.command);

            // Then
            this.context.Entry(itemOne).Reload();
            this.context.Entry(itemTwo).Reload();
            Item actualItemOne = this.context.Item.First(i => i.ItemIdentifier.Any(ii => ii.Identifier == identifierOne));
            Item actualItemTwo = this.context.Item.First(i => i.ItemIdentifier.Any(ii => ii.Identifier == identifierTwo));

            // first item
            Assert.AreEqual(itemOne.Item_Description, actualItemOne.Item_Description, "The Item_Description changed when it wasn't supposed to.");
            Assert.AreEqual(itemOne.POS_Description, actualItemOne.POS_Description, "The POS_Description changed when it wasn't supposed to.");
            Assert.AreEqual(itemOne.Food_Stamps, actualItemOne.Food_Stamps, "The Food_Stamps changed when it wasn't supposed to.");
            Assert.AreEqual(itemOne.Package_Desc1, actualItemOne.Package_Desc1, "The Package_Desc1 changed when it wasn't supposed to.");
            Assert.AreEqual(itemOne.TaxClassID, actualItemOne.TaxClassID, "The TaxClassID changed when it wasn't supposed to.");
            Assert.AreEqual(itemOne.Brand_ID, actualItemOne.Brand_ID, "The Brand_ID changed when it wasn't supposed to.");
            Assert.AreEqual(itemOne.LastModifiedUser_ID, actualItemOne.LastModifiedUser_ID, "The LastModifiedUser_ID changed when it wasn't supposed to.");
            Assert.AreEqual(itemOne.LastModifiedDate, actualItemOne.LastModifiedDate, "The LastModifiedDate changed when it wasn't supposed to.");

            // second item
            Assert.AreEqual(itemTwo.Item_Description, actualItemTwo.Item_Description, "The Item_Description changed when it wasn't supposed to.");
            Assert.AreEqual(itemTwo.POS_Description, actualItemTwo.POS_Description, "The POS_Description changed when it wasn't supposed to.");
            Assert.AreEqual(itemTwo.Food_Stamps, actualItemTwo.Food_Stamps, "The Food_Stamps changed when it wasn't supposed to.");
            Assert.AreEqual(itemTwo.Package_Desc1, actualItemTwo.Package_Desc1, "The Package_Desc1 changed when it wasn't supposed to.");
            Assert.AreEqual(itemTwo.TaxClassID, actualItemTwo.TaxClassID, "The TaxClassID changed when it wasn't supposed to.");
            Assert.AreEqual(itemTwo.Brand_ID, actualItemTwo.Brand_ID, "The Brand_ID changed when it wasn't supposed to.");
            Assert.AreEqual(itemTwo.LastModifiedUser_ID, actualItemTwo.LastModifiedUser_ID, "The LastModifiedUser_ID changed when it wasn't supposed to.");
            Assert.AreEqual(itemTwo.LastModifiedDate, actualItemTwo.LastModifiedDate, "The LastModifiedDate changed when it wasn't supposed to.");
        }

        [TestMethod]
        public void BulkUpdateItem_PriceRowsOfValidatedItemAlreadyHavePosTareValue_TareValueIsNotUpdated()
        {
            // Given
            string identifierOne = this.context.ItemIdentifier.First(ii => ii.Default_Identifier == 1
                    && ii.Deleted_Identifier == 0
                    && ii.Item.Retail_Sale == true
                    && ii.Item.Price
                        .Any(p => p.PosTare != null && p.PosTare != 0)).Identifier;
            string identifierTwo = this.context.ItemIdentifier.First(ii => ii.Default_Identifier == 1
                    && ii.Deleted_Identifier == 0
                    && ii.Item.Retail_Sale == true
                    && ii.Identifier != identifierOne
                    && ii.Item.Price
                        .Any(p => p.PosTare != null && p.PosTare != 0)).Identifier;

            ValidatedBrand brand = this.context.ValidatedBrand.First();
            TaxClass taxClass = this.context.TaxClass.First(tc => !tc.TaxClassDesc.ToLower().Contains("do not use"));
            this.validatedItems.Add(new TestValidatedItemModelBuilder()
                .WithScanCode(identifierOne).WithBrandId(brand.IconBrandId).WithTaxClass(taxClass.TaxClassDesc).WithItemId(1).WithTare("25").Build());
            this.validatedItems.Add(new TestValidatedItemModelBuilder()
                .WithScanCode(identifierTwo).WithBrandId(brand.IconBrandId).WithTaxClass(taxClass.TaxClassDesc).WithProductDesccription("Test Validated Item2")
                .WithItemId(2).WithTare("30").Build());

            this.command.ValidatedItems = this.validatedItems;

            ValidatedItemModel validatedItemOne = this.validatedItems[0];
            ValidatedItemModel validatedItemTwo = this.validatedItems[1];

            List<Price> priceOneList = this.context.Price.Where(p => p.Item.ItemIdentifier.Any(ii => ii.Identifier == identifierOne) && (p.PosTare != null && p.PosTare != 0)).ToList();
            List<Price> priceTwoList = this.context.Price.Where(p => p.Item.ItemIdentifier.Any(ii => ii.Identifier == identifierTwo) && (p.PosTare != null && p.PosTare != 0)).ToList();

            // When
            this.handler.Handle(this.command);

            // Then
            // first item prices
            foreach (var price in priceOneList)
            {
                this.context.Entry(price).Reload();
                Assert.AreNotEqual(validatedItemOne.Tare, price.PosTare, "The Price.PosTare was updated, but it was not supposed to because there was already a value input by region.");
            }

            // second item prices
            foreach (var price in priceTwoList)
            {
                this.context.Entry(price).Reload();
                Assert.AreNotEqual(validatedItemTwo.Tare, price.PosTare, "The Price.PosTare was updated, but it was not supposed to because there was already a value input by region.");
            }
        }

        [TestMethod]
        public void BulkUpdateItem_PriceRowsOfValidatedItemHaveNullPosTareValue_PriceTareValuesAreUpdatedWithValidatedItemTareValues()
        {
            // Given
            string identifierOne = this.context.ItemIdentifier.First(ii => ii.Default_Identifier == 1
                    && ii.Deleted_Identifier == 0
                    && ii.Item.Retail_Sale == true
                    && ii.Item.Price
                        .Any(p => p.PosTare == null)).Identifier;
            string identifierTwo = this.context.ItemIdentifier.First(ii => ii.Default_Identifier == 1
                    && ii.Deleted_Identifier == 0
                    && ii.Item.Retail_Sale == true
                    && ii.Identifier != identifierOne
                    && ii.Item.Price
                        .Any(p => p.PosTare == null)).Identifier;

            ValidatedBrand brand = this.context.ValidatedBrand.First();
            TaxClass taxClass = this.context.TaxClass.First(tc => !tc.TaxClassDesc.ToLower().Contains("do not use"));
            this.validatedItems.Add(new TestValidatedItemModelBuilder()
                .WithScanCode(identifierOne).WithBrandId(brand.IconBrandId).WithTaxClass(taxClass.TaxClassDesc).WithItemId(1).WithTare("25").Build());
            this.validatedItems.Add(new TestValidatedItemModelBuilder()
                .WithScanCode(identifierTwo).WithBrandId(brand.IconBrandId).WithTaxClass(taxClass.TaxClassDesc).WithProductDesccription("Test Validated Item2")
                .WithItemId(2).WithTare("30").Build());

            this.command.ValidatedItems = this.validatedItems;

            ValidatedItemModel validatedItemOne = this.validatedItems[0];
            ValidatedItemModel validatedItemTwo = this.validatedItems[1];

            List<Price> priceOneList = this.context.Price.Where(p => p.Item.ItemIdentifier.Any(ii => ii.Identifier == identifierOne) && (p.PosTare == null)).ToList();
            List<Price> priceTwoList = this.context.Price.Where(p => p.Item.ItemIdentifier.Any(ii => ii.Identifier == identifierTwo) && (p.PosTare == null)).ToList();

            // When
            this.handler.Handle(this.command);

            // Then
            // first item prices
            foreach (var priceOne in priceOneList)
            {
                this.context.Entry(priceOne).Reload();
                Assert.AreEqual(Convert.ToInt32(validatedItemOne.Tare), priceOne.PosTare, "The Price.PosTare values were not updated as expected from the Icon validated data.");
            }

            // second item prices
            foreach (var priceTwo in priceTwoList)
            {
                this.context.Entry(priceTwo).Reload();
                Assert.AreEqual(Convert.ToInt32(validatedItemTwo.Tare), priceTwo.PosTare, "The Price.PosTare values were not updated as expected from the Icon validated data.");
            }
        }

        [TestMethod]
        public void BulkUpdateItem_PriceRowsOfValidatedItemHaveZeroPosTareValue_PriceTareValuesAreUpdatedWithValidatedItemTareValues()
        {
            // Given
            string identifierOne = this.context.ItemIdentifier.First(ii => ii.Default_Identifier == 1
                    && ii.Deleted_Identifier == 0
                    && ii.Item.Retail_Sale == true
                    && ii.Item.Price
                        .Any(p => p.PosTare == 0)).Identifier;
            string identifierTwo = this.context.ItemIdentifier.First(ii => ii.Default_Identifier == 1
                    && ii.Deleted_Identifier == 0
                    && ii.Item.Retail_Sale == true
                    && ii.Identifier != identifierOne
                    && ii.Item.Price
                        .Any(p => p.PosTare == 0)).Identifier;

            ValidatedBrand brand = this.context.ValidatedBrand.First();
            TaxClass taxClass = this.context.TaxClass.First(tc => !tc.TaxClassDesc.ToLower().Contains("do not use"));
            this.validatedItems.Add(new TestValidatedItemModelBuilder()
                .WithScanCode(identifierOne).WithBrandId(brand.IconBrandId).WithTaxClass(taxClass.TaxClassDesc).WithItemId(1).WithTare("25").Build());
            this.validatedItems.Add(new TestValidatedItemModelBuilder()
                .WithScanCode(identifierTwo).WithBrandId(brand.IconBrandId).WithTaxClass(taxClass.TaxClassDesc).WithProductDesccription("Test Validated Item2")
                .WithItemId(2).WithTare("30").Build());

            this.command.ValidatedItems = this.validatedItems;

            ValidatedItemModel validatedItemOne = this.validatedItems[0];
            ValidatedItemModel validatedItemTwo = this.validatedItems[1];

            List<Price> priceOneList = this.context.Price.Where(p => p.Item.ItemIdentifier.Any(ii => ii.Identifier == identifierOne) && (p.PosTare == 0)).ToList();
            List<Price> priceTwoList = this.context.Price.Where(p => p.Item.ItemIdentifier.Any(ii => ii.Identifier == identifierTwo) && (p.PosTare == 0)).ToList();

            // When
            this.handler.Handle(this.command);

            // Then
            // first item prices
            foreach (var priceOne in priceOneList)
            {
                this.context.Entry(priceOne).Reload();
                Assert.AreEqual(Convert.ToInt32(validatedItemOne.Tare), priceOne.PosTare, "The Price.PosTare values were not updated as expected from the Icon validated data.");
            }

            // second item prices
            foreach (var priceTwo in priceTwoList)
            {
                this.context.Entry(priceTwo).Reload();
                Assert.AreEqual(Convert.ToInt32(validatedItemTwo.Tare), priceTwo.PosTare, "The Price.PosTare values were not updated as expected from the Icon validated data.");
            }
        }

        [TestMethod]
        public void BulkUpdateItem_ValidatedItemDataHasTareValueOfZero_PriceTareValuesAreNotUpdatedWithValidatedItemTareValues()
        {
            // Given
            string identifierOne = this.context.ItemIdentifier.First(ii => ii.Default_Identifier == 1
                    && ii.Deleted_Identifier == 0
                    && ii.Item.Retail_Sale == true
                    && ii.Item.Price
                        .Any(p => p.PosTare != null && p.PosTare != 0)).Identifier;
            string identifierTwo = this.context.ItemIdentifier.First(ii => ii.Default_Identifier == 1
                    && ii.Deleted_Identifier == 0
                    && ii.Item.Retail_Sale == true
                    && ii.Identifier != identifierOne
                    && ii.Item.Price
                        .Any(p => p.PosTare != null && p.PosTare != 0)).Identifier;

            ValidatedBrand brand = this.context.ValidatedBrand.First();
            TaxClass taxClass = this.context.TaxClass.First(tc => !tc.TaxClassDesc.ToLower().Contains("do not use"));
            this.validatedItems.Add(new TestValidatedItemModelBuilder()
                .WithScanCode(identifierOne).WithBrandId(brand.IconBrandId).WithTaxClass(taxClass.TaxClassDesc).WithItemId(1).WithTare("0").Build());
            this.validatedItems.Add(new TestValidatedItemModelBuilder()
                .WithScanCode(identifierTwo).WithBrandId(brand.IconBrandId).WithTaxClass(taxClass.TaxClassDesc).WithProductDesccription("Test Validated Item2")
                .WithItemId(2).WithTare("0").Build());

            this.command.ValidatedItems = this.validatedItems;

            ValidatedItemModel validatedItemOne = this.validatedItems[0];
            ValidatedItemModel validatedItemTwo = this.validatedItems[1];

            List<Price> priceOneList = this.context.Price.Where(p => p.Item.ItemIdentifier.Any(ii => ii.Identifier == identifierOne) && (p.PosTare != null && p.PosTare != 0)).ToList();
            List<Price> priceTwoList = this.context.Price.Where(p => p.Item.ItemIdentifier.Any(ii => ii.Identifier == identifierTwo) && (p.PosTare != null && p.PosTare != 0)).ToList();

            // When
            this.handler.Handle(this.command);

            // Then
            // first item prices
            foreach (var priceOne in priceOneList)
            {
                this.context.Entry(priceOne).Reload();
                Assert.AreNotEqual(Convert.ToInt32(validatedItemOne.Tare), priceOne.PosTare, "The Price.PosTare values were not expected to be updated.");
            }

            // second item prices
            foreach (var priceTwo in priceTwoList)
            {
                this.context.Entry(priceTwo).Reload();
                Assert.AreNotEqual(Convert.ToInt32(validatedItemTwo.Tare), priceTwo.PosTare, "The Price.PosTare values were not expected to be updated.");
            }
        }

        [TestMethod]
        public void BulkUpdateItem_ValidatedItemDataHasNullTareValue_PriceTareValuesAreNotUpdatedWithValidatedItemTareValues()
        {
            // Given
            string identifierOne = this.context.ItemIdentifier.First(ii => ii.Default_Identifier == 1
                    && ii.Deleted_Identifier == 0
                    && ii.Item.Retail_Sale == true
                    && ii.Item.Price
                        .Any(p => p.PosTare != null && p.PosTare != 0)).Identifier;
            string identifierTwo = this.context.ItemIdentifier.First(ii => ii.Default_Identifier == 1
                    && ii.Deleted_Identifier == 0
                    && ii.Item.Retail_Sale == true
                    && ii.Identifier != identifierOne
                    && ii.Item.Price
                        .Any(p => p.PosTare != null && p.PosTare != 0)).Identifier;

            ValidatedBrand brand = this.context.ValidatedBrand.First();
            TaxClass taxClass = this.context.TaxClass.First(tc => !tc.TaxClassDesc.ToLower().Contains("do not use"));
            this.validatedItems.Add(new TestValidatedItemModelBuilder()
                .WithScanCode(identifierOne).WithBrandId(brand.IconBrandId).WithTaxClass(taxClass.TaxClassDesc).WithItemId(1).WithTare(null).Build());
            this.validatedItems.Add(new TestValidatedItemModelBuilder()
                .WithScanCode(identifierTwo).WithBrandId(brand.IconBrandId).WithTaxClass(taxClass.TaxClassDesc).WithProductDesccription("Test Validated Item2")
                .WithItemId(2).WithTare(null).Build());

            this.command.ValidatedItems = this.validatedItems;

            ValidatedItemModel validatedItemOne = this.validatedItems[0];
            ValidatedItemModel validatedItemTwo = this.validatedItems[1];

            List<Price> priceOneList = this.context.Price.Where(p => p.Item.ItemIdentifier.Any(ii => ii.Identifier == identifierOne) && (p.PosTare != null && p.PosTare != 0)).ToList();
            List<Price> priceTwoList = this.context.Price.Where(p => p.Item.ItemIdentifier.Any(ii => ii.Identifier == identifierTwo) && (p.PosTare != null && p.PosTare != 0)).ToList();

            // When
            this.handler.Handle(this.command);

            // Then
            // first item prices
            foreach (var priceOne in priceOneList)
            {
                this.context.Entry(priceOne).Reload();
                Assert.AreNotEqual(Convert.ToInt32(validatedItemOne.Tare), priceOne.PosTare, "The Price.PosTare values were not expected to be updated.");
            }

            // second item prices
            foreach (var priceTwo in priceTwoList)
            {
                this.context.Entry(priceTwo).Reload();
                Assert.AreNotEqual(Convert.ToInt32(validatedItemTwo.Tare), priceTwo.PosTare, "The Price.PosTare values were not expected to be updated.");
            }
        }

        [TestMethod]
        public void BulkUpdateItem_ValidatedItemDataHasTareValueThatIsNotGreaterThanZeroWhenConvertedToDecimalThenInt_PriceTareValuesAreNotUpdatedWithValidatedItemTareValues()
        {
            // Given
            string identifierOne = this.context.ItemIdentifier.First(ii => ii.Default_Identifier == 1
                    && ii.Deleted_Identifier == 0
                    && ii.Item.Retail_Sale == true
                    && ii.Item.Price
                        .Any(p => p.PosTare != null && p.PosTare != 0)).Identifier;
            string identifierTwo = this.context.ItemIdentifier.First(ii => ii.Default_Identifier == 1
                    && ii.Deleted_Identifier == 0
                    && ii.Item.Retail_Sale == true
                    && ii.Identifier != identifierOne
                    && ii.Item.Price
                        .Any(p => p.PosTare != null && p.PosTare != 0)).Identifier;

            ValidatedBrand brand = this.context.ValidatedBrand.First();
            TaxClass taxClass = this.context.TaxClass.First(tc => !tc.TaxClassDesc.ToLower().Contains("do not use"));
            this.validatedItems.Add(new TestValidatedItemModelBuilder()
                .WithScanCode(identifierOne).WithBrandId(brand.IconBrandId).WithTaxClass(taxClass.TaxClassDesc).WithItemId(1).WithTare("0.001").Build());
            this.validatedItems.Add(new TestValidatedItemModelBuilder()
                .WithScanCode(identifierTwo).WithBrandId(brand.IconBrandId).WithTaxClass(taxClass.TaxClassDesc).WithProductDesccription("Test Validated Item2")
                .WithItemId(2).WithTare("0.001").Build());

            this.command.ValidatedItems = this.validatedItems;

            ValidatedItemModel validatedItemOne = this.validatedItems[0];
            ValidatedItemModel validatedItemTwo = this.validatedItems[1];

            List<Price> priceOneList = this.context.Price.Where(p => p.Item.ItemIdentifier.Any(ii => ii.Identifier == identifierOne) && (p.PosTare != null && p.PosTare != 0)).ToList();
            List<Price> priceTwoList = this.context.Price.Where(p => p.Item.ItemIdentifier.Any(ii => ii.Identifier == identifierTwo) && (p.PosTare != null && p.PosTare != 0)).ToList();

            // When
            this.handler.Handle(this.command);

            // Then
            // first item prices
            foreach (var priceOne in priceOneList)
            {
                this.context.Entry(priceOne).Reload();
                Assert.AreNotEqual(validatedItemOne.Tare, priceOne.PosTare.ToString(), "The Price.PosTare values were not expected to be updated.");
            }

            // second item prices
            foreach (var priceTwo in priceTwoList)
            {
                this.context.Entry(priceTwo).Reload();
                Assert.AreNotEqual(validatedItemTwo.Tare, priceTwo.PosTare.ToString(), "The Price.PosTare values were not expected to be updated.");
            }
        }

        [TestMethod]
        public void BulkUpdateItem_RetailSizeAndRetailUomHasChangedInIcon_ShouldUpdateRetailSizeAndRetailUomInIrma()
        {
            //Given
            string identifierOne = this.context.ItemIdentifier
                .First(ii => ii.Default_Identifier == 1 && ii.Deleted_Identifier == 0 && ii.Item.Retail_Sale == true).Identifier;
            string identifierTwo = this.context.ItemIdentifier
                .First(ii => ii.Default_Identifier == 1 && ii.Deleted_Identifier == 0 && ii.Item.Retail_Sale == true && ii.Identifier != identifierOne).Identifier;

            ValidatedBrand brand = this.context.ValidatedBrand.First();
            TaxClass taxClass = this.context.TaxClass.First(tc => !tc.TaxClassDesc.ToLower().Contains("do not use"));
            this.validatedItems.Add(new TestValidatedItemModelBuilder()
                .WithScanCode(identifierOne).WithBrandId(brand.IconBrandId).WithTaxClass(taxClass.TaxClassDesc).WithItemId(1).WithTare(null).Build());
            this.validatedItems.Add(new TestValidatedItemModelBuilder()
                .WithScanCode(identifierTwo).WithBrandId(brand.IconBrandId).WithTaxClass(taxClass.TaxClassDesc).WithProductDesccription("Test Validated Item2").WithNationalClassCode("5").WithItemId(2).Build());


            this.command.ValidatedItems = this.validatedItems;

            ValidatedItemModel validatedItemOne = this.validatedItems[0];
            ValidatedItemModel validatedItemTwo = this.validatedItems[1];

            //When
            this.handler.Handle(this.command);

            //Then
            var actualItemOne = context.Item.First(i => i.ItemIdentifier.Any(ii => ii.Identifier == identifierOne));
            var actualItemTwo = context.Item.First(i => i.ItemIdentifier.Any(ii => ii.Identifier == identifierTwo));

            Assert.AreEqual(validatedItemOne.RetailSize, actualItemOne.Package_Desc2, "The Package_Desc2 does not match the expected value.");
            Assert.AreEqual(validatedItemOne.RetailUom, this.context.ItemUnit.First(i => i.Unit_ID == actualItemOne.Package_Unit_ID).Unit_Abbreviation, "The Package_Unit_ID does not match the expected value.");
            Assert.AreEqual(validatedItemTwo.RetailSize, actualItemTwo.Package_Desc2, "The Package_Desc2 does not match the expected value.");
            Assert.AreEqual(validatedItemTwo.RetailUom, this.context.ItemUnit.First(i => i.Unit_ID == actualItemTwo.Package_Unit_ID).Unit_Abbreviation, "The Package_Unit_ID does not match the expected value.");
        }

        [TestMethod]
        public void BulkUpdateItem_IrmaRetailUomIsPoundAndIrmaRetailUnitIsNotPoundAndIconRetailUomIsNotPound_ShouldUpdateIrmaRetailUomToIconRetailUomAndRetailUnitToEach()
        {
            // Given
            ItemUnit poundUnit = this.context.ItemUnit.First(iu => iu.Unit_Abbreviation == "LB");
            if (poundUnit == null)
            {
                poundUnit = new ItemUnit { IsPackageUnit = true, Unit_Abbreviation = "LB", Weight_Unit = true, Unit_Name = "POUND" };
                this.context.ItemUnit.Add(poundUnit);
                this.context.SaveChanges();
            }

            ItemUnit eachUnit = this.context.ItemUnit.First(iu => iu.Unit_Abbreviation == "EA");
            if (eachUnit == null)
            {
                eachUnit = new ItemUnit { IsPackageUnit = true, Unit_Abbreviation = "EA", Weight_Unit = false, Unit_Name = "EACH" };
                this.context.ItemUnit.Add(eachUnit);
                this.context.SaveChanges();
            }

            ItemUnit otherItemUnit = this.context.ItemUnit.First(iu => iu.Unit_Abbreviation == "FZ");
            if (eachUnit == null)
            {
                eachUnit = new ItemUnit { IsPackageUnit = true, Unit_Abbreviation = "FZ", Weight_Unit = false, Unit_Name = "FLUID OUNCES" };
                this.context.ItemUnit.Add(eachUnit);
            }
            this.context.SaveChanges();

            Item item = new TestIrmaDbItemBuilder(context)
                .WithPackage_Unit_ID(poundUnit.Unit_ID)
                .WithRetail_Unit_ID(eachUnit.Unit_ID);

            this.context.Item.Add(item);
            this.context.SaveChanges();

            ItemIdentifier itemIdentifier = new TestItemIdentifierBuilder()
                .WithIdentifier("757575758484")
                .WithItem_Key(item.Item_Key)
                .WithDefault_Identifier(1)
                .Build();
            this.context.ItemIdentifier.Add(itemIdentifier);

            context.ValidatedBrand.Add(new ValidatedBrand { IrmaBrandId = item.Brand_ID ?? 0, IconBrandId = 777 });
            this.context.SaveChanges();

            string iconRetailUom = otherItemUnit.Unit_Abbreviation;
            this.validatedItems.Add(new TestValidatedItemModelBuilder().WithScanCode(itemIdentifier.Identifier).WithRetailUom(iconRetailUom).WithDeptNo(42).WithBrandId(777).Build());

            this.command = new BulkUpdateItemCommand { ValidatedItems = this.validatedItems };

            context.Database.ExecuteSqlCommand("update instanceDataFlags SET FlagValue = 1 WHERE FlagKey = 'EnableIconRetailUomSizeUpdates'");
            // When
            this.handler.Handle(this.command);

            // Then
            this.context.Entry<Item>(item).Reload();

            Assert.AreEqual(eachUnit.Unit_ID, item.Retail_Unit_ID);
            Assert.AreEqual(otherItemUnit.Unit_ID, item.Package_Unit_ID);
        }

        [TestMethod]
        public void BulkUpdateItem_EnableIconRetailUomSizeUpdatesInstanceDataFlagsIsTrue_ShouldUpdateIrmaPackageDesc2AndPackageUnitIdToIconValues()
        {
            // Given
            context.Database.ExecuteSqlCommand("Update InstanceDataFlags SET FlagValue = 1 WHERE FlagKey = 'EnableIconRetailUomSizeUpdates'");

            ItemUnit poundUnit = CreateItemUnit("POUND", "LB");
            ItemUnit eachUnit = CreateItemUnit("EACH", "EA");
            ItemUnit ouncesUnit = CreateItemUnit("OUNCES", "OZ");

            Item item = new TestIrmaDbItemBuilder(this.context)
                .WithPackage_Unit_ID(eachUnit.Unit_ID)
                .WithRetail_Unit_ID(eachUnit.Unit_ID);
            this.context.Item.Add(item);
            this.context.SaveChanges();

            ItemIdentifier itemIdentifier = new TestItemIdentifierBuilder()
                .WithDefault_Identifier(1)
                .WithDeleted_Identifier(0)
                .WithIdentifier("757575758484")
                .WithItem_Key(item.Item_Key)
                .Build();
            this.context.ItemIdentifier.Add(itemIdentifier);
            this.context.SaveChanges();

            ValidatedBrand validatedBrand = new ValidatedBrand { IrmaBrandId = item.Brand_ID ?? 0, IconBrandId = 777 };
            context.ValidatedBrand.Add(validatedBrand);
            this.context.SaveChanges();

            ValidatedItemModel validatedItem = new TestValidatedItemModelBuilder()
                .WithScanCode(itemIdentifier.Identifier)
                .WithRetailUom(ouncesUnit.Unit_Abbreviation)
                .WithRetailSize(75)
                .WithDeptNo(42)
                .WithBrandId(validatedBrand.IconBrandId);
            this.validatedItems.Add(validatedItem);

            this.command = new BulkUpdateItemCommand { ValidatedItems = this.validatedItems };

            decimal expectedPackageDesc2 = validatedItem.RetailSize;
            int expectedUnitId = ouncesUnit.Unit_ID;

            // When
            this.handler.Handle(this.command);

            // Then
            this.context.Entry<Item>(item).Reload();
            Assert.AreEqual(expectedPackageDesc2, item.Package_Desc2);
            Assert.AreEqual(expectedUnitId, item.Package_Unit_ID);
        }

        [TestMethod]
        public void BulkUpdateItem_EnableIconRetailUomSizeUpdatesInstanceDataFlagsIsFalse_ShouldNotUpdateIrmaPackageDesc2AndPackageUnitIdToIconValues()
        {
            // Given
            context.Database.ExecuteSqlCommand("Update InstanceDataFlags SET FlagValue = 0 WHERE FlagKey = 'EnableIconRetailUomSizeUpdates'");
            ItemUnit poundUnit = CreateItemUnit("POUND", "LB");
            ItemUnit eachUnit = CreateItemUnit("EACH", "EA");
            ItemUnit ouncesUnit = CreateItemUnit("OUNCES", "OZ");

            decimal expectedPackageDesc2 = 345;
            int expectedUnitId = eachUnit.Unit_ID;

            Item item = new TestIrmaDbItemBuilder(this.context)
                .WithPackage_Unit_ID(expectedUnitId)
                .WithRetail_Unit_ID(eachUnit.Unit_ID)
                .WithPackage_Desc2(expectedPackageDesc2);
            this.context.Item.Add(item);
            this.context.SaveChanges();

            ItemIdentifier itemIdentifier = new TestItemIdentifierBuilder()
                .WithDefault_Identifier(1)
                .WithDeleted_Identifier(0)
                .WithIdentifier("757575758484")
                .WithItem_Key(item.Item_Key)
                .Build();
            this.context.ItemIdentifier.Add(itemIdentifier);
            this.context.SaveChanges();

            ValidatedBrand validatedBrand = new ValidatedBrand { IrmaBrandId = item.Brand_ID ?? 0, IconBrandId = 777 };
            context.ValidatedBrand.Add(validatedBrand);
            this.context.SaveChanges();

            this.validatedItems.Add(new TestValidatedItemModelBuilder()
                .WithScanCode(itemIdentifier.Identifier)
                .WithRetailUom(ouncesUnit.Unit_Abbreviation)
                .WithRetailSize(75)
                .WithDeptNo(42)
                .WithBrandId(validatedBrand.IconBrandId));

            this.command = new BulkUpdateItemCommand { ValidatedItems = this.validatedItems };

            // When
            this.handler.Handle(this.command);

            // Then
            this.context.Entry<Item>(item).Reload();
            Assert.AreEqual(expectedPackageDesc2, item.Package_Desc2);
            Assert.AreEqual(expectedUnitId, item.Package_Unit_ID);
        }

        [TestMethod]
        public void BulkUpdateItem_IrmaRetailUomIsTheSameAsIconRetailUom_ShouldNotUpdateRetailUnit()
        {
            // Given
            ItemUnit poundUnit = CreateItemUnit("POUND", "LB");
            ItemUnit eachUnit = CreateItemUnit("EACH", "EA");

            Item item = new TestIrmaDbItemBuilder(this.context)
                .WithPackage_Unit_ID(poundUnit.Unit_ID)
                .WithRetail_Unit_ID(eachUnit.Unit_ID)
                .Build();
            this.context.Item.Add(item);
            this.context.SaveChanges();

            ItemIdentifier itemIdentifier = new TestItemIdentifierBuilder()
                .WithDefault_Identifier(1)
                .WithIdentifier("757575758484")
                .WithItem_Key(item.Item_Key);
            this.context.ItemIdentifier.Add(itemIdentifier);
            this.context.SaveChanges();

            ValidatedBrand validatedBrand = new ValidatedBrand { IrmaBrandId = item.Brand_ID ?? 0, IconBrandId = 777 };
            context.ValidatedBrand.Add(validatedBrand);
            this.context.SaveChanges();

            ValidatedItemModel validatedItem = new TestValidatedItemModelBuilder()
                .WithRetailUom(poundUnit.Unit_Abbreviation)
                .WithScanCode("757575758484")
                .WithDeptNo(42)
                .WithBrandId(777);
            this.validatedItems.Add(validatedItem);

            this.command = new BulkUpdateItemCommand { ValidatedItems = this.validatedItems };

            int expectedItemUnitId = eachUnit.Unit_ID;

            // When
            this.handler.Handle(this.command);

            // Then
            this.context.Entry<Item>(item).Reload();
            Assert.AreEqual(expectedItemUnitId, item.Retail_Unit_ID);
        }

        [TestMethod]
        public void BulkUpdateItem_IrmaRetailUomChangesButNotToOrFromPoundAndIrmaRetailUnitIsPoundAndIconRetailUomIsNotPound_ShouldNotUpdateRetailUnit()
        {
            // Given
            ItemUnit poundUnit = CreateItemUnit("POUND", "LB");
            ItemUnit eachUnit = CreateItemUnit("EACH", "EA");
            ItemUnit ouncesUnit = CreateItemUnit("OUNCES", "OZ");

            Item item = new TestIrmaDbItemBuilder(this.context)
                .WithPackage_Unit_ID(eachUnit.Unit_ID)
                .WithRetail_Unit_ID(poundUnit.Unit_ID)
                .Build();
            this.context.Item.Add(item);
            this.context.SaveChanges();

            ItemIdentifier itemIdentifier = new TestItemIdentifierBuilder()
                .WithDefault_Identifier(1)
                .WithIdentifier("757575758484")
                .WithItem_Key(item.Item_Key);
            this.context.ItemIdentifier.Add(itemIdentifier);
            this.context.SaveChanges();

            ValidatedBrand validatedBrand = new ValidatedBrand { IrmaBrandId = item.Brand_ID ?? 0, IconBrandId = 777 };
            context.ValidatedBrand.Add(validatedBrand);
            this.context.SaveChanges();

            ValidatedItemModel validatedItem = new TestValidatedItemModelBuilder()
                .WithRetailUom(ouncesUnit.Unit_Abbreviation)
                .WithScanCode("757575758484")
                .WithDeptNo(42)
                .WithBrandId(777);
            this.validatedItems.Add(validatedItem);

            this.command = new BulkUpdateItemCommand { ValidatedItems = this.validatedItems };

            int expectedItemUnitId = poundUnit.Unit_ID;

            // When
            this.handler.Handle(this.command);

            // Then
            this.context.Entry<Item>(item).Reload();
            Assert.AreEqual(expectedItemUnitId, item.Retail_Unit_ID);
        }

        [TestMethod]
        public void BulkUpdateItem_IrmaRetailUomAndRetailUnitArePoundAndIconRetailUomIsNotPound_ShouldUpdateRetailUnitIdToEach()
        {
            // Given
            ItemUnit poundUnit = CreateItemUnit("POUND", "LB");
            ItemUnit eachUnit = CreateItemUnit("EACH", "EA");

            Item item = new TestIrmaDbItemBuilder(this.context)
                .WithRetail_Unit_ID(poundUnit.Unit_ID)
                .WithPackage_Unit_ID(poundUnit.Unit_ID)
                .Build();
            this.context.Item.Add(item);
            this.context.SaveChanges();

            ItemIdentifier itemIdentifier = new TestItemIdentifierBuilder()
                .WithDefault_Identifier(1)
                .WithIdentifier("757575758484")
                .WithItem_Key(item.Item_Key);
            this.context.ItemIdentifier.Add(itemIdentifier);
            this.context.SaveChanges();

            ValidatedBrand validatedBrand = new ValidatedBrand { IrmaBrandId = item.Brand_ID ?? 0, IconBrandId = 777 };
            context.ValidatedBrand.Add(validatedBrand);
            this.context.SaveChanges();

            ValidatedItemModel validatedItem = new TestValidatedItemModelBuilder()
                .WithRetailUom(eachUnit.Unit_Abbreviation)
                .WithScanCode("757575758484")
                .WithDeptNo(42)
                .WithBrandId(777);
            this.validatedItems.Add(validatedItem);

            this.command = new BulkUpdateItemCommand { ValidatedItems = this.validatedItems };

            int expectedItemUnitId = eachUnit.Unit_ID;

            // When
            this.handler.Handle(this.command);

            // Then
            this.context.Entry<Item>(item).Reload();
            Assert.AreEqual(expectedItemUnitId, item.Retail_Unit_ID);
        }

        [TestMethod]
        public void BulkUpdateItem_IrmaRetailUomAndRetailUnitIsNotPoundAndIconRetailUomIsPound_ShouldUpdateRetailUnitIdToPound()
        {
            // Given
            ItemUnit poundUnit = CreateItemUnit("POUND", "LB");
            ItemUnit eachUnit = CreateItemUnit("EACH", "EA");

            Item item = new TestIrmaDbItemBuilder(this.context)
                .WithRetail_Unit_ID(eachUnit.Unit_ID)
                .WithPackage_Unit_ID(eachUnit.Unit_ID)
                .Build();
            this.context.Item.Add(item);
            this.context.SaveChanges();

            ItemIdentifier itemIdentifier = new TestItemIdentifierBuilder()
                .WithDefault_Identifier(1)
                .WithIdentifier("757575758484")
                .WithItem_Key(item.Item_Key);
            this.context.ItemIdentifier.Add(itemIdentifier);
            this.context.SaveChanges();

            ValidatedBrand validatedBrand = new ValidatedBrand { IrmaBrandId = item.Brand_ID ?? 0, IconBrandId = 777 };
            context.ValidatedBrand.Add(validatedBrand);
            this.context.SaveChanges();

            ValidatedItemModel validatedItem = new TestValidatedItemModelBuilder()
                .WithRetailUom(poundUnit.Unit_Abbreviation)
                .WithScanCode("757575758484")
                .WithDeptNo(42)
                .WithBrandId(777);
            this.validatedItems.Add(validatedItem);

            this.command = new BulkUpdateItemCommand { ValidatedItems = this.validatedItems };

            int expectedItemUnitId = poundUnit.Unit_ID;

            // When
            this.handler.Handle(this.command);

            // Then
            this.context.Entry<Item>(item).Reload();
            Assert.AreEqual(expectedItemUnitId, item.Retail_Unit_ID);
        }

        private ItemUnit CreateItemUnit(string unitName, string unitAbbreviation)
        {
            ItemUnit itemUnit = this.context.ItemUnit.First(iu => iu.Unit_Abbreviation == unitAbbreviation);
            if (itemUnit == null)
            {
                itemUnit = new ItemUnit { IsPackageUnit = true, Unit_Abbreviation = unitAbbreviation, Weight_Unit = false, Unit_Name = unitName };
                this.context.ItemUnit.Add(itemUnit);
                this.context.SaveChanges();
            }
            return itemUnit;
        }
    }
}

