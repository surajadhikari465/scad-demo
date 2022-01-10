using System.Collections.Generic;
using System.Threading.Tasks;
using Icon.Services.ItemPublisher.Infrastructure.MessageQueue.Communication;
using Icon.Services.ItemPublisher.Infrastructure.Repositories;
using Icon.Services.ItemPublisher.Repositories.Entities;
using Icon.Services.ItemPublisher.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Icon.Services.ItemPublisher.Tests.Unit.Infrastructure.MessageQueue
{
    [TestClass()]
    public class MessageServiceCacheTests
    {

        private ServiceSettings settings = new ServiceSettings();
        private readonly Mock<ICacheRepository> repository = new Mock<ICacheRepository>();

        [TestInitialize]
        public void TestInitialize()
        {
            settings = new ServiceSettings();

            repository.Setup(x => x.GetHierarchies()).Returns(Task.FromResult(new Dictionary<string, HierarchyCacheItem>(){
                {
                    "test", new HierarchyCacheItem()
                    {
                        HierarchyId=1,
                        HierarchyName="test"
                    }
                }
            }));
            repository.Setup(x => x.GetSingleHierarchy(It.IsAny<string>())).Returns(Task.FromResult(new HierarchyCacheItem()
            {
                HierarchyId = 1,
                HierarchyName = "test"
            }));
            repository.Setup(x => x.GetAttributes()).Returns(Task.FromResult(new Dictionary<string, Attributes>()
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
            repository.Setup(x => x.GetSingleAttribute(It.IsAny<string>())).Returns(Task.FromResult(
                new Attributes()
                {
                    AttributeId = 1,
                    AttributeName = "test",
                    Description = "test",
                    TraitCode = "test",
                    TraitId = 1
                }));
            repository.Setup(x => x.GetProductSelectionGroups()).Returns(Task.FromResult(new Dictionary<int, ProductSelectionGroup>()));
            repository.Setup(x => x.GetUoms()).Returns(Task.FromResult(new Dictionary<string, string>()));
        }

        /// <summary>
        /// Tests that when the attribute cache is loaded when searching for a record in the cache that we do not go to the database
        /// </summary>
        [TestMethod]
        public async Task LoadCache_AttributesCacheShouldBeLoaded()
        {
            // When.
            MessageServiceCache serviceCache = new MessageServiceCache(repository.Object);
            await serviceCache.RefreshCache();

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
            // When.
            MessageServiceCache serviceCache = new MessageServiceCache(repository.Object);
            await serviceCache.RefreshCache();
            //Then.
            Assert.IsNotNull(await serviceCache.HierarchyFromCache("test"));
            repository.Verify(x => x.GetSingleHierarchy(It.IsAny<string>()), Times.Never, "The item should have been found in the cache and should not have had to go to the database");
        }

        /// <summary>
        /// Tests that CacheLoaded is set to true after the cache loads itself. Callers use this to determine if the cache service
        /// is ready or if it's loading and can't handle requests.
        /// </summary>
        [TestMethod]
        public async Task LoadCache_CacheLoadedShouldBeTrueAfterLoad()
        {
    
            // When.
            MessageServiceCache serviceCache = new MessageServiceCache(repository.Object);
            await serviceCache.RefreshCache();
            //Then.
            Assert.IsTrue(serviceCache.CacheLoaded, "The CacheLoaded public variable should be true after the cache is loaded");
        }

        

        /// <summary>
        /// Tests that if we try to load an attribute from the cache and it's not found that we go to the database and attempt to find it.
        /// If it's found it's added to the cache.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task AttributeFromCache_ItemNotInCache_ItemRetrievedAndAddedToCache()
        {
            // When.
            MessageServiceCache serviceCache = new MessageServiceCache(repository.Object);
            await serviceCache.RefreshCache();

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
            // When.
            MessageServiceCache serviceCache = new MessageServiceCache(repository.Object);
            await serviceCache.RefreshCache();

            // The hierarchy will not be found in the cache and will have to retrieved from the database
            await serviceCache.HierarchyFromCache("test1");

            // Then.
            repository.Verify(x => x.GetSingleHierarchy(It.IsAny<string>()));
        }
    }
}