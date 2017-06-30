using GlobalEventController.DataAccess.Queries;
using GlobalEventController.Testing.Common;
using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;

namespace GlobalEventController.Tests.DataAccess.QueryTests
{
    [TestClass]
    public class GetIrmaBrandQueryHandlerTests
    {
        private GetIrmaBrandQueryHandler queryHandler;
        private IrmaContext irmaContext;
        private IrmaDbContextFactory contextFactory;
        private TransactionScope transaction;
        private TestIrmaDataHelper helper = new TestIrmaDataHelper();

        [TestInitialize]
        public void InitializeData()
        {
            this.transaction = new TransactionScope();

            this.irmaContext = new IrmaContext();
            this.contextFactory = new IrmaDbContextFactory();

            this.queryHandler = new GetIrmaBrandQueryHandler(contextFactory);
        }

        [TestCleanup]
        public void CleanupData()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void GetIrmaBrandQuery_ValidParameters_GetsIrmaBrand()
        {
            // Given
            string testBrandName = "test_brand_555";
            int fakeIconBrandId = -555;
            var savedItemBrand = helper.CreateAndSaveItemBrandForTest(this.irmaContext, testBrandName);
            var savedValidatedBrand = helper.CreateAndSaveValidatedBrandForTest(this.irmaContext, savedItemBrand, fakeIconBrandId);
            var query = new GetIrmaBrandQuery
            {
                IconBrandId = fakeIconBrandId
            };

            // When
            var actualIrmaBrand = queryHandler.Handle(query);

            // Then
            Assert.IsNotNull(actualIrmaBrand);
            Assert.AreEqual(testBrandName, actualIrmaBrand.Brand_Name);
        }

        [TestMethod]
        public void GetIrmaBrandQuery_ValidParameters_AndBrandNotAssociatedWithItems_GetsItemCountZero()
        {
            // Given
            string testBrandName = "test_brand_555";
            int fakeIconBrandId = -555;
            int expectedItemCount = 0;
            var savedItemBrand = helper.CreateAndSaveItemBrandForTest(this.irmaContext, testBrandName);
            var savedValidatedBrand = helper.CreateAndSaveValidatedBrandForTest(this.irmaContext, savedItemBrand, fakeIconBrandId);
            var query = new GetIrmaBrandQuery
            {
                IconBrandId = fakeIconBrandId
            };

            // When
            var actualIrmaBrand = queryHandler.Handle(query);

            // Then
            Assert.AreEqual(expectedItemCount, query.ResultItemCount);
        }

        [TestMethod]
        public void GetIrmaBrandQuery_InvalidIdParameter_GetsNullResultDoesntThrow()
        {
            // Given
            string testBrandName = "test_brand_555";
            int fakeIconBrandId = -555;
            int badIconBrandId = -666;
            var savedItemBrand = helper.CreateAndSaveItemBrandForTest(this.irmaContext, testBrandName);
            var savedValidatedBrand = helper.CreateAndSaveValidatedBrandForTest(this.irmaContext, savedItemBrand, fakeIconBrandId);
            var query = new GetIrmaBrandQuery
            {
                IconBrandId = badIconBrandId
            };

            // When
            var actualIrmaBrand = queryHandler.Handle(query);

            // Then
            Assert.IsNull(actualIrmaBrand);
        }


        [TestMethod]
        public void GetIrmaBrandQuery_ValidParameters_AndBrandIsAssociatedWithItems_GetsItemBrand()
        {
            // Given
            string testBrandName = "test_brand_555";
            int fakeIconBrandId = -555;
            var savedItemBrand = helper.
                CreateAndSaveItemBrandForTest(this.irmaContext, testBrandName);
            var savedValidatedBrand = helper.
                CreateAndSaveValidatedBrandForTest(this.irmaContext, savedItemBrand, fakeIconBrandId);
            var savedItem = helper.
                CreateAndSaveItemAndSubteamForTest(this.irmaContext, savedItemBrand.Brand_ID);
            var query = new GetIrmaBrandQuery
            {
                IconBrandId = fakeIconBrandId
            };

            // When
            var actualIrmaBrand = queryHandler.Handle(query);

            // Then
            Assert.IsNotNull(actualIrmaBrand);
            Assert.AreEqual(testBrandName, actualIrmaBrand.Brand_Name);
        }

        [TestMethod]
        public void GetIrmaBrandQuery_ValidParameters_AndBrandIsAssociatedWithItems_GetsItemCount()
        {
            // Given
            string testBrandName = "test_brand_555";
            int fakeIconBrandId = -555;
            int expectedItemCount = 3;
            var savedItemBrand = helper.
                CreateAndSaveItemBrandForTest(this.irmaContext, testBrandName);
            var savedSubteam = helper.CreateAndSaveSubteamForTest(this.irmaContext);
            var savedValidatedBrand = helper.
                CreateAndSaveValidatedBrandForTest(this.irmaContext, savedItemBrand, fakeIconBrandId);
            for (int i = 0; i < expectedItemCount; i++)
            {
                helper.CreateAndSaveItemForTest(this.irmaContext, savedItemBrand.Brand_ID, savedSubteam);
            }
            var query = new GetIrmaBrandQuery
            {
                IconBrandId = fakeIconBrandId
            };

            // When
            var actualIrmaBrand = queryHandler.Handle(query);

            // Then
            Assert.AreEqual(expectedItemCount, query.ResultItemCount);
        }
    }
}