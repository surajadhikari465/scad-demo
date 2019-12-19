using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;

namespace OOS.Model.UnitTests
{

    [TestFixture]
    public class UploadMessageTests
    {
        [Test]
        public void Create()
        {
            var sut = CreateSystemUnderTest();
        }

        private UploadMessage CreateSystemUnderTest()
        {
            return UploadMessage.From(Properties.Resources.upload);
        }

        [Test]
        public void CreateUploadMessage()
        {
            var upload = CreateSystemUnderTest();
            Assert.AreEqual(upload.ScanDate.GetType(), typeof(DateTime), "Scan date type should be DateTime");
            Assert.AreEqual(upload.RegionAbbreviation.GetType(), typeof(string));
            Assert.AreEqual(upload.StoreAbbreviation.GetType(), typeof(string));
            Assert.AreEqual(upload.Scans.GetType(), typeof(string[]));
            Assert.GreaterOrEqual(upload.Scans.Count(), 2);
        }


    }
}
