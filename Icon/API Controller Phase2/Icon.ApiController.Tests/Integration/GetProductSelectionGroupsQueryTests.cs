﻿using Icon.ApiController.DataAccess.Queries;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.ApiController.Tests.Integration
{
    [TestClass]
    public class GetProductSelectionGroupsQueryTests
    {
        private GetProductSelectionGroupsQuery query;
        private IconContext context;
        private GlobalIconContext globalContext;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);
            query = new GetProductSelectionGroupsQuery(globalContext);

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void Search_PrductSelectionGroupsExist_ShouldReturnProductSelectionGroups()
        {
            //Given
            var consumableType = context.ProductSelectionGroupType.First(psgType => psgType.ProductSelectionGroupTypeName == "Consumable");
            List<ProductSelectionGroup> psgs = new List<ProductSelectionGroup>
            {
                new TestProductSelectionGroupBuilder()
                    .WithProductSelectionGroupName("Test Name 1")
                    .WithProductSelectionGroupType(consumableType),
                new TestProductSelectionGroupBuilder()
                    .WithProductSelectionGroupName("Test Name 2")
                    .WithProductSelectionGroupType(consumableType)
            };
            context.ProductSelectionGroup.AddRange(psgs);
            context.SaveChanges();

            //When
            var result = query.Search(new GetProductSelectionGroupsParameters());

            //Then
            //Only asserting that the psgs that we entered exist because we cannot be sure what the state of the DEV database is like
            int psgCount = result.Where(psg => psg.ProductSelectionGroupName == "Test Name 1" || psg.ProductSelectionGroupName == "Test Name 2").Count();
            Assert.AreEqual(2, psgCount);
        }
    }
}
