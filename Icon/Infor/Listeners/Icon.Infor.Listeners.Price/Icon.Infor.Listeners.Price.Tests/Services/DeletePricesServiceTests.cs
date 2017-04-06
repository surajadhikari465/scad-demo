using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Price.Constants;
using Icon.Infor.Listeners.Price.DataAccess.Commands;
using Icon.Infor.Listeners.Price.DataAccess.Models;
using Icon.Infor.Listeners.Price.Models;
using Icon.Infor.Listeners.Price.Services;
using Mammoth.Common.DataAccess.CommandQuery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Price.Tests.Services
{
    [TestClass]
    public class DeletePricesServiceTests
    {
        private DeletePricesService service;
        private Mock<ICommandHandler<DeletePricesCommand>> mockDeletePricesCommandHandler;
        private List<PriceModel> prices;
        private Mock<IEsbMessage> message;

        [TestInitialize]
        public void Initialize()
        {
            mockDeletePricesCommandHandler = new Mock<ICommandHandler<DeletePricesCommand>>();
            service = new DeletePricesService(mockDeletePricesCommandHandler.Object);

            prices = new List<PriceModel>();
            message = new Mock<IEsbMessage>();
        }

        [TestMethod]
        public void DeletePriceService_DeletePricesExist_CallsCommandHandler()
        {
            //Given
            prices.Add(new PriceModel { Action = Esb.Schemas.Wfm.ContractTypes.ActionEnum.Delete });

            //When
            service.Process(prices, message.Object);

            //Then
            mockDeletePricesCommandHandler.Verify(m => m.Execute(It.IsAny<DeletePricesCommand>()), Times.Once);
        }

        [TestMethod]
        public void DeletePriceService_DeletePricesDoNotExist_DoesNotCallCommandHandler()
        {
            //Given
            prices.Add(new PriceModel { Action = Esb.Schemas.Wfm.ContractTypes.ActionEnum.Add });

            //When
            service.Process(prices, message.Object);

            //Then
            mockDeletePricesCommandHandler.Verify(m => m.Execute(It.IsAny<DeletePricesCommand>()), Times.Never);
        }

        [TestMethod]
        public void DeletePriceService_SomePricesDoNotExist_SetsErrorPropertiesForPricesThatDoNotExist()
        {
            // Given

            // When

            // Then
            Assert.Fail("need to write test");
        }

        [TestMethod]
        public void DeletePriceService_SomePricesDoNotExist_ExecutesDeleteCommandHandlerForOnlyExistingPrices()
        {
            // Given

            // When

            // Then
            Assert.Fail("need to write test");
        }

        [TestMethod]
        public void DeletePriceService_PricesExistsToDelete_ExecutesGetPricesQueryOneTime()
        {
            // Given

            // When

            // Then
            Assert.Fail("need to write test");
        }
    }
}
