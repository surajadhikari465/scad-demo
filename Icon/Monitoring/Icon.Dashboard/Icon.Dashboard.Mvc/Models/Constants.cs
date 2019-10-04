using Icon.Dashboard.Mvc.Controllers;
using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models.CustomConfigElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.Models
{
    public static class Constants
    {
        public const string CustomConfigSectionGroupName = nameof(DashboardCustomConfigSection);

        public static class DashboardAppSettings
        {
            public static class Keys
            {
                public const string HostingEnvironment = "hostingEnvironment";
                public const string ServiceCommandTimeoutMilliseconds = "serviceCommandTimeoutMilliseconds";
                public const string HoursForRecentErrors = "hoursForRecentErrors";
                public const string SecondsForRecentErrorsPolling = "intervalForRecentErrorsPollingInSeconds";
                public const string EnvironmentCookieDuration = "environmentCookieDurationHours";
                public const string SecurityGroupsReadOnly = "securityGroupsForReadOnly";
                public const string SecurityGroupsEditRights = "securityGroupsForEditing";

                public static ReadOnlyCollection<string> KeyList =>
                    new ReadOnlyCollection<string>(keyNames);

                private static List<string> keyNames = new List<string>
                {
                    HostingEnvironment,
                    ServiceCommandTimeoutMilliseconds,
                    HoursForRecentErrors,
                    SecondsForRecentErrorsPolling,
                    EnvironmentCookieDuration,
                    SecurityGroupsReadOnly,
                    SecurityGroupsEditRights
                };
            }
            public static class DefaultValues
            {
                public const EnvironmentEnum HostingEnvironmentEnum = EnvironmentEnum.Dev0;
                public const int ServiceCommandTimeoutMilliseconds = 10000;
                public const int HoursForRecentErros = 24;
                public const int SecondsForRecentErrorsPolling = 5;
                public const int EnvironmentCookieDurationHours = 1;
                public const string SecurityGroupsWithReadOnly = "IRMA.Applications";
                public const string SecurityGroupsWithEditRights = "IRMA.Developers";
                public const string EnvironmentNameCookieName = "environmentName";
                public const string EnvironmentAppServersCookieName = "environmentAppServers";
            }
        }       

        public static class EsbSettingKeys
        {
            // udpatable ESB settings (changed when re-assigning ESB environment)
            public const string ServerUrlKey = "ServerUrl";
            public const string TargetHostNameKey = "TargetHostName";
            public const string CertificateNameKey = "CertificateName";
            public const string JmsPasswordKey = "JmsPassword";
            public const string JndiPasswordKey = "JndiPassword";

            // ESB settings which are constant across ESB environments (non-updatable)
            public const string JmsUsernameKey = "JmsUsername";
            public const string JndiUsernameKey = "JndiUsername";
            public const string CertificateStoreNameKey = "CertificateStoreName";
            public const string CertificateStoreLocationKey = "CertificateStoreLocation";
            public const string ConnectionFactoryNameKey = "ConnectionFactoryName";
            public const string SslPasswordKey = "SslPassword";
            public const string SessionModeKey = "SessionMode";
            public const string ReconnectDelayKey = "ReconnectDelay";

            // ESB settings specific to only some apps 
            public const string QueueNameKey = "QueueName";
            public const string LocaleQueueNameKey = "LocaleQueueName";
            public const string HierarchyQueueNameKey = "HierarchyQueueName";
            public const string ItemQueueNameKey = "ItemQueueName";
            public const string ProductSelectionGroupQueueNameKey = "ProductSelectionGroupQueueName";

            // ESB settings for custom named elements
            public const string NameKey = "name";

            public static ReadOnlyCollection<string> KeyList =>
             new ReadOnlyCollection<string>(keyNames);
            private static List<string> keyNames = new List<string>
            {
                ServerUrlKey,
                TargetHostNameKey,
                CertificateNameKey,
                CertificateStoreNameKey,
                CertificateStoreLocationKey,
                JmsUsernameKey,
                JmsPasswordKey,
                JndiPasswordKey,
                JndiUsernameKey,
                ConnectionFactoryNameKey,
                SslPasswordKey,
                SessionModeKey,
                ReconnectDelayKey,
                QueueNameKey,
                LocaleQueueNameKey,
                HierarchyQueueNameKey,
                ItemQueueNameKey,
                ProductSelectionGroupQueueNameKey,
                NameKey
            };

            public static ReadOnlyCollection<string> UpdatableKeyList =>
                new ReadOnlyCollection<string>(updatableKeys);
            private static List<string> updatableKeys = new List<string>
            {
                ServerUrlKey,
                TargetHostNameKey,
                CertificateNameKey,
                JmsPasswordKey,
                JndiPasswordKey,
            };
        }

        public static class MvcNames
        {
            public static string HomeControllerName => nameof(HomeController).Replace("Controller","");
            public static string ApiJobControllerName => nameof(ApiJobsController).Replace("Controller","");
            public static string IconLogsControllerName => nameof(IconLogsController).Replace("Controller","");
            public static string MammothLogsControllerName => nameof(MammothLogsController).Replace("Controller","");
            public static string EsbControllerName => nameof(EsbController).Replace("Controller","");

            public static string HomeIndexActionName => nameof(HomeController.Index);
            public static string HomeSetAltEnvironmentActionName => nameof(HomeController.SetAlternateEnvironment);
            public static string HomeCustomActionName => nameof(HomeController.Custom);
            public static string IconLogsIndexActionName => nameof(IconLogsController.Index);
            public static string MammothLogsIndexActionName => nameof(MammothLogsController.Index);
            public static string ApiJobsIndexActionName => nameof(ApiJobsController.Index);
            public static string ApiJobsPendingActionName => nameof(ApiJobsController.Pending);
            public static string ApiJobsTableRefreshActionName => nameof(ApiJobsController.TableRefresh);
            public static string EsbControllerIndexActionName => nameof(EsbController.Index);

            public static class Views
            {
                public const string NotAuthorized = "~/Views/Shared/NotAuthorized.cshtml";
                public const string ReadOnlyAuthorized = "~/Views/Shared/ReadOnlyAuthorized.cshtml";
            }
        }
    }
}
