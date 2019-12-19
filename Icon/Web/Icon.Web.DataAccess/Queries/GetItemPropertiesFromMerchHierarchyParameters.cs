using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemPropertiesFromMerchHierarchyParameters : IQuery<MerchDependentItemPropertiesModel> 
    {
       public int MerchHierarchyClassId { get; set; }
    }
}