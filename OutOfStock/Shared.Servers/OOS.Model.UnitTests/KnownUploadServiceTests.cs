using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OOSCommon.Import;
using Rhino.Mocks;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public class KnownUploadServiceTests
    {
        private KnownUploadService sut;
        private IOOSUpdateKnown knownUploader;
        private ICreateKnownUploader knownUploaderFactory;


        [SetUp]
        public void Setup()
        {
            sut = CreateObjectUnderTest();
        }

        private KnownUploadService CreateObjectUnderTest()
        {
            knownUploaderFactory = MockRepository.GenerateMock<ICreateKnownUploader>();
            knownUploader = MockRepository.GenerateMock<IOOSUpdateKnown>();
           
            knownUploaderFactory.Expect(p => p.Make()).Return(knownUploader).Repeat.Once();          
            return new KnownUploadService(knownUploaderFactory);
        }

        [Test]
        public void Create()
        {
            Assert.IsNotNull(sut);
        }

        [Test]
        public void Given_Known_Upload_When_Upload_Is_Called_Then_Uploader_Factory_Make_Should_Be_Called()
        {
            var uploadDoc = new KnownUpload(DateTime.Now);
            knownUploader.Expect(p => p.Upload(uploadDoc)).Return(true).Repeat.Once();

            sut.Upload(uploadDoc);

            knownUploaderFactory.VerifyAllExpectations();
            knownUploader.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException(typeof(KnownUploadValidationException))]
        public void Given_Known_Upload_With_Null_Item_Data_When_Upload_Is_Called_Then_Validation_Fails()
        {
            var uploadDoc = new KnownUpload(DateTime.Now);
            sut.Upload(uploadDoc);
        }

        [Test]
        [ExpectedException(typeof(KnownUploadValidationException))]
        public void Given_Known_Upload_With_Null_Region_Vendor_Map_When_Upload_Is_Called_Then_Validation_Fails()
        {
            var uploadDoc = new KnownUpload(DateTime.Now);
            sut.Upload(uploadDoc);
        }
    }
}
