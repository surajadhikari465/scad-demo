using Icon.Monitoring.Common.Settings;
using PagerDutyAPI;
using System;
using System.Collections.Generic;

namespace Icon.Monitoring.Common.PagerDuty
{
    public class PagerDutyTrigger : IPagerDutyTrigger
    {
        private IMonitorSettings settings;

        public PagerDutyTrigger(IMonitorSettings settings)
        {
            this.settings = settings;
        }

        public PagerDutyResponse TriggerIncident(string description, Dictionary<string, string> details)
        {
            APIClientInfo apiClientInfo = new APIClientInfo("PagerDuty", this.settings.PagerDutyUri);
            var client = IntegrationAPI.MakeClient(apiClientInfo, this.settings.IntegrationKey);
            EventAPIResponse response = client.Trigger(
                String.Format("{0} - " + description, this.settings.Environment),
                details);

            PagerDutyResponse pagerDutyResponse = new PagerDutyResponse(response);
            return pagerDutyResponse;
        }
    }
}
