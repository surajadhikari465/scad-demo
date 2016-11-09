using Icon.Common.Email;
using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.Common;
using PushController.Common.Models;
using PushController.Controller.CacheHelpers;
using PushController.Controller.Decorators;
using PushController.Controller.MessageBuilders;
using PushController.DataAccess.Interfaces;
using PushController.DataAccess.Queries;
using System.Collections.Generic;
using System.Linq;

namespace PushController.Tests.Controller.DecoratorTests
{
    [TestClass]
    public class PriceUomChangeAlertDecoratorTests
    {
        private PriceUomChangeAlertDecorator decorator;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IQueryHandler<GetItemPricesByPushDataQuery, List<ItemPriceModel>>> mockGetItemPricesQuery;
        private Mock<IMessageBuilder<MessageQueuePrice>> mockMessageBuilder;
        private Mock<IPriceUomChangeConfiguration> mockConfiguration;
        private Mock<ICacheHelper<int, Locale>> mockLocaleCacheHelper;
        private Mock<ICacheHelper<string, ScanCodeModel>> mockScanCodeCacheHelper;
        private Mock<ILogger<PriceUomChangeAlertDecorator>> mockLogger;

        [TestInitialize]
        public void InitializeData()
        {
            this.mockEmailClient = new Mock<IEmailClient>();
            this.mockGetItemPricesQuery = new Mock<IQueryHandler<GetItemPricesByPushDataQuery, List<ItemPriceModel>>>();
            this.mockMessageBuilder = new Mock<IMessageBuilder<MessageQueuePrice>>();
            this.mockConfiguration = new Mock<IPriceUomChangeConfiguration>();
            this.mockLocaleCacheHelper = new Mock<ICacheHelper<int, Locale>>();
            this.mockScanCodeCacheHelper = new Mock<ICacheHelper<string, ScanCodeModel>>();
            this.mockLogger = new Mock<ILogger<PriceUomChangeAlertDecorator>>();
        }

        private void InstantiateDecorator()
        {
            this.decorator = new PriceUomChangeAlertDecorator(
                this.mockEmailClient.Object,
                this.mockGetItemPricesQuery.Object,
                this.mockConfiguration.Object,
                this.mockLocaleCacheHelper.Object,
                this.mockScanCodeCacheHelper.Object,
                this.mockLogger.Object,
                this.mockMessageBuilder.Object);
        }

        [TestMethod]
        public void EmailUomChangeDecorator_SendUomChangeEmailsIsTurnedOff_DoesNotSendEmail()
        {
            // Given
            this.mockConfiguration.Setup(c => c.SendEmails).Returns(false);
            InstantiateDecorator();

            // When
            this.decorator.BuildMessages(new List<IRMAPush>());

            // Then
            this.mockEmailClient.Verify(ec => ec.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never,
                "The 'Send' method of the EmailClient was called when it should not have been.");
        }

        [TestMethod]
        public void EmailUomChangeDecorator_SendUomChangeEmailsIsTurnedOff_ReturnsMessages()
        {
            // Given
            this.mockConfiguration.Setup(c => c.SendEmails).Returns(false);
            InstantiateDecorator();

            // When
            this.decorator.BuildMessages(new List<IRMAPush>());

            // Then
            this.mockMessageBuilder.Verify(mb => mb.BuildMessages(It.IsAny<List<IRMAPush>>()), Times.Once,
                "The method BuildMessages of the MessageBuilder class was not called, but was expected");
        }

        [TestMethod]
        public void EmailUomChangeDecorator_ItemHasNoItemPriceInformation_EmailIsNotSent()
        {
            // Given
            IRMAPush irmaPush = new TestIrmaPushBuilder().WithSoldByWeight(true);
            ItemPriceModel itemPrice = new ItemPriceModel { ItemId = 1, LocaleId = 1, ItemPriceTypeId = ItemPriceTypes.Reg, UomId = UOMs.Pound };
            Locale locale = new TestLocaleBuilder().WithBusinessUnitId(irmaPush.BusinessUnit_ID);
            ScanCodeModel scanCodeModel = new ScanCodeModel { ItemId = itemPrice.ItemId, ScanCode = irmaPush.Identifier };

            var irmaPushList = new List<IRMAPush>();
            irmaPushList.Add(irmaPush);

            this.mockConfiguration.Setup(c => c.SendEmails).Returns(true);
            this.mockConfiguration.Setup(c => c.PriceUomChangeSubject).Returns("test");
            this.mockConfiguration.Setup(c => c.PriceUomChangeRecipients).Returns("test@test.com");
            this.mockGetItemPricesQuery.Setup(ipq => ipq.Execute(It.IsAny<GetItemPricesByPushDataQuery>())).Returns(new List<ItemPriceModel>());
            this.mockLocaleCacheHelper.Setup(lch => lch.Retrieve(It.IsAny<int>())).Returns(locale);
            this.mockScanCodeCacheHelper.Setup(sch => sch.Retrieve(It.IsAny<string>())).Returns(scanCodeModel);

            InstantiateDecorator();

            // When
            this.decorator.BuildMessages(irmaPushList);

            // Then
            this.mockMessageBuilder.Verify(mb => mb.BuildMessages(It.IsAny<List<IRMAPush>>()), Times.Once,
                "The Message Builder BuildMessages method was not called one time and should have been.");
            this.mockEmailClient.Verify(ec => ec.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never,
                 "The Send method of the EmailClient occurred when it was not expected.");
        }

        [TestMethod]
        public void EmailUomChangeDecorator_SoldByWeightDidNotChange_EmailIsNotSent()
        {
            // Given
            IRMAPush irmaPush = new TestIrmaPushBuilder().WithSoldByWeight(true);
            ItemPriceModel itemPrice = new ItemPriceModel { ItemId = 1, LocaleId = 1, ItemPriceTypeId = ItemPriceTypes.Reg, UomId = UOMs.Pound };
            Locale locale = new TestLocaleBuilder().WithBusinessUnitId(irmaPush.BusinessUnit_ID);
            ScanCodeModel scanCodeModel = new ScanCodeModel { ItemId = itemPrice.ItemId, ScanCode = irmaPush.Identifier };

            var irmaPushList = new List<IRMAPush>();
            irmaPushList.Add(irmaPush);

            var itemPrices = new List<ItemPriceModel>();
            itemPrices.Add(itemPrice);

            this.mockConfiguration.Setup(c => c.SendEmails).Returns(true);
            this.mockConfiguration.Setup(c => c.PriceUomChangeSubject).Returns("test");
            this.mockConfiguration.Setup(c => c.PriceUomChangeRecipients).Returns("test@test.com");
            this.mockGetItemPricesQuery.Setup(ipq => ipq.Execute(It.IsAny<GetItemPricesByPushDataQuery>())).Returns(itemPrices);
            this.mockLocaleCacheHelper.Setup(lch => lch.Retrieve(It.IsAny<int>())).Returns(locale);
            this.mockScanCodeCacheHelper.Setup(sch => sch.Retrieve(It.IsAny<string>())).Returns(scanCodeModel);

            InstantiateDecorator();

            // When
            this.decorator.BuildMessages(irmaPushList);

            // Then
            this.mockMessageBuilder.Verify(mb => mb.BuildMessages(It.IsAny<List<IRMAPush>>()), Times.Once,
                "The Message Builder BuildMessages method was not called one time and should have been.");
            this.mockEmailClient.Verify(ec => ec.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never,
                 "The Send method of the EmailClient occurred when it was not expected.");
        }

        [TestMethod]
        public void EmailUomChangeDecorator_SoldByWeightDidChange_EmailIsSent()
        {
            // Given
            IRMAPush irmaPush = new TestIrmaPushBuilder().WithSoldByWeight(false);
            ItemPriceModel itemPrice = new ItemPriceModel { ItemId = 1, LocaleId = 1, ItemPriceTypeId = ItemPriceTypes.Reg, UomId = UOMs.Pound };
            Locale locale = new TestLocaleBuilder().WithBusinessUnitId(irmaPush.BusinessUnit_ID).WithLocaleId(1);
            ScanCodeModel scanCodeModel = new ScanCodeModel { ItemId = itemPrice.ItemId, ScanCode = irmaPush.Identifier };

            var irmaPushList = new List<IRMAPush>();
            irmaPushList.Add(irmaPush);

            var itemPrices = new List<ItemPriceModel>();
            itemPrices.Add(itemPrice);

            this.mockConfiguration.Setup(c => c.SendEmails).Returns(true);
            this.mockConfiguration.Setup(c => c.PriceUomChangeSubject).Returns("test");
            this.mockConfiguration.Setup(c => c.PriceUomChangeRecipients).Returns("test@test.com");
            this.mockGetItemPricesQuery.Setup(ipq => ipq.Execute(It.IsAny<GetItemPricesByPushDataQuery>())).Returns(itemPrices);
            this.mockLocaleCacheHelper.Setup(lch => lch.Retrieve(It.IsAny<int>())).Returns(locale);
            this.mockScanCodeCacheHelper.Setup(sch => sch.Retrieve(It.IsAny<string>())).Returns(scanCodeModel);

            InstantiateDecorator();

            // When
            this.decorator.BuildMessages(irmaPushList);

            // Then
            this.mockMessageBuilder.Verify(mb => mb.BuildMessages(It.IsAny<List<IRMAPush>>()), Times.Once,
                "The Message Builder BuildMessages method was not called one time and should have been.");
            this.mockEmailClient.Verify(ec => ec.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once,
                "The Send method of the EmailClient should have executed one time but it did not.");
        }

        [TestMethod]
        public void EmailUomChangeDecorator_SoldByWeightDidNotChange_ReturnsBuiltMessages()
        {
            // Given
            MessageQueuePrice expectedMessage = new TestPriceMessageBuilder().Build();
            var messageList = new List<MessageQueuePrice>();
            messageList.Add(expectedMessage);
            this.mockMessageBuilder.Setup(mb => mb.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(messageList);

            IRMAPush irmaPush = new TestIrmaPushBuilder().WithSoldByWeight(false);
            ItemPriceModel itemPrice = new ItemPriceModel { ItemId = 1, LocaleId = 1, ItemPriceTypeId = ItemPriceTypes.Reg, UomId = UOMs.Pound };
            Locale locale = new TestLocaleBuilder().WithBusinessUnitId(irmaPush.BusinessUnit_ID).WithLocaleId(1);
            ScanCodeModel scanCodeModel = new ScanCodeModel { ItemId = itemPrice.ItemId, ScanCode = irmaPush.Identifier };

            var irmaPushList = new List<IRMAPush>();
            irmaPushList.Add(irmaPush);

            var itemPrices = new List<ItemPriceModel>();
            itemPrices.Add(itemPrice);

            this.mockConfiguration.Setup(c => c.SendEmails).Returns(true);
            this.mockConfiguration.Setup(c => c.PriceUomChangeSubject).Returns("test");
            this.mockConfiguration.Setup(c => c.PriceUomChangeRecipients).Returns("test@test.com");
            this.mockGetItemPricesQuery.Setup(ipq => ipq.Execute(It.IsAny<GetItemPricesByPushDataQuery>())).Returns(itemPrices);
            this.mockLocaleCacheHelper.Setup(lch => lch.Retrieve(It.IsAny<int>())).Returns(locale);
            this.mockScanCodeCacheHelper.Setup(sch => sch.Retrieve(It.IsAny<string>())).Returns(scanCodeModel);

            InstantiateDecorator();

            // When
            var actual = this.decorator.BuildMessages(irmaPushList);
            MessageQueuePrice actualMessage = actual.First();

            // Then
            Assert.AreEqual(expectedMessage.BusinessUnit_ID, actualMessage.BusinessUnit_ID, "The actual BusinessUnit_ID did not match expected value.");
            Assert.AreEqual(expectedMessage.ChangeType, actualMessage.ChangeType, "The actual ChangeType did not match the expected value.");
            Assert.AreEqual(expectedMessage.CurrencyCode, actualMessage.CurrencyCode, "The actual CurrencyCode did not match the expected value.");
            Assert.AreEqual(expectedMessage.InProcessBy, actualMessage.InProcessBy, "The actual InProcessBy did not match the expected value.");
            Assert.AreEqual(expectedMessage.InsertDate, actualMessage.InsertDate, "The actual InsertDate did not match the expected value.");
            Assert.AreEqual(expectedMessage.IRMAPushID, actualMessage.IRMAPushID, "The actual IRMAPushID did not match the expected value.");
            Assert.AreEqual(expectedMessage.ItemId, actualMessage.ItemId, "The actual ItemId did not match the expected value.");
            Assert.AreEqual(expectedMessage.ItemTypeCode, actualMessage.ItemTypeCode, "The actual ItemTypeCode did not match the expected value.");
            Assert.AreEqual(expectedMessage.ItemTypeDesc, actualMessage.ItemTypeDesc, "The actual ItemTypeDesc did not match the expected value.");
            Assert.AreEqual(expectedMessage.LocaleId, actualMessage.LocaleId, "The actual LocaleId did not match the expected value.");
            Assert.AreEqual(expectedMessage.LocaleName, actualMessage.LocaleName, "The actual LocaleName did not match the expected value.");
            Assert.AreEqual(expectedMessage.MessageHistoryId, actualMessage.MessageHistoryId, "The actual MessageHistoryId did not match the expected value.");
            Assert.AreEqual(expectedMessage.MessageQueueId, actualMessage.MessageQueueId, "The actual MessageQueueId did not match the expected value.");
            Assert.AreEqual(expectedMessage.MessageStatusId, actualMessage.MessageStatusId, "The actual MessageStatusId did not match the expected value.");
            Assert.AreEqual(expectedMessage.MessageTypeId, actualMessage.MessageTypeId, "The actual MessageTypeId did not match the expected value.");
            Assert.AreEqual(expectedMessage.Multiple, actualMessage.Multiple, "The actual Multiple did not match the expected value.");
            Assert.AreEqual(expectedMessage.PreviousSaleEndDate, actualMessage.PreviousSaleEndDate, "The actual PreviousSaleEndDate did not match the expected value.");
            Assert.AreEqual(expectedMessage.PreviousSaleMultiple, actualMessage.PreviousSaleMultiple, "The actual PreviousSaleMultiple did not match the expected value.");
            Assert.AreEqual(expectedMessage.PreviousSalePrice, actualMessage.PreviousSalePrice, "The actual PreviousSalePrice did not match the expected value.");
            Assert.AreEqual(expectedMessage.PreviousSaleStartDate, actualMessage.PreviousSaleStartDate, "The actual PreviousSaleStartDate did not match the expected value.");
            Assert.AreEqual(expectedMessage.Price, actualMessage.Price, "The actual Price did not match the expected value.");
            Assert.AreEqual(expectedMessage.ProcessedDate, actualMessage.ProcessedDate, "The actual ProcessedDate did not match the expected value.");
            Assert.AreEqual(expectedMessage.RegionCode, actualMessage.RegionCode, "The actual RegionCode did not match the expected value.");
            Assert.AreEqual(expectedMessage.SaleEndDate, actualMessage.SaleEndDate, "The actual SaleEndDate did not match the expected value.");
            Assert.AreEqual(expectedMessage.SaleMultiple, actualMessage.SaleMultiple, "The actual SaleMultiple did not match the expected value.");
            Assert.AreEqual(expectedMessage.SalePrice, actualMessage.SalePrice, "The actual SalePrice did not match the expected value.");
            Assert.AreEqual(expectedMessage.SaleStartDate, actualMessage.SaleStartDate, "The actual SaleStartDate did not match the expected value.");
            Assert.AreEqual(expectedMessage.ScanCode, actualMessage.ScanCode, "The actual ScanCode did not match the expected value.");
            Assert.AreEqual(expectedMessage.ScanCodeId, actualMessage.ScanCodeId, "The actual ScanCodeId did not match the expected value.");
            Assert.AreEqual(expectedMessage.ScanCodeTypeDesc, actualMessage.ScanCodeTypeDesc, "The actual ScanCodeTypeDesc did not match the expected value.");
            Assert.AreEqual(expectedMessage.ScanCodeTypeId, actualMessage.ScanCodeTypeId, "The actual ScanCodeTypeId did not match the expected value.");
            Assert.AreEqual(expectedMessage.UomCode, actualMessage.UomCode, "The actual UomCode did not match the expected value.");
            Assert.AreEqual(expectedMessage.UomName, actualMessage.UomName, "The actual UomName did not match the expected value.");
        }

        [TestMethod]
        public void EmailUomChangeDecorator_SoldByWeightDidChange_ReturnsBuiltMessages()
        {
            // Given
            MessageQueuePrice expectedMessage = new TestPriceMessageBuilder().Build();
            var messageList = new List<MessageQueuePrice>();
            messageList.Add(expectedMessage);
            this.mockMessageBuilder.Setup(mb => mb.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(messageList);

            IRMAPush irmaPush = new TestIrmaPushBuilder().WithSoldByWeight(false);
            ItemPriceModel itemPrice = new ItemPriceModel { ItemId = 1, LocaleId = 1, ItemPriceTypeId = ItemPriceTypes.Reg, UomId = UOMs.Pound };
            Locale locale = new TestLocaleBuilder().WithBusinessUnitId(irmaPush.BusinessUnit_ID).WithLocaleId(1);
            ScanCodeModel scanCodeModel = new ScanCodeModel { ItemId = itemPrice.ItemId, ScanCode = irmaPush.Identifier };

            var irmaPushList = new List<IRMAPush>();
            irmaPushList.Add(irmaPush);

            var itemPrices = new List<ItemPriceModel>();
            itemPrices.Add(itemPrice);

            this.mockConfiguration.Setup(c => c.SendEmails).Returns(true);
            this.mockConfiguration.Setup(c => c.PriceUomChangeSubject).Returns("test");
            this.mockConfiguration.Setup(c => c.PriceUomChangeRecipients).Returns("test@test.com");
            this.mockGetItemPricesQuery.Setup(ipq => ipq.Execute(It.IsAny<GetItemPricesByPushDataQuery>())).Returns(itemPrices);
            this.mockLocaleCacheHelper.Setup(lch => lch.Retrieve(It.IsAny<int>())).Returns(locale);
            this.mockScanCodeCacheHelper.Setup(sch => sch.Retrieve(It.IsAny<string>())).Returns(scanCodeModel);

            InstantiateDecorator();

            // When
            var actual = this.decorator.BuildMessages(irmaPushList);
            MessageQueuePrice actualMessage = actual.First();

            // Then
            Assert.AreEqual(expectedMessage.BusinessUnit_ID, actualMessage.BusinessUnit_ID, "The actual BusinessUnit_ID did not match expected value.");
            Assert.AreEqual(expectedMessage.ChangeType, actualMessage.ChangeType, "The actual ChangeType did not match the expected value.");
            Assert.AreEqual(expectedMessage.CurrencyCode, actualMessage.CurrencyCode, "The actual CurrencyCode did not match the expected value.");
            Assert.AreEqual(expectedMessage.InProcessBy, actualMessage.InProcessBy, "The actual InProcessBy did not match the expected value.");
            Assert.AreEqual(expectedMessage.InsertDate, actualMessage.InsertDate, "The actual InsertDate did not match the expected value.");
            Assert.AreEqual(expectedMessage.IRMAPushID, actualMessage.IRMAPushID, "The actual IRMAPushID did not match the expected value.");
            Assert.AreEqual(expectedMessage.ItemId, actualMessage.ItemId, "The actual ItemId did not match the expected value.");
            Assert.AreEqual(expectedMessage.ItemTypeCode, actualMessage.ItemTypeCode, "The actual ItemTypeCode did not match the expected value.");
            Assert.AreEqual(expectedMessage.ItemTypeDesc, actualMessage.ItemTypeDesc, "The actual ItemTypeDesc did not match the expected value.");
            Assert.AreEqual(expectedMessage.LocaleId, actualMessage.LocaleId, "The actual LocaleId did not match the expected value.");
            Assert.AreEqual(expectedMessage.LocaleName, actualMessage.LocaleName, "The actual LocaleName did not match the expected value.");
            Assert.AreEqual(expectedMessage.MessageHistoryId, actualMessage.MessageHistoryId, "The actual MessageHistoryId did not match the expected value.");
            Assert.AreEqual(expectedMessage.MessageQueueId, actualMessage.MessageQueueId, "The actual MessageQueueId did not match the expected value.");
            Assert.AreEqual(expectedMessage.MessageStatusId, actualMessage.MessageStatusId, "The actual MessageStatusId did not match the expected value.");
            Assert.AreEqual(expectedMessage.MessageTypeId, actualMessage.MessageTypeId, "The actual MessageTypeId did not match the expected value.");
            Assert.AreEqual(expectedMessage.Multiple, actualMessage.Multiple, "The actual Multiple did not match the expected value.");
            Assert.AreEqual(expectedMessage.PreviousSaleEndDate, actualMessage.PreviousSaleEndDate, "The actual PreviousSaleEndDate did not match the expected value.");
            Assert.AreEqual(expectedMessage.PreviousSaleMultiple, actualMessage.PreviousSaleMultiple, "The actual PreviousSaleMultiple did not match the expected value.");
            Assert.AreEqual(expectedMessage.PreviousSalePrice, actualMessage.PreviousSalePrice, "The actual PreviousSalePrice did not match the expected value.");
            Assert.AreEqual(expectedMessage.PreviousSaleStartDate, actualMessage.PreviousSaleStartDate, "The actual PreviousSaleStartDate did not match the expected value.");
            Assert.AreEqual(expectedMessage.Price, actualMessage.Price, "The actual Price did not match the expected value.");
            Assert.AreEqual(expectedMessage.ProcessedDate, actualMessage.ProcessedDate, "The actual ProcessedDate did not match the expected value.");
            Assert.AreEqual(expectedMessage.RegionCode, actualMessage.RegionCode, "The actual RegionCode did not match the expected value.");
            Assert.AreEqual(expectedMessage.SaleEndDate, actualMessage.SaleEndDate, "The actual SaleEndDate did not match the expected value.");
            Assert.AreEqual(expectedMessage.SaleMultiple, actualMessage.SaleMultiple, "The actual SaleMultiple did not match the expected value.");
            Assert.AreEqual(expectedMessage.SalePrice, actualMessage.SalePrice, "The actual SalePrice did not match the expected value.");
            Assert.AreEqual(expectedMessage.SaleStartDate, actualMessage.SaleStartDate, "The actual SaleStartDate did not match the expected value.");
            Assert.AreEqual(expectedMessage.ScanCode, actualMessage.ScanCode, "The actual ScanCode did not match the expected value.");
            Assert.AreEqual(expectedMessage.ScanCodeId, actualMessage.ScanCodeId, "The actual ScanCodeId did not match the expected value.");
            Assert.AreEqual(expectedMessage.ScanCodeTypeDesc, actualMessage.ScanCodeTypeDesc, "The actual ScanCodeTypeDesc did not match the expected value.");
            Assert.AreEqual(expectedMessage.ScanCodeTypeId, actualMessage.ScanCodeTypeId, "The actual ScanCodeTypeId did not match the expected value.");
            Assert.AreEqual(expectedMessage.UomCode, actualMessage.UomCode, "The actual UomCode did not match the expected value.");
            Assert.AreEqual(expectedMessage.UomName, actualMessage.UomName, "The actual UomName did not match the expected value.");
        }

        [TestMethod]
        public void EmailUomChangeDecorator_SendEmailIsTurnedOff_ReturnsBuiltMessages()
        {
            // Given
            MessageQueuePrice expectedMessage = new TestPriceMessageBuilder().Build();
            var messageList = new List<MessageQueuePrice>();
            messageList.Add(expectedMessage);
            this.mockMessageBuilder.Setup(mb => mb.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(messageList);

            IRMAPush irmaPush = new TestIrmaPushBuilder().WithSoldByWeight(false);
            var irmaPushList = new List<IRMAPush>();
            irmaPushList.Add(irmaPush);
            InstantiateDecorator();

            // When
            var actual = this.decorator.BuildMessages(irmaPushList);
            MessageQueuePrice actualMessage = actual.First();

            // Then
            Assert.AreEqual(expectedMessage.BusinessUnit_ID, actualMessage.BusinessUnit_ID, "The actual BusinessUnit_ID did not match expected value.");
            Assert.AreEqual(expectedMessage.ChangeType, actualMessage.ChangeType, "The actual ChangeType did not match the expected value.");
            Assert.AreEqual(expectedMessage.CurrencyCode, actualMessage.CurrencyCode, "The actual CurrencyCode did not match the expected value.");
            Assert.AreEqual(expectedMessage.InProcessBy, actualMessage.InProcessBy, "The actual InProcessBy did not match the expected value.");
            Assert.AreEqual(expectedMessage.InsertDate, actualMessage.InsertDate, "The actual InsertDate did not match the expected value.");
            Assert.AreEqual(expectedMessage.IRMAPushID, actualMessage.IRMAPushID, "The actual IRMAPushID did not match the expected value.");
            Assert.AreEqual(expectedMessage.ItemId, actualMessage.ItemId, "The actual ItemId did not match the expected value.");
            Assert.AreEqual(expectedMessage.ItemTypeCode, actualMessage.ItemTypeCode, "The actual ItemTypeCode did not match the expected value.");
            Assert.AreEqual(expectedMessage.ItemTypeDesc, actualMessage.ItemTypeDesc, "The actual ItemTypeDesc did not match the expected value.");
            Assert.AreEqual(expectedMessage.LocaleId, actualMessage.LocaleId, "The actual LocaleId did not match the expected value.");
            Assert.AreEqual(expectedMessage.LocaleName, actualMessage.LocaleName, "The actual LocaleName did not match the expected value.");
            Assert.AreEqual(expectedMessage.MessageHistoryId, actualMessage.MessageHistoryId, "The actual MessageHistoryId did not match the expected value.");
            Assert.AreEqual(expectedMessage.MessageQueueId, actualMessage.MessageQueueId, "The actual MessageQueueId did not match the expected value.");
            Assert.AreEqual(expectedMessage.MessageStatusId, actualMessage.MessageStatusId, "The actual MessageStatusId did not match the expected value.");
            Assert.AreEqual(expectedMessage.MessageTypeId, actualMessage.MessageTypeId, "The actual MessageTypeId did not match the expected value.");
            Assert.AreEqual(expectedMessage.Multiple, actualMessage.Multiple, "The actual Multiple did not match the expected value.");
            Assert.AreEqual(expectedMessage.PreviousSaleEndDate, actualMessage.PreviousSaleEndDate, "The actual PreviousSaleEndDate did not match the expected value.");
            Assert.AreEqual(expectedMessage.PreviousSaleMultiple, actualMessage.PreviousSaleMultiple, "The actual PreviousSaleMultiple did not match the expected value.");
            Assert.AreEqual(expectedMessage.PreviousSalePrice, actualMessage.PreviousSalePrice, "The actual PreviousSalePrice did not match the expected value.");
            Assert.AreEqual(expectedMessage.PreviousSaleStartDate, actualMessage.PreviousSaleStartDate, "The actual PreviousSaleStartDate did not match the expected value.");
            Assert.AreEqual(expectedMessage.Price, actualMessage.Price, "The actual Price did not match the expected value.");
            Assert.AreEqual(expectedMessage.ProcessedDate, actualMessage.ProcessedDate, "The actual ProcessedDate did not match the expected value.");
            Assert.AreEqual(expectedMessage.RegionCode, actualMessage.RegionCode, "The actual RegionCode did not match the expected value.");
            Assert.AreEqual(expectedMessage.SaleEndDate, actualMessage.SaleEndDate, "The actual SaleEndDate did not match the expected value.");
            Assert.AreEqual(expectedMessage.SaleMultiple, actualMessage.SaleMultiple, "The actual SaleMultiple did not match the expected value.");
            Assert.AreEqual(expectedMessage.SalePrice, actualMessage.SalePrice, "The actual SalePrice did not match the expected value.");
            Assert.AreEqual(expectedMessage.SaleStartDate, actualMessage.SaleStartDate, "The actual SaleStartDate did not match the expected value.");
            Assert.AreEqual(expectedMessage.ScanCode, actualMessage.ScanCode, "The actual ScanCode did not match the expected value.");
            Assert.AreEqual(expectedMessage.ScanCodeId, actualMessage.ScanCodeId, "The actual ScanCodeId did not match the expected value.");
            Assert.AreEqual(expectedMessage.ScanCodeTypeDesc, actualMessage.ScanCodeTypeDesc, "The actual ScanCodeTypeDesc did not match the expected value.");
            Assert.AreEqual(expectedMessage.ScanCodeTypeId, actualMessage.ScanCodeTypeId, "The actual ScanCodeTypeId did not match the expected value.");
            Assert.AreEqual(expectedMessage.UomCode, actualMessage.UomCode, "The actual UomCode did not match the expected value.");
            Assert.AreEqual(expectedMessage.UomName, actualMessage.UomName, "The actual UomName did not match the expected value.");
        }
    }
}