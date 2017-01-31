using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class AddProductMessageCommandHandlerTests
    {
        private AddProductMessageCommandHandler commandHandler;
        private IconContext context;
        private DbContextTransaction transaction;
        private Item testItem;
        private ScanCode testScanCode;
        private HierarchyClass testMerchandiseClass;
        private HierarchyClass testBrandClass;
        private HierarchyClass testBrowsingClass;
        private HierarchyClass testTaxClass;
        private HierarchyClass testFinancialClass;
        private HierarchyClass testNationalClass;
        private HierarchyClass testKosherClass;
        private HierarchyClassTrait testFinancialTrait;
        private HierarchyClassTrait testKosherTrait;
        private ItemNutrition itemNutrition;
        private ItemSignAttribute itemSignAttribute;

        [TestInitialize]
        public void InitializeData()
        {
            context = new IconContext();
            commandHandler = new AddProductMessageCommandHandler(context);
            transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.transaction.Rollback();
        }

        [TestMethod]
        public void AddProductMessage_ItemIsValidated_MessageShouldBeCreated()
        {
            // Given.
            SetupProductMessageTestData();

            int expectedId = testItem.itemID;
            string expectedProductDescription = testItem.ItemTrait.First(it => it.traitID == Traits.ProductDescription).traitValue;
            string expectedPosDescription = testItem.ItemTrait.First(it => it.traitID == Traits.PosDescription).traitValue;
            string expectedPackageUnit = testItem.ItemTrait.First(it => it.traitID == Traits.PackageUnit).traitValue;
            string expectedFoodStamp = testItem.ItemTrait.First(it => it.traitID == Traits.FoodStampEligible).traitValue;
            string expectedPosScaleTare = testItem.ItemTrait.First(it => it.traitID == Traits.PosScaleTare).traitValue;
            string expectedRetailSize = testItem.ItemTrait.First(it => it.traitID == Traits.RetailSize).traitValue;
            string expectedRetailUom = testItem.ItemTrait.First(it => it.traitID == Traits.RetailUom).traitValue;
            string expectedDepartmentSale = "1";

            var expectedBrand = context.HierarchyClass.Single(hc => hc.ItemHierarchyClass.Any(ihc => ihc.itemID == testItem.itemID) && hc.Hierarchy.hierarchyName == HierarchyNames.Brands);
            var expectedMerch = context.HierarchyClass.Single(hc => hc.ItemHierarchyClass.Any(ihc => ihc.itemID == testItem.itemID) && hc.Hierarchy.hierarchyName == HierarchyNames.Merchandise);
            var expectedTax = context.HierarchyClass.Single(hc => hc.ItemHierarchyClass.Any(ihc => ihc.itemID == testItem.itemID) && hc.Hierarchy.hierarchyName == HierarchyNames.Tax);
            var subTeam = context.HierarchyClass.Single(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Merchandise && hc.ItemHierarchyClass.Any(ihc => ihc.itemID == testItem.itemID))
                .HierarchyClassTrait.Single(hct => hct.traitID == Traits.MerchFinMapping);
            var expectedFinancial = context.HierarchyClass.Single(fin => fin.hierarchyClassName == subTeam.traitValue);
            string expectedFinancialName = expectedFinancial.hierarchyClassName;
            string expectedFinancialId = expectedFinancial.hierarchyClassName.Split('(')[1].Trim(')') == "0000" ? "na" : expectedFinancial.hierarchyClassName.Split('(')[1].Trim(')');
            // When.
            commandHandler.Execute(new AddProductMessageCommand { ItemId = testItem.itemID });

            // Then.
            var result = context.MessageQueueProduct.Where(mq => mq.ItemId == testItem.itemID && mq.MessageStatusId == MessageStatusTypes.Staged).OrderBy(mq => mq.MessageQueueId).ToList();
            var actualMessage = result[0];
            var actualNutritionMessage = context.MessageQueueNutrition.Where(mqn => mqn.MessageQueueProduct.MessageQueueId == actualMessage.MessageQueueId).FirstOrDefault();
            
            Assert.AreEqual(testItem.itemID, actualMessage.ItemId);
            Assert.AreEqual(MessageTypes.Product, actualMessage.MessageTypeId);
            Assert.AreEqual(MessageStatusTypes.Staged, actualMessage.MessageStatusId);
            Assert.AreEqual(testItem.ScanCode.First().scanCode, actualMessage.ScanCode);
            Assert.AreEqual(testItem.ScanCode.First().scanCodeID, actualMessage.ScanCodeId);
            Assert.AreEqual(testItem.ScanCode.First().scanCodeTypeID, actualMessage.ScanCodeTypeId);
            Assert.AreEqual(testItem.ScanCode.First().ScanCodeType.scanCodeTypeDesc, actualMessage.ScanCodeTypeDesc);

            // Item Traits.
            Assert.AreEqual(expectedProductDescription, actualMessage.ProductDescription);
            Assert.AreEqual(expectedPosDescription, actualMessage.PosDescription);
            Assert.AreEqual(expectedPackageUnit, actualMessage.PackageUnit);
            Assert.AreEqual(expectedFoodStamp, actualMessage.FoodStampEligible);
            Assert.AreEqual(expectedDepartmentSale, actualMessage.DepartmentSale);
            Assert.AreEqual(expectedRetailSize, actualMessage.RetailSize);
            Assert.AreEqual(expectedRetailUom, actualMessage.RetailUom);

            // Merchandise.
            Assert.AreEqual(expectedMerch.hierarchyClassID, actualMessage.MerchandiseClassId);
            Assert.AreEqual(expectedMerch.hierarchyClassName, actualMessage.MerchandiseClassName);
            Assert.AreEqual(expectedMerch.hierarchyLevel, actualMessage.MerchandiseLevel);
            Assert.AreEqual(expectedMerch.hierarchyParentClassID, actualMessage.MerchandiseParentId);

            // Tax.
            Assert.AreEqual(expectedTax.hierarchyClassID, actualMessage.TaxClassId);
            Assert.AreEqual(expectedTax.hierarchyClassName, actualMessage.TaxClassName);
            Assert.AreEqual(expectedTax.hierarchyLevel, actualMessage.TaxLevel);
            Assert.AreEqual(expectedTax.hierarchyParentClassID, actualMessage.TaxParentId);

            // Brand.
            Assert.AreEqual(expectedBrand.hierarchyClassID, actualMessage.BrandId);
            Assert.AreEqual(expectedBrand.hierarchyClassName, actualMessage.BrandName);
            Assert.AreEqual(expectedBrand.hierarchyLevel, actualMessage.BrandLevel);
            Assert.AreEqual(expectedBrand.hierarchyParentClassID, actualMessage.BrandParentId);

            // Browsing.
            Assert.IsNull(actualMessage.BrowsingClassId);
            Assert.IsNull(actualMessage.BrowsingClassName);
            Assert.IsNull(actualMessage.BrowsingLevel);
            Assert.IsNull(actualMessage.BrowsingParentId);

            // Financial.
            Assert.AreEqual(expectedFinancialId, actualMessage.FinancialClassId);
            Assert.AreEqual(expectedFinancialName, actualMessage.FinancialClassName);
            Assert.AreEqual(expectedFinancial.hierarchyLevel, actualMessage.FinancialLevel);
            Assert.AreEqual(expectedFinancial.hierarchyParentClassID, actualMessage.FinancialParentId);

            Assert.IsNull(actualMessage.InProcessBy);
            Assert.IsNull(actualMessage.ProcessedDate);

            //SLAW attributes
            Assert.AreEqual(itemSignAttribute.Biodynamic, (actualMessage.Biodynamic == "1" ? true : false));
            Assert.AreEqual(itemSignAttribute.CheeseRaw, (actualMessage.CheeseRaw == "1" ? true : false));
            Assert.AreEqual(itemSignAttribute.MadeInHouse, (actualMessage.MadeInHouse == "1" ? true : false));
            Assert.AreEqual(itemSignAttribute.GrassFed, (actualMessage.GrassFed == "1" ? true : false));
            Assert.AreEqual(itemSignAttribute.Vegetarian, (actualMessage.Vegetarian == "1" ? true : false));
            Assert.AreEqual(testKosherClass.hierarchyClassName, actualMessage.KosherAgency);
            Assert.AreEqual(itemSignAttribute.PremiumBodyCare, (actualMessage.PremiumBodyCare == "1" ? true : false));
            Assert.AreEqual(itemSignAttribute.Msc, (actualMessage.Msc == "1" ? true : false));
            Assert.AreEqual(itemSignAttribute.AirChilled, (actualMessage.AirChilled == "1" ? true : false));
            Assert.AreEqual(itemSignAttribute.DryAged, (actualMessage.DryAged == "1" ? true : false));
            Assert.AreEqual(itemSignAttribute.FreeRange, (actualMessage.FreeRange == "1" ? true : false));
            Assert.AreEqual(itemSignAttribute.PastureRaised, (actualMessage.PastureRaised == "1" ? true : false));
            Assert.AreEqual(itemSignAttribute.WholeTrade, (actualMessage.WholeTrade == "1" ? true : false));

            //Nutritional
            Assert.AreEqual(itemNutrition.Plu, actualNutritionMessage.Plu);
            Assert.AreEqual(itemNutrition.ServingUnits, actualNutritionMessage.ServingUnits);
            Assert.AreEqual(itemNutrition.RecipeName, actualNutritionMessage.RecipeName);
            Assert.AreEqual(itemNutrition.HshRating, actualNutritionMessage.HshRating);
            Assert.AreEqual(itemNutrition.VitaminA, actualNutritionMessage.VitaminA);
        }

        [TestMethod]
        public void AddProductMessage_NonRetailItemIsValidated_MessageShouldBeCreated()
        {
            // Given.
            SetupNonRetailProductMessageTestData();

            int expectedId = testItem.itemID;
            string expectedProductDescription = testItem.ItemTrait.First(it => it.traitID == Traits.ProductDescription).traitValue;
            string expectedPosDescription = testItem.ItemTrait.First(it => it.traitID == Traits.PosDescription).traitValue;
            string expectedPackageUnit = testItem.ItemTrait.First(it => it.traitID == Traits.PackageUnit).traitValue;
            string expectedFoodStamp = testItem.ItemTrait.First(it => it.traitID == Traits.FoodStampEligible).traitValue;
            string expectedPosScaleTare = testItem.ItemTrait.First(it => it.traitID == Traits.PosScaleTare).traitValue;
            string expectedRetailSize = testItem.ItemTrait.First(it => it.traitID == Traits.RetailSize).traitValue;
            string expectedRetailUom = testItem.ItemTrait.First(it => it.traitID == Traits.RetailUom).traitValue;
            string expectedDepartmentSale = "1";

            var expectedBrand = context.HierarchyClass.Single(hc => hc.ItemHierarchyClass.Any(ihc => ihc.itemID == testItem.itemID) && hc.Hierarchy.hierarchyName == HierarchyNames.Brands);
            var expectedMerch = context.HierarchyClass.Single(hc => hc.ItemHierarchyClass.Any(ihc => ihc.itemID == testItem.itemID) && hc.Hierarchy.hierarchyName == HierarchyNames.Merchandise);
            var expectedTax = context.HierarchyClass.Single(hc => hc.ItemHierarchyClass.Any(ihc => ihc.itemID == testItem.itemID) && hc.Hierarchy.hierarchyName == HierarchyNames.Tax);
            var subTeam = context.HierarchyClass.Single(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Merchandise && hc.ItemHierarchyClass.Any(ihc => ihc.itemID == testItem.itemID))
                .HierarchyClassTrait.Single(hct => hct.traitID == Traits.MerchFinMapping);
            var expectedFinancial = context.HierarchyClass.Single(fin => fin.hierarchyClassName == subTeam.traitValue);
            string expectedFinancialId = expectedFinancial.hierarchyClassName.Split('(')[1].Trim(')') == "0000" ? "na" : expectedFinancial.hierarchyClassName.Split('(')[1].Trim(')');
            string expectedFinancialName = expectedFinancial.hierarchyClassName.ToString();

            // When.
            commandHandler.Execute(new AddProductMessageCommand { ItemId = testItem.itemID });

            // Then.
            var result = context.MessageQueueProduct.Where(mq => mq.ItemId == testItem.itemID && mq.MessageStatusId == MessageStatusTypes.Staged).OrderBy(mq => mq.MessageQueueId).ToList();
            var actualMessage = result[0];
            var actualNutritionMessage = context.MessageQueueNutrition.Where(mqn => mqn.MessageQueueProduct.MessageQueueId == actualMessage.MessageQueueId).FirstOrDefault();

            Assert.AreEqual(testItem.itemID, actualMessage.ItemId);
            Assert.AreEqual(MessageTypes.Product, actualMessage.MessageTypeId);
            Assert.AreEqual(MessageStatusTypes.Staged, actualMessage.MessageStatusId);
            Assert.AreEqual(testItem.ScanCode.First().scanCode, actualMessage.ScanCode);
            Assert.AreEqual(testItem.ScanCode.First().scanCodeID, actualMessage.ScanCodeId);
            Assert.AreEqual(testItem.ScanCode.First().scanCodeTypeID, actualMessage.ScanCodeTypeId);
            Assert.AreEqual(testItem.ScanCode.First().ScanCodeType.scanCodeTypeDesc, actualMessage.ScanCodeTypeDesc);

            // Item Traits.
            Assert.AreEqual(expectedProductDescription, actualMessage.ProductDescription);
            Assert.AreEqual(expectedPosDescription, actualMessage.PosDescription);
            Assert.AreEqual(expectedPackageUnit, actualMessage.PackageUnit);
            Assert.AreEqual(expectedFoodStamp, actualMessage.FoodStampEligible);
            Assert.AreEqual(expectedDepartmentSale, actualMessage.DepartmentSale);
            Assert.AreEqual(expectedRetailSize, actualMessage.RetailSize);
            Assert.AreEqual(expectedRetailUom, actualMessage.RetailUom);

            // Merchandise.
            Assert.AreEqual(expectedMerch.hierarchyClassID, actualMessage.MerchandiseClassId);
            Assert.AreEqual(expectedMerch.hierarchyClassName, actualMessage.MerchandiseClassName);
            Assert.AreEqual(expectedMerch.hierarchyLevel, actualMessage.MerchandiseLevel);
            Assert.AreEqual(expectedMerch.hierarchyParentClassID, actualMessage.MerchandiseParentId);

            // Tax.
            Assert.AreEqual(expectedTax.hierarchyClassID, actualMessage.TaxClassId);
            Assert.AreEqual(expectedTax.hierarchyClassName, actualMessage.TaxClassName);
            Assert.AreEqual(expectedTax.hierarchyLevel, actualMessage.TaxLevel);
            Assert.AreEqual(expectedTax.hierarchyParentClassID, actualMessage.TaxParentId);

            // Brand.
            Assert.AreEqual(expectedBrand.hierarchyClassID, actualMessage.BrandId);
            Assert.AreEqual(expectedBrand.hierarchyClassName, actualMessage.BrandName);
            Assert.AreEqual(expectedBrand.hierarchyLevel, actualMessage.BrandLevel);
            Assert.AreEqual(expectedBrand.hierarchyParentClassID, actualMessage.BrandParentId);

            // Browsing.
            Assert.IsNull(actualMessage.BrowsingClassId);
            Assert.IsNull(actualMessage.BrowsingClassName);
            Assert.IsNull(actualMessage.BrowsingLevel);
            Assert.IsNull(actualMessage.BrowsingParentId);

            // Financial.
            Assert.AreEqual(expectedFinancialName, actualMessage.FinancialClassName);
            Assert.AreEqual(expectedFinancialId, actualMessage.FinancialClassId);
            Assert.AreEqual(expectedFinancial.hierarchyLevel, actualMessage.FinancialLevel);
            Assert.AreEqual(expectedFinancial.hierarchyParentClassID, actualMessage.FinancialParentId);

            Assert.IsNull(actualMessage.InProcessBy);
            Assert.IsNull(actualMessage.ProcessedDate);

            //SLAW attributes
            Assert.AreEqual(itemSignAttribute.Biodynamic, (actualMessage.Biodynamic == "1" ? true : false));
            Assert.AreEqual(itemSignAttribute.CheeseRaw, (actualMessage.CheeseRaw == "1" ? true : false));
            Assert.AreEqual(itemSignAttribute.MadeInHouse, (actualMessage.MadeInHouse == "1" ? true : false));
            Assert.AreEqual(itemSignAttribute.GrassFed, (actualMessage.GrassFed == "1" ? true : false));
            Assert.AreEqual(itemSignAttribute.Vegetarian, (actualMessage.Vegetarian == "1" ? true : false));
            Assert.AreEqual(testKosherClass.hierarchyClassName, actualMessage.KosherAgency);
            Assert.AreEqual(itemSignAttribute.PremiumBodyCare, (actualMessage.PremiumBodyCare == "1" ? true : false));
            Assert.AreEqual(itemSignAttribute.Msc, (actualMessage.Msc == "1" ? true : false));
            Assert.AreEqual(itemSignAttribute.AirChilled, (actualMessage.AirChilled == "1" ? true : false));
            Assert.AreEqual(itemSignAttribute.DryAged, (actualMessage.DryAged == "1" ? true : false));
            Assert.AreEqual(itemSignAttribute.FreeRange, (actualMessage.FreeRange == "1" ? true : false));
            Assert.AreEqual(itemSignAttribute.PastureRaised, (actualMessage.PastureRaised == "1" ? true : false));
            Assert.AreEqual(itemSignAttribute.WholeTrade, (actualMessage.WholeTrade == "1" ? true : false));

            //Nutritional
            Assert.AreEqual(itemNutrition.Plu, actualNutritionMessage.Plu);
            Assert.AreEqual(itemNutrition.ServingUnits, actualNutritionMessage.ServingUnits);
            Assert.AreEqual(itemNutrition.RecipeName, actualNutritionMessage.RecipeName);
            Assert.AreEqual(itemNutrition.HshRating, actualNutritionMessage.HshRating);
            Assert.AreEqual(itemNutrition.VitaminA, actualNutritionMessage.VitaminA);
        }

        [TestMethod]
        public void AddProductMessage_ItemIsNotValidated_MessageShouldNotBeCreated()
        {
            // Given.
            Item item = new TestItemBuilder();
            context.Item.Add(item);
            context.SaveChanges();

            // When.
            commandHandler.Execute(new AddProductMessageCommand { ItemId = item.itemID });

            // Then.
            var result = context.MessageQueueProduct.Where(mq => mq.ItemId == item.itemID && mq.MessageStatusId == MessageStatusTypes.Ready);
            Assert.IsTrue(result.Count() == 0);
        }

        [TestMethod]
        public void AddProductMessage_RequiredHierarchyClassesHaveNoSentToEsbTrait_MessageShouldBeCreatedWithStagedStatus()
        {
            // Given.
            SetupProductMessageTestData();

            // When.
            commandHandler.Execute(new AddProductMessageCommand { ItemId = testItem.itemID });

            // Then.
            var result = context.MessageQueueProduct.SingleOrDefault(mq => mq.ItemId == testItem.itemID && mq.MessageStatusId == MessageStatusTypes.Staged);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddProductMessage_RequiredHierarchyClassesHaveNullSentToEsbTraitValue_MessageShouldBeCreatedWithStagedStatus()
        {
            // Given
            SetupProductMessageTestData();

            testMerchandiseClass.HierarchyClassTrait.Add(CreateTestHierarchyClassTrait(Traits.SentToEsb, testMerchandiseClass.hierarchyClassID, null));
            testBrandClass.HierarchyClassTrait.Add(CreateTestHierarchyClassTrait(Traits.SentToEsb, testBrandClass.hierarchyClassID, null));
            testFinancialClass.HierarchyClassTrait.Add(CreateTestHierarchyClassTrait(Traits.SentToEsb, testFinancialClass.hierarchyClassID, null));

            context.SaveChanges();

            // When
            commandHandler.Execute(new AddProductMessageCommand { ItemId = testItem.itemID });

            //Then
            var result = context.MessageQueueProduct.SingleOrDefault(mq => mq.ItemId == testItem.itemID && mq.MessageStatusId == MessageStatusTypes.Staged);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddProductMessage_BrandNotSentToEsb_MessageShouldBeCreatedWithStagedStatus()
        {
            // Given
            SetupProductMessageTestData();

            testMerchandiseClass.HierarchyClassTrait.Add(CreateTestHierarchyClassTrait(Traits.SentToEsb, testMerchandiseClass.hierarchyClassID, DateTime.Now.ToString()));
            testFinancialClass.HierarchyClassTrait.Add(CreateTestHierarchyClassTrait(Traits.SentToEsb, testFinancialClass.hierarchyClassID, DateTime.Now.ToString()));

            // When
            commandHandler.Execute(new AddProductMessageCommand { ItemId = testItem.itemID });

            // Then
            var message = context.MessageQueueProduct.Single(mq => mq.ItemId == testItem.itemID);
            Assert.AreEqual(MessageStatusTypes.Staged, message.MessageStatusId);
        }

        [TestMethod]
        public void AddProductMessage_MerchandiseClassNotSentToEsb_MessageShouldBeCreatedWithStagedStatus()
        {
            // Given
            SetupProductMessageTestData();

            testBrandClass.HierarchyClassTrait.Add(CreateTestHierarchyClassTrait(Traits.SentToEsb, testBrandClass.hierarchyClassID, DateTime.Now.ToString()));
            testFinancialClass.HierarchyClassTrait.Add(CreateTestHierarchyClassTrait(Traits.SentToEsb, testFinancialClass.hierarchyClassID, DateTime.Now.ToString()));

            // When
            commandHandler.Execute(new AddProductMessageCommand { ItemId = testItem.itemID });

            // Then
            var message = context.MessageQueueProduct.Single(mq => mq.ItemId == testItem.itemID);
            Assert.AreEqual(MessageStatusTypes.Staged, message.MessageStatusId);
        }

        [TestMethod]
        public void AddProductMessage_FinancialClassNotSentToEsb_MessageShouldBeCreatedWithStagedStatus()
        {
            // Given
            SetupProductMessageTestData();

            testBrandClass.HierarchyClassTrait.Add(CreateTestHierarchyClassTrait(Traits.SentToEsb, testBrandClass.hierarchyClassID, DateTime.Now.ToString()));
            testMerchandiseClass.HierarchyClassTrait.Add(CreateTestHierarchyClassTrait(Traits.SentToEsb, testMerchandiseClass.hierarchyClassID, DateTime.Now.ToString()));

            // When
            commandHandler.Execute(new AddProductMessageCommand { ItemId = testItem.itemID });

            // Then
            var message = context.MessageQueueProduct.Single(mq => mq.ItemId == testItem.itemID);
            Assert.AreEqual(MessageStatusTypes.Staged, message.MessageStatusId);
        }

        [TestMethod]
        public void AddProductMessage_AllRequiredHierarchiesAreSentToEsb_MessageCreatedWithReadyStatus()
        {
            // Given
            SetupProductMessageTestData();

            testMerchandiseClass.HierarchyClassTrait.Add(CreateTestHierarchyClassTrait(Traits.SentToEsb, testMerchandiseClass.hierarchyClassID, DateTime.Now.ToString()));
            testBrandClass.HierarchyClassTrait.Add(CreateTestHierarchyClassTrait(Traits.SentToEsb, testBrandClass.hierarchyClassID, DateTime.Now.ToString()));
            testFinancialClass.HierarchyClassTrait.Add(CreateTestHierarchyClassTrait(Traits.SentToEsb, testFinancialClass.hierarchyClassID, DateTime.Now.ToString()));

            context.SaveChanges();

            // When
            commandHandler.Execute(new AddProductMessageCommand { ItemId = testItem.itemID });

            //Then
            var message = context.MessageQueueProduct.Single(mq => mq.ItemId == testItem.itemID);
            Assert.AreEqual(MessageStatusTypes.Ready, message.MessageStatusId);
        }

        [TestMethod]
        public void AddProductMessage_ItemIsDepartmentSaleItem_MessageCreatedWithDepartmentSaleSetAsOne()
        {
            // Given
            SetupProductMessageTestData();

            // When
            commandHandler.Execute(new AddProductMessageCommand { ItemId = testItem.itemID });

            // Then
            var result = context.MessageQueueProduct.SingleOrDefault(mq => mq.ItemId == testItem.itemID && mq.MessageStatusId == MessageStatusTypes.Staged);
            Assert.IsNotNull(result);
            Assert.AreEqual("1", result.DepartmentSale);
        }

        [TestMethod]
        public void AddProductMessage_ItemIsAssociatedToCoupon_NoMessageCreated()
        {
            // Given
            SetupProductMessageTestData();

            testItem.itemTypeID = context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.Coupon).itemTypeID;

            HierarchyClassTrait nonMerchTrait = new HierarchyClassTrait();
            nonMerchTrait.Trait = this.context.Trait.First(t => t.traitCode == TraitCodes.NonMerchandise);
            nonMerchTrait.traitID = Traits.NonMerchandise;
            nonMerchTrait.hierarchyClassID = testMerchandiseClass.hierarchyClassID;
            nonMerchTrait.traitValue = NonMerchandiseTraits.Coupon;
            this.context.HierarchyClassTrait.Add(nonMerchTrait);
            this.context.SaveChanges();

            // When
            commandHandler.Execute(new AddProductMessageCommand { ItemId = testItem.itemID });

            // Then
            var result = context.MessageQueueProduct.SingleOrDefault(mq => mq.ItemId == testItem.itemID && mq.MessageStatusId == MessageStatusTypes.Staged);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddProductMessage_ItemIsAssociatedToBottleDeposit_MessageCreatedWithDepositItemType()
        {
            // Given
            SetupProductMessageTestData();

            this.context.Item.First(i => i.itemID == testItem.itemID).itemTypeID = this.context.ItemType.First(it => it.itemTypeCode == ItemTypeCodes.Deposit).itemTypeID;
            this.context.SaveChanges();

            // When
            commandHandler.Execute(new AddProductMessageCommand { ItemId = testItem.itemID });

            // Then
            var result = context.MessageQueueProduct.SingleOrDefault(mq => mq.ItemId == testItem.itemID && mq.MessageStatusId == MessageStatusTypes.Staged);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ItemTypeCode, ItemTypeCodes.Deposit);
            Assert.AreEqual(result.ItemTypeDesc, context.ItemType.First(it => it.itemTypeCode == ItemTypeCodes.Deposit).itemTypeDesc);
        }

        [TestMethod]
        public void AddProductMessage_ItemIsAssociatedToCrv_MessageCreatedWithDepositItemType()
        {
            // Given
            SetupProductMessageTestData();
            this.context.Item.First(i => i.itemID == testItem.itemID).itemTypeID = this.context.ItemType.First(it => it.itemTypeCode == ItemTypeCodes.Deposit).itemTypeID;
            this.context.SaveChanges();

            // When
            commandHandler.Execute(new AddProductMessageCommand { ItemId = testItem.itemID });

            // Then
            var result = context.MessageQueueProduct.SingleOrDefault(mq => mq.ItemId == testItem.itemID && mq.MessageStatusId == MessageStatusTypes.Staged);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ItemTypeCode, ItemTypeCodes.Deposit);
            Assert.AreEqual(result.ItemTypeDesc, context.ItemType.First(it => it.itemTypeCode == ItemTypeCodes.Deposit).itemTypeDesc);
        }

        [TestMethod]
        public void AddProductMessage_ItemIsAssociatedToBottleReturn_MessageCreatedWithReturnItemType()
        {
            // Given
            SetupProductMessageTestData();

            this.context.Item.First(i => i.itemID == testItem.itemID).itemTypeID = this.context.ItemType.First(it => it.itemTypeCode == ItemTypeCodes.Return).itemTypeID;
            this.context.SaveChanges();

            // When
            commandHandler.Execute(new AddProductMessageCommand { ItemId = testItem.itemID });

            // Then
            var result = context.MessageQueueProduct.SingleOrDefault(mq => mq.ItemId == testItem.itemID && mq.MessageStatusId == MessageStatusTypes.Staged);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ItemTypeCode, ItemTypeCodes.Return);
            Assert.AreEqual(result.ItemTypeDesc, context.ItemType.First(it => it.itemTypeCode == ItemTypeCodes.Return).itemTypeDesc);
        }

        [TestMethod]
        public void AddProductMessage_ItemIsAssociatedToCrvCredit_MessageCreatedWithReturnItemType()
        {
            // Given
            SetupProductMessageTestData();

            this.context.Item.First(i => i.itemID == testItem.itemID).itemTypeID = this.context.ItemType.First(it => it.itemTypeCode == ItemTypeCodes.Return).itemTypeID;
            this.context.SaveChanges();

            // When
            commandHandler.Execute(new AddProductMessageCommand { ItemId = testItem.itemID });

            // Then
            var result = context.MessageQueueProduct.SingleOrDefault(mq => mq.ItemId == testItem.itemID && mq.MessageStatusId == MessageStatusTypes.Staged);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ItemTypeCode, ItemTypeCodes.Return);
            Assert.AreEqual(result.ItemTypeDesc, context.ItemType.First(it => it.itemTypeCode == ItemTypeCodes.Return).itemTypeDesc);
        }

        private void SetupProductMessageTestData()
        {
            try
            {
                // HierarchyClasses
                testMerchandiseClass = CreateTestHierarchyClass(Hierarchies.Merchandise, "Test Merch", 5);
                testBrandClass = CreateTestHierarchyClass(Hierarchies.Brands, "Test Brand", 1);
                testBrowsingClass = CreateTestHierarchyClass(Hierarchies.Browsing, "Test Browsing", 1);
                testTaxClass = CreateTestHierarchyClass(Hierarchies.Tax, "Test Tax", 1);
                testFinancialClass = CreateTestHierarchyClass(Hierarchies.Financial, "Financial Class (1234)", 1);
                testNationalClass = CreateTestHierarchyClass(Hierarchies.National, "Test National", 1);
                testKosherClass = CreateTestHierarchyClass(Hierarchies.CertificationAgencyManagement, "Test Kosher", 1);

                context.HierarchyClass.Add(testMerchandiseClass);
                context.HierarchyClass.Add(testBrowsingClass);
                context.HierarchyClass.Add(testBrandClass);
                context.HierarchyClass.Add(testTaxClass);
                context.HierarchyClass.Add(testFinancialClass);
                context.HierarchyClass.Add(testNationalClass);
                context.HierarchyClass.Add(testKosherClass);
                context.SaveChanges();

                testFinancialTrait = CreateTestHierarchyClassTrait(
                    Traits.MerchFinMapping,
                    testMerchandiseClass.hierarchyClassID,
                    "Financial Class (1234)");
                testKosherTrait = CreateTestHierarchyClassTrait(
                   Traits.Kosher,
                   testKosherClass.hierarchyClassID,
                   "1");

                context.HierarchyClassTrait.Add(testFinancialTrait);
                context.HierarchyClassTrait.Add(testKosherTrait);
                context.SaveChanges();

                // Item
                testItem = new Item { itemTypeID = 1, ItemType = context.ItemType.First(it => it.itemTypeCode == "RTL") };
                context.Item.Add(testItem);
                context.SaveChanges();

                testItem.ItemTrait.Add(CreateTestItemTrait(testItem.itemID, Traits.ProductDescription, "ProdDes"));
                testItem.ItemTrait.Add(CreateTestItemTrait(testItem.itemID, Traits.PosDescription, "PosDesc"));
                testItem.ItemTrait.Add(CreateTestItemTrait(testItem.itemID, Traits.PackageUnit, "1"));
                testItem.ItemTrait.Add(CreateTestItemTrait(testItem.itemID, Traits.RetailSize, "1"));
                testItem.ItemTrait.Add(CreateTestItemTrait(testItem.itemID, Traits.RetailUom, "EA"));
                testItem.ItemTrait.Add(CreateTestItemTrait(testItem.itemID, Traits.FoodStampEligible, "1"));
                testItem.ItemTrait.Add(CreateTestItemTrait(testItem.itemID, Traits.PosScaleTare, "1"));
                testItem.ItemTrait.Add(CreateTestItemTrait(testItem.itemID, Traits.DepartmentSale, "1"));
                testItem.ItemTrait.Add(CreateTestItemTrait(testItem.itemID, Traits.ValidationDate, DateTime.Now.ToString()));

                testItem.ItemHierarchyClass.Add(CreateTestItemHierarchyClass(testItem.itemID, testMerchandiseClass.hierarchyClassID));
                testItem.ItemHierarchyClass.Add(CreateTestItemHierarchyClass(testItem.itemID, testBrowsingClass.hierarchyClassID));
                testItem.ItemHierarchyClass.Add(CreateTestItemHierarchyClass(testItem.itemID, testBrandClass.hierarchyClassID));
                testItem.ItemHierarchyClass.Add(CreateTestItemHierarchyClass(testItem.itemID, testTaxClass.hierarchyClassID));
                testItem.ItemHierarchyClass.Add(CreateTestItemHierarchyClass(testItem.itemID, testNationalClass.hierarchyClassID));
                
                context.SaveChanges();

                testScanCode = new ScanCode
                {
                    itemID = testItem.itemID,
                    localeID = Locales.WholeFoods,
                    scanCodeTypeID = ScanCodeTypes.PosPlu,
                    scanCode = "111222333499",
                    Item = context.Item.First(i => i.itemID == testItem.itemID),
                    ScanCodeType = context.ScanCodeType.Single(sct => sct.scanCodeTypeDesc == ScanCodeTypeDescriptions.PosPlu)
                };

                context.ScanCode.Add(testScanCode);
                context.SaveChanges();

                itemNutrition = new ItemNutrition
                {
                    Plu = "111222333499",
                    RecipeName = "Integration test recipe",
                    ServingUnits = 1,
                    HshRating = 3,
                    VitaminA = 5
                };

                context.ItemNutrition.Add(itemNutrition);
                context.SaveChanges();

                itemSignAttribute = new ItemSignAttribute
                {
                    ItemID = testItem.itemID,
                    Biodynamic = true,
                    CheeseRaw = false,
                    MadeInHouse = true,
                    GrassFed = false,
                    Vegetarian = false,
                    PremiumBodyCare = false,
                    Msc = true,
                    AirChilled = true,
                    DryAged = true,
                    FreeRange = false,
                    PastureRaised = true,
                    WholeTrade = true,
                    KosherAgencyId = testKosherClass.hierarchyClassID
                };

                context.ItemSignAttribute.Add(itemSignAttribute);
                context.SaveChanges();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        private void SetupNonRetailProductMessageTestData()
        {
            try
            {
                // HierarchyClasses
                testMerchandiseClass = CreateTestHierarchyClass(Hierarchies.Merchandise, "Test Merch", 5);
                testBrandClass = CreateTestHierarchyClass(Hierarchies.Brands, "Test Brand", 1);
                testBrowsingClass = CreateTestHierarchyClass(Hierarchies.Browsing, "Test Browsing", 1);
                testTaxClass = CreateTestHierarchyClass(Hierarchies.Tax, "Test Tax", 1);
                testFinancialClass = CreateTestHierarchyClass(Hierarchies.Financial, "Financial Class (1234)", 1);
                testNationalClass = CreateTestHierarchyClass(Hierarchies.National, "Test National", 1);
                testKosherClass = CreateTestHierarchyClass(Hierarchies.CertificationAgencyManagement, "Test Kosher", 1);

                context.HierarchyClass.Add(testMerchandiseClass);
                context.HierarchyClass.Add(testBrowsingClass);
                context.HierarchyClass.Add(testBrandClass);
                context.HierarchyClass.Add(testTaxClass);
                context.HierarchyClass.Add(testFinancialClass);
                context.HierarchyClass.Add(testNationalClass);
                context.HierarchyClass.Add(testKosherClass);
                context.SaveChanges();

                testFinancialTrait = CreateTestHierarchyClassTrait(
                    Traits.MerchFinMapping,
                    testMerchandiseClass.hierarchyClassID,
                    "Financial Class (1234)");
                testKosherTrait = CreateTestHierarchyClassTrait(
                   Traits.Kosher,
                   testKosherClass.hierarchyClassID,
                   "1");

                context.HierarchyClassTrait.Add(testFinancialTrait);
                context.HierarchyClassTrait.Add(testKosherTrait);
                context.SaveChanges();

                // Item
                testItem = new Item { itemTypeID = 1, ItemType = context.ItemType.First(it => it.itemTypeCode == "NRT") };
                context.Item.Add(testItem);
                context.SaveChanges();

                testItem.ItemTrait.Add(CreateTestItemTrait(testItem.itemID, Traits.ProductDescription, "ProdDes"));
                testItem.ItemTrait.Add(CreateTestItemTrait(testItem.itemID, Traits.PosDescription, "PosDesc"));
                testItem.ItemTrait.Add(CreateTestItemTrait(testItem.itemID, Traits.PackageUnit, "1"));
                testItem.ItemTrait.Add(CreateTestItemTrait(testItem.itemID, Traits.RetailSize, "1"));
                testItem.ItemTrait.Add(CreateTestItemTrait(testItem.itemID, Traits.RetailUom, "EA"));
                testItem.ItemTrait.Add(CreateTestItemTrait(testItem.itemID, Traits.FoodStampEligible, "1"));
                testItem.ItemTrait.Add(CreateTestItemTrait(testItem.itemID, Traits.PosScaleTare, "1"));
                testItem.ItemTrait.Add(CreateTestItemTrait(testItem.itemID, Traits.DepartmentSale, "1"));
                testItem.ItemTrait.Add(CreateTestItemTrait(testItem.itemID, Traits.ValidationDate, DateTime.Now.ToString()));

                testItem.ItemHierarchyClass.Add(CreateTestItemHierarchyClass(testItem.itemID, testMerchandiseClass.hierarchyClassID));
                testItem.ItemHierarchyClass.Add(CreateTestItemHierarchyClass(testItem.itemID, testBrowsingClass.hierarchyClassID));
                testItem.ItemHierarchyClass.Add(CreateTestItemHierarchyClass(testItem.itemID, testBrandClass.hierarchyClassID));
                testItem.ItemHierarchyClass.Add(CreateTestItemHierarchyClass(testItem.itemID, testTaxClass.hierarchyClassID));
                testItem.ItemHierarchyClass.Add(CreateTestItemHierarchyClass(testItem.itemID, testNationalClass.hierarchyClassID));

                context.SaveChanges();

                testScanCode = new ScanCode
                {
                    itemID = testItem.itemID,
                    localeID = Locales.WholeFoods,
                    scanCodeTypeID = ScanCodeTypes.PosPlu,
                    scanCode = "111222333499",
                    Item = context.Item.First(i => i.itemID == testItem.itemID),
                    ScanCodeType = context.ScanCodeType.Single(sct => sct.scanCodeTypeDesc == ScanCodeTypeDescriptions.PosPlu)
                };

                context.ScanCode.Add(testScanCode);
                context.SaveChanges();

                itemNutrition = new ItemNutrition
                {
                    Plu = "111222333499",
                    RecipeName = "Integration test recipe",
                    ServingUnits = 1,
                    HshRating = 3,
                    VitaminA = 5
                };

                context.ItemNutrition.Add(itemNutrition);
                context.SaveChanges();

                itemSignAttribute = new ItemSignAttribute
                {
                    ItemID = testItem.itemID,
                    Biodynamic = true,
                    CheeseRaw = false,
                    MadeInHouse = true,
                    GrassFed = false,
                    Vegetarian = false,
                    PremiumBodyCare = false,
                    Msc = true,
                    AirChilled = true,
                    DryAged = true,
                    FreeRange = false,
                    PastureRaised = true,
                    WholeTrade = true,
                    KosherAgencyId = testKosherClass.hierarchyClassID
                };

                context.ItemSignAttribute.Add(itemSignAttribute);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private HierarchyClass CreateTestHierarchyClass(int hierarchyId, string hierarchyClassName, int hierarchyClassLevel)
        {
            return new HierarchyClass
            {
                hierarchyID = hierarchyId,
                hierarchyClassName = hierarchyClassName,
                hierarchyLevel = hierarchyClassLevel,
                Hierarchy = context.Hierarchy.Single(h => h.hierarchyID == hierarchyId)
            };
        }

        private HierarchyClassTrait CreateTestHierarchyClassTrait(int traitId, int hierarchyClassId, string traitValue)
        {
            return new HierarchyClassTrait
            {
                traitID = traitId,
                hierarchyClassID = hierarchyClassId,
                HierarchyClass = context.HierarchyClass.First(hc => hc.hierarchyClassID == hierarchyClassId),
                Trait = context.Trait.Single(t => t.traitID == traitId),
                traitValue = traitValue
            };
        }

        private ItemTrait CreateTestItemTrait(int itemId, int traitId, string traitValue)
        {
            return new ItemTrait
            {
                itemID = itemId,
                traitID = traitId,
                traitValue = traitValue,
                localeID = Locales.WholeFoods,
                Trait = context.Trait.Single(t => t.traitID == traitId)
            };
        }

        private ItemHierarchyClass CreateTestItemHierarchyClass(int itemId, int hierarchyClassID)
        {
            return new ItemHierarchyClass
            {
                itemID = itemId,
                hierarchyClassID = hierarchyClassID,
                localeID = Locales.WholeFoods,
                Item = context.Item.First(i => i.itemID == itemId),
                HierarchyClass = context.HierarchyClass.Single(hc => hc.hierarchyClassID == hierarchyClassID)
            };
        }
    }
}
