using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
