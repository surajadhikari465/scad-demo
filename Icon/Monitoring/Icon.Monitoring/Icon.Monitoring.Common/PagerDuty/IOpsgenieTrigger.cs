using OpsgenieAlert;
using System.Collections.Generic;

namespace Icon.Monitoring.Common.Opsgenie
{
    public interface IOpsgenieTrigger
    {
        OpsgenieResponse TriggerAlert(string message, string description="", Dictionary<string, string> details =null);
    }
}
