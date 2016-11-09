using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Icon.Common.DataAccess;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using System.Collections.Generic;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Icon.Esb.Schemas.Wfm.Contracts;
using System.Linq;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Services
{
    [TestClass]
    public class AddOrUpdateHierarchyClassesServiceTests
    {
        private AddOrUpdateHierarchyClassesService service;
        private Mock<ICommandHandler<AddOrUpdateHierarchyClassesCommand>> mockCommandHandler;

        [TestInitialize]
        public void Initialize()
        {
            mockCommandHandler = new Mock<ICommandHandler<AddOrUpdateHierarchyClassesCommand>>();
            service = new AddOrUpdateHierarchyClassesService(mockCommandHandler.Object);
        }

        [TestMethod]
        public void ProcessHierarchyClassMessages_DifferentTypesOfActions_ShouldOnlyProcessAddOrUpdateHierarchyClass()
        {
            //Given
            List<HierarchyClassModel> hierarchyClasses = new List<HierarchyClassModel>
            {
                new HierarchyClassModel { Action = ActionEnum.AddOrUpdate },
                new HierarchyClassModel { Action = ActionEnum.Delete },
                new HierarchyClassModel { Action = ActionEnum.AddOrUpdate },
                new HierarchyClassModel { Action = ActionEnum.Delete },
                new HierarchyClassModel { Action = ActionEnum.AddOrUpdate },
                new HierarchyClassModel { Action = ActionEnum.Delete },
                new HierarchyClassModel { Action = ActionEnum.AddOrUpdate },
                new HierarchyClassModel { Action = ActionEnum.Delete },
                new HierarchyClassModel { Action = ActionEnum.AddOrUpdate },
                new HierarchyClassModel { Action = ActionEnum.Delete },
                new HierarchyClassModel { Action = ActionEnum.AddOrUpdate },
                new HierarchyClassModel { Action = ActionEnum.Delete }
            };

            //When
            service.ProcessHierarchyClassMessages(hierarchyClasses);

            //Then
            mockCommandHandler.Verify(
                m => m.Execute(It.Is<AddOrUpdateHierarchyClassesCommand>(
                    c => c.HierarchyClasses.All(hc => hc.Action == ActionEnum.AddOrUpdate) 
                        && c.HierarchyClasses.Count() == 6)), 
                Times.Once);
        }
    }
}
