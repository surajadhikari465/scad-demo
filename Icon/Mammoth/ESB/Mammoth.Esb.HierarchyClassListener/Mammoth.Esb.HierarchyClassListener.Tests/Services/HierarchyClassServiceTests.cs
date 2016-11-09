using Icon.Common.DataAccess;
using Mammoth.Common.DataAccess;
using Mammoth.Esb.HierarchyClassListener.Commands;
using Mammoth.Esb.HierarchyClassListener.Models;
using Mammoth.Esb.HierarchyClassListener.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Mammoth.Esb.HierarchyClassListener.Services.Tests
{
    [TestClass]
    public class HierarchyClassServiceTests
    {
        private HierarchyClassService service;
        private Mock<ICommandHandler<AddOrUpdateHierarchyClassesCommand>> mockAddOrUpdateHierarchyClassesCommandHandler;
        private Mock<ICommandHandler<AddOrUpdateMerchandiseHierarchyLineageCommand>> mockAddOrUpdateMerchandiseHierarchyLineageCommandHandler;
        private Mock<ICommandHandler<AddOrUpdateFinancialHierarchyClassCommand>> mockAddOrUpdateFinancialHierarchyClassCommandHandler;

        [TestInitialize]
        public void Initialize()
        {
            mockAddOrUpdateHierarchyClassesCommandHandler = new Mock<ICommandHandler<AddOrUpdateHierarchyClassesCommand>>();
            mockAddOrUpdateMerchandiseHierarchyLineageCommandHandler = new Mock<ICommandHandler<AddOrUpdateMerchandiseHierarchyLineageCommand>>();
            mockAddOrUpdateFinancialHierarchyClassCommandHandler = new Mock<ICommandHandler<AddOrUpdateFinancialHierarchyClassCommand>>();

            service = new HierarchyClassService(
                mockAddOrUpdateHierarchyClassesCommandHandler.Object,
                mockAddOrUpdateMerchandiseHierarchyLineageCommandHandler.Object,
                mockAddOrUpdateFinancialHierarchyClassCommandHandler.Object);
        }

        [TestMethod]
        public void AddOrUpdateHierarchyClasses_NoMerchandiseHierarchyClasses_ShouldNotCallMerchandiseHierarchyLineageCommandHandler()
        {
            //Given
            var command = new AddOrUpdateHierarchyClassesCommand
            {
                HierarchyClasses = new List<HierarchyClassModel>
                {
                    new HierarchyClassModel { HierarchyId = Hierarchies.Brands },
                    new HierarchyClassModel { HierarchyId = Hierarchies.Brands },
                    new HierarchyClassModel { HierarchyId = Hierarchies.Brands }
                },
            };

            //When
            service.AddOrUpdateHierarchyClasses(command);

            //Then
            mockAddOrUpdateHierarchyClassesCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateHierarchyClassesCommand>()), Times.Once);
            mockAddOrUpdateMerchandiseHierarchyLineageCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateMerchandiseHierarchyLineageCommand>()), Times.Never);
        }

        [TestMethod]
        public void AddOrUpdateHierarchyClasses_MerchandiseHierarchyClassesExist_ShouldNotCallMerchandiseHierarchyLineageCommandHandler()
        {
            //Given
            var command = new AddOrUpdateHierarchyClassesCommand
            {
                HierarchyClasses = new List<HierarchyClassModel>
                {
                    new HierarchyClassModel { HierarchyId = Hierarchies.Merchandise },
                    new HierarchyClassModel { HierarchyId = Hierarchies.Merchandise },
                    new HierarchyClassModel { HierarchyId = Hierarchies.Merchandise }
                },
            };

            //When
            service.AddOrUpdateHierarchyClasses(command);

            //Then
            mockAddOrUpdateHierarchyClassesCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateHierarchyClassesCommand>()), Times.Once);
            mockAddOrUpdateMerchandiseHierarchyLineageCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateMerchandiseHierarchyLineageCommand>()), Times.Once);
        }

        [TestMethod]
        public void AddOrUpdateHierarchyClasses_FinancialHierarchyClassesExist_ShouldAddOrUpdateFinancialHierarchyClasses()
        {
            //Given
            var command = new AddOrUpdateHierarchyClassesCommand
            {
                HierarchyClasses = new List<HierarchyClassModel>
                {
                    new HierarchyClassModel { HierarchyId = Hierarchies.Financial },
                    new HierarchyClassModel { HierarchyId = Hierarchies.Financial },
                    new HierarchyClassModel { HierarchyId = Hierarchies.Financial }
                },
            };

            //When
            service.AddOrUpdateHierarchyClasses(command);

            //Then
            mockAddOrUpdateHierarchyClassesCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateHierarchyClassesCommand>()), Times.Never);
            mockAddOrUpdateMerchandiseHierarchyLineageCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateMerchandiseHierarchyLineageCommand>()), Times.Never);
            mockAddOrUpdateFinancialHierarchyClassCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateFinancialHierarchyClassCommand>()), Times.Once);
        }

        [TestMethod]
        public void AddOrUpdateHierarchyClasses_NoFinancialHierarchyClassesExist_ShouldNotAddOrUpdateFinancialHierarchyClasses()
        {
            //Given
            var command = new AddOrUpdateHierarchyClassesCommand
            {
                HierarchyClasses = new List<HierarchyClassModel>
                {
                    new HierarchyClassModel { HierarchyId = Hierarchies.Brands },
                    new HierarchyClassModel { HierarchyId = Hierarchies.Brands },
                    new HierarchyClassModel { HierarchyId = Hierarchies.Brands }
                },
            };

            //When
            service.AddOrUpdateHierarchyClasses(command);

            //Then
            mockAddOrUpdateHierarchyClassesCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateHierarchyClassesCommand>()), Times.Once);
            mockAddOrUpdateMerchandiseHierarchyLineageCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateMerchandiseHierarchyLineageCommand>()), Times.Never);
            mockAddOrUpdateFinancialHierarchyClassCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateFinancialHierarchyClassCommand>()), Times.Never);
        }

        [TestMethod]
        public void AddOrUpdateHierarchyClasses_DifferentHierarchyClassesExist_ShouldAddOrUpdateAllHierarchyClasses()
        {
            //Given
            var command = new AddOrUpdateHierarchyClassesCommand
            {
                HierarchyClasses = new List<HierarchyClassModel>
                {
                    new HierarchyClassModel { HierarchyId = Hierarchies.Brands },
                    new HierarchyClassModel { HierarchyId = Hierarchies.Merchandise },
                    new HierarchyClassModel { HierarchyId = Hierarchies.Financial }
                },
            };

            //When
            service.AddOrUpdateHierarchyClasses(command);

            //Then
            mockAddOrUpdateHierarchyClassesCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateHierarchyClassesCommand>()), Times.Once);
            mockAddOrUpdateMerchandiseHierarchyLineageCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateMerchandiseHierarchyLineageCommand>()), Times.Once);
            mockAddOrUpdateFinancialHierarchyClassCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateFinancialHierarchyClassCommand>()), Times.Once);
        }
    }
}
