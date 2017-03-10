namespace Icon.Caching
{
    public static class CacheExtensions
    {
        public static T Get<T>(this ICache cache, string key)
        {
            return (T)cache.Get(key);
        }
    }
}
