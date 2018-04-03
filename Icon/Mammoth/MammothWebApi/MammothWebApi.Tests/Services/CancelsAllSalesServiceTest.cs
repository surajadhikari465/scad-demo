using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using MammothWebApi.DataAccess.Commands;
using MammothWebApi.Service.Models;
using MammothWebApi.Service.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace MammothWebApi.Tests.Services
{
    [TestClass]
    public class CancelsAllSalesServiceTest
    {
        private Mock<ILogger> mockLogger;
        private Mock<ICommandHandler<CancelAllSalesCommand>> mockCancelAllSalesCommandHandler;
        private CancelAllSalesService cancelAllSalesService;

        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ILogger>();
            this.mockCancelAllSalesCommandHandler = new Mock<ICommandHandler<CancelAllSalesCommand>>();
            this.cancelAllSalesService = new CancelAllSalesService(
                this.mockLogger.Object,
                this.mockCancelAllSalesCommandHandler.Object
                );
        }

        [TestMethod]
        public void CancelsAllSalesService_CallHandleMethod_ShouldCallExecuteMethodOnce()
        { //Given
            CancelAllSales data = new CancelAllSales { CancelAllSalesData = new List<CancelAllSalesServiceModel>() };
         
            // When.
          this.cancelAllSalesService.Handle(data);

            //Then
            this.mockCancelAllSalesCommandHandler
                  .Verify(s => s.Execute(It.IsAny<CancelAllSalesCommand>()), Times.Once);
        }
    }
}