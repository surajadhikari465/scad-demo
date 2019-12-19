using System;
using Magnum.TestFramework;
using NUnit.Framework;
using OOS.Model.UnitTests;
using OOSCommon;
using OOSCommon.DataContext;
using OOSCommon.Import;

namespace OOS.Model.IntegrationTests
{
    [TestFixture]
    public abstract class Given_a_known_upload_that_has_product_status_and_expiration
    {
        protected IKnownUploadRepository sut;
        protected IKnownUpload upload;
        protected IKnownUpload searched;
        protected DateTime date = Convert.ToDateTime("01/28/2013");

        protected const string ExpireSoonProductStatus = "Going to expire soon";
        
        [Given]
        public void Given()
        {
            upload = CreateKnownUpload();
            sut = CreateObjectUnderTest();
            sut.Insert(upload);       
            When();
        }

        private IKnownUploadRepository CreateObjectUnderTest()
        {
            var entityFactory = new EntityFactory(MockConfigurator.New());
            return new KnownUploadRepository(entityFactory);
        }

        private IKnownUpload CreateKnownUpload()
        {
            var upload = new KnownUpload(date);
            upload.AddItem(new OOSKnownItemData("upc_code", "2", "2012-12-31 10:59:38.713", "123456", ExpireSoonProductStatus, date));
            upload.AddVendorRegion(new OOSKnownVendorRegionMap("UNF-NC", "NC", "UNF"));
            return upload;
        }

        protected abstract void When();
    }
}
