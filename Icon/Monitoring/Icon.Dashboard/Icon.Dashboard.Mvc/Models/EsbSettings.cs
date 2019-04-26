using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.Models
{
    public static class EsbAppSettings
    {
        public const string ServerUrlKey = "ServerUrl";
        public const string TargetHostNameKey = "TargetHostName";
        public const string JmsUsernameKey = "JmsUsername";
        public const string JmsPasswordKey = "JmsPassword";
        public const string JndiPasswordKey = "JndiPassword";
        public const string JndiUsernameKey = "JndiUsername";

        public static ReadOnlyCollection<string> EsbAppSettingsNames =>
                new ReadOnlyCollection<string>(esbEnvironmentPropertyNames);

        private static List<string> esbEnvironmentPropertyNames = new List<string>
        {
            ServerUrlKey,
            TargetHostNameKey,
            JmsUsernameKey,
            JmsPasswordKey,
            JndiPasswordKey,
            JndiUsernameKey,
        };
    }
}
