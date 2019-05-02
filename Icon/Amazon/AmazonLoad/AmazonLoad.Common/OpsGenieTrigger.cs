using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpsgenieAlert;
using OpsGenieAlertSender = OpsgenieAlert.OpsgenieAlert;

namespace AmazonLoad.Common
{
    public class OpsgenieTrigger : IOpsgenieTrigger
    {
        public OpsgenieResponse TriggerAlert(string message, string description = "", Dictionary<string, string> details = null)
        {
            OpsGenieAlertSender opsGenieAlertSender = new OpsGenieAlertSender();

            var opsgenieResponse = opsGenieAlertSender.CreateOpsgenieAlert(ConfigurationManager.AppSettings["IntegrationKey"], ConfigurationManager.AppSettings["OpsGenieUri"],
                                                       message, description, details);

            return opsgenieResponse;
        }
    }

       
}
