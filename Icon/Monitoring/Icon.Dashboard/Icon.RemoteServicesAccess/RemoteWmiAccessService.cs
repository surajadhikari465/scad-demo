using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.RemoteServicesAccess
{
    public class RemoteWmiAccessService : IRemoteWmiAccessService
    {
        private string serviceNameQueryString = "Name LIKE 'Mammoth%' or Name LIKE 'Icon%' or Name LIKE  'Infor%' or Name LIKE 'Nutri%'";

        public RemoteServiceModel LoadRemoteService(string host, string application)
        {
            string wqlQuery = $"SELECT * FROM Win32_Service WHERE Name = '{application}'";
            return LoadRemoteServices(host, wqlQuery).FirstOrDefault();
        } 

        public List<RemoteServiceModel> LoadRemoteServices(string host)
        {
            string wqlQuery = $"SELECT * FROM Win32_Service WHERE {serviceNameQueryString}";
            return LoadRemoteServices(host, wqlQuery);
        }

        public List<RemoteServiceModel> LoadRemoteServices(IList<string> hosts)
        {
            var serviceModels = new List<RemoteServiceModel>();
            if (hosts != null || hosts.Count > 0)
            {
                foreach (var host in hosts)
                {
                    serviceModels.AddRange(LoadRemoteServices(host));
                }
            }
            return serviceModels;
        }

        public List<RemoteServiceModel> LoadRemoteServices(string host, string wqlQuery)
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
