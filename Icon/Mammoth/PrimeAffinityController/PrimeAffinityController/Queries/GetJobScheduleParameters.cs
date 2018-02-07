using Mammoth.Common.DataAccess.CommandQuery;
using PrimeAffinityController.Models;

namespace PrimeAffinityController.Queries
{
    public class GetJobScheduleParameters : IQuery<JobScheduleModel>
    {
        public string JobName { get; set; }
        public string Region { get; set; }
    }
}
