using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OOSCommon;
using SharedKernel;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public class ValidUPCTests
    {
        [Test]
        public void TestNonNumericUPCNotAllowed()
        {
            var upc = "abc123";
            Assert.IsFalse(UpcSpecification.IsSatisfiedBy(upc));
            var result = UpcSpecification.GetUpcCheckValue(upc);
            Assert.IsTrue(result == Utility.eUPCCheck.NotNumeric);
        }

        [Test]
        public void TestEmptyUPCNotAllowed()
        {
            var upc = string.Empty;
            Assert.IsFalse(UpcSpecification.IsSatisfiedBy(upc));
            var result = UpcSpecification.GetUpcCheckValue(upc);
            Assert.AreEqual(Utility.eUPCCheck.Empty, result);
        }

        [Test]
        public void TestLongUPCNotAllowed()
        {
            var upc = "01234567891234";
            Assert.IsFalse(UpcSpecification.IsSatisfiedBy(upc));
            var result = UpcSpecification.GetUpcCheckValue(upc);
            Assert.AreEqual(Utility.eUPCCheck.TooLong, result);
        }

        [Test]
        public void TestShortUPCBetweenSixAndElevenDigitsAllowed()
        {
            var upc = "012345";
            Assert.IsTrue(UpcSpecification.IsSatisfiedBy(upc));
            Assert.AreEqual("0000000012345", UpcSpecification.FormValid(new List<string> { upc }).First());
        }

        [Test]
        public void TestUPCLessThanSixDigitsIsAllowedAsPLU()
        {
            var upc = "01234";
            var result = UpcSpecification.GetUpcCheckValue(upc);
            Assert.IsFalse(UpcSpecification.IsSatisfiedBy(upc));
            Assert.AreEqual(Utility.eUPCCheck.IsPLU, result);
        }


        [Test]
        public void TestThirteenDigitUPCIsVIMUPC()
        {
            var upc = "0123456789123";
            Assert.IsTrue(UpcSpecification.IsSatisfiedBy(upc));
            Assert.AreEqual(upc, UpcSpecification.FormValid(new List<string>{upc}).First());
        }

        [Test]
        public void TestTwelveDigitUPCIsAllowedWithLastDigitAsCheckSum()
        {
            var upc = "012345678912";
            Assert.IsTrue(UpcSpecification.IsSatisfiedBy(upc));
            Assert.AreEqual("0012345678912", UpcSpecification.FormValid(new List<string>{upc}).First());
        }

        [Test]
        public void TestActualTwelveDigitProductWithChecksumNotPresent()
        {
            var upc = "471151800871";
            Assert.IsTrue(UpcSpecification.IsSatisfiedBy(upc));
            Assert.AreEqual("0471151800871", UpcSpecification.FormValid(new List<string>{upc}).First());
        }

        [Test]
        public void TestTwelveProductWithChecksumPresent()
        {
            var upc = "022592007014";
            Assert.IsTrue(UpcSpecification.IsSatisfiedBy(upc));
            Assert.AreEqual("0002259200701", UpcSpecification.FormValid(new List<string>{upc}).First());
        }

        [Test]
        public void TestRandomNoVIMDataUPC()
        {
            var upc = "569052789800";
            Assert.IsTrue(UpcSpecification.IsSatisfiedBy(upc));
            Assert.AreEqual("0569052789800", UpcSpecification.FormValid(new List<string>{upc}).First());
        }

        [Test]
        public void UpcHasChecksum()
        {
            var upc = "0657622119025";
            Assert.IsTrue(CheckumSpecification.IsSatifiedBy(upc));
        }

        [Test]
        [Category("Integration Test")]
        public void DumpChecksumUpcsFromFile()
        {
            Console.WriteLine("DumpChecksumUpcsFromFile()\n");
            var upcs = UploadMessage.From(Properties.Resources.upload).Scans.ToArray();
            WriteCheckDigitSatisfied(upcs);
        }

        private void WriteCheckDigitSatisfied(string[] upcs)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < upcs.Length; i++)
            {
                if (i > 0) builder = builder.Append("\n");
                builder.Append(upcs[i]).Append(' ').Append(CheckumSpecification.IsSatifiedBy(upcs[i]));
            }
            var result = builder.ToString();
            Console.WriteLine(result);
        }

        private string[] TransformCheckDigit(string[] upcs)
        {
            return upcs.Select(p => "0" + p.Substring(0, p.Length - 1)).ToArray();
        }

        [Test]
        [Category("Integration Test")]
        public void DumpTransformedCheckDigitUpcs()
        {
            Console.WriteLine("UPCs w/o checksum\n");
            var upcs = UploadMessage.From(Properties.Resources.upload).Scans.ToArray();
            var transformedUpcs = TransformCheckDigit(upcs);
            WriteTransformed(transformedUpcs);
        }

        private void WriteTransformed(string[] upcs)
        {
            var modifiedUpcsBuilder = new StringBuilder();
            for (var i = 0; i < upcs.Length; i++)
            {
                if (i > 0) modifiedUpcsBuilder.Append("\n");
                modifiedUpcsBuilder.Append(upcs[i]);
            }
            var result = modifiedUpcsBuilder.ToString();
            Console.WriteLine(result);
        }

    }
}
