using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using MammothWebApi.DataAccess.Commands;
using MammothWebApi.Service.Models;
using MammothWebApi.Service.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MammothWebApi.Tests.Services
{
    [TestClass]
    public class DeletePriceServiceTests
    {
        Mock<ICommandHandler<StagingPriceCommand>> mockStagingPriceCommand;
        Mock<ICommandHandler<DeletePricesCommand>> mockDeletePriceCommand;
        Mock<ICommandHandler<DeleteStagingCommand>> mockDeleteStagingCommand;
        Mock<ICommandHandler<AddEsbMessageQueuePriceCommand>> mockEsbMessageQueueCommand;
        DeletePriceService deletePriceService;
        Guid guid;
        DeletePrice priceData;

        [TestInitialize]
        public void InitializeTests()
        {
            this.mockStagingPriceCommand = new Mock<ICommandHandler<StagingPriceCommand>>();
            this.mockDeletePriceCommand = new Mock<ICommandHandler<DeletePricesCommand>>();
            this.mockDeleteStagingCommand = new Mock<ICommandHandler<DeleteStagingCommand>>();
            this.mockEsbMessageQueueCommand = new Mock<ICommandHandler<AddEsbMessageQueuePriceCommand>>();
            this.deletePriceService = new DeletePriceService(this.mockStagingPriceCommand.Object,
                this.mockDeletePriceCommand.Object,
                this.mockDeleteStagingCommand.Object,
                this.mockEsbMessageQueueCommand.Object);

            this.guid = Guid.NewGuid();

            this.priceData = new DeletePrice();
            this.priceData.Prices = BuildRegPricesServiceModelList(numberOfItems: 3, region: "SW");
        }

        [TestMethod]
        public void DeletePriceService_ValidServiceModel_StagingPriceCommandCalledOnce()
        {
            // Given

            // When
            this.deletePriceService.Handle(priceData);

            // Then
            this.mockStagingPriceCommand.Verify(s => s.Execute(It.IsAny<StagingPriceCommand>()), Times.Once);
        }

        [TestMethod]
        public void DeletePriceService_PriceServiceModelHasOneRegion_DeleteCommandHandlerCalledOnce()
        {
            // Given

            // When
            this.deletePriceService.Handle(priceData);

            // Then
            this.mockDeletePriceCommand.Verify(r => r.Execute(It.IsAny<DeletePricesCommand>()), Times.Once);
        }

        [TestMethod]
        public void DeletePriceService_PriceServiceModelHasMoreThanOneRegion_DeleteCommandHandlerCalledOncePerRegion()
        {
            // Given
            priceData.Prices.AddRange(BuildRegPricesServiceModelList(numberOfItems: 3, region: "FL"));
            priceData.Prices.AddRange(BuildRegPricesServiceModelList(numberOfItems: 3, region: "MW"));
            int expectedTimes = priceData.Prices.Select(p => p.Region).Distinct().Count();

            // When
            this.deletePriceService.Handle(priceData);

            // Then
            this.mockDeletePriceCommand.Verify(r => r.Execute(It.IsAny<DeletePricesCommand>()), Times.Exactly(expectedTimes));
        }

        [TestMethod]
        public void DeletePricesService_PriceServiceModel_DeleteStagingPriceCommandHandlerCalledOnce()
        {
            // Given

            // When
            this.deletePriceService.Handle(priceData);

            // Then
            this.mockDeleteStagingCommand.Verify(s => s.Execute(It.IsAny<DeleteStagingCommand>()), Times.Once);
        }

        [TestMethod]
        public void DeletePricesService_PriceServiceModelWithOneRegion_AddEsbMessageQueuePriceCommandHandlerCalledOnce()
        {
            // Given
            DateTime now = DateTime.Now;

            // When
            this.deletePriceService.Handle(priceData);

            // Then
            this.mockEsbMessageQueueCommand.Verify(e => e.Execute(It.Is<AddEsbMessageQueuePriceCommand>(c =>
                c.MessageActionId == MessageActions.Delete && c.Region == "SW" && c.Timestamp >= now)), Times.Once);
        }

        [TestMethod]
        public void DeletePricesService_PriceServiceModelWithMultipleRegions_AddEsbMessageQueuePriceCommandHandlerCalledOncePerRegion()
        {
            // Given
            DateTime now = DateTime.Now;
            priceData.Prices.AddRange(BuildRegPricesServiceModelList(numberOfItems: 3, region: "FL"));
            priceData.Prices.AddRange(BuildRegPricesServiceModelList(numberOfItems: 3, region: "MW"));

            // When
            this.deletePriceService.Handle(priceData);

            // Then
            this.mockEsbMessageQueueCommand.Verify(e => e.Execute(It.Is<AddEsbMessageQueuePriceCommand>(c =>
                c.MessageActionId == MessageActions.Delete && c.Region == "SW" && c.Timestamp >= now)), Times.Once);
            this.mockEsbMessageQueueCommand.Verify(e => e.Execute(It.Is<AddEsbMessageQueuePriceCommand>(c =>
                c.MessageActionId == MessageActions.Delete && c.Region == "FL" && c.Timestamp >= now)), Times.Once);
            this.mockEsbMessageQueueCommand.Verify(e => e.Execute(It.Is<AddEsbMessageQueuePriceCommand>(c =>
                c.MessageActionId == MessageActions.Delete && c.Region == "MW" && c.Timestamp >= now)), Times.Once);
        }

        [TestMethod]
        public void DeletePricesService_ExceptionThrownDuringDeletePriceCommandHandler_ShouldCallDeletePriceStagingCommandHandler()
        {
            // Given


            // When
            try
            {
                this.deletePriceService.Handle(priceData);
            }
            catch (Exception)
            {
                // Then
                this.mockDeleteStagingCommand
                    .Verify(sc => sc.Execute(It.IsAny<DeleteStagingCommand>()), Times.Once);
            }
        }

        private List<PriceServiceModel> BuildRegPricesServiceModelList(int numberOfItems, string region)
        {
            var prices = new List<PriceServiceModel>();

            for (int i = 0; i < numberOfItems; i++)
            {
                PriceServiceModel price = new PriceServiceModel
                {
                    Region = region,
                    BusinessUnitId = 112233,
                    ScanCode = String.Format("123456789{0}", i.ToString()),
                    StartDate = new DateTime(2015, 9, 1),
                    EndDate = null,
                    Multiple = 1,
                    Price = 4.99M,
                    PriceType = "REG"
                };

                prices.Add(price);
            }

            return prices;
        }

        private List<PriceServiceModel> BuildSalePricesServiceModelList(int numberOfItems, string region)
        {
            var prices = new List<PriceServiceModel>();

            for (int i = 0; i < numberOfItems; i++)
            {
                PriceServiceModel price = new PriceServiceModel
                {
                    Region = region,
                    BusinessUnitId = 112233,
                    ScanCode = String.Format("123456789{0}", i.ToString()),
                    StartDate = new DateTime(2015, 9, 1),
                    EndDate = new DateTime(2015, 10, 1),
                    Multiple = 1,
                    Price = 2.99M,
                    PriceType = "SAL"
                };

                prices.Add(price);
            }

            return prices;
        }
    }
}
