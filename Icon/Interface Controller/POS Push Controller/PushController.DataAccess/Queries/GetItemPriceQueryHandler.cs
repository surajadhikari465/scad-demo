using Icon.RenewableContext;
using Icon.Framework;
using PushController.DataAccess.Interfaces;
using System.Linq;

namespace PushController.DataAccess.Queries
{
    public class GetItemPriceQueryHandler : IQueryHandler<GetItemPriceQuery, ItemPrice>
    {
        private IRenewableContext<IconContext> context;

        public GetItemPriceQueryHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public ItemPrice Execute(GetItemPriceQuery parameters)
        {
            return context.Context.ItemPrice.SingleOrDefault(ip =>
                ip.itemID == parameters.ItemId &&
                ip.localeID == parameters.LocaleId &&
                ip.itemPriceTypeID == parameters.PriceTypeId);
        }
    }
}
