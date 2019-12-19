using System;
using System.Linq;
using Magnum.TestFramework;
using NUnit.Framework;
using OOS.Model.UnitTests;
using OOSCommon.DataContext;
using OOSCommon.Import;

namespace OOS.Model.IntegrationTests
{
    [TestFixture]
    public class KnownUploadRepositoryIntegrationTests
    {
        private KnownUploadRepository sut;
        private DateTime date;
        private KnownUpload upload;
        private const string TestDate = "12/28/2012";

        [SetUp]
        public void Setup()
        {
            date = Convert.ToDateTime(TestDate);
            sut = CreateObjectUnderTest();
            
            Given_an_upload();
        }

        private void Given_an_upload()
        {
            upload = CreateNewUpload();
        }

        [Test]
        public void Create()
        {
            Assert.IsNotNull(sut);
        }

        private KnownUploadRepository CreateObjectUnderTest()
        {
            var entityFactory = new EntityFactory(MockConfigurator.New());
            return new KnownUploadRepository(entityFactory);
        }

        [Test]
        public void Given_a_known_upload_when_the_upload_is_inserted_it_can_be_found()
        {
            sut.Insert(upload);
            var found = sut.For(date);

            Assert.AreEqual(upload, found);
        }

        private KnownUpload CreateNewUpload()
        {
            var knownUpload = new KnownUpload(date);
            knownUpload.AddItem(new OOSKnownItemData("upc_001", "3", date.ToShortDateString(), "999"));
            knownUpload.AddVendorRegion(new OOSKnownVendorRegionMap("vendor_region", "NC", "vendor_key"));
            return knownUpload;
        }

        [Test]
        public void Given_a_known_upload_when_the_upload_is_inserted_and_searched_the_values_of_searched_upload_are_equal()
        {
            var searched = sut.For(date);
            Assert.IsTrue((KnownUpload)searched == upload);
        }

        [Test]
        public void Given_an_existing_known_upload_when_the_upload_is_removed_the_upload_is_not_found()
        {
            sut.Remove(date);

            var searched = sut.For(date);
            Assert.IsNull(searched);
        }
        
        [Test]
        public void Given_an_existing_known_upload_when_the_upload_is_updated_the_known_upload_is_updated()
        {
            sut.Remove(date);
            sut.Insert(upload);
            
            upload.AddItem(new OOSKnownItemData("new_upc", "2", "01/02/2012", "111"));
            sut.Modify(upload);

            sut.Reset();
            var searched = sut.For(date);
            Assert.AreEqual(2, searched.ItemData.Count());
        }

        [Test]
        public void Given_an_existing_upload_when_the_upload_is_inserted_again_it_is_not_added_again()
        {
            sut.Insert(upload);
            sut.Insert(upload);
            var searched = sut.For(date);
            upload.ShouldEqual(searched);
        }

        [Test]
        public void Given_a_non_existing_upload_when_the_upload_is_modified_it_does_nothing()
        {
            var tempUpload = new KnownUpload(DateTime.Now);
            sut.Modify(tempUpload);
            sut.For(tempUpload.UploadDate).ShouldBeNull();
        }

        [Test]
        public void When_product_status_exist()
        {
            var status = new ProductStatus("RM", "PLW36890", "abc", "0000241820007");
            sut.ExistProductStatusProjection(status).ShouldEqual(true);
        }

        [Test]
        public void When_product_status_does_not_exist()
        {
            var status = new ProductStatus("NC", "192028", "98426", "0009948243471");
            sut.ExistProductStatusProjection(status).ShouldEqual(false);
        }
    }
}
