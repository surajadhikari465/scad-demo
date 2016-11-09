using Icon.Web.Api.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Api.Tests
{
    [TestClass]
    public class ItemsControllerTests
    {
        private ItemsController sut = new ItemsController();

        [TestMethod]
        public void Post_Should_Return_Items_With_Same_Scan_Codes()
        {
            //When
            var results = sut.GetItemsByScanCodes(new List<string> { "334" });

            //Then
            Assert.AreEqual("334", results[0].ScanCode);
        }
    }
}
