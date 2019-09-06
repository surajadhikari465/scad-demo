using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Dto;
using KitBuilder.DataAccess.Enums;
using KitBuilder.DataAccess.Repository;
using KitBuilder.DataAccess.UnitOfWork;
using KitBuilderWebApi.Controllers;
using KitBuilderWebApi.Helper;
using KitBuilderWebApi.Models;
using KitBuilderWebApi.QueryParameters;
using KitBuilderWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace KitBuilderWebApi.Tests.Controllers
{
    [TestClass]
    public class KitControllerTests
    {
        private Mock<IRepository<KitInstructionList>> mockKitInstructionListRepository;
        private Mock<IRepository<InstructionList>> mockInstrunctionListRepository;

        private Mock<IHelper<KitDtoWithStatus, KitSearchParameters>> mockKitHelper;

        private KitController kitController;
        private Mock<ILogger<KitController>> mockLogger;
        private Mock<IRepository<LinkGroup>> mockLinkGroupRepository;
        private Mock<IRepository<Kit>> mockKitRepository;
        private Mock<IRepository<Locale>> mockLocaleRepository;
        private Mock<IRepository<KitLocale>> mockKitLocaleRepository;
        private Mock<IRepository<LinkGroupItem>> mockLinkGroupItemRepository;
        private Mock<IRepository<Items>> mockItemsRepository;
        private Mock<IRepository<KitLinkGroup>> mockKitLinkGroupRepository;
        private Mock<IRepository<KitLinkGroupLocale>> mockKitLinkGroupLocaleRepository;
        private Mock<IRepository<KitLinkGroupItem>> mockKitLinkGroupItemRepository;
        private Mock<IRepository<LocaleType>> mockLocaleTypeRepository;
        private Mock<IRepository<KitLinkGroupItemLocale>> mockKitLinkGroupItemLocaleRepository;
        private Mock<IRepository<KitQueue>> mockkitQueueRepository;
        //private Mock<IQueryHandler<GetKitByKitLocaleIdParameters, KitLocale>> mockGetKitLocaleQuery;
        private Mock<IServiceProvider> mockServices;
        private Mock<IUnitOfWork> mockUnitWork;
        private List<Kit> kits;
        private List<Items> items;
        private List<LinkGroup> linkGroups;
        private List<KitLinkGroup> kitLinkGroups;
        private List<KitDtoWithStatus> kitsDto;
        IList<KitLinkGroupItem> KitLinkGroupItems;
        private List<KitLocale> kitLocaleList;
        private List<KitLinkGroupLocale> kitLinkGroupLocaleList;
        private List<AssignKitToLocaleDto> assignKitToLocaleDtoList;

        private Mock<IService<GetKitLocaleByStoreParameters, Task<KitLocaleDto>>> mockCalorieCalculator;
        [TestInitialize]
        public void InitializeTests()
        {
            mockKitInstructionListRepository = new Mock<IRepository<KitInstructionList>>();
            mockInstrunctionListRepository = new Mock<IRepository<InstructionList>>();
            mockLogger = new Mock<ILogger<KitController>>();
            mockKitLinkGroupItemRepository = new Mock<IRepository<KitLinkGroupItem>>();
            mockLinkGroupRepository = new Mock<IRepository<LinkGroup>>();
            mockKitRepository = new Mock<IRepository<Kit>>();
            mockLocaleRepository = new Mock<IRepository<Locale>>();
            mockKitLocaleRepository = new Mock<IRepository<KitLocale>>();
            //mockGetKitLocaleQuery = new Mock<IQueryHandler<GetKitByKitLocaleIdParameters, KitLocale>>();
            mockLinkGroupItemRepository = new Mock<IRepository<LinkGroupItem>>();
            mockItemsRepository = new Mock<IRepository<Items>>();
            mockKitLinkGroupRepository = new Mock<IRepository<KitLinkGroup>>();
            mockKitLinkGroupLocaleRepository = new Mock<IRepository<KitLinkGroupLocale>>();
            mockKitLinkGroupItemRepository = new Mock<IRepository<KitLinkGroupItem>>();
            mockLocaleTypeRepository = new Mock<IRepository<LocaleType>>();
            mockKitLinkGroupItemLocaleRepository = new Mock<IRepository<KitLinkGroupItemLocale>>();
            mockLocaleTypeRepository = new Mock<IRepository<LocaleType>>();
            mockServices = new Mock<IServiceProvider>();
            mockCalorieCalculator = new Mock<IService<GetKitLocaleByStoreParameters, Task<KitLocaleDto>>>();
            mockUnitWork = new Mock<IUnitOfWork>();
            mockkitQueueRepository = new Mock<IRepository<KitQueue>>();


            string locationUrl = "http://localhost:55873/api/Kits/";
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);

            mockKitHelper = new Mock<IHelper<KitDtoWithStatus, KitSearchParameters>>();

            kitController = new KitController(mockLinkGroupRepository.Object,
                mockInstrunctionListRepository.Object,
                mockKitRepository.Object,
                mockLocaleRepository.Object,
                mockKitLocaleRepository.Object,
                mockLinkGroupItemRepository.Object,
                mockItemsRepository.Object,
                mockKitLinkGroupRepository.Object,
                mockKitLinkGroupLocaleRepository.Object,
                mockKitLinkGroupItemRepository.Object,
                mockKitLinkGroupItemLocaleRepository.Object,
                mockLocaleTypeRepository.Object,
                mockKitInstructionListRepository.Object,
                mockLogger.Object,
                mockKitHelper.Object,
                mockServices.Object,
                mockCalorieCalculator.Object,
                mockkitQueueRepository.Object
                );

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
                new Kit() {KitId=1,Description = "Kit 1", ItemId = 1, KitType = KitType.Customizable},
                new Kit() {KitId=2, Description = "Kit 2", ItemId = 2, KitType = KitType.Customizable},
                new Kit() {KitId=3,Description = "Kit 3", ItemId = 3, KitType = KitType.Simple}
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

            //KitLinkGroupItems = new List<KitLinkGroupItem>
            //{
            //     new KitLinkGroupItem{ KitLinkGroupItemId=1, KitLinkGroupId = 1, LinkGroupItemId = 1},
            //     new KitLinkGroupItem{ KitLinkGroupItemId=2, KitLinkGroupId = 1, LinkGroupItemId = 7},
            //     new KitLinkGroupItem{ KitLinkGroupItemId=3, KitLinkGroupId = 1, LinkGroupItemId = 8},
            //     new KitLinkGroupItem{ KitLinkGroupItemId=4, KitLinkGroupId = 1, LinkGroupItemId = 2}
            //};

            foreach (KitLinkGroup kitLinkGroup in kitLinkGroups)
            {
                if (kitLinkGroup.KitLinkGroupId == 1)
                {
                    kitLinkGroup.KitLinkGroupItem.Add(new KitLinkGroupItem { KitLinkGroupItemId = 1, KitLinkGroupId = 1, LinkGroupItemId = 1 });
                    kitLinkGroup.KitLinkGroupItem.Add(new KitLinkGroupItem { KitLinkGroupItemId = 2, KitLinkGroupId = 1, LinkGroupItemId = 2 });
                    kitLinkGroup.KitLinkGroupItem.Add(new KitLinkGroupItem { KitLinkGroupItemId = 3, KitLinkGroupId = 1, LinkGroupItemId = 3 });
                    kitLinkGroup.KitLinkGroupItem.Add(new KitLinkGroupItem { KitLinkGroupItemId = 4, KitLinkGroupId = 1, LinkGroupItemId = 4 });
                }

                if (kitLinkGroup.KitLinkGroupId == 2)
                {
                    kitLinkGroup.KitLinkGroupItem.Add(new KitLinkGroupItem { KitLinkGroupItemId = 5, KitLinkGroupId = 2, LinkGroupItemId = 5 });
                    kitLinkGroup.KitLinkGroupItem.Add(new KitLinkGroupItem { KitLinkGroupItemId = 6, KitLinkGroupId = 2, LinkGroupItemId = 6 });
                    kitLinkGroup.KitLinkGroupItem.Add(new KitLinkGroupItem { KitLinkGroupItemId = 7, KitLinkGroupId = 2, LinkGroupItemId = 7 });
                    kitLinkGroup.KitLinkGroupItem.Add(new KitLinkGroupItem { KitLinkGroupItemId = 8, KitLinkGroupId = 2, LinkGroupItemId = 8 });
                }
            }

            List<LocaleType> localeTypeList = new List<LocaleType>
            {
                  new LocaleType { LocaleTypeCode = "C", LocaleTypeDesc = "Chain", LocaleTypeId =1 },
                  new LocaleType { LocaleTypeCode = "Re", LocaleTypeDesc = "Region", LocaleTypeId =2 },
                  new LocaleType { LocaleTypeCode = "M", LocaleTypeDesc = "Metro", LocaleTypeId =3 },
                  new LocaleType { LocaleTypeCode = "St", LocaleTypeDesc = "store", LocaleTypeId =4},
                  new LocaleType { LocaleTypeCode = "V", LocaleTypeDesc = "Venue", LocaleTypeId =5},
            };

            List<Locale> localeList = new List<Locale> {
                               new Locale { LocaleId = 1, LocaleName = "Texas", LocaleTypeId =2, ChainId=7},
                               new Locale { LocaleId = 6, LocaleName = "Kentukky", LocaleTypeId =2, ChainId=7 },
                               new Locale { LocaleId = 3, LocaleName = "Lamar", LocaleTypeId =4, MetroId = 4, RegionId=1 },
                               new Locale { LocaleId = 5, LocaleName = "Lamar5", LocaleTypeId =4,MetroId = 6, RegionId=1, ChainId=7 },
                               new Locale { LocaleId = 4, LocaleName = "Austin", LocaleTypeId =3,RegionId=1, ChainId=7 },
                               new Locale { LocaleId = 7, LocaleName = "Chain", LocaleTypeId =1},
                               new Locale { LocaleId = 8, LocaleName = "FL_MTR", LocaleTypeId =2 ,  RegionId = 1, ChainId= 7, RegionCode = "FL" },
							   //new Locale { LocaleId = 9, LocaleName = "FL_SON", LocaleTypeId =1 , StoreId = 2, MetroId = 5, RegionId = 1, ChainId= 4, StoreAbbreviation="FL", RegionCode = "FL_MTR", BusinessUnitId = 2 },
							   //new Locale { LocaleId = 10, LocaleName = "MD_CHI", LocaleTypeId =4 , StoreId = 1, MetroId = 2, RegionId = 1, ChainId= 7, StoreAbbreviation="MD", RegionCode = "MD_MTR", BusinessUnitId = 3 },
							   //new Locale { LocaleId = 11, LocaleName = "MD_MRT", LocaleTypeId =5 , StoreId = 2, MetroId = 8, RegionId = 1, ChainId= 3, StoreAbbreviation="MD", RegionCode = "MD_MTR", BusinessUnitId = 1 },
							   //new Locale { LocaleId = 12, LocaleName = "FL_MKT", LocaleTypeId =2 , StoreId = 3, MetroId = 1, RegionId = 1, ChainId= 1, StoreAbbreviation="FL", RegionCode = "FL_MTR", BusinessUnitId = 3 }
				};

            kitLocaleList = new List<KitLocale> {
                           new KitLocale { KitLocaleId = 1, KitId = 1, LocaleId = 1, MinimumCalories = 0, MaximumCalories = 0, Exclude = true, StatusId = 1 },
                           new KitLocale { KitLocaleId = 2, KitId = 1, LocaleId = 6, MinimumCalories = 0, MaximumCalories = 0, Exclude = true, StatusId = 1 },
                           new KitLocale { KitLocaleId = 3, KitId = 2, LocaleId = 3, MinimumCalories = 0, MaximumCalories = 0, Exclude = false, StatusId = 0 },
                           new KitLocale { KitLocaleId = 4, KitId = 2, LocaleId = 4, MinimumCalories = 0, MaximumCalories = 0, Exclude = false, StatusId = 0 },

             };

            assignKitToLocaleDtoList = new List<AssignKitToLocaleDto> {
                               new AssignKitToLocaleDto { LocaleId = localeList[0].LocaleId, LocaleTypeId =localeList[0].LocaleTypeId, IsExcluded = true, IsAssigned = false },
                               new AssignKitToLocaleDto {  LocaleId = localeList[1].LocaleId, LocaleTypeId = localeList[1].LocaleTypeId, IsExcluded = true, IsAssigned = false }
             };

            kitsDto = (from k in kits
                       select new KitDtoWithStatus()
                       {
                           Description = k.Description,
                           InsertDateUtc = k.InsertDateUtc,
                           ItemId = k.ItemId,
                           KitId = k.KitId
                       }).ToList();

            kitLinkGroupLocaleList = new List<KitLinkGroupLocale>
            {
                new KitLinkGroupLocale {KitLinkGroupLocaleId = 11, KitLinkGroupId= 1, KitLocaleId= 1, DisplaySequence=1}
            };

            mockItemsRepository.Setup(m => m.GetAll()).Returns(items.AsQueryable());
            mockKitRepository.Setup(m => m.GetAll()).Returns(kits.AsQueryable());
            mockLinkGroupRepository.Setup(m => m.GetAll()).Returns(linkGroups.AsQueryable());
            mockKitLinkGroupRepository.Setup(m => m.GetAll()).Returns(kitLinkGroups.AsQueryable());
            mockKitLinkGroupItemRepository.Setup(m => m.GetAll()).Returns(kitLinkGroups.SelectMany(i => i.KitLinkGroupItem).AsQueryable());
            mockLocaleRepository.Setup(m => m.GetAll()).Returns(localeList.AsQueryable());
            mockKitLocaleRepository.Setup(m => m.GetAll()).Returns(kitLocaleList.AsQueryable());
            mockLocaleTypeRepository.Setup(m => m.GetAll()).Returns(localeTypeList.AsQueryable());
            mockKitLinkGroupLocaleRepository.Setup(m => m.GetAll()).Returns(kitLinkGroupLocaleList.AsQueryable());
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

            mockKitHelper.Setup(s => s.SetOrderBy(It.IsAny<IQueryable<KitDtoWithStatus>>(), kitSearchParameters)).Returns(kitsBeforePaging.OrderBy(orderBy));

            //When
            var response = kitController.GetKits(kitSearchParameters);

            Assert.IsInstanceOfType(response, typeof(OkObjectResult), "Ok Request Expected");
        }

        [TestMethod]
        public void KitController_GetKitLocale_ValidKitId_Ok()
        {
            //Given
            var kitSearchParameters = 1;

            //When
            var response = kitController.GetKitLocale(kitSearchParameters);
            //Then
            Assert.IsInstanceOfType(response, typeof(OkObjectResult), "Ok Request Expected");
        }

        [TestMethod]
        public void KitsController_Save_InvalidKitId_ReturnsBadRequest()
        {
            var kitSaveParameters = new KitDto()
            {
                KitId = 99,
                Description = "Bad Kit",
                ItemId = 3,
                KitLinkGroup = new List<KitLinkGroupDto>(),
                KitInstructionList = new List<KitInstructionListDto>()

            };

            var response = kitController.KitSaveDetails(kitSaveParameters);

            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            Assert.IsTrue(((BadRequestObjectResult)response).Value.ToString().Contains("Unable to find Kit"), "Missing Kit Expected");

        }

        [TestMethod]
        public void KitsController_GetKitByLocaleIdKitLocaleRecordDoesNotExist_ReturnsNotFound()
        {
            int kitId = 1;
            int localeId = 8;

            var response = kitController.GetKitByLocaleId(kitId, localeId, false);

            Assert.IsInstanceOfType(response, typeof(NotFoundResult), "Not Found Expected");
        }

		[TestMethod]
		public void KitsController_GetKitViewOfCustomizableKitsKitLinkGroupLocaleDoesNotExist_ReturnsKitSetupError()
		{
			int kitId = 2;
			int localeId = 3;

			var response = kitController.GetKitView(kitId, localeId);

			Assert.IsInstanceOfType(response, typeof(OkObjectResult), "Ok Result Expected");
			Assert.IsTrue(((KitView)(((OkObjectResult)response).Value)).ErrorMessage.Equals("Error: Please make sure Kit is set up correctly for this store."), "Setup error shows");
		}

		[TestMethod]
        public void KitsController_GetKitByLocaleIdKitDoesNotExist_ReturnsNotFound()
        {
            int kitId = 999;
            int localeId = 5;

            var response = kitController.GetKitByLocaleId(kitId, localeId, false);

            Assert.IsInstanceOfType(response, typeof(NotFoundResult), "Not Found Expected");
        }

        [TestMethod]
        public void KitsController_GetKitPropertiesByLocaleIdKitLocaleRecordDoesNotExist_ReturnsNotFound()
        {
            int kitId = 1;
            int localeId = 99;

            var response = kitController.GetKitPropertiesByLocaleId(kitId, localeId);

            Assert.IsInstanceOfType(response, typeof(NotFoundResult), "Not Found Expected");
        }

        [TestMethod]
        public void KitsController_GetKitPropertiesByLocaleIdKitLocaleRecordExist_ReturnsOk()
        {
            int kitId = 1;
            int localeId = 1;

            var mockContext = new Mock<KitBuilderContext>();
            var mockDbSet = GetMockDbSet<Kit>(kits);
            var mockKitLocaleDbSet = GetMockDbSet<KitLocale>(kitLocaleList);
            mockContext.Setup(c => c.Set<Kit>()).Returns(mockDbSet.Object);

            mockKitRepository.SetupGet(s => s.UnitOfWork).Returns(mockUnitWork.Object);
            // mockUnitWork.SetupGet(s => s.Context).Returns(mockContext.Object);
            mockContext.Setup(m => m.Kit).Returns(mockDbSet.Object);
            mockContext.Setup(m => m.KitLocale).Returns(mockKitLocaleDbSet.Object);

            var response = kitController.GetKitPropertiesByLocaleId(kitId, localeId);

            Assert.IsInstanceOfType(response, typeof(OkObjectResult), "Ok Result Expected");
        }

        [TestMethod]
        public void KitsController_AssignUnassignLocationsNullListPassed_ReturnsBadRequest()
        {
            var response = kitController.AssignUnassignLocations(null, 0);

            Assert.IsInstanceOfType(response, typeof(BadRequestResult), "Bad Request Expected");
        }

        [TestMethod]
        public void KitsController_AssignUnassignLocationsValidListPassed_ReturnsOk()
        {
            var mockContext = new Mock<KitBuilderContext>();
            var mockDbSet = GetMockDbSet<KitLocale>(kitLocaleList);
            mockContext.Setup(c => c.Set<KitLocale>()).Returns(mockDbSet.Object);
            mockKitLocaleRepository.SetupGet(s => s.UnitOfWork).Returns(mockUnitWork.Object);
            mockUnitWork.SetupGet(s => s.Context).Returns(mockContext.Object);
            mockContext.Setup(m => m.KitLocale).Returns(mockDbSet.Object);

            var response = kitController.AssignUnassignLocations(assignKitToLocaleDtoList, kits[0].KitId);

            Assert.IsInstanceOfType(response, typeof(OkResult), "Ok Result Expected");
        }

        [TestMethod]
        public void KitsController_SaveKitPropertiesNullListPassed_ReturnsBadRequest()
        {
            var response = kitController.SaveKitProperties(null);

            Assert.IsInstanceOfType(response, typeof(BadRequestResult), "Bad Request Expected");
        }

        [TestMethod]
        public void KitsController_Save_InvalidItemId_ReturnsBadRequest()
        {
            var kitSaveParameters = new KitDto()
            {
                KitId = 1,
                Description = "Bad Kit",
                ItemId = 99,
                KitLinkGroup = new List<KitLinkGroupDto>(),
                KitInstructionList = new List<KitInstructionListDto>()

            };

            var response = kitController.KitSaveDetails(kitSaveParameters);

            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            Assert.IsTrue(((BadRequestObjectResult)response).Value.ToString().Contains("Unable to find Item"), "Missing Item Expected");

        }

        //[TestMethod]
        //public void KitsController_Save_InvalidLinkGroupItemIds_ReturnsBadRequest()
        //{
        //    var kitSaveParameters = new KitDto()
        //    {
        //        KitId = 1,
        //        Description = "Bad Kit",
        //        ItemId = 99,
        //        KitLinkGroup = new List<KitLinkGroupDto>(),
        //        KitInstructionList = new List<KitInstructionListDto>()

        //    };

        //    var response = kitController.KitSaveDetails(kitSaveParameters);

        //    Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
        //    Assert.IsTrue(((BadRequestObjectResult)response).Value.ToString().Contains("LinkGroupItemIds"), "LinkGroupItemIds parameter validation");

        //}

        [TestMethod]
        public void KitsController_Save_InvalidLinkGroupIds_ReturnsBadRequest()
        {
            var kitSaveParameters = new KitDto()
            {
                KitId = 1,
                Description = "Bad Kit",
                ItemId = 3,
                KitLinkGroup = null,
                KitInstructionList = new List<KitInstructionListDto>()

            };

            var response = kitController.KitSaveDetails(kitSaveParameters);

            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            Assert.IsTrue(((BadRequestObjectResult)response).Value.ToString().Contains("LinkGroups"), "LinkGroupIds parameter validation");

        }
        [TestMethod]
        public void KitsController_Save_InvalidInstructionListIds_ReturnsBadRequest()
        {
            var kitSaveParameters = new KitDto()
            {
                KitId = 1,
                Description = "Bad Kit",
                ItemId = 99,
                KitLinkGroup = new List<KitLinkGroupDto>(),
                KitInstructionList = null

            };

            var response = kitController.KitSaveDetails(kitSaveParameters);

            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            Assert.IsTrue(((BadRequestObjectResult)response).Value.ToString().Contains("InstructionListIds"), "InstructionListIds parameter validation");

        }

        private Mock<DbSet<T>> GetMockDbSet<T>(List<T> objectPassed) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(objectPassed.AsQueryable().Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(objectPassed.AsQueryable().Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(objectPassed.AsQueryable().ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(objectPassed.AsQueryable().GetEnumerator());

            return mockSet;

        }
    }
}