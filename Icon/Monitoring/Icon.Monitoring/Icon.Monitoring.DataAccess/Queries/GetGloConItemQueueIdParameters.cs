using Icon.Common.DataAccess;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetGloConItemQueueIdParameters : IQuery<int>
    {
        public bool EmptyParameter { get; set; }
    }
}