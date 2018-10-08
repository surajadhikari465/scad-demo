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
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using KitBuilderWebApi.DataAccess.Dto;
using AutoMapper;
using System.Linq.Dynamic.Core;
using KitBuilderWebApi.DataAccess.UnitOfWork;
namespace KitBuilderWebApi.Tests.Controllers
{
    [TestClass]
    public class ItemsControllerTests
    {
        private ItemsController itemsController;
        private Mock<ILogger<LinkGroupController>> mockLogger;
        private Mock<IRepository<Items>> mockItemsRepository;
        private Mock<IHelper<ItemsDto, ItemsParameters>> mockItemsHelper;
        Mock<IUnitOfWork> _mockUnitWork;
        IList<Items> items;
        IList<ItemsDto> itemsDto;

        [TestInitialize]
        public void InitializeTest()
        {
            mockLogger = new Mock<ILogger<LinkGroupController>>();
            mockItemsRepository = new Mock<IRepository<Items>>();
            _mockUnitWork = new Mock<IUnitOfWork>();

            string locationUrl = "http://localhost:55873/api/Items/";
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);

            mockItemsHelper = new Mock<IHelper<ItemsDto, ItemsParameters>>();

            itemsController = new ItemsController(mockItemsRepository.Object,
                                                  mockLogger.Object,
                                                  mockItemsHelper.Object);

            itemsController.Url = mockUrlHelper.Object;
            InitializeMapper();
            SetUpDataAndRepository();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            Mapper.Reset();
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
            items = new List<Items>
            {
                new Items{ItemId=1, ScanCode="4001", ProductDesc="Baguette", CustomerFriendlyDesc = "Baguette", KitchenDesc="Baguette" },
                new Items{ItemId=2, ScanCode="4002", ProductDesc="Ciabatta Roll", CustomerFriendlyDesc = "Ciabatta Roll", KitchenDesc="Ciabatta Roll" },
                new Items{ItemId=3, ScanCode="4003", ProductDesc="Flour Tortilla", CustomerFriendlyDesc = "Flour Tortilla", KitchenDesc="Flour Tortilla" },
                new Items{ItemId=4, ScanCode="4004", ProductDesc="Basil", CustomerFriendlyDesc = "Basil", KitchenDesc="Basil" },
                new Items{ItemId=5, ScanCode="4005", ProductDesc="Carrots", CustomerFriendlyDesc = "Carrots", KitchenDesc="Carrots" },
                new Items{ItemId=6, ScanCode="4006", ProductDesc="Lettuce", CustomerFriendlyDesc = "Lettuce", KitchenDesc="Lettuce" },
            };

            itemsDto = (from l in items
                        select new ItemsDto()
                        {
                            ItemId = l.ItemId,
                            ScanCode = l.ScanCode,
                            ProductDesc = l.ProductDesc,
                            CustomerFriendlyDesc = l.CustomerFriendlyDesc,
                            KitchenDesc = l.KitchenDesc,
                            BrandName = l.BrandName,
                            LargeImageUrl = l.LargeImageUrl,
                            SmallImageUrl = l.SmallImageUrl,
                            InsertDateUtc = l.InsertDateUtc
                        }).ToList();

            mockItemsRepository.Setup(m => m.GetAll()).Returns(items.AsQueryable());
        }

        [TestMethod]
        public void ItemsController_GetItemsNoParametersPassed_ReturnsOK()
        {   
            // Given
            var itemsParameters = new ItemsParameters();
            var itemsListBeforePaging = itemsDto.AsQueryable();
            string orderBy = "ScanCode";
            var headerDictionary = new HeaderDictionary();
            var mockResponse = new Mock<HttpResponse>();
            mockResponse.SetupGet(r => r.Headers).Returns(headerDictionary);
            var httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(a => a.Response).Returns(mockResponse.Object);
            itemsController.ControllerContext = new ControllerContext();
            itemsController.ControllerContext.HttpContext = httpContext.Object;

            mockItemsHelper.Setup(s => s.SetOrderBy(It.IsAny<IQueryable<ItemsDto>>(), itemsParameters)).Returns(itemsListBeforePaging.OrderBy(orderBy));

            //When
            var response = itemsController.GetItems(itemsParameters);

            // Then
            Assert.IsInstanceOfType(response, typeof(OkObjectResult), "Ok Request Expected");
        }

        [TestMethod]
        public void ItemsController_GetItemsParametersPassedWithInvalidOrderBy_ReturnsBadRequest()
        {   
            // Given
            var itemsParameters = new ItemsParameters();
            var itemsListBeforePaging = itemsDto.AsQueryable();
            var headerDictionary = new HeaderDictionary();
            var mockResponse = new Mock<HttpResponse>();
            mockResponse.SetupGet(r => r.Headers).Returns(headerDictionary);
            var httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(a => a.Response).Returns(mockResponse.Object);
            itemsController.ControllerContext = new ControllerContext();
            itemsController.ControllerContext.HttpContext = httpContext.Object;
            mockItemsHelper.Setup(s => s.SetOrderBy(It.IsAny<IQueryable<ItemsDto>>(), itemsParameters)).Throws(new Exception("Invalid Order By"));

            //When
            var response = itemsController.GetItems(itemsParameters);

            // Then
            Assert.IsInstanceOfType(response, typeof(BadRequestResult), "Bad Request Expected");
        }
    }
}