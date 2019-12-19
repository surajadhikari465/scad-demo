using System;
using System.Collections.Generic;
using System.Linq;
using AaronPowell.Dynamics.Collections;
using NUnit.Framework;
using OOS.Model.Feed;
using Rhino.Mocks;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public class StoreFeedConsumerServiceTests
    {
        private ILogService logService;
        private IStoreFeedConsumer consumer;
        private IStoreRepository storeRepository;
        private IStoreFactory storeFactory;

        [SetUp]
        public void Setup()
        {
            logService = MockLogService.New();
            consumer = GetFeedLocationConsumer();
            storeRepository = GetStoreRepository();
            storeFactory = GetStoreFactory();
        }

        private IStoreFeedConsumer GetFeedLocationConsumer()
        {
            return MockRepository.GenerateMock<IStoreFeedConsumer>();
        }

        private IStoreRepository GetStoreRepository()
        {
            return MockRepository.GenerateMock<IStoreRepository>();
        }

        private IStoreFactory GetStoreFactory()
        {
            return MockRepository.GenerateMock<IStoreFactory>();
        }

        [Test]
        public void TestGetNewStoresFromFeedReturnsEmptyListWhenConsumeStoreFeedFails()
        {
            consumer.Stub(p => p.Consume()).Return(null).Repeat.Once();
            storeFactory.Stub(p => p.ConstituteFrom(new List<StoreFeed>())).Return(new List<Store>());
            storeRepository.AssertWasNotCalled(p => p.ForAbbrev(Arg<string>.Is.Anything));
            
            var svc = CreateObjectUnderTest();
            svc.Consume();
            
            consumer.VerifyAllExpectations();
            storeRepository.VerifyAllExpectations();
        }

        private StoreFeedConsumerService CreateObjectUnderTest()
        {
            return new StoreFeedConsumerService(logService, consumer, storeRepository, storeFactory);
        }


        [Test]
        public void TestUpdatedStoresFromFeedReturnsEmptyListWhenConsumeFeedFails()
        {
            consumer.Stub(p => p.Consume()).Return(null).Repeat.Once();
            storeRepository.AssertWasNotCalled(p => p.ForAbbrev(Arg<string>.Is.Anything));
            storeFactory.Stub(p => p.ConstituteFrom(new List<StoreFeed>())).Return(new List<Store>());
            
            var svc = CreateObjectUnderTest();
            svc.Consume();
            
            storeRepository.VerifyAllExpectations();
            storeFactory.VerifyAllExpectations();
        }

        [Test]
        public void TestGetNewStoresFromFeedReturnsNewStoreWhenNewStoreConsumed()
        {
            var lamarStoreFeed = new StoreFeed
                                     {
                                         tlc = "LMR",
                                         name = "Lamar",
                                         region = "Southwest",
                                         status = "OPEN",
                                         facility = "Whole Foods Market"
                                     };
            var storeFeeds = new Dictionary<string, StoreFeed> {{"LMR", lamarStoreFeed}}.AsDynamic();
            storeRepository.Stub(p => p.ForAbbrev("LMR")).Return(null).Repeat.Once();
            var stores = new List<Store> {new Store(0){Abbrev = lamarStoreFeed.tlc, Name = lamarStoreFeed.name, RegionId = 13, Status = lamarStoreFeed.status}};
            storeFactory.Stub(p => p.ConstituteFrom(new List<StoreFeed>{storeFeeds.LMR})).Return(stores).Repeat.Once();

            var svc = CreateObjectUnderTest();
            svc.AddNewStores(storeFeeds.Values);

            storeRepository.VerifyAllExpectations();
            storeFactory.VerifyAllExpectations();
        }

        [Test]
        public void Given_non_Whole_Foods_facility_store_feed_store_validation_is_not_checked_when_new_store_added()
        {
            var culinaryCenter = new StoreFeed
                                     {
                                         tlc = "CBC",
                                         name = "Culinary Center",
                                         region = "Southwest",
                                         status = "SOON",
                                         facility = "Culinary Center"
                                     };
            var storeFeeds = new Dictionary<string, StoreFeed> { { "CBC", culinaryCenter } }.AsDynamic();
            storeFactory.Stub(p => p.ConstituteFrom(new List<StoreFeed>())).Return(new List<Store>()).Repeat.Once();

            var svc = CreateObjectUnderTest();
            svc.AddNewStores(storeFeeds.Values);

            storeRepository.AssertWasNotCalled(p => p.ForAbbrev(Arg<string>.Is.Anything));
            storeFactory.AssertWasCalled(p => p.ConstituteFrom(new List<StoreFeed>()));
        }


        [Test]
        public void TestUpdatedStoresFromFeedReturnsUpdatedStoreWhenStoreWithStatusChangeConsumed()
        {
            var lamarStoreFeed = new StoreFeed
            {
                tlc = "LMR",
                name = "Lamar",
                region = "Southwest",
                status = "CLOSED"
            };
            var storeFeeds = new Dictionary<string, StoreFeed> { { "LMR", lamarStoreFeed } }.AsDynamic();
            var lamarStore = new Store(0){Name = "Lamar", Abbrev = "LMR", Status = "OPEN"};
            storeRepository.Stub(p => p.ForAbbrev("LMR")).Return(lamarStore).Repeat.Once();
            var store = new Store(0) { Abbrev = lamarStoreFeed.tlc, Name = lamarStoreFeed.name, RegionId = 13, Status = lamarStoreFeed.status };
            var storesFromFeed = new List<Store> { store  };
            storeFactory.Stub(p => p.ConstituteFrom(new List<StoreFeed> { storeFeeds.LMR })).Return(storesFromFeed).Repeat.Once();
            storeRepository.Expect(p => p.Update(store));

            var sut = CreateObjectUnderTest();
            sut.UpdateExistingStores(storeFeeds.Values);

            storeRepository.VerifyAllExpectations();
            storeFactory.VerifyAllExpectations();
        }

        [Test]
        public void TestUpdatedStoresFromFeedReturnsEmptyListWhenStoreWithNoStausChangeConsumed()
        {
            var lamarStoreFeed = new StoreFeed
            {
                tlc = "LMR",
                name = "Lamar",
                region = "Southwest",
                status = "OPEN"
            };
            var stores = new Dictionary<string, StoreFeed> {{"LMR", lamarStoreFeed}};
            var lamarStore = new Store(0) { Name = "Lamar", Abbrev = "LMR", Status = "OPEN" };
            storeRepository.Stub(p => p.ForAbbrev("LMR")).Return(lamarStore).Repeat.Once();
            var storesFromFeed = new List<Store>();
            storeFactory.Stub(p => p.ConstituteFrom(new List<StoreFeed>())).Return(storesFromFeed).Repeat.Once();

            var sut = CreateObjectUnderTest();
            sut.UpdateExistingStores(stores.Values.ToList());

            storeRepository.VerifyAllExpectations();
            storeFactory.VerifyAllExpectations();
        }

    }
}
