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
    public class DeleteHierarchyClassesServiceTests : BaseHierarchyClassesServiceTest
    {
        private Mock<ICommandHandler<DeleteHierarchyClassesCommand>> mockDeleteCommandHandler;
        private Mock<ICommandHandler<GenerateHierarchyClassMessagesCommand>> mockGenerateHierarchyClassEventsCommandHandler;
        private HierarchyClassListenerSettings settings;

        [TestInitialize]
        public void Initialize()
        {
            mockDeleteCommandHandler = new Mock<ICommandHandler<DeleteHierarchyClassesCommand>>();
            mockGenerateHierarchyClassEventsCommandHandler = new Mock<ICommandHandler<GenerateHierarchyClassMessagesCommand>>();
            settings = new HierarchyClassListenerSettings { EnableNationalClassEventGeneration = true };

            service = new DeleteHierarchyClassesService(
                settings,
                mockDeleteCommandHandler.Object,
                MockGenerateEventsCommandHandler.Object,
                mockGenerateHierarchyClassEventsCommandHandler.Object);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_DifferentTypesOfActions_ShouldOnlyProcessDeletes()
        {
            //Given
            var hierarchyClasses = new List<InforHierarchyClassModel>
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
                new InforHierarchyClassModel { Action = ActionEnum.Delete }
            };

            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);

            //Then
            mockDeleteCommandHandler.Verify(
                m => m.Execute(It.Is<DeleteHierarchyClassesCommand>(
                    c => c.HierarchyClasses.All(hc => hc.Action == ActionEnum.Delete)
                        && c.HierarchyClasses.Count() == 6)),
                Times.Once);
        }

        #region Brand Class Delete
        [TestMethod]
        public void ProcessHierarchyClassMessages_BrandDelete_WhenNoData_DoesNothing()
        {
            //Given
            var hierarchyClasses = new List<InforHierarchyClassModel>();
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            mockDeleteCommandHandler.Verify(m => m
                .Execute(It.IsAny<DeleteHierarchyClassesCommand>()), Times.Never);
            MockGenerateEventsCommandHandler.Verify(m => m
                .Execute(It.IsAny<GenerateHierarchyClassEventsCommand>()), Times.Never);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_BrandDelete_WhenExistsDeletesBrandClass()
        {
            //Given
            var hierarchyName = HierarchyNames.Brands;
            var hierarchyClasses = CreateInforHierarchyClassesForDelete(
                hierarchyClassIdForUpdate, hierarchyName);
            //mockDeleteCommandHandler.Setup(x=>x.)
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockDeleteCall(mockDeleteCommandHandler, hierarchyName, Times.Once(), hierarchyClassIdForUpdate);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_BrandDelete_WhenExists_GeneratesEvent()
        {
            //Given
            var hierarchyName = HierarchyNames.Brands;
            var hierarchyClasses = CreateInforHierarchyClassesForDelete(
                hierarchyClassIdForUpdate, hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockGenerateEventsCall(MockGenerateEventsCommandHandler,
                hierarchyName, ActionEnum.Delete, Times.Once(), hierarchyClassIdForUpdate);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_BrandDelete_WhenExists_DeletesBrandClass()
        {
            //Given
            var hierarchyName = HierarchyNames.Brands;
            var hierarchyClasses = CreateInforHierarchyClassesForDelete(
                hierarchyClassIdForUpdate, hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //When
            VerifyMockDeleteCall(mockDeleteCommandHandler, hierarchyName, Times.Once(), hierarchyClassIdForUpdate);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_BrandDelete_WhenAllErrored_DoesNotGenerateEvent()
        {
            //Given
            var hierarchyName = HierarchyNames.Brands;
            var hierarchyClasses = CreateInforHierarchyClassesForDelete(
                hierarchyClassIdForUpdate, hierarchyName);
            //set an error on the command data, to simulate an error when attempting to delete
            foreach( var hc in hierarchyClasses)
            {
                hc.ErrorCode = ApplicationErrors.Codes.DeleteHierarchyClassError;
                hc.ErrorDetails = ApplicationErrors.Descriptions.DeleteHierarchyClassError;
            }
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockGenerateEventsCall(MockGenerateEventsCommandHandler,
                hierarchyName, ActionEnum.Delete, Times.Never(), hierarchyClassIdForUpdate);
        }

        #endregion

        #region National Class Delete
        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalDelete_WhenNoData_DoesNothing()
        {
            //Given
            var hierarchyClasses = new List<InforHierarchyClassModel>();
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Thenn
            mockDeleteCommandHandler.Verify(m => m
                .Execute(It.IsAny<DeleteHierarchyClassesCommand>()), Times.Never);
            MockGenerateEventsCommandHandler.Verify(m => m
                .Execute(It.IsAny<GenerateHierarchyClassEventsCommand>()), Times.Never);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalDelete_WhenExists_DeletesNationalClass()
        {
            //Given
            var hierarchyName = HierarchyNames.National;
            var hierarchyClasses = CreateInforHierarchyClassesForDelete(
                hierarchyClassIdForUpdate, hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockDeleteCall(mockDeleteCommandHandler,
                hierarchyName, Times.Once(), hierarchyClassIdForUpdate);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalDelete_WhenExists_GeneratesEvent()
        {
            var hierarchyName = HierarchyNames.National;
            var hierarchyClasses = CreateInforHierarchyClassesForDelete(
                hierarchyClassIdForUpdate, hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockGenerateEventsCall(MockGenerateEventsCommandHandler,
                hierarchyName, ActionEnum.Delete, Times.Once(), hierarchyClassIdForUpdate);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalDelete_WhenAllErrored_DoesNotGenerateEvent()
        {
            var hierarchyName = HierarchyNames.National;
            var hierarchyClasses = CreateInforHierarchyClassesForDelete(
                hierarchyClassIdForUpdate, hierarchyName);
            //set an error on the command data, to simulate an error when attempting to delete
            foreach (var hc in hierarchyClasses)
            {
                hc.ErrorCode = ApplicationErrors.Codes.DeleteHierarchyClassError;
                hc.ErrorDetails = ApplicationErrors.Descriptions.DeleteHierarchyClassError;
            }
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockGenerateEventsCall(MockGenerateEventsCommandHandler,
                hierarchyName, ActionEnum.Delete, Times.Never(), hierarchyClassIdForUpdate);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_NationalDelete_WhenExists_SettingIsTurnedOff_DoesNotGeneratesEvent()
        {
            settings.EnableNationalClassEventGeneration = false;
            var hierarchyName = HierarchyNames.National;
            var hierarchyClasses = CreateInforHierarchyClassesForDelete(
                hierarchyClassIdForUpdate, hierarchyName);
            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);
            //Then
            VerifyMockGenerateEventsCall(MockGenerateEventsCommandHandler,
                hierarchyName, ActionEnum.Delete, Times.Never(), hierarchyClassIdForUpdate);
        }

        #endregion
    }
}