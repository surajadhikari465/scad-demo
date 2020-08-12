using Icon.Framework;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetBrandAbbreviationsThatExistQueryTests
    {
        private GetBrandAbbreviationsThatExistQuery query;
        private GetBrandAbbreviationsThatExistParameters parameters;
        private IconContext context;
        private DbContextTransaction transaction;
        private List<string> brandAbbreviations = new List<string>
            {
                "TestAbbr1",
                "TestAbbr2",
                "TestAbbr3"
            };

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            query = new GetBrandAbbreviationsThatExistQuery(context);
            parameters = new GetBrandAbbreviationsThatExistParameters
                {
                    BrandAbbreviations = new List<string>()
                };
            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void GetBrandAbbreviations_BrandAbbreviationsExist_ShouldReturnBrandAbbreviations()
        {
            //Given
            HierarchyClass brand = new HierarchyClass
            {
                hierarchyClassName = "Test Brand",
                hierarchyID = Hierarchies.Brands,
                HierarchyClassTrait = new List<HierarchyClassTrait>
                    {
                        new HierarchyClassTrait 
                            {
                                traitID = Traits.BrandAbbreviation,
                                traitValue = brandAbbreviations[0]                                
                            }
                    }
            };
            context.HierarchyClass.Add(brand);
            context.SaveChanges();
            parameters.BrandAbbreviations.Add(brandAbbreviations[0]);

            //When
            var results = query.Search(parameters);

            //Then
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(brandAbbreviations[0], results[0]);
        }

        [TestMethod]
        public void GetBrandAbbreviations_BrandAbbreviationsDontExist_ShouldReturnEmptyList()
        {
            //Given
            parameters.BrandAbbreviations.AddRange(brandAbbreviations);

            //When
            var results = query.Search(parameters);

            //Then
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetBrandAbbreviations_1BrandAbbreviationFromParametersExists_ShouldReturnOnlyThatBrandAbbreviation()
        {
            //Given
            HierarchyClass brand = new HierarchyClass
            {
                hierarchyClassName = "Test Brand",
                hierarchyID = Hierarchies.Brands,
                HierarchyClassTrait = new List<HierarchyClassTrait>
                    {
                        new HierarchyClassTrait 
                            {
                                traitID = Traits.BrandAbbreviation,
                                traitValue = brandAbbreviations[0]                                
                            }
                    }
            };
            context.HierarchyClass.Add(brand);
            context.SaveChanges();
            parameters.BrandAbbreviations.AddRange(brandAbbreviations);

            //When
            var results = query.Search(parameters);

            //Then
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(brandAbbreviations[0], results[0]);
        }
    }
}
