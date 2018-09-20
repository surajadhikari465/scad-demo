using KitBuilderWebApi.Controllers;
using KitBuilderWebApi.DataAccess.Dto;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DataAccess.UnitOfWork;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.Helper;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace KitBuilderWebApi.Tests.Controllers
{
    [TestClass]
    public class KitControllerTests
    {
        private KitController kitController;
        private Mock<IRepository<LinkGroup>> mockLinkGroupRepository;
        private Mock<IRepository<Kit>> mockKitRepository;
        private Mock<IRepository<KitLinkGroup>> mockKitLinkGroupRepository;
        private Mock<IRepository<KitLocale>> mockKitLocaleRepository;
        private Mock<IRepository<LinkGroupItem>> mockLinkGroupItemRepository;
        private Mock<IRepository<Items>> mockItemsRepository;
        private Mock<IRepository<KitInstructionList>> mockKitInstructionListRepository;
        private Mock<IRepository<KitLinkGroupItem>> mockKitLinkGroupItemRepository;
        private Mock<IHelper<KitDto, KitSearchParameters>> mockKitHelper;
        private Mock<ILogger<KitController>> mockLogger;
        private Mock<IUnitOfWork> mockUnitWork;
        private List<Kit> kits;
        private List<Items> items;
        private List<LinkGroup> linkGroups;
        private List<KitLinkGroup> kitLinkGroups;
        private List<KitDto> kitsDto;


        [TestInitialize]
        public void InitializeTests()
        {
            mockLogger = new Mock<ILogger<KitController>>();
            mockKitLinkGroupItemRepository = new Mock<IRepository<KitLinkGroupItem>>();
            mockKitInstructionListRepository = new Mock<IRepository<KitInstructionList>>();
            mockItemsRepository = new Mock<IRepository<Items>>();
            mockLinkGroupItemRepository = new Mock<IRepository<LinkGroupItem>>();
            mockKitLinkGroupItemRepository = new Mock<IRepository<KitLinkGroupItem>>();
            mockKitLinkGroupRepository = new Mock<IRepository<KitLinkGroup>>();
            mockKitRepository = new Mock<IRepository<Kit>>();
            mockLinkGroupRepository = new Mock<IRepository<LinkGroup>>();
            mockKitLocaleRepository = new Mock<IRepository<KitLocale>>();

            string locationUrl = "http://localhost:55873/api/Kits/";
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);

            mockKitHelper = new Mock<IHelper<KitDto, KitSearchParameters>>();

            kitController = new KitController(mockKitRepository.Object,
                mockKitInstructionListRepository.Object, 
                mockKitLocaleRepository.Object, 
                mockLinkGroupRepository.Object, 
                mockLinkGroupItemRepository.Object, 
                mockItemsRepository.Object,
                mockKitLinkGroupRepository.Object,
                mockKitLinkGroupItemRepository.Object,
                mockLogger.Object,
                mockKitHelper.Object);

            MappingHelper.InitializeMapper();
            SetUpDataAndRepository();

        }

        [TestCleanup]
        public void Cleanup()
        {
            MappingHelper.CleanupMapper();
        }

        private void SetUpDataAndRepository()
        {
            kits = new List<Kit>()
            {
                new Kit() {KitId=1,Description = "Kit 1", ItemId = 1},
                new Kit() {KitId=2, Description = "Kit 2", ItemId = 2},
                new Kit() {KitId=3,Description = "Kit 3", ItemId = 3}
            };

            items = new List<Items>
            {
                new Items{ItemId=1, ScanCode="4001", ProductDesc="Baguette", CustomerFriendlyDesc = "Baguette", KitchenDesc="Baguette" },
                new Items{ItemId=2, ScanCode="4002", ProductDesc="Ciabatta Roll", CustomerFriendlyDesc = "Ciabatta Roll", KitchenDesc="Ciabatta Roll" },
                new Items{ItemId=3, ScanCode="4003", ProductDesc="Flour Tortilla", CustomerFriendlyDesc = "Flour Tortilla", KitchenDesc="Flour Tortilla" },
                new Items{ItemId=4, ScanCode="4004", ProductDesc="Basil", CustomerFriendlyDesc = "Basil", KitchenDesc="Basil" },
                new Items{ItemId=5, ScanCode="4005", ProductDesc="Carrots", CustomerFriendlyDesc = "Carrots", KitchenDesc="Carrots" },
                new Items{ItemId=6, ScanCode="4006", ProductDesc="Lettuce", CustomerFriendlyDesc = "Lettuce", KitchenDesc="Lettuce" },
            };

            linkGroups = new List<LinkGroup>()
            {
                new LinkGroup() {LinkGroupId = 1, GroupName = "Bread", GroupDescription = "Bread Options"},
                new LinkGroup() {LinkGroupId = 2, GroupName = "Proteins", GroupDescription = "Protein Options"},
                new LinkGroup() {LinkGroupId = 3, GroupName = "Toppings", GroupDescription = "Topping Options"},
            };

            kitLinkGroups = new List<KitLinkGroup>()
            {
                new KitLinkGroup() {KitLinkGroupId = 1, KitId = 1, LinkGroupId = 1},
                new KitLinkGroup() {KitLinkGroupId = 2, KitId = 2, LinkGroupId = 3},
            };

            kitsDto = (from k in kits
                select new KitDto()
                {
                    Description = k.Description,
                    InsertDate = k.InsertDate,
                    InstructionListId = k.InstructionListId,
                    ItemId = k.ItemId,
                    KitId = k.KitId
                }).ToList();


            mockItemsRepository.Setup(m => m.GetAll()).Returns(items.AsQueryable());
            mockKitRepository.Setup(m => m.GetAll()).Returns(kits.AsQueryable());
            mockLinkGroupRepository.Setup(m => m.GetAll()).Returns(linkGroups.AsQueryable());
            mockKitLinkGroupRepository.Setup(m => m.GetAll()).Returns(kitLinkGroups.AsQueryable());

        }

        [TestMethod]
        public void KitsController_GetItemsNoParametersPassed_ReturnsOK()
        {
            var kitSearchParameters = new KitSearchParameters();
            var kitsBeforePaging = kitsDto.AsQueryable();
            string orderBy = "Description";
            var headerDictionary = new HeaderDictionary();
            var mockResponse = new Mock<HttpResponse>();
            mockResponse.SetupGet(r => r.Headers).Returns(headerDictionary);
            var httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(a => a.Response).Returns(mockResponse.Object);
            kitController.ControllerContext = new ControllerContext();
            kitController.ControllerContext.HttpContext = httpContext.Object;

            mockKitHelper.Setup(s => s.SetOrderBy(It.IsAny<IQueryable<KitDto>>(), kitSearchParameters)).Returns(kitsBeforePaging.OrderBy(orderBy));

            //When
            var response = kitController.GetKits(kitSearchParameters);

            Assert.IsInstanceOfType(response, typeof(OkObjectResult), "Ok Request Expected");
        }

        [TestMethod]
        public void KitsController_Save_InvalidKitId_ReturnsBadRequest()
        {
            var kitSaveParameters = new KitSaveParameters()
            {
                KitId = 99,
                KitDescription = "Bad Kit",
                KitItem = 99,
                LinkGroupItemIds = new List<int>(),
                LinkGroupIds = new List<int>(),
                InstructionListIds = new List<int>()

            };

            var response = kitController.KitSaveDetails(kitSaveParameters);

            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            Assert.IsTrue(((BadRequestObjectResult)response).Value.ToString().Contains("Unable to find Kit"), "Missing Kit Expected");

        }

        [TestMethod]
        public void KitsController_Save_InvalidItemId_ReturnsBadRequest()
        {
            var kitSaveParameters = new KitSaveParameters()
            {
                KitId = 1,
                KitDescription = "Bad Kit",
                KitItem = 99, 
                LinkGroupItemIds = new List<int>(),
                LinkGroupIds= new List<int>(),
                InstructionListIds =  new List<int>()

            };

            var response = kitController.KitSaveDetails(kitSaveParameters);
            
            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            Assert.IsTrue(((BadRequestObjectResult)response).Value.ToString().Contains("Unable to find Item"), "Missing Item Expected");

        }

        [TestMethod]
        public void KitsController_Save_InvalidLinkGroupItemIds_ReturnsBadRequest()
        {
            var kitSaveParameters = new KitSaveParameters()
            {
                KitId = 1,
                KitDescription = "Bad Kit",
                KitItem = 99,
                LinkGroupItemIds = null, 
                LinkGroupIds = new List<int>(),
                InstructionListIds = new List<int>()

            };

            var response = kitController.KitSaveDetails(kitSaveParameters);

            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            Assert.IsTrue(((BadRequestObjectResult)response).Value.ToString().Contains("LinkGroupItemIds"), "LinkGroupItemIds parameter validation");

        }

        [TestMethod]
        public void KitsController_Save_InvalidLinkGroupIds_ReturnsBadRequest()
        {
            var kitSaveParameters = new KitSaveParameters()
            {
                KitId = 1,
                KitDescription = "Bad Kit",
                KitItem = 99,
                LinkGroupItemIds = new List<int>(),
                LinkGroupIds = null,
                InstructionListIds = new List<int>()

            };

            var response = kitController.KitSaveDetails(kitSaveParameters);

            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            Assert.IsTrue(((BadRequestObjectResult)response).Value.ToString().Contains("LinkGroupIds"), "LinkGroupIds parameter validation");

        }
        [TestMethod]
        public void KitsController_Save_InvalidInstructionListIds_ReturnsBadRequest()
        {
            var kitSaveParameters = new KitSaveParameters()
            {
                KitId = 1,
                KitDescription = "Bad Kit",
                KitItem = 99,
                LinkGroupItemIds = new List<int>(),
                LinkGroupIds = new List<int>(),
                InstructionListIds = null

            };

            var response = kitController.KitSaveDetails(kitSaveParameters);

            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            Assert.IsTrue(((BadRequestObjectResult)response).Value.ToString().Contains("InstructionListIds"), "InstructionListIds parameter validation");

        }



    }
}
