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
    public class GetFailedIconPosPushPublishesQuery : IQueryHandler<GetFailedIconPosPushPublishesParameters, List<IConPOSPushPublish>>
    {
        private IrmaContext context;

        public List<IConPOSPushPublish> Search(GetFailedIconPosPushPublishesParameters parameters)
        {
            using (context = new IrmaContext(parameters.RegionConnectionStringName))
            {
                return context.IConPOSPushPublish.Where(p => p.ProcessingFailedDate.HasValue)
                    .ToList();
            }
        }
    }
}
