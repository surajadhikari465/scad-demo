using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Queries
{
    public class GetLocalesByChainParameters : IQuery<List<Locale>>
    {
        public string ChainName { get; set; }
    }
}
