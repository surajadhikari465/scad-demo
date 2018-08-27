using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mammoth.Framework;
using MammothQueries = Mammoth.ApiController.DataAccess.Queries;
using Icon.ApiController.DataAccess.Queries;
using Icon.Common.Context;
using System.Collections.Generic;
using Mammoth.Common.Testing.Builders;

namespace Mammoth.ApiController.DataAccess.Tests.Queries
{
    [TestClass]
    public class GetMessageQueueQueryTests : QueryHandlerTestBase<MammothQueries.GetMessageQueueQuery<MessageQueueItemLocale>, GetMessageQueueParameters<MessageQueueItemLocale>, MammothContext>
    {
        protected override void Initialize()
        {
            queryHandler = new MammothQueries.GetMessageQueueQuery<MessageQueueItemLocale>(new MammothContextFactory());
            context.Database.ExecuteSqlCommand("truncate table esb.MessageQueueItemLocale;");
        }

        [TestMethod]
        public void GetMessageQueue_MessageQueuesWithSameInProcessByExist_ShouldReturnMessageQueues()
        {
            //Given
            List<MessageQueueItemLocale> messageQueues = new List<MessageQueueItemLocale>
            {
                new TestMessageQueueItemLocaleBuilder().WithInProcessBy(55),
                new TestMessageQueueItemLocaleBuilder().WithInProcessBy(55),
                new TestMessageQueueItemLocaleBuilder().WithInProcessBy(55),
                new TestMessageQueueItemLocaleBuilder(),
                new TestMessageQueueItemLocaleBuilder(),
                new TestMessageQueueItemLocaleBuilder()
            };
            context.MessageQueueItemLocales.AddRange(messageQueues);
            context.SaveChanges();

            parameters.Instance = 55;

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(3, results.Count);
            foreach (var messageQueue in results)
            {
                Assert.AreEqual(55, messageQueue.InProcessBy);
            }
        }

        [TestMethod]
        public void GetMessageQueue_MessageQueuesWithIrmaItemKeyAndDefaultScanCode_ShouldHaveExpectedValues()
        {
            //Given
            var expectedInProcessBy = 55;
            var expectedIrmaItemKeys = new List<int>
            {
                5678900,
                5678901,
                5678902
            };
            var expectedDefaultScanCodes = new List<bool>
            {
                true,
                false,
                true
            };
            List<MessageQueueItemLocale> messageQueues = new List<MessageQueueItemLocale>
            {
                new TestMessageQueueItemLocaleBuilder().WithInProcessBy(expectedInProcessBy)
                    .WithIrmaItemKey(expectedIrmaItemKeys[0]).WithDefaultScanCode(expectedDefaultScanCodes[0]),
                new TestMessageQueueItemLocaleBuilder().WithInProcessBy(expectedInProcessBy)
                    .WithIrmaItemKey(expectedIrmaItemKeys[1]).WithDefaultScanCode(expectedDefaultScanCodes[1]),
                new TestMessageQueueItemLocaleBuilder().WithInProcessBy(expectedInProcessBy)
                    .WithIrmaItemKey(expectedIrmaItemKeys[2]).WithDefaultScanCode(expectedDefaultScanCodes[2]),
            };
            context.MessageQueueItemLocales.AddRange(messageQueues);
            context.SaveChanges();

            parameters.Instance = expectedInProcessBy;

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(3, results.Count);
            for (int i=0; i<3; i++)
            {
                var messageQueue = results[i];
                Assert.AreEqual(expectedInProcessBy, messageQueue.InProcessBy);
                Assert.AreEqual(expectedDefaultScanCodes[i], messageQueue.DefaultScanCode);
                Assert.AreEqual(expectedIrmaItemKeys[i], messageQueue.IrmaItemKey);
            }
        }
    }
}