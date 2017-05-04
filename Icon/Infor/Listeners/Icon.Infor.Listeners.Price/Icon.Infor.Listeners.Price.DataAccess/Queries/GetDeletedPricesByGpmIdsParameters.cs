using Icon.Infor.Listeners.Price.DataAccess.Models;
using Mammoth.Common.DataAccess.CommandQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Price.DataAccess.Queries
{
    public class GetDeletedPricesByGpmIdsParameters : IQuery<IEnumerable<DeletedPriceModel>>
    {
        public IEnumerable<Guid> PriceIds { get; set; }
    }
}
