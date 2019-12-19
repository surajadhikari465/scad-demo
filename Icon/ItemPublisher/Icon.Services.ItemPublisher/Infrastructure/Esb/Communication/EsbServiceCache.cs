using Icon.Services.ItemPublisher.Infrastructure.Repositories;
using Icon.Services.ItemPublisher.Repositories.Entities;
using Icon.Services.ItemPublisher.Services;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Infrastructure.Esb
{
    /// <summary>
    /// EsbServiceCache is a class that manages the cache of database records our
    /// application needs to build ESB messages. Cuurently the cache is loaded when the app starts
    /// and does not periodically refresh itself.
    /// </summary>
    public class EsbServiceCache : IEsbServiceCache
    {
        private ServiceSettings serviceSettings;
        private ICacheRepository cacheRepository;
        private ConcurrentDictionary<string, Attributes> attributesCache = new ConcurrentDictionary<string, Attributes>();
        private ConcurrentDictionary<string, HierarchyCacheItem> hierarchyCache = new ConcurrentDictionary<string, HierarchyCacheItem>();
        public ConcurrentDictionary<int, ProductSelectionGroup> ProductSelectionGroupCache { get; private set; } = new ConcurrentDictionary<int, ProductSelectionGroup>();
        private readonly System.Timers.Timer timer;

        public bool CacheLoaded { get; private set; } = false;

        public EsbServiceCache(ServiceSettings serviceSettings, ICacheRepository cacheRepository)
        {
            this.serviceSettings = serviceSettings;
            this.cacheRepository = cacheRepository;
            this.timer = new System.Timers.Timer(this.serviceSettings.TimerIntervalCacheRefreshInMilliseconds);
            this.timer.Elapsed += Timer_Elapsed;
            this.timer.Start();

            // it's ok that this isn't awaited. We will check the CacheLoaded var to know if the cache is ready to use or not.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            this.Load();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private async void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            await this.Load();
        }

        private async Task Load()
        {
            this.CacheLoaded = false;
            Task loadAttributes = this.LoadAttributes();
            Task loadHierarchies = this.LoadHierarchies();
            Task loadProductSelectionGroups = this.LoadProductSelectionGroups();
            await Task.WhenAll(new[] { loadAttributes, loadHierarchies, loadProductSelectionGroups });
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