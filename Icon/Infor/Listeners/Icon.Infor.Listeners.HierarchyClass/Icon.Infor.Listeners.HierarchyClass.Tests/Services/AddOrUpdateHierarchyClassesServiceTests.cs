using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Icon.Common.DataAccess;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using System.Collections.Generic;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Icon.Esb.Schemas.Wfm.Contracts;
using System.Linq;
using Icon.Framework;
using Icon.Common.Context;
using System.Data.SqlClient;
using Icon.Infor.Listeners.HierarchyClass.Constants;

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

            mockHierarchyClassListenerSettings.Setup(s => s.EnableNationalClassEventGeneration).Returns(true);

            service = new AddOrUpdateHierarchyClassesService(
                mockHierarchyClassListenerSettings.Object,
                mockAddOrUpdateCommandHandler.Object,
                mockGenerateEventsCommandHandler.Object);
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
            mockGenerateEventsCommandHandler.Verify(m => m
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
            VerifyMockGenerateEventsCall(mockGenerateEventsCommandHandler,
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
            VerifyMockGenerateEventsCall(mockGenerateEventsCommandHandler,
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
            VerifyMockGenerateEventsCall(mockGenerateEventsCommandHandler,
                hierarchyName, ActionEnum.AddOrUpdate, Times.Never(), hierarchyClassIdForUpdate);
        }
        #endregion

        #region National Class AddOrUpdate
        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalAddOrUpdate_WhenNewAndEventGenOn_AddsNationalClass()
        {
            //Given
            var hierarchyName = HierarchyNames.National;
            mockHierarchyClassListenerSettings
                .Setup(m => m.EnableNationalClassEventGeneration)
                .Returns(true);
            var hierarchyClasses = CreateInforHierarchyClassesForAdd(hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockAddOrUpdateCall(mockAddOrUpdateCommandHandler,
                hierarchyName, Times.Once());
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalAddOrUpdate_WhenNewAndEventGenOn_GeneratesEvent()
        {
            //Given
            var hierarchyName = HierarchyNames.National;
            mockHierarchyClassListenerSettings
                .Setup(m => m.EnableNationalClassEventGeneration)
                .Returns(true);
            var hierarchyClasses = CreateInforHierarchyClassesForAdd(hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockGenerateEventsCall(mockGenerateEventsCommandHandler,
                hierarchyName, ActionEnum.AddOrUpdate, Times.Once());
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalAddOrUpdate_WhenNewAndEventGenOff_DoesNotGenerateEvent()
        {
            //Given
            var hierarchyName = HierarchyNames.National;
            mockHierarchyClassListenerSettings
                .Setup(m => m.EnableNationalClassEventGeneration)
                .Returns(false);
            var hierarchyClasses = CreateInforHierarchyClassesForAdd(hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockGenerateEventsCall(mockGenerateEventsCommandHandler,
                hierarchyName, ActionEnum.AddOrUpdate, Times.Never());
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalAddOrUpdate_WhenNewAndEventGenOff_AddsNationalClass()
        {
            //Given
            var hierarchyName = HierarchyNames.National;
            mockHierarchyClassListenerSettings
                .Setup(m => m.EnableNationalClassEventGeneration)
                .Returns(false);
            var hierarchyClasses = CreateInforHierarchyClassesForAdd(hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockAddOrUpdateCall(mockAddOrUpdateCommandHandler,
                hierarchyName, Times.Once());
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalAddOrUpdate_WhenExistsAndEventGenOn_UpdatesNationalClass()
        {
            //Given
            var hierarchyName = HierarchyNames.National;
            mockHierarchyClassListenerSettings
                .Setup(m => m.EnableNationalClassEventGeneration)
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
        public void ProcessHierarchyClassMessages_NationalAddOrUpdate_WhenExistsAndEventGenOn_GeneratesEvent()
        {
            //Given
            var hierarchyName = HierarchyNames.National;
            mockHierarchyClassListenerSettings
                .Setup(m => m.EnableNationalClassEventGeneration)
                .Returns(true);
            var hierarchyClasses = CreateInforHierarchyClassesForUpdate(
                hierarchyClassIdForUpdate, hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockGenerateEventsCall(mockGenerateEventsCommandHandler,
                hierarchyName, ActionEnum.AddOrUpdate, Times.Once(), hierarchyClassIdForUpdate);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalAddOrUpdate_WhenExistsAndEventGenOff_DoesNotGenerateEvent()
        {
            //Given
            var hierarchyName = HierarchyNames.National;
            mockHierarchyClassListenerSettings
                .Setup(m => m.EnableNationalClassEventGeneration)
                .Returns(false);
            var hierarchyClasses = CreateInforHierarchyClassesForUpdate(
                hierarchyClassIdForUpdate, hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockGenerateEventsCall(mockGenerateEventsCommandHandler,
                hierarchyName, ActionEnum.AddOrUpdate, Times.Never(), hierarchyClassIdForUpdate);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalAddOrUpdate_WhenExistsAndEventGenOff_UpdatesNationalClass()
        {
            //Given
            var hierarchyName = HierarchyNames.National;
            mockHierarchyClassListenerSettings
                .Setup(m => m.EnableNationalClassEventGeneration)
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
        public void ProcessHierarchyClassMessages_NationalAddOrUpdate_WhenErrorAddingAndEventGenOn_DoesNotGenerateEvent()
        {
            //Given
            var hierarchyName = HierarchyNames.National;
            mockHierarchyClassListenerSettings
                .Setup(m => m.EnableNationalClassEventGeneration)
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
            VerifyMockGenerateEventsCall(mockGenerateEventsCommandHandler,
                hierarchyName, ActionEnum.AddOrUpdate, Times.Never(), hierarchyClassIdForUpdate);
        }
        #endregion
    }
}
