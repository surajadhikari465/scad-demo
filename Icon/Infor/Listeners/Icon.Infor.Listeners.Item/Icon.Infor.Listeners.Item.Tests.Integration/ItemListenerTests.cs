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

namespace Icon.Infor.Listeners.Item.Tests.Integration
{
    [TestClass]
    public class ItemListenerTests
    {
        protected ItemListener CreateItemListenerForTest(Mock<IRenewableContext<IconContext>> mockGlobalContext)
        {
            ItemListener il = new ItemListener(
                              new ItemMessageParser(new NLogLogger<ItemMessageParser>()),
                              new ItemModelValidator(new ValidateItemsCommandHandler(mockGlobalContext.Object)),
                              mockGlobalContext.Object,
                              new ItemService(
                                      new ItemAddOrUpdateCommandHandler(mockGlobalContext.Object),
                                      new GenerateItemMessagesCommandHandler(mockGlobalContext.Object),
                                      new ArchiveItemsCommandHandler(),
                                      new ArchiveMessageCommandHandler(mockGlobalContext.Object)
                                  ),
                                  ListenerApplicationSettings.CreateDefaultSettings("Infor Item Listener"),
                                  EsbConnectionSettings.CreateSettingsFromConfig(),
                                  new EsbSubscriber(EsbConnectionSettings.CreateSettingsFromConfig()),
                                  EmailClient.CreateFromConfig(),
                                  new ItemListenerNotifier(EmailClient.CreateFromConfig()),
                                  new NLogLogger<ItemListener>());
            return il;
        }

        [TestMethod]
        public void HandleMessage_GivenAProductMessageFromInforWithASingleItem_ShouldSaveItemToDatabase()
        {
            //Given
            IconContext context = new IconContext();
            Mock<IRenewableContext<IconContext>> mockGlobalContext = new Mock<IRenewableContext<IconContext>>();
            mockGlobalContext.SetupGet(m => m.Context).Returns(context);

            using (var transaction = context.Database.BeginTransaction())
            {
                ItemListener il = CreateItemListenerForTest(mockGlobalContext);

                Mock<IEsbMessage> mockMessage = new Mock<IEsbMessage>();
                mockMessage.SetupGet(m => m.MessageText)
                    .Returns(File.ReadAllText(@"TestMessages/ProductMessageFromInfor.xml"));
                mockMessage.Setup(m => m.GetProperty("IconMessageID"))
                    .Returns("ec339683-14e6-4142-8de9-7b5c00960d62");

                //When
                il.HandleMessage(null, new EsbMessageEventArgs { Message = mockMessage.Object });

                //Then
                var item = context.ScanCode.AsNoTracking()
                    .FirstOrDefault(sc => sc.scanCode == "1000397" && sc.itemID == 4000110)
                    .Item;

                Assert.IsNotNull(item, "Item was not successfully saved to the database.");

                var messageArchiveProduct = context.MessageArchiveProduct.FirstOrDefault(p => p.ItemId == 4000110);

                Assert.IsNotNull(messageArchiveProduct, "Item update was not successfully archived to the database.");
            }
        }


        [TestMethod]
        public void HandleMessage_ProductMessageFromInforWithItemThatHasOrganicTrait_SavseItemSignAttributeAsOrganic()
        {
            //Given
            IconContext context = new IconContext();
            Mock<IRenewableContext<IconContext>> mockGlobalContext = new Mock<IRenewableContext<IconContext>>();
            mockGlobalContext.SetupGet(m => m.Context).Returns(context);

            using (var transaction = context.Database.BeginTransaction())
            {
                ItemListener il = CreateItemListenerForTest(mockGlobalContext);

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
                var itemSignAttribute = context.ScanCode.AsNoTracking()
                    .FirstOrDefault(sc => sc.scanCode == "1000397" && sc.itemID == 4000110)
                    .Item.ItemSignAttribute.FirstOrDefault();

                Assert.IsNotNull(itemSignAttribute, "Item was not successfully saved to the database.");
                Assert.AreEqual(organicAgencyName, itemSignAttribute.OrganicAgencyName);
            }
        }
    }
}
