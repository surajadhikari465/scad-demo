using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetLocalesByChainParameters : IQuery<List<Locale>>
    {
        public string ChainName { get; set; }
    }
}
