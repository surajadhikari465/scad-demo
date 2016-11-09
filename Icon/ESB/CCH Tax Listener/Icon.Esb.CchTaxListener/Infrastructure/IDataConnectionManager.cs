using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.CchTax.Infrastructure
{
    public interface IDataConnectionManager
    {
        IDataConnection Connection { get; set; }
        IDataConnection InitializeConnection(string connectionString);
    }
}
