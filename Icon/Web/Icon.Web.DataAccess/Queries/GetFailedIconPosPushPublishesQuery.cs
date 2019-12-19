using Icon.Common.DataAccess;
using Irma.Framework;
using System.Collections.Generic;
using System.Linq;

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
