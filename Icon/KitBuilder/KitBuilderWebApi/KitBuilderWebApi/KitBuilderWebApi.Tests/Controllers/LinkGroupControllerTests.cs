using System;
using KitBuilderWebApi.Controllers;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.Helper;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Http;
using KitBuilderWebApi.DataAccess.Dto;
using AutoMapper;
using System.Linq.Dynamic.Core;
using KitBuilderWebApi.DataAccess.UnitOfWork;

namespace KitBuilderWebApi.Tests.Controllers
{
    [TestClass]
    public class LinkGroupControllerTests
    {
        private LinkGroupController linkGroupController;
        private Mock<ILogger<LinkGroupController>> mockLogger;
        private Mock<IRepository<LinkGroup>> mockLinkGroupRepository;
        private Mock<IRepository<LinkGroupItem>> mockLinkGroupItemRepository;
        private Mock<IRepository<Items>> mockItemsRepository;
        private Mock<IHelper<LinkGroupDto, LinkGroupParameters>> mockLinkGroupHelper;
        private Mock<IRepository<KitLinkGroupItem>> mockKitlinkGroupItemRepository;
        private Mock<IRepository<KitLinkGroup>> mockKitlinkGroupRepository;
        IQueryable<LinkGroup> queryableLinkGroup;
        Mock<IUnitOfWork> _mockUnitWork;
        IList<LinkGroup> linkGroups;
        IList<LinkGroupDto> linkGroupsDto;

        [TestInitialize]
        public void InitializeTest()
        {
            mockLogger = new Mock<ILogger<LinkGroupController>>();
            mockLinkGroupRepository = new Mock<IRepository<LinkGroup>>();
            mockLinkGroupItemRepository = new Mock<IRepository<LinkGroupItem>>();
            mockItemsRepository = new Mock<IRepository<Items>>();
            mockKitlinkGroupItemRepository = new Mock<IRepository<KitLinkGroupItem>>();
            mockKitlinkGroupRepository = new Mock<IRepository<KitLinkGroup>>();
            _mockUnitWork = new Mock<IUnitOfWork>();

            string locationUrl = "http://localhost:55873/api/LinkGroups/";
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);

            mockLinkGroupHelper = new Mock<IHelper<LinkGroupDto, LinkGroupParameters>>();

            linkGroupController = new LinkGroupController(mockLinkGroupRepository.Object,
                                                          mockLinkGroupItemRepository.Object,
                                                          mockItemsRepository.Object,
                                                          mockKitlinkGroupRepository.Object,
                                                          mockLogger.Object,
                                                          mockLinkGroupHelper.Object);

            linkGroupController.Url = mockUrlHelper.Object;

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
                                 InsertDate = l.InsertDate
                             }).ToList();

            mockLinkGroupRepository.Setup(m => m.GetAll()).Returns(linkGroups.AsQueryable());
        }

        [TestMethod]
        public void LinkGroupController_GetLinkGroupsNoParametersPassed_ReturnsOK()
        {   // Given

            var LinkGroupParameters = new LinkGroupParameters();
            var linkGroupListBeforePaging = linkGroupsDto.AsQueryable();
            string orderBy = "GroupName";
            var headerDictionary = new HeaderDictionary();
            var mockResponse = new Mock<HttpResponse>();
            mockResponse.SetupGet(r => r.Headers).Returns(headerDictionary);
            var httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(a => a.Response).Returns(mockResponse.Object);
            linkGroupController.ControllerContext = new ControllerContext();
            linkGroupController.ControllerContext.HttpContext = httpContext.Object;

            mockLinkGroupHelper.Setup(s => s.SetOrderBy(linkGroupListBeforePaging, LinkGroupParameters)).Returns(linkGroupListBeforePaging.OrderBy(orderBy));

            //When
            var response = linkGroupController.GetLinkGroups(LinkGroupParameters);

            // Then
            Assert.IsInstanceOfType(response, typeof(OkObjectResult), "Ok Request Expected");
        }

        [TestMethod]
        public void LinkGroupController_GetLinkGroupsParametersPassedWithInvalidOrderBy_ReturnsBadRequest()
        {   // Given
            var LinkGroupParameters = new LinkGroupParameters();
            LinkGroupParameters.OrderBy = "InvalidField";
            var linkGroupListBeforePaging = linkGroupsDto.AsQueryable();
            var headerDictionary = new HeaderDictionary();
            var mockResponse = new Mock<HttpResponse>();
            mockResponse.SetupGet(r => r.Headers).Returns(headerDictionary);
            var httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(a => a.Response).Returns(mockResponse.Object);
            linkGroupController.ControllerContext = new ControllerContext();
            linkGroupController.ControllerContext.HttpContext = httpContext.Object;

            mockLinkGroupHelper.Setup(s => s.SetOrderBy(It.IsAny<IQueryable<LinkGroupDto>>(), LinkGroupParameters)).Throws(new Exception("Invalid Order By"));

            //When
            var response = linkGroupController.GetLinkGroups(LinkGroupParameters);

            // Then
            Assert.IsInstanceOfType(response, typeof(BadRequestResult), "Bad Request Expected");
        }

        [TestMethod]
        public void LinkGroupController_GetLinkGroupByIdlinkGroupDoesNotExist_ReturnsNotFound()
        {   // Given
            int linkGroupId = 999;

            //When
            var response = linkGroupController.GetLinkGroupById(linkGroupId, false);

            // Then
            Assert.IsInstanceOfType(response, typeof(NotFoundResult), "Not Found Expected");
        }

        [TestMethod]
        public void LinkGroupController_GetLinkGroupByIdlinkGroupExist_ReturnsOk()
        {   // Given
            int linkGroupId = linkGroups.FirstOrDefault().LinkGroupId;
            InitializeMapper();

            //When
            var response = linkGroupController.GetLinkGroupById(linkGroupId, false);

            // Then
            Assert.IsInstanceOfType(response, typeof(OkObjectResult), "Ok Result Expected");
            Assert.IsNotNull(response, "The OkResult response is null.");

            Mapper.Reset();
        }

        [TestMethod]
        public void LinkGroupController_CreateLinkGroupNullLinkGroupPassed_ReturnsBadRequest()
        {   // Given
            LinkGroupDto linkGroupDto = null;

            //When
            var response = linkGroupController.CreateLinkGroup(linkGroupDto);

            // Then
            Assert.IsInstanceOfType(response, typeof(BadRequestResult), "Bad Request Expected");
        }

        [TestMethod]
        public void LinkGroupController_CreateLinkGroupValidLinkGroupPassed_ReturnsCreatedStatusCode()
        {   // Given
            LinkGroupDto linkGroupDto = new LinkGroupDto { LinkGroupId = 999, GroupName = "Taco2", GroupDescription = "Cheese taco2" };
            InitializeMapper();
            mockLinkGroupRepository.SetupGet(s => s.UnitOfWork).Returns(_mockUnitWork.Object);

            //When
            var response = linkGroupController.CreateLinkGroup(linkGroupDto);

            // Then
            Assert.IsInstanceOfType(response, typeof(CreatedAtRouteResult), "Bad Request Expected");
            _mockUnitWork.Verify(m => m.Commit(), Times.Once);
            Mapper.Reset();
        }

        [TestMethod]
        public void LinkGroupController_DeleteLinkGroupInvalidIdPassed_ReturnsNotFound()
        {   // Given
            int linkGroupId = 999;

            //When
            var response = linkGroupController.DeleteLinkGroup(linkGroupId);

            // Then
            Assert.IsInstanceOfType(response, typeof(NotFoundResult), "Not Found Request Expected");
        }

        [TestMethod]
        public void LinkGroupController_DeleteLinkGroupValidLinkGroupPassed_ReturnsNoContent()
        {   // Given
            int linkGroupId = linkGroups.FirstOrDefault().LinkGroupId;
            mockLinkGroupRepository.Setup(s => s.ExecWithStoreProcedure(It.IsAny<string>(), It.IsAny<object[]>())).Returns(1);
            InitializeMapper();
            mockLinkGroupRepository.SetupGet(s => s.UnitOfWork).Returns(_mockUnitWork.Object);

            //When
            var response = linkGroupController.DeleteLinkGroup(linkGroupId);

            // Then
            Assert.IsInstanceOfType(response, typeof(NoContentResult), "No Content Result Expected");
            Mapper.Reset();
        }

    }
}