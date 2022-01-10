using Icon.Services.ItemPublisher.Repositories.Entities;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue
{
    public interface IMessageServiceCache
    {
        Task<Attributes> AttributeFromCache(string attributeName);

        Task<HierarchyCacheItem> HierarchyFromCache(string name);

        ConcurrentDictionary<int, ProductSelectionGroup> ProductSelectionGroupCache { get; }

        ConcurrentDictionary<string,string> UomCache { get; }

        bool CacheLoaded { get; }
        Task RefreshCache();

    }
}