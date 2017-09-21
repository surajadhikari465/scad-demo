using System;
using System.Text;
using Esb.Core.EsbServices;
using Icon.Common.Email;
using Icon.Esb.Services.ConfirmationBod;
using Icon.Esb.Subscriber;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using Icon.Infor.Listeners.HierarchyClass;
using Icon.Infor.Listeners.HierarchyClass.Notifier;
using Icon.Infor.Listeners.HierarchyClass.Models;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Notifiers
{
    /// <summary>
    /// Summary description for HierarchyListenerNotifierTests
    /// </summary>
    [TestClass]
    public class HierarchyListenerNotifierTests
    {
        private HierarchyClassListenerNotifier notifier;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IEsbMessage> mockMessage;
        private HierarchyClassListenerSettings settings;
        private Mock<IEsbService<ConfirmationBodEsbRequest>> mockConfirmBodRequest;
        [TestInitialize]
        public void Initialize()
        {
            mockEmailClient = new Mock<IEmailClient>();
            settings = new HierarchyClassListenerSettings();         
            mockConfirmBodRequest = new Mock<IEsbService<ConfirmationBodEsbRequest>>();
            notifier = new HierarchyClassListenerNotifier(mockEmailClient.Object, settings, mockConfirmBodRequest.Object);

            mockMessage = new Mock<IEsbMessage>();
            mockMessage.Setup(m => m.GetProperty("IconMessageID")).Returns("123");
        }

        [TestMethod]
        public void NotifyOfHierarchyError_NoHierarchy_ShouldNotNotify()
        {
            //Given
            List<InforHierarchyClassModel> inforHierarchyClassModels = new List<InforHierarchyClassModel>();

            //When
            notifier.NotifyOfError(mockMessage.Object, ConfirmationBodEsbErrorTypes.Data, inforHierarchyClassModels);

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
        [TestMethod]
        public void NotifyOfHierarchyError_HierarchyModelHasErrors_ShouldNotify()
        {
            //Given
            List<InforHierarchyClassModel> inforHierarchyClassModels = new List<InforHierarchyClassModel> { new InforHierarchyClassModel() };

            //When
            notifier.NotifyOfError(mockMessage.Object, ConfirmationBodEsbErrorTypes.Data, inforHierarchyClassModels);

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockConfirmBodRequest.Verify(m => m.Send(It.IsAny<ConfirmationBodEsbRequest>()), Times.Never);
           
        }
        [TestMethod]
        public void NotifyOfHierarchyError_ConfirmBodEnabledAndHierarchyModelHasDataErrors_ShouldSendDataError()
        {
            //Given
            settings.EnableConfirmBods = true;
            List<InforHierarchyClassModel> inforHierarchyClassModels = new List<InforHierarchyClassModel> { new InforHierarchyClassModel() };

            //When
            notifier.NotifyOfError(mockMessage.Object, ConfirmationBodEsbErrorTypes.Data, inforHierarchyClassModels);

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockConfirmBodRequest.Verify(m => m.Send(It.Is<ConfirmationBodEsbRequest>(cb=>cb.ErrorType == ConfirmationBodEsbErrorTypes.Data)), Times.Once);
        }

        [TestMethod]
        public void NotifyOfHierarchyError_ConfirmBodEnabledAndHierarchyModelHasDataErrors_ShouldSendSchemaError()
        {
            //Given
            settings.EnableConfirmBods = true;
            List<InforHierarchyClassModel> inforHierarchyClassModels = new List<InforHierarchyClassModel> { new InforHierarchyClassModel() };

            //When
            notifier.NotifyOfError(mockMessage.Object, ConfirmationBodEsbErrorTypes.Schema, inforHierarchyClassModels);

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockConfirmBodRequest.Verify(m => m.Send(It.Is<ConfirmationBodEsbRequest>(cb => cb.ErrorType == ConfirmationBodEsbErrorTypes.Schema)), Times.Once);
        }

        [TestMethod]
        public void NotifyOfHierarchyError_ConfirmBodEnabledAndHierarchyModelHasDataErrors_ShouldSendDatabaseConstraintError()
        {
            //Given
            settings.EnableConfirmBods = true;
            List<InforHierarchyClassModel> inforHierarchyClassModels = new List<InforHierarchyClassModel> { new InforHierarchyClassModel() };

            //When
            notifier.NotifyOfError(mockMessage.Object, ConfirmationBodEsbErrorTypes.DatabaseConstraint, inforHierarchyClassModels);

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockConfirmBodRequest.Verify(m => m.Send(It.Is<ConfirmationBodEsbRequest>(cb => cb.ErrorType == ConfirmationBodEsbErrorTypes.DatabaseConstraint)), Times.Once);
        }
    }
}
