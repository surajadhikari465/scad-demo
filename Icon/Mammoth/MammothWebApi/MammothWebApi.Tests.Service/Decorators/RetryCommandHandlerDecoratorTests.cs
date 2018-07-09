using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using MammothWebApi.DataAccess.Settings;
using MammothWebApi.Service.Decorators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace MammothWebApi.Tests.Service.Decorators
{
    [TestClass]
    public class RetryCommandHandlerDecoratorTests
    {
        private RetryCommandHandlerDecorator<object> decorator;
        private Mock<ICommandHandler<object>> commandHandler;
        private DataAccessSettings settings;
        private Mock<ILogger> logger;

        [TestInitialize]
        public void Initialize()
        {
            commandHandler = new Mock<ICommandHandler<object>>();
            settings = new DataAccessSettings { DatabaseRetryCount = 3 };
            logger = new Mock<ILogger>();
            decorator = new RetryCommandHandlerDecorator<object>(
                commandHandler.Object,
                settings,
                logger.Object);
        }

        [TestMethod]
        public void RetryCommandHandlerDecorator_RetryCountIs3AndExceptionOccursOnAllExecutions_Executes4Times()
        {
            //Given
            commandHandler.Setup(m => m.Execute(It.IsAny<object>()))
                .Throws(new Exception("Test"));

            //When
            try
            {
                decorator.Execute(new object());
            }
            catch(Exception ex)
            {
                Assert.AreEqual("Test", ex.Message);
            }

            //Then
            commandHandler.Verify(m => m.Execute(It.IsAny<object>()), Times.Exactly(4));
            logger.Verify(m => m.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Exactly(5));
        }

        [TestMethod]
        public void RetryCommandHandlerDecorator_RetryCountIs3AndExceptionOccursOnFirstExecution_Executes1Time()
        {
            //Given
            commandHandler.SetupSequence(m => m.Execute(It.IsAny<object>()))
                .Throws(new Exception("Test"))
                .Pass();

            //When
            try
            {
                decorator.Execute(new object());
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Test", ex.Message);
            }

            //Then
            commandHandler.Verify(m => m.Execute(It.IsAny<object>()), Times.Exactly(2));
            logger.Verify(m => m.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Exactly(1));
        }

        [TestMethod]
        public void RetryCommandHandlerDecorator_RetryCountIs3AndExceptionDoesNotOccur_DoesNotRetryExecution()
        {
            //When
            decorator.Execute(new object());

            //Then
            commandHandler.Verify(m => m.Execute(It.IsAny<object>()), Times.Exactly(1));
            logger.Verify(m => m.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
        }
    }
}
