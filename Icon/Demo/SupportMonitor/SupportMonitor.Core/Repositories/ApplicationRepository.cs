using SupportMonitor.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportMonitor.Core.Repositories
{
    public class ApplicationRepository : IRepository<Application>
    {
        public IEnumerable<Application> All()
        {
            throw new NotImplementedException();
        }
    }
}
