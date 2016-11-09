using Icon.Common.DataAccess;
using Icon.Web.Common.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
