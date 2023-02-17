using System.Collections.Generic;

namespace GPMService.Producer.Helpers
{
    public static class UtilityFunctions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) =>
            dictionary.TryGetValue(key, out var ret) ? ret : default(TValue);
    }
}
