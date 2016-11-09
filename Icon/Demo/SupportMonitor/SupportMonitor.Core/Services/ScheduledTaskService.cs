using SupportMonitor.Core.Models;
using SupportMonitor.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportMonitor.Core.Services
{
    public class ScheduledTaskService
    {
        private IRepository<ScheduledTaskApplication> taskRepository;

        public IEnumerable<ScheduledTaskApplication> Get(string serverName, string applicationName)
        {
            return taskRepository.All();
        }
    }
}
