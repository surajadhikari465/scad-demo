using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Common.Cache
{
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
