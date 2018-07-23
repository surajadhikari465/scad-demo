using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mammoth.ApiController.QueueReaders;
using Icon.ApiController.DataAccess.Queries;
using Icon.Common.Email;
using Icon.Common.DataAccess;
using Mammoth.Framework;
using System.Collections.Generic;
using Mammoth.ApiController.DataAccess.Queries;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.Common;
using Icon.Logging;
using Moq;
using Mammoth.Common.Testing.Builders;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;
using Mammoth.Common.DataAccess;
using System.Globalization;

namespace Mammoth.ApiController.Tests.QueueReaders
{
    [TestClass]
    public class MammothItemLocaleQueueReaderTests
    {
        private MammothItemLocaleQueueReader queueReader;
        private Mock<ILogger> mockLogger;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueItemLocale>, List<MessageQueueItemLocale>>> mockGetMessageQueueQuery;
        private Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueItemLocale>>> mockUpdateMessageQueueStatusCommandHandler;
        private ApiControllerSettings settings;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger>();
            mockEmailClient = new Mock<IEmailClient>();
            mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueItemLocale>, List<MessageQueueItemLocale>>>();
            mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueItemLocale>>>();
            settings = new ApiControllerSettings { Instance = 1, Source = "Mammoth", MiniBulkLimitItemLocale = 100 };

            queueReader = new MammothItemLocaleQueueReader(mockLogger.Object,
                mockEmailClient.Object,
                mockGetMessageQueueQuery.Object,
                mockUpdateMessageQueueStatusCommandHandler.Object,
                settings);
        }

        [TestMethod]
        public void BuildMiniBulk_Given1MessageQueueItemLocale_ShouldReturnMiniBulkWith1Item()
        {
            //Given
            MessageQueueItemLocale messageQueue = new TestMessageQueueItemLocaleBuilder()
                .PopulateAllAttributes();

            //When
            var miniBulk = queueReader.BuildMiniBulk(new List<MessageQueueItemLocale> { messageQueue });

            //Then
            var item = miniBulk.item.Single();
            AssertMessageQueueIsEqualToContractItem(messageQueue, item);
        }

        [TestMethod]
        public void BuildMiniBulk_Given3MessageQueueItemLocale_ShouldReturnMiniBulkWith3Items()
        {
            //Given
            List<MessageQueueItemLocale> messageQueues = new List<MessageQueueItemLocale>
            {
                new TestMessageQueueItemLocaleBuilder().PopulateAllAttributes(),
                new TestMessageQueueItemLocaleBuilder()
                    .WithScanCode("98765"),
                new TestMessageQueueItemLocaleBuilder()
            };

            //When
            var miniBulk = queueReader.BuildMiniBulk(messageQueues);

            //Then
            Assert.AreEqual(miniBulk.item.Count(), messageQueues.Count);
            for (int i = 0; i < messageQueues.Count; i++)
            {
                AssertMessageQueueIsEqualToContractItem(messageQueues[i], miniBulk.item[i]);
            }
        }

        [TestMethod]
        public void BuildMiniBulk_MessageScaleExtraTextContainsIllegalXmlCharacter_ShouldFailTheMessage()
        {
            //Given
            List<MessageQueueItemLocale> messages = new List<MessageQueueItemLocale>
            {
                new TestMessageQueueItemLocaleBuilder().WithItemId(1).WithBusinessUnitId(55),
                new TestMessageQueueItemLocaleBuilder().WithItemId(2).WithBusinessUnitId(55).WithScaleExtraText("Hidden illegal character for xml in here."),
                new TestMessageQueueItemLocaleBuilder().WithItemId(3).WithBusinessUnitId(55)
            };

            //When
            var miniBulk = queueReader.BuildMiniBulk(messages);

            //Then
            Assert.AreEqual(2, miniBulk.item.Count());
            mockUpdateMessageQueueStatusCommandHandler.Verify(
                m => m.Execute(It.Is<UpdateMessageQueueStatusCommand<MessageQueueItemLocale>>(
                    (c) => c.QueuedMessages[0].ItemId == 2)), 
                Times.Once);
        }

        [TestMethod]
        public void GetQueueMessages_MessagesExist_ShouldReturnMessages()
        {
            //Given
            var list = new List<MessageQueueItemLocale>();
            mockGetMessageQueueQuery.Setup(m =>
                m.Search(It.Is<GetMessageQueueParameters<MessageQueueItemLocale>>(p => p.Instance == settings.Instance && p.MessageQueueStatusId == MessageStatusTypes.Ready)))
                .Returns(list);

            //When
            var results = queueReader.GetQueuedMessages();

            //Then
            Assert.AreEqual(list, results);
        }

        [TestMethod]
        public void GroupMessagesForMiniBulk_NumberOfMessagesIsLessThanMaxMiniBulkSize_ShouldReturnAllMessages()
        {
            //Given
            settings.MiniBulkLimitItemLocale = 10;
            List<MessageQueueItemLocale> messages = new List<MessageQueueItemLocale>
            {
                new TestMessageQueueItemLocaleBuilder().WithItemId(1).WithBusinessUnitId(55),
                new TestMessageQueueItemLocaleBuilder().WithItemId(2).WithBusinessUnitId(55),
                new TestMessageQueueItemLocaleBuilder().WithItemId(3).WithBusinessUnitId(55),
                new TestMessageQueueItemLocaleBuilder().WithItemId(4).WithBusinessUnitId(55),
                new TestMessageQueueItemLocaleBuilder().WithItemId(5).WithBusinessUnitId(55),
            };

            //When
            var actualMessages = queueReader.GroupMessagesForMiniBulk(messages);

            //Then
            Assert.AreEqual(5, actualMessages.Count);
            Assert.IsTrue(actualMessages.Contains(messages[0]));
            Assert.IsTrue(actualMessages.Contains(messages[1]));
            Assert.IsTrue(actualMessages.Contains(messages[2]));
            Assert.IsTrue(actualMessages.Contains(messages[3]));
            Assert.IsTrue(actualMessages.Contains(messages[4]));
        }

        [TestMethod]
        public void GroupMessagesForMiniBulk_NumberOfMessagesIsEqualToMaxMiniBulkSize_ShouldReturnAllMessages()
        {
            //Given
            settings.MiniBulkLimitItemLocale = 5;
            List<MessageQueueItemLocale> messages = new List<MessageQueueItemLocale>
            {
                new TestMessageQueueItemLocaleBuilder().WithItemId(1).WithBusinessUnitId(55),
                new TestMessageQueueItemLocaleBuilder().WithItemId(2).WithBusinessUnitId(55),
                new TestMessageQueueItemLocaleBuilder().WithItemId(3).WithBusinessUnitId(55),
                new TestMessageQueueItemLocaleBuilder().WithItemId(4).WithBusinessUnitId(55),
                new TestMessageQueueItemLocaleBuilder().WithItemId(5).WithBusinessUnitId(55),
            };

            //When
            var actualMessages = queueReader.GroupMessagesForMiniBulk(messages);

            //Then
            Assert.AreEqual(5, actualMessages.Count);
            Assert.IsTrue(actualMessages.Contains(messages[0]));
            Assert.IsTrue(actualMessages.Contains(messages[1]));
            Assert.IsTrue(actualMessages.Contains(messages[2]));
            Assert.IsTrue(actualMessages.Contains(messages[3]));
            Assert.IsTrue(actualMessages.Contains(messages[4]));
        }

        [TestMethod]
        public void GroupMessagesForMiniBulk_NumberOfMessagesIsMoreThanMaxMiniBulkSize_ShouldReturnMaxMiniBulkSizeNumberOfMessages()
        {
            //Given
            settings.MiniBulkLimitItemLocale = 3;
            List<MessageQueueItemLocale> messages = new List<MessageQueueItemLocale>
            {
                new TestMessageQueueItemLocaleBuilder().WithItemId(1).WithBusinessUnitId(55),
                new TestMessageQueueItemLocaleBuilder().WithItemId(2).WithBusinessUnitId(55),
                new TestMessageQueueItemLocaleBuilder().WithItemId(3).WithBusinessUnitId(55),
                new TestMessageQueueItemLocaleBuilder().WithItemId(4).WithBusinessUnitId(55),
                new TestMessageQueueItemLocaleBuilder().WithItemId(5).WithBusinessUnitId(55),
            };

            //When
            var actualMessages = queueReader.GroupMessagesForMiniBulk(messages);

            //Then
            Assert.AreEqual(3, actualMessages.Count);
            Assert.IsTrue(actualMessages.Contains(messages[0]));
            Assert.IsTrue(actualMessages.Contains(messages[1]));
            Assert.IsTrue(actualMessages.Contains(messages[2]));
            Assert.IsFalse(actualMessages.Contains(messages[3]));
            Assert.IsFalse(actualMessages.Contains(messages[4]));
        }

        [TestMethod]
        public void GroupMessagesForMiniBulk_MessagesHaveDuplicateItemAndStore_ShouldReturnDistinctMessagesbyItemAndStore()
        {
            //Given
            settings.MiniBulkLimitItemLocale = 5;
            List<MessageQueueItemLocale> messages = new List<MessageQueueItemLocale>
            {
                new TestMessageQueueItemLocaleBuilder().WithItemId(1).WithBusinessUnitId(55).WithAuthorized(true),
                new TestMessageQueueItemLocaleBuilder().WithItemId(2).WithBusinessUnitId(55),
                new TestMessageQueueItemLocaleBuilder().WithItemId(1).WithBusinessUnitId(55).WithAuthorized(false),
                new TestMessageQueueItemLocaleBuilder().WithItemId(4).WithBusinessUnitId(55),
                new TestMessageQueueItemLocaleBuilder().WithItemId(5).WithBusinessUnitId(55),
            };

            //When
            var actualMessages = queueReader.GroupMessagesForMiniBulk(messages);

            //Then
            Assert.AreEqual(4, actualMessages.Count);
            Assert.IsTrue(actualMessages.Contains(messages[0]));
            Assert.IsTrue(actualMessages.Contains(messages[1]));
            Assert.IsFalse(actualMessages.Contains(messages[2]));
            Assert.IsTrue(actualMessages.Contains(messages[3]));
            Assert.IsTrue(actualMessages.Contains(messages[4]));

            Assert.IsTrue(actualMessages.Single(m => m.ItemId == 1).Authorized);
        }

        private void AssertMessageQueueIsEqualToContractItem(MessageQueueItemLocale messageQueue, Contracts.ItemType item)
        {
            var itemBase = item.@base;
            var locale = item.locale.First();
            var scanCode = (locale.Item as Contracts.StoreItemAttributesType).scanCode.First();
            var itemTraits = (locale.Item as Contracts.StoreItemAttributesType).traits;

            Assert.AreEqual(messageQueue.ItemId, item.id);
            Assert.AreEqual(messageQueue.ItemTypeCode, itemBase.type.code);
            Assert.AreEqual(messageQueue.ItemTypeDesc, itemBase.type.description);
            Assert.AreEqual(messageQueue.BusinessUnitId.ToString(), locale.id);
            Assert.AreEqual(messageQueue.LocaleName, locale.name);
            Assert.AreEqual(messageQueue.ScanCode, scanCode.code);

            AssertTraitValuesAreEqual(messageQueue.CaseDiscount, Attributes.Codes.CaseDiscountEligible, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.TmDiscount, Attributes.Codes.TmDiscountEligible, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.AgeRestriction, Attributes.Codes.AgeRestrict, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.RestrictedHours, Attributes.Codes.RestrictedHours, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.Authorized, Attributes.Codes.AuthorizedForSale, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.Discontinued, Attributes.Codes.Discontinued, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.LabelTypeDescription, Attributes.Codes.LabelTypeDesc, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.LocalItem, Attributes.Codes.LocalItem, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.ProductCode, Attributes.Codes.ProductCode, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.RetailUnit, Attributes.Codes.RetailUnit, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.SignDescription, Attributes.Codes.SignCaption, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.Locality, Attributes.Codes.Locality, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.SignRomanceLong, Attributes.Codes.SignRomanceLong, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.SignRomanceShort, Attributes.Codes.SignRomanceShort, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.ColorAdded, Attributes.Codes.ColorAdded, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.CountryOfProcessing, Attributes.Codes.CountryOfProcessing, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.Origin, Attributes.Codes.Origin, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.ElectronicShelfTag, Attributes.Codes.ElectronicShelfTag, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.Exclusive, Attributes.Codes.Exclusive, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.NumberOfDigitsSentToScale, Attributes.Codes.NumberOfDigitsSentToScale, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.ChicagoBaby, Attributes.Codes.ChicagoBaby, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.TagUom, Attributes.Codes.TagUom, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.ScaleExtraText, Attributes.Codes.ScaleExtraText, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.Msrp, Attributes.Codes.Msrp, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.IrmaVendorKey, Attributes.Codes.IrmaVendorKey, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.SupplierName, Attributes.Codes.VendorName, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.SupplierItemID, Attributes.Codes.VendorItemId, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.SupplierCaseSize, Attributes.Codes.VendorCaseSize, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.OrderedByInfor, Attributes.Codes.OrderedByInfor, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.AltRetailSize, Attributes.Codes.AltRetailSize, itemTraits);
            AssertTraitValuesAreEqual(messageQueue.AltRetailUOM, Attributes.Codes.AltRetailUom, itemTraits);
        }

        private void AssertTraitValuesAreEqual(DateTime? messageQueueValue, string attributeCode, Contracts.TraitType[] itemTraits)
        {
            AssertTraitValuesAreEqual(messageQueueValue?.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture), attributeCode, itemTraits);
        }

        private void AssertTraitValuesAreEqual(decimal? messageQueueValue, string attributeCode, Contracts.TraitType[] itemTraits)
        {
            AssertTraitValuesAreEqual(messageQueueValue?.ToString("0.00"), attributeCode, itemTraits);
        }

        private void AssertTraitValuesAreEqual(bool? messageQueueValue, string attributeCode, Contracts.TraitType[] itemTraits)
        {
            if (messageQueueValue.HasValue)
                AssertTraitValuesAreEqual(messageQueueValue.Value, attributeCode, itemTraits);
            else
                AssertTraitValuesAreEqual(messageQueueValue?.ToString(), attributeCode, itemTraits);
        }

        private void AssertTraitValuesAreEqual(int? messageQueueValue, string attributeCode, Contracts.TraitType[] itemTraits)
        {
            AssertTraitValuesAreEqual(messageQueueValue?.ToString(), attributeCode, itemTraits);
        }

        private void AssertTraitValuesAreEqual(string messageQueueValue, string attributeCode, Contracts.TraitType[] itemTraits)
        {
            Assert.AreEqual(messageQueueValue == null ? string.Empty : messageQueueValue, itemTraits.First(it => it.code == attributeCode).type.value.First().value);
        }

        private void AssertTraitValuesAreEqual(bool messageQueueValue, string attributeCode, Contracts.TraitType[] itemTraits)
        {
            Assert.AreEqual(messageQueueValue ? "1" : "0", itemTraits.First(it => it.code == attributeCode).type.value.First().value);
        }
    }
}
