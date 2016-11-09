using Icon.Framework;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetAvailableScanCodesByCategoryQueryTests
    {
        private GetAvailableScanCodesByCategoryQuery queryHandler;
        private GetAvailableScanCodesByCategoryParameters parameters;
        private IconContext context;
        private DbContextTransaction transaction;
        private PLUCategory category;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = context.Database.BeginTransaction();

            queryHandler = new GetAvailableScanCodesByCategoryQuery(context);
            parameters = new GetAvailableScanCodesByCategoryParameters();

            category = new PLUCategory
            {
                BeginRange = "55000000000",
                EndRange = "56000000000",
                PluCategoryName = "Test PLU Category"
            };
            context.PLUCategory.Add(category);
            context.SaveChanges();

            parameters.CategoryId = category.PluCategoryID;
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void GetAvailableScanCodesByCategory_NoScanCodesExistInRange_ShouldReturnFullRange()
        {
            //Given
            parameters.MaxScanCodes = 100;

            //When
            var results = queryHandler.Search(parameters);

            //Then
            List<string> expectedScanCodeSequence = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                expectedScanCodeSequence.Add((55000000000 + i).ToString());
            }
            var actualScanCodeSequence = results.Select(r => r.identifier).OrderBy(s => s);

            Assert.AreEqual(100, results.Count);
            Assert.IsTrue(expectedScanCodeSequence.SequenceEqual(actualScanCodeSequence));
        }

        [TestMethod]
        public void GetAvailableScanCodesByCategory_ScanCodesExistInRange_ShouldNotAddExistingScanCodesToTheRange()
        {
            //Given
            Item item = new Item
            {
                itemTypeID = ItemTypes.RetailSale,
                ScanCode = new List<ScanCode> { new ScanCode { scanCode = "55000000001", scanCodeTypeID = ScanCodeTypes.Upc }}
            };
            context.Item.Add(item);
            context.SaveChanges();

            parameters.MaxScanCodes = 100;

            //When
            var results = queryHandler.Search(parameters);

            //Then
            List<string> expectedScanCodeSequence = new List<string>();
            expectedScanCodeSequence.Add("55000000000");
            for (int i = 0; i < 99; i++)
            {
                expectedScanCodeSequence.Add((55000000002 + i).ToString());
            }
            var actualScanCodeSequence = results.Select(r => r.identifier).OrderBy(s => s);

            Assert.AreEqual(100, results.Count);
            Assert.IsTrue(expectedScanCodeSequence.SequenceEqual(actualScanCodeSequence));
        }

        [TestMethod]
        public void GetAvailableScanCodesByCategory_BeginRangeExists_ShouldNotAddBeginRangeScanCode()
        {
            //Given
            Item item = new Item
            {
                itemTypeID = ItemTypes.RetailSale,
                ScanCode = new List<ScanCode> { new ScanCode { scanCode = "55000000000", scanCodeTypeID = ScanCodeTypes.Upc } }
            };
            context.Item.Add(item);
            context.SaveChanges();

            parameters.MaxScanCodes = 100;

            //When
            var results = queryHandler.Search(parameters);

            //Then
            List<string> expectedScanCodeSequence = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                expectedScanCodeSequence.Add((55000000001 + i).ToString());
            }
            var actualScanCodeSequence = results.Select(r => r.identifier).OrderBy(s => s);

            Assert.AreEqual(100, results.Count);
            Assert.IsTrue(expectedScanCodeSequence.SequenceEqual(actualScanCodeSequence));
        }

        [TestMethod]
        public void GetAvailableScanCodesByCategory_EndRangeExists_ShouldNotAddEndRangeScanCode()
        {
            //Given
            category.EndRange = "55000000005";
            context.SaveChanges();

            Item item = new Item
            {
                itemTypeID = ItemTypes.RetailSale,
                ScanCode = new List<ScanCode> { new ScanCode { scanCode = "55000000005", scanCodeTypeID = ScanCodeTypes.Upc } }
            };
            context.Item.Add(item);
            context.SaveChanges();

            parameters.MaxScanCodes = 10;

            //When
            var results = queryHandler.Search(parameters);

            //Then
            List<string> expectedScanCodeSequence = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                expectedScanCodeSequence.Add((55000000000 + i).ToString());
            }
            var actualScanCodeSequence = results.Select(r => r.identifier).OrderBy(s => s);

            Assert.AreEqual(5, results.Count);
            Assert.IsTrue(expectedScanCodeSequence.SequenceEqual(actualScanCodeSequence));
        }

        [TestMethod]
        public void GetAvailableScanCodesByCategory_MaxScanCodesIsGreaterThanRange_ShouldReturnFullRange()
        {
            //Given
            category.EndRange = "55000000005";
            context.SaveChanges();
            
            parameters.MaxScanCodes = 10;

            //When
            var results = queryHandler.Search(parameters);

            //Then
            List<string> expectedScanCodeSequence = new List<string>();
            for (int i = 0; i < 6; i++)
            {
                expectedScanCodeSequence.Add((55000000000 + i).ToString());
            }
            var actualScanCodeSequence = results.Select(r => r.identifier).OrderBy(s => s);

            Assert.AreEqual(6, results.Count);
            Assert.IsTrue(expectedScanCodeSequence.SequenceEqual(actualScanCodeSequence));
        }

        [TestMethod]
        public void GetAvailableScanCodesByCategory_MaxScanCodesIsZero_ShouldReturnDefaultMaxNumberOfResultsWhichIs10000()
        {
            //Given
            parameters.MaxScanCodes = 0;

            //When
            var results = queryHandler.Search(parameters);

            //Then
            List<string> expectedScanCodeSequence = new List<string>();
            for (int i = 0; i < 10000; i++)
            {
                expectedScanCodeSequence.Add((55000000000 + i).ToString());
            }
            var actualScanCodeSequence = results.Select(r => r.identifier).OrderBy(s => s);

            Assert.AreEqual(10000, results.Count);
            Assert.IsTrue(expectedScanCodeSequence.SequenceEqual(actualScanCodeSequence));
        }

        [TestMethod]
        public void GetAvailableScanCodesByCategory_MaxScanCodesIs10_ShouldReturn10Results()
        {
            //Given
            parameters.MaxScanCodes = 10;

            //When
            var results = queryHandler.Search(parameters);

            //Then
            List<string> expectedScanCodeSequence = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                expectedScanCodeSequence.Add((55000000000 + i).ToString());
            }
            var actualScanCodeSequence = results.Select(r => r.identifier).OrderBy(s => s);

            Assert.AreEqual(10, results.Count);
            Assert.IsTrue(expectedScanCodeSequence.SequenceEqual(actualScanCodeSequence));
        }
    }
}
