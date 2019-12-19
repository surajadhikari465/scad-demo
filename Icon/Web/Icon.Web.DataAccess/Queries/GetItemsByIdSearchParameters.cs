using System.Collections.Generic;
using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemsByIdSearchParameters : IQuery<GetItemsResult>
    {
        public List<int> ItemIds { get; set; }
    }
}
