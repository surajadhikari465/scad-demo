using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.RemoteServicesAccess
{
    public class RemoteServiceModel
    {
        public RemoteServiceModel() { }

        public RemoteServiceModel(ManagementObject wmiManagementObject) : this()
        {
            Server = wmiManagementObject["SystemName"].ToString();
            FullName = wmiManagementObject["Name"].ToString();
            DisplayName = wmiManagementObject["DisplayName"].ToString();
            Description = wmiManagementObject["Description"].ToString();

            var pathName = wmiManagementObject["PathName"].ToString();
            if (pathName.Contains("-"))
            {
                pathName = pathName.Substring(0, pathName.IndexOf('-'));
            }
            var remotePath = pathName.Replace(":", "$").Replace("\"", "").Trim();
            var exePath = $"\\\\{Server}\\{remotePath}";
            var config = ConfigurationManager.OpenExeConfiguration(exePath);
            ConfigFilePath = config.FilePath;
        }

        public string Server { get; set; }
        public string DisplayName { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string ConfigFilePath { get; set; }
        public int? LoggingID { get; set; }
        public string LoggingName { get; set; }
        public string Environment { get; set; }
        public string TypeOfApplication
        {
            get { return "WindowsService"; }
        }
    }
}
