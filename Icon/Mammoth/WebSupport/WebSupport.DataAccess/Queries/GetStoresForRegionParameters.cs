using Icon.Common.DataAccess;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupport.DataAccess.TransferObjects;

namespace WebSupport.DataAccess.Queries
{
    public class GetStoresForRegionParameters : IQuery<IList<StoreTransferObject>>
    {
        public string Region { get; set; }
        public IList<StoreTransferObject> Results { get; set; }
    }
}
