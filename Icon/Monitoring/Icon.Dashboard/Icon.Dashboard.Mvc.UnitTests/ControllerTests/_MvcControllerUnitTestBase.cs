using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.DataFileAccess.Models;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Icon.Dashboard.Mvc.UnitTests.ControllerTests
{
    public class _MvcControllerUnitTestBase
    {
        protected string dataFilePath = "MvcDashboardUnitTestData.xml";
        protected Mock<HttpServerUtilityBase> mockServer = new Mock<HttpServerUtilityBase>();
        protected Mock<IDataFileServiceWrapper> mockDataServiceWrapper = new Mock<IDataFileServiceWrapper>();
        protected Mock<IIconDatabaseServiceWrapper> mockIconLoggingServiceWrapper = new Mock<IIconDatabaseServiceWrapper>();
        protected Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();

        public _MvcControllerUnitTestBase()
        {
            SetupFakeIconApplicationData();
            SetupFakeIconAppLogData();
            SetupFakeApiJobSummaryData();
        }

        protected void SetMockHttpContext(Controller controller)
        {
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), controller);
        }

        protected void SetupMockHttpContext(Controller controller, object itemsKey = null, object itemsValue = null)
        {
            SetMockHttpContext(controller);
            var itemsDictionary = new Dictionary<object, object>();
            if (itemsKey != null && itemsValue != null)
            {
                itemsDictionary.Add(itemsKey, itemsValue);
            }
            mockHttpContext.Setup(c => c.Items).Returns(itemsDictionary);
        }

        protected HttpServerUtilityBase serverUtility
        {
            get
            {
                return mockServer.Object;
            }
        }

        protected IDataFileServiceWrapper dataServiceWrapper
        {
            get
            {
                return mockDataServiceWrapper.Object;
            }
        }
        protected IIconDatabaseServiceWrapper loggingServiceWrapper
        {
            get
            {
                return mockIconLoggingServiceWrapper.Object;
            }
        }

        protected WindowsService fakeServiceA = null;
        protected WindowsService fakeServiceB = null;
        protected ServiceViewModel FakeServiceViewModelA = null;
        protected ServiceViewModel FakeServiceViewModelB = null;

        protected ScheduledTask fakeTaskA = null;
        protected ScheduledTask fakeTaskB = null;
        protected TaskViewModel FakeTaskViewModelA = null;
        protected TaskViewModel FakeTaskViewModelB = null;

        protected List<IApplication> AllFakeApps
        {
            get
            {
                return new List<IApplication>() { fakeServiceA, fakeServiceB, fakeTaskA, fakeTaskB };
            }
        }

        protected List<IconApplicationViewModel> AllFakeAppViewModels
        {
            get
            {
                var list = new List<IconApplicationViewModel>() { FakeServiceViewModelA, FakeServiceViewModelB, FakeTaskViewModelA, FakeTaskViewModelB };
                return list;
            }
        }

        protected void SetupFakeIconApplicationData()
        {
            fakeServiceA = Mock.Of<WindowsService>();
            fakeServiceA.ConfigFilePath = @"\\xxxxxx\uuuuuuu\aaa\bbbbbb\asdfl.xml";
            fakeServiceA.DataFlowFrom = "None";
            fakeServiceA.DataFlowTo = "ESB";
            fakeServiceA.DisplayName = "AAAAAA Service";
            fakeServiceA.Name = "Fake.Name.AAAAAA";
            fakeServiceA.Server = "test-Server1";
            Mock.Get(fakeServiceA).SetupGet(a => a.TypeOfApplication).Returns(ApplicationTypeEnum.WindowsService);

            FakeServiceViewModelA = Mock.Of<ServiceViewModel>();
            FakeServiceViewModelA.ConfigFilePath = @"\\xxxxxx\uuuuuuu\aaa\bbbbbb\asdfl.xml";
            FakeServiceViewModelA.DataFlowFrom = "None";
            FakeServiceViewModelA.DataFlowTo = "ESB";
            FakeServiceViewModelA.DisplayName = "AAAAAA Service";
            FakeServiceViewModelA.Name = "Fake.Name.AAAAAA";
            FakeServiceViewModelA.Server = "test-Server1";
            Mock.Get(FakeServiceViewModelA).SetupGet(a => a.TypeOfApplication).Returns(ApplicationTypeEnum.WindowsService);

            fakeServiceB = Mock.Of<WindowsService>();
            fakeServiceB.ConfigFilePath = @"\\xxxxxx\uuuuuuu\aaa\bbbbbb\asdfl.xml";
            fakeServiceB.DataFlowFrom = "None";
            fakeServiceB.DataFlowTo = "Unknown";
            fakeServiceB.DisplayName = "BBBB Service";
            fakeServiceB.Name = "Fake.Name.BBBBB";
            fakeServiceB.Server = "test-Server1";
            Mock.Get(fakeServiceB).SetupGet(a => a.TypeOfApplication).Returns(ApplicationTypeEnum.WindowsService);

            FakeServiceViewModelB = Mock.Of<ServiceViewModel>();
            FakeServiceViewModelB.ConfigFilePath = @"\\xxxxxx\uuuuuuu\aaa\bbbbbb\asdfl.xml";
            FakeServiceViewModelB.DataFlowFrom = "None";
            FakeServiceViewModelB.DataFlowTo = "Unknown";
            FakeServiceViewModelB.DisplayName = "BBBB Service";
            FakeServiceViewModelB.Name = "Fake.Name.BBBBB";
            FakeServiceViewModelB.Server = "test-Server1";
            Mock.Get(FakeServiceViewModelB).SetupGet(a => a.TypeOfApplication).Returns(ApplicationTypeEnum.WindowsService);

            fakeTaskA = Mock.Of<ScheduledTask>();
            fakeTaskA.ConfigFilePath = @"\\xxxxxx\uuuuuuu\aaa\bbbbbb\asdfl.xml";
            fakeTaskA.DataFlowFrom = "None";
            fakeTaskA.DataFlowTo = "Other";
            fakeTaskA.DisplayName = "AAAAAA Task";
            fakeTaskA.Environment = EnvironmentEnum.Dev;
            fakeTaskA.Name = "Fake.Name.AAAAAA";
            fakeTaskA.Server = "test-Server1";
            Mock.Get(fakeTaskA).SetupGet(a => a.TypeOfApplication).Returns(ApplicationTypeEnum.ScheduledTask);

            FakeTaskViewModelA = Mock.Of<TaskViewModel>();
            FakeTaskViewModelA.ConfigFilePath = @"\\xxxxxx\uuuuuuu\aaa\bbbbbb\asdfl.xml";
            FakeTaskViewModelA.DataFlowFrom = "None";
            FakeTaskViewModelA.DataFlowTo = "Unknown";
            FakeTaskViewModelA.DisplayName = "AAAAAA Service";
            FakeTaskViewModelA.Name = "Fake.Name.AAAAAA";
            FakeTaskViewModelA.Server = "test-Server1";
            Mock.Get(FakeTaskViewModelA).SetupGet(a => a.TypeOfApplication).Returns(ApplicationTypeEnum.WindowsService);

            fakeTaskB = Mock.Of<ScheduledTask>();
            fakeTaskB.ConfigFilePath = @"\\xxxxxx\uuuuuuu\aaa\bbbbbb\asdfl.xml";
            fakeTaskB.DataFlowFrom = "None";
            fakeTaskB.DataFlowTo = "FTP";
            fakeTaskB.DisplayName = "BBBB Task";
            fakeTaskB.Environment = EnvironmentEnum.Dev;
            fakeTaskB.Name = "Fake.Name.BBBBB";
            fakeTaskB.Server = "test-Server1";
            Mock.Get(fakeTaskB).SetupGet(a => a.TypeOfApplication).Returns(ApplicationTypeEnum.ScheduledTask);

            FakeTaskViewModelB = Mock.Of<TaskViewModel>();
            FakeTaskViewModelB.ConfigFilePath = @"\\xxxxxx\uuuuuuu\aaa\bbbbbb\asdfl.xml";
            FakeTaskViewModelB.DataFlowFrom = "None";
            FakeTaskViewModelB.DataFlowTo = "None";
            FakeTaskViewModelB.DisplayName = "BBBB Task";
            FakeTaskViewModelB.Name = "Fake.Name.BBBBB";
            FakeTaskViewModelB.Server = "test-Server1";
            Mock.Get(FakeTaskViewModelB).SetupGet(a => a.TypeOfApplication).Returns(ApplicationTypeEnum.ScheduledTask);
        }

        protected IApp GetFakeApiControllerApp(int id = 99, string name = "ICON Test ApiController App")
        {
            var fakeApp = Mock.Of<IApp>();
            fakeApp.AppID = id;
            fakeApp.AppName = name;
            return fakeApp;
        }

        protected IApp FakeAppA = null;
        protected IApp FakeAppB = null;
        protected IApp FakeAppC = null;
        protected IAppLog FakeAppLogA1 = null;
        protected IAppLog FakeAppLogB1 = null;
        protected IAppLog FakeAppLogB2 = null;
        protected IAppLog FakeAppLogB3 = null;
        protected IAppLog FakeAppLogC1 = null;
        protected IconAppLogViewModel FakeIconAppLogViewModelA1 = null;
        protected IconAppLogViewModel FakeIconAppLogViewModelB1 = null;
        protected IconAppLogViewModel FakeIconAppLogViewModelB2 = null;
        protected IconAppLogViewModel FakeIconAppLogViewModelB3 = null;
        protected IconAppLogViewModel FakeIconAppLogViewModelC1 = null;

        protected void SetupFakeIconAppLogData()
        {
            FakeAppA = Mock.Of<IApp>();
            FakeAppA.AppID = 5;
            FakeAppA.AppName = "API Controller";

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

            FakeIconAppLogViewModelA1 = Mock.Of<IconAppLogViewModel>();
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

            FakeAppB = Mock.Of<IApp>();
            FakeAppB.AppID = 2;
            FakeAppB.AppName = "ESB Listener";

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

            FakeIconAppLogViewModelB1 = Mock.Of<IconAppLogViewModel>();
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

            FakeIconAppLogViewModelB2 = Mock.Of<IconAppLogViewModel>();
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

            FakeIconAppLogViewModelB3 = Mock.Of<IconAppLogViewModel>();
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

            FakeAppC = Mock.Of<IApp>();
            FakeAppC.AppID = 11;
            FakeAppC.AppName = "TLog Controller";

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

            FakeIconAppLogViewModelC1 = Mock.Of<IconAppLogViewModel>();
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

        protected List<IAppLog> AllFakeIconAppLogs
        {
            get
            {
                return new List<IAppLog>() { FakeAppLogA1, FakeAppLogB1, FakeAppLogB2, FakeAppLogB3, FakeAppLogC1 };
            }
        }

        protected List<IconAppLogViewModel> AllFakeIconAppLogViewModels
        {
            get
            {
                var list = new List<IconAppLogViewModel>()
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

        protected IMessageType MessageTypeLocale = Mock.Of<IMessageType>(); //new MessageType() { MessageTypeId = 11, MessageTypeName = IconApiControllerMessageType.Locale };
        protected IMessageType MessageTypeHierarchy = Mock.Of<IMessageType>();//new MessageType() { MessageTypeId = 22, MessageTypeName = IconApiControllerMessageType.Hierarchy };
        protected IMessageType MessageTypeItemLocale = Mock.Of<IMessageType>();//new MessageType() { MessageTypeId = 33, MessageTypeName = IconApiControllerMessageType.ItemLocale };
        protected IMessageType MessageTypePrice = Mock.Of<IMessageType>();// new MessageType() { MessageTypeId = 44, MessageTypeName = IconApiControllerMessageType.Price };
        protected IMessageType MessageTypeDepartmentSale = Mock.Of<IMessageType>();//new MessageType() { MessageTypeId = 55, MessageTypeName = IconApiControllerMessageType.DepartmentSale };
        protected IMessageType MessageTypeProduct = Mock.Of<IMessageType>();//new MessageType() { MessageTypeId = 66, MessageTypeName = IconApiControllerMessageType.Product };
        protected IMessageType MessageTypeCCHTaxUpdate = Mock.Of<IMessageType>();//new MessageType() { MessageTypeId = 77, MessageTypeName = IconApiControllerMessageType.CCHTaxUpdate };
        protected IMessageType MessageTypeProductSelectionGroup = Mock.Of<IMessageType>();//new MessageType() { MessageTypeId = 88, MessageTypeName = IconApiControllerMessageType.ProductSelectionGroup };
        protected IMessageType MessageTypeeWIC = Mock.Of<IMessageType>();//new MessageType() { MessageTypeId = 99, MessageTypeName = IconApiControllerMessageType.eWIC };
        protected IMessageType MessageTypeInforNewItem = Mock.Of<IMessageType>();//new MessageType() { MessageTypeId = 101, MessageTypeName = IconApiControllerMessageType.InforNewItem };

        protected IAPIMessageMonitorLog FakeApiMessageMonitorLogA1 = Mock.Of<IAPIMessageMonitorLog>();
        protected IAPIMessageMonitorLog FakeApiMessageMonitorLogB1 = Mock.Of<IAPIMessageMonitorLog>();
        protected IAPIMessageMonitorLog FakeApiMessageMonitorLogB2 = Mock.Of<IAPIMessageMonitorLog>();
        protected IAPIMessageMonitorLog FakeApiMessageMonitorLogB3 = Mock.Of<IAPIMessageMonitorLog>();
        protected IAPIMessageMonitorLog FakeApiMessageMonitorLogC1 = Mock.Of<IAPIMessageMonitorLog>();
        protected ApiMessageJobSummaryViewModel FakeJobSummaryViewModelA1 = null;
        protected ApiMessageJobSummaryViewModel FakeJobSummaryViewModelB1 = null;
        protected ApiMessageJobSummaryViewModel FakeJobSummaryViewModelB2 = null;
        protected ApiMessageJobSummaryViewModel FakeJobSummaryViewModelB3 = null;
        protected ApiMessageJobSummaryViewModel FakeJobSummaryViewModelC1 = null;


        protected void SetupFakeApiJobSummaryData()
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

        protected List<IAPIMessageMonitorLog> AllFakeApiMessageMonitorLogs
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

        protected List<ApiMessageJobSummaryViewModel> AllFakeJobSummaryViewModels
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
    }
}
