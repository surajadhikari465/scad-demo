using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Icon.Common.DataAccess;
using Mammoth.Esb.HierarchyClassListener.Commands;
using System.Collections.Generic;
using Mammoth.Esb.HierarchyClassListener.Models;
using Icon.Esb.Schemas.Wfm.Contracts;
using Mammoth.Common.DataAccess;
using Mammoth.Esb.HierarchyClassListener.Services;
using System.Linq;

namespace Mammoth.Esb.HierarchyClassListener.Tests.Services
{
    [TestClass]
    public class DeleteMerchandiseClassesServiceUnitTests
    {
        private DeleteMerchandiseClassService deleteMerchandiseClassesService;
        private Mock<ICommandHandler<DeleteMerchandiseClassParameter>> mockDeleteMerchandiseClassesCommandHandler;
        private List<HierarchyClassModel> hierarchyClasses;
        private DeleteMerchandiseClassRequest deleteMerchandiseClassesRequest;

        [TestInitialize]
        public void InitializeTest()
        {
            this.mockDeleteMerchandiseClassesCommandHandler = new Mock<ICommandHandler<DeleteMerchandiseClassParameter>>();
            this.deleteMerchandiseClassesService = new DeleteMerchandiseClassService(this.mockDeleteMerchandiseClassesCommandHandler.Object);
            this.hierarchyClasses = new List<HierarchyClassModel>();

            for (int i = 0; i < 3; i++)
            {
                HierarchyClassModel hc = new HierarchyClassModel
                {
                    Action = ActionEnum.Delete,
                    HierarchyClassId = i + 1,
                    HierarchyClassName = $"Delete MerchandiseClass {i} Unit Test",
                    HierarchyClassParentId = 0,
                    HierarchyId = Hierarchies.Merchandise,
                    HierarchyLevelName = String.Empty,
                    Timestamp = DateTime.UtcNow
                };

                this.hierarchyClasses.Add(hc);
            }
        }

        [TestMethod]
        public void DeleteMerchandiseClassesService_RequestHasNoMerchandiseClasses_DeleteCommandHandlerNotCalled()
        {
            // Given
            this.hierarchyClasses.ForEach(hc => hc.HierarchyId = Hierarchies.Financial);
            this.deleteMerchandiseClassesRequest = new DeleteMerchandiseClassRequest { HierarchyClasses = this.hierarchyClasses };

            // When
            this.deleteMerchandiseClassesService.ProcessHierarchyClasses(this.deleteMerchandiseClassesRequest);

            // Then
            this.mockDeleteMerchandiseClassesCommandHandler.Verify(h => h.Execute(It.IsAny<DeleteMerchandiseClassParameter>()), Times.Never);
        }

        [TestMethod]
        public void DeleteMerchandiseClassesService_RequestHasValidMerchandiseClasses_DeleteCommandHandlerIsCalled()
        {
            // Given
            this.deleteMerchandiseClassesRequest = new DeleteMerchandiseClassRequest { HierarchyClasses = this.hierarchyClasses };

            // When
            this.deleteMerchandiseClassesService.ProcessHierarchyClasses(this.deleteMerchandiseClassesRequest);

            // Then
            for (int i = 0; i < this.hierarchyClasses.Count; i++)
            {
                this.mockDeleteMerchandiseClassesCommandHandler.Verify(h =>
                    h.Execute(It.Is<DeleteMerchandiseClassParameter>(c =>
                        c.MerchandiseClasses[i].HierarchyClassId == this.hierarchyClasses[i].HierarchyClassId
                        && c.MerchandiseClasses[i].HierarchyClassName == this.hierarchyClasses[i].HierarchyClassName
                        && c.MerchandiseClasses[i].HierarchyId == this.hierarchyClasses[i].HierarchyId)), Times.Once);
            }
        }

        [TestMethod]
        public void DeleteMerchandiseClassesService_RequestHasNoDeleteAction_DeleteCommandHandlerNotCalled()
        {
            // Given
            this.hierarchyClasses.ForEach(hc => hc.Action = ActionEnum.Add);
            this.deleteMerchandiseClassesRequest = new DeleteMerchandiseClassRequest { HierarchyClasses = this.hierarchyClasses };

            // When
            this.deleteMerchandiseClassesService.ProcessHierarchyClasses(this.deleteMerchandiseClassesRequest);

            // Then
            this.mockDeleteMerchandiseClassesCommandHandler.Verify(dc => dc.Execute(It.IsAny<DeleteMerchandiseClassParameter>()), Times.Never);
        }

        [TestMethod]
        public void DeleteMerchandiseClassesService_RequestIncludesSomeNonMerchandiseHierarchyClasses_OnlyCallsDeleteCommandHandlerForMerchandiseClasses()
        {
            // Given
            this.hierarchyClasses.First().HierarchyId = Hierarchies.Brands;
            this.deleteMerchandiseClassesRequest = new DeleteMerchandiseClassRequest { HierarchyClasses = this.hierarchyClasses };

            // When
            this.deleteMerchandiseClassesService.ProcessHierarchyClasses(this.deleteMerchandiseClassesRequest);

            // Then
            this.mockDeleteMerchandiseClassesCommandHandler.Verify(h =>
                h.Execute(It.Is<DeleteMerchandiseClassParameter>(c =>
                    c.MerchandiseClasses.Count == 2
                    && c.MerchandiseClasses[0].HierarchyClassId == this.hierarchyClasses[1].HierarchyClassId
                    && c.MerchandiseClasses[1].HierarchyClassId == this.hierarchyClasses[2].HierarchyClassId)), Times.Once);
        }
    }
}
