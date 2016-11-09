using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSupport.DataAccess.Queries;
using Irma.Framework;
using System.Data.Entity;
using Moq;
using System.Linq;
using System.Collections.Generic;

namespace WebSupport.DataAccess.Test.Queries
{
    [TestClass]
    public class GetExistingScanCodesQueryTests
    {
        private GetExistingScanCodesQuery query;
        private GetExistingScanCodesParameters parameters;
        private IrmaContext context;
        private DbContextTransaction transaction;
        private Mock<IIrmaContextFactory> mockIrmaContextFactory;

        [TestInitialize]
        public void Initialize()
        {
            context = new IrmaContext();
            transaction = context.Database.BeginTransaction();

            mockIrmaContextFactory = new Mock<IIrmaContextFactory>();
            mockIrmaContextFactory.Setup(m => m.CreateContext(It.IsAny<string>()))
                .Returns(context);

            parameters = new GetExistingScanCodesParameters();
            query = new GetExistingScanCodesQuery(mockIrmaContextFactory.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void GetExistingScanCodes_AllScanCodesExist_ShouldReturnEmptyList()
        {
            //Given
            parameters.ScanCodes = context.ItemIdentifier
                .Take(5)
                .Select(ii => ii.Identifier)
                .ToList();

            //When
            var result = query.Search(parameters);

            //Then
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void GetExistingScanCodes_2ScanCodesOutOf5DontExist_ShouldReturn2NonExistingScanCodes()
        {
            //Given
            parameters.ScanCodes = context.ItemIdentifier                
                .Take(3)
                .Select(ii => ii.Identifier)
                .ToList()
                .Concat(new List<string> { "999999999", "1111111111111" })
                .ToList();

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(2, result.Count);
        }
    }
}
