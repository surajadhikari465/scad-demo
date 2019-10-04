using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Models.CustomConfigElements;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.TestData
{
    public class ConfigTestData
    {
        public ConfigTestData()
        {
            Elements = new ElementData();
            Models = new ModelData();
            ViewModels = new ViewModelData();
            PopulateConfigDataModelWithTypicalValues();
        }

        public ElementData Elements { get; set; }
        public ModelData Models { get; set; }
        public ViewModelData ViewModels { get; set; }

        public DashboardConfigDataModel ConfigDataModel = new DashboardConfigDataModel();

        private void PopulateConfigDataModelWithTypicalValues()
        {
            this.ConfigDataModel.HostingEnvironmentSetting = EnvironmentEnum.Tst0;
            this.ConfigDataModel.SecurityGroupsWithReadRights = new List<string>
            {
                 "IRMA.Applications",
                 "IRMA.Support"
            };
            this.ConfigDataModel.SecurityGroupsWithEditRights = new List<string>
            {
                 "IRMA.Developers"
            };
            this.ConfigDataModel.ServiceCommandTimeout = 10000;
            this.ConfigDataModel.HoursForRecentErrors = 24;
            this.ConfigDataModel.MillisecondsForRecentErrorsPolling = 2000;
            this.ConfigDataModel.EnvironmentCookieName = "environmentName";
            this.ConfigDataModel.EnvironmentAppServersCookieName = "environmentAppServers";
            this.ConfigDataModel.EnvironmentCookieDurationHours = 1;
            this.ConfigDataModel.EnvironmentDefinitions = this.Models.EnvironmentsList.ToList();
            this.ConfigDataModel.EsbEnvironmentDefinitions = this.Models.EsbEnvironmentsList.ToList();
        }

        public class ElementData
        {
            public ElementData()
            {
                EnvironmentsSection.Environments.Add(Dev0);
                EnvironmentsSection.Environments.Add(Dev1);
                EnvironmentsSection.Environments.Add(Tst0);
                EnvironmentsSection.Environments.Add(Tst1);
                EnvironmentsSection.Environments.Add(QA);
                EnvironmentsSection.Environments.Add(QA1);
                EnvironmentsSection.Environments.Add(Perf);
                //EnvironmentsSection.Environments.Add(Prd);

                EsbEnvironmentsSection.EsbEnvironments.Add(EsbDev);
                //EsbEnvironmentsSection.EsbEnvironments.Add(EsbDevDup);
                EsbEnvironmentsSection.EsbEnvironments.Add(EsbTst);
                EsbEnvironmentsSection.EsbEnvironments.Add(EsbTstDup);
                EsbEnvironmentsSection.EsbEnvironments.Add(EsbQaFunc);
                EsbEnvironmentsSection.EsbEnvironments.Add(EsbQaPerf);
                EsbEnvironmentsSection.EsbEnvironments.Add(EsbQaDup);
                //EsbEnvironmentsSection.EsbEnvironments.Add(EsbPrd);
            }

            public EnvironmentsSection EnvironmentsSection = new EnvironmentsSection();

            public EnvironmentsSection EmptyEnvironmentsSection = new EnvironmentsSection();

            public EsbEnvironmentsSection EsbEnvironmentsSection = new EsbEnvironmentsSection();

            public EsbEnvironmentsSection EmptyEsbEnvironmentsSection = new EsbEnvironmentsSection();

            public EnvironmentElement Dev0 = new EnvironmentElement
            {
                Name = "Dev0",
                IsEnabled = true,
                DashboardUrl = @"http://irmadevapp1/IconDashboard",
                WebServer = "irmadevapp1",
                AppServers = "vm-icon-dev1",
                MammothWebSupportUrl = @"http://irmadevapp1/MammothWebSupport",
                IconWebUrl = @"http://icon-dev/",
                TibcoAdminUrls = @"https://cerd1668:8090/",
                IconDatabaseServers = @"cewd1815\sqlshared2012d",
                IconDatabaseCatalogName = "iCONDev",
                MammothDatabaseServers = @"MAMMOTH-DB01-DEV\MAMMOTH",
                MammothDatabaseCatalogName = "Mammoth_Dev",
                IrmaDatabaseServers = @"idd-fl\fld,idd-ma\mad,idd-mw\mwd,idd-na\nad,idd-rm\rmd,idd-so\sod",
                IrmaDatabaseCatalogName = "ItemCatalog_Test",
            };

            public EnvironmentElement Dev1 = new EnvironmentElement
            {
                Name = "Dev1",
                IsEnabled = true,
                DashboardUrl = @"http://IRMATest1Web01/IconDashboard",
                WebServer = "IRMATest1Web01",
                AppServers = "IRMADevJob01,IRMADevJob02",
                MammothWebSupportUrl = @"http://MammothWebSupportDev/",
                IconWebUrl = @"http://IconWebDev/",
                TibcoAdminUrls = @"https://cerd1668:8090/",
                IconDatabaseServers = @"icon-db01-dev\Icon",
                IconDatabaseCatalogName = "icon",
                MammothDatabaseServers = string.Empty,
                MammothDatabaseCatalogName = string.Empty,
                IrmaDatabaseServers = "fl-db01-dev,mw-db01-dev,rm-db01-dev,so-db01-dev,uk-db01-dev",
                IrmaDatabaseCatalogName = "ItemCatalog",
            };

            public EnvironmentElement Tst0 = new EnvironmentElement
            {
                Name = "Tst0",
                IsEnabled = true,
                DashboardUrl = @"http://irmatestapp1/IconDashboard",
                WebServer = "irmatestapp1",
                AppServers = "vm-icon-test1,vm-icon-test2",
                MammothWebSupportUrl = @"http://irmatestapp1/MammothWebSupport",
                IconWebUrl = @"http://icon-test/",
                TibcoAdminUrls = @"https://cerd1669:18090/,https://cerd1670:18090/",
                IconDatabaseServers = @"CEWD1815\SQLSHARED2012D",
                IconDatabaseCatalogName = "iCON",
                MammothDatabaseServers = @"MAMMOTH-DB01-DEV\MAMMOTH",
                MammothDatabaseCatalogName = "Mammoth",
                IrmaDatabaseServers = @"idt-nc\nct,idt-ne\net,idt-pn\pnt,idt-sp\spt,idt-sw\swt,idt-uk\ukt",
                IrmaDatabaseCatalogName = "ItemCatalog_Test",
            };

            public EnvironmentElement Tst1 = new EnvironmentElement
            {
                Name = "Tst1",
                IsEnabled = true,
                DashboardUrl = @"http://IRMATest1Web01/IconDashboard",
                WebServer = "IRMATest1Web01",
                AppServers = "IRMATest1Job01,IRMATest1Job02,IRMATest1Job03,IRMATest1Job04,IRMATest1Job05,IRMATest1Job06",
                MammothWebSupportUrl = "http://MammothWebSupportTest1",
                IconWebUrl = "http://IconWebTest1",
                TibcoAdminUrls = @"https://cerd1669:18090/,https://cerd1670:18090/",
                IconDatabaseServers = @"icon-db01-tst01",
                IconDatabaseCatalogName = "Icon",
                MammothDatabaseServers = @"mammoth-db01-tst01",
                MammothDatabaseCatalogName = "Mammoth_Dev",
                IrmaDatabaseServers = "fl-db01-tst01,ma-db01-tst01,mw-db01-tst01,na-db01-tst01,nc-db01-tst01,ne-db01-tst01,pn-db01-tst01,rm-db01-tst01,so-db01-tst01,sp-db01-tst01,sw-db01-tst01,uk-db01-tst01",
                IrmaDatabaseCatalogName = "ItemCatalog",
            };

            public EnvironmentElement QA = new EnvironmentElement
            {
                Name = "QA",
                IsEnabled = true,
                DashboardUrl = @"http://irmaqaapp1/IconDashboard/",
                WebServer = "irmaqaapp1",
                AppServers = "vm-icon-qa1,vm-icon-qa2,mammoth-app01-qa",
                MammothWebSupportUrl = @"http://irmaqaapp1/MammothWebSupport",
                IconWebUrl = @"http://icon-qa/",
                TibcoAdminUrls = @"https://cerd1673:28090/,https://cerd1674:28090/",
                IconDatabaseServers = @"IDQ-Icon\SQLSHARED3Q",
                IconDatabaseCatalogName = "iCON",
                MammothDatabaseServers = @"MAMMOTH-DB01-QA\MAMMOTH",
                MammothDatabaseCatalogName = "Mammoth",
                IrmaDatabaseServers = @"idq-fl\flq,idq-ma\maq,idq-mw\mwq,idq-na\naq,idq-rm\rmq,idq-so\soq,idq-nc\ncq,idq-ne\neq,idq-pn\pnq,idq-sp\spq,idq-sw\swq,idq-uk\ukq",
                IrmaDatabaseCatalogName = "ItemCatalog",
            };

            public EnvironmentElement QA1 = new EnvironmentElement
            {
                Name = "QA1",
                IsEnabled = true,
                DashboardUrl = @"http://IRMAQAWeb01/IconDashboard/",
                WebServer = "IRMAQAWeb01",
                AppServers = "IRMAQAJob01,IRMAQAJob02,IRMAQAJob03,IRMAQAJob04,IRMAQAJob05,IRMAQAJob06",
                MammothWebSupportUrl = @"http://MammothWebSupportQA",
                IconWebUrl = @"http://IconWebQA",
                TibcoAdminUrls = @"https://cerd1673:28090/,https://cerd1674:28090/",
                IconDatabaseServers = @"icon-db-qa",
                IconDatabaseCatalogName = "ICON",
                MammothDatabaseServers = @"qa-01-mammoth02\mammoth",
                MammothDatabaseCatalogName = "Mammoth",
                IrmaDatabaseServers = @"irma-fl-db-qa,irma-ma-db-qa,irma-mw-db-qa,irma-na-db-qa,irma-nc-db-qa,irma-ne-db-qa,irma-pn-db-qa,irma-rm-db-qa,irma-so-db-qa,irma-sp-db-qa,irma-sw-db-qa,irma-uk-db-qa",
                IrmaDatabaseCatalogName = "ItemCatalog",
            };

            public EnvironmentElement Perf = new EnvironmentElement
            {
                Name = "Perf",
                IsEnabled = true,
                DashboardUrl = @"http://irmaqaapp1/IconDashboardPerf",
                WebServer = "irmaqaapp1",
                AppServers = "vm-icon-qa3,vm-icon-qa4",
                MammothWebSupportUrl = @"http://irmaqaapp1/MammothWebSupportPerf",
                IconWebUrl = @"http://icon-perf/",
                TibcoAdminUrls = @"https://cerd1666:28090/,https://cerd1667:28090/",
                IconDatabaseServers = @"icon-db-perf",
                IconDatabaseCatalogName = "Icon",
                MammothDatabaseServers = @"QA-01-MAMMOTH02\MAMMOTH02",
                MammothDatabaseCatalogName = "Mammoth",
                IrmaDatabaseServers = @"irma-fl-db-perf,irma-ma-db-perf,irma-mw-db-perf,irma-na-db-perf,irma-nc-db-perf,irma-ne-db-perf,irma-pn-db-perf,irma-rm-db-perf,irma-so-db-perf,irma-sp-db-perf,irma-sw-db-perf,irma-uk-db-perf",
                IrmaDatabaseCatalogName = string.Empty,
            };

            public EnvironmentElement Prd
            {
                get
                {
                    return new EnvironmentElement
                    {
                        Name = "Prd",
                        IsEnabled = false,
                        DashboardUrl = @"http://irmaqaapp1/IconDashboardPerf/",
                        WebServer = "irmaprdapp1",
                        AppServers = "vm-icon-prd1,vm-icon-prd2,vm-icon-prd3,vm-icon-prd4,vm-icon-prd5,vm-icon-prd6,mammoth-app01-prd,mammoth-app02-prd",
                        MammothWebSupportUrl = @"http://irmaprdapp1/MammothWebSupport",
                        IconWebUrl = @"https://icon-prd/",
                        TibcoAdminUrls = @"https://cerp1642.wfm.pvt:38090/,https://odrp1633.wfm.pvt:38090/",
                        IconDatabaseServers = @"idp-icon\shared3p",
                        IconDatabaseCatalogName = "Icon",
                        MammothDatabaseServers = @"MAMMOTH-DB01-PRD\MAMMOTH",
                        MammothDatabaseCatalogName = "Mammoth",
                        IrmaDatabaseServers = @"idp-fl\flp,idp-ma\map,idp-mw\mwp,idp-na\nap,idp-rm\rmp,idp-so\sop,idp-nc\ncp,idp-ne\nep,idp-pn\pnp,idp-sp\spp,idp-sw\swp,idp-uk\ukp",
                        IrmaDatabaseCatalogName = "ItemCatalog",
                    };
                }
            }

            public EsbEnvironmentElement EsbDev
            {
                get
                {
                    return new EsbEnvironmentElement
                    {
                        Name = "DEV",
                        ServerUrl = @"ssl://DEV-ESB-EMS-1.wfm.pvt:7233",
                        TargetHostName = "DEV-ESB-EMS-1.wfm.pvt",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        SslPassword = "esb",
                        SessionMode = "ClientAcknowledge",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "jms!C0n",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "jndiIconUser8",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "jmsM@mm#!",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "jndiM@mm$$$",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "ewic\"#*8",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "jndiEw!cUser"
                    };
                }
            }

            public EsbEnvironmentElement EsbTst
            {
                get
                {
                    return new EsbEnvironmentElement
                    {
                        Name = "TEST",
                        ServerUrl = @"ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293",
                        TargetHostName = "TST-ESB-EMS-1.wfm.pvt",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        SslPassword = "esb",
                        SessionMode = "ClientAcknowledge",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "jms!C0n",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "jndiIconUser8",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "jmsM@mm#!",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "jndiM@mm$$$",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "ewic\"#*8",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "jndiEw!cUser"
                    };
                }
            }

            public EsbEnvironmentElement EsbTstDup
            {
                get
                {
                    return new EsbEnvironmentElement
                    {
                        Name = "TEST-DUP",
                        ServerUrl = @"ssl://cerd1637.wfm.pvt:17293",
                        TargetHostName = "cerd1637.wfm.pvt",
                        CertificateName = @"CN=CA-ROOT-I, O=Whole Foods Market",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        SslPassword = "esb",
                        SessionMode = "ClientAcknowledge",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "jms!C0n",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "jndiIconUser8",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "jmsM@mm#!",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "jndiM@mm$$$",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "ewic\"#*8",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "jndiEw!cUser"
                    };
                }
            }

            public EsbEnvironmentElement EsbQaFunc
            {
                get
                {
                    return new EsbEnvironmentElement
                    {
                        Name = "QA-FUNC",
                        ServerUrl = @"ssl://QA-ESB-EMS-1.wfm.pvt:27293,ssl://QA-ESB-EMS-2.wfm.pvt:27293",
                        TargetHostName = "QA-ESB-EMS-1.wfm.pvt",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        SslPassword = "esb",
                        SessionMode = "ClientAcknowledge",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "mehm42415",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "jndiIconUser8",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "jmsM@mm#!QA",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "jndiM@mm$$$QA",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "ewic\"#*8",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "jndiEw!cUser"
                    };
                }
            }

            public EsbEnvironmentElement EsbQaDup
            {
                get
                {
                    return new EsbEnvironmentElement
                    {
                        Name = "QA-DUP",
                        ServerUrl = @"ssl://DUP-ESB-EMS-1.wfm.pvt:27293,ssl://DUP-ESB-EMS-2.wfm.pvt:27293",
                        TargetHostName = "DUP-ESB-EMS-1.wfm.pvt",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        SslPassword = "esb",
                        SessionMode = "ClientAcknowledge",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "jms!C0n",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "jndiIconUser8",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "jmsM@mm#!",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "jndiM@mm$$$",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "ewic\"#*8DUP",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "jndiEw!cUserDUP"
                    };
                }
            }

            public EsbEnvironmentElement EsbQaPerf
            {
                get
                {
                    return new EsbEnvironmentElement
                    {
                        Name = "QA-PERF",
                        ServerUrl = @"ssl://PERF-ESB-EMS-1.wfm.pvt:27293,ssl://PERF-ESB-EMS-2.wfm.pvt:27293",
                        TargetHostName = "PERF-ESB-EMS-1.wfm.pvt",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=uday.bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        SslPassword = "esb",
                        SessionMode = "ClientAcknowledge",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "jms!C0nPERF",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "jndiIconUserPERF",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "jmsM@mm24;PERF",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "jndiM@mm$$$PERF",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "ewic\"#*8",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "jndiEw!cUser"
                    };
                }
            }

            public EsbEnvironmentElement EsbPrd
            {
                get
                {
                    return new EsbEnvironmentElement
                    {
                        Name = "PRD",
                        ServerUrl = "ssl://PROD-ESB-EMS-1.wfm.pvt:37293,ssl://PROD-ESB-EMS-2.wfm.pvt:37293,ssl://PROD-ESB-EMS-3.wfm.pvt:37293, ssl://PROD-ESB-EMS-4.wfm.pvt:37293",
                        TargetHostName = "PROD-ESB-EMS-1.wfm.pvt",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        SslPassword = "esb",
                        SessionMode = "ClientAcknowledge",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = @"*",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = @"*",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = @"*",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = @"*",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = @"*",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = @"*",
                    };
                }
            }
        }

        public class ModelData
        {
            public ModelData()
            {
                EnvironmentsList.Add(Dev0);
                EnvironmentsList.Add(Dev1);
                EnvironmentsList.Add(Tst0);
                EnvironmentsList.Add(Tst1);
                EnvironmentsList.Add(QA);
                EnvironmentsList.Add(QA1);
                EnvironmentsList.Add(Perf);
                EnvironmentsList.Add(Prd);

                EsbEnvironmentsList.Add(EsbDev);
                //EsbEnvironmentsList.Add(EsbDevDup);
                EsbEnvironmentsList.Add(EsbTst);
                EsbEnvironmentsList.Add(EsbTstDup);
                EsbEnvironmentsList.Add(EsbQaFunc);
                EsbEnvironmentsList.Add(EsbQaPerf);
                EsbEnvironmentsList.Add(EsbQaDup);
                //EsbEnvironmentsList.Add(EsbProd);
            }

            public List<EnvironmentModel> EnvironmentsList = new List<EnvironmentModel>();

            public List<EsbEnvironmentModel> EsbEnvironmentsList = new List<EsbEnvironmentModel>();

            public EnvironmentModel Dev0
            {
                get
                {
                    return new EnvironmentModel
                    {
                        Name = "Dev0",
                        EnvironmentEnum = EnvironmentEnum.Dev0,
                        IsEnabled = true,
                        DashboardUrl = @"http://irmadevapp1/IconDashboard",
                        WebServer = "irmadevapp1",
                        AppServers = new List<string> { "vm-icon-dev1" },
                        MammothWebSupportUrl = @"http://irmadevapp1/MammothWebSupport",
                        IconWebUrl = @"http://icon-dev/",
                        TibcoAdminUrls = new List<string> { @"https://cerd1668:8090/" },
                        IconDatabaseServer = @"cewd1815\sqlshared2012d",
                        IconDatabaseName = "iCONDev",
                        MammothDatabaseServer = @"MAMMOTH-DB01-DEV\MAMMOTH",
                        MammothDatabaseName = "Mammoth_Dev",
                        IrmaDatabaseServers = new List<string> { @"idd-fl\fld", @"idd-ma\mad", @"idd-mw\mwd", @"idd-na\nad", @"idd-rm\rmd", @"idd-so\sod" },
                        IrmaDatabaseName = "ItemCatalog_Test",
                    };
                }
            }

            public EnvironmentModel Dev1
            {
                get
                {
                    return new EnvironmentModel
                    {
                        Name = "Dev1",
                        EnvironmentEnum = EnvironmentEnum.Dev1,
                        IsEnabled = true,
                        DashboardUrl = @"http://IrmaDevWeb01/IconDashboard",
                        WebServer = "IrmaDevWeb01",
                        AppServers = new List<string> { "IRMADevJob01", "IRMADevJob02" },
                        MammothWebSupportUrl = @"http://MammothWebSupportDev/",
                        IconWebUrl = @"http://IconWebDev/",
                        TibcoAdminUrls = new List<string> { @"https://cerd1668:8090/" },
                        IconDatabaseServer = @"icon-db01-dev",
                        IconDatabaseName = "icon",
                        MammothDatabaseServer = string.Empty,
                        MammothDatabaseName = string.Empty,
                        IrmaDatabaseServers = new List<string> { "fl-db01-dev", "mw-db01-dev", "rm-db01-dev", "so-db01-dev", "uk-db01-dev" },
                        IrmaDatabaseName = "ItemCatalog",
                    };
                }
            }

            public EnvironmentModel Tst0
            {
                get
                {
                    return new EnvironmentModel
                    {
                        Name = "Tst0",
                        EnvironmentEnum = EnvironmentEnum.Tst0,
                        IsEnabled = true,
                        DashboardUrl = @"http://irmatestapp1/IconDashboard",
                        WebServer = "irmatestapp1",
                        AppServers = new List<string> { "vm-icon-test1", "vm-icon-test2" },
                        MammothWebSupportUrl = @"http://irmatestapp1/MammothWebSupport",
                        IconWebUrl = @"http://icon-test/",
                        TibcoAdminUrls = new List<string> { @"https://cerd1669:18090/", @"https://cerd1670:18090/" },
                        IconDatabaseServer = @"CEWD1815\SQLSHARED2012D",
                        IconDatabaseName = "iCON",
                        MammothDatabaseServer = @"MAMMOTH-DB01-DEV\MAMMOTH",
                        MammothDatabaseName = "Mammoth",
                        IrmaDatabaseServers = new List<string> { @"idt-nc\nct", @"idt-ne\net", @"idt-pn\pnt", @"idt-sp\spt", @"idt-sw\swt", @"idt-uk\ukt" },
                        IrmaDatabaseName = "ItemCatalog_Test",
                    };
                }
            }

            public EnvironmentModel Tst1
            {
                get
                {
                    return new EnvironmentModel
                    {
                        Name = "Tst1",
                        EnvironmentEnum = EnvironmentEnum.Tst1,
                        IsEnabled = true,
                        DashboardUrl = @"http://IRMATest1Web01/IconDashboard",
                        WebServer = "IRMATest1Web01",
                        AppServers = new List<string> { "IRMATest1Job01", "IRMATest1Job02", "IRMATest1Job03", "IRMATest1Job04", "IRMATest1Job05", "IRMATest1Job06" },
                        MammothWebSupportUrl = "http://MammothWebSupportTest1",
                        IconWebUrl = "http://IconWebTest1",
                        TibcoAdminUrls = new List<string> { @"https://cerd1669:18090/", @"https://cerd1670:18090/" },
                        IconDatabaseServer = @"icon-db01-tst01",
                        IconDatabaseName = "Icon",
                        MammothDatabaseServer = @"mammoth-db01-tst01",
                        MammothDatabaseName = "Mammoth",
                        IrmaDatabaseServers = new List<string> { "fl-db01-tst01", "ma-db01-tst01", "mw-db01-tst01", "na-db01-tst01", "nc-db01-tst01", "ne-db01-tst01", "pn-db01-tst01", "rm-db01-tst01", "so-db01-tst01", "sp-db01-tst01", "sw-db01-tst01", "uk-db01-tst01" },
                        IrmaDatabaseName = "ItemCatalog",
                    };
                }
            }

            public EnvironmentModel QA
            {
                get
                {
                    return new EnvironmentModel
                    {
                        Name = "QA",
                        EnvironmentEnum = EnvironmentEnum.QA,
                        IsEnabled = true,
                        DashboardUrl = @"http://irmaqaapp1/IconDashboard/",
                        WebServer = "irmaqaapp1",
                        AppServers = new List<string> { "vm-icon-qa1", "vm-icon-qa2", "mammoth-app01-qa" },
                        MammothWebSupportUrl = @"http://irmaqaapp1/MammothWebSupport",
                        IconWebUrl = @"http://icon-qa/",
                        TibcoAdminUrls = new List<string> { @"https://cerd1673:28090/", @"https://cerd1674:28090/" },
                        IconDatabaseServer = @"IDQ-Icon\SQLSHARED3Q",
                        IconDatabaseName = "iCON",
                        MammothDatabaseServer = @"MAMMOTH-DB01-QA\MAMMOTH",
                        MammothDatabaseName = "Mammoth",
                        IrmaDatabaseServers = new List<string> { @"idq-fl\flq", @"idq-ma\maq", @"idq-mw\mwq", @"idq-na\naq", @"idq-rm\rmq", @"idq-so\soq", @"idq-nc\ncq", @"idq-ne\neq", @"idq-pn\pnq", @"idq-sp\spq", @"idq-sw\swq", @"idq-uk\ukq" },
                        IrmaDatabaseName = "ItemCatalog",
                    };
                }
            }

            public EnvironmentModel QA1
            {
                get
                {
                    return new EnvironmentModel
                    {
                        Name = "QA1",
                        EnvironmentEnum = EnvironmentEnum.QA1,
                        IsEnabled = true,
                        DashboardUrl = @"http://IRMAQAWeb01/IconDashboard/",
                        WebServer = "IRMAQAWeb01",
                        AppServers = new List<string> { "IRMAQAJob01", "IRMAQAJob02", "IRMAQAJob03", "IRMAQAJob04", "IRMAQAJob05", "IRMAQAJob06" },
                        MammothWebSupportUrl = @"http://MammothWebSupportQA",
                        IconWebUrl = @"http://IconWebQA",
                        TibcoAdminUrls = new List<string> { @"https://cerd1673:28090/", @"https://cerd1674:28090/" },
                        IconDatabaseServer = @"icon-db-qa",
                        IconDatabaseName = "ICON",
                        MammothDatabaseServer = @"qa-01-mammoth02\mammoth",
                        MammothDatabaseName = "Mammoth",
                        IrmaDatabaseServers = new List<string> { @"irma-fl-db-qa", "irma-ma-db-qa", "irma-mw-db-qa", "irma-na-db-qa", "irma-nc-db-qa", "irma-ne-db-qa", "irma-pn-db-qa", "irma-rm-db-qa", "irma-so-db-qa", "irma-sp-db-qa", "irma-sw-db-qa", "irma-uk-db-qa" },
                        IrmaDatabaseName = "ItemCatalog",
                    };
                }
            }

            public EnvironmentModel Perf
            {
                get
                {
                    return new EnvironmentModel
                    {
                        Name = "Perf",
                        EnvironmentEnum = EnvironmentEnum.Perf,
                        IsEnabled = true,
                        DashboardUrl = @"http://irmaqaapp1/IconDashboardPerf",
                        WebServer = "irmaqaapp1",
                        AppServers = new List<string> { "vm-icon-qa3", "vm-icon-qa4" },
                        MammothWebSupportUrl = @"http://irmaqaapp1/MammothWebSupportPerf",
                        IconWebUrl = @"http://icon-perf/",
                        TibcoAdminUrls = new List<string> { @"https://cerd1666:28090/", @"https://cerd1667:28090/" },
                        IconDatabaseServer = @"CEWD1815\SQLSHARED2012D",
                        IconDatabaseName = "iCONLoadTest",
                        MammothDatabaseServer = @"qa-01-mammoth02\mammoth02",
                        MammothDatabaseName = "Mammoth",
                        IrmaDatabaseServers = new List<string> { "irma-fl-db-perf", "irma-ma-db-perf", "irma-mw-db-perf", "irma-na-db-perf", "irma-nc-db-perf", "irma-ne-db-perf", "irma-pn-db-perf", "irma-rm-db-perf", "irma-so-db-perf", "irma-sp-db-perf", "irma-sw-db-perf", "irma-uk-db-perf" },
                        IrmaDatabaseName = "ItemCatalog",
                    };
                }
            }

            public EnvironmentModel Prd
            {
                get
                {
                    return new EnvironmentModel
                    {
                        Name = "Prd",
                        EnvironmentEnum = EnvironmentEnum.Prd,
                        IsEnabled = false,
                        DashboardUrl = @"http://irmaqaapp1/IconDashboardPerf/",
                        WebServer = "irmaprdapp1",
                        AppServers = new List<string> { "vm-icon-prd1", "vm-icon-prd2", "vm-icon-prd3", "vm-icon-prd4", "vm-icon-prd5", "vm-icon-prd6", "mammoth-app01-prd", "mammoth-app02-prd" },
                        MammothWebSupportUrl = @"http://irmaprdapp1/MammothWebSupport",
                        IconWebUrl = @"https://icon-prd/",
                        TibcoAdminUrls = new List<string> { @"https://cerp1642.wfm.pvt:38090/", @"https://odrp1633.wfm.pvt:38090/" },
                        IconDatabaseServer = @"idp-icon\shared3p",
                        IconDatabaseName = "Icon",
                        MammothDatabaseServer = @"MAMMOTH-DB01-PRD\MAMMOTH",
                        MammothDatabaseName = "Mammoth",
                        IrmaDatabaseServers = new List<string> { @"idp-fl\flp", @"idp-ma\map", @"idp-mw\mwp", @"idp-na\nap", @"idp-rm\rmp", @"idp-so\sop", @"idp-nc\ncp", @"idp-ne\nep", @"idp-pn\pnp", @"idp-sp\spp", @"idp-sw\swp", @"idp-uk\ukp" },
                        IrmaDatabaseName = "ItemCatalog",
                    };
                }
            }

            public EsbEnvironmentModel EsbDev
            {
                get
                {
                    return new EsbEnvironmentModel
                    {
                        Name = "DEV",
                        EsbEnvironment = EsbEnvironmentEnum.DEV,
                        ServerUrl = "ssl://DEV-ESB-EMS-1.wfm.pvt:7233",
                        TargetHostName = "DEV-ESB-EMS-1.wfm.pvt",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        SslPassword = "esb",
                        SessionMode = "ClientAcknowledge",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "jms!C0n",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "jndiIconUser8",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "jmsM@mm#!",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "jndiM@mm$$$",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "ewic\"#*8",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "jndiEw!cUser"
                    };
                }
            }

            public EsbEnvironmentModel EsbDevDup
            {
                get
                {
                    return new EsbEnvironmentModel
                    {
                        Name = "DEV-DUP",
                        EsbEnvironment = EsbEnvironmentEnum.DEV_DUP,
                        ServerUrl = "ssl://cerd1636.wfm.pvt:7233",
                        TargetHostName = "cerd1636.wfm.pvt",
                        CertificateName = @"CN=CA-ROOT-I, O=Whole Foods Market",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        SslPassword = "esb",
                        SessionMode = "ClientAcknowledge",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "jms!C0n",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "jndiIconUser8",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "jmsM@mm#!",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "jndiM@mm$$$",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "ewic\"#*8",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "jndiEw!cUser"
                    };
                }
            }

            public EsbEnvironmentModel EsbTst
            {
                get
                {
                    return new EsbEnvironmentModel
                    {
                        Name = "TEST",
                        EsbEnvironment = EsbEnvironmentEnum.TEST,
                        ServerUrl = "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293",
                        TargetHostName = "TST-ESB-EMS-1.wfm.pvt",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        SslPassword = "esb",
                        SessionMode = "ClientAcknowledge",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "jms!C0n",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "jndiIconUser8",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "jmsM@mm#!",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "jndiM@mm$$$",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "ewic\"#*8",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "jndiEw!cUser"
                    };
                }
            }

            public EsbEnvironmentModel EsbTstDup
            {
                get
                {
                    return new EsbEnvironmentModel
                    {
                        Name = "TEST-DUP",
                        EsbEnvironment = EsbEnvironmentEnum.TEST_DUP,
                        ServerUrl = "ssl://cerd1637.wfm.pvt:17293",
                        TargetHostName = "cerd1637.wfm.pvt",
                        CertificateName = @"E=suchetha.aleti@wholefoods.com, CN=cerd1617.wfm.pvt, OU=InfraESBAdmins@wholefoods.com, O=""Whole Foods Market "", L=Austin, S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        SslPassword = "esb",
                        SessionMode = "ClientAcknowledge",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "jms!C0n",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "jndiIconUser*",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "jmsM@mm#!",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "jndiM@mm$$$",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "ewic\"#*8",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "jndiEw!cUser"
                    };
                }
            }

            public EsbEnvironmentModel EsbQaFunc
            {
                get
                {
                    return new EsbEnvironmentModel
                    {
                        Name = "QA-FUNC",
                        EsbEnvironment = EsbEnvironmentEnum.QA_FUNC,
                        ServerUrl = "ssl://QA-ESB-EMS-1.wfm.pvt:27293,ssl://QA-ESB-EMS-2.wfm.pvt:27293",
                        TargetHostName = "QA-ESB-EMS-1.wfm.pvt",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        SslPassword = "esb",
                        SessionMode = "ClientAcknowledge",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "mehm42415",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "jndiIconUser8",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "jmsM@mm#!QA",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "jndiM@mm$$$QA",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "ewic\"#*8",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "jndiEw!cUser"
                    };
                }
            }

            public EsbEnvironmentModel EsbQaDup
            {
                get
                {
                    return new EsbEnvironmentModel
                    {
                        Name = "QA-DUP",
                        EsbEnvironment = EsbEnvironmentEnum.QA_DUP,
                        ServerUrl = "ssl://DUP-ESB-EMS-1.wfm.pvt:27293,ssl://DUP-ESB-EMS-2.wfm.pvt:27293",
                        TargetHostName = "DUP-ESB-EMS-1.wfm.pvt",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        SslPassword = "esb",
                        SessionMode = "ClientAcknowledge",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "jms!C0n",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "jndiIconUser8",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "jmsM@mm#!",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "jndiM@mm$$$",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "ewic\"#*8DUP",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "jndiEw!cUserDUP"
                    };
                }
            }

            public EsbEnvironmentModel EsbQaPerf
            {
                get
                {
                    return new EsbEnvironmentModel
                    {
                        Name = "QA-PERF",
                        EsbEnvironment = EsbEnvironmentEnum.QA_PERF,
                        ServerUrl = "ssl://PERF-ESB-EMS-1.wfm.pvt:27293,ssl://PERF-ESB-EMS-2.wfm.pvt:27293",
                        TargetHostName = "PERF-ESB-EMS-1.wfm.pvt",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=uday.bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        SslPassword = "esb",
                        SessionMode = "ClientAcknowledge",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "jms!C0nPERF",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "jndiIconUserPERF",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "jmsM@mm24;PERF",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "jndiM@mm$$$PERF",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "ewic\"#*8",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "jndiEw!cUser"
                    };
                }
            }

            public EsbEnvironmentModel EsbProd
            {
                get
                {
                    return new EsbEnvironmentModel
                    {
                        Name = "PROD",
                        EsbEnvironment = EsbEnvironmentEnum.PRD,
                        ServerUrl = "ssl://PROD-ESB-EMS-1.wfm.pvt:37293,ssl://PROD-ESB-EMS-2.wfm.pvt:37293,ssl://PROD-ESB-EMS-3.wfm.pvt:37293, ssl://PROD-ESB-EMS-4.wfm.pvt:37293",
                        TargetHostName = "PROD-ESB-EMS-1.wfm.pvt",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        SslPassword = "esb",
                        SessionMode = "ClientAcknowledge",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "*",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "*",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "*",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "*",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "*",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "*"
                    };
                }
            }
        }

        public class ViewModelData
        {
            public ViewModelData()
            {
                EnvironmentsList.Add(Dev0);
                EnvironmentsList.Add(Dev1);
                EnvironmentsList.Add(Tst0);
                EnvironmentsList.Add(Tst1);
                EnvironmentsList.Add(QA);
                EnvironmentsList.Add(Perf);
                //EnvironmentsList.Add(Prd);

                EsbEnvironmentsList.Add(EsbDev);
                EsbEnvironmentsList.Add(EsbDevDup);
                EsbEnvironmentsList.Add(EsbTst);
                EsbEnvironmentsList.Add(EsbTstDup);
                EsbEnvironmentsList.Add(EsbQaFunc);
                EsbEnvironmentsList.Add(EsbQaPerf);
                EsbEnvironmentsList.Add(EsbQaDup);

                EnvironmentCollectionViewModel.Environments = EnvironmentsList.ToList();
            }

            public DashboardEnvironmentCollectionViewModel EnvironmentCollectionViewModel = new DashboardEnvironmentCollectionViewModel();

            public List<EnvironmentViewModel> EnvironmentsList = new List<EnvironmentViewModel>();

            public List<EsbEnvironmentViewModel> EsbEnvironmentsList = new List<EsbEnvironmentViewModel>();

            public EnvironmentViewModel Dev0
            {
                get
                {
                    return new EnvironmentViewModel
                    {
                        Name = "Dev0",
                        EnvironmentEnum = EnvironmentEnum.Dev0,
                        IsHostingEnvironment = false,
                        IsProduction = false,
                        BootstrapClass = "default",
                        AppServers = new List<AppServerViewModel>
                        {
                            new AppServerViewModel("vm-icon-dev1")
                        }
                    };
                }
            }

            public EnvironmentViewModel Dev1
            {
                get
                {
                    return new EnvironmentViewModel
                    {
                        Name = "Dev1",
                        EnvironmentEnum = EnvironmentEnum.Dev1,
                        IsHostingEnvironment = false,
                        IsProduction = false,
                        BootstrapClass = "default",
                        AppServers = new List<AppServerViewModel>
                        {

                        }
                    };
                }
            }

            public EnvironmentViewModel Tst0
            {
                get
                {
                    return new EnvironmentViewModel
                    {
                        Name = "Tst0",
                        EnvironmentEnum = EnvironmentEnum.Tst0,
                        IsHostingEnvironment = true,
                        IsProduction = false,
                        BootstrapClass = "primary",
                        AppServers = new List<AppServerViewModel>
                        {
                            new AppServerViewModel("vm-icon-test1"),
                            new AppServerViewModel("vm-icon-test2")
                        }
                    };
                }
            }

            public EnvironmentViewModel Tst1
            {
                get
                {
                    return new EnvironmentViewModel
                    {
                        Name = "Tst1",
                        EnvironmentEnum = EnvironmentEnum.Tst1,
                        IsHostingEnvironment = false,
                        IsProduction = false,
                        BootstrapClass = "primary",
                        AppServers = new List<AppServerViewModel>
                        {
                            new AppServerViewModel("IRMATest1Job01"),
                            new AppServerViewModel("IRMATest1Job02"),
                            new AppServerViewModel("IRMATest1Job03"),
                            new AppServerViewModel("IRMATest1Job04"),
                            new AppServerViewModel("IRMATest1Job05"),
                            new AppServerViewModel("IRMATest1Job06"),
                        }
                    };
                }
            }

            public EnvironmentViewModel QA
            {
                get
                {
                    return new EnvironmentViewModel
                    {
                        Name = "QA",
                        EnvironmentEnum = EnvironmentEnum.QA,
                        IsHostingEnvironment = false,
                        IsProduction = false,
                        BootstrapClass = "warning",
                        AppServers = new List<AppServerViewModel>
                        {
                            new AppServerViewModel("vm-icon-qa1"),
                            new AppServerViewModel("vm-icon-qa2"),
                            new AppServerViewModel("mammoth-app01-qa")
                        }
                    };
                }
            }

            public EnvironmentViewModel Perf
            {
                get
                {
                    return new EnvironmentViewModel
                    {
                        Name = "Perf",
                        EnvironmentEnum = EnvironmentEnum.Perf,
                        IsHostingEnvironment = false,
                        IsProduction = false,
                        BootstrapClass = "info",
                        AppServers = new List<AppServerViewModel>
                        {
                            new AppServerViewModel("vm-icon-qa3"),
                            new AppServerViewModel("vm-icon-qa4"),
                        }
                    };
                }
            }

            public EnvironmentViewModel Prd
            {
                get
                {
                    return new EnvironmentViewModel
                    {
                        Name = "Prd",
                        EnvironmentEnum = EnvironmentEnum.Prd,
                        IsHostingEnvironment = false,
                        IsProduction = true,
                        BootstrapClass = "danger",
                        AppServers = new List<AppServerViewModel>
                        {
                            new AppServerViewModel("vm-icon-prd1"),
                            new AppServerViewModel("vm-icon-prd2"),
                            new AppServerViewModel("vm-icon-prd3"),
                            new AppServerViewModel("vm-icon-prd4"),
                            new AppServerViewModel("vm-icon-prd5"),
                            new AppServerViewModel("vm-icon-prd6"),
                            new AppServerViewModel("mammoth-app01-prd"),
                            new AppServerViewModel("mammoth-app02-prd")
                        }
                    };
                }
            }

            public EsbEnvironmentViewModel EsbDev
            {
                get
                {
                    return new EsbEnvironmentViewModel
                    {
                        Name = "DEV",
                        EsbEnvironment = EsbEnvironmentEnum.DEV,
                        ServerUrl = "ssl://DEV-ESB-EMS-1.wfm.pvt:7233",
                        TargetHostName = "DEV-ESB-EMS-1.wfm.pvt",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "jms!C0n",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "jndiIconUser8",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "jmsM@mm#!",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "jndiM@mm$$$",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "ewic\"#*8",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "jndiEw!cUser"
                    };
                }
            }

            public EsbEnvironmentViewModel EsbDevDup
            {
                get
                {
                    return new EsbEnvironmentViewModel
                    {
                        Name = "DEV-DUP",
                        EsbEnvironment = EsbEnvironmentEnum.DEV_DUP,
                        ServerUrl = "ssl://cerd1636.wfm.pvt:7233",
                        TargetHostName = "cerd1636.wfm.pvt",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "jms!C0n",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "jndiIconUser8",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "jmsM@mm#!",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "jndiM@mm$$$",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "ewic\"#*8",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "jndiEw!cUser"
                    };
                }
            }

            public EsbEnvironmentViewModel EsbTst
            {
                get
                {
                    return new EsbEnvironmentViewModel
                    {
                        Name = "TEST",
                        EsbEnvironment = EsbEnvironmentEnum.TEST,
                        ServerUrl = "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293",
                        TargetHostName = "TST-ESB-EMS-1.wfm.pvt",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "jms!C0n",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "jndiIconUser8",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "jmsM@mm#!",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "jndiM@mm$$$",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "ewic\"#*8",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "jndiEw!cUser"
                    };
                }
            }

            public EsbEnvironmentViewModel EsbTstDup
            {
                get
                {
                    return new EsbEnvironmentViewModel
                    {
                        Name = "TEST-DUP",
                        EsbEnvironment = EsbEnvironmentEnum.TEST_DUP,
                        ServerUrl = "ssl://cerd1637.wfm.pvt:17293",
                        TargetHostName = "cerd1637.wfm.pvt",
                        CertificateName = @"CN=CA-ROOT-I, O=Whole Foods Market",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "jms!C0n",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "jndiIconUser8",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "jmsM@mm#!",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "jndiM@mm$$$",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "ewic\"#*8",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "jndiEw!cUser"
                    };
                }
            }

            public EsbEnvironmentViewModel EsbQaFunc
            {
                get
                {
                    return new EsbEnvironmentViewModel
                    {
                        Name = "QA-FUNC",
                        EsbEnvironment = EsbEnvironmentEnum.QA_FUNC,
                        ServerUrl = "ssl://QA-ESB-EMS-1.wfm.pvt:27293,ssl://QA-ESB-EMS-2.wfm.pvt:27293",
                        TargetHostName = "QA-ESB-EMS-1.wfm.pvt",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "mehm42415",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "jndiIconUser8",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "jmsM@mm#!QA",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "jndiM@mm$$$QA",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "ewic\"#*8",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "jndiEw!cUser"
                    };
                }
            }

            public EsbEnvironmentViewModel EsbQaDup
            {
                get
                {
                    return new EsbEnvironmentViewModel
                    {
                        Name = "QA-DUP",
                        EsbEnvironment = EsbEnvironmentEnum.QA_DUP,
                        ServerUrl = "ssl://DUP-ESB-EMS-1.wfm.pvt:27293,ssl://DUP-ESB-EMS-2.wfm.pvt:27293",
                        TargetHostName = "DUP-ESB-EMS-1.wfm.pvt",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "jms!C0n",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "jndiIconUser8",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "jmsM@mm#!",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "jndiM@mm$$$",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "ewic\"#*8DUP",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "jndiEw!cUserDUP"
                    };
                }
            }

            public EsbEnvironmentViewModel EsbQaPerf
            {
                get
                {
                    return new EsbEnvironmentViewModel
                    {
                        Name = "QA-PERF",
                        EsbEnvironment = EsbEnvironmentEnum.QA_PERF,
                        ServerUrl = "ssl://PERF-ESB-EMS-1.wfm.pvt:27293,ssl://PERF-ESB-EMS-2.wfm.pvt:27293",
                        TargetHostName = "PERF-ESB-EMS-1.wfm.pvt",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=uday.bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "jms!C0nPERF",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "jndiIconUserPERF",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "jmsM@mm24;PERF",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "jndiM@mm$$$PERF",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "ewic\"#*8",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "jndiEw!cUser"
                    };
                }
            }

            public EsbEnvironmentViewModel EsbQaProd
            {
                get
                {
                    return new EsbEnvironmentViewModel
                    {
                        Name = "PROD",
                        EsbEnvironment = EsbEnvironmentEnum.PRD,
                        ServerUrl = "ssl://PROD-ESB-EMS-1.wfm.pvt:37293,ssl://PROD-ESB-EMS-2.wfm.pvt:37293,ssl://PROD-ESB-EMS-3.wfm.pvt:37293,ssl://PROD-ESB-EMS-4.wfm.pvt:37293",
                        TargetHostName = "PROD-ESB-EMS-1.wfm.pvt",
                        CertificateName = "E=uday.bhaskar@wholefoods.com, CN=uday.bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US",
                        CertificateStoreName = "Root",
                        CertificateStoreLocation = "LocalMachine",
                        JmsUsernameIcon = "iconUser",
                        JmsPasswordIcon = "*",
                        JndiUsernameIcon = "jndiIconUser",
                        JndiPasswordIcon = "*",
                        JmsUsernameMammoth = "mammothUser",
                        JmsPasswordMammoth = "*",
                        JndiUsernameMammoth = "jndiMammothUser",
                        JndiPasswordMammoth = "*",
                        JmsUsernameEwic = "ewicIconUser",
                        JmsPasswordEwic = "*",
                        JndiUsernameEwic = "jndiIconUserEwic",
                        JndiPasswordEwic = "*"
                    };
                }
            }
        }
    }
}
