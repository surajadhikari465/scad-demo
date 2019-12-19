using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public class OOSCountSummaryTests
    {
        [Test]
        public void TestOOCountSummaryDoesNotAddDuplicates()
        {
            var summary = new OOSCountSummary();
            summary.Add("store", "team", 10);
            summary.Add("store", "team", 10);
            summary.Add("store", "team", 10);
            summary.Add("apple", "grape", 20);
            Assert.AreEqual(2, summary.Count());
        }

        [Test]
        public void TestOOSCountSummaryOnlyRemovesItemOnce()
        {
            var summary = new OOSCountSummary();
            summary.Add("store", "team", 10);
            summary.Add("apple", "grape", 20);
            summary.Remove("apple", "grape");
            summary.Remove("apple", "grape");

            Assert.AreEqual(1, summary.Count());
        }

        [Test]
        public void TestOOSCountSummaryWithNulls()
        {
            var summary = new OOSCountSummary();
            summary.Add(null, null, 10);
            Assert.AreEqual(1, summary.Count());
            summary.Add(null, null, 100);
            Assert.AreEqual(1, summary.Count());
            summary.Remove(null, null);
            Assert.AreEqual(0, summary.Count());
        }


    }
}
