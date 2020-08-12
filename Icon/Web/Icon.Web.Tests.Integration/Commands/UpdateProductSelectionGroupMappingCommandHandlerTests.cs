using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] 
    public class UpdateProductSelectionGroupCommandHandlerTests
    {
        private UpdateProductSelectionGroupCommandHandler commandHandler;
        private IconContext context;
        private DbContextTransaction transaction;

        private ProductSelectionGroup testPsg;
        private List<ProductSelectionGroupType> testPsgTypes;
        private List<Trait> testTraits;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            commandHandler = new UpdateProductSelectionGroupCommandHandler(context);

            testPsgTypes = new List<ProductSelectionGroupType>
            {
                new ProductSelectionGroupType
                {
                    ProductSelectionGroupTypeName = "Test PSG Type"
                },
                new ProductSelectionGroupType
                {
                    ProductSelectionGroupTypeName = "New Test PSG Type"
                }
            };
            testTraits = new List<Trait>
            {
                new Trait
                {
                    traitDesc = "Test Trait",
                    traitCode = "TST",
                    traitPattern = "test"
                },
                new Trait
                {
                    traitDesc = "New Test Trait",
                    traitCode = "TS2",
                    traitPattern = "test"
                }
            };

            transaction = context.Database.BeginTransaction();

            context.ProductSelectionGroupType.AddRange(testPsgTypes);
            context.Trait.AddRange(testTraits);

            context.SaveChanges();

            testPsg = new ProductSelectionGroup
            {
                ProductSelectionGroupName = "Test PSG",
                ProductSelectionGroupType = context.ProductSelectionGroupType.First(p => p.ProductSelectionGroupTypeName == "Test PSG Type"),
                Trait = context.Trait.First(t => t.traitDesc == "Test Trait"),
                TraitValue = "Test Trait Value"
            };
            context.ProductSelectionGroup.Add(testPsg);

            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            context.Dispose();
        }

        [TestMethod]
        public void UpdateProductSelectionGroup_NewValues_ShouldUpdatePsg()
        {
            //When
            UpdateProductSelectionGroupCommand command = new UpdateProductSelectionGroupCommand
            {
                ProductSelectionGroupId = testPsg.ProductSelectionGroupId,
                ProductSelectionGroupName = "New Name",
                ProductSelectionGroupTypeId = ProductSelectionGroupTypes.Consumable,
                TraitId = 10,
                TraitValue = "New Trait Value"
            };
            commandHandler.Execute(command);

            //Then
            var updatedPsg = context.ProductSelectionGroup
                .FirstOrDefault(p => p.ProductSelectionGroupId == testPsg.ProductSelectionGroupId);

            Assert.IsNotNull(updatedPsg);
            Assert.AreEqual(command.ProductSelectionGroupName, updatedPsg.ProductSelectionGroupName);
            Assert.AreEqual(command.ProductSelectionGroupTypeId, updatedPsg.ProductSelectionGroupTypeId);
            Assert.AreEqual(command.TraitId, updatedPsg.TraitId);
            Assert.AreEqual(command.TraitValue, updatedPsg.TraitValue);
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void UpdateProductSelectionGroup_PsgWithSameNameAndTraitAndTraitValueExists_ShouldThrowException()
        {
            //Given
            var existingPsg = new ProductSelectionGroup
            {
                ProductSelectionGroupName = "Existing Test PSG",
                ProductSelectionGroupType = context.ProductSelectionGroupType.First(p => p.ProductSelectionGroupTypeName == "Test PSG Type"),
                Trait = context.Trait.First(t => t.traitDesc == "Test Trait"),
                TraitValue = "Test Trait Value"
            };
            context.ProductSelectionGroup.Add(existingPsg);
            context.SaveChanges();

            //When
            UpdateProductSelectionGroupCommand command = new UpdateProductSelectionGroupCommand
            {
                ProductSelectionGroupId = testPsg.ProductSelectionGroupId,
                ProductSelectionGroupName = existingPsg.ProductSelectionGroupName,
                //ProductSelectionGroupTypeName = existingPsg.ProductSelectionGroupType.ProductSelectionGroupTypeName,
                //TraitName = existingPsg.Trait.traitDesc,
                ProductSelectionGroupTypeId = existingPsg.ProductSelectionGroupTypeId,
                TraitId = existingPsg.TraitId,
                TraitValue = existingPsg.TraitValue
            };
            commandHandler.Execute(command);
        }
    }
}
