using System;

namespace Icon.Caching
{
    /// <summary>
    /// Interface for Initializing, Setting, and Getting from Cache.
    /// </summary>
    public interface ICache
    {
        void Initialize();
        void Set(string key, object value);
        void Set(string key, object value, DateTime expiresAt);
        void Set(string key, object value, TimeSpan validFor);
        object Get(string key);
        void Remove(string key);
        bool Exists(string key);
    }
}
