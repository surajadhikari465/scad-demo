using Mammoth.Common.ControllerApplication;
using Mammoth.Common.ControllerApplication.Services;
using Mammoth.ItemLocale.Controller.DataAccess.Models;
using Mammoth.ItemLocale.Controller.Services;
using Mammoth.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.ItemLocale.Controller.Tests.Services
{
    [TestClass]
    public class ValidateItemLocaleServiceDecoratorTests
    {
        private ValidateItemLocaleServiceDecorator decorator;
        private Mock<IService<ItemLocaleEventModel>> mockService;
        private Mock<ILogger> mockLogger;
        private ItemLocaleControllerApplicationSettings settings;

        [TestInitialize]
        public void Initialize()
        {
            this.settings = new ItemLocaleControllerApplicationSettings { CurrentRegion = "FL" };
            mockLogger = new Mock<ILogger>();
            mockService = new Mock<IService<ItemLocaleEventModel>>();
            decorator = new ValidateItemLocaleServiceDecorator(mockService.Object, mockLogger.Object, settings);
        }

        [TestMethod]
        public void ValidateItemLocaleServiceDecorator_ListHasNullAuthorizeAttribute_DoesNotPassInvalidRowToItemLocaleService()
        {
            // Given
            List<ItemLocaleEventModel> itemLocales = GetItemLocaleEventModels(numberOfItems: 3);
            itemLocales[0].Authorized = null; // set first row as invalid

            // When
            this.decorator.Process(itemLocales);

            // Then
            this.mockService.Verify(s => s.Process(It.Is<List<ItemLocaleEventModel>>(m => m.Count == 2)), Times.Once);
            this.mockService.Verify(s => s.Process(It.Is<List<ItemLocaleEventModel>>(m => 
                m.Where(x => x.Authorized == true && x.CaseDiscount == true && x.TmDiscount == true).Count() == 2)), Times.Once);
            this.mockLogger.Verify(l => l.Info(It.Is<string>(s => s == String.Format("Region: {0}. ScanCode: {1}. BusinessUnitId: {2}. QueueId: {3}. " +
                                                    "The item being processed did not have store specific data. " +
                                                    "This row will be deleted and the data will be processed again when the following fields are populated: " +
                                                    "Authorized, CaseDiscount (IBM_Discount), or TM Discount (Discountable).", this.settings.CurrentRegion, itemLocales[0].ScanCode, itemLocales[0].BusinessUnitId, itemLocales[0].QueueId))), Times.Once);
            Assert.AreEqual(ApplicationErrors.InvalidDataErrorCode, itemLocales[0].ErrorMessage);
        }

        [TestMethod]
        public void ValidateItemLocaleServiceDecorator_ListHasNullCaseDiscountAttribute_DoesNotPassInvalidRowToItemLocaleService()
        {
            // Given
            List<ItemLocaleEventModel> itemLocales = GetItemLocaleEventModels(numberOfItems: 3);
            itemLocales[0].CaseDiscount = null; // set first row as invalid

            // When
            this.decorator.Process(itemLocales);

            // Then
            this.mockService.Verify(s => s.Process(It.Is<List<ItemLocaleEventModel>>(m => m.Count == 2)), Times.Once);
            this.mockService.Verify(s => s.Process(It.Is<List<ItemLocaleEventModel>>(m =>
                m.Where(x => x.Authorized == true && x.CaseDiscount == true && x.TmDiscount == true).Count() == 2)), Times.Once);
            this.mockLogger.Verify(l => l.Info(It.Is<string>(s => s == String.Format("Region: {0}. ScanCode: {1}. BusinessUnitId: {2}. QueueId: {3}. " +
                                                    "The item being processed did not have store specific data. " +
                                                    "This row will be deleted and the data will be processed again when the following fields are populated: " +
                                                    "Authorized, CaseDiscount (IBM_Discount), or TM Discount (Discountable).", this.settings.CurrentRegion, itemLocales[0].ScanCode, itemLocales[0].BusinessUnitId, itemLocales[0].QueueId))), Times.Once);
            Assert.AreEqual(ApplicationErrors.InvalidDataErrorCode, itemLocales[0].ErrorMessage);
        }

        [TestMethod]
        public void ValidateItemLocaleServiceDecorator_ListHasNullTmDiscountAttribute_DoesNotPassInvalidRowToItemLocaleService()
        {
            // Given
            List<ItemLocaleEventModel> itemLocales = GetItemLocaleEventModels(numberOfItems: 3);
            itemLocales[0].TmDiscount = null; // set first row as invalid

            // When
            this.decorator.Process(itemLocales);

            // Then
            this.mockService.Verify(s => s.Process(It.Is<List<ItemLocaleEventModel>>(m => m.Count == 2)), Times.Once);
            this.mockService.Verify(s => s.Process(It.Is<List<ItemLocaleEventModel>>(m =>
                m.Where(x => x.Authorized == true && x.CaseDiscount == true && x.TmDiscount == true).Count() == 2)), Times.Once);
            this.mockLogger.Verify(l => l.Info(It.Is<string>(s => s == String.Format("Region: {0}. ScanCode: {1}. BusinessUnitId: {2}. QueueId: {3}. " +
                                                    "The item being processed did not have store specific data. " +
                                                    "This row will be deleted and the data will be processed again when the following fields are populated: " +
                                                    "Authorized, CaseDiscount (IBM_Discount), or TM Discount (Discountable).", this.settings.CurrentRegion, itemLocales[0].ScanCode, itemLocales[0].BusinessUnitId, itemLocales[0].QueueId))), Times.Once);
            Assert.AreEqual(ApplicationErrors.InvalidDataErrorCode, itemLocales[0].ErrorMessage);
        }

        [TestMethod]
        public void ValidateItemLocaleServiceDecorator_ListHasAllValidRows_AllRowsPassedToItemLocaleService()
        {
            // Given
            List<ItemLocaleEventModel> itemLocales = GetItemLocaleEventModels(numberOfItems: 3);

            // When
            this.decorator.Process(itemLocales);

            // Then
            this.mockService.Verify(s => s.Process(It.Is<List<ItemLocaleEventModel>>(m => m.Count == 3)), Times.Once);
            this.mockLogger.Verify(l => l.Info(It.IsAny<string>()), Times.Never);
            foreach (var item in itemLocales)
            {
                Assert.IsNull(item.ErrorMessage);
            }
        }

        [TestMethod]
        public void ValidateItemLocaleServiceDecorator_ListHasOneNullAuthorizedAttribute_OthersProcessed()
        {
            // Given
            List<ItemLocaleEventModel> itemLocales = GetItemLocaleEventModels(numberOfItems: 3);
            itemLocales.ForEach(i => i.QueueId = 999); // Queue Id is the same when processed in batch
            itemLocales[0].Authorized = null; // set first row as invalid

            // When
            this.decorator.Process(itemLocales);

            // Then
            this.mockService.Verify(s => s.Process(It.Is<List<ItemLocaleEventModel>>(m => m.Count == 2)), Times.Once);

            this.mockService.Verify(s => s.Process(It.Is<List<ItemLocaleEventModel>>(m =>
                m.Where(x => x.Authorized == true && x.CaseDiscount == true && x.TmDiscount == true).Count() == 2)), Times.Once);
            this.mockLogger.Verify(l => l.Info(It.Is<string>(s => s == String.Format("Region: {0}. ScanCode: {1}. BusinessUnitId: {2}. QueueId: {3}. " +
                                                    "The item being processed did not have store specific data. " +
                                                    "This row will be deleted and the data will be processed again when the following fields are populated: " +
                                                    "Authorized, CaseDiscount (IBM_Discount), or TM Discount (Discountable).", this.settings.CurrentRegion, itemLocales[0].ScanCode, itemLocales[0].BusinessUnitId, itemLocales[0].QueueId))), Times.Once);
            Assert.AreEqual(ApplicationErrors.InvalidDataErrorCode, itemLocales[0].ErrorMessage);
            foreach (var item in itemLocales.Skip(1))
            {
                Assert.IsNull(item.ErrorMessage);
            }
        }

        private List<ItemLocaleEventModel> GetItemLocaleEventModels(int numberOfItems)
        {
            var models = new List<ItemLocaleEventModel>();
            for (int i = 0; i < numberOfItems; i++)
            {
                ItemLocaleEventModel model = new ItemLocaleEventModel
                {
                    QueueId = i,
                    ScanCode = string.Format("12345{0}", i.ToString()),
                    BusinessUnitId = 1,
                    EventTypeId = 1,
                    Authorized = true,
                    CaseDiscount = true,
                    TmDiscount = true
                };
                models.Add(model);
            }
            return models;
        }
    }
}
