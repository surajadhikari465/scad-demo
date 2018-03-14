using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Infor.Listeners.Item.MessageParsers;
using Icon.Infor.Listeners.Item.Services;
using Icon.Infor.Listeners.Item.Commands;
using Icon.Esb.ListenerApplication;
using Icon.Esb;
using Icon.Esb.Subscriber;
using Icon.Common.Email;
using Icon.Logging;
using Icon.Framework;
using Icon.Common.Context;
using Moq;
using System.IO;
using System.Linq;
using Icon.Infor.Listeners.Item.Validators;
using Icon.Infor.Listeners.Item.Notifiers;
using System.Xml.Linq;
using Icon.Infor.Listeners.Item.Queries;
using System.Transactions;
using Esb.Core.EsbServices;
using Icon.Esb.Services.ConfirmationBod;
using System.Collections.Generic;

namespace Icon.Infor.Listeners.Item.Tests.Integration
{
    [TestClass]
    public class ItemListenerTests
    {
        protected const string xmlFilePath = @"TestMessages/ProductMessageFromInfor2.xml";
        protected IconDbContextFactory contextFactory = new IconDbContextFactory();
        protected ItemListenerSettings settings = ItemListenerSettings.CreateFromConfig();
        protected ItemListenerTestData testData = new ItemListenerTestData();
        protected ItemListener itemListener;

        [TestInitialize]
        public void Initialize()
        {
            itemListener = new ItemListener(
                new ItemMessageParser(settings, new NLogLogger<ItemMessageParser>()),
                new ItemModelValidator(settings, new GetItemValidationPropertiesQuery(contextFactory)),
                new ItemService(
                        new ItemAddOrUpdateCommandHandler(contextFactory),
                        new GenerateItemMessagesCommandHandler(contextFactory),
                        new ArchiveItemsCommandHandler(contextFactory),
                        new ArchiveMessageCommandHandler(contextFactory)
                    ),
                    ListenerApplicationSettings.CreateDefaultSettings("Infor Item Listener"),
                    EsbConnectionSettings.CreateSettingsFromConfig(),
                    new EsbSubscriber(EsbConnectionSettings.CreateSettingsFromConfig()),
                    EmailClient.CreateFromConfig(),
                    new ItemListenerNotifier(
                        EmailClient.CreateFromConfig(),
                        settings,
                        new Mock<IEsbService<ConfirmationBodEsbRequest>>().Object),
                    new NLogLogger<ItemListener>());
        }

        protected void SetTraitValueInXmlData(IEnumerable<XElement> traitElements, string traitCode, string traitValue)
        {
            traitElements.Where(trait => trait.Value.ToString() == traitCode).First()
                    .ElementsAfterSelf().First().Descendants().Last().Value = traitValue;
        }

        [TestMethod]
        public void HandleMessage_ProductMessageFromInforForSingleItem_SavesItem()
        {
            //Given
            using (var transaction = new TransactionScope())
            {
                var xmlData = File.ReadAllText(xmlFilePath);
                var mockMessageObj = testData.GetMockEsbMessage(xmlData);

                //When
                itemListener.HandleMessage(null, new EsbMessageEventArgs { Message = mockMessageObj });

                //Then
                using (var context = new IconContext())
                {
                    var item = context.ScanCode.AsNoTracking()
                        .FirstOrDefault(sc => sc.scanCode == testData.Item_A_ScanCode && sc.itemID == testData.Item_A_ItemID)
                        .Item;
                    Assert.IsNotNull(item, "Item was not successfully saved to the database.");
                }
            }
        }

        [TestMethod]
        public void HandleMessage_ProductMessageFromInforForSingleItem_ArchivesMessage()
        {
            //Given
            using (var transaction = new TransactionScope())
            {
                var xmlData = File.ReadAllText(xmlFilePath);
                var mockMessageObj = testData.GetMockEsbMessage(xmlData);

                //When
                itemListener.HandleMessage(null, new EsbMessageEventArgs { Message = mockMessageObj });

                //Then
                using (var context = new IconContext())
                {
                    var messageArchiveProduct = context.MessageArchiveProduct.FirstOrDefault(p => p.ItemId == testData.Item_A_ItemID);

                    Assert.IsNotNull(messageArchiveProduct, "Item update was not successfully archived to the database.");
                    Assert.IsNull(messageArchiveProduct.ErrorCode, "Item update was not successfully archived to the database.");
                }
            }
        }

        [TestMethod]
        public void HandleMessage_ProductMessageFromInforForItemWithTraitOrganic_SavesTrait()
        {
            //Given
            var expectedOrganicAgencyName = testData.Attribs.OrganicAgency;
            using (var transaction = new TransactionScope())
            {
                //load the test message as an xml doc
                var xmlData = XDocument.Load(xmlFilePath);
                // set the "OG" element's <trt:value> element to have our desired value
                SetTraitValueInXmlData(xmlData.Root.Descendants(), "OG", expectedOrganicAgencyName);
                var mockMessageObj = testData.GetMockEsbMessage(xmlData.ToString());

                //When
                itemListener.HandleMessage(null, new EsbMessageEventArgs { Message = mockMessageObj });

                //Then
                using (var context = new IconContext())
                {
                    var itemSignAttribute = context.ScanCode.AsNoTracking()
                        .FirstOrDefault(sc => sc.scanCode == testData.Item_A_ScanCode && sc.itemID == testData.Item_A_ItemID)
                        .Item
                        .ItemSignAttribute
                        .FirstOrDefault();

                    Assert.IsNotNull(itemSignAttribute, "Item was not successfully saved to the database.");
                    Assert.AreEqual(expectedOrganicAgencyName, itemSignAttribute.OrganicAgencyName);
                }
            }
        }
        
        [TestMethod]
        public void HandleMessage_ProductMessageFromInforForItemWithGpmTraitsForSlaw_SavesTraits()
        {
            //Given
            using (var transaction = new TransactionScope())
            {
                //load the test message as an xml doc
                var xmlData = XDocument.Load(xmlFilePath);
                var mockMessageObj = testData.GetMockEsbMessage(xmlData.ToString());
                string traitValue = String.Empty;

                //When
                itemListener.HandleMessage(null, new EsbMessageEventArgs { Message = mockMessageObj });

                //Then
                using (var context = new IconContext())
                {
                    Framework.Item item = context.ScanCode.AsNoTracking()
                        .FirstOrDefault(sc => sc.scanCode == testData.Item_A_ScanCode && sc.itemID == testData.Item_A_ItemID)
                        .Item;

                    Assert.IsNotNull(item, "Item was not successfully saved to the database.");

                    traitValue = item.ItemTrait.Single(t => t.Trait.traitCode == "FXT").traitValue;
                    Assert.AreEqual(testData.Attribs.FlexibleText, traitValue,
                        $"Actual {nameof(testData.Attribs.FlexibleText)} value ({traitValue}) did not match expected ({testData.Attribs.FlexibleText})");

                    traitValue = item.ItemTrait.Single(t => t.Trait.traitCode == "MOG").traitValue;
                    Assert.AreEqual( testData.Attribs.MadeWithOrganicGrapes, traitValue,
                        $"Actual {nameof(testData.Attribs.MadeWithOrganicGrapes)} value ({traitValue}) did not match expected ({testData.Attribs.MadeWithOrganicGrapes})");

                    traitValue = item.ItemTrait.Single(t => t.Trait.traitCode == "PRB").traitValue;
                    Assert.AreEqual( testData.Attribs.PrimeBeef, traitValue,
                        $"Actual {nameof(testData.Attribs.PrimeBeef)} value ({traitValue}) did not match expected ({testData.Attribs.PrimeBeef})");

                    traitValue = item.ItemTrait.Single(t => t.Trait.traitCode == "RFA").traitValue;
                    Assert.AreEqual( testData.Attribs.RainforestAlliance, traitValue,
                        $"Actual {nameof(testData.Attribs.RainforestAlliance)}  value({traitValue}) did not match expected ({testData.Attribs.RainforestAlliance})");

                    traitValue = item.ItemTrait.Single(t => t.Trait.traitCode == "RFD").traitValue;
                    Assert.AreEqual( testData.Attribs.Refrigerated, traitValue,
                        $"Actual {nameof(testData.Attribs.Refrigerated)} value ({traitValue}) did not match expected ({testData.Attribs.Refrigerated})");

                    traitValue = item.ItemTrait.Single(t => t.Trait.traitCode == "SMF").traitValue;
                    Assert.AreEqual( testData.Attribs.SmithsonianBirdFriendly, traitValue,
                        $"Actual {nameof(testData.Attribs.SmithsonianBirdFriendly)}  value({traitValue}) did not match expected ({testData.Attribs.SmithsonianBirdFriendly})");

                    traitValue = item.ItemTrait.Single(t => t.Trait.traitCode == "WIC").traitValue;
                    Assert.AreEqual( testData.Attribs.WIC, traitValue,
                        $"Actual {nameof(testData.Attribs.WIC)} ({traitValue}) value did not match expected ({testData.Attribs.WIC})");

                    traitValue = item.ItemTrait.Single(t => t.Trait.traitCode == "ITG").traitValue;
                    Assert.AreEqual( testData.Attribs.SelfCheckoutItemTareGroup, traitValue,
                        $"Actual {nameof(testData.Attribs.SelfCheckoutItemTareGroup)} value ({traitValue}) did not match expected ({testData.Attribs.SelfCheckoutItemTareGroup})");
                }
            }
        }

        [TestMethod]
        public void HandleMessage_ProductMessageFromInforForItemWithTraitsForEsl_SavesTraits()
        {
            //Given
            using (var transaction = new TransactionScope())
            {
                //load the test message as an xml doc
                var xmlData = XDocument.Load(xmlFilePath);
                var mockMessageObj = testData.GetMockEsbMessage(xmlData.ToString());
                string traitValue = String.Empty;

                //When
                itemListener.HandleMessage(null, new EsbMessageEventArgs { Message = mockMessageObj });

                //Then
                using (var context = new IconContext())
                {
                    Framework.Item item = context.ScanCode.AsNoTracking()
                        .FirstOrDefault(sc => sc.scanCode == testData.Item_A_ScanCode && sc.itemID == testData.Item_A_ItemID)
                        .Item;

                    Assert.IsNotNull(item, "Item was not successfully saved to the database.");

                    traitValue = item.ItemTrait.Single(t => t.Trait.traitCode == "FXT").traitValue;
                    Assert.AreEqual(testData.Attribs.FlexibleText, traitValue,
                        $"Actual {nameof(testData.Attribs.FlexibleText)} value ({traitValue}) did not match expected ({testData.Attribs.FlexibleText})");

                    traitValue = item.ItemTrait.Single(t => t.Trait.traitCode == "MOG").traitValue;
                    Assert.AreEqual(testData.Attribs.MadeWithOrganicGrapes, traitValue,
                        $"Actual {nameof(testData.Attribs.MadeWithOrganicGrapes)} value ({traitValue}) did not match expected ({testData.Attribs.MadeWithOrganicGrapes})");

                    traitValue = item.ItemTrait.Single(t => t.Trait.traitCode == "PRB").traitValue;
                    Assert.AreEqual(testData.Attribs.PrimeBeef, traitValue,
                        $"Actual {nameof(testData.Attribs.PrimeBeef)} value ({traitValue}) did not match expected ({testData.Attribs.PrimeBeef})");

                    traitValue = item.ItemTrait.Single(t => t.Trait.traitCode == "RFA").traitValue;
                    Assert.AreEqual(testData.Attribs.RainforestAlliance, traitValue,
                        $"Actual {nameof(testData.Attribs.RainforestAlliance)}  value({traitValue}) did not match expected ({testData.Attribs.RainforestAlliance})");

                    traitValue = item.ItemTrait.Single(t => t.Trait.traitCode == "RFD").traitValue;
                    Assert.AreEqual(testData.Attribs.Refrigerated, traitValue,
                        $"Actual {nameof(testData.Attribs.Refrigerated)} value ({traitValue}) did not match expected ({testData.Attribs.Refrigerated})");

                    traitValue = item.ItemTrait.Single(t => t.Trait.traitCode == "SMF").traitValue;
                    Assert.AreEqual(testData.Attribs.SmithsonianBirdFriendly, traitValue,
                        $"Actual {nameof(testData.Attribs.SmithsonianBirdFriendly)}  value({traitValue}) did not match expected ({testData.Attribs.SmithsonianBirdFriendly})");

                    traitValue = item.ItemTrait.Single(t => t.Trait.traitCode == "WIC").traitValue;
                    Assert.AreEqual(testData.Attribs.WIC, traitValue,
                        $"Actual {nameof(testData.Attribs.WIC)} ({traitValue}) value did not match expected ({testData.Attribs.WIC})");

                    traitValue = item.ItemTrait.Single(t => t.Trait.traitCode == "ITG").traitValue;
                    Assert.AreEqual(testData.Attribs.SelfCheckoutItemTareGroup, traitValue,
                        $"Actual {nameof(testData.Attribs.SelfCheckoutItemTareGroup)} value ({traitValue}) did not match expected ({testData.Attribs.SelfCheckoutItemTareGroup})");

                }
            }
        }

        [TestMethod]
        public void HandleMessage_ProductMessageFromInforForItemWithTraitsForOnePlum_SavesTraits()
        {
            //Given
            using (var transaction = new TransactionScope())
            {
                //load the test message as an xml doc
                var xmlData = XDocument.Load(xmlFilePath);
                var mockMessageObj = testData.GetMockEsbMessage(xmlData.ToString());
                string traitValue = String.Empty;

                //When
                itemListener.HandleMessage(null, new EsbMessageEventArgs { Message = mockMessageObj });

                //Then
                using (var context = new IconContext())
                {
                    Framework.Item item = context.ScanCode.AsNoTracking()
                        .FirstOrDefault(sc => sc.scanCode == testData.Item_A_ScanCode && sc.itemID == testData.Item_A_ItemID)
                        .Item;

                    Assert.IsNotNull(item, "Item was not successfully saved to the database.");

                    traitValue = item.ItemTrait.Single(t => t.Trait.traitCode == "RFD").traitValue;
                    Assert.AreEqual(testData.Attribs.Refrigerated, traitValue,
                        $"Actual {nameof(testData.Attribs.Refrigerated)} value ({traitValue}) did not match expected ({testData.Attribs.Refrigerated})");

                    traitValue = item.ItemTrait.Single(t => t.Trait.traitCode == "SLF").traitValue;
                    Assert.AreEqual(testData.Attribs.ShelfLife, traitValue,
                        $"Actual {nameof(testData.Attribs.ShelfLife)} ({traitValue}) value did not match expected ({testData.Attribs.ShelfLife})");


                }
            }
        }
    }
}
