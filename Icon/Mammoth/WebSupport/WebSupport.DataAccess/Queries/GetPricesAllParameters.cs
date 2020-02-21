using Icon.Common.DataAccess;
using System.Collections.Generic;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries 
{
    public class GetPricesAllParameters : IQuery<List<PriceResetPrice>>
    {
        public string Region { get; set; }
        public List<int> BusinessUnitId { get; set; }
    }
}