using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationDashboard.Helpers
{
    public static class CssHelper
    {
        public static string AppStatus(string status)
        {
            return status == "Running" ? "status-running" : "";
        }
    }
}
