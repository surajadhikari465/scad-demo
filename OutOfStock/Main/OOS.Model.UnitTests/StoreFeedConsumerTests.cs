using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OOS.Model.Feed;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public class StoreFeedConsumerTests
    {
        private const string SERVICE_URL = "http://www.wholefoodsmarket.com/common/irest";

        [Test]
        [Category("Integration Test")]
        public void TestConsumeStoreFeed()
        {
            var consumer = CreateObjectUnderTest();

            var consumed = consumer.Consume();

            Assert.Greater(consumed.Count, 0);

            Assert.IsTrue(consumed.LMR != null);
            Assert.AreEqual("Lamar", consumed.LMR.name);
        }

        private IStoreFeedConsumer CreateObjectUnderTest()
        {
            var logService = MockLogService.New();
            return new StoreFeedConsumer(logService, SERVICE_URL);
        }

    }
}
