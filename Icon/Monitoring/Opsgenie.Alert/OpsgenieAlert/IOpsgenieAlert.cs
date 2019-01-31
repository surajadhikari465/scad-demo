using System.Collections.Generic;

namespace OpsgenieAlert
{
    public interface IOpsgenieAlert
    {
        OpsgenieResponse CreateOpsgenieAlert(string api, string url, string message, string description = "",
                                                    Dictionary<string, string> details = null);
    }
}