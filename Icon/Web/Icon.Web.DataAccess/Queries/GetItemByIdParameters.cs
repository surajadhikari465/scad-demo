using Icon.Common.DataAccess;
using Icon.Framework;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemByIdParameters : IQuery<Item>
    {
        public int ItemId { get; set; }
    }
}
