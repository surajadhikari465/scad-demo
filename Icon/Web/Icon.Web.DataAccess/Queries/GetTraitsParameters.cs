using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetTraitsParameters : IQuery<List<Trait>>
    {
        public bool IncludeNavigation { get; set; }
    }
}
