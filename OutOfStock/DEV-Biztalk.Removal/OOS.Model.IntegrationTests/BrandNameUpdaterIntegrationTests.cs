using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OOS.Model.IntegrationTests.Upload;
using OOS.Model.UnitTests;
using StructureMap;

namespace OOS.Model.IntegrationTests
{
    [TestFixture]
    public class BrandNameUpdaterIntegrationTests
    {
        private BrandNameUpdater updater;

        [SetUp]
        public void Setup()
        {
            Bootstrapper.Bootstrap();
            updater = CreateObjectUnderTest();
        }

        private BrandNameUpdater CreateObjectUnderTest()
        {
            return ObjectFactory.GetInstance<BrandNameUpdater>();
        }

        [Test]
        public void UpcsForUploads()
        {
            var upload1 = new OffShelfUpload(DateTime.Now);
            upload1.Scan("upc1");
            upload1.Scan("upc2");
            var upload2 = new OffShelfUpload(DateTime.Now);
            upload2.Scan("upc3");
            var uploads = new List<OffShelfUpload> { upload1, upload2 };
            var upcs = updater.UpcsFrom(uploads);
            upcs.SequenceEqual(new List<string> {"upc1", "upc2", "upc3"});
        }

        [Test]
        public void UpdateBrandName()
        {
            updater.Update();
        }

    }
}
