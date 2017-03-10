using System;

namespace Icon.Caching
{
    public interface ICachePolicy<TQuery>
    {
        DateTime AbsoluteExpiration { get; set; }
        TimeSpan SlidingExpiration { get; set; }
    }
}
