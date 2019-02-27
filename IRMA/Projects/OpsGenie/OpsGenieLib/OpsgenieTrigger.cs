using OpsgenieAlert;
using System.Collections.Generic;
using WholeFoods.Utility;
using OpsGenieAlertSender = OpsgenieAlert.OpsgenieAlert;

namespace OpsgenieLib
{
    public class OpsgenieTrigger
    {
        private string url;
        private string serviceKey;

        public OpsgenieTrigger(string url, string serviceKey)
        {
            this.url = url;
            this.serviceKey = serviceKey;
        }

        public OpsgenieResponse TriggerAlert(string message, string description = "", Dictionary<string, string> details = null)
        {
            OpsGenieAlertSender opsGenieAlertSender = new OpsGenieAlertSender();

            var opsgenieResponse = opsGenieAlertSender.CreateOpsgenieAlert(serviceKey, url, message, description, details);

            return opsgenieResponse;
        }

        public static OpsgenieTrigger CreateFromConfig()
        {
            string url = ConfigurationServices.AppSettings("ErrorAlertUrl");
            string serviceKey = ConfigurationServices.AppSettings("ErrorAlertServiceKey");
            return new OpsgenieTrigger(url, serviceKey);
        }
    }
}