using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetMessageHistoryParameters : IQuery<List<MessageHistory>>
    {
        public List<int> MessageHistoriesById { get; set; }
    }
}
