using Icon.Framework;
using PushController.DataAccess.Interfaces;

namespace PushController.DataAccess.Queries
{
    public class GetItemPriceQuery : IQuery<ItemPrice>
    {
        public int ItemId { get; set; }
        public int LocaleId { get; set; }
        public int PriceTypeId { get; set; }
    }
}
