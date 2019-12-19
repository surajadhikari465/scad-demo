using Icon.Common.DataAccess;
using Irma.Framework;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetFailedIconItemChangesParameters : IQuery<List<IconItemChangeQueue>>
    {
        public string RegionConnectionStringName { get; set; }
    }
}
