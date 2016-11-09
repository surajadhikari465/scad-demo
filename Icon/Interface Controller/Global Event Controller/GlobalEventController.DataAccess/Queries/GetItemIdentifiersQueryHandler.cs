using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetItemIdentifiersQueryHandler : IQueryHandler<GetItemIdentifiersQuery, List<ItemIdentifier>>
    {
        private IrmaContext context;

        public GetItemIdentifiersQueryHandler(IrmaContext context)
        {
            this.context = context;
        }

        public List<ItemIdentifier> Handle(GetItemIdentifiersQuery parameters)
        {
            IQueryable<ItemIdentifier> query = context.ItemIdentifier.Include(ii => ii.Item);
            query = query.Where(parameters.Predicate);
            return query.ToList();
        }
    }
}
