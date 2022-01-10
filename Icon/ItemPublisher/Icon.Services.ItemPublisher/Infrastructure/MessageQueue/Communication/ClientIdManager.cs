using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue.Communication
{

    public class ClientIdManager : IClientIdManager
    {
        private Guid sessionGuid;
        private string computedClientId = string.Empty;

        public void Initialize(string appName)
        {
            sessionGuid = Guid.NewGuid();
            computedClientId = $"{appName}.{System.Environment.MachineName}.{sessionGuid}";
        }

        public string GetClientId()
        {
            return computedClientId.Substring(0, Math.Min(computedClientId.Length, 255));
        }

    }

    public interface IClientIdManager
    {
        string GetClientId();
        void Initialize(string appName);
    }

}
