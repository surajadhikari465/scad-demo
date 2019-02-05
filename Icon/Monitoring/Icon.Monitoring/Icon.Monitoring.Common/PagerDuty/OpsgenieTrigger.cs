using Icon.Monitoring.Common.Settings;
using OpsgenieAlert;
using System.Collections.Generic;
using OpsGenieAlertSender = OpsgenieAlert.OpsgenieAlert;

namespace Icon.Monitoring.Common.Opsgenie
{
    public class OpsgenieTrigger : IOpsgenieTrigger
    {
        private IMonitorSettings settings;

        public OpsgenieTrigger(IMonitorSettings settings)
        {
            this.settings = settings;
        }

       public OpsgenieResponse TriggerAlert(string message, string description = "", Dictionary<string, string> details = null)
        {
            OpsGenieAlertSender opsGenieAlertSender = new OpsGenieAlertSender();

          var opsgenieResponse=   opsGenieAlertSender.CreateOpsgenieAlert(this.settings.IntegrationKey, this.settings.OpsGenieUri,
                                                     message, description, details);
 
            return opsgenieResponse;
        }
    }
}
