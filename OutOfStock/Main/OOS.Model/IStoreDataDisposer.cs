using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public interface IStoreDataDisposer
    {
        int DeleteStoreData(string storeAbbrev, DateTime startDate, DateTime endDate);
        int DeleteAllStoreData(string storeAbbrev);
    }
}
