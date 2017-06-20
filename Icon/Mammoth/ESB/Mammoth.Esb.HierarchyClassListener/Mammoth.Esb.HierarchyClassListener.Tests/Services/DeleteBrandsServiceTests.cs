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
    public class DeleteBrandsServiceTests
    {
        private DeleteBrandService deleteBrandService;
        private Mock<ICommandHandler<DeleteBrandsCommand>> mockDeleteBrandsCommandHandler;
        private List<HierarchyClassModel> hierarchyClasses;
        private DeleteBrandRequest deleteBrandRequest;

        [TestInitialize]
        public void InitializeTest()
        {
            this.mockDeleteBrandsCommandHandler = new Mock<ICommandHandler<DeleteBrandsCommand>>();
            this.deleteBrandService = new DeleteBrandService(this.mockDeleteBrandsCommandHandler.Object);
            this.hierarchyClasses = new List<HierarchyClassModel>();

            for (int i = 0; i < 3; i++)
            {
                HierarchyClassModel hc = new HierarchyClassModel
                {
                    Action = ActionEnum.Delete,
                    HierarchyClassId = i + 1,
                    HierarchyClassName = $"Delete Brand {i} Unit Test",
                    HierarchyClassParentId = 0,
                    HierarchyId = Hierarchies.Brands,
                    HierarchyLevelName = String.Empty,
                    Timestamp = DateTime.UtcNow
                };

                this.hierarchyClasses.Add(hc);
            }
        }

        [TestMethod]
        public void DeleteBrandsService_HierarchyClassesWithNoBrands_DoesNotCallDeleteBrandCommandHandler()
        {
            // Given
            this.hierarchyClasses.ForEach(hc => hc.HierarchyId = Hierarchies.Financial);
            this.deleteBrandRequest = new DeleteBrandRequest { HierarchyClasses = this.hierarchyClasses };

            // When
            this.deleteBrandService.ProcessHierarchyClasses(this.deleteBrandRequest);

            // Then
            this.mockDeleteBrandsCommandHandler.Verify(h => h.Execute(It.IsAny<DeleteBrandsCommand>()), Times.Never);
        }

        [TestMethod]
        public void DeleteBrandsService_HierarchyClassesWithBrands_CallsDeleteBrandCommandHandler()
        {
            // Given
            this.deleteBrandRequest = new DeleteBrandRequest { HierarchyClasses = this.hierarchyClasses };

            // When
            this.deleteBrandService.ProcessHierarchyClasses(this.deleteBrandRequest);

            // Then
            for (int i = 0; i < this.hierarchyClasses.Count; i++)
            {
                this.mockDeleteBrandsCommandHandler.Verify(h =>
                    h.Execute(It.Is<DeleteBrandsCommand>(c =>
                        c.Brands[i].HierarchyClassId == this.hierarchyClasses[i].HierarchyClassId
                        && c.Brands[i].HierarchyClassName == this.hierarchyClasses[i].HierarchyClassName
                        && c.Brands[i].HierarchyId == this.hierarchyClasses[i].HierarchyId)), Times.Once);
            }
        }

        [TestMethod]
        public void DeleteBrandsService_HierarchyClassesWithBrandsButNoDeleteAction_DeleteBrandsCommandHandlerNotCalled()
        {
            // Given
            this.hierarchyClasses.ForEach(hc => hc.Action = ActionEnum.Add);
            this.deleteBrandRequest = new DeleteBrandRequest { HierarchyClasses = this.hierarchyClasses };

            // When
            this.deleteBrandService.ProcessHierarchyClasses(this.deleteBrandRequest);

            // Then
            this.mockDeleteBrandsCommandHandler.Verify(dc => dc.Execute(It.IsAny<DeleteBrandsCommand>()), Times.Never);
        }

        [TestMethod]
        public void DeleteBrandsService_HierarchyClassesHaveSomeBrandDeletesAndSomeNonBrandDeletes_OnlyCallsDeleteBrandCommandHandlerForBrandDeletes()
        {
            // Given
            this.hierarchyClasses.First().HierarchyId = Hierarchies.Merchandise;
            this.deleteBrandRequest = new DeleteBrandRequest { HierarchyClasses = this.hierarchyClasses };

            // When
            this.deleteBrandService.ProcessHierarchyClasses(this.deleteBrandRequest);

            // Then
            this.mockDeleteBrandsCommandHandler.Verify(h =>
                h.Execute(It.Is<DeleteBrandsCommand>(c =>
                    c.Brands.Count == 2
                    && c.Brands[0].HierarchyClassId == this.hierarchyClasses[1].HierarchyClassId
                    && c.Brands[1].HierarchyClassId == this.hierarchyClasses[2].HierarchyClassId)), Times.Once);
        }
    }
}
