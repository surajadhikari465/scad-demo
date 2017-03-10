namespace Icon.Caching
{
    /// <summary>
    /// Helpful extension for getting from the cache.
    /// </summary>
    public static class CacheExtensions
    {
        public static T Get<T>(this ICache cache, string key)
        {
            return (T)cache.Get(key);
        }
    }
}
