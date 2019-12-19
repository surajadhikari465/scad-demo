using Icon.Framework;
using System.Collections.Generic;
using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Queries
{
    public class GetLocaleParameters : IQuery<List<Locale>>
    {
        public int? LocaleId { get; set; }
    }
}
