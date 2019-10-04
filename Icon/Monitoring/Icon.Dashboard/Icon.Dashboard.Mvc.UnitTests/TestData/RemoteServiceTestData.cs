using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.TestData
{
    public class RemoteServiceTestData
    {
        public RemoteServiceTestData()
        {
            AppLogs = new AppLogData();
            Services = new ServiceData();
            ApiJob = new ApiJobData();

            FakeEnvironmentViewModel = new EnvironmentViewModel
            {
                Name = "UnitTest",
                id = 1,
                AppServers = new List<AppServerViewModel>
                {
                    new AppServerViewModel { id = 11, parentId = 1, ServerName = "fakeServer1" },
                    new AppServerViewModel { id = 12, parentId = 1, ServerName = "fakeServer2" },
                }
            };
        }
        public EnvironmentViewModel FakeEnvironmentViewModel = null;

        public ServiceData Services { get; set; } 
        public AppLogData AppLogs { get; set; }
        public ApiJobData ApiJob { get; set; }

        public class ServiceData {

            public ServiceViewModel GloconViewModel = null;
            public ServiceViewModel MammothProductListenerViewModel = null;
            public ServiceViewModel MammothItemLocaleControllerViewModel = null;
            public ServiceViewModel IconR10ListenerViewModel = null;
            public ServiceViewModel IconEwicAplListenerViewModel = null;

            public DatabaseConfigurationData DbConfigModels { get; set; }
            public AppSettingsData AppSettings { get; set; }
            public EsbConnectionData EsbConnections { get; set; }

            public List<ServiceViewModel> ServiceViewModelList
            {
                get
                {
                    var list = new List<ServiceViewModel>() {
                    GloconViewModel,
                    MammothProductListenerViewModel,
                    MammothItemLocaleControllerViewModel,
                    IconR10ListenerViewModel
                    //IconEwicAplListenerViewModel
                };
                    return list;
                }
            }

            public ServiceData()
            {
                DbConfigModels = new DatabaseConfigurationData();
                AppSettings = new AppSettingsData();
                EsbConnections = new EsbConnectionData();

                GloconViewModel = new ServiceViewModel
                {
                    Name = "GlobalEventController.Controller",
                    ConfigFilePath = @"\\zzzzz\E$\aaa\bbbbbb\ssssss.exe.config",
                    DisplayName = "Global Event Contrtoller",
                    Server = "vm-icon-test1",
                    Family = "Icon",
                    Status = "Running",
                    ValidCommands = new List<string> { "Stop" },
                    StatusIsGreen = true,
                    LoggingName = "FakeLoggingName",
                    LoggingID = 7,
                    EsbEnvironmentEnum = EsbEnvironmentEnum.None,
                    ConfigFilePathIsValid = true,
                    CommandsEnabled = true,
                    AccountName = @"wfm\serviceAcct",
                    DatabaseConfiguration = DbConfigModels.FakeDbViewModel_Icon_Tst0_Glocon,
                    HostName = "CEWD6586",
                    Description = "fake service description",
                    AppSettings = new Dictionary<string, string>
                {
                    { "GlobalEvents","IconToIrmaValidatedNewItems,IconToIrmaItemValidation,IconToIrmaItemUpdates,IconToIrmaBrandNameUpdate,IconToIrmaNutritionUpdate,IconToIrmaNutritionAdd,IconToIrmaNutritionDelete,IconToIrmaBrandDelete,IconToIrmaNationalHierarchyUpdate,IconToIrmaNationalHierarchyDelete"},
                    { "MaxQueueEntriesToProcess", "1000"},
                    { "ClientSettingsProvider.ServiceUri", ""},
                    { "ControllerUserName", "iconcontrolleruser"},
                    { "DbContextConnectionTimeout", "120"},
                    { "SendEmails", "true"},
                    { "EmailHost", "smtp.wholefoods.com"},
                    { "EmailPort", "25"},
                    { "EmailUsername", ""},
                    { "EmailPassword", ""},
                    { "Sender", "Icon-Test@wholefoods.com"},
                    { "Recipients", "irma.developers@wholefoods.com"},
                    { "ControllerInstanceId", "1"},
                    { "RunInterval", "30000"},
                    { "ServiceDescription", "Processes Global Event Controller from IRMA to Icon."},
                    { "ServiceDisplayName", "Icon Global Event Controller Service"},
                    { "ServiceName", "GlobalEventControllerService"},
                    { "MaintenanceDay", "0"},
                    { "MaintenanceStartTime", "02:00"},
                    { "MaintenanceEndTime", "04:00"},
                    { "EmailSubjectEnvironment", "Test"},
                    { "SendUomChangeEmails_FL", "false"},
                    { "SendUomChangeEmails_MA", "false"},
                    { "SendUomChangeEmails_MW", "false"},
                    { "SendUomChangeEmails_NA", "false"},
                    { "SendUomChangeEmails_NC", "false"},
                    { "SendUomChangeEmails_NE", "false"},
                    { "SendUomChangeEmails_PN", "false"},
                    { "SendUomChangeEmails_RM", "false"},
                    { "SendUomChangeEmails_SO", "false"},
                    { "SendUomChangeEmails_SP", "false"},
                    { "SendUomChangeEmails_SW", "false"},
                    { "SendUomChangeEmails_UK", "false"},
                    { "FL_Recipients", "irma.developers@wholefoods.com"},
                    { "MA_Recipients", "irma.developers@wholefoods.com"},
                    { "MW_Recipients", "irma.developers@wholefoods.com"},
                    { "NA_Recipients", "irma.developers@wholefoods.com"},
                    { "NC_Recipients", "irma.developers@wholefoods.com"},
                    { "NE_Recipients", "irma.developers@wholefoods.com"},
                    { "PN_Recipients", "irma.developers@wholefoods.com"},
                    { "RM_Recipients", "irma.developers@wholefoods.com"},
                    { "SO_Recipients", "irma.developers@wholefoods.com"},
                    { "SP_Recipients", "irma.developers@wholefoods.com"},
                    { "SW_Recipients", "irma.developers@wholefoods.com"},
                    { "UK_Recipients", "irma.developers@wholefoods.com"},
                    { "BrandDeleteEmailSubject", "Brand Delete Alert"},
                    { "SendBrandDeleteEmails", "true"},
                    { "EnableInforUpdates", "true"},
                },
                    EsbConnections = new List<EsbConnectionViewModel>(),
                };

                MammothProductListenerViewModel = new ServiceViewModel
                {
                    Name = "Mammoth.Esb.ProductListener.Service",
                    ConfigFilePath = @"\\zzzzz\E$\aaa\bbbbbb\ssssss.exe.config",
                    DisplayName = "Mammoth Product Listener",
                    Server = "vm-icon-test2",
                    Family = "Mammoth",
                    Status = "Running",
                    ValidCommands = new List<string> { "Stop" },
                    StatusIsGreen = true,
                    LoggingName = "FakeLoggingName",
                    LoggingID = 7,
                    ConfigFilePathIsValid = true,
                    CommandsEnabled = true,
                    AccountName = @"wfm\serviceAcct",
                    DatabaseConfiguration = DbConfigModels.FakeDbViewModel_Icon_Tst0_EF,
                    HostName = "CEWD6587",
                    Description = "fake service description",
                    AppSettings = new Dictionary<string, string>
                {
                    { "SendEmails", "true"},
                    { "EmailHost", "smtp.wholefoods.com"},
                    { "EmailPort", "25"},
                    { "EmailUsername", ""},
                    { "EmailPassword", ""},
                    { "Sender", "Mammoth-Test@wholefoods.com"},
                    { "Recipients", "irma.developers@wholefoods.com"},
                    { "EnablePrimeAffinityMessages", "true"},
                    { "ExcludedPSNumbers", "2100,2200,2220"},
                    { "EligiblePriceTypes", "SAL,ISS,FRZ"},
                    { "NonReceivingSystems", "AMZ,Slaw,Mammoth,Spice,Icon"},
                    { "PrimeAffinityPsgName", "PrimeAffinityPSG"},
                    { "PrimeAffinityPsgType", "Consumable"},
                },
                    EsbEnvironmentEnum = EsbEnvironmentEnum.TEST,
                    HasEsbSettingsInCustomConfigSection = true,
                    EsbConnections = new List<EsbConnectionViewModel>
                {
                    new EsbConnectionViewModel
                    {
                        ConnectionName = "ESB",
                        ServerUrl = "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293",
                        JmsUsername = "mammothUser",
                        JmsPassword = "jmsM@mm#!",
                        JndiUsername = "jndiMammothUser",
                        JndiPassword = "jndiM@mm$$$",
                        ConnectionFactoryName = "ItemQueueConnectionFactory",
                        SslPassword = "esb",
                        QueueName = "WFMESB.Enterprise.Item.ItemGateway.Queue.V2",
                        SessionMode = "ClientAcknowledge" ,
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        TargetHostName = "TST-ESB-EMS-1.wfm.pvt",
                    },
                    new EsbConnectionViewModel
                    {
                        ConnectionName = "R10",
                        ServerUrl = "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293",
                        JmsUsername = "iconUser",
                        JmsPassword = "jms!C0n",
                        JndiUsername = "jndiIconUser",
                        JndiPassword = "jndiIconUser8",
                        ConnectionFactoryName = "ItemQueueConnectionFactory",
                        SslPassword = "esb",
                        QueueName = "WFMESB.Commerce.Retail.ItemMgmt.Item.Queue.V2",
                        SessionMode = "ClientAcknowledge",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        TargetHostName = "TST-ESB-EMS-1.wfm.pvt"
                    },
                }
                };

                MammothItemLocaleControllerViewModel = new ServiceViewModel
                {
                    Name = "Mammoth.ItemLocale.Controller",
                    ConfigFilePath = @"\\zzzzz\uuuuuuu\aaa\bbbbbb\ssssss.xml",
                    DisplayName = "Mammoth Item-Locale Controller",
                    Server = "mammoth-app01-qa",
                    Family = "Mammoth",
                    Status = "Running",
                    ValidCommands = new List<string> { "Stop" },
                    StatusIsGreen = true,
                    LoggingName = "FakeLoggingName",
                    LoggingID = 4,
                    ConfigFilePathIsValid = true,
                    CommandsEnabled = true,
                    AccountName = @"wfm\serviceAcct",
                    //DatabaseSummary = FakeDbViewModel_Mammoth_QA_Sql.Summary,
                    DatabaseConfiguration = DbConfigModels.FakeDbViewModel_Mammoth_QA_Sql,
                    HostName = "CEWD6586",
                    Description = "fake service description",
                    //EsbConnectionType = Enums.EsbConnectionTypeEnum.Mammoth,
                    //HasEsbConfiguration = true,
                    AppSettings = new Dictionary<string, string>
                {
                    { "NonReceivingSystemsAll", "R10,Mammoth,Icon" },
                    { "NonReceivingSystemsItemLocale", "R10,Mammoth,Icon" },
                    { "NonReceivingSystemsPrice", "R10,Mammoth,Icon,ESL1Plum" },
                    { "ControllerInstanceId", "11" },
                    { "RunInterval", "30000" },
                    { "ControllerType", "i" },
                    { "ApiDescription", "Mammoth Message Producer to ESB for ItemLocale data" },
                    { "ApiDisplayName", "Mammoth API Controller - ItemLocale" },
                    { "ApiServiceName", "MammothAPIController-ItemLocale" },
                    { "MaintenanceDay", "0" },
                    { "MaintenanceStartTime", "02:00" },
                    { "MaintenanceEndTime", "02:00" },
                    { "SendEmails", "false" },
                    { "EmailHost", "smtp.wholefoods.com" },
                    { "EmailPort", "25" },
                    { "EmailUsername", "" },
                    { "EmailPassword", "" },
                    { "Sender", "Mammoth-Test@wholefoods.com" },
                    { "Recipients", "irma.developers@wholefoods.com" },
                },
                    EsbEnvironmentEnum = EsbEnvironmentEnum.QA_FUNC,
                    HasEsbSettingsInCustomConfigSection = false,
                    EsbConnections = new List<EsbConnectionViewModel>
                {
                    new EsbConnectionViewModel
                    {
                        ConnectionName = "ESB",
                        ServerUrl = "ssl://QA-ESB-EMS-1.wfm.pvt:27293,ssl://QA-ESB-EMS-2.wfm.pvt:27293" ,
                        JmsUsername = "mammothUser" ,
                        JmsPassword = "jmsM@mm#!" ,
                        JndiUsername = "jndiMammothUser" ,
                        JndiPassword = "jndiM@mm$$$" ,
                        ConnectionFactoryName = "ItemQueueConnectionFactory" ,
                        LocaleQueueName = "WFMESB.Commerce.Retail.LocaleMgmt.Locale.Queue.V2" ,
                        HierarchyQueueName = "WFMESB.Commerce.Retail.HierarchyMgmt.Hierarchy.Queue.V2" ,
                        ItemQueueName = "WFMESB.Commerce.Retail.ItemMgmt.Item.Queue.V2" ,
                        ProductSelectionGroupQueueName = "WFMESB.Commerce.Retail.ItemMgmt.SelectionGroup.Queue.V2" ,
                        SessionMode = "AutoAcknowledge" ,
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=c\"Austin \", S=TX, C=US" ,
                        CertificateStoreName = "Root" ,
                        CertificateStoreLocation = "LocalMachine" ,
                        TargetHostName = "QA-ESB-EMS-1.wfm.pvt" ,
                        SslPassword = "esb" ,
                    },
                },
                };

                IconR10ListenerViewModel = new ServiceViewModel
                {
                    Name = "Icon.Esb.R10Listener.WindowsService",
                    ConfigFilePath = @"\\zzzzz\E$\aaa\bbbbbb\ssssss.xml",
                    DisplayName = "R10 Listener",
                    Server = "vm-icon-test1",
                    Family = "Icon",
                    Status = "Running",
                    ValidCommands = new List<string> { "Stop" },
                    StatusIsGreen = true,
                    LoggingName = "FakeLoggingName",
                    LoggingID = 4,
                    EsbEnvironmentEnum = EsbEnvironmentEnum.TEST,
                    ConfigFilePathIsValid = true,
                    CommandsEnabled = true,
                    AccountName = @"wfm\serviceAcct",
                    //DatabaseSummary = FakeDbViewModel_Icon_Tst0_EF.Summary,
                    DatabaseConfiguration = DbConfigModels.FakeDbViewModel_Icon_Tst0_EF,
                    HostName = "cewd6592",
                    Description = "fake service description",
                    //EsbConnectionType = Enums.EsbConnectionTypeEnum.Icon,
                    //HasEsbConfiguration = true,
                    AppSettings = new Dictionary<string, string>
                {
                    { "ReconnectDelay", "30000" },
                    { "SendEmails", "false" },
                    { "EmailHost", "smtp.wholefoods.com" },
                    { "EmailPort", "25" },
                    { "EmailUsername", "" },
                    { "EmailPassword", "" },
                    { "Sender", "Icon-Test@wholefoods.com" },
                    { "Recipients", "irma.developers@wholefoods.com" },
                    { "NumberOfListenerThreads", "1" },
                    { "ResendMessageCount", "1" },
                },
                    EsbConnections = new List<EsbConnectionViewModel>
                {
                    new EsbConnectionViewModel
                    {
                        ConnectionName = "ESB",
                        ServerUrl = "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293" ,
                        JmsUsername = "iconUser" ,
                        JmsPassword = "jms!C0n" ,
                        JndiUsername = "jndiIconUser" ,
                        JndiPassword = "jndiIconUser8" ,
                        ConnectionFactoryName = "ItemQueueConnectionFactory" ,
                        SslPassword = "esb" ,
                        QueueName = "WFMESB.Technical.ItemMgmt.PointOfSaleResponse.Reply.Queue.V2" ,
                        SessionMode = "ClientAcknowledge" ,
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US" ,
                        CertificateStoreName = "Root" ,
                        CertificateStoreLocation = "LocalMachine" ,
                        TargetHostName = "TST-ESB-EMS-1.wfm.pvt" ,
                    }
                },
                };

                IconEwicAplListenerViewModel = new ServiceViewModel
                {
                    Name = "Icon.Esb.EwicAplListener.WindowsService",
                    ConfigFilePath = @"\\zzzzz\E$\aaa\bbbbbb\ssssss.xml",
                    DisplayName = "eWIC APL Listener",
                    Server = "vm-icon-test1",
                    Family = "Icon",
                    Status = "Running",
                    ValidCommands = new List<string> { "Stop" },
                    StatusIsGreen = true,
                    LoggingName = "FakeLoggingName",
                    LoggingID = 4,
                    EsbEnvironmentEnum = EsbEnvironmentEnum.TEST,
                    ConfigFilePathIsValid = true,
                    CommandsEnabled = true,
                    AccountName = @"wfm\serviceAcct",
                    //DatabaseSummary = FakeDbViewModel_Icon_Tst0_EF.Summary,
                    DatabaseConfiguration = DbConfigModels.FakeDbViewModel_Icon_Tst0_EF,
                    HostName = "cew6592",
                    Description = "fake service description",
                    //EsbConnectionType = Enums.EsbConnectionTypeEnum.Icon,
                    //HasEsbConfiguration = true,
                    AppSettings = new Dictionary<string, string>
                {
                    { "SendEmails", "true" },
                    { "EmailHost", "smtp.wholefoods.com" },
                    { "EmailPort", "25" },
                    { "EmailUsername", "" },
                    { "EmailPassword", "" },
                    { "Sender", "Icon-Dev@wholefoods.com" },
                    { "Recipients", "irma.developers@wholefoods.com" },
                    { "NumberOfListenerThreads", "1" },
                },
                    EsbConnections = new List<EsbConnectionViewModel>
                {
                    new EsbConnectionViewModel
                    {
                        ConnectionName = "listener",
                        ServerUrl = "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-1.wfm.pvt:17293",
                        JmsUsername = "ewicIconUser",
                        JmsPassword = "ewic\"#*8",
                        JndiUsername = "jndiEwicIconUser",
                        JndiPassword = "jndiEw!cUser",
                        ConnectionFactoryName = "EwicQueueConnectionFactory",
                        SslPassword = "esb",
                        QueueName = "WFMESB.Commerce.Retail.EWICMgmt.EWIC.Queue.V1",
                        SessionMode = "ClientAcknowledge",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \" S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        TargetHostName = "TST-ESB-EMS-1.wfm.pvt",
                    },
                    new EsbConnectionViewModel
                    {
                        ConnectionName = "producer",
                        ServerUrl = "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-1.wfm.pvt:17293",
                        JmsUsername = "ewicIconUser",
                        JmsPassword = "ewic\"#*8",
                        JndiUsername = "jndiEwicIconUser",
                        JndiPassword = "jndiEw!cUser ",
                        ConnectionFactoryName = "EwicQueueConnectionFactory",
                        SslPassword = "esb",
                        QueueName = "WFMESB.Commerce.PointOfSaleMgmt.NonSequencedRequest.Queue.V2",
                        SessionMode = "ClientAcknowledge",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \" S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        TargetHostName = "TST-ESB-EMS-1.wfm.pvt",
                    }
                },
                };
            }
        }

        public class DatabaseConfigurationData
        {
            public DatabaseConfigurationData()
            {
                FakeDbViewModel_Icon_Tst0_EF = new AppDatabaseConfigurationViewModel()
                {
                    Summary = "Icon Tst0",
                    LoggingSummary = "Icon-Tst0",
                    Databases = new List<AppDatabaseViewModel>
                {
                    new AppDatabaseViewModel
                    {
                        ServerName = @"cewd1815\SQLSHARED2012D",
                        DatabaseName = "Icon",
                        ConnectionStringName = "IconContext",
                        Environment = EnvironmentEnum.Tst0,
                        Category = DatabaseCategoryEnum.Icon,
                        IsEntityFramework = true,
                        IsUsedForLogging = false,
                    },
                    new AppDatabaseViewModel
                    {
                        ServerName = @"cewd1815\SQLSHARED2012D",
                        DatabaseName = "Icon",
                        ConnectionStringName = "dbLogIconEsb",
                        Environment = EnvironmentEnum.Tst0,
                        Category = DatabaseCategoryEnum.Icon,
                        IsEntityFramework = false,
                        IsUsedForLogging = true,
                    },
                }
                };

                FakeDbViewModel_Mammoth_Tst0_Sql = new AppDatabaseConfigurationViewModel()
                {
                    Summary = "Mammoth Tst0",
                    LoggingSummary = "Mammoth-QA",
                    Databases = new List<AppDatabaseViewModel>
                {
                    new AppDatabaseViewModel
                    {
                        ServerName = @"MAMMOTH-DB01-DEV\MAMMOTH",
                        DatabaseName = "Mammoth",
                        ConnectionStringName = "IconContext",
                        Environment = EnvironmentEnum.Tst0,
                        Category = DatabaseCategoryEnum.Mammoth,
                        IsEntityFramework = false,
                        IsUsedForLogging = false,
                    },
                    new AppDatabaseViewModel
                    {
                        ServerName = @"MAMMOTH-DB01-DEV\MAMMOTH",
                        DatabaseName = "Mammoth",
                        ConnectionStringName = "dbLogMammoth",
                        Environment = EnvironmentEnum.Tst0,
                        Category = DatabaseCategoryEnum.Mammoth,
                        IsEntityFramework = false,
                        IsUsedForLogging = true,
                    },
                }
                };

                FakeDbViewModel_Mammoth_QA_Sql = new AppDatabaseConfigurationViewModel()
                {
                    Summary = "Mammoth QA",
                    LoggingSummary = "Mammoth-QA",
                    Databases = new List<AppDatabaseViewModel>
                {
                    new AppDatabaseViewModel
                    {
                        ServerName = @"MAMMOTH-DB01-QA\MAMMOTH",
                        DatabaseName = "Mammoth",
                        ConnectionStringName = "IconContext",
                        Environment = EnvironmentEnum.QA,
                        Category = DatabaseCategoryEnum.Mammoth,
                        IsEntityFramework = false,
                        IsUsedForLogging = false,
                    },
                    new AppDatabaseViewModel
                    {
                        ServerName = @"MAMMOTH-DB01-QA\MAMMOTH",
                        DatabaseName = "Mammoth",
                        ConnectionStringName = "dbLogMammoth",
                        Environment = EnvironmentEnum.QA,
                        Category = DatabaseCategoryEnum.Mammoth,
                        IsEntityFramework = false,
                        IsUsedForLogging = true,
                    },
                }
                };

                FakeDbViewModel_Icon_Tst0_Glocon = new AppDatabaseConfigurationViewModel()
                {
                    Summary = "Icon Tst0 & IRMA Dev0/Tst0",
                    LoggingSummary = "Icon-Tst0",
                    Databases = new List<AppDatabaseViewModel>
                {
                    new AppDatabaseViewModel
                    {
                        ServerName = @"cewd1815\SQLSHARED2012D",
                        DatabaseName = "Icon",
                        ConnectionStringName = "IconContext",
                        Environment = EnvironmentEnum.Tst0,
                        Category = DatabaseCategoryEnum.Icon,
                        IsEntityFramework = true,
                        IsUsedForLogging = false,
                    },
                    new AppDatabaseViewModel
                    {
                        ServerName = @"cewd1815\SQLSHARED2012D",
                        DatabaseName = "Icon",
                        ConnectionStringName = "dbLogIconGlobalController",
                        Environment = EnvironmentEnum.Tst0,
                        Category = DatabaseCategoryEnum.Icon,
                        IsEntityFramework = false,
                        IsUsedForLogging = true,
                    },
                    new AppDatabaseViewModel
                    {
                        ServerName = @"idd-fl\fld",
                        DatabaseName = "ItemCatalog_Test",
                        ConnectionStringName = "ItemCatalog_FL",
                        Environment = EnvironmentEnum.Dev0,
                        Category = DatabaseCategoryEnum.IRMA,
                        IsEntityFramework = true,
                        IsUsedForLogging = false,
                    },
                    new AppDatabaseViewModel
                    {
                        ServerName = @"idd-ma\mad",
                        DatabaseName = "ItemCatalog_Test",
                        ConnectionStringName = "ItemCatalog_MA",
                        Environment = EnvironmentEnum.Dev0,
                        Category = DatabaseCategoryEnum.IRMA,
                        IsEntityFramework = true,
                        IsUsedForLogging = false,
                    },
                    new AppDatabaseViewModel
                    {
                        ServerName = @"idd-mw\mwd",
                        DatabaseName = "ItemCatalog_Test",
                        ConnectionStringName = "ItemCatalog_MW",
                        Environment = EnvironmentEnum.Dev0,
                        Category = DatabaseCategoryEnum.IRMA,
                        IsEntityFramework = true,
                        IsUsedForLogging = false,
                    },
                    new AppDatabaseViewModel
                    {
                        ServerName = @"idd-na\nad",
                        DatabaseName = "ItemCatalog_Test",
                        ConnectionStringName = "ItemCatalog_NA",
                        Environment = EnvironmentEnum.Dev0,
                        Category = DatabaseCategoryEnum.IRMA,
                        IsEntityFramework = true,
                        IsUsedForLogging = false,
                    },
                    new AppDatabaseViewModel
                    {
                        ServerName = @"idd-rm\rmd",
                        DatabaseName = "ItemCatalog_Test",
                        ConnectionStringName = "ItemCatalog_RM",
                        Environment = EnvironmentEnum.Dev0,
                        Category = DatabaseCategoryEnum.IRMA,
                        IsEntityFramework = true,
                        IsUsedForLogging = false,
                    },
                    new AppDatabaseViewModel
                    {
                        ServerName = @"idd-so\sod",
                        DatabaseName = "ItemCatalog_Test",
                        ConnectionStringName = "ItemCatalog_SO",
                        Environment = EnvironmentEnum.Dev0,
                        Category = DatabaseCategoryEnum.IRMA,
                        IsEntityFramework = true,
                        IsUsedForLogging = false,
                    },
                    new AppDatabaseViewModel
                    {
                        ServerName = @"idt-nc\nct",
                        DatabaseName = "ItemCatalog_Test",
                        ConnectionStringName = "ItemCatalog_NC",
                        Environment = EnvironmentEnum.Tst0,
                        Category = DatabaseCategoryEnum.IRMA,
                        IsEntityFramework = true,
                        IsUsedForLogging = false,
                    },
                    new AppDatabaseViewModel
                    {
                        ServerName = @"idt-ne\net",
                        DatabaseName = "ItemCatalog_Test",
                        ConnectionStringName = "ItemCatalog_NE",
                        Environment = EnvironmentEnum.Tst0,
                        Category = DatabaseCategoryEnum.IRMA,
                        IsEntityFramework = true,
                        IsUsedForLogging = false,
                    },
                    new AppDatabaseViewModel
                    {
                        ServerName = @"idt-pn\pnt",
                        DatabaseName = "ItemCatalog_Test",
                        ConnectionStringName = "ItemCatalog_PN",
                        Environment = EnvironmentEnum.Tst0,
                        Category = DatabaseCategoryEnum.IRMA,
                        IsEntityFramework = true,
                        IsUsedForLogging = false,
                    },
                    new AppDatabaseViewModel
                    {
                        ServerName = @"idt-sp\spt",
                        DatabaseName = "ItemCatalog_Test",
                        ConnectionStringName = "ItemCatalog_SP",
                        Environment = EnvironmentEnum.Tst0,
                        Category = DatabaseCategoryEnum.IRMA,
                        IsEntityFramework = true,
                        IsUsedForLogging = false,
                    },
                    new AppDatabaseViewModel
                    {
                        ServerName = @"idt-sw\swt",
                        DatabaseName = "ItemCatalog_Test",
                        ConnectionStringName = "ItemCatalog_SW",
                        Environment = EnvironmentEnum.Tst0,
                        Category = DatabaseCategoryEnum.IRMA,
                        IsEntityFramework = true,
                        IsUsedForLogging = false,
                    },
                    new AppDatabaseViewModel
                    {
                        ServerName = @"idd-uk\ukt",
                        DatabaseName = "ItemCatalog_Test",
                        ConnectionStringName = "ItemCatalog_UK",
                        Environment = EnvironmentEnum.Tst0,
                        Category = DatabaseCategoryEnum.IRMA,
                        IsEntityFramework = true,
                        IsUsedForLogging = false,
                    },
                }
                };
            }
            public AppDatabaseConfigurationViewModel FakeDbViewModel_Icon_Tst0_EF = null;
            public AppDatabaseConfigurationViewModel FakeDbViewModel_Icon_Tst0_Glocon = null;
            public AppDatabaseConfigurationViewModel FakeDbViewModel_Mammoth_Tst0_Sql = null;
            public AppDatabaseConfigurationViewModel FakeDbViewModel_Mammoth_QA_Sql = null;        
        }

        public class AppSettingsData
        {
            public Dictionary<string, string> FakeAppSettings_NoEsb_1 = new Dictionary<string, string>
            {
                { "NonReceivingSystemsAll", "R10,Mammoth,Icon" },
                { "NonReceivingSystemsItemLocale", "R10,Mammoth,Icon" },
                { "NonReceivingSystemsPrice", "R10,Mammoth,Icon,ESL1Plum" },
                { "ControllerInstanceId", "11" },
                { "RunInterval", "30000" },
                { "ControllerType", "i" },
                { "ApiDescription", "Mammoth Message Producer to ESB for ItemLocale data" },
                { "ApiDisplayName", "Mammoth API Controller - ItemLocale" },
                { "ApiServiceName", "MammothAPIController-ItemLocale" },
                { "MaintenanceDay", "0" },
                { "MaintenanceStartTime", "02:00" },
                { "MaintenanceEndTime", "02:00" },
                { "SendEmails", "false" },
                { "EmailHost", "smtp.wholefoods.com" },
                { "EmailPort", "25" },
                { "EmailUsername", "" },
                { "EmailPassword", "" },
                { "Sender", "Mammoth-Test@wholefoods.com" },
                { "Recipients", "irma.developers@wholefoods.com" },
            };

            public Dictionary<string, string> FakeAppSettings_NoEsb_2 = new Dictionary<string, string>
            {
                { "SendEmails", "false" },
                { "EmailHost", "smtp.wholefoods.com" },
                { "EmailPort", "25" },
                { "EmailUsername", "" },
                { "EmailPassword", "" },
                { "Sender", "Icon-Test@wholefoods.com" },
                { "Recipients", "irma.developers@wholefoods.com" },
                { "NumberOfListenerThreads", "1" },
                { "ResendMessageCount", "1" },
            };

            public Dictionary<string, string> FakeAppSettings_UndisinguishedWithEsb = new Dictionary<string, string>
            {
                { "ReconnectDelay", "30000" },
                { "SendEmails", "false" },
                { "EmailHost", "smtp.wholefoods.com" },
                { "EmailPort", "25" },
                { "EmailUsername", "" },
                { "EmailPassword", "" },
                { "Sender", "Icon-Test@wholefoods.com" },
                { "Recipients", "irma.developers@wholefoods.com" },
                { "NumberOfListenerThreads", "1" },
                { "ResendMessageCount", "1" },
                { "ServerUrl", "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293" },
                { "JmsUsername", "iconUser" },
                { "JmsPassword", "jms!C0n" },
                { "JndiUsername", "jndiIconUser" },
                { "JndiPassword", "jndiIconUser8" },
                { "ConnectionFactoryName", "ItemQueueConnectionFactory" },
                { "SslPassword", "esb" },
                { "QueueName", "WFMESB.Technical.ItemMgmt.PointOfSaleResponse.Reply.Queue.V2" },
                { "SessionMode", "ClientAcknowledge" },
                { "CertificateName", "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US" },
                { "CertificateStoreName", "Root" },
                { "CertificateStoreLocation", "LocalMachine" },
                { "TargetHostName", "TST-ESB-EMS-1.wfm.pvt" },
            };

            public Dictionary<string, string> FakeEsbSettings_TEST_Icon = new Dictionary<string, string>
            {
                { "ServerUrl", "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293" },
                { "JmsUsername", "iconUser" },
                { "JmsPassword", "jms!C0n" },
                { "JndiUsername", "jndiIconUser" },
                { "JndiPassword", "jndiIconUser8" },
                { "ConnectionFactoryName", "ItemQueueConnectionFactory" },
                { "SslPassword", "esb" },
                { "QueueName", "WFMESB.Technical.ItemMgmt.PointOfSaleResponse.Reply.Queue.V2" },
                { "SessionMode", "ClientAcknowledge" },
                { "CertificateName", "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US" },
                { "CertificateStoreName", "Root" },
                { "CertificateStoreLocation", "LocalMachine" },
                { "TargetHostName", "TST-ESB-EMS-1.wfm.pvt" },
            };

            public Dictionary<string, string> FakeEsbSettings_TEST_Mammoth = new Dictionary<string, string>
            {
                { "ServerUrl", "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293" },
                { "JmsUsername", "mammothUser" },
                { "JmsPassword", "jmsM@mm#!" },
                { "JndiUsername", "jndiMammothUser" },
                { "JndiPassword", "jndiM@mm$$$" },
                { "ConnectionFactoryName", "ItemQueueConnectionFactory" },
                { "LocaleQueueName", "WFMESB.Commerce.Retail.LocaleMgmt.Locale.Queue.V2" },
                { "HierarchyQueueName", "WFMESB.Commerce.Retail.HierarchyMgmt.Hierarchy.Queue.V2" },
                { "ItemQueueName", "WFMESB.Commerce.Retail.ItemMgmt.Item.Queue.V2" },
                { "ProductSelectionGroupQueueName", "WFMESB.Commerce.Retail.ItemMgmt.SelectionGroup.Queue.V2" },
                { "SessionMode", "AutoAcknowledge" },
                { "CertificateName", "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US" },
                { "CertificateStoreName", "Root" },
                { "CertificateStoreLocation", "LocalMachine" },
                { "TargetHostName", "TST-ESB-EMS-1.wfm.pvt" },
                { "SslPassword", "esb" },
            };

            public Dictionary<string, string> FakeEsbSettings_TEST_Mammoth_WithNonEsb = new Dictionary<string, string>
            {
                { "ServerUrl", "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293" },
                { "JmsUsername", "mammothUser" },
                { "JmsPassword", "jmsM@mm#!" },
                { "JndiUsername", "jndiMammothUser" },
                { "JndiPassword", "jndiM@mm$$$" },
                { "ConnectionFactoryName", "ItemQueueConnectionFactory" },
                { "LocaleQueueName", "WFMESB.Commerce.Retail.LocaleMgmt.Locale.Queue.V2" },
                { "HierarchyQueueName", "WFMESB.Commerce.Retail.HierarchyMgmt.Hierarchy.Queue.V2" },
                { "ItemQueueName", "WFMESB.Commerce.Retail.ItemMgmt.Item.Queue.V2" },
                { "ProductSelectionGroupQueueName", "WFMESB.Commerce.Retail.ItemMgmt.SelectionGroup.Queue.V2" },
                { "SessionMode", "AutoAcknowledge" },
                { "CertificateName", "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US" },
                { "CertificateStoreName", "Root" },
                { "CertificateStoreLocation", "LocalMachine" },
                { "TargetHostName", "TST-ESB-EMS-1.wfm.pvt" },
                { "SslPassword", "esb" },
                { "SendEmails", "true"},
                { "EmailHost", "smtp.wholefoods.com"},
                { "EmailPort", "25"},
                { "EmailUsername", ""},
                { "EmailPassword", ""},
                { "Sender", "Mammoth-Test@wholefoods.com"},
                { "Recipients", "irma.developers@wholefoods.com"},
                { "EnablePrimeAffinityMessages", "true"},
                { "ExcludedPSNumbers", "2100,2200,2220"},
                { "EligiblePriceTypes", "SAL,ISS,FRZ"},
                { "NonReceivingSystems", "AMZ,Slaw,Mammoth,Spice,Icon"},
                { "PrimeAffinityPsgName", "PrimeAffinityPSG"},
                { "PrimeAffinityPsgType", "Consumable"},
            };

            public Dictionary<string, string> FakeEsbSettings_TEST_Ewic_Listener = new Dictionary<string, string>
            {
                { "name", "listener"},
                { "serverUrl", "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293"},
                { "jmsUsername", "ewicIconUser"},
                { "jmsPassword", "ewic\"#*8"},
                { "jndiUsername", "jndiEwicIconUser"},
                { "jndiPassword", "jndiEw!cUser"},
                { "connectionFactoryName", "EwicQueueConnectionFactory"},
                { "sslPassword", "esb"},
                { "queueName", "WFMESB.Commerce.Retail.EWICMgmt.EWIC.Queue.V1"},
                { "sessionMode", "ClientAcknowledge"},
                { "certificateName", "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US" },
                { "certificateStoreName", "Root"},
                { "certificateStoreLocation", "LocalMachine"},
                { "targetHostName", "TST-ESB-EMS-1.wfm.pvt"},
                { "reconnectDelay", "30000"},
            };

            public Dictionary<string, string> FakeEsbSettings_TEST_Ewic_Producer = new Dictionary<string, string>
            {
                { "name", "producer"},
                { "serverUrl", "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293"},
                { "jmsUsername", "ewicIconUser"},
                { "jmsPassword", "ewic\"#*8"},
                { "jndiUsername", "jndiEwicIconUser"},
                { "jndiPassword", "jndiEw!cUser"},
                { "connectionFactoryName", "EwicQueueConnectionFactory"},
                { "sslPassword", "esb"},
                { "queueName", "WFMESB.Commerce.PointOfSaleMgmt.NonSequencedRequest.Queue.V2"},
                { "sessionMode", "ClientAcknowledge"},
                { "certificateName", "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US" },
                { "certificateStoreName", "Root"},
                { "certificateStoreLocation", "LocalMachine"},
                { "targetHostName", "TST-ESB-EMS-1.wfm.pvt"},
                { "reconnectDelay", "30000"},
            };

            public Dictionary<string, string> FakeAppSettings_IconGloCon_NoEsb = new Dictionary<string, string>
            {
                { "GlobalEvents", "IconToIrmaValidatedNewItems,IconToIrmaItemValidation,IconToIrmaItemUpdates,IconToIrmaBrandNameUpdate,IconToIrmaNutritionUpdate,IconToIrmaNutritionAdd,IconToIrmaNutritionDelete,IconToIrmaBrandDelete,IconToIrmaNationalHierarchyUpdate,IconToIrmaNationalHierarchyDelete" },
                { "MaxQueueEntriesToProcess", "1000" },
                { "ClientSettingsProvider.ServiceUri", "" },
                { "ControllerUserName", "iconcontrolleruser" },
                { "DbContextConnectionTimeout", "120" },
                { "SendEmails", "true" },
                { "EmailHost", "smtp.wholefoods.com" },
                { "EmailPort", "25" },
                { "EmailUsername", "" },
                { "EmailPassword", "" },
                { "Sender", "Icon-Test@wholefoods.com" },
                { "Recipients", "irma.developers@wholefoods.com" },
                { "ControllerInstanceId", "1" },
                { "RunInterval", "30000" },
                { "ServiceDescription", "Processes Global Event Controller from IRMA to Icon." },
                { "ServiceDisplayName", "Icon Global Event Controller Service" },
                { "ServiceName", "GlobalEventControllerService" },
                { "MaintenanceDay", "0" },
                { "MaintenanceStartTime", "02:00" },
                { "MaintenanceEndTime", "04:00" },
                { "EmailSubjectEnvironment", "Test" },
                { "SendUomChangeEmails_FL", "false" },
                { "SendUomChangeEmails_MA", "false" },
                { "SendUomChangeEmails_MW", "false" },
                { "SendUomChangeEmails_NA", "false" },
                { "SendUomChangeEmails_NC", "false" },
                { "SendUomChangeEmails_NE", "false" },
                { "SendUomChangeEmails_PN", "false" },
                { "SendUomChangeEmails_RM", "false" },
                { "SendUomChangeEmails_SO", "false" },
                { "SendUomChangeEmails_SP", "false" },
                { "SendUomChangeEmails_SW", "false" },
                { "SendUomChangeEmails_UK", "false" },
                { "FL_Recipients", "irma.developers@wholefoods.com" },
                { "MA_Recipients", "irma.developers@wholefoods.com" },
                { "MW_Recipients", "irma.developers@wholefoods.com" },
                { "NA_Recipients", "irma.developers@wholefoods.com" },
                { "NC_Recipients", "irma.developers@wholefoods.com" },
                { "NE_Recipients", "irma.developers@wholefoods.com" },
                { "PN_Recipients", "irma.developers@wholefoods.com" },
                { "RM_Recipients", "irma.developers@wholefoods.com" },
                { "SO_Recipients", "irma.developers@wholefoods.com" },
                { "SP_Recipients", "irma.developers@wholefoods.com" },
                { "SW_Recipients", "irma.developers@wholefoods.com" },
                { "UK_Recipients", "irma.developers@wholefoods.com" },
                { "BrandDeleteEmailSubject", "Brand Delete Alert" },
                { "SendBrandDeleteEmails", "true" },
                { "EnableInforUpdates", "true" },
            };

            public Dictionary<string, string> FakeAppSettings_MammothItemLocaleController_NoEsb = new Dictionary<string, string>
            {
                { "InstanceID", "21" },
                { "RunIntervalInMilliseconds", "30000" },
                { "RegionsToProcess", "MA" },
                { "MaxNumberOfRowsToMark", "300" },
                { "BaseAddress", "http://mammoth-test/" },
                { "ApiRowLimit", "300" },
                { "SendEmails", "false" },
                { "EmailHost", "smtp.wholefoods.com" },
                { "EmailPort", "25" },
                { "EmailUsername", "" },
                { "EmailPassword", "" },
                { "Sender", "Mammoth-Test@wholefoods.com" },
                { "Recipients", "irma.developers@wholefoods.com" },
                { "NonAlertErrors", "InvalidData" },
            };

            public Dictionary<string, string> FakeAppSettings_IconEwicAplListener_ESB_Tst = new Dictionary<string, string>
            {
                { "SendEmails", "true"},
                { "EmailHost", "smtp.wholefoods.com"},
                { "EmailPort", "25"},
                { "EmailUsername", ""},
                { "EmailPassword", ""},
                { "Sender", "Icon-Test@wholefoods.com"},
                { "Recipients", "irma.developers@wholefoods.com"},
                { "NumberOfListenerThreads", "1"},
            };

            public Dictionary<string, string> FakeAppSettings_MammothProductListener_ESB_Perf = new Dictionary<string, string>
            {
                { "SendEmails", "true"},
                { "EmailHost", "smtp.wholefoods.com"},
                { "EmailPort", "25"},
                { "EmailUsername", ""},
                { "EmailPassword", ""},
                { "Sender", "Mammoth-Perf@wholefoods.com"},
                { "Recipients", "irma.developers@wholefoods.com"},
                { "EnablePrimeAffinityMessages", "true"},
                { "ExcludedPSNumbers", "2100,2200,2220"},
                { "EligiblePriceTypes", "SAL,ISS,FRZ"},
                { "NonReceivingSystems", "AMZ,Slaw,Mammoth,Spice,Infor,Icon"},
                { "PrimeAffinityPsgName", "PrimeAffinityPSG"},
                { "PrimeAffinityPsgType", "Consumable"},
            };

            public Dictionary<string, string> FakeAppSettings_IconInforListener_ESB_Dup = new Dictionary<string, string>
            {
                { "ServiceDisplayName", "Infor HierarchyClass Listener"},
                { "ServiceName", "InforHierarchyClassListener"},
                { "ServiceDescription", "Processes Infor HierarchyClass updates to Icon."},
                { "SendEmails", "true"},
                { "EmailHost", "smtp.wholefoods.com"},
                { "EmailPort", "25"},
                { "EmailUsername", ""},
                { "EmailPassword", ""},
                { "Sender", "Icon-Test@wholefoods.com"},
                { "Recipients", "irma.developers@wholefoods.com"},
                { "Regions", "MW,NA,NC,NE,PN,RM,SO,SP,SW"},
                { "MaxNumberOfRetries", "3"},
                { "RetryDelayInMilliseconds", "30000"},
                { "HierarchyClassServices", "AddOrUpdateHierarchyClasses,DeleteHierarchyClasses,SendTaxToMammoth"},
                { "EnableNationalClassMessageGeneration", "true"},
                { "ValidateSequenceId", "false"},
                { "EnableConfirmBods", "false"},
            };

            public Dictionary<string, string> FakeAppSettings_MammothApiController_ESB_Tst = new Dictionary<string, string>
            {
                { "NonReceivingSystemsAll", "R10,Mammoth,Icon" },
                { "NonReceivingSystemsItemLocale", "R10,Mammoth,Icon" },
                { "NonReceivingSystemsPrice", "R10,Mammoth,Icon,ESL1Plum" },
                { "ServerUrl", "ssl:TST-ESB-EMS-1.wfm.pvt:17293,ssl:TST-ESB-EMS-2.wfm.pvt:17293" },
                { "JmsUsername", "mammothUser" },
                { "JmsPassword", "jmsM@mm#!" },
                { "JndiUsername", "jndiMammothUser" },
                { "JndiPassword", "jndiM@mm$$$" },
                { "ConnectionFactoryName", "ItemQueueConnectionFactory" },
                { "LocaleQueueName", "WFMESB.Commerce.Retail.LocaleMgmt.Locale.Queue.V2" },
                { "HierarchyQueueName", "WFMESB.Commerce.Retail.HierarchyMgmt.Hierarchy.Queue.V2" },
                { "ItemQueueName", "WFMESB.Commerce.Retail.ItemMgmt.Item.Queue.V2" },
                { "ProductSelectionGroupQueueName", "WFMESB.Commerce.Retail.ItemMgmt.SelectionGroup.Queue.V2" },
                { "SessionMode", "AutoAcknowledge" },
                { "CertificateName", "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=&quot;Austin &quot;, S=TX, C=US" },
                { "CertificateStoreName", "Root" },
                { "CertificateStoreLocation", "LocalMachine" },
                { "TargetHostName", "TST-ESB-EMS-1.wfm.pvt" },
                { "SslPassword", "esb" },
                { "ControllerInstanceId", "11" },
                { "RunInterval", "30000" },
                { "ControllerType", "i" },
                { "ApiDescription", "Mammoth Message Producer to ESB for ItemLocale data" },
                { "ApiDisplayName", "Mammoth API Controller - ItemLocale" },
                { "ApiServiceName", "MammothAPIController-ItemLocale" },
                { "MaintenanceDay", "0" },
                { "MaintenanceStartTime", "02:00" },
                { "MaintenanceEndTime", "02:00" },
                { "SendEmails", "false" },
                { "EmailHost", "smtp.wholefoods.com" },
                { "EmailPort", "25" },
                { "EmailUsername", "" },
                { "EmailPassword", "" },
                { "Sender", "Mammoth-Test@wholefoods.com" },
                { "Recipients", "irma.developers@wholefoods.com" },
            };

            public Dictionary<string, string> FakeAppSettings_MammothApiController_ESB_Tst_NonEsbSettingsOnly = new Dictionary<string, string>
            {
                { "NonReceivingSystemsAll", "R10,Mammoth,Icon" },
                { "NonReceivingSystemsItemLocale" ,"R10,Mammoth,Icon" },
                { "NonReceivingSystemsPrice", "R10,Mammoth,Icon,ESL1Plum" },
                { "ControllerInstanceId", "11" },
                { "RunInterval", "30000" },
                { "ControllerType", "i" },
                { "ApiDescription", "Mammoth Message Producer to ESB for ItemLocale data" },
                { "ApiDisplayName", "Mammoth API Controller - ItemLocale" },
                { "ApiServiceName", "MammothAPIController-ItemLocale" },
                { "MaintenanceDay", "0" },
                { "MaintenanceStartTime", "02:00" },
                { "MaintenanceEndTime", "02:00" },
                { "SendEmails", "false" },
                { "EmailHost", "smtp.wholefoods.com" },
                { "EmailPort", "25" },
                { "EmailUsername", "" },
                { "EmailPassword", "" },
                { "Sender", "Mammoth-Test@wholefoods.com" },
                { "Recipients", "irma.developers@wholefoods.com" },
            };

            public Dictionary<string, string> FakeAppSettings_MammothApiController_ESB_Tst_EsbSettingsOnly = new Dictionary<string, string>
            {
                { "NonReceivingSystemsAll", "R10,Mammoth,Icon" },
                { "NonReceivingSystemsItemLocale", "R10,Mammoth,Icon" },
                { "NonReceivingSystemsPrice", "R10,Mammoth,Icon,ESL1Plum" },
                { "ServerUrl", "ssl:TST-ESB-EMS-1.wfm.pvt:17293,ssl:TST-ESB-EMS-2.wfm.pvt:17293" },
                { "JmsUsername", "mammothUser" },
                { "JmsPassword", "jmsM@mm#!" },
                { "JndiUsername", "jndiMammothUser" },
                { "JndiPassword", "jndiM@mm$$$" },
                { "ConnectionFactoryName", "ItemQueueConnectionFactory" },
                { "LocaleQueueName", "WFMESB.Commerce.Retail.LocaleMgmt.Locale.Queue.V2" },
                { "HierarchyQueueName", "WFMESB.Commerce.Retail.HierarchyMgmt.Hierarchy.Queue.V2" },
                { "ItemQueueName", "WFMESB.Commerce.Retail.ItemMgmt.Item.Queue.V2" },
                { "ProductSelectionGroupQueueName", "WFMESB.Commerce.Retail.ItemMgmt.SelectionGroup.Queue.V2" },
                { "SessionMode", "AutoAcknowledge" },
                { "CertificateName", "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=&quot;Austin &quot;, S=TX, C=US" },
                { "CertificateStoreName", "Root" },
                { "CertificateStoreLocation", "LocalMachine" },
                { "TargetHostName", "TST-ESB-EMS-1.wfm.pvt" },
                { "SslPassword", "esb" },
            };

            public Dictionary<string, string> FakeEsbSettings_Perf_Mammoth_FromEsbSectionForEsb_Lowercase = new Dictionary<string, string>
            {
                { "name", "ESB" },
                { "serverUrl", "ssl://PERF-ESB-EMS-1.wfm.pvt:27293,ssl://PERF-ESB-EMS-2.wfm.pvt:27293" },
                { "jmsUsername", "mammothUser" },
                { "jmsPassword", "jmsM@mm#!" },
                { "jndiUsername", "jndiMammothUser" },
                { "jndiPassword", "jndiM@mm$$$" },
                { "connectionFactoryName", "ItemQueueConnectionFactory" },
                { "queueName", "WFMESB.Enterprise.Item.ItemGateway.Queue.V2" },
                { "sessionMode", "ClientAcknowledge" },
                { "certificateName", "E=uday.bhaskar@wholefoods.com, CN=uday.bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US" },
                { "certificateStoreName", "Root" },
                { "certificateStoreLocation", "LocalMachine" },
                { "targetHostName", "PERF-ESB-EMS-1.wfm.pvt" },
                { "sslPassword", "esb" },
                { "reconnectDelay", "30000" },
            };

            public Dictionary<string, string> FakeEsbSettings_Perf_Mammoth_FromEsbSectionForEsb_Uppercase = new Dictionary<string, string>
            {
                { "Name", "ESB" },
                { "ServerUrl", "ssl://PERF-ESB-EMS-1.wfm.pvt:27293,ssl://PERF-ESB-EMS-2.wfm.pvt:27293" },
                { "JmsUsername", "mammothUser" },
                { "JmsPassword", "jmsM@mm#!" },
                { "JndiUsername", "jndiMammothUser" },
                { "JndiPassword", "jndiM@mm$$$" },
                { "ConnectionFactoryName", "ItemQueueConnectionFactory" },
                { "QueueName", "WFMESB.Enterprise.Item.ItemGateway.Queue.V2" },
                { "SessionMode", "ClientAcknowledge" },
                { "CertificateName", "E=uday.bhaskar@wholefoods.com, CN=uday.bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US" },
                { "CertificateStoreName", "Root" },
                { "CertificateStoreLocation", "LocalMachine" },
                { "TargetHostName", "PERF-ESB-EMS-1.wfm.pvt" },
                { "SslPassword", "esb" },
                { "ReconnectDelay", "30000" },
            };

            public Dictionary<string, string> FakeEsbSettings_Perf_Icon_FromEsbSectionForR10_Lowercase = new Dictionary<string, string>
            {
                { "name", "R10" },
                { "serverUrl", "ssl://PERF-ESB-EMS-1.wfm.pvt:27293,ssl://PERF-ESB-EMS-2.wfm.pvt:27293" },
                { "JmsUsername", "iconUser" },
                { "JmsPassword", "jms!C0n" },
                { "JndiUsername", "jndiIconUser" },
                { "JndiPassword", "jndiIconUser8" },
                { "connectionFactoryName", "ItemQueueConnectionFactory" },
                { "queueName", "WFMESB.Commerce.Retail.ItemMgmt.Item.Queue.V2" },
                { "sessionMode", "ClientAcknowledge" },
                { "certificateName", "E=uday.bhaskar@wholefoods.com, CN=uday.bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US" },
                { "certificateStoreName", "Root" },
                { "certificateStoreLocation", "LocalMachine" },
                { "targetHostName", "PERF-ESB-EMS-1.wfm.pvt" },
                { "sslPassword", "esb" },
                { "reconnectDelay", "30000" },
            };

            public Dictionary<string, string> FakeEsbSettings_Perf_Icon_FromEsbSectionForR10_Uppercase = new Dictionary<string, string>
            {
                { "Name", "R10" },
                { "ServerUrl", "ssl://PERF-ESB-EMS-1.wfm.pvt:27293,ssl://PERF-ESB-EMS-2.wfm.pvt:27293" },
                { "JmsUsername", "iconUser" },
                { "JmsPassword", "jms!C0n" },
                { "JndiUsername", "jndiIconUser" },
                { "JndiPassword", "jndiIconUser8" },
                { "ConnectionFactoryName", "ItemQueueConnectionFactory" },
                { "QueueName", "WFMESB.Commerce.Retail.ItemMgmt.Item.Queue.V2" },
                { "SessionMode", "ClientAcknowledge" },
                { "CertificateName", "E=uday.bhaskar@wholefoods.com, CN=uday.bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US" },
                { "CertificateStoreName", "Root" },
                { "CertificateStoreLocation", "LocalMachine" },
                { "TargetHostName", "PERF-ESB-EMS-1.wfm.pvt" },
                { "SslPassword", "esb" },
                { "ReconnectDelay", "30000" },
            };
        }

        public class EsbConnectionData
        {
            public EsbConnectionData()
            {
                TST_Icon_Single.Add(TST_Icon_ESB);
            }

            public List<EsbConnectionViewModel> TST_Icon_Single = new List<EsbConnectionViewModel>();
   
            public EsbConnectionViewModel TST_Icon_ESB = new EsbConnectionViewModel
            {
                ConnectionName = "ESB",
                ConnectionType = EsbConnectionTypeEnum.Icon,
                EnvironmentEnum = EsbEnvironmentEnum.TEST,
                ServerUrl = "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293",
                TargetHostName = "TST-ESB-EMS-1.wfm.pvt",
                JmsUsername = "iconUser",
                JmsPassword = "jms!C0n",
                JndiUsername = "jndiIconUser",
                JndiPassword = "jndiIconUser8",
                CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                CertificateStoreName = "Root",
                CertificateStoreLocation = "LocalMachine",
                ConnectionFactoryName = "ItemQueueConnectionFactory",
                SessionMode = "ClientAcknowledge",
                SslPassword = "esb",
                QueueName = "WFMESB.Technical.ItemMgmt.PointOfSaleResponse.Reply.Queue.V2",
            };

            public EsbConnectionViewModel TST_Icon_R10 = new EsbConnectionViewModel
            {
                ConnectionName = "R10",
                ConnectionType = EsbConnectionTypeEnum.Icon,
                EnvironmentEnum = EsbEnvironmentEnum.TEST,
                //CanViewPasswords = false,
                ServerUrl = "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293",
                TargetHostName = "TST-ESB-EMS-1.wfm.pvt",
                JmsUsername = "iconUser",
                JmsPassword = "jms!C0n",
                JndiUsername = "jndiIconUser",
                JndiPassword = "jndiIconUser8",
                CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                CertificateStoreName = "Root",
                CertificateStoreLocation = "LocalMachine",
                ConnectionFactoryName = "ItemQueueConnectionFactory",
                SessionMode = "ClientAcknowledge",
                SslPassword = "esb",
                QueueName = "WFMESB.Commerce.Retail.ItemMgmt.Item.Queue.V2",
            };

            public EsbConnectionViewModel TST_Mammoth = new EsbConnectionViewModel
            {
                //ConnectionName = "",
                ConnectionType = EsbConnectionTypeEnum.Mammoth,
                EnvironmentEnum = EsbEnvironmentEnum.TEST,
                //CanViewPasswords = false,
                ServerUrl = "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293",
                TargetHostName = "TST-ESB-EMS-1.wfm.pvt",
                JmsUsername = "mammothUser",
                JmsPassword = "jmsM@mm#!",
                JndiUsername = "jndiMammothUser",
                JndiPassword = "jndiM@mm$$$",
                CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                CertificateStoreName = "Root",
                CertificateStoreLocation = "LocalMachine",
                ConnectionFactoryName = "ItemQueueConnectionFactory",
                SessionMode = "AutoAcknowledge",
                SslPassword = "esb",
                LocaleQueueName = "WFMESB.Commerce.Retail.LocaleMgmt.Locale.Queue.V2",
                HierarchyQueueName = "WFMESB.Commerce.Retail.HierarchyMgmt.Hierarchy.Queue.V2",
                ItemQueueName = "WFMESB.Commerce.Retail.ItemMgmt.Item.Queue.V2",
                ProductSelectionGroupQueueName = "WFMESB.Commerce.Retail.ItemMgmt.SelectionGroup.Queue.V2",
            };

            public EsbConnectionViewModel TST_Mammoth_ESB_ForApiController = new EsbConnectionViewModel
            {
                ConnectionName = "ESB",
                ConnectionType = EsbConnectionTypeEnum.Mammoth,
                EnvironmentEnum = EsbEnvironmentEnum.TEST,
                //CanViewPasswords = false,
                ServerUrl = "ssl:TST-ESB-EMS-1.wfm.pvt:17293,ssl:TST-ESB-EMS-2.wfm.pvt:17293",
                TargetHostName = "TST-ESB-EMS-1.wfm.pvt",
                JmsUsername = "mammothUser",
                JmsPassword = "jmsM@mm#!",
                JndiUsername = "jndiMammothUser",
                JndiPassword = "jndiM@mm$$$",
                CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=&quot;Austin &quot;, S=TX, C=US",
                CertificateStoreName = "Root",
                CertificateStoreLocation = "LocalMachine",
                ConnectionFactoryName = "ItemQueueConnectionFactory",
                SessionMode = "AutoAcknowledge",
                SslPassword = "esb",
                LocaleQueueName = "WFMESB.Commerce.Retail.LocaleMgmt.Locale.Queue.V2",
                HierarchyQueueName = "WFMESB.Commerce.Retail.HierarchyMgmt.Hierarchy.Queue.V2",
                ItemQueueName = "WFMESB.Commerce.Retail.ItemMgmt.Item.Queue.V2",
                ProductSelectionGroupQueueName = "WFMESB.Commerce.Retail.ItemMgmt.SelectionGroup.Queue.V2",
            };

            public EsbConnectionViewModel TST_Mammoth_ESB_ForProductListener = new EsbConnectionViewModel
            {
                ConnectionName = "ESB",
                ConnectionType = EsbConnectionTypeEnum.Mammoth,
                EnvironmentEnum = EsbEnvironmentEnum.TEST,
                //CanViewPasswords = false,
                ServerUrl = "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293",
                TargetHostName = "TST-ESB-EMS-1.wfm.pvt",
                JmsUsername = "mammothUser",
                JmsPassword = "jmsM@mm#!",
                JndiUsername = "jndiMammothUser",
                JndiPassword = "jndiM@mm$$$",
                CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                CertificateStoreName = "Root",
                CertificateStoreLocation = "LocalMachine",
                ConnectionFactoryName = "ItemQueueConnectionFactory",
                SessionMode = "ClientAcknowledge",
                SslPassword = "esb",
                QueueName = "WFMESB.Enterprise.Item.ItemGateway.Queue.V2",
            };

            public EsbConnectionViewModel QA_Mammoth_ESB_ForItemLocaleController = new EsbConnectionViewModel
            {
                ConnectionName = "ESB",
                ConnectionType = EsbConnectionTypeEnum.Mammoth,
                EnvironmentEnum = EsbEnvironmentEnum.QA_FUNC,
                //CanViewPasswords = false,
                ServerUrl = "ssl://QA-ESB-EMS-1.wfm.pvt:27293,ssl://QA-ESB-EMS-2.wfm.pvt:27293",
                TargetHostName = "QA-ESB-EMS-1.wfm.pvt",
                JmsUsername = "mammothUser",
                JmsPassword = "jmsM@mm#!",
                JndiUsername = "jndiMammothUser",
                JndiPassword = "jndiM@mm$$$",
                CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=c\"Austin \", S=TX, C=US",
                CertificateStoreName = "Root",
                CertificateStoreLocation = "LocalMachine",
                ConnectionFactoryName = "ItemQueueConnectionFactory",
                SessionMode = "AutoAcknowledge",
                SslPassword = "esb",
                LocaleQueueName = "WFMESB.Commerce.Retail.LocaleMgmt.Locale.Queue.V2",
                HierarchyQueueName = "WFMESB.Commerce.Retail.HierarchyMgmt.Hierarchy.Queue.V2",
                ItemQueueName = "WFMESB.Commerce.Retail.ItemMgmt.Item.Queue.V2",
                ProductSelectionGroupQueueName = "WFMESB.Commerce.Retail.ItemMgmt.SelectionGroup.Queue.V2",
            };

            public EsbConnectionViewModel TST_Ewic_Listener = new EsbConnectionViewModel
            {
                ConnectionName = "listener",
                ConnectionType = EsbConnectionTypeEnum.Ewic,
                EnvironmentEnum = EsbEnvironmentEnum.TEST,
                //CanViewPasswords = false,
                ServerUrl = "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293",
                TargetHostName = "TST-ESB-EMS-1.wfm.pvt",
                JmsUsername = "ewicIconUser",
                JmsPassword = "ewic\"#*8",
                JndiUsername = "jndiEwicIconUser",
                JndiPassword = "jndiEw!cUser ",
                CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                CertificateStoreName = "Root",
                CertificateStoreLocation = "LocalMachine",
                ConnectionFactoryName = "EwicQueueConnectionFactory",
                SessionMode = "ClientAcknowledge",
                SslPassword = "esb",
                QueueName = "WFMESB.Commerce.Retail.EWICMgmt.EWIC.Queue.V1",
            };

            public EsbConnectionViewModel TST_Ewic_Producer = new EsbConnectionViewModel
            {
                ConnectionName = "producer",
                ConnectionType = EsbConnectionTypeEnum.Ewic,
                EnvironmentEnum = EsbEnvironmentEnum.TEST,
                //CanViewPasswords = false,
                ServerUrl = "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293",
                TargetHostName = "TST-ESB-EMS-1.wfm.pvt",
                JmsUsername = "ewicIconUser",
                JmsPassword = "ewic\"#*8",
                JndiUsername = "jndiEwicIconUser",
                JndiPassword = "jndiEw!cUser",
                CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                CertificateStoreName = "Root",
                CertificateStoreLocation = "LocalMachine",
                ConnectionFactoryName = "EwicQueueConnectionFactory",
                SessionMode = "ClientAcknowledge",
                SslPassword = "esb",
                QueueName = "WFMESB.Commerce.PointOfSaleMgmt.NonSequencedRequest.Queue.V2",
            };
        }

        public class AppLogData
        {
            public IApp FakeAppA = Mock.Of<IApp>();
            public IApp FakeAppB = Mock.Of<IApp>();
            public IApp FakeAppC = Mock.Of<IApp>();
         
            public IAppLog FakeAppLogA1 = null;
            public IAppLog FakeAppLogB1 = null;
            public IAppLog FakeAppLogB2 = null;
            public IAppLog FakeAppLogB3 = null;
            public IAppLog FakeAppLogC1 = null;
            public AppLogEntryViewModel FakeIconAppLogViewModelA1 = null;
            public AppLogEntryViewModel FakeIconAppLogViewModelB1 = null;
            public AppLogEntryViewModel FakeIconAppLogViewModelB2 = null;
            public AppLogEntryViewModel FakeIconAppLogViewModelB3 = null;
            public AppLogEntryViewModel FakeIconAppLogViewModelC1 = null;

            public List<IAppLog> IconAppLogList
            {
                get
                {
                    return new List<IAppLog>()
                    {
                        FakeAppLogA1,
                        FakeAppLogB1,
                        FakeAppLogB2,
                        FakeAppLogB3,
                        FakeAppLogC1
                    };
                }
            }

            public List<AppLogEntryViewModel> IconAppLogEntryViewModelList
            {
                get
                {
                    var list = new List<AppLogEntryViewModel>()
                {
                    FakeIconAppLogViewModelA1,
                    FakeIconAppLogViewModelB1,
                    FakeIconAppLogViewModelB2,
                    FakeIconAppLogViewModelB3,
                    FakeIconAppLogViewModelC1
                };
                    return list;
                }
            }

            public List<IAppLog> MammothAppLogList
            {
                get
                {
                    return IconAppLogList;
                }
            }

            public List<AppLogEntryViewModel> AllFakeMammothAppLogViewModels
            {
                get
                {
                    var list = new List<AppLogEntryViewModel>()
                {
                    FakeIconAppLogViewModelA1,
                    FakeIconAppLogViewModelB1,
                    FakeIconAppLogViewModelB2,
                    FakeIconAppLogViewModelB3,
                    FakeIconAppLogViewModelC1
                };
                    return list;
                }
            }

            public AppLogData()
            {
                FakeAppA.AppID = 5;
                FakeAppA.AppName = "API Controller";

                FakeAppB.AppID = 2;
                FakeAppB.AppName = "ESB Listener";

                FakeAppC.AppID = 11;
                FakeAppC.AppName = "TLog Controller";

                FakeAppLogA1 = Mock.Of<IAppLog>();
                FakeAppLogA1.AppID = FakeAppA.AppID;
                FakeAppLogA1.AppLogID = 1001;
                FakeAppLogA1.InsertDate = new DateTime(2016, 9, 26, 14, 22, 33);
                FakeAppLogA1.Level = "Info";
                //FakeAppLogA1.LogDate = new DateTime(2016, 9, 26, 14, 22, 33);
                Mock.Get(FakeAppLogA1).SetupGet(a => a.LoggingTimestamp).Returns(new DateTime(2016, 9, 26, 14, 22, 33));
                FakeAppLogA1.Logger = "Icon.ApiController.Program.X";
                FakeAppLogA1.MachineName = "TEST_MACHINE_X";
                FakeAppLogA1.Message = "API controller has become self aware and is launching the missiles";
                FakeAppLogA1.UserName = @"WFM\FakeUser";
                Mock.Get(FakeAppLogA1).SetupGet(a => a.AppName).Returns(FakeAppA.AppName);

                FakeIconAppLogViewModelA1 = Mock.Of<AppLogEntryViewModel>();
                FakeIconAppLogViewModelA1.AppID = FakeAppLogA1.AppID;
                FakeIconAppLogViewModelA1.AppLogID = FakeAppLogA1.AppLogID;
                FakeIconAppLogViewModelA1.AppName = FakeAppA.AppName;
                FakeIconAppLogViewModelA1.InsertDate = FakeAppLogA1.InsertDate;
                FakeIconAppLogViewModelA1.Level = FakeAppLogA1.Level;
                FakeIconAppLogViewModelA1.LogDate = FakeAppLogA1.LoggingTimestamp;
                FakeIconAppLogViewModelA1.Logger = FakeAppLogA1.Logger;
                FakeIconAppLogViewModelA1.MachineName = FakeAppLogA1.MachineName;
                FakeIconAppLogViewModelA1.Message = FakeAppLogA1.Message;
                FakeIconAppLogViewModelA1.UserName = FakeAppLogA1.UserName;

                FakeAppLogB1 = Mock.Of<IAppLog>();
                FakeAppLogB1.AppID = FakeAppB.AppID;
                FakeAppLogB1.AppLogID = 2001;
                FakeAppLogB1.InsertDate = new DateTime(2016, 9, 26, 16, 22, 33);
                FakeAppLogB1.Level = "Error";
                //FakeAppLogB1.LogDate = new DateTime(2016, 9, 26, 16, 22, 33);
                Mock.Get(FakeAppLogB1).SetupGet(a => a.LoggingTimestamp).Returns(new DateTime(2016, 9, 26, 16, 22, 33));
                FakeAppLogB1.Logger = "Icon.Esb.R10Listener.R10Listener";
                FakeAppLogB1.MachineName = "TEST_MACHINE_Y";
                FakeAppLogB1.Message = "R10 Listener unable to do anything";
                FakeAppLogB1.UserName = @"WFM\FakeUser";
                //Mock.Get(FakeAppLogB1).SetupGet(a => a).Returns(FakeAppB);
                Mock.Get(FakeAppLogB1).SetupGet(a => a.AppName).Returns(FakeAppB.AppName);

                FakeIconAppLogViewModelB1 = Mock.Of<AppLogEntryViewModel>();
                FakeIconAppLogViewModelB1.AppID = FakeAppLogB1.AppID;
                FakeIconAppLogViewModelB1.AppLogID = FakeAppLogB1.AppLogID;
                FakeIconAppLogViewModelB1.AppName = FakeAppB.AppName;
                FakeIconAppLogViewModelB1.InsertDate = FakeAppLogB1.InsertDate;
                FakeIconAppLogViewModelB1.Level = FakeAppLogB1.Level;
                FakeIconAppLogViewModelB1.LogDate = FakeAppLogB1.LoggingTimestamp;
                FakeIconAppLogViewModelB1.Logger = FakeAppLogB1.Logger;
                FakeIconAppLogViewModelB1.MachineName = FakeAppLogB1.MachineName;
                FakeIconAppLogViewModelB1.Message = FakeAppLogB1.Message;
                FakeIconAppLogViewModelB1.UserName = FakeAppLogB1.UserName;

                FakeAppLogB2 = Mock.Of<IAppLog>();
                FakeAppLogB2.AppID = FakeAppB.AppID;
                FakeAppLogB2.AppLogID = 2002;
                FakeAppLogB2.InsertDate = new DateTime(2016, 9, 26, 16, 22, 35);
                FakeAppLogB2.Level = "Info";
                //FakeAppLogB2.LogDate = new DateTime(2016, 9, 26, 16, 22, 35);
                Mock.Get(FakeAppLogB2).SetupGet(a => a.LoggingTimestamp).Returns(new DateTime(2016, 9, 26, 16, 22, 35));
                FakeAppLogB2.Logger = "Icon.Esb.R10Listener.R10Listener";
                FakeAppLogB2.MachineName = "TEST_MACHINE_Y";
                FakeAppLogB2.Message = "Everything's fine";
                FakeAppLogB2.UserName = @"WFM\FakeUser";
                //Mock.Get(FakeAppLogB2).SetupGet(a => a.App).Returns(FakeAppB);
                Mock.Get(FakeAppLogB2).SetupGet(a => a.AppName).Returns(FakeAppB.AppName);

                FakeIconAppLogViewModelB2 = Mock.Of<AppLogEntryViewModel>();
                FakeIconAppLogViewModelB2.AppID = FakeAppLogB2.AppID;
                FakeIconAppLogViewModelB2.AppLogID = FakeAppLogB2.AppLogID;
                FakeIconAppLogViewModelB2.AppName = FakeAppB.AppName;
                FakeIconAppLogViewModelB2.InsertDate = FakeAppLogB2.InsertDate;
                FakeIconAppLogViewModelB2.Level = FakeAppLogB2.Level;
                FakeIconAppLogViewModelB2.LogDate = FakeAppLogB2.LoggingTimestamp;
                FakeIconAppLogViewModelB2.Logger = FakeAppLogB2.Logger;
                FakeIconAppLogViewModelB2.MachineName = FakeAppLogB2.MachineName;
                FakeIconAppLogViewModelB2.Message = FakeAppLogB2.Message;
                FakeIconAppLogViewModelB2.UserName = FakeAppLogB2.UserName;

                FakeAppLogB3 = Mock.Of<IAppLog>();
                FakeAppLogB3.AppID = FakeAppB.AppID;
                FakeAppLogB3.AppLogID = 2003;
                FakeAppLogB3.InsertDate = new DateTime(2016, 9, 26, 16, 22, 55);
                FakeAppLogB3.Level = "Info";
                //FakeAppLogB3.LogDate = new DateTime(2016, 9, 26, 16, 22, 55);
                Mock.Get(FakeAppLogB3).SetupGet(a => a.LoggingTimestamp).Returns(new DateTime(2016, 9, 26, 16, 22, 55));
                FakeAppLogB3.Logger = "Icon.Esb.R10Listener.R10Listener";
                FakeAppLogB3.MachineName = "TEST_MACHINE_Y";
                FakeAppLogB3.Message = "The quick brown fox jumped over the lazy dog!";
                FakeAppLogB3.UserName = @"WFM\FakeUser";
                //Mock.Get(FakeAppLogB3).SetupGet(a => a.App).Returns(FakeAppB);
                Mock.Get(FakeAppLogB3).SetupGet(a => a.AppName).Returns(FakeAppB.AppName);

                FakeIconAppLogViewModelB3 = Mock.Of<AppLogEntryViewModel>();
                FakeIconAppLogViewModelB3.AppID = FakeAppLogB3.AppID;
                FakeIconAppLogViewModelB3.AppLogID = FakeAppLogB3.AppLogID;
                FakeIconAppLogViewModelB3.AppName = FakeAppB.AppName;
                FakeIconAppLogViewModelB3.InsertDate = FakeAppLogB3.InsertDate;
                FakeIconAppLogViewModelB3.Level = FakeAppLogB3.Level;
                FakeIconAppLogViewModelB3.LogDate = FakeAppLogB3.LoggingTimestamp;
                FakeIconAppLogViewModelB3.Logger = FakeAppLogB3.Logger;
                FakeIconAppLogViewModelB3.MachineName = FakeAppLogB3.MachineName;
                FakeIconAppLogViewModelB3.Message = FakeAppLogB3.Message;
                FakeIconAppLogViewModelB3.UserName = FakeAppLogB3.UserName;

                FakeAppLogC1 = Mock.Of<IAppLog>();
                FakeAppLogC1.AppID = FakeAppC.AppID;
                FakeAppLogC1.AppLogID = 3003;
                FakeAppLogC1.InsertDate = new DateTime(2016, 9, 26, 18, 22, 33);
                FakeAppLogC1.Level = "Info";
                //FakeAppLogC1.LogDate = new DateTime(2016, 9, 26, 18, 22, 33);
                Mock.Get(FakeAppLogC1).SetupGet(a => a.LoggingTimestamp).Returns(new DateTime(2016, 9, 26, 18, 22, 33));
                FakeAppLogC1.Logger = "TlogController.Controller.Processors.TlogProcessor.Z";
                FakeAppLogC1.MachineName = "TEST_MACHINE_Z";
                FakeAppLogC1.Message = "Some stuff happened";
                FakeAppLogC1.UserName = @"WFM\FakeUser";
                //Mock.Get(FakeAppLogC1).SetupGet(a => a.App).Returns(FakeAppC);
                Mock.Get(FakeAppLogC1).SetupGet(a => a.AppName).Returns(FakeAppC.AppName);

                FakeIconAppLogViewModelC1 = Mock.Of<AppLogEntryViewModel>();
                FakeIconAppLogViewModelC1.AppID = FakeAppLogC1.AppID;
                FakeIconAppLogViewModelC1.AppLogID = FakeAppLogC1.AppLogID;
                FakeIconAppLogViewModelC1.AppName = FakeAppC.AppName;
                FakeIconAppLogViewModelC1.InsertDate = FakeAppLogC1.InsertDate;
                FakeIconAppLogViewModelC1.Level = FakeAppLogC1.Level;
                FakeIconAppLogViewModelC1.LogDate = FakeAppLogC1.LoggingTimestamp;
                FakeIconAppLogViewModelC1.Logger = FakeAppLogC1.Logger;
                FakeIconAppLogViewModelC1.MachineName = FakeAppLogC1.MachineName;
                FakeIconAppLogViewModelC1.Message = FakeAppLogC1.Message;
                FakeIconAppLogViewModelC1.UserName = FakeAppLogC1.UserName;
            }
            public IApp GetFakeApiControllerApp(int id = 99, string name = "ICON Test ApiController App")
            {
                var fakeApp = Mock.Of<IApp>();
                fakeApp.AppID = id;
                fakeApp.AppName = name;
                return fakeApp;
            }
        }

        public class ApiJobData
        {
            public IMessageType MessageTypeLocale = Mock.Of<IMessageType>(); //new MessageType() { MessageTypeId = 11, MessageTypeName = IconApiControllerMessageType.Locale };
            public IMessageType MessageTypeHierarchy = Mock.Of<IMessageType>();//new MessageType() { MessageTypeId = 22, MessageTypeName = IconApiControllerMessageType.Hierarchy };
            public IMessageType MessageTypeItemLocale = Mock.Of<IMessageType>();//new MessageType() { MessageTypeId = 33, MessageTypeName = IconApiControllerMessageType.ItemLocale };
            public IMessageType MessageTypePrice = Mock.Of<IMessageType>();// new MessageType() { MessageTypeId = 44, MessageTypeName = IconApiControllerMessageType.Price };
            public IMessageType MessageTypeDepartmentSale = Mock.Of<IMessageType>();//new MessageType() { MessageTypeId = 55, MessageTypeName = IconApiControllerMessageType.DepartmentSale };
            public IMessageType MessageTypeProduct = Mock.Of<IMessageType>();//new MessageType() { MessageTypeId = 66, MessageTypeName = IconApiControllerMessageType.Product };
            public IMessageType MessageTypeCCHTaxUpdate = Mock.Of<IMessageType>();//new MessageType() { MessageTypeId = 77, MessageTypeName = IconApiControllerMessageType.CCHTaxUpdate };
            public IMessageType MessageTypeProductSelectionGroup = Mock.Of<IMessageType>();//new MessageType() { MessageTypeId = 88, MessageTypeName = IconApiControllerMessageType.ProductSelectionGroup };
            public IMessageType MessageTypeeWIC = Mock.Of<IMessageType>();//new MessageType() { MessageTypeId = 99, MessageTypeName = IconApiControllerMessageType.eWIC };
            public IMessageType MessageTypeInforNewItem = Mock.Of<IMessageType>();//new MessageType() { MessageTypeId = 101, MessageTypeName = IconApiControllerMessageType.InforNewItem };

            public IAPIMessageMonitorLog FakeApiMessageMonitorLogA1 = Mock.Of<IAPIMessageMonitorLog>();
            public IAPIMessageMonitorLog FakeApiMessageMonitorLogB1 = Mock.Of<IAPIMessageMonitorLog>();
            public IAPIMessageMonitorLog FakeApiMessageMonitorLogB2 = Mock.Of<IAPIMessageMonitorLog>();
            public IAPIMessageMonitorLog FakeApiMessageMonitorLogB3 = Mock.Of<IAPIMessageMonitorLog>();
            public IAPIMessageMonitorLog FakeApiMessageMonitorLogC1 = Mock.Of<IAPIMessageMonitorLog>();

            public ApiMessageJobSummaryViewModel FakeJobSummaryViewModelA1 = null;
            public ApiMessageJobSummaryViewModel FakeJobSummaryViewModelB1 = null;
            public ApiMessageJobSummaryViewModel FakeJobSummaryViewModelB2 = null;
            public ApiMessageJobSummaryViewModel FakeJobSummaryViewModelB3 = null;
            public ApiMessageJobSummaryViewModel FakeJobSummaryViewModelC1 = null;

            public List<IAPIMessageMonitorLog> ApiMessageMonitorLogList
            {
                get
                {
                    return new List<IAPIMessageMonitorLog>()
                    {
                        FakeApiMessageMonitorLogA1,
                        FakeApiMessageMonitorLogB1,
                        FakeApiMessageMonitorLogB2,
                        FakeApiMessageMonitorLogB3,
                        FakeApiMessageMonitorLogC1
                    };
                }
            }

            public List<ApiMessageJobSummaryViewModel> ApiMessageJobSummaryViewModelList
            {
                get
                {
                    var list = new List<ApiMessageJobSummaryViewModel>()
                    {
                        FakeJobSummaryViewModelA1,
                        FakeJobSummaryViewModelB1,
                        FakeJobSummaryViewModelB2,
                        FakeJobSummaryViewModelB3,
                        FakeJobSummaryViewModelC1
                    };
                    return list;
                }
            }

            public ApiJobData()
            {
                MessageTypeLocale.MessageTypeId = 11;
                MessageTypeLocale.MessageTypeName = IconApiControllerMessageType.Locale;
                MessageTypeHierarchy.MessageTypeId = 22;
                MessageTypeHierarchy.MessageTypeName = IconApiControllerMessageType.Hierarchy;
                MessageTypeItemLocale.MessageTypeId = 33;
                MessageTypeItemLocale.MessageTypeName = IconApiControllerMessageType.ItemLocale;
                MessageTypePrice.MessageTypeId = 44;
                MessageTypePrice.MessageTypeName = IconApiControllerMessageType.Price;
                MessageTypeDepartmentSale.MessageTypeId = 55;
                MessageTypeDepartmentSale.MessageTypeName = IconApiControllerMessageType.DepartmentSale;
                MessageTypeProduct.MessageTypeId = 66;
                MessageTypeProduct.MessageTypeName = IconApiControllerMessageType.Product;
                MessageTypeCCHTaxUpdate.MessageTypeId = 77;
                MessageTypeCCHTaxUpdate.MessageTypeName = IconApiControllerMessageType.CCHTaxUpdate;
                MessageTypeProductSelectionGroup.MessageTypeId = 88;
                MessageTypeProductSelectionGroup.MessageTypeName = IconApiControllerMessageType.ProductSelectionGroup;
                MessageTypeeWIC.MessageTypeId = 99;
                MessageTypeeWIC.MessageTypeName = IconApiControllerMessageType.eWIC;
                MessageTypeInforNewItem.MessageTypeId = 101;
                MessageTypeInforNewItem.MessageTypeName = IconApiControllerMessageType.InforNewItem;

                FakeApiMessageMonitorLogA1 = Mock.Of<IAPIMessageMonitorLog>();
                FakeApiMessageMonitorLogA1.APIMessageMonitorLogID = 11111;
                FakeApiMessageMonitorLogA1.CountFailedMessages = 0;
                FakeApiMessageMonitorLogA1.CountProcessedMessages = 5;
                //FakeApiMessageMonitorLogA1.MessageTypeName = IconApiControllerMessageType.Hierarchy;
                Mock.Get(FakeApiMessageMonitorLogA1).SetupGet(a => a.MessageTypeName).Returns(IconApiControllerMessageType.Hierarchy);
                FakeApiMessageMonitorLogA1.MessageTypeID = MessageTypeHierarchy.MessageTypeId;
                FakeApiMessageMonitorLogA1.StartTime = new DateTime(2016, 9, 26, 5, 15, 30, 100);
                FakeApiMessageMonitorLogA1.EndTime = new DateTime(2016, 9, 26, 5, 15, 30, 627);

                FakeJobSummaryViewModelA1 = Mock.Of<ApiMessageJobSummaryViewModel>();
                FakeJobSummaryViewModelA1.APIMessageMonitorLogID = FakeApiMessageMonitorLogA1.APIMessageMonitorLogID;
                FakeJobSummaryViewModelA1.CountFailedMessages = FakeApiMessageMonitorLogA1.CountFailedMessages;
                FakeJobSummaryViewModelA1.CountProcessedMessages = FakeApiMessageMonitorLogA1.CountProcessedMessages;
                FakeJobSummaryViewModelA1.MessageType = FakeApiMessageMonitorLogA1.MessageTypeName;
                FakeJobSummaryViewModelA1.StartTime = FakeApiMessageMonitorLogA1.StartTime;
                FakeJobSummaryViewModelA1.EndTime = FakeApiMessageMonitorLogA1.EndTime;

                FakeApiMessageMonitorLogB1 = Mock.Of<IAPIMessageMonitorLog>();
                FakeApiMessageMonitorLogB1.APIMessageMonitorLogID = 12221;
                FakeApiMessageMonitorLogB1.CountFailedMessages = 0;
                FakeApiMessageMonitorLogB1.CountProcessedMessages = 12;
                //FakeApiMessageMonitorLogB1.MessageTypeName = IconApiControllerMessageType.Price;
                Mock.Get(FakeApiMessageMonitorLogB1).SetupGet(a => a.MessageTypeName).Returns(IconApiControllerMessageType.Price);
                FakeApiMessageMonitorLogB1.MessageTypeID = MessageTypePrice.MessageTypeId;
                FakeApiMessageMonitorLogB1.StartTime = new DateTime(2016, 9, 26, 5, 20, 20, 444);
                FakeApiMessageMonitorLogB1.EndTime = new DateTime(2016, 9, 26, 5, 20, 21, 222);

                FakeJobSummaryViewModelB1 = Mock.Of<ApiMessageJobSummaryViewModel>();
                FakeJobSummaryViewModelB1.APIMessageMonitorLogID = FakeApiMessageMonitorLogB1.APIMessageMonitorLogID;
                FakeJobSummaryViewModelB1.CountFailedMessages = FakeApiMessageMonitorLogB1.CountFailedMessages;
                FakeJobSummaryViewModelB1.CountProcessedMessages = FakeApiMessageMonitorLogB1.CountProcessedMessages;
                FakeJobSummaryViewModelB1.MessageType = FakeApiMessageMonitorLogB1.MessageTypeName;
                FakeJobSummaryViewModelB1.StartTime = FakeApiMessageMonitorLogB1.StartTime;
                FakeJobSummaryViewModelB1.EndTime = FakeApiMessageMonitorLogB1.EndTime;

                FakeApiMessageMonitorLogB2 = Mock.Of<IAPIMessageMonitorLog>();
                FakeApiMessageMonitorLogB2.APIMessageMonitorLogID = 12223;
                FakeApiMessageMonitorLogB2.CountFailedMessages = 2;
                FakeApiMessageMonitorLogB2.CountProcessedMessages = 3;
                //FakeApiMessageMonitorLogB2.MessageTypeName = IconApiControllerMessageType.Price;
                Mock.Get(FakeApiMessageMonitorLogB2).SetupGet(a => a.MessageTypeName).Returns(IconApiControllerMessageType.Price);
                FakeApiMessageMonitorLogB2.MessageTypeID = MessageTypePrice.MessageTypeId;
                FakeApiMessageMonitorLogB2.StartTime = new DateTime(2016, 9, 26, 5, 20, 20, 444);
                FakeApiMessageMonitorLogB2.EndTime = new DateTime(2016, 9, 26, 5, 20, 24, 333);

                FakeJobSummaryViewModelB2 = Mock.Of<ApiMessageJobSummaryViewModel>();
                FakeJobSummaryViewModelB2.APIMessageMonitorLogID = FakeApiMessageMonitorLogB2.APIMessageMonitorLogID;
                FakeJobSummaryViewModelB2.CountFailedMessages = FakeApiMessageMonitorLogB2.CountFailedMessages;
                FakeJobSummaryViewModelB2.CountProcessedMessages = FakeApiMessageMonitorLogB2.CountProcessedMessages;
                FakeJobSummaryViewModelB2.MessageType = FakeApiMessageMonitorLogB2.MessageTypeName;
                FakeJobSummaryViewModelB2.StartTime = FakeApiMessageMonitorLogB2.StartTime;
                FakeJobSummaryViewModelB2.EndTime = FakeApiMessageMonitorLogB2.EndTime;

                FakeApiMessageMonitorLogB3 = Mock.Of<IAPIMessageMonitorLog>();
                FakeApiMessageMonitorLogB3.APIMessageMonitorLogID = 12223;
                FakeApiMessageMonitorLogB3.CountFailedMessages = 0;
                FakeApiMessageMonitorLogB3.CountProcessedMessages = 125;
                //FakeApiMessageMonitorLogB3.MessageTypeName = IconApiControllerMessageType.Price;
                Mock.Get(FakeApiMessageMonitorLogB3).SetupGet(a => a.MessageTypeName).Returns(IconApiControllerMessageType.Price);
                FakeApiMessageMonitorLogB3.MessageTypeID = MessageTypePrice.MessageTypeId;
                FakeApiMessageMonitorLogB3.StartTime = new DateTime(2016, 9, 26, 5, 20, 24, 108);
                FakeApiMessageMonitorLogB3.EndTime = new DateTime(2016, 9, 26, 5, 20, 24, 788);

                FakeJobSummaryViewModelB3 = Mock.Of<ApiMessageJobSummaryViewModel>();
                FakeJobSummaryViewModelB3.APIMessageMonitorLogID = FakeApiMessageMonitorLogB3.APIMessageMonitorLogID;
                FakeJobSummaryViewModelB3.CountFailedMessages = FakeApiMessageMonitorLogB3.CountFailedMessages;
                FakeJobSummaryViewModelB3.CountProcessedMessages = FakeApiMessageMonitorLogB3.CountProcessedMessages;
                FakeJobSummaryViewModelB3.MessageType = FakeApiMessageMonitorLogB3.MessageTypeName;
                FakeJobSummaryViewModelB3.StartTime = FakeApiMessageMonitorLogB3.StartTime;
                FakeJobSummaryViewModelB3.EndTime = FakeApiMessageMonitorLogB3.EndTime;

                FakeApiMessageMonitorLogC1 = Mock.Of<IAPIMessageMonitorLog>();
                FakeApiMessageMonitorLogC1.APIMessageMonitorLogID = 13331;
                FakeApiMessageMonitorLogC1.CountFailedMessages = 1;
                FakeApiMessageMonitorLogC1.CountProcessedMessages = 24;
                //FakeApiMessageMonitorLogC1.MessageTypeName = IconApiControllerMessageType.ItemLocale;
                Mock.Get(FakeApiMessageMonitorLogC1).SetupGet(a => a.MessageTypeName).Returns(IconApiControllerMessageType.ItemLocale);
                FakeApiMessageMonitorLogC1.MessageTypeID = MessageTypeItemLocale.MessageTypeId;
                FakeApiMessageMonitorLogC1.StartTime = new DateTime(2016, 9, 26, 5, 30, 5, 150);
                FakeApiMessageMonitorLogC1.EndTime = new DateTime(2016, 9, 26, 5, 30, 5, 660);

                FakeJobSummaryViewModelC1 = Mock.Of<ApiMessageJobSummaryViewModel>();
                FakeJobSummaryViewModelC1.APIMessageMonitorLogID = FakeApiMessageMonitorLogC1.APIMessageMonitorLogID;
                FakeJobSummaryViewModelC1.CountFailedMessages = FakeApiMessageMonitorLogC1.CountFailedMessages;
                FakeJobSummaryViewModelC1.CountProcessedMessages = FakeApiMessageMonitorLogC1.CountProcessedMessages;
                FakeJobSummaryViewModelC1.MessageType = FakeApiMessageMonitorLogC1.MessageTypeName;
                FakeJobSummaryViewModelC1.StartTime = FakeApiMessageMonitorLogC1.StartTime;
                FakeJobSummaryViewModelC1.EndTime = FakeApiMessageMonitorLogC1.EndTime;
            }
        }
    }
}
