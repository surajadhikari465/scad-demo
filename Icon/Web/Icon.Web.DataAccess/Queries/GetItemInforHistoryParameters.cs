using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemInforHistoryParameters : IQuery<IEnumerable<ItemInforHistoryDbModel>>
    {
        public int ItemId { get; set; }
    }
}
