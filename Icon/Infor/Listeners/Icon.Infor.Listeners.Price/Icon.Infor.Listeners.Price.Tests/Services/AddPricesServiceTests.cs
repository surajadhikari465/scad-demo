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
    public class AddPricesServiceTests
    {
        //private AddPricesServiceHandler service;
        //private Mock<ICommandHandler<AddPricesCommand>> mockAddPricesCommandHandler;
        //private List<PriceModel> prices;
        //private Mock<IEsbMessage> message;

        //[TestInitialize]
        //public void Initialize()
        //{
        //    mockAddPricesCommandHandler = new Mock<ICommandHandler<AddPricesCommand>>();
        //    service = new AddPricesServiceHandler(mockAddPricesCommandHandler.Object);

        //    prices = new List<PriceModel>();
        //    message = new Mock<IEsbMessage>();
        //}

        //[TestMethod]
        //public void AddPriceService_AddPricesExist_CallsCommandHandler()
        //{
        //    //Given
        //    prices.Add(new PriceModel { Action = Esb.Schemas.Wfm.ContractTypes.ActionEnum.Add });

        //    //When
        //    service.Process(prices, message.Object);

        //    //Then
        //    mockAddPricesCommandHandler.Verify(m => m.Execute(It.IsAny<AddPricesCommand>()), Times.Once);
        //}

        //[TestMethod]
        //public void AddPriceService_AddPricesDoNotExist_DoesNotCallCommandHandler()
        //{
        //    //Given
        //    prices.Add(new PriceModel { Action = Esb.Schemas.Wfm.ContractTypes.ActionEnum.Delete });

        //    //When
        //    service.Process(prices, message.Object);

        //    //Then
        //    mockAddPricesCommandHandler.Verify(m => m.Execute(It.IsAny<AddPricesCommand>()), Times.Never);
        //}
    }
}
