using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public class OffShelfUploadTests
    {
        [Test]
        public void CreateUpload()
        {
            var report = CreateObjectUnderTest();
            Assert.AreEqual(0, report.Count);
        }

        private OffShelfUpload CreateObjectUnderTest()
        {
            var scanDate = "08/20/2012";
            return new OffShelfUpload(scanDate);
        }

        [Test]
        public void AddScan()
        {
            var upload = CreateObjectUnderTest();
            var upc = "0000000049863";
            upload.Scan(upc);
            Assert.AreEqual(1, upload.Count);
        }

        [Test]
        [ExpectedException(typeof(InvalidUPCException))]
        public void EmptyUpcThrowsException()
        {
            var upload = CreateObjectUnderTest();
            upload.Scan("");
        }

        [Test]
        [ExpectedException(typeof(InvalidUPCException))]
        public void NullUpcThrowsException()
        {
            var upload = CreateObjectUnderTest();
            upload.Scan(null);
        }
    }
}
