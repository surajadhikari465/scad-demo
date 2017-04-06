using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Price.DataAccess.Commands;
using Icon.Infor.Listeners.Price.Models;
using Icon.Infor.Listeners.Price.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using Mammoth.Common.DataAccess.CommandQuery;

namespace Icon.Infor.Listeners.Price.Tests.Services
{
    [TestClass]
    public class ReplacePricesServiceTests
    {
        //private ReplacePricesService service;
        //private Mock<ICommandHandler<ReplacePricesCommand>> mockReplacePricesCommandHandler;
        //private List<PriceModel> prices;
        //private Mock<IEsbMessage> message;

        //[TestInitialize]
        //public void Initialize()
        //{
        //    mockReplacePricesCommandHandler = new Mock<ICommandHandler<ReplacePricesCommand>>();
        //    service = new ReplacePricesService(mockReplacePricesCommandHandler.Object);

        //    prices = new List<PriceModel>();
        //    message = new Mock<IEsbMessage>();
        //}

        //[TestMethod]
        //public void ReplacePriceService_ReplacePricesExist_CallsCommandHandler()
        //{
        //    //Given
        //    prices.Add(new PriceModel { Action = Esb.Schemas.Wfm.ContractTypes.ActionEnum.Replace });

        //    //When
        //    service.Process(prices, message.Object);

        //    //Then
        //    mockReplacePricesCommandHandler.Verify(m => m.Execute(It.IsAny<ReplacePricesCommand>()), Times.Once);
        //}

        //[TestMethod]
        //public void ReplacePriceService_ReplacePricesDoNotExist_DoesNotCallCommandHandler()
        //{
        //    //Given
        //    prices.Add(new PriceModel { Action = Esb.Schemas.Wfm.ContractTypes.ActionEnum.Delete });

        //    //When
        //    service.Process(prices, message.Object);

        //    //Then
        //    mockReplacePricesCommandHandler.Verify(m => m.Execute(It.IsAny<ReplacePricesCommand>()), Times.Never);
        //}
    }
}
