using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetProductSelectionGroupsQuery : IQueryHandler<GetProductSelectionGroupsParameters, List<ProductSelectionGroup>>
    {
        private IRenewableContext<IconContext> globalContext;

        public GetProductSelectionGroupsQuery(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public List<ProductSelectionGroup> Search(GetProductSelectionGroupsParameters parameters)
        {
            return globalContext.Context.ProductSelectionGroup
                .Include(psg => psg.ProductSelectionGroupType)
                .ToList();
        }
    }
}
