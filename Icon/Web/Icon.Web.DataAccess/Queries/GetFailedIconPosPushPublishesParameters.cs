using Icon.Common.DataAccess;
using Irma.Framework;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetFailedIconPosPushPublishesParameters : IQuery<List<IConPOSPushPublish>>
    {
        public string RegionConnectionStringName { get; set; }
    }
}
