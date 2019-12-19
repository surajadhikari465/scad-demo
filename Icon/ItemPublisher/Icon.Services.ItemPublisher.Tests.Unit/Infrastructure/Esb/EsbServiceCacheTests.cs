using Icon.Services.ItemPublisher.Infrastructure.Repositories;
using Icon.Services.ItemPublisher.Repositories.Entities;
using Icon.Services.ItemPublisher.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Infrastructure.Esb.Tests
{
    [TestClass()]
    public class EsbServiceCacheTests
    {
        /// <summary>
        /// Tests that when the attribute cache is loaded when searching for a record in the cache that we do not go to the database
        /// </summary>
        [TestMethod]
        public async Task LoadCache_AttributesCacheShouldBeLoaded()
        {
            // Given.
            ServiceSettings settings = new ServiceSettings()
            {
                TimerIntervalCacheRefreshInMilliseconds = 43200
            };
            Mock<ICacheRepository> repository = new Mock<ICacheRepository>();
            repository.Setup(x => x.GetAttributes()).Returns(Task.FromResult<Dictionary<string, Attributes>>(
                new Dictionary<string, Attributes>()
                {
                    {  "A", new Attributes()
                        {
                            AttributeId=1,
                            AttributeName="A",
                            Description="Test",
                            TraitCode = "T"
                        }
                    }
                }));

            // When.
            EsbServiceCache serviceCache = new EsbServiceCache(settings, repository.Object);

            //Then.
            Assert.IsNotNull(await serviceCache.AttributeFromCache("A"), "We inserted one record into the database so loading the cache should have returned our record");
            repository.Verify(x => x.GetSingleAttribute(It.IsAny<string>()), Times.Never, "The item should have been found in the cache and should not have had to go to the database");
        }

        /// <summary>
        /// Tests that when the hierarchy cache is loaded when searching for a record in the cache that we do not go to the database
        /// </summary>
        [TestMethod]
        public async Task HierarchyCache_HierarchyCacheShouldBeLoaded()
        {
            // Given.
            ServiceSettings settings = new ServiceSettings()
            {
                TimerIntervalCacheRefreshInMilliseconds = 43200
            };
            Mock<ICacheRepository> repository = new Mock<ICacheRepository>();
            repository.Setup(x => x.GetHierarchies()).Returns(Task.FromResult<Dictionary<string, HierarchyCacheItem>>(
                new Dictionary<string, HierarchyCacheItem>()
                {
                    {
                    "test", new HierarchyCacheItem()
                        {
                            HierarchyId=1,
                            HierarchyName="test"
                        }
                    }
                }));

            // When.
            EsbServiceCache serviceCache = new EsbServiceCache(settings, repository.Object);

            //Then.
            Assert.IsNotNull(await serviceCache.HierarchyFromCache("test"));
            repository.Verify(x => x.GetSingleHierarchy(It.IsAny<string>()), Times.Never, "The item should have been found in the cache and should not have had to go to the database");
        }

        /// <summary>
        /// Tests that CacheLoaded is set to true after the cache loads itself. Callers use this to determine if the cache service
        /// is ready or if it's loading and can't handle requests.
        /// </summary>
        [TestMethod]
        public void LoadCache_CacheLoadedShouldBeTrueAfterLoad()
        {
            // Given.
            Mock<ICacheRepository> repository = new Mock<ICacheRepository>();
            ServiceSettings settings = new ServiceSettings()
            {
                TimerIntervalCacheRefreshInMilliseconds = 43200
            };
            repository.Setup(x => x.GetAttributes()).Returns(Task.FromResult<Dictionary<string, Attributes>>(
                new Dictionary<string, Attributes>()
                {
                }));

            repository.Setup(x => x.GetHierarchies()).Returns(Task.FromResult<Dictionary<string, HierarchyCacheItem>>(
              new Dictionary<string, HierarchyCacheItem>()
              {
              }));

            repository.Setup(x => x.GetProductSelectionGroups()).Returns(Task.FromResult<Dictionary<int, ProductSelectionGroup>>(
              new Dictionary<int, ProductSelectionGroup>()
              {
              }));

            // When.
            EsbServiceCache serviceCache = new EsbServiceCache(settings, repository.Object);

            //Then.
            Assert.IsTrue(serviceCache.CacheLoaded, "The CacheLoaded public variable should be true after the cache is loaded");
        }

        /// <summary>
        /// Tests that the cache refreshes itself within the specified interval
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CacheRefresh_CacheRefereshesDuringIntervalSpecifiedInConfig()
        {
            // Given.
            Mock<ICacheRepository> repository = new Mock<ICacheRepository>();
            ServiceSettings settings = new ServiceSettings()
            {
                TimerIntervalCacheRefreshInMilliseconds = 100
            };
            repository.Setup(x => x.GetAttributes()).Returns(Task.FromResult<Dictionary<string, Attributes>>(
                new Dictionary<string, Attributes>()
                {
                }));

            // When.
            EsbServiceCache serviceCache = new EsbServiceCache(settings, repository.Object);

            await Task.Delay(1000);

            // We're just verifying that the GetAttributes is called between 5 and 15 times with a goal of around 10
            // for an interval of 1000ms and a cache refresh interval of 100ms
            // In the real world this duration will be 12 hours or more.

            // Then.
            repository.Verify(x => x.GetAttributes(), Times.Between(5, 15, Range.Inclusive));
        }

        /// <summary>
        /// Tests that if we try to load an attribute from the cache and it's not found that we go to the database and attempt to find it.
        /// If it's found it's added to the cache.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task AttributeFromCache_ItemNotInCache_ItemRetrievedAndAddedToCache()
        {
            // Given.
            Mock<ICacheRepository> repository = new Mock<ICacheRepository>();
            ServiceSettings settings = new ServiceSettings()
            {
                TimerIntervalCacheRefreshInMilliseconds = 100
            };
            repository.Setup(x => x.GetAttributes()).Returns(Task.FromResult<Dictionary<string, Attributes>>(
                new Dictionary<string, Attributes>()
                {
                }));

            repository.Setup(x => x.GetSingleAttribute(It.IsAny<string>())).Returns(Task.FromResult<Attributes>(
                        new Attributes()
                        {
                            AttributeId = 1,
                            AttributeName = "test",
                            Description = "test",
                            TraitCode = "test",
                            TraitId = 1
                        }));

            // When.
            EsbServiceCache serviceCache = new EsbServiceCache(settings, repository.Object);

            // The attribute will not be found in the cache and will have to retrieved from the database
            await serviceCache.AttributeFromCache("test");

            // Then.
            repository.Verify(x => x.GetSingleAttribute(It.IsAny<string>()));
        }

        /// <summary>
        /// Tests that if we try to load a hierarchy from the cache and it's not found that we go to the database and attempt to find it.
        /// If it's found it's added to the cache.
        /// </summary>
        [TestMethod]
        public async Task HierarchyFromCache_ItemNotInCache_ItemRetrievedAndAddedToCache()
        {
            // Given.
            Mock<ICacheRepository> repository = new Mock<ICacheRepository>();
            ServiceSettings settings = new ServiceSettings()
            {
                TimerIntervalCacheRefreshInMilliseconds = 100
            };
            repository.Setup(x => x.GetHierarchies()).Returns(Task.FromResult<Dictionary<string, HierarchyCacheItem>>(
            new Dictionary<string, HierarchyCacheItem>()
            {
            }));

            repository.Setup(x => x.GetSingleHierarchy(It.IsAny<string>())).Returns(Task.FromResult<HierarchyCacheItem>(
            new HierarchyCacheItem()
            {
                HierarchyId = 1,
                HierarchyName = "test"
            }));

            // When.
            EsbServiceCache serviceCache = new EsbServiceCache(settings, repository.Object);

            // The hierarchy will not be found in the cache and will have to retrieved from the database
            await serviceCache.HierarchyFromCache("test");

            // Then.
            repository.Verify(x => x.GetSingleHierarchy(It.IsAny<string>()));
        }
    }
}