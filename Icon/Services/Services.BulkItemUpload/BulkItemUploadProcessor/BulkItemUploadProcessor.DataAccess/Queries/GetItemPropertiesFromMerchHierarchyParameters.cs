using BulkItemUploadProcessor.Common.Models;
using Icon.Common.DataAccess;

namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class GetItemPropertiesFromMerchHierarchyParameters : IQuery<MerchDependentItemPropertiesModel>
    {
        public int MerchHierarchyClassId { get; set; }
    }
}