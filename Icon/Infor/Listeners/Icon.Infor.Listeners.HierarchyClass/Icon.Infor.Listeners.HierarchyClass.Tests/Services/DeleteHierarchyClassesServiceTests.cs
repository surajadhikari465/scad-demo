using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Infor.Listeners.HierarchyClass.Services;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Common.DataAccess;
using Moq;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Icon.Esb.Schemas.Wfm.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Services
{
    [TestClass]
    public class DeleteHierarchyClassesServiceTests
    {
        private DeleteHierarchyClassesService service;
        private Mock<ICommandHandler<DeleteHierarchyClassesCommand>> mockCommandHandler;

        [TestInitialize]
        public void Initialize()
        {
            mockCommandHandler = new Mock<ICommandHandler<DeleteHierarchyClassesCommand>>();
            service = new DeleteHierarchyClassesService(mockCommandHandler.Object);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_DifferentTypesOfActions_ShouldOnlyProcessAddOrUpdateHierarchyClass()
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
                new InforHierarchyClassModel { Action = ActionEnum.AddOrUpdate },
                new InforHierarchyClassModel { Action = ActionEnum.Delete }
            };

            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);

            //Then
            mockCommandHandler.Verify(
                m => m.Execute(It.Is<DeleteHierarchyClassesCommand>(
                    c => c.HierarchyClasses.All(hc => hc.Action == ActionEnum.Delete)
                        && c.HierarchyClasses.Count() == 6)),
                Times.Once);
        }
    }
}
