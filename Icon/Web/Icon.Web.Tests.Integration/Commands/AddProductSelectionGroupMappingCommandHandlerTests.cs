using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class AddProductSelectionGroupCommandHandlerTests
    {
        private AddProductSelectionGroupCommandHandler commandHandler;
        private IconContext context;
        private DbContextTransaction transaction;

        private ProductSelectionGroupType testPsgType;
        private Trait testTrait;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            commandHandler = new AddProductSelectionGroupCommandHandler(context);

            testPsgType = new ProductSelectionGroupType
            {
                ProductSelectionGroupTypeName = "Test PSG Type"
            };
            testTrait = new Trait
            {
                traitDesc = "Test Trait",
                traitCode = "TST",
                traitPattern = "test"
            };

            transaction = context.Database.BeginTransaction();

            context.ProductSelectionGroupType.Add(testPsgType);
            context.Trait.Add(testTrait);
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            context.Dispose();
        }
        
        [TestMethod]
        public void AddProductSelectionGroup_PsgDoesNotExist_ShouldAddNewMappingAssociatedToPsg()
        {
            // Given
            AddProductSelectionGroupCommand command = new AddProductSelectionGroupCommand
            {
                ProductSelectionGroupName = "New Test PSG",
                ProductSelectionGroupTypeId = testPsgType.ProductSelectionGroupTypeId,
                TraitId = testTrait.traitID,
                TraitValue = "Test Trait Value",
                MerchandiseHierarchyClassId = null
            };

            // When
            commandHandler.Execute(command);

            //Then
            var newPsg = context.ProductSelectionGroup.SingleOrDefault(p => p.ProductSelectionGroupName == command.ProductSelectionGroupName);

            Assert.IsNotNull(newPsg);
            Assert.AreEqual(command.ProductSelectionGroupName, newPsg.ProductSelectionGroupName);
            Assert.AreEqual(testPsgType.ProductSelectionGroupTypeId, newPsg.ProductSelectionGroupType.ProductSelectionGroupTypeId);
            Assert.AreEqual(testTrait.traitID, newPsg.TraitId);
            Assert.AreEqual(command.TraitValue, newPsg.TraitValue);
            Assert.AreEqual(command.MerchandiseHierarchyClassId, newPsg.MerchandiseHierarchyClassId);
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void AddProductSelectionGroup_PsgWithSameNameAndTraitAndTraitValueAndMerchandiseHierarchyClassIdExists_ShouldThrowException()
        {
            //Given
            var existingPsg = new ProductSelectionGroup
            {
                ProductSelectionGroupName = "Existing Test PSG",
                ProductSelectionGroupType = testPsgType,
                Trait = testTrait,
                TraitValue = "Test Trait Value",
                MerchandiseHierarchyClassId = null
            };
            context.ProductSelectionGroup.Add(existingPsg);
            context.SaveChanges();

            //When
            AddProductSelectionGroupCommand command = new AddProductSelectionGroupCommand
            {
                ProductSelectionGroupName = "Existing Test PSG",
                ProductSelectionGroupTypeId = testPsgType.ProductSelectionGroupTypeId,
                TraitId = testTrait.traitID,
                TraitValue = "Test Trait Value",
                MerchandiseHierarchyClassId = null
            };
            commandHandler.Execute(command);

            // Then
            // Expected Exception
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void AddProductSelectionGroup_PsgWithNotNullTraitAndTraitValueAndMerchandiseHierarchyClassId_ShouldThrowException()
        {
            //Given
            AddProductSelectionGroupCommand command = new AddProductSelectionGroupCommand
            {
                ProductSelectionGroupName = "Existing Test PSG",
                ProductSelectionGroupTypeId = testPsgType.ProductSelectionGroupTypeId,
                TraitId = testTrait.traitID,
                TraitValue = "Test Trait Value",
                MerchandiseHierarchyClassId = 5
            };

            //When
            commandHandler.Execute(command);

            // Then
            // Expected Exception
        }
      

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void AddProductSelectionGroup_PsgWithNotNullTraitAndNullTraitValueAndNotNullMerchandiseHierarchyClassId_ShouldThrowException()
        {
            //Given
            AddProductSelectionGroupCommand command = new AddProductSelectionGroupCommand
            {
                ProductSelectionGroupName = "Existing Test PSG",
                ProductSelectionGroupTypeId = testPsgType.ProductSelectionGroupTypeId,
                TraitId = testTrait.traitID,
                TraitValue = null,
                MerchandiseHierarchyClassId = 5
            };

            //When
            commandHandler.Execute(command);

            // Then
            // Expected Exception
        }
    }
}
