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
        public void linkGroupController_GetInstructionsList_NoParametersPassed_Returns_OK()
        {   // Given

            var LinkGroupParameters = new LinkGroupParameters();
            var linkGroupListBeforePaging = linkGroupsDto.AsQueryable();
            string orderBy =  "GroupName" ;
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

        public void linkGroupController_GetInstructionsList_ParametersPassedWithInvalidOrderBy_Returns_OK()
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

    }
}
