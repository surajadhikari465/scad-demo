using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Price.Controller.DataAccess.Models;
using System.Collections.Generic;

namespace Mammoth.Price.Controller.DataAccess.Queries
{
    public class GetExistingPriceDataParameters : IQuery<List<PriceEventModel>>
    {
        public int Instance { get; set; }
        public string Region { get; set; }
    }
}
