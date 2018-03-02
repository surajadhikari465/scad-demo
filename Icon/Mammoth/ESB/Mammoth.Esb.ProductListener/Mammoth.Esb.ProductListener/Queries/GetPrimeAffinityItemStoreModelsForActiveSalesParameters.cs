using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Esb.ProductListener.Models;
using System.Collections.Generic;

namespace Mammoth.Esb.ProductListener.Queries
{
    public class GetPrimeAffinityItemStoreModelsForActiveSalesParameters : IQuery<List<PrimeAffinityItemStoreModel>>
    {
        public List<ItemDataAccessModel> Items { get; set; }
        public List<string> EligiblePriceTypes { get; set; }
    }
}
