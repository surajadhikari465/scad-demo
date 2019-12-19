using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public interface ISkuCountRepository
    {
        SkuCount For(string storeAbbrev, string team);
        int Count { get; }
        void Insert(string storeAbbrev, string team, int count);
        void Modify(string storeAbbrev, string team, int count);
        void Remove(string storeAbbrev, string team);
    }
}
