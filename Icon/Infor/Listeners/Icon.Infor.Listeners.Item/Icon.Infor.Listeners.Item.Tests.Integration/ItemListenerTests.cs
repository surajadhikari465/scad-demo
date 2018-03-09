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

namespace Icon.Infor.Listeners.Item.Tests.Integration
{
    [TestClass]
    public class ItemListenerTests
    {
        protected ItemListener CreateItemListenerForTest()
        {
            IconDbContextFactory contextFactory = new IconDbContextFactory();
            ItemListenerSettings settings = ItemListenerSettings.CreateFromConfig();

            ItemListener il = new ItemListener(
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
            return il;
        }

        [TestMethod]
        public void HandleMessage_GivenAProductMessageFromInforWithASingleItem_ShouldSaveItemToDatabase()
        {
            //Given
            using (var transaction = new TransactionScope())
            {
                ItemListener il = CreateItemListenerForTest();
                Mock<IEsbMessage> mockMessage = new Mock<IEsbMessage>();
                mockMessage.SetupGet(m => m.MessageText)
                    .Returns(File.ReadAllText(@"TestMessages/ProductMessageFromInfor.xml"));
                mockMessage.Setup(m => m.GetProperty("IconMessageID"))
                    .Returns("ec339683-14e6-4142-8de9-7b5c00960d62");

                //When
                il.HandleMessage(null, new EsbMessageEventArgs { Message = mockMessage.Object });

                //Then
                using (var context = new IconContext())
                {
                    var item = context.ScanCode.AsNoTracking()
                        .FirstOrDefault(sc => sc.scanCode == "888888888" && sc.itemID == 999999999)
                        .Item;

                    Assert.IsNotNull(item, "Item was not successfully saved to the database.");

                    var messageArchiveProduct = context.MessageArchiveProduct.FirstOrDefault(p => p.ItemId == 999999999);

                    Assert.IsNotNull(messageArchiveProduct, "Item update was not successfully archived to the database.");
                    Assert.IsNull(messageArchiveProduct.ErrorCode, "Item update was not successfully archived to the database.");
                }
            }
        }

        [TestMethod]
        public void HandleMessage_ProductMessageFromInforWithItemThatHasOrganicTrait_SavesItemSignAttributeAsOrganic()
        {
            //Given
            using (var transaction = new TransactionScope())
            {
                ItemListener il = CreateItemListenerForTest();
                Mock<IEsbMessage> mockMessage = new Mock<IEsbMessage>();

                //load the test message as an xml doc
                var xmlTestData = XDocument.Load(@"TestMessages/ProductMessageFromInfor.xml");
                // set the "OG" element's <trt:value> element to have our desired value
                const string organicAgencyName = "xyz123";
                xmlTestData.Root.Descendants().Where(trait => trait.Value.ToString() == "OG").First()
                    .ElementsAfterSelf().First().Descendants().Last().Value = organicAgencyName;

                mockMessage.SetupGet(m => m.MessageText)
                    .Returns(xmlTestData.ToString());
                mockMessage.Setup(m => m.GetProperty("IconMessageID"))
                    .Returns("ec339683-14e6-4142-8de9-7b5c00960d62");

                //When
                il.HandleMessage(null, new EsbMessageEventArgs { Message = mockMessage.Object });

                //Then
                using (var context = new IconContext())
                {
                    var itemSignAttribute = context.ScanCode.AsNoTracking()
                        .FirstOrDefault(sc => sc.scanCode == "888888888" && sc.itemID == 999999999)
                        .Item
                        .ItemSignAttribute
                        .FirstOrDefault();

                    Assert.IsNotNull(itemSignAttribute, "Item was not successfully saved to the database.");
                    Assert.AreEqual(organicAgencyName, itemSignAttribute.OrganicAgencyName);
                }
            }
        }

        [TestMethod]
        public void HandleMessage_ProductMessageFromInforWithItemThatHasEslTraits_SavesAsExpected()
        {
            //Given
            using (var transaction = new TransactionScope())
            {
                ItemListener il = CreateItemListenerForTest();

                Mock<IEsbMessage> mockMessage = new Mock<IEsbMessage>();

                //load the test message as an xml doc
                var xmlTestData = XDocument.Load(@"TestMessages/ProductMessageFromInfor2.xml");
                const string expected_FXT = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure etc...";
                const string expected_MOG = "1";
                const string expected_PRB = "0";
                const string expected_RFA = "1";
                const string expected_RFD = "0";
                const string expected_SMF = "1";
                const string expected_WIC = "1";
                const string expected_SLF = "600";
                const string expected_ITG = "Self Checkout item tare group has a maximum of sixty chars!";
                mockMessage.SetupGet(m => m.MessageText)
                    .Returns(xmlTestData.ToString());
                mockMessage.Setup(m => m.GetProperty("IconMessageID"))
                    .Returns("ec339683-14e6-4142-8de9-7b5c00960d62");

                //When
                il.HandleMessage(null, new EsbMessageEventArgs { Message = mockMessage.Object });

                //Then
                using (var context = new IconContext())
                {
                    Framework.Item item = context.ScanCode.AsNoTracking()
                        .FirstOrDefault(sc => sc.scanCode == "77206234" && sc.itemID == 170477)
                        .Item;

                    Assert.IsNotNull(item, "Item was not successfully saved to the database.");
                    Assert.AreEqual(expected_FXT, item.ItemTrait.Single(t => t.Trait.traitCode == "FXT").traitValue);
                    Assert.AreEqual(expected_MOG, item.ItemTrait.Single(t => t.Trait.traitCode == "MOG").traitValue);
                    Assert.AreEqual(expected_PRB, item.ItemTrait.Single(t => t.Trait.traitCode == "PRB").traitValue);
                    Assert.AreEqual(expected_RFA, item.ItemTrait.Single(t => t.Trait.traitCode == "RFA").traitValue);
                    Assert.AreEqual(expected_RFD, item.ItemTrait.Single(t => t.Trait.traitCode == "RFD").traitValue);
                    Assert.AreEqual(expected_SMF, item.ItemTrait.Single(t => t.Trait.traitCode == "SMF").traitValue);
                    Assert.AreEqual(expected_WIC, item.ItemTrait.Single(t => t.Trait.traitCode == "WIC").traitValue);
                    Assert.AreEqual(expected_SLF, item.ItemTrait.Single(t => t.Trait.traitCode == "SLF").traitValue);
                    Assert.AreEqual(expected_ITG, item.ItemTrait.Single(t => t.Trait.traitCode == "ITG").traitValue);
                }
            }
        }
    }
}
