using System.Collections.Concurrent;
using System.Threading.Tasks;
using Icon.Services.ItemPublisher.Infrastructure.Repositories;
using Icon.Services.ItemPublisher.Repositories.Entities;
using Icon.Services.ItemPublisher.Services;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue.Communication
{
    /// <summary>
    /// ServiceCache is a class that manages the cache of database records our
    /// application needs to build DVS messages. Currently, the cache is loaded when the app starts
    /// and does not periodically refresh itself.
    /// </summary>
    public class MessageServiceCache : IMessageServiceCache
    {
        
        private ICacheRepository cacheRepository;
        private ConcurrentDictionary<string, Attributes> attributesCache = new ConcurrentDictionary<string, Attributes>();
        private ConcurrentDictionary<string, HierarchyCacheItem> hierarchyCache = new ConcurrentDictionary<string, HierarchyCacheItem>();
        public ConcurrentDictionary<int, ProductSelectionGroup> ProductSelectionGroupCache { get; private set; } = new ConcurrentDictionary<int, ProductSelectionGroup>();
        public ConcurrentDictionary<string, string> UomCache { get; private set; } = new ConcurrentDictionary<string, string>();
        

        public bool CacheLoaded { get; private set; } = false;

        public MessageServiceCache(ICacheRepository cacheRepository)
        {
            this.cacheRepository = cacheRepository;
        }

        public async Task RefreshCache()
        {
            this.CacheLoaded = false;
            Task loadAttributes = this.LoadAttributes();
            Task loadHierarchies = this.LoadHierarchies();
            Task loadProductSelectionGroups = this.LoadProductSelectionGroups();
            Task loadUom = this.LoadUomCache();
            await Task.WhenAll(new[] { loadAttributes, loadHierarchies, loadProductSelectionGroups, loadUom });
            this.CacheLoaded = true;
        }

        private async Task LoadAttributes()
        {
            this.attributesCache.Clear();
            this.attributesCache = new ConcurrentDictionary<string, Attributes>(await this.cacheRepository.GetAttributes());
        }

        private async Task LoadHierarchies()
        {
            this.hierarchyCache.Clear();
            this.hierarchyCache = new ConcurrentDictionary<string, HierarchyCacheItem>(await this.cacheRepository.GetHierarchies());
        }

        private async Task LoadProductSelectionGroups()
        {
            this.ProductSelectionGroupCache = new ConcurrentDictionary<int, ProductSelectionGroup>(await this.cacheRepository.GetProductSelectionGroups());
        }

        private async Task LoadUomCache()
        {
            this.UomCache = new ConcurrentDictionary<string, string>(await this.cacheRepository.GetUoms());
        }

        public async Task<Attributes> AttributeFromCache(string attributeName)
        {
            if (this.attributesCache.ContainsKey(attributeName))
            {
                return this.attributesCache[attributeName];
            }
            else
            {
                Attributes attribute = await this.cacheRepository.GetSingleAttribute(attributeName);

                if (attribute == null)
                {
                    return null;
                }
                else
                {
                    this.attributesCache[attribute.AttributeName] = attribute;
                    return attribute;
                }
            }
        }

        public async Task<HierarchyCacheItem> HierarchyFromCache(string name)
        {
            if (this.hierarchyCache.ContainsKey(name))
            {
                return this.hierarchyCache[name];
            }
            else
            {
                HierarchyCacheItem hierarchy = await this.cacheRepository.GetSingleHierarchy(name);

                if (hierarchy == null)
                {
                    return null;
                }
                else
                {
                    this.hierarchyCache[hierarchy.HierarchyName] = hierarchy;
                    return hierarchy;
                }
            }
        }
    }
}