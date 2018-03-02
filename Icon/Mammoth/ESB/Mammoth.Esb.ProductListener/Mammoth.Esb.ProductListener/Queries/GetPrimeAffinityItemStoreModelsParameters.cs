using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Esb.ProductListener.Models;
using System.Collections.Generic;

namespace Mammoth.Esb.ProductListener.Queries
{
    public class GetPrimeAffinityItemStoreModelsParameters : IQuery<IEnumerable<PrimeAffinityItemStoreModel>>
    {
        public IEnumerable<int> ItemIds { get; set; }
    }
}
