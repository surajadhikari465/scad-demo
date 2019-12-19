using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOSCommon.Import;

namespace OOS.Model
{
    public interface IEventCreatorFactory
    {
        IOOSUpdateReported ForStore(string name);
        IOOSUpdateReported ForStoreInRegion(string region, string name);
        IOOSUpdateReported ForStoreByAbbreviation(string storeAbbrev);
    }
}
