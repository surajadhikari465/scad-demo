using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public interface IStoreRepository
    {
        Store ForName(string storeName);
        Store ForAbbrev(string storeAbbrev);
        void Add(Store store);
        void Update(Store store);
    }
}
