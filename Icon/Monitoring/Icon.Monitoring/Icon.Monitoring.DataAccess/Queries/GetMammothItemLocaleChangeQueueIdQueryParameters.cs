namespace Icon.Monitoring.DataAccess.Queries
{
    using Icon.Common.DataAccess;

    public class GetMammothItemLocaleChangeQueueIdQueryParameters : IQuery<int>
    {
        public bool EmptyParameter { get; set; }
    }
}
