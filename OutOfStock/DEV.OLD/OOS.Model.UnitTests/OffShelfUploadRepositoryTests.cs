using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OOSCommon.DataContext;
using Rhino.Mocks;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public class OffShelfUploadRepositoryTests
    {
        private OffShelfUploadRepository sut;

        [SetUp]
        public void Setup()
        {
            sut = CreateObjectUnderTest();
            sut.Add(NewUpload());
        }

        private OffShelfUpload NewUpload()
        {
            var scanDate = "2012-08-12 19:43:53.620";
            return new OffShelfUpload(scanDate);
        }


        private OffShelfUploadRepository CreateObjectUnderTest()
        {
            var entityFactory = MockRepository.GenerateStub<IOOSEntitiesFactory>();
            var oosEntities = new DisposableMockOOSEntities();
            entityFactory.Stub(p => p.New()).Return(oosEntities).Repeat.Any();
            return new OffShelfUploadRepository(entityFactory);
        }

        [Test]
        public void FindOneAlreadyThereGivenDateString()
        {
            var scanDate = "2012-08-12 19:43:53.620";
            var upload = sut.Find(scanDate);
            Assert.AreEqual(Convert.ToDateTime(scanDate), upload.ScanDate);
        }

        [Test]
        public void FindOneAlreadyThereGivenDateTime()
        {
            var scanDate = "2012-08-12 19:43:53.620";
            var upload = sut.Find(Convert.ToDateTime(scanDate));
            Assert.AreEqual(Convert.ToDateTime(scanDate), upload.ScanDate);
        }

        [Test]
        public void AddAnotherAlreadyPresentDoesNothing()
        {
            sut.Add(new OffShelfUpload("2012-08-12 19:43:53.620"));
            Assert.AreEqual(1, sut.Count);
        }

        [Test]
        public void FindAll()
        {
            var scans = sut.FindAll();
            Assert.Greater(scans.Count(), 0);
        }

        [Test]
        public void FindDateRange()
        {
            var scans = sut.FindBetween("2012-08-10", "2012-08-14");
            Assert.AreEqual(1, scans.Count());
            Assert.AreEqual(scans.First().ScanDate, Convert.ToDateTime("2012-08-12 19:43:53.620"));
        }
    }
}
