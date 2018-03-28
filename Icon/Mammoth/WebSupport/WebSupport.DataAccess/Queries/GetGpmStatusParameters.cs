using Icon.Common.DataAccess;
using Mammoth.Framework;
using System.Collections.Generic;

namespace WebSupport.DataAccess.Queries
{
    public class GetGpmStatusParameters : IQuery<IList<RegionGpmStatus>>
    {
        public string Region { get; set; }
    }
}
