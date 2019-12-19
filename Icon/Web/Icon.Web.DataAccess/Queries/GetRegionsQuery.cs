using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{

    public class GetRegionsQuery : IQueryHandler<GetRegionsParameters, List<Regions>>
    {
        private readonly IconContext context;

        public GetRegionsQuery(IconContext context)
        {
            this.context = context;
        }

        public List<Regions> Search(GetRegionsParameters parameters)
        {
            return context.Regions.ToList();
        }
    }
}
