using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Esb.ProductListener.Models;
using System.Collections.Generic;

namespace Mammoth.Esb.ProductListener.Queries
{
    public class GetItemsParameters : IQuery<IEnumerable<ItemDataAccessModel>>
    {
        public List<int> ItemIds;
    }
}