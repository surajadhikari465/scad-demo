using SupportMonitor.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportMonitor.Core.Repositories
{
    public class ServiceApplicationRepository : IRepository<ServiceApplication>
    {
        public IEnumerable<ServiceApplication> All()
        {
            throw new NotImplementedException();
        }
    }
}
