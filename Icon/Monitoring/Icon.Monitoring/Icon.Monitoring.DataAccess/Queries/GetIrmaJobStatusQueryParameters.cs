namespace Icon.Monitoring.DataAccess.Queries
{
    using Icon.Common.DataAccess;
    using Icon.Monitoring.DataAccess.Model;

    public class GetIrmaJobStatusQueryParameters : IQuery<JobStatus>
    {
        public string Classname { get; set; }
    }
}
