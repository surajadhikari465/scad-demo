using System;

namespace WebSupport.Managers
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
}