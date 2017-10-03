using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    public class GetJobScheduleParameters : IQuery<JobSchedule>
    {
        public int JobScheduleId { get; set; }
    }
}
