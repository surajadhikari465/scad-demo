using BulkItemUploadProcessor.Common.Models;
using System.Collections.Generic;

namespace BulkItemUploadProcessor.Service.Cache.Interfaces
{
    public interface IMerchItemPropertiesCache
    {
        void Refresh();
        Dictionary<int, MerchPropertiesModel> Properties { get; set; }

    }
}