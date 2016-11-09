using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Common.Cache
{
    public interface ICachePolicy<TQuery>
    {
        DateTime AbsoluteExpiration { get; set; }
        TimeSpan SlidingExpiration { get; set; }
    }
}
