using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OOS.Model.Commands;
using Rhino.Mocks;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public class StoreSKUUpdaterTests
    {
        private ISkuCountRepository skuCountRepository;
        private IStoreValidator storeValidator;

        [SetUp]
        public void Setup()
        {
            skuCountRepository = MockRepository.GenerateStub<ISkuCountRepository>();
            storeValidator = MockRepository.GenerateStub<IStoreValidator>();
        }

        [Test]
        public void Given_Existing_Store_If_User_Requests_SKU_Update_Then_SKU_Is_Updated()
        {
            var updateSkuCommand = new UpdateStoreSkuCommand("BCA", "Grocery", 700);
            var updater = CreateObjectUnderTest();
            
            updater.Insert(updateSkuCommand);
            storeValidator.AssertWasCalled(p => p.Validate("BCA"));
            skuCountRepository.AssertWasCalled(p => p.Insert("BCA", "Grocery", 700));
        }

        private StoreSkuUpdater CreateObjectUnderTest()
        {
            return new StoreSkuUpdater(skuCountRepository, storeValidator);
        }

    }
}
