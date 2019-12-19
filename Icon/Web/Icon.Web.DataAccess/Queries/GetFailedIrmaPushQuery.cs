using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetFailedIrmaPushQuery : IQueryHandler<GetFailedIrmaPushParameters, List<IRMAPush>>
    {
        private IconContext context;

        public GetFailedIrmaPushQuery(IconContext context)
        {
            this.context = context;
        }

        public List<IRMAPush> Search(GetFailedIrmaPushParameters parameters)
        {
            var failedIrmaPushes = context.IRMAPush.Where(ip => ip.EsbReadyFailedDate.HasValue || ip.UdmFailedDate.HasValue)
                .ToList();
            return failedIrmaPushes;
        }
    }
}
