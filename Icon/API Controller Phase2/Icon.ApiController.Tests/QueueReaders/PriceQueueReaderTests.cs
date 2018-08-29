using Icon.ApiController.Common;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.DataAccess.Commands;

using Icon.ApiController.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Framework;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Contracts = Icon.Esb.Schemas.Wfm.PreGpm.Contracts;

namespace Icon.ApiController.Tests.QueueReaders
{
    [TestClass]
    public class PriceQueueReaderTests
    {
        private PriceQueueReader queueReader;
        private Mock<ILogger<PriceQueueReader>> mockLogger;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IQueryHandler<GetMessageQueueParameters<MessageQueuePrice>, List<MessageQueuePrice>>> mockGetMessageQueueQuery;
        private Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueuePrice>>> mockUpdateMessageQueueStatusCommandHandler;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger<PriceQueueReader>>();
            mockEmailClient = new Mock<IEmailClient>();
            mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueuePrice>, List<MessageQueuePrice>>>();
            mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueuePrice>>>();

            queueReader = new PriceQueueReader(
                mockLogger.Object,
                mockEmailClient.Object,
                mockGetMessageQueueQuery.Object,
                mockUpdateMessageQueueStatusCommandHandler.Object);
        }

        [TestMethod]
        public void GroupPriceMessages_InvalidArgument_ShouldThrowException()
        {
            // Given.
            int exceptionCount = 0;

            // When.
            var messages = new List<MessageQueuePrice>();

            try { queueReader.GroupMessagesForMiniBulk(messages); }
            catch (Exception) { exceptionCount++; }

            messages = null;

            try { queueReader.GroupMessagesForMiniBulk(messages); }
            catch (Exception) { exceptionCount++; }

            // Then.
            Assert.AreEqual(2, exceptionCount);
        }

        [TestMethod]
        public void GroupPriceMessages_OneMessage_ShouldReturnOneMessageForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice>
            {
                TestHelpers.GetFakeMessageQueuePrice(1, 100, 1.99m, null, null)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueuePrices);

            // Then.
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(1, messages[0].ItemId);
            Assert.AreEqual(100, messages[0].LocaleId);
        }

        [TestMethod]
        public void GroupPriceMessages_TwoMessagesWithSameItemId_ShouldReturnOneMessageForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice>
            {
                TestHelpers.GetFakeMessageQueuePrice(1, 100, 1.99m, null, null),
                TestHelpers.GetFakeMessageQueuePrice(1, 100, 1.99m, null, null)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueuePrices);

            // Then.
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(1, messages[0].ItemId);
            Assert.AreEqual(100, messages[0].LocaleId);
        }

        [TestMethod]
        public void GroupPriceMessages_DistinctFirstItemIdWithDuplicateSecondAndThirdItemId_ShouldReturnTwoMessagesForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice>
            {
                TestHelpers.GetFakeMessageQueuePrice(1, 100, 1.99m, null, null),
                TestHelpers.GetFakeMessageQueuePrice(2, 100, 1.99m, null, null),
                TestHelpers.GetFakeMessageQueuePrice(2, 100, 1.99m, null, null)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueuePrices);

            // Then.
            Assert.AreEqual(2, messages.Count);
        }

        [TestMethod]
        public void GroupPriceMessages_TwoMessagesWithDifferentLocaleId_ShouldReturnOneMessageForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice>
            {
                TestHelpers.GetFakeMessageQueuePrice(1, 100, 1.99m, null, null),
                TestHelpers.GetFakeMessageQueuePrice(2, 101, 1.99m, null, null)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueuePrices);

            // Then.
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(1, messages[0].ItemId);
            Assert.AreEqual(100, messages[0].LocaleId);
        }

        [TestMethod]
        public void GroupPriceMessages_TwoMessagesWithDifferentItemIdAndSameLocaleId_ShouldReturnTwoMessagesForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice>
            {
                TestHelpers.GetFakeMessageQueuePrice(1, 100, 1.99m, null, null),
                TestHelpers.GetFakeMessageQueuePrice(2, 100, 1.99m, null, null)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueuePrices);

            // Then.
            Assert.AreEqual(2, messages.Count);
            Assert.AreEqual(1, messages[0].ItemId);
            Assert.AreEqual(100, messages[0].LocaleId);
            Assert.AreEqual(2, messages[1].ItemId);
            Assert.AreEqual(100, messages[1].LocaleId);
        }

        [TestMethod]
        public void GetPriceMiniBulk_InvalidArguments_ExceptionShouldBeThrown()
        {
            // Given.
            var messages = new List<MessageQueuePrice>();

            int caughtExceptions = 0;

            // When.
            try { var miniBulk = queueReader.BuildMiniBulk(messages); }
            catch (ArgumentException) { caughtExceptions++; }

            messages = null;
            try { var miniBulk = queueReader.BuildMiniBulk(messages); }
            catch (ArgumentException) { caughtExceptions++; }

            // Then.
            int expectedExceptions = 2;
            Assert.AreEqual(expectedExceptions, caughtExceptions);
        }

        [TestMethod]
        public void GetPriceMiniBulk_RegPriceMessage_ShouldReturnMiniBulkWithOneEntry()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice>
            {
                TestHelpers.GetFakeMessageQueuePrice(1, 300, 2.99m, null, null)
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueuePrices);

            // Then.
            Assert.AreEqual(1, miniBulk.item.Length);
            Assert.AreEqual("AddOrUpdate", miniBulk.item[0].locale[0].Action.ToString());
            Assert.AreEqual("10111", miniBulk.item[0].locale[0].id);
        }

        [TestMethod]
        public void GetPriceMiniBulk_RegPriceMessageWithUnknownUom_UomShouldBeEach()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice>
            {
                TestHelpers.GetFakeMessageQueuePrice(1, 300, 2.99m, null, null)
            };

            fakeMessageQueuePrices[0].UomCode = "ZZ";
            fakeMessageQueuePrices[0].UomName = "ZZZZ";

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueuePrices);
            var uomCode = (miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType).prices[0].uom.code;
            var uomName = (miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType).prices[0].uom.name;

            // Then.
            Assert.AreEqual(Contracts.WfmUomCodeEnumType.EA, uomCode);
            Assert.AreEqual(Contracts.WfmUomDescEnumType.EACH, uomName);
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetPriceMiniBulk_RegWithNewTprMessage_ShouldReturnMiniBulkWithTwoEntries()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice>
            {
                TestHelpers.GetFakeMessageQueuePrice(1, 300, 2.99m, 1.99m, null)
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueuePrices);

            // Then.
            Assert.AreEqual(2, miniBulk.item.Length);
            Assert.AreEqual("AddOrUpdate", miniBulk.item[0].locale[0].Action.ToString());
            Assert.AreEqual("AddOrUpdate", miniBulk.item[1].locale[0].Action.ToString());
            Assert.AreEqual("10111", miniBulk.item[0].locale[0].id);
            Assert.AreEqual("10111", miniBulk.item[1].locale[0].id);
        }

        [TestMethod]
        public void GetPriceMiniBulk_RegWithNewTprMessageWithUnknownUom_UomShouldBeEach()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice>
            {
                TestHelpers.GetFakeMessageQueuePrice(1, 300, 2.99m, 1.99m, null)
            };

            fakeMessageQueuePrices[0].UomCode = "ZZ";
            fakeMessageQueuePrices[0].UomName = "ZZZZ";

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueuePrices);

            var regUomCode = (miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType).prices[0].uom.code;
            var regUomName = (miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType).prices[0].uom.name;
            var tprUomCode = (miniBulk.item[1].locale[0].Item as Contracts.StoreItemAttributesType).prices[0].uom.code;
            var tprUomName = (miniBulk.item[1].locale[0].Item as Contracts.StoreItemAttributesType).prices[0].uom.name;

            // Then.
            Assert.AreEqual(Contracts.WfmUomCodeEnumType.EA, regUomCode);
            Assert.AreEqual(Contracts.WfmUomDescEnumType.EACH, regUomName);
            Assert.AreEqual(Contracts.WfmUomCodeEnumType.EA, tprUomCode);
            Assert.AreEqual(Contracts.WfmUomDescEnumType.EACH, tprUomName);
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void GetPriceMiniBulk_RegWithNewTprMessage_SaleEndDateShouldIncludeTime()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice>
            {
                TestHelpers.GetFakeMessageQueuePrice(1, 300, 2.99m, 1.99m, null)
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueuePrices);

            // Then.
            var newTpr = (miniBulk.item[1].locale[0].Item as Contracts.StoreItemAttributesType).prices[0];
            var expectedSaleEndDate = fakeMessageQueuePrices[0].SaleEndDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);

            Assert.AreEqual(expectedSaleEndDate, newTpr.priceEndDate);
        }

        [TestMethod]
        public void GetPriceMiniBulk_RegWithExistingTprMessage_ShouldReturnMiniBulkWithThreeEntries()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice>
            {
                TestHelpers.GetFakeMessageQueuePrice(1, 300, 12.99m, 1.99m, 2.50m, DateTime.Today.AddDays(1))
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueuePrices);

            // Then.
            Assert.AreEqual(3, miniBulk.item.Length);
            Assert.AreEqual("AddOrUpdate", miniBulk.item[0].locale[0].Action.ToString());
            Assert.AreEqual("Delete", miniBulk.item[1].locale[0].Action.ToString());
            Assert.AreEqual("AddOrUpdate", miniBulk.item[2].locale[0].Action.ToString());
            Assert.AreEqual("10111", miniBulk.item[0].locale[0].id);
            Assert.AreEqual("10111", miniBulk.item[1].locale[0].id);
            Assert.AreEqual("10111", miniBulk.item[2].locale[0].id);
        }

        [TestMethod]
        public void GetPriceMiniBulk_RegWithExistingTprMessageWithUnknownUom_UomShouldBeEach()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice>
            {
                TestHelpers.GetFakeMessageQueuePrice(1, 300, 12.99m, 1.99m, 2.50m, DateTime.Today.AddDays(1))
            };

            fakeMessageQueuePrices[0].UomCode = "ZZ";
            fakeMessageQueuePrices[0].UomName = "ZZZZ";

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueuePrices);

            var regUomCode = (miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType).prices[0].uom.code;
            var regUomName = (miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType).prices[0].uom.name;
            var existingTprUomCode = (miniBulk.item[1].locale[0].Item as Contracts.StoreItemAttributesType).prices[0].uom.code;
            var existingTprUomName = (miniBulk.item[1].locale[0].Item as Contracts.StoreItemAttributesType).prices[0].uom.name;
            var newTprUomCode = (miniBulk.item[2].locale[0].Item as Contracts.StoreItemAttributesType).prices[0].uom.code;
            var newTprUomName = (miniBulk.item[2].locale[0].Item as Contracts.StoreItemAttributesType).prices[0].uom.name;

            // Then.
            Assert.AreEqual(Contracts.WfmUomCodeEnumType.EA, regUomCode);
            Assert.AreEqual(Contracts.WfmUomDescEnumType.EACH, regUomName);
            Assert.AreEqual(Contracts.WfmUomCodeEnumType.EA, existingTprUomCode);
            Assert.AreEqual(Contracts.WfmUomDescEnumType.EACH, existingTprUomName);
            Assert.AreEqual(Contracts.WfmUomCodeEnumType.EA, newTprUomCode);
            Assert.AreEqual(Contracts.WfmUomDescEnumType.EACH, newTprUomName);
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Exactly(3));
        }

        [TestMethod]
        public void GetPriceMiniBulk_RegWithExistingTprMessage_SaleEndDatesShouldIncludeTime()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice>
            {
                TestHelpers.GetFakeMessageQueuePrice(1, 300, 12.99m, 1.99m, 2.50m, DateTime.Today.AddDays(1))
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueuePrices);

            // Then.
            var previousTpr = (miniBulk.item[1].locale[0].Item as Contracts.StoreItemAttributesType).prices[0];
            var newTpr = (miniBulk.item[2].locale[0].Item as Contracts.StoreItemAttributesType).prices[0];

            var expectedSaleEndDateForPreviousTpr = fakeMessageQueuePrices[0].PreviousSaleEndDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
            var expectedSaleEndDateForNewTpr = fakeMessageQueuePrices[0].SaleEndDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);

            Assert.AreEqual(expectedSaleEndDateForPreviousTpr, previousTpr.priceEndDate);
            Assert.AreEqual(expectedSaleEndDateForNewTpr, newTpr.priceEndDate);
        }

        [TestMethod]
        public void GetPriceMiniBulk_RegWithExistingTprMessage_TprDeleteStartAndEndDatesShouldMatchMessage()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice>
            {
                TestHelpers.GetFakeMessageQueuePrice(1, 300, 12.99m, 1.99m, 2.50m, DateTime.Today.AddDays(1))
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueuePrices);

            // Then.
            var previousTpr = (miniBulk.item[1].locale[0].Item as Contracts.StoreItemAttributesType).prices[0];

            var expectedSaleStartDateForTprDelete = fakeMessageQueuePrices[0].PreviousSaleStartDate.Value;
            var expectedSaleEndDateForTprDelete = fakeMessageQueuePrices[0].PreviousSaleEndDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);

            Assert.AreEqual(expectedSaleStartDateForTprDelete, previousTpr.priceStartDate);
            Assert.AreEqual(expectedSaleEndDateForTprDelete, previousTpr.priceEndDate);
        }
        
        [TestMethod]
        public void GetPriceMiniBulk_RegWithExistingTprMessageAndDifferentSaleStartDateThanPrevious_TprDeleteShouldBeGenerated()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice>
            {
                TestHelpers.GetFakeMessageQueuePrice(1, 300, 12.99m, 2.50m, 2.50m, DateTime.Today.AddDays(1))
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueuePrices);

            // Then.
            Assert.AreEqual(3, miniBulk.item.Length);
            Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate, miniBulk.item[0].locale[0].Action);
            Assert.AreEqual(Contracts.ActionEnum.Delete, miniBulk.item[1].locale[0].Action);
            Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate, miniBulk.item[2].locale[0].Action);

            var regPrice = (miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType).prices[0];
            var tprDelete = (miniBulk.item[1].locale[0].Item as Contracts.StoreItemAttributesType).prices[0];
            var tprAdd = (miniBulk.item[2].locale[0].Item as Contracts.StoreItemAttributesType).prices[0];

            Assert.AreEqual(ItemPriceDescriptions.RegularPrice, regPrice.type.description);
            Assert.AreEqual(ItemPriceDescriptions.TemporaryPriceReduction, tprDelete.type.description);
            Assert.AreEqual(ItemPriceDescriptions.TemporaryPriceReduction, tprAdd.type.description);
        }

        [TestMethod]
        public void GetPriceMiniBulk_RegWithExistingTprMessageAndMatchingSalesStartDateAsPrevious_TprDeleteShouldNotBeGenerated()
        {
            // Given.
            DateTime startDate = DateTime.Now;

            var fakeMessageQueuePrices = new List<MessageQueuePrice>
            {
                TestHelpers.GetFakeMessageQueuePrice(1, 300, 12.99m, 1.99m, 2.50m)
            };

            fakeMessageQueuePrices[0].SaleStartDate = startDate;
            fakeMessageQueuePrices[0].PreviousSaleStartDate = startDate;

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueuePrices);

            // Then.
            Assert.AreEqual(2, miniBulk.item.Length);
            Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate, miniBulk.item[0].locale[0].Action);
            Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate, miniBulk.item[1].locale[0].Action);

            var regPrice = (miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType).prices[0];
            var tpr = (miniBulk.item[1].locale[0].Item as Contracts.StoreItemAttributesType).prices[0];

            Assert.AreEqual(ItemPriceDescriptions.RegularPrice, regPrice.type.description);
            Assert.AreEqual(ItemPriceDescriptions.TemporaryPriceReduction, tpr.type.description);
        }

        [TestMethod]
        public void GetPriceMiniBulk_RegWithExistingExpiredTpr_TprDeleteShouldNotBeGenerated()
        {
            // Given.
            DateTime startDate = DateTime.Now;

            var fakeMessageQueuePrices = new List<MessageQueuePrice>
            {
                TestHelpers.GetFakeMessageQueuePrice(1, 300, 12.99m, 1.99m, 2.50m)
            };

            fakeMessageQueuePrices[0].SaleStartDate = startDate;
            fakeMessageQueuePrices[0].PreviousSaleEndDate = new DateTime(2015, 1, 1);

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueuePrices);

            // Then.
            Assert.AreEqual(2, miniBulk.item.Length);
            Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate, miniBulk.item[0].locale[0].Action);
            Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate, miniBulk.item[1].locale[0].Action);

            var regPrice = (miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType).prices[0];
            var tpr = (miniBulk.item[1].locale[0].Item as Contracts.StoreItemAttributesType).prices[0];

            Assert.AreEqual(ItemPriceDescriptions.RegularPrice, regPrice.type.description);
            Assert.AreEqual(ItemPriceDescriptions.TemporaryPriceReduction, tpr.type.description);
        }

        [TestMethod]
        public void GetPriceMiniBulk_OneMessageOfEachTypeAndExistingTprIsNotExpired_ShouldReturnMiniBulkWithSixEntries()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice>
            {
                TestHelpers.GetFakeMessageQueuePrice(1, 300, 1.99m, null, null),
                TestHelpers.GetFakeMessageQueuePrice(2, 300, 5.99m, 3.99m, null),
                TestHelpers.GetFakeMessageQueuePrice(3, 300, 2.99m, 1.99m, 2.50m, DateTime.Today.AddDays(1))
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueuePrices);

            // Then.
            Assert.AreEqual(6, miniBulk.item.Length);
            Assert.AreEqual("AddOrUpdate", miniBulk.item[0].locale[0].Action.ToString());
            Assert.AreEqual("AddOrUpdate", miniBulk.item[1].locale[0].Action.ToString());
            Assert.AreEqual("AddOrUpdate", miniBulk.item[2].locale[0].Action.ToString());
            Assert.AreEqual("AddOrUpdate", miniBulk.item[3].locale[0].Action.ToString());
            Assert.AreEqual("Delete", miniBulk.item[4].locale[0].Action.ToString());
            Assert.AreEqual("AddOrUpdate", miniBulk.item[5].locale[0].Action.ToString());
        }

        [TestMethod]
        public void GetPriceMiniBulk_OneMessageOfEachTypeWithMultiple_ShouldReturnMiniBulkWithSixEntries()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice>
            {
                TestHelpers.GetFakeMessageQueuePriceWithMultiple(1, 300, 1.99m, null, null, 2, null),
                TestHelpers.GetFakeMessageQueuePriceWithMultiple(2, 300, 5.99m, 3.99m, null, 2, 2),
                TestHelpers.GetFakeMessageQueuePriceWithMultiple(3, 300, 2.99m, 1.99m, 2.50m, 2, 2, DateTime.Today.AddDays(1))
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueuePrices);

            // Then.
            Assert.AreEqual(6, miniBulk.item.Length);
            Assert.AreEqual("AddOrUpdate", miniBulk.item[0].locale[0].Action.ToString());
            Assert.AreEqual("AddOrUpdate", miniBulk.item[1].locale[0].Action.ToString());
            Assert.AreEqual("AddOrUpdate", miniBulk.item[2].locale[0].Action.ToString());
            Assert.AreEqual("AddOrUpdate", miniBulk.item[3].locale[0].Action.ToString());
            Assert.AreEqual("Delete", miniBulk.item[4].locale[0].Action.ToString());
            Assert.AreEqual("AddOrUpdate", miniBulk.item[5].locale[0].Action.ToString());
        }

        [TestMethod]
        public void GetPriceMiniBulk_CancelAllSales_TprDeleteShouldBeGenerated()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice>
            {
                TestHelpers.GetFakeMessageQueuePriceWithMultiple(1, 300, 1.99m, null, null, 2, null),
            };

            fakeMessageQueuePrices[0].ChangeType = Constants.PriceChangeTypes.CancelAllSales;
            fakeMessageQueuePrices[0].PreviousSaleEndDate = DateTime.Today.Date;
            fakeMessageQueuePrices[0].PreviousSalePrice = 1.99m;

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueuePrices);

            // Then.
            Assert.AreEqual(fakeMessageQueuePrices.Count, miniBulk.item.Length);
        }
    }
}
