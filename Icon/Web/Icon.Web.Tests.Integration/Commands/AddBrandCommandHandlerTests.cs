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
    public class AddBrandCommandHandlerTests
    {
        private AddBrandCommandHandler commandHandler;
        
        private IconContext context;
        private DbContextTransaction transaction;
        private string testBrandAbbreviation;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            commandHandler = new AddBrandCommandHandler(this.context);

            testBrandAbbreviation = "ABBR";

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void AddBrand_SuccessfulExecution_BrandShouldBeAddedToTheDatabase()
        {
            // Given.
            HierarchyClass brandToAdd = new TestHierarchyClassBuilder();

            var command = new AddBrandCommand
            {
                Brand = brandToAdd
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var newBrand = context.HierarchyClass.Single(hc => hc.hierarchyClassID == brandToAdd.hierarchyClassID);

            Assert.AreEqual(brandToAdd.hierarchyClassName, newBrand.hierarchyClassName);
            Assert.AreEqual(brandToAdd.hierarchyLevel, newBrand.hierarchyLevel);
            Assert.IsNull(newBrand.hierarchyParentClassID);
        }

        [TestMethod]
        public void AddBrand_SuccessfulExecution_BrandAbbreviationHierarchyClassTraitShouldBeCreatedIfValueIsProvided()
        {
            // Given.
            HierarchyClass brandToAdd = new TestHierarchyClassBuilder();

            var command = new AddBrandCommand
            {
                Brand = brandToAdd,
                BrandAbbreviation = testBrandAbbreviation
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var newBrandAbbreviation = context.HierarchyClassTrait.Single(hct =>
                hct.hierarchyClassID == brandToAdd.hierarchyClassID && hct.traitID == Traits.BrandAbbreviation).traitValue;

            Assert.AreEqual(testBrandAbbreviation, newBrandAbbreviation);
        }

        [TestMethod]
        public void AddBrand_SuccessfulExecution_BrandAbbreviationHierarchyClassTraitShouldNotBeCreatedIfValueIsNotProvided()
        {
            // Given.
            HierarchyClass brandToAdd = new TestHierarchyClassBuilder();

            var command = new AddBrandCommand
            {
                Brand = brandToAdd,
                BrandAbbreviation = null
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var newBrandAbbreviation = context.HierarchyClassTrait.SingleOrDefault(hct =>
                hct.hierarchyClassID == brandToAdd.hierarchyClassID && hct.traitID == Traits.BrandAbbreviation);

            Assert.IsNull(newBrandAbbreviation);
        }

        [TestMethod]
        public void AddBrand_SuccessfulExecution_SentToEsbHierarchyClassTraitShouldBeCreatedWithNullValue()
        {
            // Given.
            HierarchyClass brandToAdd = new TestHierarchyClassBuilder();

            var command = new AddBrandCommand
            {
                Brand = brandToAdd,
                BrandAbbreviation = null
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var sentToEsbTrait = context.HierarchyClassTrait.Single(hct =>
                hct.hierarchyClassID == brandToAdd.hierarchyClassID && hct.traitID == Traits.SentToEsb).traitValue;

            Assert.IsNull(sentToEsbTrait);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateValueException))]
        public void AddBrand_DuplicateBrandName_ExceptionShouldBeThrown()
        {
            // Given.
            HierarchyClass brandToAdd = new TestHierarchyClassBuilder();

            var command = new AddBrandCommand
            {
                Brand = brandToAdd
            };

            HierarchyClass duplicateBrand = new TestHierarchyClassBuilder();

            var duplicateCommand = new AddBrandCommand
            {
                Brand = duplicateBrand
            };

            // When.
            commandHandler.Execute(command);
            commandHandler.Execute(duplicateCommand);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateValueException))]
        public void AddBrand_DuplicateTrimmedBrandName_ExceptionShouldBeThrown()
        {
            // Given.
            HierarchyClass brandToAdd = new TestHierarchyClassBuilder();

            var command = new AddBrandCommand
            {
                Brand = brandToAdd
            };

            HierarchyClass duplicateTrimmedBrand = new TestHierarchyClassBuilder();
            duplicateTrimmedBrand.hierarchyClassName += "UniqueWhenNotTrimmed";

            var duplicateCommand = new AddBrandCommand
            {
                Brand = duplicateTrimmedBrand
            };

            // When.
            commandHandler.Execute(command);
            commandHandler.Execute(duplicateCommand);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateValueException))]
        public void AddBrand_DuplicateBrandAbbreviation_ExceptionShouldBeThrown()
        {
            // Given.
            HierarchyClass brandToAdd = new TestHierarchyClassBuilder();
            brandToAdd.hierarchyClassName = brandToAdd.hierarchyClassName.Substring(0, 23);

            var command = new AddBrandCommand
            {
                Brand = brandToAdd,
                BrandAbbreviation = testBrandAbbreviation
            };

            HierarchyClass brandWithDuplicateAbbreviation = new TestHierarchyClassBuilder();
            brandWithDuplicateAbbreviation.hierarchyClassName = brandWithDuplicateAbbreviation.hierarchyClassName.Substring(0, 23) + "1";

            // Calling ToLower() on testBrandAbbreviation to ensure that the duplicate check is not case-sensitive.
            var duplicateCommand = new AddBrandCommand
            {
                Brand = brandWithDuplicateAbbreviation,
                BrandAbbreviation = testBrandAbbreviation.ToLower()
            };

            // When.
            commandHandler.Execute(command);
            commandHandler.Execute(duplicateCommand);

            // Then.
            // Expected exception.
        }
    }
}
