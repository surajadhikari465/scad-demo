using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OpsgenieAlert;
using Services.Extract.Credentials;
using Services.Extract.Models;


namespace Services.Extract.Tests
{
    [TestClass]
    public class ExtractRunnerTests
    {

        private NLogLogger<ExtractJobRunner> logger;
        private ExtractJobRunner runner;
        private string WorkspacePath = @".\Workspace";
        private IOpsgenieAlert OpsGenieAlert;
        private ICredentialsCacheManager CredentialsCacheManager;
        private IFileDestinationCache FileDestinationCache;


        [TestInitialize]
        public void Init()
        {
            logger = new NLogLogger<ExtractJobRunner>();
            OpsGenieAlert = new OpsgenieAlert.OpsgenieAlert();
            CredentialsCacheManager = new CredentialsCacheManager(new S3CredentialsCache(), new SFtpCredentialsCache(),
                new EsbCredentialsCache());
            FileDestinationCache = new FileDestinationsCache();
            runner = new ExtractJobRunner(logger, OpsGenieAlert, CredentialsCacheManager, FileDestinationCache);



        }

        [TestMethod]
        public void ExtractJobRunner_CreateWorkSpace_ShouldCreateWorkspace()
        {

            const string workspacePath = @".\Workspace";
            runner.CreateWorksapce(workspacePath);

            Assert.IsTrue(Directory.Exists(workspacePath));

        }

        [TestMethod]
        public void ExtractJobRunner_CleanupWorkspace_ShouldRemoveFilesFromWorkspace()
        {

            runner.CleanWorkspace(WorkspacePath);

            TestHelper.CreateTestFile(WorkspacePath);


            var files = Directory.GetFiles(WorkspacePath);
            Assert.AreEqual(1, files.Length);

            runner.CleanWorkspace(WorkspacePath);

            files = Directory.GetFiles(WorkspacePath);
            Assert.AreEqual(0, files.Length);

        }

        [TestMethod]
        public void ExtractJobRunner_CompressFilesZipOutputEqualsFalse_ShouldReturn()
        {
            var config = new ExtractJobConfiguration {ZipOutput = false, OutputFileName = "output.txt"};
            runner.SetConfiguration(config);
            runner.TransformFilenames();

            runner.SetupWorkspace(WorkspacePath);
            var file = TestHelper.CreateTestFile(WorkspacePath);

            var files = new List<FileInfo> {file};

            runner.CompressFiles(files, WorkspacePath + @"\" + config.OutputFileName);

            var outputfile = WorkspacePath + @"\output.zip";
            Assert.IsFalse(File.Exists(outputfile));

        }

        [TestMethod]
        public void ExtractJobRunner_CompressFilesZipOutputEqualsTrue_ShouldZipFile()
        {
            var config = new ExtractJobConfiguration {ZipOutput = true, OutputFileName = "output.txt"};
            runner.SetConfiguration(config);
            runner.TransformFilenames();

            runner.SetupWorkspace(WorkspacePath);
            var file = TestHelper.CreateTestFile(WorkspacePath);

            var files = new List<FileInfo> {file};

            runner.CompressFiles(files, WorkspacePath + @"\" + config.OutputFileName);

            var outputfile = WorkspacePath + @"\output.zip";
            Assert.IsTrue(File.Exists(outputfile));

        }

        [TestMethod]
        public void ExtractJobRunner_CompressFilesGZipOutputEqualsTrue_LargeFile_ShouldGZipFile()
        {
            var config = new ExtractJobConfiguration { ZipOutput = true, CompressionType = "gzip" ,OutputFileName = "test.txt" };
            runner.SetConfiguration(config);
            runner.TransformFilenames();

            runner.SetupWorkspace(WorkspacePath);
            var file = TestHelper.CreateLargeTestFile(WorkspacePath);

            var files = new List<FileInfo> { file };

            runner.CompressFiles(files, WorkspacePath + @"\" + config.OutputFileName);

            var outputfile = WorkspacePath + @"\test.gz";
            Assert.IsTrue(File.Exists(outputfile));

        }



        [TestMethod]
        public void ExtractJobRunner_CallCleanup_ShouldDisposeConnections()
        {
            runner.Connections.Add("test", new SqlConnection(""));

            var extractSourcesAndDestinationFiles = new List<ExtractSourcesAndDestinationFile>
            {
                new ExtractSourcesAndDestinationFile() {DestinationFile = "test", Source = new SqlConnection("")}
            };

            runner.ActiveSourcesAndDestinationFiles = extractSourcesAndDestinationFiles;

            runner.Cleanup();

        }

        [TestMethod]
        public void ExtractJobRunner_DynamicToConcatenatedValuesString_ShouldCovertValuesToString()
        {
            dynamic data = new ExpandoObject();
            data.Test0 = 0;
            data.Test1 = 1;

            var results = runner.DynamicToConcatenatedValuesString(data, ",", false, true);

            Assert.AreEqual("0,1", results);
        }

        [TestMethod]
        public void ExtractJobRunner_DynamicToConcatenatedValuesString_ShouldRemoveDelimeterFromValue()
        {
            dynamic data = new ExpandoObject();
            data.Test0 = "test|0";
            data.Test1 = "test|1";

            var results = runner.DynamicToConcatenatedValuesString(data, "|", false, true);

            Assert.AreEqual("test0|test1", results);
        }

        [TestMethod]
        public void ExtractJobRunner_DynamicToConcatenatedHeaderString_ShouldCovertHeadersToString()
        {
            dynamic data = new ExpandoObject();
            data.Test0 = 0;
            data.Test1 = 1;

            var results = runner.DynamicToConcatenatedHeaderString(data, ",", false);

            Assert.AreEqual("Test0,Test1", results);
        }

        [TestMethod]
        public void ExtractJobRunner_DynamicToConcatenatedHeaderString_ShouldRemoveDelimeterFromValue()
        {
            dynamic data = new ExpandoObject();
            data.Testxyz0 = "test0";
            data.Testxyz1 = "test1";

            var results = runner.DynamicToConcatenatedHeaderString(data, "xyz", false);

            Assert.AreEqual("Test0xyzTest1", results);
        }

        [TestMethod]
        public void IrmaTest()
        {

            var sw = new Stopwatch();
            sw.Start();
            var config = new ExtractJobConfiguration
            {
                Delimiter = "|",
                Source = "Irma",
                Query = "select top 1 * from Item",
                Regions = "FL,MA".Split(','),
                OutputFileName = "test_{source}_{date:MMddyyyyHHmmss}.txt",
                ZipOutput = true,
                CompressionType = "zip",
                ConcatenateOutputFiles = false,
                IncludeHeaders = true,
                Destination = new Destination
                {
                    Type = "pathkey",
                    PathKey = "CAP",
                    
                }
            };
            runner.Run(config);
            sw.Stop();
        }

        [Ignore("Not a real test. Used to execute runner in debug environment.")]
        [TestMethod]
        public void IrmaTest_RegionalFiles()
        {
            var sw = new Stopwatch();
            sw.Start();

            var config = new ExtractJobConfiguration
            {
                Delimiter = "|",
                Source = "Irma",
                Query = "exec [extract].[APT_FutureCostsExtract] @DeltaLoad ",
                Regions = "FL,MA".Split(','),
                Parameters = new ExtractJobParameter[1]
                {
                    new ExtractJobParameter()
                    {
                        Key = "@DeltaLoad",
                        Value = 0
                    }
                },
                OutputFileName = "AP_{region}_pdx_future_cost_{date:yyyyMMdd}.csv",
                ZipOutput = true,
                CompressionType = "gzip",
                ConcatenateOutputFiles = false,
                IncludeHeaders = false,
                Destination = new Destination
                {
                    Type = "file",
                    Path = @"c:\temp\1\"
                }
            };
            runner.Run(config);
            sw.Stop();
        }

        [Ignore("Not a real test. Used to execute runner in debug environment.")]
        [TestMethod]
        public void IrmaTest_FilesByStores()
        {
            var sw = new Stopwatch();
            sw.Start();

            var config = new ExtractJobConfiguration
            {
                Delimiter = "|",
                Source = "Irma",
                StagingQuery = "Exec [dbo].[UpdateTitle] @TitleID, @TitleDesc ",
                StagingParameters = new[]
                {
                    new ExtractJobParameter() {Key = "@TitleID", Value = 32},
                    new ExtractJobParameter() {Key = "@TitleDesc", Value = "System - Do Not Use"}
                },
                DynamicParameterQuery =
                    "select distinct(STORE_NUMBER) as Value, 'STORE_NUMBER' as [Key] from [dbo].[VendorLaneExtract]",
                Query =
                    "select top 100 * from dbo.VendorLaneExtract where STORE_NUMBER = @STORE_NUMBER AND ITEM_KEY = @ITEM_KEY",
                Parameters = new[] {new ExtractJobParameter() {Key = "@ITEM_KEY", Value = 158800}},
                Regions = "SO".Split(','),
                OutputFileName = "item_vendor_lane_{region}_{STORE_NUMBER}_{date:yyyyMMdd}.csv",
                ZipOutput = true,
                CompressionType = "gzip",
                ConcatenateOutputFiles = false,
                IncludeHeaders = true,
                Destination = new Destination
                {
                    Type = "esb",
                    CredentialsKey = "inStock",
                }
            };
            runner.Run(config);
            sw.Stop();
        }

        [Ignore("Not a real test. Used to execute runner in debug environment.")]
        [TestMethod]
        public void IconTest()
        {
            runner = new ExtractJobRunner(logger, OpsGenieAlert, CredentialsCacheManager, FileDestinationCache);
            var config = new ExtractJobConfiguration
            {
                Delimiter = "|",
                Source = "Icon",
                Query = "select * from hierarchyClass where HierarchyId = @Id",
                Parameters = new[] {new ExtractJobParameter() {Key = "@Id", Value = 5}},
                Regions = new string[] { },
                OutputFileName = "test_{source}.txt",
                ZipOutput = true,
                IncludeHeaders = true,
                ConcatenateOutputFiles = false,
                Destination = new Destination
                {
                    Type = "s3",
                    CredentialsKey = "s3DART",
                    Path = @"inbound/scad/irma"
                }
            };
            runner.Run(config);
        }

        [Ignore("Not a real test. Used to execute runner in debug environment.")]
        [TestMethod]
        public void IrmaUserAuditTest_RegionalFiles()
        {
            var sw = new Stopwatch();
            sw.Start();

            var config = new ExtractJobConfiguration
            {
                Delimiter = ",",
                Source = "Irma",
                Query = "SELECT s.Store_No,s.Store_Name,uo.* FROM users uo" +
                        "    LEFT JOIN(" +
                        "        SELECT u.user_id, ust.Store_No FROM users u" +
                        "        JOIN UserStoreTeamTitle ust ON u.user_id = ust.user_id" +
                        "        GROUP BY u.user_id, ust.Store_No" +
                        "        HAVING count(ust.store_no) >= 1) us ON uo.user_id = us.user_id" +
                        "    LEFT JOIN store s ON us.store_no = s.store_no" +
                        "    ORDER BY s.Store_No; ",
                Regions = "FL".Split(','),
                OutputFileName = "IRMA_UserAudit_{date:yyyyMMdd}.csv",
                ZipOutput = false,
                ConcatenateOutputFiles = false,
                IncludeHeaders = true,
                Destination = new Destination
                {
                    Type = "file",
                    Path = @"c:\temp\1\"
                }
            };
            runner.Run(config);
            sw.Stop();
        }

        [Ignore("Not a real test. Used to execute runner in debug environment.")]
        [TestMethod]
        public void IrmaUserAuditTest_WithOutDelimiter_RegionalFiles()
        {
            var sw = new Stopwatch();
            sw.Start();

            var config = new ExtractJobConfiguration
            {
                Source = "Irma",
                Query = "SELECT s.Store_No,s.Store_Name,uo.* FROM users uo" +
                        "    LEFT JOIN(" +
                        "        SELECT u.user_id, ust.Store_No FROM users u" +
                        "        JOIN UserStoreTeamTitle ust ON u.user_id = ust.user_id" +
                        "        GROUP BY u.user_id, ust.Store_No" +
                        "        HAVING count(ust.store_no) >= 1) us ON uo.user_id = us.user_id" +
                        "    LEFT JOIN store s ON us.store_no = s.store_no" +
                        "    ORDER BY s.Store_No; ",
                Regions = "FL".Split(','),
                OutputFileName = "IRMA_UserAudit_{date:yyyyMMdd}.csv",
                ZipOutput = false,
                ConcatenateOutputFiles = false,
                IncludeHeaders = true,
                Destination = new Destination
                {
                    Type = "file",
                    Path = @"c:\temp\1\"
                }
            };
            runner.Run(config);
            sw.Stop();
        }

        [TestMethod]
        public void TestFromFile()
        {
            var config = JsonConvert.DeserializeObject<ExtractJobConfiguration>(
                File.ReadAllText(@".\ExtractTestConfiguration.json"));
            runner = new ExtractJobRunner(logger, OpsGenieAlert, CredentialsCacheManager, FileDestinationCache);
            runner.Run(config);
        }

        [TestCleanup]
        public void CleanUp()
        {
            runner.Cleanup();
        }
    }
}
