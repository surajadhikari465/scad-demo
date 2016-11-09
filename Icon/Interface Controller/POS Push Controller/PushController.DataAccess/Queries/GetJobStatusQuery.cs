using Irma.Framework;
using PushController.DataAccess.Interfaces;

namespace PushController.DataAccess.Queries
{
    public class GetJobStatusQuery : IQuery<JobStatus>
    {
        public IrmaContext Context { get; set; }
        public string JobName { get; set; }
    }
}
