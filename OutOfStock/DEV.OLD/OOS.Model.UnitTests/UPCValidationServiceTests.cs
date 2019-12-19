using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OOS.Model.Repository;
using StructureMap;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public class UPCValidationServiceTests
    {
        [SetUp]
        public void Setup()
        {
            Bootstrapper.Bootstrap();
        }


        [Test]
        [Category("Integration Test")]
        public void TestValidateProductWithGoodThirteenDigitUPC()
        {
            string upc = "0471151800871";
            var service = CreateObjectUnderTest();   
            var valid = service.ValidateProduct(upc);
            Assert.IsTrue(valid);
        }

        private UPCValidationService CreateObjectUnderTest()
        {
            return ObjectFactory.GetInstance<UPCValidationService>();
        }

        [Test]
        [Category("Integration Test")]
        public void TestProductValidationFailedWhenUPCTruncatedCheckDigitLeftPaddedWithZero()
        {
            string upc = "0047115180087";
            var service = CreateObjectUnderTest();
            var valid = service.ValidateProduct(upc);
            Assert.IsFalse(valid);

        }

        [Test]
        [Category("Integration Test")]
        public void TestValidRegionProduct()
        {
            var upc = "0007085900074";
            var service = CreateObjectUnderTest();
            var valid = service.ValidateProduct(upc);
            Assert.IsTrue(valid);
        }

    }
}
