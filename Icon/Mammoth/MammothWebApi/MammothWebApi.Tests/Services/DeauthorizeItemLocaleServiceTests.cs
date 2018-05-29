using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using MammothWebApi.DataAccess.Commands;
using MammothWebApi.Service.Models;
using MammothWebApi.Service.Services;
using MammothWebApi.Tests.ModelBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace MammothWebApi.Tests.Services
{
    [TestClass]
    public class DeauthorizeItemLocaleServiceTests
    {
        private Mock<ICommandHandler<DeleteItemLocalePriceCommand>> mockDeleteItemLocalePriceCommand;
        private DeauthorizeItemLocaleService deauthorizeItemLocaleService;
        private DeauthorizeItemLocale dauthorizeItemLocaleData;
        private Mock<ILogger> mockLogger;
        private int itemsCount = 0;
        private List<ItemLocaleServiceModel> itemLocaleServiceModelList;

        [TestInitialize]
        public void InitializeTests()
        {
            this.mockLogger = new Mock<ILogger>();
            this.mockDeleteItemLocalePriceCommand = new Mock<ICommandHandler<DeleteItemLocalePriceCommand>>();
            this.deauthorizeItemLocaleService = new DeauthorizeItemLocaleService(
                this.mockDeleteItemLocalePriceCommand.Object,
                this.mockLogger.Object
                );

            itemsCount = 3;
            this.itemLocaleServiceModelList = new List<ItemLocaleServiceModel>
            {
                new TestItemLocaleServiceModelBuilder().WithRegion("SW").WithScanCode("543210").WithBusinessUnit(1).Build(),
                new TestItemLocaleServiceModelBuilder().WithRegion("MW").WithScanCode("543211").WithBusinessUnit(2).Build(),
                new TestItemLocaleServiceModelBuilder().WithRegion("PN").WithScanCode("543212").WithBusinessUnit(3).Build()
            };

            this.dauthorizeItemLocaleData = new DeauthorizeItemLocale { ItemLocaleServiceModelList = this.itemLocaleServiceModelList };
        }

        [TestMethod]
        public void DeauthorizeItemLocaleService_ValidServiceModel_DeleteItemLocalePriceCommandCalledOnce()
        {
            // Given

            // When
            this.deauthorizeItemLocaleService.Handle(dauthorizeItemLocaleData);

            // Then
            this.mockDeleteItemLocalePriceCommand.Verify(s => s.Execute(It.IsAny<DeleteItemLocalePriceCommand>()), Times.Exactly(itemsCount));
        }
    }
}