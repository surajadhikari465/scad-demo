using System.Collections.Generic;

namespace PushController.Controller.CacheHelpers
{
    public interface ICacheHelper<TKey, TValue>
    {
        void Populate(List<TKey> dataToCache);
        TValue Retrieve(TKey key);
    }
}
