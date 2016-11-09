using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;

namespace Icon.Web.DataAccess.Queries
{
    public class GetPluRequestByIdParameters : IQuery<PLURequest>
    {
        public int PluRequestId { get; set; }
    }
}
