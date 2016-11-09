using SupportMonitor.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SupportMonitor.Core
{
    public class ApplicationReader
    {
        public List<ServiceApplication> GetApplicationsOnServer(string serverName, IEnumerable<string> applicationNames)
        {
            List<ServiceApplication> serverApplications = null;
            using (var instance = PowerShell.Create())
            {
                instance.AddScript("Get-Service");
                var result = instance.Invoke<ServiceController>();
                serverApplications = result.Select(
                    pso => new ServiceApplication
                    {
                            Name = pso.ServiceName,
                            Status = pso.Status.ToString()
                        }
                    )
                    .ToList();
            }

            return serverApplications;
        }
    }
}
