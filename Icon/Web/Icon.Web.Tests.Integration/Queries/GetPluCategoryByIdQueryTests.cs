using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Web.DataAccess.Queries;
using Icon.Framework;
using System.Data.Entity;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.Tests.Common.Builders;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass] [Ignore]
    public class GetPluCategoryByIdQueryTests
    {
        private GetPluCategoryByIdQuery query;
        private GetPluCategoryByIdParameters parameters;
        private IconContext context;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            query = new GetPluCategoryByIdQuery(context);
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
        public void GetPluCategoryByIdQuery_PluCategoriesExist_ShouldReturnPluCategory()
        {
            //Given
            PLUCategory pluCategory = new TestPluCategoryBuilder();
            context.PLUCategory.Add(pluCategory);
            context.SaveChanges();

            parameters = new GetPluCategoryByIdParameters(){PluCategoryID = pluCategory.PluCategoryID};

            //When
            PLUCategory actaulPluCategory = query.Search(parameters);

            //Then
            Assert.AreEqual(pluCategory.PluCategoryID, actaulPluCategory.PluCategoryID);
            Assert.AreEqual(pluCategory.PluCategoryName, actaulPluCategory.PluCategoryName);
        }
    }
}
