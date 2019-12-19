using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemByIdQuery : IQueryHandler<GetItemByIdParameters, Item>
    {
        private IconContext context;

        public GetItemByIdQuery(IconContext context)
        {
            this.context = context;
        }

        public Item Search(GetItemByIdParameters parameters)
        {
            Item item = context.Item
                .Single(i => i.ItemId == parameters.ItemId);

            // Reload entity in case it has been changed by a stored procedure.
            context.Entry<Item>(item).Reload();

            return item;
        }
    }
}
