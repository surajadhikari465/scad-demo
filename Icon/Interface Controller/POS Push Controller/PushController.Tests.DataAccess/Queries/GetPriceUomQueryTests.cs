using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushController.DataAccess.Queries;
using System;
using System.Data.Entity;

namespace PushController.Tests.DataAccess.Queries
{
    [TestClass]
    public class GetPriceUomQueryTests
    {
        private GetPriceUomQueryHandler getPriceUomQueryHandler;
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private UOM testUom;
        private int testUomId;
        private int notFoundUomId;
        
        [TestInitialize]
        public void Initialize()
        {
            context = new GlobalIconContext(new IconContext());
            
            notFoundUomId = 8888888;
            testUom = new TestUomBuilder().Build();

            getPriceUomQueryHandler = new GetPriceUomQueryHandler(context);

            transaction = context.Context.Database.BeginTransaction();

            context.Context.UOM.Add(testUom);
            context.Context.SaveChanges();

            testUomId = testUom.uomID;
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void GetUom_UomIsFound_UomShouldBeReturned()
        {
            // Given.
            var query = new GetPriceUomQuery
            {
                PriceUomId = testUomId
            };

            // When.
            var queryResults = getPriceUomQueryHandler.Execute(query);

            // Then.
            Assert.IsNotNull(queryResults);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetUom_UomIsNotFound_ExceptionShouldBeThrown()
        {
            // Given.
            var query = new GetPriceUomQuery
            {
                PriceUomId = notFoundUomId
            };

            // When.
            var queryResults = getPriceUomQueryHandler.Execute(query);

            // Then.
            // Expected exception.
        }
    }
}
