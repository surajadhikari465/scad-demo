using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OOS.Model.Repository;
using Rhino.Mocks;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public class StoreRepositoryTests
    {
        [SetUp]
        public void Setup()
        {
            Bootstrapper.Bootstrap();
        }

        [Test]
        [Category("Integration Test")]
        public void TestFindClosedStore()
        {
            var repository = CreateObjectUnderTest();
            var store = repository.ForAbbrev("dls");
            var closed = store.Status.Equals(Store.Closed, StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(closed);
        }

        private StoreRepository CreateObjectUnderTest()
        {
            var config = GetConfigurator();
            var dbFactory = CreateDbFactory();
            var userProfile = CreateUserProfileMock();
            return new StoreRepository(config, dbFactory, userProfile);
        }

        private IConfigurator GetConfigurator()
        {
            return MockConfigurator.New();
        }

        private IUserProfile CreateUserProfileMock()
        {
            var userProfile = MockRepository.GenerateMock<IUserProfile>();
            userProfile.Stub(p => p.UserName).Return("OOS.User").Repeat.Once();
            return userProfile;
        }


        private IOOSEntitiesFactory CreateDbFactory()
        {
            var config = MockConfigurator.New();
            return new OOSEntitiesFactory(config);
        }

        [Test]
        [Category("Integration Test")]
        public void TestFindStoreNotClosed()
        {
            var repository = CreateObjectUnderTest();
            var store = repository.ForAbbrev("lmr");
            var notClosed = !store.Status.Equals(Store.Closed, StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(notClosed);
        }

        [Test]
        [Category("Integration Test")]
        public void TestFindNonExistingStore()
        {
            var repository = CreateObjectUnderTest();
            var store = repository.ForAbbrev("not exists");
            
            Assert.IsNull(store);
        }

        [Test]
        [Category("Integration Test")]
        public void TestFindStoreByName()
        {
            var repository = CreateObjectUnderTest();
            var store = repository.ForName("Lamar");

            Assert.AreEqual("Lamar", store.Name);
        }

        [Test]
        [Category("Integration Test")]
        public void TestAddStore()
        {
            var store = CreateTestStore();
            var repository = CreateObjectUnderTest();
            repository.Add(store);
        }

        private Store CreateTestStore()
        {
            return new Store(0)
                       {
                           Abbrev = "ABC",
                           Name = "Six Pack Abs",
                           Status = "NEW"
                       };
        }

        [Test]
        [Category("Integration Test")]
        public void TestFindBusinessUnitNumber()
        {
            var repository = CreateObjectUnderTest();
            var storeNumber = repository.FindPSBusinessUnitNumber("LMR");
            Assert.AreEqual("10145", storeNumber);
        }

        [Test]
        [Category("Integration Test")]
        public void TestFindStatusID()
        {
            var repository = CreateObjectUnderTest();
            var statusId = repository.FindStatusId("Open");
            Assert.AreEqual(3, statusId);
        }

        [Test]
        [Category("Integration Test")]
        public void TestUpdateStore()
        {
            var sut = CreateObjectUnderTest();
            var store = CreateTestStore();
            sut.Update(store);
        }

        [Test]
        [Category("Integration Test")]
        public void TestUpdateStoreFromSage()
        {
            var sut = CreateObjectUnderTest();
            var store = CreateTestStoreFromSage();
            sut.Update(store);
        }

        private Store CreateTestStoreFromSage()
        {
            return new Store(0)
            {
                Abbrev = "ABS",
                Name = "Arabella Station",
                Status = "OPEN"
            };

        }
    }
}
