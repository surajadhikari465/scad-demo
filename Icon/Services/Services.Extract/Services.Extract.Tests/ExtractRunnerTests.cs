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



        [TestInitialize]
        public void Init()
        {
            logger = new NLogLogger<ExtractJobRunner>();
            OpsGenieAlert = new OpsgenieAlert.OpsgenieAlert();
            CredentialsCacheManager = new CredentialsCacheManager(new S3CredentialsCache(), new SFtpCredentialsCache());
            runner = new ExtractJobRunner(logger, OpsGenieAlert, CredentialsCacheManager);
            

            
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
            Assert.AreEqual(0,files.Length);

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
            var config = new ExtractJobConfiguration { ZipOutput = true, OutputFileName = "output.txt" };
            runner.SetConfiguration(config);
            runner.TransformFilenames();

            runner.SetupWorkspace(WorkspacePath);
            var file = TestHelper.CreateTestFile(WorkspacePath);

            var files = new List<FileInfo> { file };

            runner.CompressFiles(files, WorkspacePath + @"\" + config.OutputFileName);

            var outputfile = WorkspacePath + @"\output.zip";
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
                    Type = "file",
                    Path = @"c:\temp\1\"
                }
            };
            runner.Run(config);
            sw.Stop();
        }

        //[Ignore("Not a real test. Used to execute runner in debug environment.")]
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

        //[Ignore("Not a real test. Used to execute runner in debug environment.")]
        [TestMethod]
        public void IconTest()
        {
            runner = new ExtractJobRunner(logger, OpsGenieAlert, CredentialsCacheManager);
            var config = new ExtractJobConfiguration
            {
                Delimiter = "|",
                Source = "Icon",
                Query = "select * from hierarchyClass where HierarchyId = @Id",
                Parameters = new[] { new ExtractJobParameter() { Key = "@Id", Value = 5}  },
                Regions = new string[] {  },
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

        [TestMethod]
        public void TestFromFile()
        {
           var config =  JsonConvert.DeserializeObject<ExtractJobConfiguration>(
                File.ReadAllText(@".\ExtractTestConfiguration.json"));
           runner = new ExtractJobRunner(logger, OpsGenieAlert, CredentialsCacheManager);
           runner.Run(config);
        }

        [TestCleanup]
        public void CleanUp()
        {
            runner.Cleanup();
        }


    }
}
