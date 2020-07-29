using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetSkuBySkuIdParameters : IQuery<SkuModel>
    {
        public int SkuId { get; set; }
    }
}