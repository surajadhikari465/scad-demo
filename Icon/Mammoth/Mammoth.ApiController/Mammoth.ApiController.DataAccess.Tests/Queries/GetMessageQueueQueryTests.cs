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
            queryHandler = new MammothQueries.GetMessageQueueQuery<MessageQueueItemLocale>(new GlobalContext<MammothContext>(context));
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
    }
}