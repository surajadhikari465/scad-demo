using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Extract.Credentials
{
    public interface IActiveMqCredentialsCache
    {
        Dictionary<string, ActiveMqCredential> Credentials { get; set; }
        void Refresh();
    }
}
