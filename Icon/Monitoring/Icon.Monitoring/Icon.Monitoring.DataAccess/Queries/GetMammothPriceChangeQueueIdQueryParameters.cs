namespace Icon.Monitoring.DataAccess.Queries
{
    using Icon.Common.DataAccess;

    public class GetMammothPriceChangeQueueIdQueryParameters : IQuery<int>
    {
        public bool EmptyParameter { get; set; }
    }
}
