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
        private string serviceNameQueryString = "DisplayName LIKE 'Mammoth%' or DisplayName LIKE 'Icon%' or DisplayName LIKE 'Infor%' or DisplayName LIKE 'Nutri%'";

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

        public ManagementObjectCollection GetManagementObjectCollection(string host, string application)
        {
            string wqlQuery = $"SELECT * FROM Win32_Service WHERE Name = '{application}'";
            string providerPath = $@"\\{host}\root\cimv2";

            ObjectQuery objectQuery = new ObjectQuery(wqlQuery);
            ConnectionOptions options = new ConnectionOptions();

            ManagementScope mgmtScope = new ManagementScope(providerPath, options);
            mgmtScope.Connect();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(mgmtScope, objectQuery);
            ManagementObjectCollection retObjectCollection = searcher.Get();

            return retObjectCollection;
        }

        public void StartRemoteService(string host, string application, string[] args)
        {
            var wmiManagementObjects = GetManagementObjectCollection(host, application);

            foreach (ManagementObject mo in wmiManagementObjects)
            {
                mo.InvokeMethod("StartService", args);
            }
        }        

        public void StopRemoteService(string host, string application, string[] args)
        {
            var wmiManagementObjects = GetManagementObjectCollection(host, application);

            foreach (ManagementObject mo in wmiManagementObjects)
            {
                mo.InvokeMethod("StopService", args);
            }
        }
    }
}
