using Icon.Services.ItemPublisher.Repositories.Entities;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Infrastructure.Esb
{
    public interface IEsbServiceCache
    {
        Task<Attributes> AttributeFromCache(string attributeName);

        Task<HierarchyCacheItem> HierarchyFromCache(string name);

        ConcurrentDictionary<int, ProductSelectionGroup> ProductSelectionGroupCache { get; }

        ConcurrentDictionary<string,string> UomCache { get; }

        bool CacheLoaded { get; }
    }
}