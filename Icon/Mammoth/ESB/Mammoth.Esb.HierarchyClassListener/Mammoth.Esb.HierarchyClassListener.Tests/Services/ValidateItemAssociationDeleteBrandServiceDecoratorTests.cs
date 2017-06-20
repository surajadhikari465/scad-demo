using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb.ListenerApplication;
using Icon.Logging;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.Models;
using Mammoth.Esb.HierarchyClassListener.Models;
using Mammoth.Esb.HierarchyClassListener.Queries;
using Mammoth.Esb.HierarchyClassListener.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Esb.HierarchyClassListener.Tests.Services
{
    [TestClass]
    public class ValidateItemAssociationDeleteBrandServiceDecoratorTests
    {
        private Mock<IHierarchyClassService<DeleteBrandRequest>> mockService;
        private ValidateItemAssociationDeleteBrandServiceDecorator decorator;
        private DeleteBrandRequest request;
        private Mock<IQueryHandler<GetItemsByBrandIdQuery, IEnumerable<Item>>> mockGetItemsByBrandQuery;
        private ListenerApplicationSettings settings;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<ILogger<MammothHierarchyClassListener>> mockLogger;
        private List<HierarchyClassModel> brands;
        private List<Item> itemsAssociatedToBrand;

        [TestInitialize]
        public void InitializeTest()
        {
            this.mockLogger = new Mock<ILogger<MammothHierarchyClassListener>>();
            this.settings = new ListenerApplicationSettings();
            this.mockEmailClient = new Mock<IEmailClient>();
            this.mockGetItemsByBrandQuery = new Mock<IQueryHandler<GetItemsByBrandIdQuery, IEnumerable<Item>>>();
            this.mockService = new Mock<IHierarchyClassService<DeleteBrandRequest>>();
            this.decorator = new ValidateItemAssociationDeleteBrandServiceDecorator(
                this.mockService.Object,
                this.mockGetItemsByBrandQuery.Object,
                this.settings,
                this.mockEmailClient.Object,
                this.mockLogger.Object);
            this.request = new DeleteBrandRequest();

            this.brands = new List<HierarchyClassModel>();
            this.brands.Add(new HierarchyClassModel
            {
                Action = Icon.Esb.Schemas.Wfm.Contracts.ActionEnum.Delete,
                HierarchyClassId = 1,
                HierarchyClassName = "Brand With Item Associations",
                HierarchyClassParentId = 0,
                HierarchyId = Hierarchies.Brands,
                HierarchyLevelName = null,
                Timestamp = DateTime.UtcNow
            });
            this.brands.Add(new HierarchyClassModel
            {
                Action = Icon.Esb.Schemas.Wfm.Contracts.ActionEnum.Delete,
                HierarchyClassId = 8987789,
                HierarchyClassName = "Random Unit Test Brand To Be Deleted",
                HierarchyClassParentId = 0,
                HierarchyId = Hierarchies.Brands,
                HierarchyLevelName = null,
                Timestamp = DateTime.UtcNow
            });

            this.itemsAssociatedToBrand = new List<Item>();
            this.itemsAssociatedToBrand.Add(new Item
            {
                ItemID = 1,
                AddedDate = DateTime.UtcNow.AddDays(-2),
                BrandHCID = this.brands.First().HierarchyClassId,
                Desc_POS = "Unit Test Item",
                Desc_Product = "Unit Test Item",
                FoodStampEligible = true,
                ScanCode = "112233445566"
            });
            this.itemsAssociatedToBrand.Add(new Item
            {
                ItemID = 2,
                AddedDate = DateTime.UtcNow.AddDays(-2),
                BrandHCID = 33,
                Desc_POS = "Unit Test Item 2",
                Desc_Product = "Unit Test Item With Random Brand",
                FoodStampEligible = true,
                ScanCode = "665544332211"
            });

            this.request.HierarchyClasses = new List<HierarchyClassModel>();
            this.request.HierarchyClasses.AddRange(this.brands);
        }

        [TestMethod]
        public void ValidateItemAssociationDeleteBrandService_ItemsAssociatedToBrand_BrandsRemovedFromRequest()
        {
            // Given
            this.mockGetItemsByBrandQuery
                .Setup(q => q.Search(It.IsAny<GetItemsByBrandIdQuery>()))
                .Returns(this.itemsAssociatedToBrand);

            // When
            this.decorator.ProcessHierarchyClasses(this.request);

            // Then
            this.mockService
                .Verify(s => s.ProcessHierarchyClasses(It.Is<DeleteBrandRequest>(r =>
                    r.HierarchyClasses.Select(hc => hc.HierarchyClassId).Contains(this.brands[0].HierarchyClassId))),
                    Times.Never);

            this.mockService
                .Verify(s => s.ProcessHierarchyClasses(It.Is<DeleteBrandRequest>(r =>
                    r.HierarchyClasses.Select(hc => hc.HierarchyClassId).Contains(this.brands[1].HierarchyClassId))),
                    Times.Once);
        }

        [TestMethod]
        public void ValidateItemAssociationDeleteBrandService_ItemsAssociatedToBrand_ErrorLoggedAndEmailed()
        {
            // Given
            this.mockGetItemsByBrandQuery
                .Setup(q => q.Search(It.IsAny<GetItemsByBrandIdQuery>()))
                .Returns(this.itemsAssociatedToBrand);

            // When
            this.decorator.ProcessHierarchyClasses(this.request);

            // Then
            this.mockLogger
                .Verify(l => l.Error(It.Is<string>(s => 
                    s == "The following brands have items associated to them and will not be deleted: Brand With Item Associations")),
                    Times.Once);

            this.mockEmailClient
                .Verify(ec => ec.Send(It.Is<string>(s => s.Contains("The following Brands cannot be deleted because they are still associated to items.")),
                    It.Is<string>(s => s == "Mammoth Hierarchy Class Listener: Brand Delete Errors")),
                    Times.Once);

            this.mockService
                .Verify(s => s.ProcessHierarchyClasses(It.IsAny<DeleteBrandRequest>()),
                    Times.Once);
        }

        [TestMethod]
        public void ValidateItemAssociationDeleteBrandService_NoItemsAssociatedToBrand_NoErrorLoggedOrEmailed()
        {
            // Given
            this.mockGetItemsByBrandQuery
                .Setup(q => q.Search(It.IsAny<GetItemsByBrandIdQuery>()))
                .Returns(new List<Item>());

            // When
            this.decorator.ProcessHierarchyClasses(this.request);

            // Then
            this.mockLogger
                .Verify(l => l.Error(It.IsAny<string>()),
                    Times.Never);

            this.mockEmailClient
                .Verify(ec => ec.Send(It.IsAny<string>(), It.IsAny<string>()),
                    Times.Never);

            this.mockService
                .Verify(s => s.ProcessHierarchyClasses(It.IsAny<DeleteBrandRequest>()),
                    Times.Once);
        }

        [TestMethod]
        public void ValidateItemAssociationDeleteBrandService_NoItemsAssociatedToBrand_RequestUnchangedAndDeleteBrandServiceCalled()
        {
            // Given
            this.mockGetItemsByBrandQuery
                .Setup(q => q.Search(It.IsAny<GetItemsByBrandIdQuery>()))
                .Returns(new List<Item>());

            // When
            this.decorator.ProcessHierarchyClasses(this.request);

            // Then
            this.mockService
                .Verify(s => s.ProcessHierarchyClasses(It.Is<DeleteBrandRequest>(r =>
                    r.HierarchyClasses.Select(hc => hc.HierarchyClassId).Contains(this.brands[0].HierarchyClassId)
                    && r.HierarchyClasses.Select(hc => hc.HierarchyClassId).Contains(this.brands[1].HierarchyClassId))),
                    Times.Once);
        }
    }
}
