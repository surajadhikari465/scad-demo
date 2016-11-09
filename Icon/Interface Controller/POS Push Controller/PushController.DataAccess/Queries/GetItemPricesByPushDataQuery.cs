using Icon.Framework;
using PushController.Common;
using PushController.DataAccess.Interfaces;
using System.Collections.Generic;

namespace PushController.DataAccess.Queries
{
    public class GetItemPricesByPushDataQuery : IQuery<List<ItemPriceModel>>
    {
        public List<IRMAPush> IrmaPushList { get; set; }
    }
}
