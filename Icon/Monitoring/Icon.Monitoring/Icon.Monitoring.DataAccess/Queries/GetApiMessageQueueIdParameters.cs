using Icon.Common.DataAccess;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetApiMessageQueueIdParameters : IQuery<int>
    {
        public string MessageQueueType { get; set; }
    }
}