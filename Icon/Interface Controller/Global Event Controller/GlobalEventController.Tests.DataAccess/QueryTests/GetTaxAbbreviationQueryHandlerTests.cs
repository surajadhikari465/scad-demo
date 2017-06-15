using GlobalEventController.DataAccess.Queries;
using Icon.Framework;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;

namespace GlobalEventController.Tests.DataAccess.QueryTests
{
    [TestClass]
    public class GetTaxAbbreviationQueryHandlerTests
    {
        private IconContext context;
        private DbContextTransaction transaction;
        private GetTaxAbbreviationQueryHandler queryHandler;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();
            this.queryHandler = new GetTaxAbbreviationQueryHandler(this.context);
            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            this.transaction.Rollback();
            this.context.Dispose();
        }

        protected int SaveHierarchyClassWithTaxAbbreviationTraitForTest(string taxAbbreviationTrait)
        {
            HierarchyClass hierarchyClass = null;
            if (String.IsNullOrWhiteSpace(taxAbbreviationTrait))
            {
                hierarchyClass = new TestHierarchyClassBuilder().Build();
                var trait =
                    new HierarchyClassTrait
                    {
                        hierarchyClassID = hierarchyClass.hierarchyClassID,
                        traitID = Traits.TaxAbbreviation,
                        traitValue = taxAbbreviationTrait
                    };
                hierarchyClass.HierarchyClassTrait.Add(trait);
            }
            else
            {
                hierarchyClass = new TestHierarchyClassBuilder()
                  .WithTaxAbbreviationTrait(taxAbbreviationTrait)
                  .Build();
            }

            this.context.HierarchyClass.Add(hierarchyClass);
            this.context.SaveChanges();

            return hierarchyClass.hierarchyClassID;
        } 

        [TestMethod]
        public void GetTaxAbbreviationQuery_ValidHierarchyClassId_ReturnsHierarchyClassObject()
        {
            // Given
            string expectedTaxAbbreviation = "XYZ";
            var savedHierarchyClassId = SaveHierarchyClassWithTaxAbbreviationTraitForTest(expectedTaxAbbreviation);
            var query = new GetTaxAbbreviationQuery { HierarchyClassId = savedHierarchyClassId };

            // When
            var actualTaxAbbreviation = queryHandler.Handle(query);

            // Then
            Assert.IsNotNull(actualTaxAbbreviation);
            Assert.AreEqual(expectedTaxAbbreviation, actualTaxAbbreviation);
        }

        [TestMethod]
        public void GetTaxAbbreviationQuery_ReadOnlyQueryParameter_HasExpectedValue()
        {
            // Given
            string expectedParameterValue = "ABR";

            // When
            var query = new GetTaxAbbreviationQuery { HierarchyClassId = 999 };

            // Then
            Assert.AreEqual(expectedParameterValue, query.TaxTraitCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetTaxAbbreviationQuery_HierarchyClassIdLessThanOne_ThrowsArgumentException()
        {
            // Given
            var query = new GetTaxAbbreviationQuery { HierarchyClassId = 0 };

            // When
            var actual = queryHandler.Handle(query);

            // Then
            // Expected ArgumentOutOfRangeException
        }

        [TestMethod]
        public void GetTaxAbbreviationQuery_HierarchyClassIdLessThanOne_ThrowsException_WithExpectedMessage()
        {
            // Given
            var query = new GetTaxAbbreviationQuery { HierarchyClassId = 0 };
            string expectedExceptionMessage = "The value of HierarchyClassId must be greater than 0.";

            try
            {
                // When
                var actual = queryHandler.Handle(query);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Then
                Assert.IsTrue(ex.Message.Contains(expectedExceptionMessage));
            }
        }

        [TestMethod]
        public void GetTaxAbbreviationQuery_HierarchyClassWithoutTaxAbbreviaton_ReturnsNull()
        {
            // Given
            var savedHierarchyClassId = SaveHierarchyClassWithTaxAbbreviationTraitForTest(null);
            var query = new GetTaxAbbreviationQuery { HierarchyClassId = savedHierarchyClassId };

            // When
            var actual = queryHandler.Handle(query);

            // Then
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void GetTaxAbbreviationQuery_HierarchyClassWithEmptyTaxAbbreviaton_ReturnsEmptyString()
        {
            // Given
            var savedHierarchyClassId = SaveHierarchyClassWithTaxAbbreviationTraitForTest(String.Empty);
            var query = new GetTaxAbbreviationQuery { HierarchyClassId = savedHierarchyClassId };

            // When
            var actual = queryHandler.Handle(query);

            // Then
            Assert.AreEqual(String.Empty, actual);
        }

        [TestMethod]
        public void GetTaxAbbreviationQuery_HierarchyClassWithWhitespaceAbbreviaton_ReturnsWhitespace()
        {
            // Given
            var whitepacesString = " \t ";
            var savedHierarchyClassId = SaveHierarchyClassWithTaxAbbreviationTraitForTest(whitepacesString);
            var query = new GetTaxAbbreviationQuery { HierarchyClassId = savedHierarchyClassId };

            // When
            var actual = queryHandler.Handle(query);

            // Then
            Assert.AreEqual(whitepacesString, actual);
        }
    }
}
