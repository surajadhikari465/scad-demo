using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Infrastructure.ItemSearch;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemsParameters : IQuery<GetItemsResult>
    {
        public List<ItemSearchCriteria> ItemAttributeJsonParameters { get; set; } = new List<ItemSearchCriteria>();
        public int Top { get; set; } = 1;
        public int Skip { get; set; }
        public string OrderByValue { get; set; } = "ItemId";
        public string OrderByOrder { get; set; } = "ASC";
    }

    public class GetItemsResult
    {
        public int TotalRecordsCount { get; set; }
        public IEnumerable<ItemDbModel> Items { get; set; }
        public string Query { get; set; }

        public GetItemsResult()
        {
            Items = new List<ItemDbModel>();
        }
    }
}
