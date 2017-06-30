using GlobalEventController.Common;
using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.Testing.Common;
using Irma.Framework;
using Irma.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Transactions;

namespace GlobalEventController.Tests.DataAccess.BulkCommandTests
{
    [TestClass]
    public class BulkUpdateItemSignAttributesCommandHandlerTests
    {
        private BulkUpdateItemSignAttributesCommandHandler commandHandler;
        private BulkUpdateItemSignAttributesCommand command;
        private IrmaDbContextFactory contextFactory;
        private IrmaContext context;
        private TransactionScope transaction;
        private Item testItem1;
        private Item testItem2;
        private string badIdentifier;

        [TestInitialize]
        public void Initialize()
        {
            try
            {
                transaction = new TransactionScope();
                context = new IrmaContext();
                contextFactory = new IrmaDbContextFactory();
                commandHandler = new BulkUpdateItemSignAttributesCommandHandler(contextFactory);
                command = new BulkUpdateItemSignAttributesCommand { ValidatedItems = new List<ValidatedItemModel>() };

                testItem1 = new TestIrmaDbItemBuilder(context)
                    .WithItemIdentifier(new TestItemIdentifierBuilder().WithDefault_Identifier(1))
                    .WithItemSignAttribute(new TestIrmaItemSignAttributeBuilder());
                testItem2 = new TestIrmaDbItemBuilder(context)
                    .WithItemIdentifier(new TestItemIdentifierBuilder().WithIdentifier("1234567895").WithDefault_Identifier(1))
                    .WithItemSignAttribute(new TestIrmaItemSignAttributeBuilder());

                context.Item.Add(testItem1);
                context.Item.Add(testItem2);

                context.SaveChanges();

                badIdentifier = "4560008";
            }
            catch (DbEntityValidationException dbe)
            {
                Debug.WriteLine(string.Join("\n",
                    dbe.EntityValidationErrors.SelectMany(ev => ev.ValidationErrors)
                                          .Select(v => v.ErrorMessage)));
                throw;
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void BulkUpdateItemSignAttributes_ItemSignAttributesExists_ShouldUpdateAttributes()
        {
            //Given
            command.ValidatedItems.Add(new TestValidatedItemModelBuilder()
                .WithItemId(1)
                .WithScanCode(testItem1.ItemIdentifier.First().Identifier)
                .WithHasItemSignAttributes(true)
                .WithAirChilled(true)
                .WithAnimalWelfareRating("Test AWR")
                .WithHealthyEatingRating("Good")
                .WithBiodynamic(true)
                .WithCheeseMilkType("Test MT")
                .WithCheeseRaw(true)
                .WithDryAged(true)
                .WithEcoScaleRating("Test ESR")
                .WithFreeRange(true)
                .WithFreshOrFrozen("Test FOF")
                .WithGlutenFree(true)
                .WithGrassFed(true)
                .WithKosher(true)
                .WithMadeInHouse(true)
                .WithMsc(true)
                .WithNonGmo(true)
                .WithOrganic(true)
                .WithPastureRaised(true)
                .WithPremiumBodyCare(true)
                .WithSeafoodCatchType("Test SCT")
                .WithVegan(true)
                .WithVegetarian(true)
                .WithWholeTrade(true));
            command.ValidatedItems.Add(new TestValidatedItemModelBuilder()
                .WithItemId(2)
                .WithScanCode(testItem2.ItemIdentifier.First().Identifier)
                .WithHasItemSignAttributes(true)
                .WithAirChilled(false)
                .WithWholeTrade(true));

            //When
            commandHandler.Handle(command);

            //Then
            var updatedItem1 = context.Item
                .AsNoTracking()
                .Single(i => i.Item_Key == testItem1.Item_Key);
            var updatedItem2 = context.Item
                .AsNoTracking()
                .Single(i => i.Item_Key == testItem2.Item_Key);

            AssertAreEqual(command.ValidatedItems[0], updatedItem1.ItemSignAttribute.Single());
            AssertAreEqual(command.ValidatedItems[1], updatedItem2.ItemSignAttribute.Single());
        }

        [TestMethod]
        public void BulkUpdateItemSignAttributes_ItemSignAttributesDontExist_ShouldAddAttributes()
        {
            //Given
            context.ItemSignAttribute.Remove(testItem1.ItemSignAttribute.First());
            context.ItemSignAttribute.Remove(testItem2.ItemSignAttribute.First());
            context.SaveChanges();

            command.ValidatedItems.Add(new TestValidatedItemModelBuilder()
                .WithItemId(1)
                .WithScanCode(testItem1.ItemIdentifier.First().Identifier)
                .WithHasItemSignAttributes(true)
                .WithAirChilled(true)
                .WithAnimalWelfareRating("Test AWR")
                .WithHealthyEatingRating("Good")
                .WithBiodynamic(true)
                .WithCheeseMilkType("Test MT")
                .WithCheeseRaw(true)
                .WithDryAged(true)
                .WithEcoScaleRating("Test ESR")
                .WithFreeRange(true)
                .WithFreshOrFrozen("Test FOF")
                .WithGlutenFree(true)
                .WithGrassFed(true)
                .WithKosher(true)
                .WithMadeInHouse(true)
                .WithMsc(true)
                .WithNonGmo(true)
                .WithOrganic(true)
                .WithPastureRaised(true)
                .WithPremiumBodyCare(true)
                .WithSeafoodCatchType("Test SCT")
                .WithVegan(true)
                .WithVegetarian(true)
                .WithWholeTrade(true));
            command.ValidatedItems.Add(new TestValidatedItemModelBuilder()
                .WithItemId(2)
                .WithScanCode(testItem2.ItemIdentifier.First().Identifier)
                .WithHasItemSignAttributes(true)
                .WithAirChilled(false)
                .WithWholeTrade(true));

            //When
            commandHandler.Handle(command);

            //Then
            var updatedItem1 = context.Item
                .AsNoTracking()
                .Single(i => i.Item_Key == testItem1.Item_Key);
            var updatedItem2 = context.Item
                .AsNoTracking()
                .Single(i => i.Item_Key == testItem2.Item_Key);

            AssertAreEqual(command.ValidatedItems[0], updatedItem1.ItemSignAttribute.Single());
            AssertAreEqual(command.ValidatedItems[1], updatedItem2.ItemSignAttribute.Single());
        }

        [TestMethod]
        public void BulkUpdateItemSignAttributes_IdentifierIsNotDefaultIdentifier_ShouldNotUpdateAttributes()
        {
            //Given
            testItem1.ItemIdentifier.Add(new TestItemIdentifierBuilder().WithIdentifier(badIdentifier).WithDefault_Identifier(0));
            context.SaveChanges();

            Assert.IsTrue(context.ItemIdentifier.Count(ii => ii.Identifier == badIdentifier) == 1);

            command.ValidatedItems.Add(new TestValidatedItemModelBuilder()
                .WithItemId(1)
                .WithScanCode(badIdentifier)
                .WithHasItemSignAttributes(true)
                .WithAirChilled(true)
                .WithAnimalWelfareRating("Test AWR")
                .WithHealthyEatingRating("Good")
                .WithBiodynamic(true)
                .WithCheeseMilkType("Test MT")
                .WithCheeseRaw(true)
                .WithDryAged(true)
                .WithEcoScaleRating("Test ESR")
                .WithFreeRange(true)
                .WithFreshOrFrozen("Test FOF")
                .WithGlutenFree(true)
                .WithGrassFed(true)
                .WithKosher(true)
                .WithMadeInHouse(true)
                .WithMsc(true)
                .WithNonGmo(true)
                .WithOrganic(true)
                .WithPastureRaised(true)
                .WithPremiumBodyCare(true)
                .WithSeafoodCatchType("Test SCT")
                .WithVegan(true)
                .WithVegetarian(true)
                .WithWholeTrade(true));

            //When
            commandHandler.Handle(command);

            //Then
            var itemSignAttributes = context.Item
                .AsNoTracking()
                .Single(i => i.Item_Key == testItem1.Item_Key)
                .ItemSignAttribute.Single();
            Assert.IsNull(itemSignAttributes.AirChilled);
            Assert.IsNull(itemSignAttributes.AnimalWelfareRating);
            Assert.IsNull(itemSignAttributes.HealthyEatingRating);
            Assert.IsNull(itemSignAttributes.Biodynamic);
            Assert.IsNull(itemSignAttributes.CheeseMilkType);
            Assert.IsNull(itemSignAttributes.CheeseRaw);
            Assert.IsNull(itemSignAttributes.DryAged);
            Assert.IsNull(itemSignAttributes.EcoScaleRating);
            Assert.IsNull(itemSignAttributes.FreeRange);
            Assert.IsNull(itemSignAttributes.FreshOrFrozen);
            Assert.IsNull(itemSignAttributes.GlutenFree);
            Assert.IsNull(itemSignAttributes.GrassFed);
            Assert.IsNull(itemSignAttributes.Kosher);
            Assert.IsNull(itemSignAttributes.MadeInHouse);
            Assert.IsNull(itemSignAttributes.Msc);
            Assert.IsNull(itemSignAttributes.NonGmo);
            Assert.IsNull(itemSignAttributes.Organic);
            Assert.IsNull(itemSignAttributes.PastureRaised);
            Assert.IsNull(itemSignAttributes.PremiumBodyCare);
            Assert.IsNull(itemSignAttributes.SeafoodCatchType);
            Assert.IsNull(itemSignAttributes.Vegan);
            Assert.IsNull(itemSignAttributes.Vegetarian);
            Assert.IsNull(itemSignAttributes.WholeTrade);
        }

        [TestMethod]
        public void BulkUpdateItemSignAttributes_IdentifierIsRemoved_ShouldNotUpdateAttributes()
        {
            //Given
            testItem1.ItemIdentifier.Add(new TestItemIdentifierBuilder().WithIdentifier(badIdentifier).WithDefault_Identifier(1).WithRemove_Identifier(1));
            context.SaveChanges();

            Assert.IsTrue(context.ItemIdentifier.Count(ii => ii.Identifier == badIdentifier) == 1);

            command.ValidatedItems.Add(new TestValidatedItemModelBuilder()
                .WithItemId(1)
                .WithScanCode(badIdentifier)
                .WithHasItemSignAttributes(true)
                .WithAirChilled(true)
                .WithAnimalWelfareRating("Test AWR")
                .WithHealthyEatingRating("Good")
                .WithBiodynamic(true)
                .WithCheeseMilkType("Test MT")
                .WithCheeseRaw(true)
                .WithDryAged(true)
                .WithEcoScaleRating("Test ESR")
                .WithFreeRange(true)
                .WithFreshOrFrozen("Test FOF")
                .WithGlutenFree(true)
                .WithGrassFed(true)
                .WithKosher(true)
                .WithMadeInHouse(true)
                .WithMsc(true)
                .WithNonGmo(true)
                .WithOrganic(true)
                .WithPastureRaised(true)
                .WithPremiumBodyCare(true)
                .WithSeafoodCatchType("Test SCT")
                .WithVegan(true)
                .WithVegetarian(true)
                .WithWholeTrade(true));

            //When
            commandHandler.Handle(command);

            //Then
            var itemSignAttributes = context.Item
                .AsNoTracking()
                .Single(i => i.Item_Key == testItem1.Item_Key)
                .ItemSignAttribute.Single();
            Assert.IsNull(itemSignAttributes.AirChilled);
            Assert.IsNull(itemSignAttributes.AnimalWelfareRating);
            Assert.IsNull(itemSignAttributes.HealthyEatingRating);
            Assert.IsNull(itemSignAttributes.Biodynamic);
            Assert.IsNull(itemSignAttributes.CheeseMilkType);
            Assert.IsNull(itemSignAttributes.CheeseRaw);
            Assert.IsNull(itemSignAttributes.DryAged);
            Assert.IsNull(itemSignAttributes.EcoScaleRating);
            Assert.IsNull(itemSignAttributes.FreeRange);
            Assert.IsNull(itemSignAttributes.FreshOrFrozen);
            Assert.IsNull(itemSignAttributes.GlutenFree);
            Assert.IsNull(itemSignAttributes.GrassFed);
            Assert.IsNull(itemSignAttributes.Kosher);
            Assert.IsNull(itemSignAttributes.MadeInHouse);
            Assert.IsNull(itemSignAttributes.Msc);
            Assert.IsNull(itemSignAttributes.NonGmo);
            Assert.IsNull(itemSignAttributes.Organic);
            Assert.IsNull(itemSignAttributes.PastureRaised);
            Assert.IsNull(itemSignAttributes.PremiumBodyCare);
            Assert.IsNull(itemSignAttributes.SeafoodCatchType);
            Assert.IsNull(itemSignAttributes.Vegan);
            Assert.IsNull(itemSignAttributes.Vegetarian);
            Assert.IsNull(itemSignAttributes.WholeTrade);
        }

        [TestMethod]
        public void BulkUpdateItemSignAttributes_IdentifierIsDeleted_ShouldNotUpdateAttributes()
        {
            //Given
            testItem1.ItemIdentifier.Add(new TestItemIdentifierBuilder()
                .WithIdentifier(badIdentifier)
                .WithDefault_Identifier(1)
                .WithDeleted_Identifier(1));
            context.SaveChanges();

            Assert.IsTrue(context.ItemIdentifier.Count(ii => ii.Identifier == badIdentifier) == 1);

            command.ValidatedItems.Add(new TestValidatedItemModelBuilder()
                .WithItemId(1)
                .WithScanCode(badIdentifier)
                .WithHasItemSignAttributes(true)
                .WithAirChilled(true)
                .WithAnimalWelfareRating("Test AWR")
                .WithHealthyEatingRating("Good")
                .WithBiodynamic(true)
                .WithCheeseMilkType("Test MT")
                .WithCheeseRaw(true)
                .WithDryAged(true)
                .WithEcoScaleRating("Test ESR")
                .WithFreeRange(true)
                .WithFreshOrFrozen("Test FOF")
                .WithGlutenFree(true)
                .WithGrassFed(true)
                .WithKosher(true)
                .WithMadeInHouse(true)
                .WithMsc(true)
                .WithNonGmo(true)
                .WithOrganic(true)
                .WithPastureRaised(true)
                .WithPremiumBodyCare(true)
                .WithSeafoodCatchType("Test SCT")
                .WithVegan(true)
                .WithVegetarian(true)
                .WithWholeTrade(true));

            //When
            commandHandler.Handle(command);

            //Then
            var itemSignAttributes = context.Item
                .AsNoTracking()
                .Single(i => i.Item_Key == testItem1.Item_Key)
                .ItemSignAttribute.Single();
            Assert.IsNull(itemSignAttributes.AirChilled);
            Assert.IsNull(itemSignAttributes.AnimalWelfareRating);
            Assert.IsNull(itemSignAttributes.HealthyEatingRating);
            Assert.IsNull(itemSignAttributes.Biodynamic);
            Assert.IsNull(itemSignAttributes.CheeseMilkType);
            Assert.IsNull(itemSignAttributes.CheeseRaw);
            Assert.IsNull(itemSignAttributes.DryAged);
            Assert.IsNull(itemSignAttributes.EcoScaleRating);
            Assert.IsNull(itemSignAttributes.FreeRange);
            Assert.IsNull(itemSignAttributes.FreshOrFrozen);
            Assert.IsNull(itemSignAttributes.GlutenFree);
            Assert.IsNull(itemSignAttributes.GrassFed);
            Assert.IsNull(itemSignAttributes.Kosher);
            Assert.IsNull(itemSignAttributes.MadeInHouse);
            Assert.IsNull(itemSignAttributes.Msc);
            Assert.IsNull(itemSignAttributes.NonGmo);
            Assert.IsNull(itemSignAttributes.Organic);
            Assert.IsNull(itemSignAttributes.PastureRaised);
            Assert.IsNull(itemSignAttributes.PremiumBodyCare);
            Assert.IsNull(itemSignAttributes.SeafoodCatchType);
            Assert.IsNull(itemSignAttributes.Vegan);
            Assert.IsNull(itemSignAttributes.Vegetarian);
            Assert.IsNull(itemSignAttributes.WholeTrade);
        }

        [TestMethod]
        public void BulkUpdateItemSignAttributes_ValidatedItemModelDoesNotHaveItemSignAttributes_ShouldNotAddAttributes()
        {
            //Given
            Item testItem3 = new TestIrmaDbItemBuilder()
                .WithItemIdentifier(new TestItemIdentifierBuilder().WithIdentifier("52341").WithDefault_Identifier(1));
            context.Item.Add(testItem3);
            context.SaveChanges();

            command.ValidatedItems.Add(new TestValidatedItemModelBuilder()
                .WithItemId(1)
                .WithScanCode("52341")
                .WithHasItemSignAttributes(false));

            //When
            commandHandler.Handle(command);

            //Then
            var signAttributes = context.ItemSignAttribute
                .AsNoTracking()
                .SingleOrDefault(isa => isa.Item_Key == testItem3.Item_Key);

            Assert.IsNull(signAttributes);
        }

        [TestMethod]
        public void BulkUpdateItemSignAttributes_ItemsHaveDifferentOrganicValue_ShouldUpdateTrueAndFalseOrganicItemsOnItemTable()
        {
            //Given
            testItem2.Organic = false;

            Item testItem3 = new TestIrmaDbItemBuilder()
                .WithItemIdentifier(new TestItemIdentifierBuilder().WithIdentifier("7822").WithDefault_Identifier(1))
                .WithOrganic(true);
            context.Item.Add(testItem3);
            context.SaveChanges();

            command.ValidatedItems.Add(new TestValidatedItemModelBuilder()
                .WithItemId(1)
                .WithScanCode(testItem1.ItemIdentifier.First().Identifier)
                .WithHasItemSignAttributes(true)
                .WithOrganic(true));
            command.ValidatedItems.Add(new TestValidatedItemModelBuilder()
                .WithItemId(2)
                .WithScanCode(testItem2.ItemIdentifier.First().Identifier)
                .WithHasItemSignAttributes(true)
                .WithOrganic(false));
            command.ValidatedItems.Add(new TestValidatedItemModelBuilder()
                .WithItemId(3)
                .WithScanCode(testItem3.ItemIdentifier.First().Identifier)
                .WithHasItemSignAttributes(true)
                .WithOrganic(null));

            //When
            commandHandler.Handle(command);

            //Then
            var updatedItem1 = context.Item
                .AsNoTracking()
                .Single(i => i.Item_Key == testItem1.Item_Key);
            var updatedItem2 = context.Item
                .AsNoTracking()
                .Single(i => i.Item_Key == testItem2.Item_Key);
            var updatedItem3 = context.Item
                .AsNoTracking()
                .Single(i => i.Item_Key == testItem3.Item_Key);

            Assert.IsTrue(updatedItem1.Organic);
            Assert.IsFalse(updatedItem2.Organic);
            Assert.IsFalse(updatedItem3.Organic);
        }

        private void AssertAreEqual(ValidatedItemModel validatedItemModel, ItemSignAttribute itemSignAttribute)
        {
            Assert.AreEqual(validatedItemModel.AirChilled, itemSignAttribute.AirChilled);
            Assert.AreEqual(validatedItemModel.Biodynamic, itemSignAttribute.Biodynamic);
            Assert.AreEqual(validatedItemModel.CheeseMilkType, itemSignAttribute.CheeseMilkType);
            Assert.AreEqual(validatedItemModel.CheeseRaw, itemSignAttribute.CheeseRaw);
            Assert.AreEqual(validatedItemModel.EcoScaleRating, itemSignAttribute.EcoScaleRating);
            Assert.AreEqual(validatedItemModel.GlutenFree, itemSignAttribute.GlutenFree);
            Assert.AreEqual(validatedItemModel.Kosher, itemSignAttribute.Kosher);
            Assert.AreEqual(validatedItemModel.NonGmo, itemSignAttribute.NonGmo);
            Assert.AreEqual(validatedItemModel.Organic, itemSignAttribute.Organic);
            Assert.AreEqual(validatedItemModel.PremiumBodyCare, itemSignAttribute.PremiumBodyCare);
            Assert.AreEqual(validatedItemModel.FreshOrFrozen, itemSignAttribute.FreshOrFrozen);
            Assert.AreEqual(validatedItemModel.SeafoodCatchType, itemSignAttribute.SeafoodCatchType);
            Assert.AreEqual(validatedItemModel.Vegan, itemSignAttribute.Vegan);
            Assert.AreEqual(validatedItemModel.Vegetarian, itemSignAttribute.Vegetarian);
            Assert.AreEqual(validatedItemModel.WholeTrade, itemSignAttribute.WholeTrade);
            Assert.AreEqual(validatedItemModel.Msc, itemSignAttribute.Msc);
            Assert.AreEqual(validatedItemModel.GrassFed, itemSignAttribute.GrassFed);
            Assert.AreEqual(validatedItemModel.PastureRaised, itemSignAttribute.PastureRaised);
            Assert.AreEqual(validatedItemModel.FreeRange, itemSignAttribute.FreeRange);
            Assert.AreEqual(validatedItemModel.DryAged, itemSignAttribute.DryAged);
            Assert.AreEqual(validatedItemModel.AirChilled, itemSignAttribute.AirChilled);
            Assert.AreEqual(validatedItemModel.MadeInHouse, itemSignAttribute.MadeInHouse);
        }
    }
}
