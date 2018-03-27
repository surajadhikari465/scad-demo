using Icon.Common.DataAccess;
using Mammoth.Framework;
using System.Collections.Generic;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    public class GetGpmStatusParameters : IQuery<IList<RegionGpmStatus>>
    {
        public string Region { get; set; }
    }
}
