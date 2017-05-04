using System;

namespace Icon.Caching
{
    /// <summary>
    /// Interface for CachePolicy
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    public interface ICachePolicy<TQuery>
    {
        DateTime AbsoluteExpiration { get; set; }
        TimeSpan SlidingExpiration { get; set; }
    }
}
