using Icon.Common.DataAccess;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetApiMessageUnprocessedRowCountParameters : IQuery<int>
    {
        public string MessageQueueType { get; set; }

        public string RegionCode { get; set; }
    }
}
