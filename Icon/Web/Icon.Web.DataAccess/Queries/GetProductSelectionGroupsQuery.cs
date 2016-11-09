using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetProductSelectionGroupsQuery : IQueryHandler<GetProductSelectionGroupsParameters, List<ProductSelectionGroup>>
    {
        private IconContext context;

        public GetProductSelectionGroupsQuery(IconContext context)
        {
            this.context = context;
        }

        public List<ProductSelectionGroup> Search(GetProductSelectionGroupsParameters parameters)
        {
            return context.ProductSelectionGroup.ToList();
        }
    }
}
