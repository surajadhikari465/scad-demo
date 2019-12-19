using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NUnit.Framework;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public class SageStoreFeedStringDecoderTests
    {
        [Test]
        public void DecodeNameWithSpaceFromSageStoreFeed()
        {
            var name = "Annapolis+Culinary+Center";
            var result = HttpUtility.UrlDecode(name);
            var expected = "Annapolis Culinary Center";
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void DecodeNameWithEscapesFromSageStoreFeed()
        {
            var name = "Alpharetta+Harry%27s";
            var expected = "Alpharetta Harry's";
            var result = HttpUtility.UrlDecode(name);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void DecodeNameWithNoEscapes()
        {
            var name = "Alpharetta Harry's";
            var expected = "Alpharetta Harry's";
            var result = HttpUtility.UrlDecode(name);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void DecodeHours()
        {
            var hours = "8%3A00+a.m.+to+10%3A00+p.m.+seven+days+a+week";
            var expected = "8:00 a.m. to 10:00 p.m. seven days a week";
            var result = HttpUtility.UrlDecode(hours);
            Assert.AreEqual(expected, result);
        }

    }
}
