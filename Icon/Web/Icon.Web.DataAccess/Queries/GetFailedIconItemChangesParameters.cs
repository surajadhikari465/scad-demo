using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Queries
{
    public class GetFailedIconItemChangesParameters : IQuery<List<IconItemChangeQueue>>
    {
        public string RegionConnectionStringName { get; set; }
    }
}
