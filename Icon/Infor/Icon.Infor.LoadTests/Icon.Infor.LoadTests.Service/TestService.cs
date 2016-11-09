using Icon.Infor.LoadTests.ApiControllerTests;
using Icon.Infor.LoadTests.Constants;
using Icon.Infor.LoadTests.GloCon;
using Icon.Infor.LoadTests.InforItemListener;
using Icon.Infor.LoadTests.ReCon;
using Icon.Infor.LoadTests.Service.Models;
using Icon.Logging;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Icon.Infor.LoadTests.Service
{
    public class TestService
    {
        #region Static Members

        private const string timeFormat = "d':'hh':'mm':'ss";

        private static readonly ILoadTestConfiguration defaultConfig = new LoadTestConfiguration
        {
            ApplicationInstances = new List<ApplicationInstance>
            {
                new ApplicationInstance { Server = ApplicationServers.IconServer2 }
            }
        };

        private static Lazy<TestService> instance = new Lazy<TestService>(
            () => new TestService());

        public static TestService Instance
        {
            get { return instance.Value; }
        }

        #endregion Static Members

        #region Private Fields

        private ILogger logger;
        private List<ILoadTest> Tests = new List<ILoadTest>
        {
            new ReConItemLoadTest { Name = LoadTestNames.ReConTest, Configuration = defaultConfig }
        };

        #endregion

        #region Ctor

        private TestService()
        {
            this.logger = new NLogLogger(this.GetType());
        }

        #endregion 

        #region Public Methods

        public List<LoadTestModel> GetAllTests()
        {
            try
            {
                logger.Info(new { Message = "Getting all tests." }.Dump());

                var tests = this.Tests.Select(t => new LoadTestModel
                {
                    Name = t.Name,
                    Status = t.IsRunning ? "Running" : "Idle",
                    ElapsedTime = t.IsRunning
                        ? TimeSpan.FromSeconds(t.GetStatus().ElapsedTime).ToString(timeFormat)
                        : TimeSpan.FromMilliseconds(0).ToString(timeFormat),
                }).ToList();

                logger.Info(new { Message = "Returning tests.", Tests = tests }.Dump());
                return tests;
            }
            catch (Exception ex)
            {
                logger.Error(
                    new
                    {
                        Message = "Unexpected exception occurred when getting all tests.",
                        Exception = ex.ToString()
                    }.Dump());
                throw;
            }
        }

        public LoadTestModel GetTestByName(string name)
        {
            try
            {
                logger.Info(new { Message = "Getting test", TestName = name }.Dump());

                var loadTest = this.Tests.FirstOrDefault(t => t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

                var loadTestModel = new LoadTestModel
                {
                    Name = loadTest.Name,
                    Status = loadTest.IsRunning ? "Running" : "Idle",
                    ElapsedTime = loadTest.IsRunning
                        ? TimeSpan.FromSeconds(loadTest.GetStatus().ElapsedTime).ToString(timeFormat)
                        : TimeSpan.FromMilliseconds(0).ToString(timeFormat),
                    EntityCount = loadTest.IsRunning ? loadTest?.Configuration?.EntityCount ?? 0 : 0,
                    TestRunTime = loadTest.IsRunning
                        ? loadTest?.Configuration?.TestRunTime.ToString(timeFormat)
                        : TimeSpan.FromMilliseconds(0).ToString(timeFormat),
                    PopulateTestDataInterval = loadTest.IsRunning
                        ? loadTest?.Configuration?.PopulateTestDataInterval.ToString(timeFormat)
                        : TimeSpan.FromMilliseconds(0).ToString(timeFormat),
                    EmailRecipients = loadTest.IsRunning
                        ? string.Join(";", loadTest?.Configuration?.EmailRecipients)
                        : string.Empty
                };

                logger.Info(new { Message = "Returning test", TestName = name }.Dump());
                return loadTestModel;
            }
            catch(Exception ex)
            {
                logger.Error(new { Message = "Could not get test", TestName = name, Exception = ex.ToString() }.Dump());
                throw;
            }
        }

        public bool StartTest(LoadTestModel loadTestModel)
        {
            try
            {
                logger.Info(new { Message = "Starting test", TestName = loadTestModel.Name }.Dump());

                var loadTest = this.Tests.FirstOrDefault(t => t.Name.Equals(loadTestModel.Name, StringComparison.InvariantCultureIgnoreCase));
                if (loadTest == null)
                {
                    logger.Error(new { Message = "Unable to start test. Could not find test. ", TestName = loadTestModel.Name }.Dump());
                    return false;
                }

                loadTest.Configuration = new LoadTestConfiguration
                {
                    EntityCount = loadTestModel.EntityCount,
                    ApplicationInstances = new List<ApplicationInstance>
                    {
                        new ApplicationInstance { Server = ApplicationServers.IconServer2 }
                    },
                    EmailRecipients = loadTestModel.EmailRecipients?.Split(';').ToList(),
                    TestRunTime = string.IsNullOrWhiteSpace(loadTestModel.TestRunTime)
                        ? TimeSpan.FromMilliseconds(0)
                        : TimeSpan.Parse(loadTestModel.TestRunTime),
                    PopulateTestDataInterval = string.IsNullOrWhiteSpace(loadTestModel.PopulateTestDataInterval)
                        ? TimeSpan.FromMilliseconds(0)
                        : TimeSpan.Parse(loadTestModel.PopulateTestDataInterval)
                };

                loadTest.Run();

                logger.Info(new { Message = "Successfully started test", TestName = loadTestModel.Name }.Dump());
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(
                    new
                    {
                        Message = "Unexpected exception occurred when starting test",
                        TestName = loadTestModel.Name,
                        Exception = ex.ToString()
                    }.Dump());

                return false;
            }
        }

        public bool StopTest(string testName)
        {
            try
            {
                logger.Info(new { Message = "Stopping test", TestName = testName }.Dump());

                var loadTest = this.Tests.FirstOrDefault(t => t.Name.Equals(testName, StringComparison.InvariantCultureIgnoreCase));
                if (loadTest == null)
                {
                    logger.Error(new { Message = "Unable to start test. Could not find test. ", TestName = testName }.Dump());
                    return false;
                }

                loadTest.Stop();

                logger.Info(new { Message = "Successfully stopped test", TestName = testName }.Dump());
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(
                    new
                    {
                        Message = "Unexpected exception occurred when starting test",
                        TestName = testName,
                        Exception = ex.ToString()
                    }.Dump());

                return false;
            }
        }

        #endregion
    }
}
