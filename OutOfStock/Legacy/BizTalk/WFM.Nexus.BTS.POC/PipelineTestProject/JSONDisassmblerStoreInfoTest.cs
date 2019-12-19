using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WFM.Nexus.Pipeline.Json;
using WFM.Nexus.Pipeline.Json.Model;

namespace PipelineTestProject
{
    /// <summary>
    ///This is a test class for JSONDisassmblerStoreInfoTest and is intended
    ///to contain all JSONDisassmblerStoreInfoTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JSONDisassmblerStoreInfoTest
    {
        private string url = "https://api.wholelabs.com/v1/stores.json?limit=2000";
        //private string url = "http://api.wholelabs.com/v1/stores";

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for SerializeXml
        ///</summary>
        [TestMethod()]
        public void SerializeXmlTestHelper()
        {
            Stream originalStream = GetResponseStream(url);

            StoresResult storesResult = StoreInfoDisassembler.DeserializeJSON<StoresResult>(originalStream);

            MemoryStream memStream = StoreInfoDisassembler.SerializeXml<StoresResult>(storesResult);
            memStream.Position = 0;
            StreamReader reader = new StreamReader(memStream);

            string xml = reader.ReadToEnd();
            Assert.IsTrue(xml.Length > 0);
        }



        /// <summary>
        ///A test for DeserializeJSON
        ///</summary>
        [TestMethod()]
        public void DeserializeJSONTest()
        {
            Stream originalStream = GetResponseStream(url);

            StoresResult actual = StoreInfoDisassembler.DeserializeJSON<StoresResult>(originalStream);

            Assert.IsTrue(actual.stores.Count > 0);

            MemoryStream memStream = StoreInfoDisassembler.SerializeXml<StoresResult>(actual);
            string s = Encoding.UTF8.GetString(memStream.ToArray()); 
        }

        [TestMethod()]
        public void TestStream()
        {
            Stream originalStream = GetResponseStream(url);
            StreamReader reader = new StreamReader(originalStream);

            string jsonString = reader.ReadToEnd();
            Assert.IsTrue(jsonString.Length > 0);
        }

        [TestMethod()]
        public void SerializeToJSON()
        {
            //StoresResult result = new StoresResult();
            //result.stores = new StoresResult();
            //result.query = new Query();
            //result.stores.Add(new Store());

            //DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(StoresResult));
            //var ms = new MemoryStream();
            //jsonSerializer.WriteObject(ms, result);
            //string s = Encoding.UTF8.GetString(ms.ToArray()); 


        }


        private static Stream GetResponseStream(string url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse ws = request.GetResponse();
             return ws.GetResponseStream();
        }

    }
}
