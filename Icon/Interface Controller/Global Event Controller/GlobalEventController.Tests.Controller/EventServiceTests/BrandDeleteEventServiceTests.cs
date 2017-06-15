using GlobalEventController.Common;
using GlobalEventController.Controller.EventServices;
using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using GlobalEventController.Testing.Common;
using Icon.Common;
using Icon.Common.Email;
using Icon.Logging;
using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using static GlobalEventController.DataAccess.Commands.BrandDeleteCommand;

namespace GlobalEventController.Tests.Controller.EventServiceTests
{
    [TestClass]
    public class BrandDeleteEventServiceTests
    {
        private IrmaContext irmaContext;
        private IEventService brandDeleteEventService;
        private Mock<ICommandHandler<BrandDeleteCommand>> mockDeleteCommandHandler;
        private Mock<IQueryHandler<GetIrmaBrandQuery, ItemBrand>> mockGetBrandQueryHandler;
        private Mock<ILogger<BrandDeleteEventService>> mockLogger;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IGlobalControllerSettings> mockGloconConfiguration;

        int iconBrandId = TestingConstants.IconBrandId_Positive;
        int irmaBrandId = TestingConstants.IrmaBrandId_Negative;
        string region = TestingConstants.TestRegion;
        string brandName = TestingConstants.BrandNameX;
        private const string expectedEmailSubject = "Something Interesting Happened";

        [TestInitialize]
        public void InitializeData()
        {
            irmaContext = new IrmaContext();

            mockDeleteCommandHandler = new Mock<ICommandHandler<BrandDeleteCommand>>();
            mockGetBrandQueryHandler = new Mock<IQueryHandler<GetIrmaBrandQuery, ItemBrand>>();
            mockLogger = new Mock<ILogger<BrandDeleteEventService>>();
            mockEmailClient = new Mock<IEmailClient>();
            mockGloconConfiguration = new Mock<IGlobalControllerSettings>();

            brandDeleteEventService = new BrandDeleteEventService(
                irmaContext,
                mockDeleteCommandHandler.Object,
                mockGetBrandQueryHandler.Object,
                mockLogger.Object,
                mockEmailClient.Object,
                mockGloconConfiguration.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            irmaContext.Dispose();
        }

        #region class-specific helper methods

        private string GetExpectedMessageForBrandDeleteStillAssociatedWithItems(
            string brandName, string region, int numOfAssociatedItems, int iconBrandId, int irmaBrandId)
        {
            var msg = $"Brand \"{brandName}\" in Region {region}" +
                " could not be deleted because it is still associated with" +
                $" {numOfAssociatedItems} " +
                (numOfAssociatedItems > 1 ? "items" : "item") +
                " in the IRMA database." +
                $" (IconBrandId = {iconBrandId}" +
                $" IrmaBrandId = {irmaBrandId}).";

            return msg;
        }

        private void SetMockDeleteCommandResult(Mock<ICommandHandler<BrandDeleteCommand>> mockCommandHandler,
            BrandDeleteResult desiredResult)
        {
            mockCommandHandler.Setup(m => m.Handle(It.IsAny<BrandDeleteCommand>()))
                .Callback<BrandDeleteCommand>((cmd) => cmd.Result = desiredResult);
        }

        private void SetMockDeleteCommandResult(BrandDeleteResult desiredResult)
        {
            SetMockDeleteCommandResult(this.mockDeleteCommandHandler, desiredResult);
        }

        private void SetMockBrandSubqueryResult(Mock<IQueryHandler<GetIrmaBrandQuery, ItemBrand>> mockQueryHandler,
            ItemBrand brandToReturn, int desiredItemCount)
        {
            // use a Moq callback to have the mocked delete command set the Result property
            mockQueryHandler.Setup(m => m.Handle(It.IsAny<GetIrmaBrandQuery>()))
                .Callback<GetIrmaBrandQuery>((q) => q.ResultItemCount = desiredItemCount)
                .Returns(brandToReturn);
        }

        private void SetMockBrandSubqueryResult(Mock<IQueryHandler<GetIrmaBrandQuery, ItemBrand>> mockQueryHandler,
            int irmaBrandId, string brandName, int desiredItemCount)
        {
            var itemBrand = new ItemBrand
            {
                Brand_ID = TestingConstants.IrmaBrandId_Negative,
                Brand_Name = TestingConstants.BrandNameX
            };
            SetMockBrandSubqueryResult(mockQueryHandler, itemBrand, desiredItemCount);
        }

        private void SetMockBrandSubqueryResult(int irmaBrandId, string brandName, int desiredItemCount)
        {
            SetMockBrandSubqueryResult(this.mockGetBrandQueryHandler, irmaBrandId, brandName, desiredItemCount);
        }
        private void SetMockEmailClientSettings(bool emailEnabled)
        {
            SetMockEmailClientSettings(this.mockGloconConfiguration, emailEnabled, expectedEmailSubject);
        }

        private void SetMockEmailClientSettings( bool emailEnabled, string emailSubject)
        {
            SetMockEmailClientSettings(this.mockGloconConfiguration, emailEnabled, emailSubject);
        }

        private void SetMockEmailClientSettings(Mock<IGlobalControllerSettings> mockSettings, bool emailEnabled, string emailSubject)
        {
            mockSettings.SetupGet(c => c.BrandDeleteEmailAlertsEnabled)
                .Returns(emailEnabled);
            mockSettings.SetupGet(c => c.BrandDeleteEmailSubject)
                .Returns(emailSubject);
        }

        private void SetServiceProperties(IEventService service, int referenceId, string region, string message)
        {
            service.ReferenceId = referenceId;
            service.Region = region;
            service.Message = message;
        }

        private void SetServiceProperties(int referenceId, string region, string message)
        {
            SetServiceProperties(this.brandDeleteEventService, referenceId, region, message);
        }
        #endregion

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BrandDeleteEventServiceRun_WhenArgumentReferenceIdIsNull_ArgumentExceptionThrown()
        {
            //Given
            brandDeleteEventService.ReferenceId = null;
            brandDeleteEventService.Message = "TestMessage";
            brandDeleteEventService.Region = "TestRegion";

            //When
            brandDeleteEventService.Run();

            //Then
            //Should get ArgumentException
        }

        [TestMethod]
        public void BrandDeleteEventServiceRun_WhenArgumentReferenceIdIsNull_ExceptionMessageMatchesExpected()
        {
            //Given
            brandDeleteEventService.ReferenceId = null;
            brandDeleteEventService.Message = "TestMessage";
            brandDeleteEventService.Region = "TestRegion";

            //When
            try
            {
                brandDeleteEventService.Run();
            }

            //Then
            catch (ArgumentException argEx)
            {
                string expectedMsg = "BrandDeleteEventService was called with invalid arguments." +
                    " ReferenceId must be greater than 0. Region and Message must not be null or empty." +
                    " ReferenceId = , Message = 'TestMessage', Region = 'TestRegion'";
                Assert.AreEqual(expectedMsg, argEx.Message);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BrandDeleteEventServiceRun_WhenArgumentReferenceIdIsNegative_ArgumentExceptionThrown()
        {
            //Given
            brandDeleteEventService.ReferenceId = -1;
            brandDeleteEventService.Message = "TestMessage";
            brandDeleteEventService.Region = "TestRegion";

            //When
            brandDeleteEventService.Run();

            //Then
            //Should get ArgumentException   
        }

        [TestMethod]
        public void BrandDeleteEventServiceRun_WhenArgumentReferenceIdIsNegative_ExceptionMessageMatchesExpected()
        {
            //Given
            brandDeleteEventService.ReferenceId = -1;
            brandDeleteEventService.Message = "TestMessage";
            brandDeleteEventService.Region = "TestRegion";
            string expectedMsg = "BrandDeleteEventService was called with invalid arguments." +
                " ReferenceId must be greater than 0. Region and Message must not be null or empty." +
                " ReferenceId = -1, Message = 'TestMessage', Region = 'TestRegion'";

            //When
            try
            {
                brandDeleteEventService.Run();
            }

            //Then
            catch (ArgumentException argEx)
            {
                Assert.AreEqual(expectedMsg, argEx.Message);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BrandDeleteEventServiceRun_WhenArgumentMessageIsNullOrEmpty_ArgumentExceptionThrown()
        {
            //Given
            brandDeleteEventService.ReferenceId = 1;
            brandDeleteEventService.Message = null;
            brandDeleteEventService.Region = "TestRegion";

            //When
            brandDeleteEventService.Run();

            //Then
            //Should get ArgumentException
        }

        [TestMethod]
        public void BrandDeleteEventServiceRun_WhenArgumentMessageIsNullOrEmpty_ExceptionMessageMatchesExpected()
        {
            //Given
            brandDeleteEventService.ReferenceId = 1;
            brandDeleteEventService.Message = null;
            brandDeleteEventService.Region = "TestRegion";
            string expectedMsg = "BrandDeleteEventService was called with invalid arguments." +
                " ReferenceId must be greater than 0. Region and Message must not be null or empty." +
                " ReferenceId = 1, Message = '', Region = 'TestRegion'";

            //When
            try
            {
                brandDeleteEventService.Run();
            }

            //Then
            catch (ArgumentException argEx)
            {
                Assert.AreEqual(expectedMsg, argEx.Message);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BrandDeleteEventServiceRun_WhenArgumentRegionIsNullOrEmpty_ArgumentExceptionThrown()
        {
            //Given
            brandDeleteEventService.ReferenceId = 1;
            brandDeleteEventService.Message = "TestMessage";
            brandDeleteEventService.Region = null;

            //When
            brandDeleteEventService.Run();

            //Then
            //Should get ArgumentException
        }

        [TestMethod]
        public void BrandDeleteEventServiceRun_WhenArgumentRegionIsNullOrEmpty_ExceptionMessageMatchesExpected()
        {
            //Given
            brandDeleteEventService.ReferenceId = 1;
            brandDeleteEventService.Message = "TestMessage";
            brandDeleteEventService.Region = null;
            string expectedMsg = "BrandDeleteEventService was called with invalid arguments." +
                " ReferenceId must be greater than 0. Region and Message must not be null or empty." +
                " ReferenceId = 1, Message = 'TestMessage', Region = ''";

            //When
            try
            {
                brandDeleteEventService.Run();
            }

            //Then
            catch (ArgumentException argEx)
            {
                Assert.AreEqual(expectedMsg, argEx.Message);
            }
        }

        [TestMethod]
        public void BrandDeleteEventServiceRun_WhenArgumentsAreValid_CommandHandlerCalledOnce()
        {
            //Given
            mockDeleteCommandHandler.Setup(q => q.Handle(It.IsAny<BrandDeleteCommand>()));

            brandDeleteEventService.ReferenceId = 1;
            brandDeleteEventService.Message = "TestBrandName";
            brandDeleteEventService.Region = "TestRegion";

            //When
            brandDeleteEventService.Run();

            //Then         
            mockDeleteCommandHandler.Verify(command => command.Handle(It.IsAny<BrandDeleteCommand>()), Times.Once);
        }

        [TestMethod]
        public void BrandDeleteEventServiceRun_WhenArgumentsAreValid_CommandHandlerCalledWithExpectedParameters()
        {
            //Given
            mockDeleteCommandHandler.Setup(q => q.Handle(It.IsAny<BrandDeleteCommand>()));

            brandDeleteEventService.ReferenceId = 1;
            brandDeleteEventService.Message = "TestBrandName";
            brandDeleteEventService.Region = "TestRegion";

            //When
            brandDeleteEventService.Run();

            //Then         
            mockDeleteCommandHandler.Verify(command => command.Handle(It.Is<BrandDeleteCommand>(
                (cmd) => cmd.Region == "TestRegion" && cmd.IconBrandId == 1)),
                Times.Once);
        }

        [TestMethod]
        public void BrandDeleteEventServiceRun_WhenArgumentsAreValid_AndBrandFound_DoesNotLog()
        {
            // Given
            brandDeleteEventService.ReferenceId = this.iconBrandId;
            brandDeleteEventService.Region = this.region;
            brandDeleteEventService.Message = this.brandName;

            // use a Moq callback to have the mocked delete command set the Result property,
            // so that the service class sees the command handler as having successfully completed
            mockDeleteCommandHandler.Setup(m => m.Handle(It.IsAny<BrandDeleteCommand>()))
                .Callback<BrandDeleteCommand>((cmd) => 
                    cmd.Result = BrandDeleteResult.ValidatedAndItemBrandsDeleted);

            //When
            brandDeleteEventService.Run();

            //Then
            mockLogger.Verify(log => log.Error(It.IsAny<string>()), Times.Never,
                "Logging Error was not expected to be called.");
        }

        [TestMethod]
        public void BrandDeleteEventServiceRun_WhenBrandNotFound_DoesNotThrow()
        {
            // Given
            SetServiceProperties(this.iconBrandId, this.region, this.brandName);
            // use a Moq callback to have the mocked delete command set the Result property,
            // so that the service class sees the command handler as having not found a match
            mockDeleteCommandHandler.Setup(m => m.Handle(It.IsAny<BrandDeleteCommand>()))
                .Callback<BrandDeleteCommand>((cmd) =>
                    cmd.Result = BrandDeleteResult.NothingDeleted);

            // When
            try
            {
                brandDeleteEventService.Run();
            }
            catch (Exception ex)
            {
                // Then
                Assert.Fail("Did not expect to encounter exception: ", ex);
            }
        }

        [TestMethod]
        public void BrandDeleteEventServiceRun_WhenArgumentsAreValid_ButBrandNotFound_LogsError()
        {
            // Given
            SetServiceProperties(this.iconBrandId, this.region, this.brandName);

            // use a Moq callback to have the mocked delete command set the Result property,
            // so that the service class sees the command handler as having not found a match
            mockDeleteCommandHandler.Setup(m => m.Handle(It.IsAny<BrandDeleteCommand>()))
                .Callback<BrandDeleteCommand>((cmd) =>
                    cmd.Result = BrandDeleteResult.NothingDeleted);
            
            // When
            brandDeleteEventService.Run();

            // Then
            mockLogger.Verify(log => log.Error(It.IsAny<string>()), Times.Once,
                "Expected error condition to trigger logging an error");
        }

        [TestMethod]
        public void BrandDeleteEventServiceRun_WhenBrandNotFound_LogsError_WithExpectedMessage()
        {
            // Given
            SetServiceProperties(this.iconBrandId, this.region, this.brandName);
            string expectedMsg = "This brand was not found in the IRMA ValidatedBrand table so it will not be deleted:" +
                $" IconBrandId = {brandDeleteEventService.ReferenceId}, Region = '{brandDeleteEventService.Region}'";

            // use a Moq callback to have the mocked delete command set the Result property,
            // so that the service class sees the command handler as having not found a match
            mockDeleteCommandHandler.Setup(m => m.Handle(It.IsAny<BrandDeleteCommand>()))
                .Callback<BrandDeleteCommand>((cmd) =>
                    cmd.Result = BrandDeleteResult.NothingDeleted);

            // When
            brandDeleteEventService.Run();

            // Then
            mockLogger.Verify(log => log.Error(It.IsAny<string>()),
                Times.Once);
            mockLogger.Verify(log => log.Error(It.Is<string>(s=> s == expectedMsg)),
                Times.Once,
                "Expected error condition to trigger logging an error with a specific message");
        }

        [TestMethod]
        public void BrandDeleteEventServiceRun_WhenBrandStillAssociatedToItem_QueriesForDetailsToReport()
        {
            // Given
            SetServiceProperties(this.iconBrandId, this.region, this.brandName);

            // use a Moq callback to have the mocked delete command set the Result property
            mockDeleteCommandHandler.Setup(m => m.Handle(It.IsAny<BrandDeleteCommand>()))
                .Callback<BrandDeleteCommand>((cmd) =>
                    cmd.Result = BrandDeleteResult.ValidatedBrandDeletedButItemBrandAssociatedWithItems);

            // When
            brandDeleteEventService.Run();

            // Then
            mockGetBrandQueryHandler.Verify(m => m.Handle(It.IsAny<GetIrmaBrandQuery>()),
                Times.Once);
        }

        [TestMethod]
        public void BrandDeleteEventServiceRun_WhenBrandStillAssociatedToItem_QueriesForDetailsToReport_WithExpectedParameters()
        {
            // Given
            SetServiceProperties(this.iconBrandId, this.region, this.brandName);

            // use a Moq callback to have the mocked delete command set the Result property
            mockDeleteCommandHandler.Setup(m => m.Handle(It.IsAny<BrandDeleteCommand>()))
                .Callback<BrandDeleteCommand>((cmd) =>
                    cmd.Result = BrandDeleteResult.ValidatedBrandDeletedButItemBrandAssociatedWithItems);

            // When
            brandDeleteEventService.Run();

            // Then
            mockGetBrandQueryHandler.Verify(m => m.Handle(
                It.Is<GetIrmaBrandQuery>(x=>x.IconBrandId==TestingConstants.IconBrandId_Positive)),
                Times.Once);
        }

        [TestMethod]
        public void BrandDeleteEventServiceRun_WhenBrandStillAssociatedToItem_LogsWarning()
        {
            // Given
            SetServiceProperties(this.iconBrandId, this.region, this.brandName);

            // use a Moq callback to have the mocked delete command set the Result property
            mockDeleteCommandHandler.Setup(m => m.Handle(It.IsAny<BrandDeleteCommand>()))
                .Callback<BrandDeleteCommand>((cmd) =>
                    cmd.Result = BrandDeleteResult.ValidatedBrandDeletedButItemBrandAssociatedWithItems);

            // When
            brandDeleteEventService.Run();

            // Then
            mockLogger.Verify(log => log.Warn(It.IsAny<string>()),
                Times.Once,
                "Expected error condition to trigger logging a warning");
        } 

        [TestMethod]
        public void BrandDeleteEventServiceRun_WhenBrandStillAssociatedToSingleItem_LogsWarningWithExpectedMessage()
        {
            // Given
            SetServiceProperties(this.iconBrandId, this.region, this.brandName);
            int numberItemsAssociated = 1;
            string expectedMsg = GetExpectedMessageForBrandDeleteStillAssociatedWithItems(
                brandName, region, numberItemsAssociated, iconBrandId, irmaBrandId);
            // have the mocked delete command set the Result property
            SetMockDeleteCommandResult(
                mockDeleteCommandHandler, BrandDeleteResult.ValidatedBrandDeletedButItemBrandAssociatedWithItems);
            //  simulate finding item(s) still associated with the brand
            SetMockBrandSubqueryResult(
                TestingConstants.IrmaBrandId_Negative, TestingConstants.BrandNameX, numberItemsAssociated);

            // When
            brandDeleteEventService.Run();

            // Then
            mockLogger.Verify(log => log.Warn(It.Is<string>(s => s == expectedMsg)),
                Times.Once,
                "Expected error condition to trigger logging a warning with a specific message");
        }

        [TestMethod]
        public void BrandDeleteEventServiceRun_WhenBrandStillAssociatedToMultipleItems_LogsWarningWithExpectedMessage()
        {
            // Given
            SetServiceProperties(this.iconBrandId, this.region, this.brandName);
            int numberItemsAssociated = 3;
            string expectedMsg = GetExpectedMessageForBrandDeleteStillAssociatedWithItems(
                brandName, region, numberItemsAssociated, iconBrandId, irmaBrandId);
            //string expectedMsg = $"Brand {TestingConstants.BrandNameX} in Region {TestingConstants.TestRegion}" +
            //    " could not be deleted because it is still associated with" +
            //    $" {numberItemsAssociated} items in the IRMA database." +
            //    $" (IconBrandId = {TestingConstants.IconBrandId_Positive}" +
            //    $" IrmaBrandId = {TestingConstants.IrmaBrandId_Negative}).";

            // have the mocked delete command set the Result property
            SetMockDeleteCommandResult(mockDeleteCommandHandler, BrandDeleteResult.ValidatedBrandDeletedButItemBrandAssociatedWithItems);
            //  simulate finding item(s) still associated with the brand
            SetMockBrandSubqueryResult(TestingConstants.IrmaBrandId_Negative, TestingConstants.BrandNameX, numberItemsAssociated);


            // When
            brandDeleteEventService.Run();

            //Then
            mockLogger.Verify(log => log.Warn(It.Is<string>(s => s == expectedMsg)),
                Times.Once,
                "Expected error condition to trigger logging a warning with a specific message");
        }

        [TestMethod]
        public void BrandDeleteEventServiceRun_WhenBrandStillAssociatedToItem_AndAlertingEnabled_SendsAlert()
        {
            // Given
            SetServiceProperties(this.iconBrandId, this.region, this.brandName);
            bool emailEnabled = true;
            int numberItemsAssociated = 1;
            string expectedMsg = GetExpectedMessageForBrandDeleteStillAssociatedWithItems(
                brandName, region, numberItemsAssociated, iconBrandId, irmaBrandId);
            // have the mocked delete command set the Result property
            SetMockDeleteCommandResult(mockDeleteCommandHandler, BrandDeleteResult.ValidatedBrandDeletedButItemBrandAssociatedWithItems);
            //  simulate finding item(s) still associated with the brand
            SetMockBrandSubqueryResult(TestingConstants.IrmaBrandId_Negative, TestingConstants.BrandNameX, numberItemsAssociated);
            // make sure alerting seems to be enabled
            SetMockEmailClientSettings(emailEnabled, expectedEmailSubject);

            // When
            brandDeleteEventService.Run();

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()),
                Times.Once,
                "expected [mock] email to be sent");
        }

        [TestMethod]
        public void BrandDeleteEventServiceRun_WhenBrandStillAssociatedToMultipleItems_AndAlertingEnabled_SendsAlert()
        {
            // Given
            SetServiceProperties(this.iconBrandId, this.region, this.brandName);
            bool emailEnabled = true;
            int numberItemsAssociated = 3;
            string expectedMsg = GetExpectedMessageForBrandDeleteStillAssociatedWithItems(
                brandName, region, numberItemsAssociated, iconBrandId, irmaBrandId);

            // have the mocked delete command set the Result propertyy
            SetMockDeleteCommandResult(mockDeleteCommandHandler, BrandDeleteResult.ValidatedBrandDeletedButItemBrandAssociatedWithItems);
            //  simulate finding item(s) still associated with the brand
            SetMockBrandSubqueryResult(TestingConstants.IrmaBrandId_Negative, TestingConstants.BrandNameX, numberItemsAssociated);
            // make sure alerting seems to be enabled
            SetMockEmailClientSettings(emailEnabled);

            // When
            brandDeleteEventService.Run();

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()),
                Times.Once,
                "expected [mock] email to be sent");
        }

        [TestMethod]
        public void BrandDeleteEventServiceRun_WhenBrandStillAssociatedToItem_AndAlertingDisabled_DoesNotSendAlert()
        {
            // Given
            SetServiceProperties(this.iconBrandId, this.region, this.brandName);
            int numberItemsAssociated = 1;
            bool emailEnabled = false;
            string expectedMsg = GetExpectedMessageForBrandDeleteStillAssociatedWithItems(
                brandName, region, numberItemsAssociated, iconBrandId, irmaBrandId);
            // have the mocked delete command set the Result property
            SetMockDeleteCommandResult(BrandDeleteResult.ValidatedBrandDeletedButItemBrandAssociatedWithItems);
            // simulate mocked sub-query to find item(s) still associated with the brand
            SetMockBrandSubqueryResult(TestingConstants.IrmaBrandId_Negative, TestingConstants.BrandNameX, numberItemsAssociated);
            // make sure alerting seems to be disabled
            SetMockEmailClientSettings(emailEnabled);

            // When
            brandDeleteEventService.Run();

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never,
                "did not expect [mock] email to be sent");
        }

        [TestMethod]
        public void BrandDeleteEventServiceRun_WhenBrandStillAssociatedToItem_AndAlertingEnabled_SendsAlertWithExpectedMessage()
        {
            // Given
            SetServiceProperties(this.iconBrandId, this.region, this.brandName);
            int numberItemsAssociated = 1;
            string expectedMsg = GetExpectedMessageForBrandDeleteStillAssociatedWithItems(
                this.brandName, this.region, numberItemsAssociated, this.iconBrandId, this.irmaBrandId);
            bool emailEnabled = true;
            SetMockDeleteCommandResult(BrandDeleteResult.ValidatedBrandDeletedButItemBrandAssociatedWithItems);
            SetMockBrandSubqueryResult(this.irmaBrandId, this.brandName, numberItemsAssociated);
            SetMockEmailClientSettings(emailEnabled);

            // When
            brandDeleteEventService.Run();

            //Then
            mockEmailClient.Verify(m => m.Send(It.Is<string>(s=>s == expectedMsg), It.IsAny<string>()),
                Times.Once,
                "expected [mock] email to be sent");
        }

        [TestMethod]
        public void BrandDeleteEventServiceRun_WhenBrandStillAssociatedToMultipleItems_SendsAlertWithExpectedMessage()
        {
            // Given
            SetServiceProperties(this.iconBrandId, this.region, this.brandName);
            int numberItemsAssociated = 2;
            string expectedMsg = GetExpectedMessageForBrandDeleteStillAssociatedWithItems(
                this.brandName, this.region, numberItemsAssociated, this.iconBrandId, this.irmaBrandId);
            bool emailEnabled = true;
            SetMockDeleteCommandResult(BrandDeleteResult.ValidatedBrandDeletedButItemBrandAssociatedWithItems);
            SetMockBrandSubqueryResult(this.irmaBrandId, this.brandName, numberItemsAssociated);
            SetMockEmailClientSettings(emailEnabled);

            // When
            brandDeleteEventService.Run();

            //Then
            mockEmailClient.Verify(m => m.Send(It.Is<string>(s => s == expectedMsg), It.IsAny<string>()),
                Times.Once,
                "expected [mock] email to be sent with specific message");
        }

        [TestMethod]
        public void BrandDeleteEventServiceRun_WhenBrandStillAssociatedToItem_AndAlertingEnabled_SendsAlertWithExpectedSubject()
        {
            // Given
            SetServiceProperties(this.iconBrandId, this.region, this.brandName);
            int numberItemsAssociated = 1;
            string expectedMsg = GetExpectedMessageForBrandDeleteStillAssociatedWithItems(
                this.brandName, this.region, numberItemsAssociated, this.iconBrandId, this.irmaBrandId);
            bool emailEnabled = true;
            SetMockDeleteCommandResult(BrandDeleteResult.ValidatedBrandDeletedButItemBrandAssociatedWithItems);
            SetMockBrandSubqueryResult(this.irmaBrandId, this.brandName, numberItemsAssociated);
            SetMockEmailClientSettings(emailEnabled);

            // When
            brandDeleteEventService.Run();

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.Is<string>(s => s == expectedEmailSubject)),
                Times.Once,
                "expected [mock] email to be sent with expected subject");
        }
    }
}