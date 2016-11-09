using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
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
    public class AddUpdatePriceServiceTests
    {
        private AddUpdatePriceService priceService;
        private Mock<ILogger> logger;
        private Mock<ICommandHandler<StagingPriceCommand>> mockStagingPriceCommand;
        private Mock<ICommandHandler<AddOrUpdatePricesCommand>> mockAddUpdatePriceCommand;
        private Mock<ICommandHandler<DeleteStagingCommand>> mockDeleteStagingCommand;
        private Mock<ICommandHandler<AddEsbMessageQueuePriceCommand>> mockEsbPriceCommand;

        [TestInitialize]
        public void InitializeTests()
        {
            this.logger = new Mock<ILogger>();
            this.mockStagingPriceCommand = new Mock<ICommandHandler<StagingPriceCommand>>();
            this.mockAddUpdatePriceCommand = new Mock<ICommandHandler<AddOrUpdatePricesCommand>>();
            this.mockDeleteStagingCommand = new Mock<ICommandHandler<DeleteStagingCommand>>();
            this.mockEsbPriceCommand = new Mock<ICommandHandler<AddEsbMessageQueuePriceCommand>>();
            this.priceService = new AddUpdatePriceService(
                this.logger.Object,
                this.mockStagingPriceCommand.Object,
                this.mockAddUpdatePriceCommand.Object,
                this.mockDeleteStagingCommand.Object,
                this.mockEsbPriceCommand.Object);
        }

        [TestMethod]
        public void AddUpdatePriceService_ValidServiceModel_StagingPriceCommandCalledOnce()
        {
            // Given
            AddUpdatePrice priceData = new AddUpdatePrice();
            priceData.Prices = BuildRegPricesServiceModelList(numberOfItems: 3, region: "SW");

            // When
            this.priceService.Handle(priceData);

            // Then
            this.mockStagingPriceCommand.Verify(s => s.Execute(It.IsAny<StagingPriceCommand>()), Times.Once);
        }

        [TestMethod]
        public void AddUpdatePriceService_ServiceModelHasOneRegion_AddUpdatePriceCommandHandlerCalledOnce()
        {
            // Given
            AddUpdatePrice priceData = new AddUpdatePrice();
            priceData.Prices = BuildRegPricesServiceModelList(numberOfItems: 3, region: "SW");

            // When
            this.priceService.Handle(priceData);

            // Then
            this.mockAddUpdatePriceCommand.Verify(r => r.Execute(It.Is<AddOrUpdatePricesCommand>(c =>
                c.Region == priceData.Prices.First().Region)),
                Times.Once);
        }

        [TestMethod]
        public void AddUpdatePriceService_ServiceModelHasMoreThanOneRegion_AddUpdatePriceCommandHandlerCalledOncePerRegion()
        {
            // Given
            AddUpdatePrice priceData = new AddUpdatePrice();
            priceData.Prices = BuildRegPricesServiceModelList(numberOfItems: 3, region: "SW");
            priceData.Prices.AddRange(BuildRegPricesServiceModelList(numberOfItems: 3, region: "FL"));
            priceData.Prices.AddRange(BuildRegPricesServiceModelList(numberOfItems: 3, region: "MW"));
            int expectedTimes = priceData.Prices.Select(p => p.Region).Distinct().Count();

            // When
            this.priceService.Handle(priceData);

            // Then
            this.mockAddUpdatePriceCommand.Verify(r => r.Execute(It.IsAny<AddOrUpdatePricesCommand>()), Times.Exactly(expectedTimes));
        }

        [TestMethod]
        public void AddUpdatePriceService_ValidServiceModel_DeleteStagingPriceCommandCalledOnce()
        {
            // Given
            AddUpdatePrice priceData = new AddUpdatePrice();
            priceData.Prices = BuildRegPricesServiceModelList(numberOfItems: 3, region: "SW");

            // When
            this.priceService.Handle(priceData);

            // Then
            this.mockDeleteStagingCommand.Verify(s => s.Execute(It.IsAny<DeleteStagingCommand>()), Times.Once);
        }

        [TestMethod]
        public void AddUpdatePriceService_PriceServiceModelWithOneRegion_AddEsbMessageQueuePriceCalledOnce()
        {
            // Given
            DateTime now = DateTime.Now;
            AddUpdatePrice priceData = new AddUpdatePrice();
            priceData.Prices = BuildRegPricesServiceModelList(numberOfItems: 3, region: "SW");

            // When
            this.priceService.Handle(priceData);

            // Then
            this.mockEsbPriceCommand.Verify(e => e.Execute(It.Is<AddEsbMessageQueuePriceCommand>(c => c.Region == "SW" && c.Timestamp >= now)), Times.Once);
        }

        [TestMethod]
        public void AddUpdatePriceService_PriceServiceModelWithMultipleRegions_AddEsbMessageQueuePriceCalledOncePerRegion()
        {
            // Given
            DateTime now = DateTime.Now;
            AddUpdatePrice priceData = new AddUpdatePrice();
            priceData.Prices = BuildRegPricesServiceModelList(numberOfItems: 3, region: "SW");
            priceData.Prices.AddRange(BuildRegPricesServiceModelList(numberOfItems: 3, region: "FL"));
            priceData.Prices.AddRange(BuildRegPricesServiceModelList(numberOfItems: 3, region: "MW"));
            int expectedTimes = priceData.Prices.Select(p => p.Region).Distinct().Count();

            // When
            this.priceService.Handle(priceData);

            // Then
            this.mockEsbPriceCommand.Verify(e => e.Execute(It.Is<AddEsbMessageQueuePriceCommand>(c => 
                c.Region == "SW" && c.Timestamp >= now)), Times.Once);
            this.mockEsbPriceCommand.Verify(e => e.Execute(It.Is<AddEsbMessageQueuePriceCommand>(c =>
                c.Region == "FL" && c.Timestamp >= now)), Times.Once);
            this.mockEsbPriceCommand.Verify(e => e.Execute(It.Is<AddEsbMessageQueuePriceCommand>(c =>
                c.Region == "MW" && c.Timestamp >= now)), Times.Once);
        }

        [TestMethod]
        public void AddUpdatePriceService_AddUpdatePriceCommandThrowsException_DataDeletedFromStaging()
        {
            // Given
            this.mockAddUpdatePriceCommand.Setup(c => c.Execute(It.IsAny<AddOrUpdatePricesCommand>()))
                .Throws<InvalidOperationException>();
            AddUpdatePrice priceData = new AddUpdatePrice();
            priceData.Prices = BuildRegPricesServiceModelList(numberOfItems: 3, region: "SW");

            // When
            try
            {
                this.priceService.Handle(priceData);
            }
            catch (Exception)
            {
                // Then
                this.mockDeleteStagingCommand
                    .Verify(sc => sc.Execute(It.IsAny<DeleteStagingCommand>()), Times.Once);
            }
        }

        [TestMethod]
        public void AddUpdatePriceService_AddEsbMessageQueuePriceCommandThrowsException_DataDeletedFromStaging()
        {
            // Given
            this.mockEsbPriceCommand.Setup(c => c.Execute(It.IsAny<AddEsbMessageQueuePriceCommand>()))
                .Throws<InvalidOperationException>();
            AddUpdatePrice priceData = new AddUpdatePrice();
            priceData.Prices = BuildRegPricesServiceModelList(numberOfItems: 3, region: "SW");

            // When
            try
            {
                this.priceService.Handle(priceData);
            }
            catch (Exception)
            {
                // Then
                this.mockDeleteStagingCommand
                    .Verify(sc => sc.Execute(It.IsAny<DeleteStagingCommand>()), Times.Once);
            }
        }

        [TestMethod]
        public void AddUpdatePriceService_AddEsbMessageQueuePriceCommandThrowsException_ThrowsOriginalException()
        {
            // Given
            string expectedExceptionMessage = "Test Exception";
            Exception expectedInnerException = new Exception("Test Inner Exception");
            Exception expectedException = new InvalidOperationException(expectedExceptionMessage, expectedInnerException);
            this.mockEsbPriceCommand.Setup(c => c.Execute(It.IsAny<AddEsbMessageQueuePriceCommand>()))
                .Throws(expectedException);

            AddUpdatePrice priceData = new AddUpdatePrice();
            priceData.Prices = BuildRegPricesServiceModelList(numberOfItems: 3, region: "SW");

            // When
            try
            {
                this.priceService.Handle(priceData);
            }
            catch (Exception actualException)
            {
                // Then
                Assert.AreEqual(expectedExceptionMessage, actualException.Message);
                Assert.AreSame(expectedException, actualException);
                Assert.AreEqual(expectedInnerException.Message, actualException.InnerException.Message);
                Assert.AreSame(expectedInnerException, actualException.InnerException);
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
