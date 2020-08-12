using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class UpdateBrandHierarchyClassTraitsCommandHandlerTests
    {
        private BrandHierarchyClassTraitsCommandHandler commandHandler;
        private IconContext context;
        private DbContextTransaction transaction;
        private HierarchyClass testBrand;
        private HierarchyClass brandWithDuplicateAbbreviation;
        private string testBrandAbbreviation;
        private string updatedBrandAbbreviation;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            commandHandler = new BrandHierarchyClassTraitsCommandHandler(this.context);

            testBrandAbbreviation = "ABBR";
            updatedBrandAbbreviation = "UPDABBR";

            transaction = context.Database.BeginTransaction();

            testBrand = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Brands);
            context.HierarchyClass.Add(testBrand);
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void StageDuplicateBrandName()
        {
            brandWithDuplicateAbbreviation = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Brands);
            context.HierarchyClass.Add(brandWithDuplicateAbbreviation);
            context.SaveChanges();
        }

        private void StageDuplicateBrandAbbreviation()
        {
            brandWithDuplicateAbbreviation = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Brands)
                .WithBrandAbbreviation(updatedBrandAbbreviation);

            brandWithDuplicateAbbreviation.HierarchyClassTrait.Single().Trait = context.Trait.Single(t => t.traitCode == TraitCodes.BrandAbbreviation);

            context.HierarchyClass.Add(brandWithDuplicateAbbreviation);
            context.SaveChanges();
        }


        [TestMethod]
        public void UpdateBrandHierarchyClassTraits_UserUpdatesExistingBrandAbbreviation_BrandAbbreviationShouldBeUpdated()
        {
            // Given.
            var existingBrandAbbreviation = new HierarchyClassTrait
            {
                hierarchyClassID = testBrand.hierarchyClassID,
                traitID = Traits.BrandAbbreviation,
                traitValue = testBrandAbbreviation,
                Trait = context.Trait.Single(t => t.traitCode == TraitCodes.BrandAbbreviation)
            };

            context.HierarchyClassTrait.Add(existingBrandAbbreviation);
            context.SaveChanges();

            var brandToUpdate = new HierarchyClass
            {
                hierarchyClassID = testBrand.hierarchyClassID,
                hierarchyClassName = testBrand.hierarchyClassName
            };

            var command = new UpdateBrandHierarchyClassTraitsCommand
            {
                Brand = brandToUpdate,
                BrandAbbreviation = updatedBrandAbbreviation
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var updatedBrand = context.HierarchyClass.Single(hc => hc.hierarchyClassID == testBrand.hierarchyClassID);
            var updatedBrandAbbreviationTrait = updatedBrand.HierarchyClassTrait.Single(hct => hct.Trait.traitCode == TraitCodes.BrandAbbreviation);

            Assert.AreEqual(updatedBrandAbbreviation, updatedBrandAbbreviationTrait.traitValue);
        }

        [TestMethod]
        public void UpdateBrandHierarchyClassTraits_UserMakesNoChangeToBrandAbbreviation_UpdateShouldCompleteSuccessfully()
        {
            // Given.
            var existingBrandAbbreviation = new HierarchyClassTrait
            {
                hierarchyClassID = testBrand.hierarchyClassID,
                traitID = Traits.BrandAbbreviation,
                traitValue = testBrandAbbreviation,
                Trait = context.Trait.Single(t => t.traitCode == TraitCodes.BrandAbbreviation)
            };

            context.HierarchyClassTrait.Add(existingBrandAbbreviation);
            context.SaveChanges();

            var brandToUpdate = new HierarchyClass
            {
                hierarchyClassID = testBrand.hierarchyClassID,
                hierarchyClassName = testBrand.hierarchyClassName
            };

            var command = new UpdateBrandHierarchyClassTraitsCommand
            {
                Brand = brandToUpdate,
                BrandAbbreviation = existingBrandAbbreviation.traitValue
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var updatedBrand = context.HierarchyClass.Single(hc => hc.hierarchyClassID == testBrand.hierarchyClassID);
            var updatedBrandAbbreviationTrait = updatedBrand.HierarchyClassTrait.Single(hct => hct.Trait.traitCode == TraitCodes.BrandAbbreviation);

            Assert.AreEqual(existingBrandAbbreviation.traitValue, updatedBrandAbbreviationTrait.traitValue);
        }

        [TestMethod]
        public void UpdateBrandHierarchyClassTraits_UserRemovesExistingBrandAbbreviation_BrandAbbreviationShouldBeRemoved()
        {
            // Given.
            var existingBrandAbbreviation = new HierarchyClassTrait
            {
                hierarchyClassID = testBrand.hierarchyClassID,
                traitID = Traits.BrandAbbreviation,
                traitValue = testBrandAbbreviation,
                Trait = context.Trait.Single(t => t.traitCode == TraitCodes.BrandAbbreviation)
            };

            context.HierarchyClassTrait.Add(existingBrandAbbreviation);
            context.SaveChanges();

            var brandToUpdate = new HierarchyClass
            {
                hierarchyClassID = testBrand.hierarchyClassID,
                hierarchyClassName = testBrand.hierarchyClassName
            };

            var command = new UpdateBrandHierarchyClassTraitsCommand
            {
                Brand = brandToUpdate,
                BrandAbbreviation = null
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var updatedBrand = context.HierarchyClass.Single(hc => hc.hierarchyClassID == testBrand.hierarchyClassID);
            var updatedBrandAbbreviationTrait = updatedBrand.HierarchyClassTrait.SingleOrDefault(hct => hct.Trait.traitCode == TraitCodes.BrandAbbreviation);

            Assert.IsNull(updatedBrandAbbreviationTrait);
        }

        [TestMethod]
        public void UpdateBrandHierarchyClassTraits_BrandHasNoAbbreviationAndUserDoesNotUpdateAbbreviation_BrandAbbreviationTraitShouldNotExist()
        {
            // Given.
            var brandToUpdate = new HierarchyClass
            {
                hierarchyClassID = testBrand.hierarchyClassID,
                hierarchyClassName = testBrand.hierarchyClassName
            };

            var command = new UpdateBrandHierarchyClassTraitsCommand
            {
                Brand = brandToUpdate,
                BrandAbbreviation = null
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var updatedBrand = context.HierarchyClass.Single(hc => hc.hierarchyClassID == testBrand.hierarchyClassID);
            var updatedBrandAbbreviationTrait = updatedBrand.HierarchyClassTrait.SingleOrDefault(hct => hct.Trait.traitCode == TraitCodes.BrandAbbreviation);

            Assert.IsNull(updatedBrandAbbreviationTrait);
        }
    }
}
