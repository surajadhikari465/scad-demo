using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.LoadTests.LoadTestSteps
{
    public class StartServiceStep : ILoadTestStep
    {
        public string ServiceName { get; set; }
        public string Server { get; set; }

        public StartServiceStep(string serviceName, string server)
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
                if (service.Status != ServiceControllerStatus.Running)
                {
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running);
                }
            }

            return true;
        }
    }
}
