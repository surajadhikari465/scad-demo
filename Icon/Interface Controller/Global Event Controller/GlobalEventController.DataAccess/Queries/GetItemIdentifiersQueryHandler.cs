using GlobalEventController.DataAccess.Infrastructure;
using Icon.DbContextFactory;
using Irma.Framework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetItemIdentifiersQueryHandler : IQueryHandler<GetItemIdentifiersQuery, List<ItemIdentifier>>
    {
        private IDbContextFactory<IrmaContext> contextFactory;

        public GetItemIdentifiersQueryHandler(IDbContextFactory<IrmaContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public List<ItemIdentifier> Handle(GetItemIdentifiersQuery parameters)
        {
            using (var context = contextFactory.CreateContext())
            {
                IQueryable<ItemIdentifier> query = context.ItemIdentifier.Include(ii => ii.Item);
                query = query.Where(parameters.Predicate);
                return query.ToList();
            }
        }
    }
}
