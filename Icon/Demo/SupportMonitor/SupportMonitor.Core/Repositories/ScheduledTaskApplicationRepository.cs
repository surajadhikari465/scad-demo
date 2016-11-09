using SupportMonitor.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportMonitor.Core.Repositories
{
    public class ScheduledTaskApplicationRepository : IRepository<ScheduledTaskApplication>
    {
        private List<ScheduledTaskApplication> tasks;

        public ScheduledTaskApplicationRepository()
        {
            tasks = new List<ScheduledTaskApplication>();
        }

        public IEnumerable<ScheduledTaskApplication> All()
        {
            return tasks.AsEnumerable();
        }
    }
}
