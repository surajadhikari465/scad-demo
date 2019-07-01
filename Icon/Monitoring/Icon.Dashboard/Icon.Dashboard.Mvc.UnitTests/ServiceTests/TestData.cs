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
            new EsbEnvironmentViewModel
            {
                Name = "DEV",
                ServerUrl = "ssl://DEV-ESB-EMS-1.wfm.pvt:7233",
                 TargetHostName = "DEV-ESB-EMS-1.wfm.pvt",
                 JmsUsernameIcon = "iconUser",
                 JmsPasswordIcon = "ouVgOD5ld5V6",
                 JndiUsernameIcon = "jndiIconUser",
                 JndiPasswordIcon = "jndiIconUser",
                 JmsUsernameMammoth = "mammothUser",
                 JmsPasswordMammoth = "tXt5NGpSqaEk",
                 JndiUsernameMammoth = "jndiMammothUser",
                 JndiPasswordMammoth = "maMmothU$er",
                 JmsUsernameEwic = "ewicIconUser",
                 JmsPasswordEwic = "vvACtpXx00WN",
                 JndiUsernameEwic = "jndiIconUser",
                 JndiPasswordEwic = "jndiIconUser"
            },
            new EsbEnvironmentViewModel
            {
                Name = "DEV-DUP",
                 ServerUrl = "ssl://cerd1636.wfm.pvt:7233",
                 TargetHostName = "cerd1636.wfm.pvt",
                 JmsUsernameIcon = "iconUser",
                 JmsPasswordIcon= "ouVgOD5ld5V6",
                 JndiUsernameIcon = "jndiIconUser",
                 JndiPasswordIcon = "jndiIconUser",
                 JmsUsernameMammoth = "mammothUser",
                 JmsPasswordMammoth = "tXt5NGpSqaEk",
                 JndiUsernameMammoth = "jndiMammothUser",
                 JndiPasswordMammoth = "maMmothU$er",
                 JmsUsernameEwic = "ewicIconUser",
                 JmsPasswordEwic = "vvACtpXx00WN",
                 JndiUsernameEwic = "jndiIconUser",
                 JndiPasswordEwic = "jndiIconUser"
            },
            new EsbEnvironmentViewModel
            {
                Name = "TEST",
                ServerUrl = "ssl://cerd1617.wfm.pvt:7233",
                TargetHostName = "cerd1617.wfm.pvt",
                JmsUsernameIcon = "iconUser",
                JmsPasswordIcon = "Pjetuc9M7Kmi",
                JndiUsernameIcon = "jndiIconUser",
                JndiPasswordIcon = "jndiIconUser",
                JmsUsernameMammoth = "mammothUser",
                JmsPasswordMammoth = "J40L5dsM7DQr",
                JndiUsernameMammoth = "jndiMammothUser",
                JndiPasswordMammoth = "maMmothU$er",
                JmsUsernameEwic = "ewicIconUser",
                JmsPasswordEwic = "U4XEFcxlhgEW",
                JndiUsernameEwic = "jndiIconUser",
                JndiPasswordEwic = "jndiIconUser"
            },
            new EsbEnvironmentViewModel
            {
                Name = "TEST-DUP",
                ServerUrl = "ssl://cerd1637.wfm.pvt:17293",
                TargetHostName = "cerd1637.wfm.pvt",
                JmsUsernameIcon = "iconUser",
                JmsPasswordIcon = "Pjetuc9M7Kmi",
                JndiUsernameIcon = "jndiIconUser",
                JndiPasswordIcon = "jndiIconUser",
                JmsUsernameMammoth = "mammothUser",
                JmsPasswordMammoth = "J40L5dsM7DQr",
                JndiUsernameMammoth = "jndiMammothUser",
                JndiPasswordMammoth = "maMmothU$er",
                JmsUsernameEwic = "ewicIconUser",
                JmsPasswordEwic = "U4XEFcxlhgEW",
                JndiUsernameEwic = "jndiIconUser",
                JndiPasswordEwic = "jndiIconUser"
            },
            new EsbEnvironmentViewModel
            {
                Name = "QA-FUNC",
                ServerUrl = "ssl://cerd1619.wfm.pvt:27293,ssl://cerd1622.wfm.pvt:27293 ",
                TargetHostName = "cerd1619.wfm.pvt, cerd1622.wfm.pvt",
                JmsUsernameIcon = "iconUser",
                JmsPasswordIcon = "3E0y369%iz",
                JndiUsernameIcon = "iconUser",
                JndiPasswordIcon = "jndiI8conUserdUp",
                JmsUsernameMammoth = "mammothUser",
                JmsPasswordMammoth = "82l3jZ27lz5R",
                JndiUsernameMammoth = "jndiMammothUser",
                JndiPasswordMammoth = "t4H41v48MT7s",
                JmsUsernameEwic = "",
                JmsPasswordEwic = "",
                JndiUsernameEwic = "",
                JndiPasswordEwic = ""
            },
            new EsbEnvironmentViewModel
            {
                Name = "QA-DUP",
                ServerUrl = "ssl://cerd1639.wfm.pvt:27293, ssl://cerd1640.wfm.pvt:27293 ",
                TargetHostName = "cerd1639.wfm.pvt,cerd1640.wfm.pvt",
                JmsUsernameIcon = "iconUser",
                JmsPasswordIcon = "3E0y369%iz",
                JndiUsernameIcon = "iconUser",
                JndiPasswordIcon = "jndiI8conUserdUp",
                JmsUsernameMammoth = "mammothUser",
                JmsPasswordMammoth = "82l3jZ27lz5R",
                JndiUsernameMammoth = "jndiMammothUser",
                JndiPasswordMammoth = "t4H41v48MT7s",
                JmsUsernameEwic = "ewicIconUser",
                JmsPasswordEwic = "",
                JndiUsernameEwic = "jndiIconUser",
                JndiPasswordEwic = ""
            },
            new EsbEnvironmentViewModel
            {
                Name = "QA-PERF",
                ServerUrl = "ssl://cerd1630.wfm.pvt:27293, ssl://cerd1631.wfm.pvt:27293 ",
                TargetHostName = "cerd1630.wfm.pvt,cerd1631.wfm.pvt",
                JmsUsernameIcon = "iconUser",
                JmsPasswordIcon = "3E0y369%iz",
                JndiUsernameIcon = "iconUser",
                JndiPasswordIcon = "jndiI8conUserdUp",
                JmsUsernameMammoth = "mammothUser",
                JmsPasswordMammoth = "82l3jZ27lz5R",
                JndiUsernameMammoth = "jndiMammothUser",
                JndiPasswordMammoth = "t4H41v48MT7s",
                JmsUsernameEwic = "ewicIconUser",
                JmsPasswordEwic = "",
                JndiUsernameEwic = "jndiIconUser",
                JndiPasswordEwic = ""
            },

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
            ConfigFilePath = @"E:\SampleAppConfig_A.exe.config",
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
