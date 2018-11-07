using System;
using KitBuilderWebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Linq.Expressions;
using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Dto;
using KitBuilder.DataAccess.Repository;
using KitBuilder.DataAccess.UnitOfWork;

namespace KitBuilderWebApi.Tests.Controllers
{
    [TestClass]
    public class LinkGroupItemControllerTests
    {
        private LinkGroupItemController listGroupItemController;
        private Mock<ILogger<LinkGroupController>> mockLogger;
        private Mock<IRepository<LinkGroup>> mockLinkGroupRepository;
        private Mock<IRepository<LinkGroupItem>> mockLinkGroupItemRepository;
        private Mock<IRepository<Items>> mockItemsRepository;
        Mock<IUnitOfWork> _mockUnitWork;

        IList<LinkGroup> linkGroups;
        IList<LinkGroupItem> linkGroupItems;
        IList<LinkGroupDto> linkGroupsDto;

        [TestCleanup]
        public void TestCleanUp()
        {
            Mapper.Reset();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            mockLogger = new Mock<ILogger<LinkGroupController>>();
            mockLinkGroupRepository = new Mock<IRepository<LinkGroup>>();
            mockLinkGroupItemRepository = new Mock<IRepository<LinkGroupItem>>();
            mockItemsRepository = new Mock<IRepository<Items>>();
            _mockUnitWork = new Mock<IUnitOfWork>();

            string locationUrl = "http://localhost:55873/api/LinkGroups/";
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);

            listGroupItemController = new LinkGroupItemController(mockLinkGroupRepository.Object,
                                                                  mockLinkGroupItemRepository.Object,
                                                                  mockItemsRepository.Object,
                                                                  mockLogger.Object);

            listGroupItemController.Url = mockUrlHelper.Object;
            InitializeMapper();
            SetUpDataAndRepository();
        }
        private void InitializeMapper()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<LinkGroup, LinkGroupDto>()
                 .ForMember(dest => dest.LinkGroupItemDto, conf => conf.MapFrom(src => src.LinkGroupItem));

                cfg.CreateMap<LinkGroupDto, LinkGroup>();
                cfg.CreateMap<LinkGroupItem, LinkGroupItemDto>();
                cfg.CreateMap<LinkGroupItemDto, LinkGroupItem>();
                cfg.CreateMap<Items, ItemsDto>();
                cfg.CreateMap<ItemsDto, Items>();
                cfg.CreateMap<InstructionList, InstructionListDto>();
                cfg.CreateMap<InstructionListDto, InstructionList>();
            });
        }
        private void SetUpDataAndRepository()
        {
            linkGroupsDto = new List<LinkGroupDto>();

            IList<Items> items = new List<Items>
            {
                new Items{ItemId=1, ScanCode="4001", ProductDesc="Baguette", CustomerFriendlyDesc = "Baguette", KitchenDesc="Baguette" },
                new Items{ItemId=2, ScanCode="4002", ProductDesc="Ciabatta Roll", CustomerFriendlyDesc = "Ciabatta Roll", KitchenDesc="Ciabatta Roll" },
                new Items{ItemId=3, ScanCode="4003", ProductDesc="Flour Tortilla", CustomerFriendlyDesc = "Flour Tortilla", KitchenDesc="Flour Tortilla" },
                new Items{ItemId=4, ScanCode="4004", ProductDesc="Basil", CustomerFriendlyDesc = "Basil", KitchenDesc="Basil" },
                new Items{ItemId=5, ScanCode="4005", ProductDesc="Carrots", CustomerFriendlyDesc = "Carrots", KitchenDesc="Carrots" },
                new Items{ItemId=6, ScanCode="4006", ProductDesc="Lettuce", CustomerFriendlyDesc = "Lettuce", KitchenDesc="Lettuce" },
            };

            linkGroups = new List<LinkGroup>
           {
               new LinkGroup{ LinkGroupId=1, GroupName = "Taco", GroupDescription = "Cheese taco"},
               new LinkGroup{ LinkGroupId=2, GroupName = "Topping", GroupDescription = "Topping"},
               new LinkGroup{ LinkGroupId=3, GroupName = "Add Cheese", GroupDescription = "Add Cheese"},
           };

            linkGroupItems = new List<LinkGroupItem>
            {
                new LinkGroupItem{ LinkGroupItemId= 1, LinkGroupId= 1, ItemId= 3 },
                new LinkGroupItem{ LinkGroupItemId= 2, LinkGroupId= 1, ItemId= 1 },
                new LinkGroupItem{ LinkGroupItemId= 3, LinkGroupId= 2, ItemId= 6 },
                new LinkGroupItem{ LinkGroupItemId= 4, LinkGroupId= 1, ItemId= 2 },
                new LinkGroupItem{ LinkGroupItemId= 5, LinkGroupId= 2, ItemId= 5 },
            };

            int count = 1;
            foreach (LinkGroup linkGroup in linkGroups)
            {
                foreach (Items item in items)
                {
                    linkGroup.LinkGroupItem.Add(new LinkGroupItem { LinkGroupItemId = count, LinkGroupId = linkGroup.LinkGroupId, ItemId = item.ItemId });
                    count = count + 1;
                }
            }

            linkGroupsDto = (from l in linkGroups
                             select new LinkGroupDto()
                             {
                                 LinkGroupId = l.LinkGroupId,
                                 GroupName = l.GroupName,
                                 GroupDescription = l.GroupDescription,
                                 InsertDateUtc = l.InsertDateUtc
                             }).ToList();
        }

        [TestMethod]
        public void LinkGroupItemController_GetLinkGroupItemLinkGroupItemDoesNotexists_ReturnsNotFound()
        {
            //Given
            int linkGroupId = 999;
            int linkGroupItemId = 999;

            //When
            var response = listGroupItemController.GetLinkGroupItem(linkGroupId, linkGroupItemId);

            //Then
            Assert.IsInstanceOfType(response, typeof(NotFoundResult), "Not Found Expected");

        }

        //Success
        [TestMethod]
        public void LinkGroupItemController_GetLinkGroupItemValidDataPassed_ReturnsOK()
        {
            //Given
            int linkGroupId = 2;
            int linkGroupItemId = 1;
            mockLinkGroupItemRepository.Setup(m => m.Get(It.IsAny<int>())).Returns(linkGroupItems.Where(l => l.LinkGroupId == 2).FirstOrDefault());

            //When
            var response = listGroupItemController.GetLinkGroupItem(linkGroupId, linkGroupItemId);

            //Then
            Assert.IsInstanceOfType(response, typeof(OkObjectResult), "Ok Object Expected");
        }

        [TestMethod]
        public void LinkGroupItemController_CreateLinkGroupItemNoParametersPassed_ReturnsBadRequest()
        {
            //Given
            int linkGroupId = 10;
            LinkGroupItemDto linkGroupItemDto = null;

            //When
            var response = listGroupItemController.CreateLinkGroupItem(linkGroupId, linkGroupItemDto);

            // Then
            Assert.IsInstanceOfType(response, typeof(BadRequestResult), "Bad Request Expected");
        }

        [TestMethod]
        public void LinkGroupItemController_CreateLinkGroupItemLinkGroupDoesNotExists_ReturnsNotFound()
        {
            // Given
            int linkGroupId = 999;
            LinkGroupItemDto linkGroupItemDto = new LinkGroupItemDto() { LinkGroupItemId = 1, LinkGroupId = 1, ItemId = 3 };

            //When
            var response = listGroupItemController.CreateLinkGroupItem(linkGroupId, linkGroupItemDto);

            // Then
            Assert.IsInstanceOfType(response, typeof(NotFoundResult), "Record Not Found Expected");
        }

        [TestMethod]
        public void LinkGroupItemController_CreateLinkGroupItemValidParametersPassed_Returns_Success()
        {
            // Given
            int linkGroupId = 1;
            LinkGroupItemDto linkGroupItemDto = new LinkGroupItemDto() { LinkGroupItemId = 999, LinkGroupId = 1, ItemId = 3 };
            mockLinkGroupRepository.Setup(m => m.Get(It.IsAny<int>())).Returns(linkGroups.Where(l => l.LinkGroupId == linkGroupId).FirstOrDefault());
            mockLinkGroupRepository.SetupGet(s => s.UnitOfWork).Returns(_mockUnitWork.Object);

            //When
            var response = listGroupItemController.CreateLinkGroupItem(linkGroupId, linkGroupItemDto);

            // Then
            Assert.IsInstanceOfType(response, typeof(StatusCodeResult), "Status Code Result Expected");
            _mockUnitWork.Verify(m => m.Commit(), Times.Once);
        }

        [TestMethod]
        public void LinkGroupItemController_DeleteLinkGroupItemLinkGroupItemDoesNotExists_ReturnsNotFound()
        {
            // Given
            int linkGroupId = 999;
            int linkGroupItemId = 999;

            //When
            var response = listGroupItemController.DeleteLinkGroupItem(linkGroupId, linkGroupItemId);

            // Then
            Assert.IsInstanceOfType(response, typeof(NotFoundResult), "Record Not Found Expected");
        }

        [TestMethod]
        public void linkGroupItemController_DeleteLinkGroupItemValidDataPassed_ReturnsNoContent()
        {
            // Given
            int linkGroupId = 1;
            int linkGroupItemId = 1;
            mockLinkGroupRepository.Setup(m => m.Get(It.IsAny<int>())).Returns(linkGroups.Where(l => l.LinkGroupId == linkGroupId).FirstOrDefault());
            mockLinkGroupItemRepository.Setup(m => m.Get(It.IsAny<int>())).Returns(linkGroupItems.Where(l => l.LinkGroupItemId == linkGroupItemId).FirstOrDefault());
            mockLinkGroupItemRepository.SetupGet(s => s.UnitOfWork).Returns(_mockUnitWork.Object);

            //When
            var response = listGroupItemController.DeleteLinkGroupItem(linkGroupId, linkGroupItemId);

            // Then
            Assert.IsInstanceOfType(response, typeof(NoContentResult), "No Content Expected");
        }

        [TestMethod]
        public void LinkGroupItemController_CreateLinkGroupItemsNoParametersPassed_ReturnsBadRequest()
        {
            //Given
            int linkGroupId = 1;
            List<LinkGroupItemDto> linkGroupItemDto = null;

            //When
            var response = listGroupItemController.CreateLinkGroupItems(linkGroupId, linkGroupItemDto);

            // Then
            Assert.IsInstanceOfType(response, typeof(BadRequestResult), "Bad Request Expected");
        }

        // Record NotFound
        [TestMethod]
        public void linkGroupItemController_CreateLinkGroupItemsLinkGroupDoesNotExists_ReturnsNotFound()
        {
            // Given
            int linkGroupId = 999;
            List<LinkGroupItemDto> linkGroupItemDto = new List<LinkGroupItemDto>() {
                new LinkGroupItemDto { LinkGroupItemId= 1, LinkGroupId= 1, ItemId= 3 },
                new LinkGroupItemDto { LinkGroupItemId= 2, LinkGroupId= 2, ItemId= 3 },
                new LinkGroupItemDto { LinkGroupItemId= 3, LinkGroupId= 1, ItemId= 1 },
            };

            //When
            var response = listGroupItemController.CreateLinkGroupItems(linkGroupId, linkGroupItemDto);

            // Then
            Assert.IsInstanceOfType(response, typeof(NotFoundResult), "Record Not Found Expected");
        }

        [TestMethod]
        public void LinkGroupItemController_CreateLinkGroupItemValidParametersPassed_ReturnsSuccess()
        {
            // Given
            int linkGroupId = 1;
            List<LinkGroupItemDto> linkGroupItemDto = new List<LinkGroupItemDto>() {
                new LinkGroupItemDto { LinkGroupItemId= 999, LinkGroupId= 1, ItemId= 2 },
                new LinkGroupItemDto { LinkGroupItemId= 1000, LinkGroupId= 1, ItemId= 3 },
                new LinkGroupItemDto { LinkGroupItemId= 1001, LinkGroupId= 1, ItemId= 1 },
            };
            mockLinkGroupRepository.SetupGet(s => s.UnitOfWork).Returns(_mockUnitWork.Object);
            mockLinkGroupRepository.Setup(m => m.Get(It.IsAny<int>())).Returns(linkGroups.Where(l => l.LinkGroupId == linkGroupId).FirstOrDefault());

            //When
            var response = listGroupItemController.CreateLinkGroupItems(linkGroupId, linkGroupItemDto);

            // Then
            Assert.IsInstanceOfType(response, typeof(StatusCodeResult), "Status Code Expected");
        }

        [TestMethod]
        public void LinkGroupItemController_DeleteLinkGroupItemsLinkGroupDoesNotExists_ReturnsNotFound()
        {
            // Given
            int linkGroupId = 999;
            List<int> linkGroupItemIds = new List<int> { 1, 6, 3 };

            //When
            var response = listGroupItemController.DeleteLinkGroupItems(linkGroupId, linkGroupItemIds);

            // Then
            Assert.IsInstanceOfType(response, typeof(NotFoundResult), "Record Not Found Expected");
        }

        [TestMethod]
        public void linkGroupItemController_DeleteLinkGroupItemsValidParametersPassed_ReturnsNoContent()
        {
            // Given
            int linkGroupId = 1;
            List<int> linkGroupItemIds = new List<int> { 1, 2, 3 };
            var linkGroupItemsList = linkGroupItems.Where(l => linkGroupItemIds.Contains(l.LinkGroupItemId));
            mockLinkGroupItemRepository.SetupGet(s => s.UnitOfWork).Returns(_mockUnitWork.Object);
            mockLinkGroupRepository.SetupGet(s => s.UnitOfWork).Returns(_mockUnitWork.Object);
            mockLinkGroupRepository.Setup(m => m.Get(It.IsAny<int>())).Returns(linkGroups.Where(l => l.LinkGroupId == linkGroupId).FirstOrDefault());
            mockLinkGroupItemRepository.Setup(s => s.FindBy(It.IsAny<Expression<Func<LinkGroupItem, bool>>>())).Returns(linkGroupItemsList.AsQueryable());

            //When
            var response = listGroupItemController.DeleteLinkGroupItems(linkGroupId, linkGroupItemIds);

            // Then
            Assert.IsInstanceOfType(response, typeof(NoContentResult), "No Content Expected");
        }
    }
}