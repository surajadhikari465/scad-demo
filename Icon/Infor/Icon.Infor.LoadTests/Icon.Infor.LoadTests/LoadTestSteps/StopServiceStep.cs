using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.LoadTests.LoadTestSteps
{
    public class StopServiceStep : ILoadTestStep
    {
        public string ServiceName { get; set; }
        public string Server { get; set; }

        public StopServiceStep(string serviceName, string server)
        {
            ServiceName = serviceName;
            Server = server;
        }

        public LoadTestStepResult Execute()
        {
            var services = ServiceController.GetServices(Server)
                    .Where(s => s.ServiceName == ServiceName);

            foreach (var service in services)
            {
                if (service.Status == ServiceControllerStatus.Running)
                {
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);
                }
            }

            return true;
        }
    }
}
