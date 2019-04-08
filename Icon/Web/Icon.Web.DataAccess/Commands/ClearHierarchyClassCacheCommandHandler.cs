using Icon.Common.DataAccess;
using Icon.Web.Common.Cache;

namespace Icon.Web.DataAccess.Commands
{
    public class ClearHierarchyClassCacheCommandHandler : ICommandHandler<ClearHierarchyClassCacheCommand>
    {
        private ICache cache;

        public ClearHierarchyClassCacheCommandHandler(ICache cache)
        {
            this.cache = cache;
        }

        public void Execute(ClearHierarchyClassCacheCommand command)
        {
            if (cache.Exists("GetHierarchyLineageParameters"))
            {
                cache.Remove("GetHierarchyLineageParameters");
            }
            if (cache.Exists("GetCertificationAgenciesParameters"))
            {
                cache.Remove("GetCertificationAgenciesParameters");
            }
        }
    }
}
