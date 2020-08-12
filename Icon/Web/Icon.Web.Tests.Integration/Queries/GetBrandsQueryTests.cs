using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Tests.Common.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetBrandsQueryTests
    {
        private IconContext context;
        private DbContextTransaction transaction;
        private GetBrandsParameters queryParameters;
        private GetBrandsQuery query;
        private int testBrandId;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();
            this.transaction = context.Database.BeginTransaction();

            this.queryParameters = new GetBrandsParameters();
            this.query = new GetBrandsQuery(this.context);
        }

        [TestCleanup]
        public void CleanupData()
        {
            this.context.HierarchyClassTrait
                .RemoveRange(this.context.HierarchyClassTrait.Where(hct => hct.traitID == Traits.BrandAbbreviation && hct.hierarchyClassID == testBrandId));
            this.context.HierarchyClass.RemoveRange(this.context.HierarchyClass.Where(hc => hc.hierarchyClassID == testBrandId));

            if (transaction.UnderlyingTransaction.Connection != null)
            {
                this.transaction.Rollback();
            }
            this.transaction.Dispose();
            this.context.Dispose();
        }

        [TestMethod]
        public void GetBrandsQuery_RunSearch_ResultCountGreaterThanZero()
        {
            // When
            var result = query.Search(new GetBrandsParameters());

            // Then
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetBrandsQuery_RunSearch_ColumnsReturnBrandModel()
        {
            // Given
            HierarchyClass expectedBrand = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Brands)
                .WithHierarchyClassName("GetBrands Test").WithHierarchyLevel(1).WithHierarchyParentClassId(null).Build();
            HierarchyClassTrait brandAbbreviation = new TestHierarchyClassTraitBuilder().WithTraitId(Traits.BrandAbbreviation)
                .WithTraitValue("GBT").WithHierarchyClassId(expectedBrand.hierarchyClassID).Build();
            
            this.context.HierarchyClass.Add(expectedBrand);
            this.context.HierarchyClassTrait.Add(brandAbbreviation);
            this.context.SaveChanges();

            this.testBrandId = expectedBrand.hierarchyClassID;

            // When
            List<BrandModel> brands = this.query.Search(this.queryParameters);

            // Then
            BrandModel actualBrand = brands.First(b => b.HierarchyClassId == expectedBrand.hierarchyClassID);
            string expectedBrandAbbreviation = expectedBrand.HierarchyClassTrait.First(hct => hct.traitID == Traits.BrandAbbreviation).traitValue;

            Assert.AreEqual(expectedBrand.hierarchyClassID, actualBrand.HierarchyClassId, "The expected hierarchyClassID do not match the actual hierarchyClassId");
            Assert.AreEqual(expectedBrand.hierarchyClassName, actualBrand.HierarchyClassName, "The expected hierarchyClassName does not match the actual HierarchyClassName");
            Assert.AreEqual(expectedBrand.hierarchyID, actualBrand.HierarchyId, "The expected HierarchyId does not match the actual HierarchyId");
            Assert.AreEqual(expectedBrand.hierarchyLevel, actualBrand.HierarchyLevel, "The expected hierarchyLevel does not match the actual HierarchyLevel.");
            Assert.AreEqual(expectedBrand.hierarchyParentClassID, actualBrand.HierarchyParentClassId, "The expected hierarchyParentClassId does not match the expected hierarchyParentClassId");
            Assert.AreEqual(expectedBrandAbbreviation, actualBrand.BrandAbbreviation, "The expected Brand Abbreviation does not match the actual Brand Abbreviation");
        }
    }
}
