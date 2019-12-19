using System;
using System.Collections.Generic;
using OOSCommon.Import;

namespace OOSCommon
{
    public interface IBuildKnownUpload
    {
        void Build(DateTime date, IEnumerable<OOSKnownItemData> itemData, IEnumerable<OOSKnownVendorRegionMap> maps);
        KnownUpload ToKnownUpload();
    }
}
