using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemParameters : IQuery<ItemDbModel>
    {
        public string ScanCode { get; set; }
    }
}