using System;
using System.Linq;
using Magnum.TestFramework;
using NUnit.Framework;
using OOSCommon.DataContext;
using OOSCommon.Import;
using Rhino.Mocks;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public class KnownUploadRepositoryTests
    {
        private DateTime date;
        private KnownUploadRepository sut;
        private const string TestDate = "12/28/2012";

        [SetUp]
        public void Setup()
        {
            date = Convert.ToDateTime(TestDate);
            sut = CreateObjectUnderTest();
        }

        [Test]
        public void Create()
        {
            Assert.IsNotNull(sut);
        }

        private KnownUploadRepository CreateObjectUnderTest()
        {
            var entityFactory = MockRepository.GenerateStub<ICreateDisposableEntities>();
            var oosEntities = new DisposableMockOOSEntities();
            entityFactory.Stub(p => p.New()).Return(oosEntities).Repeat.Any();
            return new KnownUploadRepository(entityFactory);
        }

        [Test]
        public void Given_a_known_upload_when_the_upload_is_inserted_it_can_be_found()
        {
            var upload = CreateNewUpload();
            sut.Insert(upload);
            var found = sut.For(date);
            
            Assert.AreEqual(upload, found);
        }

        private KnownUpload CreateNewUpload()
        {
            return new KnownUpload(date);
        }

        [Test]
        public void Given_a_non_existing_upload_when_the_upload_is_searched_the_lookup_fails()
        {
            var lookup = sut.For(date);
            Assert.IsNull(lookup);
        }

        [Test]
        public void Given_an_existing_upload_when_another_same_upload_is_inserted_it_is_not_added_again()
        {
            var upload = CreateNewUpload();
            sut.Insert(upload);
            var searched = sut.For(upload.UploadDate);
            Assert.AreEqual(searched, upload);
            sut.Insert(upload);
            
            var next = sut.For(upload.UploadDate);
            Assert.AreEqual(next, searched);
        }

        [Test]
        public void Given_an_existing_upload_when_the_upload_is_updated_the_upload_is_updated()
        {
            var upload = CreateNewUpload();
            sut.Insert(upload);
            var updated = new KnownUpload(date);
            updated.AddItem(new OOSKnownItemData("upc_code", "2", "2012-12-31 10:59:38.713", "vin_number"));
            sut.Modify(updated);

            var searched = sut.For(date);
            Assert.IsTrue(searched.ItemData.ToList().SequenceEqual(updated.ItemData));
        }

        [Test]
        public void Given_a_non_existing_upload_when_modify_is_called_on_the_upload_the_system_does_nothing()
        {
            var upload = CreateNewUpload();
            sut.Modify(upload);
            var searched = sut.For(date);
            Assert.IsNull(searched);
        }

        [Test]
        public void Given_an_existing_upload_when_the_upload_is_removed_the_upload_is_not_found()
        {
            var upload = CreateNewUpload();
            sut.Insert(upload);

            sut.Remove(upload.UploadDate);
            Assert.IsNull(sut.For(upload.UploadDate));
        }

        [Test]
        public void When_product_status_and_expiration_date_is_included_in_the_item_data()
        {
            var upload = CreateNewUpload();
            var productStatus = "Going to expire soon";
            var expirationDate = Convert.ToDateTime("01/28/2013");
            upload.AddItem(new OOSKnownItemData("upc_code", "2", "2012-12-31 10:59:38.713", "vin_number", productStatus, expirationDate));
            sut.Insert(upload);

            var searched = sut.For(date);
            searched.ShouldNotBeNull();
            searched.ItemData[0].ProductStatus.ShouldBeEqualTo("Going to expire soon");
        }
    }
}
