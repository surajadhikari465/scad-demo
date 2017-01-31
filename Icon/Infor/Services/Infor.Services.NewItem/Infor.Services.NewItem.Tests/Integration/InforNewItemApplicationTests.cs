﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Irma.Framework;
using System.Linq;
using Icon.Common;
using Infor.Services.NewItem.Processor;
using Icon.Esb.Producer;
using Moq;
using System.Collections.Generic;
using System.IO;

namespace Infor.Services.NewItem.Tests.Integration
{
    [TestClass]
    public class InforNewItemApplicationTests
    {
        [TestInitialize]
        public void Initialize()
        {
            using (var context = new IrmaContext("ItemCatalog_FL"))
            {
                context.Database.ExecuteSqlCommand("DELETE dbo.IconItemChangeQueue");
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var context = new IrmaContext("ItemCatalog_FL"))
            {
                context.Database.ExecuteSqlCommand("DELETE dbo.IconItemChangeQueue");
            }
        }

        [TestMethod]
        public void Given1NewItemEvent_WhenTheApplicationIsRun_ShouldGenerateNewItemMessageForEvents()
        {
            //Given
            Mock<IEsbProducer> mockProducer = new Mock<IEsbProducer>();

            //uncomment to generate the item messages and save them to a locale file for viewing and troubleshooting
            //int messageCount = 0;
            //mockProducer.Setup(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            //    .Callback<string, Dictionary<string, string>>((s, d) => File.WriteAllText(@"message" + ++messageCount + ".xml", s));

            string instanceId = AppSettingsAccessor.GetStringSetting("InstanceId");
            List<string> identifiers = new List<string>();
            using (var context = new IrmaContext("ItemCatalog_FL"))
            {
                var itemIdentifiers = context.ItemIdentifier
                    .Where(
                        ii => ii.Default_Identifier == 1
                        && ii.Deleted_Identifier == 0
                        && ii.Remove_Identifier == 0
                        && ii.Item.Retail_Sale == true
                        && ii.Item.Deleted_Item == false
                        && ii.Item.Remove_Item == 0
                        && context.ValidatedScanCode.Any(vsc => vsc.ScanCode == ii.Identifier))
                    .Take(1)
                    .ToList();
                context.IconItemChangeQueue.AddRange(itemIdentifiers.Select(ii => new IconItemChangeQueue
                {
                    Identifier = ii.Identifier,
                    Item_Key = ii.Item_Key,
                    ItemChgTypeID = 1,
                    InProcessBy = instanceId
                }));
                context.SaveChanges();
                identifiers.AddRange(itemIdentifiers.Select(ii => ii.Identifier));
            }

            var container = SimpleInjectorInitializer.InitializeContainer(false);
            container.Options.AllowOverridingRegistrations = true;
            container.Register(() => mockProducer.Object);
            container.Register(() => new InforNewItemApplicationSettings { Regions = new List<string> { "FL" }, NumberOfItemsPerMessage = 1 });

            //When
            var processor = container.GetInstance<INewItemProcessor>();
            processor.ProcessNewItemEvents(int.Parse(instanceId));

            //Then
            using (var context = new IrmaContext("ItemCatalog_FL"))
            {
                Assert.IsFalse(context.IconItemChangeQueue.Any(q => identifiers.Contains(q.Identifier)));
            }
        }
    }
}
