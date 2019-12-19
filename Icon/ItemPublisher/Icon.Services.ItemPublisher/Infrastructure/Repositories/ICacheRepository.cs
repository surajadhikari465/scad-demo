using Icon.Services.ItemPublisher.Repositories.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Infrastructure.Repositories
{
    public interface ICacheRepository
    {
        Task<Dictionary<string, Attributes>> GetAttributes();

        Task<Dictionary<string, HierarchyCacheItem>> GetHierarchies();

        Task<Dictionary<int, ProductSelectionGroup>> GetProductSelectionGroups();

        Task<Attributes> GetSingleAttribute(string attributeName);

        Task<HierarchyCacheItem> GetSingleHierarchy(string name);
    }
}