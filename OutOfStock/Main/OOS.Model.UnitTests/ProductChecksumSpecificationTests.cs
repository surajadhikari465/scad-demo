using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OOSCommon;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public class ProductChecksumSpecificationTests
    {
        [Test]
        public void TestProductChecksumNotPresent()
        {
            string upc = "0471151800871";
            Assert.IsFalse(ChecksumPresent(upc));
        }

        private static bool ChecksumPresent(string upc)
        {
            return CheckumSpecification.IsSatifiedBy(upc);
        }

        [Test]
        public void TestProduct1ChecksumNotPresent()
        {
            string upc = "542501039186";
            Assert.IsFalse(ChecksumPresent(upc));
        }

        [Test]
        public void TestProduct2ChecksumNotPresent()
        {
            string upc = "569052789800";
            Assert.IsFalse(ChecksumPresent(upc));
        }

        [Test]
        public void TestTwelveProductWithChecksumPresent()
        {
            string upc = "022592007014";
            Assert.IsTrue(ChecksumPresent(upc));
        }

        [Test]
        public void TestActualTwelveDigitProductWithChecksumNotPresent()
        {
            string upc = "471151800871";
            Assert.IsFalse(ChecksumPresent(upc));
        }

        [Test]
        public void TestActualTwelveDigitProduct1WithChecksumNotPresent()
        {
            string upc = "078087206057";
            Assert.IsFalse(ChecksumPresent(upc));
        }

        [Test]
        public void TestActualTwelveDigitProduct2WithChecksumNotPresent()
        {
            string upc = "078087206058";
            Assert.IsFalse(ChecksumPresent(upc));
        }

        [Test]
        public void TestActualTwelveDigitProduct3WithChecksumPresent()
        {
            string upc = "078087206059";
            Assert.IsTrue(ChecksumPresent(upc));
        }

        [Test]
        [ExpectedException(typeof(InvalidUPCException))]
        public void TestEmptyString()
        {
            string upc = "";
            Assert.IsFalse(ChecksumPresent(upc));
        }


    }
}
