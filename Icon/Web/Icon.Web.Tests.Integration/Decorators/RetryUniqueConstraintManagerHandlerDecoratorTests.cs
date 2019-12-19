
using Icon.Logging;
using Icon.Web.DataAccess.Decorators;
using Icon.Web.DataAccess.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data.SqlClient;

namespace MammothWebApi.Tests.Service.Decorators
{
    [TestClass]
    public class RetryUniqueConstraintManagerHandlerDecoratorTests
    {
        private RetryUniqueConstraintManagerHandlerDecorator<object> decorator;
        private Mock<IManagerHandler<object>> managerHandler;
        private Mock<ILogger> logger;

        [TestInitialize]
        public void Initialize()
        {
            managerHandler = new Mock<IManagerHandler<object>>();
            logger = new Mock<ILogger>();
            decorator = new RetryUniqueConstraintManagerHandlerDecorator<object>(
                managerHandler.Object,
                logger.Object);
        }

        [TestMethod]
        public void RetryUniqueConstraintManagerHandlerDecorator_RetryCountIs3AndExceptionOccursOnAllExecutions_Executes4Times()
        {
            //Given
            managerHandler.Setup(m => m.Execute(It.IsAny<object>()))
                .Throws(SqlExceptionCreator.NewSqlException(2601));

            //When
            try
            {
                decorator.Execute(new object());
            }
            catch (Exception)
            {
               
            }

            //Then
            managerHandler.Verify(m => m.Execute(It.IsAny<object>()), Times.Exactly(4));
        }

        [TestMethod]
        public void RetryUniqueConstraintManagerHandlerDecorator_ExceptionDoesNotOccur_DoesNotRetryExecution()
        {
            //When
            decorator.Execute(new object());

            //Then
            managerHandler.Verify(m => m.Execute(It.IsAny<object>()), Times.Exactly(1));
        }
    }
}
