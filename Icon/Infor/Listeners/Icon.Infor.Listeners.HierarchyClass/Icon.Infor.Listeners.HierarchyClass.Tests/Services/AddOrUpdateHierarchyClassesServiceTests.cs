using Icon.Common.DataAccess;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Infor.Listeners.HierarchyClass.Constants;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Icon.Infor.Listeners.HierarchyClass.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Services
{
    [TestClass]
    public class AddOrUpdateHierarchyClassesServiceTests : BaseHierarchyClassesServiceTest
    {
        private Mock<ICommandHandler<AddOrUpdateHierarchyClassesCommand>> mockAddOrUpdateCommandHandler;

        [TestInitialize]
        public void Initialize()
        {
            mockAddOrUpdateCommandHandler = new Mock<ICommandHandler<AddOrUpdateHierarchyClassesCommand>>();

            MockHierarchyClassListenerSettings.Setup(s => s.EnableNationalClassMessageGeneration).Returns(true);

            service = new AddOrUpdateHierarchyClassesService(
                MockHierarchyClassListenerSettings.Object,
                mockAddOrUpdateCommandHandler.Object,
                MockGenerateEventsCommandHandler.Object,
                MockGenerateMessagesCommandHandler.Object);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_DifferentTypesOfActions_ShouldOnlyProcessAddOrUpdates()
        {
            //Given
            List<InforHierarchyClassModel> hierarchyClasses = new List<InforHierarchyClassModel>
            {
                new InforHierarchyClassModel { Action = ActionEnum.AddOrUpdate },
                new InforHierarchyClassModel { Action = ActionEnum.Delete },
                new InforHierarchyClassModel { Action = ActionEnum.AddOrUpdate },
                new InforHierarchyClassModel { Action = ActionEnum.Delete },
                new InforHierarchyClassModel { Action = ActionEnum.AddOrUpdate },
                new InforHierarchyClassModel { Action = ActionEnum.Delete },
                new InforHierarchyClassModel { Action = ActionEnum.AddOrUpdate },
                new InforHierarchyClassModel { Action = ActionEnum.Delete },
                new InforHierarchyClassModel { Action = ActionEnum.AddOrUpdate },
                new InforHierarchyClassModel { Action = ActionEnum.Delete },
                new InforHierarchyClassModel { Action = ActionEnum.AddOrUpdate }
            };

            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);

            //Then
            mockAddOrUpdateCommandHandler.Verify(
                m => m.Execute(It.Is<AddOrUpdateHierarchyClassesCommand>(
                    c => c.HierarchyClasses.All(hc => hc.Action == ActionEnum.AddOrUpdate)
                        && c.HierarchyClasses.Count() == 6)),
                Times.Once);
        }

        #region Brand Class AddOrUpdate
        [TestMethod]
        public void ProcessHierarchyClassMessages_BrandAddOrUpdate_WhenNoData_DoesNothing()
        {
            //Given
            var hierarchyClasses = new List<InforHierarchyClassModel>();
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            mockAddOrUpdateCommandHandler.Verify(m => m
                .Execute(It.IsAny<AddOrUpdateHierarchyClassesCommand>()), Times.Never);
            MockGenerateEventsCommandHandler.Verify(m => m
                .Execute(It.IsAny<GenerateHierarchyClassEventsCommand>()), Times.Never);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_BrandAddOrUpdate_WhenNew_AddsBrandClass()
        {
            //Given
            var hierarchyName = HierarchyNames.Brands;
            var hierarchyClasses = CreateInforHierarchyClassesForAdd(hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockAddOrUpdateCall(mockAddOrUpdateCommandHandler, hierarchyName, Times.Once());
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_BrandAddOrUpdate_WhenNew_GeneratesEvent()
        {
            //Given
            var hierarchyName = HierarchyNames.Brands;
            var hierarchyClasses = CreateInforHierarchyClassesForAdd(hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockGenerateEventsCall(MockGenerateEventsCommandHandler,
                hierarchyName, ActionEnum.AddOrUpdate, Times.Once());
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_BrandAddOrUpdate_WhenExists_UpdatesBrandClass()
        {
            //Given
            var hierarchyName = HierarchyNames.Brands;
            var hierarchyClasses = CreateInforHierarchyClassesForUpdate(
                hierarchyClassIdForUpdate, hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockAddOrUpdateCall(mockAddOrUpdateCommandHandler,
                hierarchyName, Times.Once(), hierarchyClassIdForUpdate);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_BrandAddOrUpdate_WhenExists_GeneratesEvent()
        {
            //Given
            var hierarchyName = HierarchyNames.Brands;
            var hierarchyClasses = CreateInforHierarchyClassesForUpdate(
                hierarchyClassIdForUpdate, hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockGenerateEventsCall(MockGenerateEventsCommandHandler,
                hierarchyName, ActionEnum.AddOrUpdate, Times.Once(), hierarchyClassIdForUpdate);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_BrandAddOrUpdate_WhenErrorAdding_DoesNotGenerateEvent()
        {
            //Given
            var hierarchyName = HierarchyNames.Brands;
            var hierarchyClasses = CreateInforHierarchyClassesForUpdate(
                hierarchyClassIdForUpdate, hierarchyName);
            //set an error on the command data, to simulate an error when attempting to delete
            foreach (var hc in hierarchyClasses)
            {

                hc.ErrorCode = ApplicationErrors.Codes.AddOrUpdateHierarchyClassError;
                hc.ErrorDetails = ApplicationErrors.Descriptions.AddOrUpdateHierarchyClassError;
            }
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockGenerateEventsCall(MockGenerateEventsCommandHandler,
                hierarchyName, ActionEnum.AddOrUpdate, Times.Never(), hierarchyClassIdForUpdate);
        }
        #endregion

        #region National Class AddOrUpdate
        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalAddOrUpdate_WhenNewAndMessageGenOn_AddsNationalClass()
        {
            //Given
            var hierarchyName = HierarchyNames.National;
            MockHierarchyClassListenerSettings
                .Setup(m => m.EnableNationalClassMessageGeneration)
                .Returns(true);
            var hierarchyClasses = CreateInforHierarchyClassesForAdd(hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockAddOrUpdateCall(mockAddOrUpdateCommandHandler,
                hierarchyName, Times.Once());
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalAddOrUpdate_WhenNewAndMessageGenOn_GeneratesEvent()
        {
            //Given
            var hierarchyName = HierarchyNames.National;
            MockHierarchyClassListenerSettings
                .Setup(m => m.EnableNationalClassMessageGeneration)
                .Returns(true);
            var hierarchyClasses = CreateInforHierarchyClassesForAdd(hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockGenerateEventsCall(MockGenerateEventsCommandHandler,
                hierarchyName, ActionEnum.AddOrUpdate, Times.Once());
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalAddOrUpdate_WhenNewAndMessageGenOff_GeneratesEvent()
        {
            //Given
            var hierarchyName = HierarchyNames.National;
            MockHierarchyClassListenerSettings
                .Setup(m => m.EnableNationalClassMessageGeneration)
                .Returns(false);
            var hierarchyClasses = CreateInforHierarchyClassesForAdd(hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockGenerateEventsCall(MockGenerateEventsCommandHandler,
                hierarchyName, ActionEnum.AddOrUpdate, Times.Once());
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalAddOrUpdate_WhenNewAndEventMessageOff_AddsNationalClass()
        {
            //Given
            var hierarchyName = HierarchyNames.National;
            MockHierarchyClassListenerSettings
                .Setup(m => m.EnableNationalClassMessageGeneration)
                .Returns(false);
            var hierarchyClasses = CreateInforHierarchyClassesForAdd(hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockAddOrUpdateCall(mockAddOrUpdateCommandHandler,
                hierarchyName, Times.Once());
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalAddOrUpdate_WhenExistsAndMessageGenOn_UpdatesNationalClass()
        {
            //Given
            var hierarchyName = HierarchyNames.National;
            MockHierarchyClassListenerSettings
                .Setup(m => m.EnableNationalClassMessageGeneration)
                .Returns(true);
            var hierarchyClasses = CreateInforHierarchyClassesForUpdate(
                hierarchyClassIdForUpdate, hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockAddOrUpdateCall(mockAddOrUpdateCommandHandler,
                hierarchyName, Times.Once(), hierarchyClassIdForUpdate);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalAddOrUpdate_WhenExistsAndMessageGenOn_GeneratesEvent()
        {
            //Given
            var hierarchyName = HierarchyNames.National;
            MockHierarchyClassListenerSettings
                .Setup(m => m.EnableNationalClassMessageGeneration)
                .Returns(true);
            var hierarchyClasses = CreateInforHierarchyClassesForUpdate(
                hierarchyClassIdForUpdate, hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockGenerateEventsCall(MockGenerateEventsCommandHandler,
                hierarchyName, ActionEnum.AddOrUpdate, Times.Once(), hierarchyClassIdForUpdate);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalAddOrUpdate_WhenExistsAndMessageGenOff_GenerateEvent()
        {
            //Given
            var hierarchyName = HierarchyNames.National;
            MockHierarchyClassListenerSettings
                .Setup(m => m.EnableNationalClassMessageGeneration)
                .Returns(false);
            var hierarchyClasses = CreateInforHierarchyClassesForUpdate(
                hierarchyClassIdForUpdate, hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockGenerateEventsCall(MockGenerateEventsCommandHandler,
                hierarchyName, ActionEnum.AddOrUpdate, Times.Once(), hierarchyClassIdForUpdate);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalAddOrUpdate_WhenExistsAndMessageGenOff_UpdatesNationalClass()
        {
            //Given
            var hierarchyName = HierarchyNames.National;
            MockHierarchyClassListenerSettings
                .Setup(m => m.EnableNationalClassMessageGeneration)
                .Returns(false);
            var hierarchyClasses = CreateInforHierarchyClassesForUpdate(
                hierarchyClassIdForUpdate, hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockAddOrUpdateCall(mockAddOrUpdateCommandHandler,
                hierarchyName, Times.Once(), hierarchyClassIdForUpdate);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalAddOrUpdate_WhenErrorAddingAndMessageGenOn_DoesNotGenerateEvent()
        {
            //Given
            var hierarchyName = HierarchyNames.National;
            MockHierarchyClassListenerSettings
                .Setup(m => m.EnableNationalClassMessageGeneration)
                .Returns(true);
            var hierarchyClasses = CreateInforHierarchyClassesForUpdate(
                hierarchyClassIdForUpdate, hierarchyName);
            //set an error on the command data, to simulate an error when attempting to delete
            foreach (var hc in hierarchyClasses)
            {

                hc.ErrorCode = ApplicationErrors.Codes.AddOrUpdateHierarchyClassError;
                hc.ErrorDetails = ApplicationErrors.Descriptions.AddOrUpdateHierarchyClassError;
            }
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockGenerateEventsCall(MockGenerateEventsCommandHandler,
                hierarchyName, ActionEnum.AddOrUpdate, Times.Never(), hierarchyClassIdForUpdate);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalAddOrUpdate_WhenNewAndMessageGenOff_DoesNotGenerateMessage()
        {
            //Given
            var hierarchyName = HierarchyNames.National;
            var hierarchyLevelName = GetDefaultHierarchyLevelNameForTest(hierarchyName);
            MockHierarchyClassListenerSettings
                .Setup(m => m.EnableNationalClassMessageGeneration)
                .Returns(false);
            var hierarchyClasses = CreateInforHierarchyClassesForAdd(hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockGenerateMessagesCall(
                MockGenerateMessagesCommandHandler,
                hierarchyName,
                ActionEnum.AddOrUpdate,
                Times.Never(),
                hierarchyClassIdForUpdate);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalAddOrUpdate_WhenExistsAndMessageGenOn_GeneratesMessage()
        {
            //Given
            var hierarchyName = HierarchyNames.National;
            MockHierarchyClassListenerSettings
                .Setup(m => m.EnableNationalClassMessageGeneration)
                .Returns(true);
            var hierarchyClasses = CreateInforHierarchyClassesForUpdate(
                hierarchyClassIdForUpdate, hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockGenerateMessagesCall(
                MockGenerateMessagesCommandHandler,
                hierarchyName,
                ActionEnum.AddOrUpdate,
                Times.Once(),
                hierarchyClassIdForUpdate);
        }
        #endregion
    }
}
