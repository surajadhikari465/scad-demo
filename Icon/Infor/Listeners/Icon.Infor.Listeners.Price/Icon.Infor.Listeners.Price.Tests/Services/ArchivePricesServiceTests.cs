using Icon.Common.DataAccess;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Price.DataAccess.Commands;
using Icon.Infor.Listeners.Price.DataAccess.Models;
using Icon.Infor.Listeners.Price.Models;
using Icon.Infor.Listeners.Price.Services;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Icon.Infor.Listeners.Price.Tests.Services
{
    [TestClass]
    public class ArchivePricesServiceTests
    {
        //private ArchivePricesService service;
        //private Mock<ICommandHandler<ArchivePricesCommand>> mockArchivePricesCommandHandler;
        //private Mock<ILogger<ArchivePricesService>> mockLogger;
        //private List<PriceModel> prices;
        //private Mock<IEsbMessage> message;

        //[TestInitialize]
        //public void Initialize()
        //{
        //    mockArchivePricesCommandHandler = new Mock<ICommandHandler<ArchivePricesCommand>>();
        //    mockLogger = new Mock<ILogger<ArchivePricesService>>();
        //    service = new ArchivePricesService(mockArchivePricesCommandHandler.Object, mockLogger.Object);

        //    prices = new List<PriceModel>();
        //    message = new Mock<IEsbMessage>();
        //}

        //[TestMethod]
        //public void ArchivePriceService_ArchivePricesExist_CallsCommandHandler()
        //{
        //    //Given
        //    prices.Add(new PriceModel());

        //    //When
        //    service.Process(prices, message.Object);

        //    //Then
        //    mockArchivePricesCommandHandler.Verify(m => m.Execute(It.IsAny<ArchivePricesCommand>()), Times.Once);
        //}

        //[TestMethod]
        //public void ArchivePriceService_ArchivePricesDoNotExist_DoesNotCallCommandHandler()
        //{
        //    //When
        //    service.Process(prices, message.Object);

        //    //Then
        //    mockArchivePricesCommandHandler.Verify(m => m.Execute(It.IsAny<ArchivePricesCommand>()), Times.Never);
        //}
    }
}
