using SupportMonitor.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportMonitor.Core.Repositories
{
    public class ServerRepository : IRepository<Server>
    {
        public IEnumerable<Server> All()
        {
            throw new NotImplementedException();
        }
    }
}
