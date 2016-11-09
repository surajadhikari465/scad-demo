using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetMessageQueueParameters<T> : IQuery<List<T>>
    {
        public int Instance { get; set; }
        public int MessageQueueStatusId { get; set; }
    }
}
