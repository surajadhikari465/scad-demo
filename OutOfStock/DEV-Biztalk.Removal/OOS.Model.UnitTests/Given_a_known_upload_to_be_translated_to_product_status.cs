using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magnum.TestFramework;
using OOSCommon;
using OOSCommon.Import;
using Rhino.Mocks;

namespace OOS.Model.UnitTests
{
    [Scenario]
    public abstract class Given_a_known_upload_to_be_translated_to_product_status
    {
        protected ITranslateKnownUploadToProductStatusProjections sut;
        protected IEnumerable<ProductStatus> projections;
        protected IKnownUpload upload;

        [Given]
        public void Given()
        {
            sut = CreateSubjectUnderTest();
            upload = MakeKnownUpload();
            When();

            projections = sut.Translate(upload);
        }

        private ITranslateKnownUploadToProductStatusProjections CreateSubjectUnderTest()
        {
            return new KnownUploadToProductStatusProjectionTranslator();
        }


        private IKnownUpload MakeKnownUpload()
        {
            var upload = MockRepository.GenerateMock<IKnownUpload>();
            return upload;
        }

        protected abstract void When();
    }
}
