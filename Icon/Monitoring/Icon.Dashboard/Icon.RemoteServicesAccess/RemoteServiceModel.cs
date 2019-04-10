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
            SystemName = wmiManagementObject["SystemName"].ToString();
            FullName = wmiManagementObject["Name"].ToString();
            DisplayName = wmiManagementObject["DisplayName"]?.ToString();
            Description = wmiManagementObject["Description"]?.ToString();
            ProcessId = int.Parse(wmiManagementObject["ProcessId"].ToString());
            StartMode = wmiManagementObject["StartMode"]?.ToString();
            RunningAs = wmiManagementObject["StartName"].ToString();
            State = wmiManagementObject["State"].ToString();
            ConfigFilePath = wmiManagementObject["PathName"].ToString();
        }

        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string ConfigFilePath { get; set; }
        public int? LoggingID { get; set; }
        public string LoggingName { get; set; }
        public string Environment { get; set; }
        public string State { get; set; }
        public int ProcessId { get; set; }
        public string StartMode { get; set; }
        public string RunningAs { get; set; }
        public string TypeOfApplication
        {
            get { return "WindowsService"; }
        }
    }
}
