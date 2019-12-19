using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OOS.Model.UnitTests;
using StructureMap;

namespace OOS.Model.IntegrationTests
{
    [TestFixture]
    public class OffShelfUploadRepositoryIntegrationTests
    {
        private IOffShelfUploadRepository repo;

        [SetUp]
        public void Setup()
        {
            Bootstrapper.Bootstrap();
            repo = CreateObjectUnderTest();
        }

        [Test]
        public void FindAll()
        {
            var uploads = repo.FindAll();
            Assert.Greater(uploads.Count(), 0);
            Assert.Greater(uploads.ElementAt(uploads.Count() - 1).Count, 0);
        }

        private IOffShelfUploadRepository CreateObjectUnderTest()
        {
            return ObjectFactory.GetInstance<IOffShelfUploadRepository>();
        }

        [Test]
        public void FindUploadScans()
        {
            var upload = repo.Find("2012-02-29 15:21:05.187");
            Assert.IsNotNull(upload);
            Assert.Greater(upload.Count, 0);
        }


        [Test]
        public void FindUploadsBetween()
        {
            var uploads = repo.FindBetween("2012-02-17 15:21:05.187", "2012-02-29 15:21:05.187");
            Assert.IsNotNull(uploads);
            Assert.Greater(uploads.Count(), 0);
        }

        [Test]
        public void FindUploadsBetweenSameDateTime()
        {
            var uploads = repo.FindBetween("2012-02-29 15:21:05.187", "2012-02-29 15:21:05.187");
            Assert.IsNotNull(uploads);
            Assert.AreEqual(1, uploads.Count());
        }
    }
}
