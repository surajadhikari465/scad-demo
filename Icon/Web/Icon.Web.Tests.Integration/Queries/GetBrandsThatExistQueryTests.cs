using Icon.Framework;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass] [Ignore]
    public class GetBrandsThatExistQueryTests
    {
        private GetExistingBrandsQuery query;
        private GetExistingBrandsParameters parameters;
        private IconContext context;
        private DbContextTransaction transaction;
        private List<string> brandNames = new List<string>
            {
                "BrandsThatExistQueryTest1",
                "BrandsThatExistQueryTest2",
                "BrandsThatExistQueryTest3"
            };
        
        [TestInitialize]
        public void Initialize()
        {            
            context = new IconContext();
            query = new GetExistingBrandsQuery(context);
            parameters = new GetExistingBrandsParameters
                {
                    BrandNames = new List<string>()
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
        public void GetBrandsThatExist_BrandsExist_ShouldReturnBrandNames()
        {
            //Given
            context.HierarchyClass.Add(new HierarchyClass
                {
                    hierarchyClassName = brandNames[0],
                    hierarchyID = Hierarchies.Brands
                });
            context.SaveChanges();
            parameters.BrandNames.Add(brandNames[0]);

            //When
            var results = query.Search(parameters);

            //Then
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(brandNames[0], results[0]);
        }

        [TestMethod]
        public void GetBrandsThatExist_BrandsDontExist_ShouldReturnEmptyList()
        {
            //Given
            parameters.BrandNames.AddRange(brandNames);

            //When
            var results = query.Search(parameters);

            //Then
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetBrandsThatExist_1BrandExistsFromParameters_ShouldReturnBrandNameThatExists()
        {
            //Given
            context.HierarchyClass.AddRange(new List<HierarchyClass>
                {   
                    new HierarchyClass
                        {
                            hierarchyClassName = brandNames[0],
                            hierarchyID = Hierarchies.Brands
                        }
                });
            context.SaveChanges();
            parameters.BrandNames.AddRange(brandNames);

            //When
            var results = query.Search(parameters);

            //Then
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(brandNames[0], results[0]);
        }
    }
}
