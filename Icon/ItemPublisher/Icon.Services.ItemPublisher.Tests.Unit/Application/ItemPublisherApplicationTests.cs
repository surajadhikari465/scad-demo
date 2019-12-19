using Icon.Logging;
using Icon.Services.ItemPublisher.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Application.Tests
{
    /// <summary>
    /// This test runs the service and waits for a period of time and verifies the process method is called without exceptions.
    /// </summary>
    [TestClass()]
    public class ItemPublisherApplicationTests
    {
        [TestMethod]
        public async Task Start_CallsProcess_NoErrorsAreThrown()
        {
            // Given.
            Mock<IItemPublisherService> itmePublisherServiceMock = new Mock<IItemPublisherService>();
            Mock<ILogger<ItemPublisherApplication>> loggerMock = new Mock<ILogger<ItemPublisherApplication>>();
            itmePublisherServiceMock.Setup(x => x.Process(It.IsAny<int>()));

            ServiceSettings serviceSettings = new ServiceSettings()
            {
                BatchSize = 1,
                TimerIntervalInMilliseconds = 100
            };

            // When.
            ItemPublisherApplication application = new ItemPublisherApplication(itmePublisherServiceMock.Object, loggerMock.Object, serviceSettings);

            application.Start();
            await Task.Delay(500);

            // Then.
            itmePublisherServiceMock.Verify(x => x.Process(It.IsAny<int>()));
        }
    }
}