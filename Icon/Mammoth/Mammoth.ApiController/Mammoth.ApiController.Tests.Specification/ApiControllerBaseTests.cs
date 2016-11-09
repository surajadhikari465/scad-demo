using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.ApiController.Controller;
using Mammoth.Framework;
using System.Linq;
using System.Collections.Generic;
using Mammoth.Common.Testing.Builders;

namespace Mammoth.ApiController.Tests.Specification
{
    [TestClass]
    public class ApiControllerBaseTests
    {
        [Ignore]
        [TestMethod]
        public void Execute_2000ItemLocaleMessageQueues_ShouldSendMessagesForMessageQueues()
        {
            //Given
            using (MammothContext context = new MammothContext())
            {
                context.Database.ExecuteSqlCommand("DELETE esb.MessageHistory");
                context.Database.ExecuteSqlCommand("DELETE esb.MessageQueueItemLocale");
                List<MessageQueueItemLocale> messageQueues = new List<MessageQueueItemLocale>();
                for (int i = 1; i < 1000; i++)
                {
                    messageQueues.Add(new TestMessageQueueItemLocaleBuilder()
                        .PopulateAllAttributes()
                        .WithItemId(i));
                }
                for (int i = 1000; i < 2001; i++)
                {
                    messageQueues.Add(new TestMessageQueueItemLocaleBuilder()
                        .WithItemId(i));
                }
                context.MessageQueueItemLocales.AddRange(messageQueues);
                context.SaveChanges();
            }
            var apiControllerBase = SimpleInjectorInitializer.InitializeContainer(55, "i").GetInstance<ApiControllerBase>();

            //When
            apiControllerBase.Execute();

            //Then
            using (MammothContext context = new MammothContext())
            {
                Console.WriteLine(context.MessageHistories.Count());
                Assert.AreEqual(2000, context.MessageQueueItemLocales.Count());
                Assert.IsTrue(context.MessageQueueItemLocales.All(il => il.ProcessedDate != null));
                Assert.IsTrue(context.MessageQueueItemLocales.All(il => il.InProcessBy == null));
                //context.Database.ExecuteSqlCommand("DELETE esb.MessageHistory");
                context.Database.ExecuteSqlCommand("DELETE esb.MessageQueueItemLocale");
            }
        }

        [Ignore]
        [TestMethod]
        public void Execute_2000PriceMessageQueues_ShouldSendMessagesForMessageQueues()
        {
            //Given
            using (MammothContext context = new MammothContext())
            {
                context.Database.ExecuteSqlCommand("DELETE esb.MessageHistory");
                context.Database.ExecuteSqlCommand("DELETE esb.MessageQueuePrice");
                List<MessageQueuePrice> messageQueues = new List<MessageQueuePrice>();
                for (int i = 1; i < 1000; i++)
                {
                    messageQueues.Add(new TestMessageQueuePriceBuilder()
                        .PopulateAllAttributesReg()
                        .WithItemId(i));
                }
                for (int i = 1000; i < 2001; i++)
                {
                    messageQueues.Add(new TestMessageQueuePriceBuilder()
                        .PopulateAllAttributesTpr()
                        .WithItemId(i));
                }
                context.MessageQueuePrices.AddRange(messageQueues);
                context.SaveChanges();
            }
            var apiControllerBase = SimpleInjectorInitializer.InitializeContainer(55, "r").GetInstance<ApiControllerBase>();

            //When
            apiControllerBase.Execute();

            //Then
            using (MammothContext context = new MammothContext())
            {
                Console.WriteLine(context.MessageHistories.Count());
                Assert.AreEqual(2000, context.MessageQueuePrices.Count());
                Assert.IsTrue(context.MessageQueuePrices.All(il => il.ProcessedDate != null));
                Assert.IsTrue(context.MessageQueuePrices.All(il => il.InProcessBy == null));
                //context.Database.ExecuteSqlCommand("DELETE esb.MessageHistory");
                context.Database.ExecuteSqlCommand("DELETE esb.MessageQueuePrice");
            }
        }
    }
}
