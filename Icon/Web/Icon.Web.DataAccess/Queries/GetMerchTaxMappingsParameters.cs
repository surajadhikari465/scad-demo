using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetMerchTaxMappingsParameters : IQuery<List<MerchTaxMappingModel>>
    {
        public int MerchandiseHierarchyClassId { get; set; }
    }
}
