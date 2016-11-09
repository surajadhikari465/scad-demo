namespace Icon.Monitoring.DataAccess.Queries
{
    using System.Linq;

    using Dapper;
    using Model;
    using Common.Enums;

    public class GetIrmaJobStatusQuery : IQueryByRegionHandler<GetIrmaJobStatusQueryParameters, JobStatus>
    {
        private const string JobStatusQuery = @"SELECT * FROM [dbo].[JobStatus] (nolock) WHERE Classname = '{0}'";

        private IDbProvider db;

        public IrmaRegions TargetRegion { get; set; }

        public GetIrmaJobStatusQuery(IDbProvider db)
        {
            this.db = db;
        }

        public JobStatus Search(GetIrmaJobStatusQueryParameters parameters)
        {

            var sqlQuery = string.Format(JobStatusQuery, parameters.Classname);
         
            var pushJob = this.db.Connection.Query<JobStatus>(sqlQuery, transaction: this.db.Transaction).FirstOrDefault();
            if(pushJob != null)
            {
                pushJob.Region = TargetRegion;
            }

            return pushJob;
        }
    }
}
