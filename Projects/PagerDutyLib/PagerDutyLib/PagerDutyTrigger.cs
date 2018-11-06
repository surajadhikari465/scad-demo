using System.Collections.Generic;
using WholeFoods.Utility;

namespace PagerDutyLib
{
    public class PagerDutyTrigger
    {
        private string clientName;
        private string url;
        private string serviceKey;

        public PagerDutyTrigger(string clientName, string url, string serviceKey)
        {
            this.clientName = clientName;
            this.url = url;
            this.serviceKey = serviceKey;
        }

        public EventAPIResponse TriggerIncident(string description, Dictionary<string, string> details)
        {
            APIClientInfo apiClientInfo = new APIClientInfo(clientName, url);

            var client = IntegrationAPI.MakeClient(apiClientInfo, serviceKey);

            EventAPIResponse response = client.Trigger(
                description,
                details);

            return response;
        }

        public static PagerDutyTrigger CreateFromConfig(string clientName)
        {
            string url = ConfigurationServices.AppSettings("PagerDutyUrl");
            string serviceKey = ConfigurationServices.AppSettings("PagerDutyServiceKey");
            return new PagerDutyTrigger(clientName, url, serviceKey);
        }
    }
}
