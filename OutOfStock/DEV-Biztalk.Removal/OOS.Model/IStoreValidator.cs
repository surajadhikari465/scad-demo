using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public interface IStoreValidator
    {
        void Validate(string storeAbbrev);
        void ValidateStoreInRegion(string regionAbbrev, string storeAbbrev);
    }
}
