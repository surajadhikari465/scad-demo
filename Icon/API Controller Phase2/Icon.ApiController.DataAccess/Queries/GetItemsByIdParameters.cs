
using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetItemsByIdParameters : IQuery<List<Item>>
    {
        public List<int> ItemsById { get; set; }
    }
}
