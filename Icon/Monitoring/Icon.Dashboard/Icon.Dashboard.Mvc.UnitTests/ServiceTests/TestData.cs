using Icon.Dashboard.Mvc.ViewModels;
using Icon.Dashboard.RemoteServicesAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.ServiceTests
{
    public class TestData
    {
        public List<IconLoggedAppViewModel> IconApps = new List<IconLoggedAppViewModel>
        {
            new IconLoggedAppViewModel { AppID = 1, AppName = "Web App" },
            new IconLoggedAppViewModel { AppID = 2, AppName = "Interface Controller" },
            new IconLoggedAppViewModel { AppID = 3, AppName = "ESB Subscriber" },
            new IconLoggedAppViewModel { AppID = 4, AppName = "Icon Service" },
            new IconLoggedAppViewModel { AppID = 5, AppName = "API Controller" },
            new IconLoggedAppViewModel { AppID = 6, AppName = "POS Push Controller" },
            new IconLoggedAppViewModel { AppID = 7, AppName = "Global Controller" },
            new IconLoggedAppViewModel { AppID = 8, AppName = "Regional Controller" },
            new IconLoggedAppViewModel { AppID = 9, AppName = "Subteam Controller" },
            new IconLoggedAppViewModel { AppID = 10, AppName = "Icon Data Purge" },
            new IconLoggedAppViewModel { AppID = 11, AppName = "TLog Controller" },
            new IconLoggedAppViewModel { AppID = 12, AppName = "Nutrition Web API" },
            new IconLoggedAppViewModel { AppID = 13, AppName = "Vim Locale Controller" },
            new IconLoggedAppViewModel { AppID = 14, AppName = "Icon Monitoring" },
            new IconLoggedAppViewModel { AppID = 15, AppName = "Infor New Item Service" },
            new IconLoggedAppViewModel { AppID = 16, AppName = "Infor Hierarchy Class Listener" },
            new IconLoggedAppViewModel { AppID = 17, AppName = "Infor Item Listener" },
            new IconLoggedAppViewModel { AppID = 18, AppName = "Icon Web Api" },
        };

        public List<IconLoggedAppViewModel> MammothApps = new List<IconLoggedAppViewModel>
        {
            new IconLoggedAppViewModel { AppID = 1, AppName = "Web Api" },
            new IconLoggedAppViewModel { AppID = 2, AppName = "ItemLocale Controller" },
            new IconLoggedAppViewModel { AppID = 3, AppName = "Price Controller" },
            new IconLoggedAppViewModel { AppID = 4, AppName = "API Controller" },
            new IconLoggedAppViewModel { AppID = 5, AppName = "Product Listener" },
            new IconLoggedAppViewModel { AppID = 6, AppName = "Locale Listener" },
            new IconLoggedAppViewModel { AppID = 7, AppName = "Hierarchy Class Listener" },
            new IconLoggedAppViewModel { AppID = 8, AppName = "Mammoth Data Purge" },
            new IconLoggedAppViewModel { AppID = 9, AppName = "Price Listener" },
            new IconLoggedAppViewModel { AppID = 10, AppName = "Price Message Archiver" },
            new IconLoggedAppViewModel { AppID = 11, AppName = "Active Price Service" },
            new IconLoggedAppViewModel { AppID = 12, AppName = "Active Price Message Archiver" },
            new IconLoggedAppViewModel { AppID = 13, AppName = "R10 Price Service" },
            new IconLoggedAppViewModel { AppID = 14, AppName = "IRMA Price Service" },
            new IconLoggedAppViewModel { AppID = 15, AppName = "Mammoth Web Support" },
            new IconLoggedAppViewModel { AppID = 16, AppName = "Error Message Listener" },
            new IconLoggedAppViewModel { AppID = 17, AppName = "Error Message Monitor" },
            new IconLoggedAppViewModel { AppID = 18, AppName = "Job Scheduler" },
            new IconLoggedAppViewModel { AppID = 19, AppName = "Expiring Tpr Service" },
            new IconLoggedAppViewModel { AppID = 20, AppName = "Prime Affinity Listener" },
            new IconLoggedAppViewModel { AppID = 21, AppName = "Prime Affinity Controller" },
            new IconLoggedAppViewModel { AppID = 22, AppName = "Emergency Price Service" },
            new IconLoggedAppViewModel { AppID = 23, AppName = "OnePlum Listener" },
            new IconLoggedAppViewModel { AppID = 24, AppName = "Esl Listener" },
        };

        public List<EsbEnvironmentViewModel> EsbEnvironments = new List<EsbEnvironmentViewModel>
        {
            new EsbEnvironmentViewModel { Name = "DEV", ServerUrl = "ssl://cerd1616.wfm.pvt:7233", TargetHostName = "cerd1616.wfm.pvt", JmsUsername = "iconUser", JmsPassword = "ouVgOD5ld5V6", JndiUsername = "jndiIconUser", JndiPassword = "jndiIconUser" },
            new EsbEnvironmentViewModel { Name = "DEV-DUP", ServerUrl = "ssl://cerd1636.wfm.pvt:7233", TargetHostName = "cerd1636.wfm.pvt", JmsUsername = "iconUser", JmsPassword = "ouVgOD5ld5V6", JndiUsername = "jndiIconUser", JndiPassword = "jndiIconUser" },
            new EsbEnvironmentViewModel { Name = "TEST", ServerUrl = "ssl://cerd1617.wfm.pvt:7233", TargetHostName = "cerd1617.wfm.pvt", JmsUsername = "iconUser", JmsPassword = "Pjetuc9M7Kmi", JndiUsername = "jndiIconUser", JndiPassword = "jndiIconUser" },
            new EsbEnvironmentViewModel { Name = "TEST-DUP", ServerUrl = "ssl://cerd1637.wfm.pvt:17293", TargetHostName = "cerd1637.wfm.pvt", JmsUsername = "iconUser", JmsPassword = "Pjetuc9M7Kmi", JndiUsername = "jndiIconUser", JndiPassword = "jndiIconUser" },
            new EsbEnvironmentViewModel { Name = "QA-FUNC", ServerUrl = "ssl://cerd1619.wfm.pvt:27293, ssl://cerd1622.wfm.pvt:27293 ", TargetHostName = "cerd1619.wfm.pvt,cerd1622.wfm.pvt", JmsUsername = "iconUser", JmsPassword = "Icond3Up", JndiUsername = "iconUser", JndiPassword = "jndiI8conUserdUp" },
            new EsbEnvironmentViewModel { Name = "QA-DUP", ServerUrl = "ssl://cerd1639.wfm.pvt:27293, ssl://cerd1640.wfm.pvt:27293 ", TargetHostName = "cerd1639.wfm.pvt,cerd1640.wfm.pvt", JmsUsername = "iconUser", JmsPassword = "Icond3Up", JndiUsername = "iconUser", JndiPassword = "jndiI8conUserdUp" },
            new EsbEnvironmentViewModel { Name = "QA-PERF", ServerUrl = "ssl://cerd1630.wfm.pvt:27293, ssl://cerd1631.wfm.pvt:27293 ", TargetHostName = "cerd1630.wfm.pvt,cerd1631.wfm.pvt", JmsUsername = "iconUser", JmsPassword = "Icond3Up", JndiUsername = "iconUser", JndiPassword = "jndiI8conUserdUp" },
        };

        public RemoteServiceModel SampleGloconService = new RemoteServiceModel
        {
            SystemName = "CEWD6592",
            DisplayName = "Icon Global Event Controller Service",
            FullName = "GlobalEventControllerService",
            Description = "Processes Global Event Controller from IRMA to Icon.",
            ConfigFilePath = @"E:\SampleAppConfig_A.exe.config",
            LoggingID = 0,
            LoggingName = "",
            Environment = "TEST",
            State = "Running",
            ProcessId = 111,
            StartMode = "Auto",
            RunningAs = @"wfm\IconTestUserDev"
        };
    }
}
