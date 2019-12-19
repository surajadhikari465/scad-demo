using Icon.Common.DataAccess;
using Irma.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetFailedIconItemChangesQuery : IQueryHandler<GetFailedIconItemChangesParameters, List<IconItemChangeQueue>>
    {
        private IrmaContext context;

        public List<IconItemChangeQueue> Search(GetFailedIconItemChangesParameters parameters)
        {
            using (context = new IrmaContext(parameters.RegionConnectionStringName))
            {
                return context.IconItemChangeQueue
                    .Where(i => i.ProcessFailedDate.HasValue)
                    .ToList();
            }
        }
    }
}
