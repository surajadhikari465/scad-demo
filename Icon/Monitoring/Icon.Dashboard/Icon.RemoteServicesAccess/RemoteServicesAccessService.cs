using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Management;
using System.Security;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.RemoteServicesAccess
{
    public class RemoteServicesAccessService : IRemoteServicesAccessService
    {
        public RemoteServicesAccessService() { }

        public List<RemoteServiceModel> LoadRemoteServicesData(List<string> hosts, List<string> searchTerms)
        {
            var serviceModels = new List<RemoteServiceModel>();
            if (hosts != null || hosts.Count > 0)
            {
                StringBuilder wqlQuery = new StringBuilder();
                wqlQuery.Append("SELECT * FROM Win32_Service");
                if (searchTerms != null && searchTerms.Count > 0)
                {
                    wqlQuery.Append(" WHERE");
                    for (int i = 0; i < searchTerms.Count; i++)
                    {
                        wqlQuery.Append($" NAME Like '{searchTerms[i]}%'");
                        if (i < searchTerms.Count - 1)
                        {
                            wqlQuery.Append($" or ");
                        }
                    }
                }

                foreach (var host in hosts)
                {
                    serviceModels.AddRange(LoadRemoteServicesData(host, wqlQuery.ToString()));
                }
            }
            return serviceModels;
        }

        public List<RemoteServiceModel> LoadRemoteServicesData(string host, string wqlQuery)
        {
            var serviceModels = new List<RemoteServiceModel>();
            if (!string.IsNullOrWhiteSpace(host))
            {
                string providerPath = $@"\\{host}\root\cimv2";
                ObjectQuery objectQuery = new ObjectQuery(wqlQuery);
                ConnectionOptions options = new ConnectionOptions();

                try
                {
                    ManagementScope mgmtScope = new ManagementScope(providerPath, options);
                    mgmtScope.Connect();

                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(mgmtScope, objectQuery);
                    ManagementObjectCollection retObjectCollection = searcher.Get();
                    foreach (ManagementObject mo in retObjectCollection)
                    {
                        serviceModels.Add(new RemoteServiceModel(mo));
                    }
                }
                catch (Exception ex)
                {
                    var errMsg = ex.Message;
                }
            }
            return serviceModels;
        }
    }
}