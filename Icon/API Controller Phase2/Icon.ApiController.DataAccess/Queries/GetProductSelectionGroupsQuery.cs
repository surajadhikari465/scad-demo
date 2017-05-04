using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Icon.DbContextFactory;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetProductSelectionGroupsQuery : IQueryHandler<GetProductSelectionGroupsParameters, List<ProductSelectionGroup>>
    {
        private IDbContextFactory<IconContext> iconContextFactory;

        public GetProductSelectionGroupsQuery(IDbContextFactory<IconContext> iconContextFactory)
        {
            this.iconContextFactory = iconContextFactory;
        }

        public List<ProductSelectionGroup> Search(GetProductSelectionGroupsParameters parameters)
        {
            using (var context = iconContextFactory.CreateContext())
            {
                return context.ProductSelectionGroup
                    .Include(psg => psg.ProductSelectionGroupType)
                    .ToList();
            }
        }
    }
}
