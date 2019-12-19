//TODO: fix tests
//using System.IO;
//using System.Linq;
//using BulkItemUploadProcessor.Common;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using OfficeOpenXml;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

//namespace BulkItemUploadProcessor.Service.Tests
//{
//    [TestClass]
//    public class ExtensionTests
//    {
//        private ExcelPackage ExcelPackage;
//        private string[] GetHeaderColumsExpectedHeaders;

//        [TestInitialize]
//        public void Init()
//        {
//            ExcelPackage = new ExcelPackage(new FileInfo(@".\TestData\IconExport_SingleItem.xlsx"));
//        }

//        [TestMethod]
//        public void Extensions_GetHeaderColumns_ShouldReturnColumnValuesFromFirstRow()
//        {
//            GetHeaderColumsExpectedHeaders =
//                File.ReadAllLines(@".\TestData\IconExport_SingleItem_ExpectedHeaders.txt");

//            var actualHeaders = ExcelPackage.Workbook.Worksheets["Items"].GetHeaderColumns();


//           // Assert.IsTrue(GetHeaderColumsExpectedHeaders.SequenceEqual(actualHeaders));
//        }

//        [TestMethod]
//        public void Extensions_ParseClassId_ShouldReturnIdAfterLastPipe()
//        {
//            var hierarchy = "Amazon1 | Amazon | Amazon | Amazon | Amazon Prepack:Amazon Prepack (5350) | 5000333";

//            var id = hierarchy.ParseClassId();

//            Assert.AreEqual("5000333", id);
//        }

//        [TestMethod]
//        public void Extensions_ParseClassId_PipeLastCharacter_ShouldReturnEmptyString()
//        {
//            var hierarchy = "Amazon1 | Amazon | Amazon | Amazon | Amazon Prepack:Amazon Prepack (5350) |";

//            var id = hierarchy.ParseClassId();

//            Assert.AreEqual("", id);
//        }

//        [TestMethod]
//        public void Extensions_ParseClassId_NoPipes_ShouldReturnWholeString()
//        {
//            var hierarchy = "Amazon1 (5350)";

//            var id = hierarchy.ParseClassId();

//            Assert.AreEqual("Amazon1 (5350)", id);
//        }

//        [TestMethod]
//        public void Extensions_ParseName_WithPipes_ShouldReturnPartBeforeLastPipe()
//        {
//            var data = "Test | 1234";
//            var parsed = data.ParseName();
//            Assert.AreEqual("Test", parsed);

//        }

//        [TestMethod]
//        public void Extensions_ParseName_WithNoPipes_ShouldReturnWholeString()
//        {
//            var data = "Test1234";
//            var parsed = data.ParseName();
//            Assert.AreEqual("Test1234", parsed);

//        }

//        [TestMethod]
//        public void Extensions_ParseName_WithPipeFirst_ShouldReturnEmptyString()
//        {
//            var data = "|Test1234";
//            var parsed = data.ParseName();
//            Assert.AreEqual("", parsed);

//        }

//        [TestCleanup]
//        public void Cleanup()
//        {
//            ExcelPackage.Dispose();
//        }
//    }
//}