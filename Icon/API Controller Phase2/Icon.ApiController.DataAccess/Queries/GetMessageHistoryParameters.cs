
using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetMessageHistoryParameters : IQuery<List<MessageHistory>>
    {
        public int MessageTypeId { get; set; }
        public int Instance { get; set; }
    }
}
