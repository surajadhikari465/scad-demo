using Icon.Common.DataAccess;
using Icon.Common.Email;
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
    public class ValidateItemAssociationForDeleteMerchandiseHierarchyDecoratorTests
    {
        private Mock<IHierarchyClassService<IHierarchyClassRequest>> mockService;
        private ValidateItemAssociationForDeleteMerchandiseHierarchyDecorator decorator;
        private DeleteMerchandiseClassRequest request;
        Mock<IQueryHandler<IGetAssociatedItemsParameter, IEnumerable<Item>>> mockGetItemsByMerchandiseHierarchyQuery;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<ILogger<MammothHierarchyClassListener>> mockLogger;
        private List<HierarchyClassModel> merchandiseHierarchies;
        private List<Item> itemsAssociatedToMerchandiseHierarchy;

        [TestInitialize]
        public void InitializeTest()
        {
            this.mockLogger = new Mock<ILogger<MammothHierarchyClassListener>>();
            this.mockEmailClient = new Mock<IEmailClient>();
            this.mockGetItemsByMerchandiseHierarchyQuery = new Mock<IQueryHandler<IGetAssociatedItemsParameter, IEnumerable<Item>>>();
            this.mockService = new Mock<IHierarchyClassService<IHierarchyClassRequest>>();
            this.decorator = new ValidateItemAssociationForDeleteMerchandiseHierarchyDecorator(
                this.mockService.Object,
                this.mockGetItemsByMerchandiseHierarchyQuery.Object,
                this.mockEmailClient.Object,
                this.mockLogger.Object);
            this.request = new DeleteMerchandiseClassRequest();

            this.merchandiseHierarchies = new List<HierarchyClassModel>();
            this.merchandiseHierarchies.Add(new HierarchyClassModel
            {
                Action = Icon.Esb.Schemas.Wfm.Contracts.ActionEnum.Delete,
                HierarchyClassId = 1,
                HierarchyClassName = "Merchandise Hierarchy With Item Associations",
                HierarchyClassParentId = 0,
                HierarchyId = Hierarchies.Merchandise,
                HierarchyLevelName = null,
                Timestamp = DateTime.UtcNow
            });
            this.merchandiseHierarchies.Add(new HierarchyClassModel
            {
                Action = Icon.Esb.Schemas.Wfm.Contracts.ActionEnum.Delete,
                HierarchyClassId = 8987789,
                HierarchyClassName = "Random Unit Test Merchandise Hierarchy To Be Deleted",
                HierarchyClassParentId = 0,
                HierarchyId = Hierarchies.Merchandise,
                HierarchyLevelName = null,
                Timestamp = DateTime.UtcNow
            });

            this.itemsAssociatedToMerchandiseHierarchy = new List<Item>();
            this.itemsAssociatedToMerchandiseHierarchy.Add(new Item
            {
                ItemID = 1,
                AddedDate = DateTime.UtcNow.AddDays(-2),
                HierarchyMerchandiseID = this.merchandiseHierarchies.First().HierarchyClassId,
                Desc_POS = "Unit Test Item",
                Desc_Product = "Unit Test Item",
                FoodStampEligible = true,
                ScanCode = "112233445566"
            });
            this.itemsAssociatedToMerchandiseHierarchy.Add(new Item
            {
                ItemID = 2,
                AddedDate = DateTime.UtcNow.AddDays(-2),
                HierarchyMerchandiseID = 33,
                Desc_POS = "Unit Test Item 2",
                Desc_Product = "Unit Test Item With Random Merchandise Hierarchy",
                FoodStampEligible = true,
                ScanCode = "665544332211"
            });

            this.request.HierarchyClasses = new List<HierarchyClassModel>();
            this.request.HierarchyClasses.AddRange(this.merchandiseHierarchies);
        }

        [TestMethod]
        public void ValidateItemAssociationDeleteMerchandiseHierarchyService_ItemsWithAssociatedItems_RemovedFromRequest()
        {
            // Given
            this.mockGetItemsByMerchandiseHierarchyQuery
                .Setup(q => q.Search(It.IsAny<GetItemsByMerchandiseHierarchyIdParameter>()))
                .Returns(this.itemsAssociatedToMerchandiseHierarchy);

            // When
            this.decorator.ProcessHierarchyClasses(this.request);

            // Then
            this.mockService
                .Verify(s => s.ProcessHierarchyClasses(It.Is<DeleteMerchandiseClassRequest>(r =>
                    r.HierarchyClasses.Select(hc => hc.HierarchyClassId).Contains(this.merchandiseHierarchies[0].HierarchyClassId))),
                    Times.Never);

            this.mockService
                .Verify(s => s.ProcessHierarchyClasses(It.Is<DeleteMerchandiseClassRequest>(r =>
                    r.HierarchyClasses.Select(hc => hc.HierarchyClassId).Contains(this.merchandiseHierarchies[1].HierarchyClassId))),
                    Times.Once);
        }

        [TestMethod]
        public void ValidateItemAssociationDeleteMerchandiseHierarchyService_WIthItemsAssociated_ErrorLoggedAndEmailed()
        {
            // Given
            this.mockGetItemsByMerchandiseHierarchyQuery
                .Setup(q => q.Search(It.IsAny<GetItemsByMerchandiseHierarchyIdParameter>()))
                .Returns(this.itemsAssociatedToMerchandiseHierarchy);

            // When
            this.decorator.ProcessHierarchyClasses(this.request);

            // Then
            this.mockLogger
                .Verify(l => l.Error(It.Is<string>(s => 
                    s == "The following Merchandise Hierarchies have items associated to them and will not be deleted: Merchandise Hierarchy With Item Associations")),
                    Times.Once);

            this.mockEmailClient
                .Verify(ec => ec.Send(It.Is<string>(s => s.Contains("The following Merchandise Hierarchies cannot be deleted because they are still associated to items.")),
                    It.Is<string>(s => s == "Mammoth Hierarchy Class Listener: Merchandise Hierarchy Delete Errors")),
                    Times.Once);

            this.mockService
                .Verify(s => s.ProcessHierarchyClasses(It.IsAny<DeleteMerchandiseClassRequest>()),
                    Times.Once);
        }

        [TestMethod]
        public void ValidateItemAssociationDeleteMerchandiseHierarchyService_NoItemsAssociated_NoErrorLoggedOrEmailed()
        {
            // Given
            this.mockGetItemsByMerchandiseHierarchyQuery
                .Setup(q => q.Search(It.IsAny<GetItemsByMerchandiseHierarchyIdParameter>()))
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
                .Verify(s => s.ProcessHierarchyClasses(It.IsAny<DeleteMerchandiseClassRequest>()),
                    Times.Once);
        }

        [TestMethod]
        public void ValidateItemAssociationDeleteMerchandiseHierarchyService_NoItemsAssociated_RequestUnchangedAndServiceCalled()
        {
            // Given
            this.mockGetItemsByMerchandiseHierarchyQuery
                .Setup(q => q.Search(It.IsAny<GetItemsByMerchandiseHierarchyIdParameter>()))
                .Returns(new List<Item>());

            // When
            this.decorator.ProcessHierarchyClasses(this.request);

            // Then
            this.mockService
                .Verify(s => s.ProcessHierarchyClasses(It.Is<DeleteMerchandiseClassRequest>(r =>
                    r.HierarchyClasses.Select(hc => hc.HierarchyClassId).Contains(this.merchandiseHierarchies[0].HierarchyClassId)
                    && r.HierarchyClasses.Select(hc => hc.HierarchyClassId).Contains(this.merchandiseHierarchies[1].HierarchyClassId))),
                    Times.Once);
        }
    }
}
