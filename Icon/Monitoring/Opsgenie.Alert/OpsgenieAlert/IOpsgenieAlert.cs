﻿using System.Collections.Generic;

namespace OpsgenieAlert
{
    public interface IOpsgenieAlert
    {
        void CreateOpsgenieAlert(string message, string description, Dictionary<string,
                                               string> details, string api, string url);
    }
}