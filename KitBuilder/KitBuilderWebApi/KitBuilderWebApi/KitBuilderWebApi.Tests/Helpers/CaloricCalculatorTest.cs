using AutoMapper;
using KitBuilderWebApi.Controllers;
using KitBuilderWebApi.Helper;
using KitBuilderWebApi.Services;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Dto;
using KitBuilder.DataAccess.Repository;
using KitBuilder.DataAccess.Queries;

namespace KitBuilderWebApi.Tests.Services
{
	[TestClass]
	public class CaloricCalculatorTest
	{
		private CaloricCalculator caloricCalculator;
		private IQueryHandler<GetKitByKitLocaleIdParameters, KitLocale> getKitLocaleQuery;
		private Mock<IQueryHandler<GetKitByKitLocaleIdParameters, KitLocale>> mockGetKitLocaleQuery;
		private Mock<IRepository<Locale>> mockLocaleRepository;

		[TestInitialize]
		public void InitializeTest()
		{
			mockGetKitLocaleQuery = new Mock<IQueryHandler<GetKitByKitLocaleIdParameters, KitLocale>>();
			mockLocaleRepository = new Mock<IRepository<Locale>>();
			GetKitLocaleByStoreParameters parameters = new GetKitLocaleByStoreParameters();

			caloricCalculator = new CaloricCalculator(parameters,
				mockGetKitLocaleQuery.Object,
				mockLocaleRepository.Object);
		}

		private void SetUpDataAndRepository()
		{
			MappingHelper.InitializeMapper();

			//linkGroupsDto = new List<LinkGroupDto>();

			//IList<Items> items = new List<Items>
			//{
			//	new Items{ItemId=1, ScanCode="4001", ProductDesc="Baguette", CustomerFriendlyDesc = "Baguette", KitchenDesc="Baguette" },
			//	new Items{ItemId=2, ScanCode="4002", ProductDesc="Ciabatta Roll", CustomerFriendlyDesc = "Ciabatta Roll", KitchenDesc="Ciabatta Roll" },
			//	new Items{ItemId=3, ScanCode="4003", ProductDesc="Flour Tortilla", CustomerFriendlyDesc = "Flour Tortilla", KitchenDesc="Flour Tortilla" },
			//	new Items{ItemId=4, ScanCode="4004", ProductDesc="Basil", CustomerFriendlyDesc = "Basil", KitchenDesc="Basil" },
			//	new Items{ItemId=5, ScanCode="4005", ProductDesc="Carrots", CustomerFriendlyDesc = "Carrots", KitchenDesc="Carrots" },
			//	new Items{ItemId=6, ScanCode="4006", ProductDesc="Lettuce", CustomerFriendlyDesc = "Lettuce", KitchenDesc="Lettuce" },
			//};

			//linkGroups = new List<LinkGroup>
		 //  {
			//   new LinkGroup{ LinkGroupId=1, GroupName = "Taco", GroupDescription = "Cheese taco"},
			//   new LinkGroup{ LinkGroupId=2, GroupName = "Topping", GroupDescription = "Topping"},
			//   new LinkGroup{ LinkGroupId=3, GroupName = "Add Cheese", GroupDescription = "Add Cheese"},
		 //  };

			//linkGroupItems = new List<LinkGroupItem>
			//{
			//	new LinkGroupItem{ LinkGroupItemId= 1, LinkGroupId= 1, ItemId= 3 },
			//	new LinkGroupItem{ LinkGroupItemId= 2, LinkGroupId= 1, ItemId= 1 },
			//	new LinkGroupItem{ LinkGroupItemId= 3, LinkGroupId= 2, ItemId= 6 },
			//	new LinkGroupItem{ LinkGroupItemId= 4, LinkGroupId= 1, ItemId= 2 },
			//	new LinkGroupItem{ LinkGroupItemId= 5, LinkGroupId= 2, ItemId= 5 },
			//};

			//int count = 1;
			//foreach (LinkGroup linkGroup in linkGroups)
			//{
			//	foreach (Items item in items)
			//	{
			//		linkGroup.LinkGroupItem.Add(new LinkGroupItem { LinkGroupItemId = count, LinkGroupId = linkGroup.LinkGroupId, ItemId = item.ItemId });
			//		count = count + 1;
			//	}
			//}

			//linkGroupsDto = (from l in linkGroups
			//				 select new LinkGroupDto()
			//				 {
			//					 LinkGroupId = l.LinkGroupId,
			//					 GroupName = l.GroupName,
			//					 GroupDescription = l.GroupDescription,
			//					 InsertDateUtc = l.InsertDateUtc
			//				 }).ToList();

			//mockInstructionListRepository.Setup(m => m.GetAll()).Returns(instructionLists.AsQueryable());
			//mockInstructionTypeRespository.Setup(m => m.GetAll()).Returns(instructionTypes.AsQueryable());
			//mockStatusRespository.Setup(m => m.GetAll()).Returns(statuses.AsQueryable());


		}

		[TestCleanup]
		public void Cleanup()
		{
			Mapper.Reset();
		}
	}
}
