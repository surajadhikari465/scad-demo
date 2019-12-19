using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemHistoryParameters : IQuery<IEnumerable<ItemHistoryDbModel>>
    {
        public int ItemId { get; set; }
    }
}
