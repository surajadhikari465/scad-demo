using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace OOSCommon.UnitTests
{
    [TestFixture]
    public class MovementStellaUpcSpecificationTests
    {
        [Test]
        public void IsNonStellaUpc()
        {
            var upc = "0002418200117";
            var valid = MovementStellaUpcSpecification.IsSatifiedBy(upc);
            Assert.IsFalse(valid);
        }

        [Test]
        public void IsStellaUpc()
        {
            var upc = "078616202312";
            var valid = MovementStellaUpcSpecification.IsSatifiedBy(upc);
            Assert.IsTrue(valid);
        }

        [Test]
        public void FormValidStellaUpc()
        {
            var upcs = new List<string> { "0002418200117", "0078616202312" };
            upcs = MovementStellaUpcSpecification.TranslateUpc(upcs);
            Assert.IsTrue(new List<string> { "002418200117", "078616202312" }.SequenceEqual(upcs));
        }
    }
}
