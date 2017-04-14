using Icon.Common.DataAccess;
using Icon.Infor.Listeners.Item.Models;
using System.Collections.Generic;

namespace Icon.Infor.Listeners.Item.Queries
{
    public class GetItemValidationPropertiesParameters : IQuery<List<GetItemValidationPropertiesResultModel>>
    {
        public IEnumerable<ItemModel> Items { get; set; }
    }
}