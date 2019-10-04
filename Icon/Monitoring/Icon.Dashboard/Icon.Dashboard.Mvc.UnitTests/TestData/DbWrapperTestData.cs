using Icon.Dashboard.Mvc.ViewModels;
using Icon.Dashboard.RemoteServicesAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.TestData
{
    public class DbWrapperTestData
    {
        public List<LoggedAppViewModel> IconApps = new List<LoggedAppViewModel>
        {
            new LoggedAppViewModel { AppID = 1, AppName = "Web App" },
            new LoggedAppViewModel { AppID = 2, AppName = "Interface Controller" },
            new LoggedAppViewModel { AppID = 3, AppName = "ESB Subscriber" },
            new LoggedAppViewModel { AppID = 4, AppName = "Icon Service" },
            new LoggedAppViewModel { AppID = 5, AppName = "API Controller" },
            new LoggedAppViewModel { AppID = 6, AppName = "POS Push Controller" },
            new LoggedAppViewModel { AppID = 7, AppName = "Global Controller" },
            new LoggedAppViewModel { AppID = 8, AppName = "Regional Controller" },
            new LoggedAppViewModel { AppID = 9, AppName = "Subteam Controller" },
            new LoggedAppViewModel { AppID = 10, AppName = "Icon Data Purge" },
            new LoggedAppViewModel { AppID = 11, AppName = "TLog Controller" },
            new LoggedAppViewModel { AppID = 12, AppName = "Nutrition Web API" },
            new LoggedAppViewModel { AppID = 13, AppName = "Vim Locale Controller" },
            new LoggedAppViewModel { AppID = 14, AppName = "Icon Monitoring" },
            new LoggedAppViewModel { AppID = 15, AppName = "Infor New Item Service" },
            new LoggedAppViewModel { AppID = 16, AppName = "Infor Hierarchy Class Listener" },
            new LoggedAppViewModel { AppID = 17, AppName = "Infor Item Listener" },
            new LoggedAppViewModel { AppID = 18, AppName = "Icon Web Api" },
        };

        public List<LoggedAppViewModel> MammothApps = new List<LoggedAppViewModel>
        {
            new LoggedAppViewModel { AppID = 1, AppName = "Web Api" },
            new LoggedAppViewModel { AppID = 2, AppName = "ItemLocale Controller" },
            new LoggedAppViewModel { AppID = 3, AppName = "Price Controller" },
            new LoggedAppViewModel { AppID = 4, AppName = "API Controller" },
            new LoggedAppViewModel { AppID = 5, AppName = "Product Listener" },
            new LoggedAppViewModel { AppID = 6, AppName = "Locale Listener" },
            new LoggedAppViewModel { AppID = 7, AppName = "Hierarchy Class Listener" },
            new LoggedAppViewModel { AppID = 8, AppName = "Mammoth Data Purge" },
            new LoggedAppViewModel { AppID = 9, AppName = "Price Listener" },
            new LoggedAppViewModel { AppID = 10, AppName = "Price Message Archiver" },
            new LoggedAppViewModel { AppID = 11, AppName = "Active Price Service" },
            new LoggedAppViewModel { AppID = 12, AppName = "Active Price Message Archiver" },
            new LoggedAppViewModel { AppID = 13, AppName = "R10 Price Service" },
            new LoggedAppViewModel { AppID = 14, AppName = "IRMA Price Service" },
            new LoggedAppViewModel { AppID = 15, AppName = "Mammoth Web Support" },
            new LoggedAppViewModel { AppID = 16, AppName = "Error Message Listener" },
            new LoggedAppViewModel { AppID = 17, AppName = "Error Message Monitor" },
            new LoggedAppViewModel { AppID = 18, AppName = "Job Scheduler" },
            new LoggedAppViewModel { AppID = 19, AppName = "Expiring Tpr Service" },
            new LoggedAppViewModel { AppID = 20, AppName = "Prime Affinity Listener" },
            new LoggedAppViewModel { AppID = 21, AppName = "Prime Affinity Controller" },
            new LoggedAppViewModel { AppID = 22, AppName = "Emergency Price Service" },
            new LoggedAppViewModel { AppID = 23, AppName = "OnePlum Listener" },
            new LoggedAppViewModel { AppID = 24, AppName = "Esl Listener" },
        };

        

        public RemoteServiceModel SampleGloconService = new RemoteServiceModel
        {
            SystemName = "CEWD6592",
            DisplayName = "Icon Global Event Controller Service",
            FullName = "GlobalEventControllerService",
            Description = "Processes Global Event Controller from IRMA to Icon.",
            ConfigFilePath = @"TestData\SampleAppConfig_NoEsb_GloCon.exe.config",
            LoggingID = 0,
            LoggingName = "",
            Environment = "TEST",
            State = "Running",
            ProcessId = 1824,
            StartMode = "Auto",
            RunningAs = @"wfm\IconTestUserDev"
        };

        public RemoteServiceModel SampleApiControllerHierarchyService = new RemoteServiceModel
        {
            SystemName = "CEWD6592",
            DisplayName = "Icon API Controller - Hierarchy",
            FullName = "IconAPIController-Hierarchy",
            Description = "Icon Message Producer to ESB for Hierarchy data.",
            ConfigFilePath = @"""E:\Icon\API Controller Phase 2\Hierarchy\Icon.ApiController.Controller.exe""  -displayname ""Icon API Controller - Hierarchy"" -servicename ""IconAPIController-Hierarchy""",
            LoggingID = 0,
            LoggingName = "",
            Environment = "TEST",
            State = "Running",
            ProcessId = 3416,
            StartMode = "Auto",
            RunningAs = @"wfm\IconTestUserDev"
        };

        public RemoteServiceModel SampleMammothItemLocaleControllerMAService = new RemoteServiceModel
        {
            SystemName = "CEWD6592",
            DisplayName = "Mammoth ItemLocale Controller (Instance: MA)",
            FullName = "Mammoth.ItemLocale.Controller$MA",
            Description = "Processes events off the Mammoth.ItemLocaleChangeQueue and sends them to the WebApi.",
            ConfigFilePath = "TestData\\SampleAppConfig_NoEsb_MammothItemLocale.exe.config",
            LoggingID = 0,
            LoggingName = "",
            Environment = "TEST",
            State = "Running",
            ProcessId = 61684,
            StartMode = "Auto",
            RunningAs = @"wfm\MammothTest"
        };
    }
}
