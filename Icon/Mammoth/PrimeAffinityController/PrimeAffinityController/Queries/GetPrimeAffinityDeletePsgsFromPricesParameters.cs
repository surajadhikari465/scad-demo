using Mammoth.Common.DataAccess.CommandQuery;
using PrimeAffinityController.Models;
using System.Collections.Generic;

namespace PrimeAffinityController.Queries
{
    public class GetPrimeAffinityDeletePsgsFromPricesParameters : IQuery<IEnumerable<PrimeAffinityPsgPriceModel>>
    {
        public List<string> PriceTypes { get; set; }
        public string Region { get; set; }
        public List<int> ExcludedPSNumbers { get; set; }
    }
}
