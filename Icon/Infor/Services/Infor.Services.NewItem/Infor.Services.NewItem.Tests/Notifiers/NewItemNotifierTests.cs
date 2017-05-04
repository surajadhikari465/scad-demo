using Infor.Services.NewItem.Notifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infor.Services.NewItem.Infrastructure;
using Icon.Logging;
using Moq;
using Infor.Services.NewItem.Models;
using Icon.Common.Email;
using Infor.Services.NewItem.Constants;

namespace Infor.Services.NewItem.Tests.Notifiers
{
    [TestClass]
    public class NewItemNotifierTests
    {
        private NewItemNotifier notifier;
        private NewItemNotifierSettings settings;
        private Mock<IRegionalEmailClientFactory> mockRegionalEmailClientFactory;
        private Mock<ILogger<NewItemNotifier>> mockLogger;
        private Mock<IEmailClient> mockEmailClient;
        private List<NewItemModel> newItems;

        [TestInitialize]
        public void Initialize()
        {
            settings = new NewItemNotifierSettings { RegionalNotificationEnabled = new Dictionary<string, bool>() };
            mockRegionalEmailClientFactory = new Mock<IRegionalEmailClientFactory>();
            mockLogger = new Mock<ILogger<NewItemNotifier>>();

            notifier = new NewItemNotifier(settings, mockRegionalEmailClientFactory.Object, mockLogger.Object);

            mockEmailClient = new Mock<IEmailClient>();
            newItems = new List<NewItemModel>();
        }

        [TestMethod]
        public void NotifyOfNewItemError_ErrorsDontExist_DoesNotSendEmail()
        {
            //Given
            newItems.Add(new NewItemModel());

            //When
            notifier.NotifyOfNewItemError(newItems);

            //Then
            mockRegionalEmailClientFactory.Verify(m => m.CreateEmailClient(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void NotifyOfNewItemError_ErrorsExistAndErrorIsATypeThatWeNotifyForAndRegionIsEnabled_SendsErrorEmail()
        {
            //Given
            settings.RegionalNotificationEnabled.Add("FL", true);
            newItems.Add(new NewItemModel { ScanCode = "1234", Region = "FL", ErrorCode = ApplicationErrors.Codes.InvalidBrand, ErrorDetails = "TestErrorDetails" });
            
            mockRegionalEmailClientFactory.Setup(m => m.CreateEmailClient(It.IsAny<string>()))
                .Returns(mockEmailClient.Object);

            //When
            notifier.NotifyOfNewItemError(newItems);

            //Then
            mockRegionalEmailClientFactory.Verify(m => m.CreateEmailClient(It.IsAny<string>()), Times.Once);
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockLogger.Verify(m => m.Info(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void NotifyOfNewItemError_ErrorsExistAndErrorIsATypeThatWeNotifyForButRegionIsNotEnabled_DoesNotSendsErrorEmail()
        {
            //Given
            settings.RegionalNotificationEnabled.Add("FL", false);
            newItems.Add(new NewItemModel { ScanCode = "1234", Region = "FL", ErrorCode = ApplicationErrors.Codes.InvalidBrand, ErrorDetails = "TestErrorDetails" });

            mockRegionalEmailClientFactory.Setup(m => m.CreateEmailClient(It.IsAny<string>()))
                .Returns(mockEmailClient.Object);

            //When
            notifier.NotifyOfNewItemError(newItems);

            //Then
            mockRegionalEmailClientFactory.Verify(m => m.CreateEmailClient(It.IsAny<string>()), Times.Never);
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            mockLogger.Verify(m => m.Info(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void NotifyOfNewItemError_ErrorsExistAndErrorIsATypeThatWeDontNotifyFor_DoesNotSendErrorEmail()
        {
            //Given
            settings.RegionalNotificationEnabled.Add("FL", true);
            newItems.Add(new NewItemModel { ScanCode = "1234", Region = "FL", ErrorCode = "TestErrorCode", ErrorDetails = "TestErrorDetails" });
            Mock<IEmailClient> mockEmailClient = new Mock<IEmailClient>();
            mockRegionalEmailClientFactory.Setup(m => m.CreateEmailClient(It.IsAny<string>()))
                .Returns(mockEmailClient.Object);

            //When
            notifier.NotifyOfNewItemError(newItems);

            //Then
            mockRegionalEmailClientFactory.Verify(m => m.CreateEmailClient(It.IsAny<string>()), Times.Never);
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            mockLogger.Verify(m => m.Info(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void NotifyOfNewItemError_ErrorsExistForDifferentRegions_SendsErrorEmailForDifferentRegions()
        {
            //Given
            settings.RegionalNotificationEnabled.Add("FL", true);
            settings.RegionalNotificationEnabled.Add("MW", true);
            newItems.Add(new NewItemModel { ScanCode = "1234", Region = "FL", ErrorCode = ApplicationErrors.Codes.InvalidBrand, ErrorDetails = "TestErrorDetails" });
            newItems.Add(new NewItemModel { ScanCode = "1234", Region = "MW", ErrorCode = ApplicationErrors.Codes.InvalidBrand, ErrorDetails = "TestErrorDetails" });
            Mock<IEmailClient> mockEmailClient = new Mock<IEmailClient>();
            mockRegionalEmailClientFactory.Setup(m => m.CreateEmailClient(It.IsAny<string>()))
                .Returns(mockEmailClient.Object);

            //When
            notifier.NotifyOfNewItemError(newItems);

            //Then
            mockRegionalEmailClientFactory.Verify(m => m.CreateEmailClient(It.IsAny<string>()), Times.Exactly(2));
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            mockLogger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void NotifyOfNewItemError_ErrorOccursWhenTryingToSendNotification_LogsError()
        {
            //Given
            settings.RegionalNotificationEnabled.Add("FL", true);
            newItems.Add(new NewItemModel { ScanCode = "1234", Region = "FL", ErrorCode = ApplicationErrors.Codes.InvalidBrand, ErrorDetails = "TestErrorDetails" });

            mockRegionalEmailClientFactory.Setup(m => m.CreateEmailClient(It.IsAny<string>()))
                .Throws(new Exception());

            //When
            notifier.NotifyOfNewItemError(newItems);

            //Then
            mockRegionalEmailClientFactory.Verify(m => m.CreateEmailClient(It.IsAny<string>()), Times.Once);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
        }
    }
}
