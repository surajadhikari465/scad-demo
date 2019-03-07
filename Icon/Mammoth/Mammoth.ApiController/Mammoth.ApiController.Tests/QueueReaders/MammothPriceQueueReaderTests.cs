using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mammoth.ApiController.QueueReaders;
using Icon.Logging;
using Icon.Common.Email;
using Icon.ApiController.DataAccess.Queries;
using Icon.Common.DataAccess;
using Mammoth.Framework;
using System.Collections.Generic;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.Common;
using Moq;
using Mammoth.Common.DataAccess;
using Mammoth.Common.Testing.Builders;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.PreGpm.Contracts;

namespace Mammoth.ApiController.Tests.QueueReaders
{
    [TestClass]
    public class MammothPriceQueueReaderTests
    {
        private MammothPriceQueueReader queueReader;
        private Mock<ILogger> mockLogger;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IQueryHandler<GetMessageQueueParameters<MessageQueuePrice>, List<MessageQueuePrice>>> mockGetMessageQueueQuery;
        private Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueuePrice>>> mockUpdateMessageQueueStatusCommandHandler;
        private ApiControllerSettings settings;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger>();
            mockEmailClient = new Mock<IEmailClient>();
            mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueuePrice>, List<MessageQueuePrice>>>();
            mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueuePrice>>>();
            settings = new ApiControllerSettings { Instance = 1, QueueLookAhead = 100, Source = "Mammoth", MiniBulkLimitPrice = 100 };

            queueReader = new MammothPriceQueueReader(mockLogger.Object,
                mockEmailClient.Object,
                mockGetMessageQueueQuery.Object,
                mockUpdateMessageQueueStatusCommandHandler.Object,
                settings);
        }


        [TestMethod]
        public void BuildMiniBulk_Given1MessageQueuePrice_ShouldReturnMiniBulkWith1Item()
        {
            //Given
            MessageQueuePrice messageQueue = new TestMessageQueuePriceBuilder()
                .PopulateAllAttributesReg();

            //When
            var miniBulk = queueReader.BuildMiniBulk(new List<MessageQueuePrice> { messageQueue });

            //Then
            var item = miniBulk.item.Single();
            AssertMessageQueueIsEqualToContractItem(messageQueue, item);
        }

        [TestMethod]
        public void BuildMiniBulk_Given3MessageQueuePrice_ShouldReturnMiniBulkWith3Items()
        {
            //Given
            List<MessageQueuePrice> messageQueues = new List<MessageQueuePrice>
            {
                new TestMessageQueuePriceBuilder().PopulateAllAttributesReg(),
                new TestMessageQueuePriceBuilder().PopulateAllAttributesTpr(),
                new TestMessageQueuePriceBuilder().PopulateAllAttributesReg()
            };

            //When
            var miniBulk = queueReader.BuildMiniBulk(messageQueues);

            //Then
            Assert.AreEqual(3, miniBulk.item.Count());
            AssertMessageQueueIsEqualToContractItem(messageQueues[0], miniBulk.item[0]);
            AssertMessageQueueIsEqualToContractItem(messageQueues[1], miniBulk.item[1]);
            AssertMessageQueueIsEqualToContractItem(messageQueues[2], miniBulk.item[2]);
        }

        [TestMethod]
        public void GetQueueMessages_MessagesExist_ShouldReturnMessages()
        {
            //Given
            var list = new List<MessageQueuePrice>();
            mockGetMessageQueueQuery.Setup(m =>
                m.Search(It.Is<GetMessageQueueParameters<MessageQueuePrice>>(p => p.Instance == settings.Instance && p.MessageQueueStatusId == MessageStatusTypes.Ready)))
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
            List<MessageQueuePrice> messages = new List<MessageQueuePrice>
            {
                new TestMessageQueuePriceBuilder().WithItemId(1).WithBusinessUnitId(55),
                new TestMessageQueuePriceBuilder().WithItemId(2).WithBusinessUnitId(55),
                new TestMessageQueuePriceBuilder().WithItemId(3).WithBusinessUnitId(55),
                new TestMessageQueuePriceBuilder().WithItemId(4).WithBusinessUnitId(55),
                new TestMessageQueuePriceBuilder().WithItemId(5).WithBusinessUnitId(55),
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
            List<MessageQueuePrice> messages = new List<MessageQueuePrice>
            {
                new TestMessageQueuePriceBuilder().WithItemId(1).WithBusinessUnitId(55),
                new TestMessageQueuePriceBuilder().WithItemId(2).WithBusinessUnitId(55),
                new TestMessageQueuePriceBuilder().WithItemId(3).WithBusinessUnitId(55),
                new TestMessageQueuePriceBuilder().WithItemId(4).WithBusinessUnitId(55),
                new TestMessageQueuePriceBuilder().WithItemId(5).WithBusinessUnitId(55),
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
            settings.MiniBulkLimitPrice = 3;
            List<MessageQueuePrice> messages = new List<MessageQueuePrice>
            {
                new TestMessageQueuePriceBuilder().WithItemId(1).WithBusinessUnitId(55),
                new TestMessageQueuePriceBuilder().WithItemId(2).WithBusinessUnitId(55),
                new TestMessageQueuePriceBuilder().WithItemId(3).WithBusinessUnitId(55),
                new TestMessageQueuePriceBuilder().WithItemId(4).WithBusinessUnitId(55),
                new TestMessageQueuePriceBuilder().WithItemId(5).WithBusinessUnitId(55),
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

        private void AssertMessageQueueIsEqualToContractItem(MessageQueuePrice messageQueue, Contracts.ItemType item)
        {
            Assert.AreEqual(messageQueue.ItemId, item.id);
            Assert.AreEqual(messageQueue.ItemTypeCode, item.@base.type.code);
            Assert.AreEqual(messageQueue.ItemTypeDesc, item.@base.type.description);

            var locale = item.locale.First();
            var scanCode = (locale.Item as Contracts.StoreItemAttributesType).scanCode.Single().code;            
            Assert.AreEqual(messageQueue.BusinessUnitId.ToString(), locale.id.ToString());
            Assert.AreEqual(messageQueue.LocaleName, locale.name);
            Assert.AreEqual(messageQueue.ScanCode, scanCode);
            Assert.AreEqual(Contracts.LocaleCodeType.STR, locale.type.code);
            Assert.AreEqual(Contracts.LocaleDescType.Store, locale.type.description);
            Assert.IsTrue(locale.ActionSpecified);
            if (messageQueue.MessageActionId == MessageActions.AddOrUpdate)
            {
                Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate, locale.Action);
            }
            else
            {
                Assert.AreEqual(Contracts.ActionEnum.Delete, locale.Action);
            }

            var price = (locale.Item as Contracts.StoreItemAttributesType).prices.Single();
            Assert.AreEqual(messageQueue.Price, price.priceAmount.amount);
            Assert.AreEqual(messageQueue.Multiple, price.priceMultiple);
            Assert.AreEqual(messageQueue.StartDate, price.priceStartDate);
            if (price.type.id.ToString() == ItemPriceTypes.Codes.RegularPrice)
            {
                Assert.IsFalse(price.priceEndDateSpecified);
            }
            else if(price.type.id.ToString() == ItemPriceTypes.Codes.TemporaryPriceReduction)
            {
                Assert.IsTrue(price.priceEndDateSpecified);
                Assert.AreEqual(messageQueue.SubPriceTypeCode, price.type.type.id.ToString());
                Assert.AreEqual(messageQueue.SubPriceTypeCode, price.type.type.description);
            }
            else
            {
                throw new Exception("No accepted Item Price Code for price. Item Price code is " + price.type.id);
            }
            Assert.AreEqual(ItemPriceTypes.Descriptions.ByCode[price.type.id.ToString()], price.type.description);
            Assert.AreEqual(Enum.Parse(typeof(Contracts.CurrencyTypeCodeEnum), messageQueue.CurrencyCode), price.currencyTypeCode);
            Assert.IsTrue(price.uom.codeSpecified);
            Assert.IsTrue(price.uom.nameSpecified);
            Assert.IsTrue(price.priceAmount.amountSpecified);
            Assert.IsTrue(price.priceStartDateSpecified);
            if (messageQueue.UomCode == UomCodes.Each)
            {
                Assert.AreEqual(Contracts.WfmUomCodeEnumType.EA, price.uom.code);
                Assert.AreEqual(Contracts.WfmUomDescEnumType.EACH, price.uom.name);
            }
            else
            {
                Assert.AreEqual(Contracts.WfmUomCodeEnumType.LB, price.uom.code);
                Assert.AreEqual(Contracts.WfmUomDescEnumType.POUND, price.uom.name);
            }
        }
    }
}
