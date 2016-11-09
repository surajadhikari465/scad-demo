using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetAvailablePlusByCategoryQueryTests
    {
        private IconContext context;
        
        private DbContextTransaction transaction;
        private GetAvailablePlusByCategoryQuery query;      
        private List<ScanCode> scanCodes;
        private PLUCategory posPlucategory;
        private PLUCategory scalePlucategory;

        [TestInitialize]
        public void Initialize()
        {
            List<Item>  items = new List<Item>();
            Item item1 = new Item { itemTypeID = 1 };
            Item item2 = new Item { itemTypeID = 1 };
            Item item3 = new Item { itemTypeID = 1 };
            Item item4 = new Item { itemTypeID = 1 };
            Item item5 = new Item { itemTypeID = 1 };

            context = new IconContext();

            context.Database.Connection.Open();
            transaction = context.Database.BeginTransaction();

            context.Item.Add(item1);
            context.Item.Add(item2);
            context.Item.Add(item3);
            context.Item.Add(item4);
            context.Item.Add(item5);
            context.SaveChanges();            

            scanCodes = new List<ScanCode>
            {
                new ScanCode { itemID = item1.itemID, scanCode = "21111100000", scanCodeTypeID = 3, localeID = 1 },
                new ScanCode { itemID = item2.itemID, scanCode = "21119800000", scanCodeTypeID = 3, localeID = 1 },
                new ScanCode { itemID = item3.itemID, scanCode = "21111200000", scanCodeTypeID = 3, localeID = 1 },
                new ScanCode { itemID = item4.itemID, scanCode = "1111", scanCodeTypeID = 2, localeID = 1 },
                new ScanCode { itemID = item5.itemID, scanCode = "1999", scanCodeTypeID = 2, localeID = 1 }
            };

            var testScanCodes = scanCodes.Select(sc => sc.scanCode).ToList();
            context.ScanCode.RemoveRange(context.ScanCode.Where(sc => testScanCodes.Contains(sc.scanCode)));
            context.SaveChanges();

            context.ScanCode.AddRange(scanCodes);
            int count = context.SaveChanges();
            posPlucategory = new PLUCategory() { PluCategoryName = "TestPosCat1", BeginRange = "100", EndRange = "200" };
            scalePlucategory = new PLUCategory() { PluCategoryName = "TestScalePluCat2", BeginRange = "21111100000", EndRange = "21119900000" };
          
            context.PLUCategory.RemoveRange(context.PLUCategory.Where(pc => pc.PluCategoryName.Equals(posPlucategory.PluCategoryName)));
            context.PLUCategory.RemoveRange(context.PLUCategory.Where(pc => pc.PluCategoryName.Equals(scalePlucategory.PluCategoryName)));            

            context.PLUCategory.Add(posPlucategory);
            context.PLUCategory.Add(scalePlucategory);
            context.SaveChanges();

            query = new GetAvailablePlusByCategoryQuery(this.context);
        }

        [TestCleanup]
        public void DeleteTestData()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction.Dispose();
                transaction = null;
            }

            context.Dispose();
            query = null;
        }

        [TestMethod]
        public void GetAvailablePlusByCategoryQuery_Search_PosPluCategory_ReturnsAvailabePosPlusForCategory()
        {
            // Given
            GetAvailablePlusByCategoryParameters parameters = new GetAvailablePlusByCategoryParameters
            {
                PluCategoryId = posPlucategory.PluCategoryID
            };

            // When
            var items = query.Search(parameters);
            List<string> existingPosPlus = scanCodes.Where(sc => sc.scanCodeTypeID == ScanCodeTypes.PosPlu).Select(sc => sc.scanCode).ToList();
            
            // Then
            Assert.IsFalse(items.Any(ii => existingPosPlus.Contains(ii.identifier)));
        }

        [TestMethod]
        public void GetAvailablePlusByCategoryQuery_Search_ScalePluCategory_ReturnsAvailabeScalePlusForCategory()
        {
            // Given
            GetAvailablePlusByCategoryParameters parameters = new GetAvailablePlusByCategoryParameters
            {
                PluCategoryId = scalePlucategory.PluCategoryID
            };

            // When
            var items = query.Search(parameters);
            List<string> existingScalePlus = scanCodes.Where(sc => sc.scanCodeTypeID == ScanCodeTypes.ScalePlu).Select(sc =>sc.scanCode).ToList();
            
            // Then
            Assert.IsFalse(items.Any(ii => existingScalePlus.Contains(ii.identifier)));
        }

        [TestMethod]
        public void GetAvailablePlusByCategoryQuery_SearchForMaxOnePlu_ReturnsOnlyOneAvailabeScalePlusForCategory()
        {
            // Given
            GetAvailablePlusByCategoryParameters parameters = new GetAvailablePlusByCategoryParameters
            {
                PluCategoryId = scalePlucategory.PluCategoryID,
                MaxPlus = 1
            };

            // When
            var items = query.Search(parameters);
            List<string> existingScalePlus = scanCodes.Where(sc => sc.scanCodeTypeID == ScanCodeTypes.ScalePlu).Select(sc => sc.scanCode).ToList();

            // Then
            Assert.IsFalse(items.Any(ii => existingScalePlus.Contains(ii.identifier)));
            Assert.AreEqual(items.Count, 1);
        }
    }
}

