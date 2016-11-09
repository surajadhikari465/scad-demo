using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Framework;
using RegionalEventController.DataAccess.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Icon.Testing.Builders;
using Icon.Testing.CustomModels;

namespace RegionalEventController.Tests.DataAccess.QueryTests
{
    [TestClass]
    public class GetBrandNewScanCodesQueryHandlerTests
    {
        private IconContext context;
        private GetBrandNewScanCodesQuery query;
        private GetBrandNewScanCodesQueryHandler handler;
        private List<string> scanCodes;
        private List<string> scanCodesInIcon;
        private List<string> scanCodesInIrmaItem;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();
            this.query = new GetBrandNewScanCodesQuery();
            this.handler = new GetBrandNewScanCodesQueryHandler(this.context);
            this.scanCodes = new List<string>();
            this.scanCodesInIcon = new List<string>();
            this.scanCodesInIrmaItem = new List<string>();
        }

        [TestCleanup]
        public void CleanupData()
        {
            IEnumerable<IRMAItem> irmaItemsToRemove = this.context.IRMAItem.Where(ii => ii.identifier.Contains("888999777888"));
            if (irmaItemsToRemove.Count() > 0)
            {
                this.context.IRMAItem.RemoveRange(irmaItemsToRemove);
                this.context.SaveChanges();
            }            

            if (this.context != null)
            {
                this.context.Dispose();
            }
        }

        [TestMethod]
        public void GetBrandNewScanCodes_NoScanCodesFoundInScanCodeOrIrmaItemTables_ReturnsAllScanCodes()
        {
            // Given
            BuildIdentifierQueryList();
            this.query.scanCodes = this.scanCodes;

            List<string> scanCodesNotInIcon = this.scanCodes.Except(this.scanCodesInIcon).ToList();
            List<string> scanCodesNotInIrmaItem = this.scanCodes.Except(this.scanCodesInIrmaItem).ToList();
            List<string> expected = scanCodesNotInIcon.Union(scanCodesNotInIrmaItem).ToList();

            // When
            List<string> actual = this.handler.Execute(this.query);

            // Then
            Assert.AreEqual(expected.Count, actual.Count, "The returned list of scanCodes count did not match the expected scanCode list count.");
            expected.Sort();
            actual.Sort();
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i], "The scanCodes did not match.");
            }
        }

        [TestMethod]
        public void GetBrandNewScanCodes_SomeScanCodesFoundInScanCodeTable_ReturnsOnlyScanCodesNotFoundInScanCodeTable()
        {
            // Given
            BuildIdentifierQueryList();
            List<ScanCode> upcs = GetExistingScanCodes(1);
            string upcInIcon = upcs.First().scanCode;

            this.scanCodes.Add(upcInIcon);
            this.scanCodesInIcon.Add(upcInIcon);
            this.query.scanCodes = this.scanCodes;

            List<string> scanCodesNotInIrmaItem = this.scanCodes.Except(this.scanCodesInIrmaItem).ToList();
            List<string> expected = scanCodesNotInIrmaItem.Except(this.scanCodesInIcon).ToList();

            // When
            List<string> actual = this.handler.Execute(this.query);

            // Then
            Assert.AreEqual(expected.Count, actual.Count, "The returned list of scanCodes count did not match the expected scanCode list count.");
            expected.Sort();
            actual.Sort();
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i], "The actual scanCode did not match the expected scanCode.");
            }
        }

        [TestMethod]
        public void GetBrandNewScaCodes_SomeScanCodesFoundInIrmaItemTable_ReturnsOnlyScanCodesNotFoundInIrmaItemTable()
        {
            // Given
            BuildIdentifierQueryList();
            string upcInIrmaItem = null;

            List<IRMAItem> irmaItems = GetExistingIrmaItems(1);
            upcInIrmaItem = irmaItems.First().identifier;

            this.scanCodes.Add(upcInIrmaItem);
            this.scanCodesInIrmaItem.Add(upcInIrmaItem);
            this.query.scanCodes = this.scanCodes;

            List<string> scanCodesNotInIrmaItem = this.scanCodes.Except(this.scanCodesInIrmaItem).ToList();
            List<string> expected = scanCodesNotInIrmaItem.Except(this.scanCodesInIcon).ToList();

            // When
            List<string> actual = this.handler.Execute(this.query);

            // Then
            Assert.AreEqual(expected.Count, actual.Count, "The returned list of scanCodes count did not match the expected scanCode list count.");
            expected.Sort();
            actual.Sort();
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i], "The actual scanCode did not match the expected scanCode.");
            }
        }

        [TestMethod]
        public void GetBrandNewScanCodes_SomeScanCodesFoundInIrmaItemAndScanCodeTable_ReturnsOnlyScanCodesNotFoundInEitherIrmaItemOrScanCodeTables()
        {
            // Given
            BuildIdentifierQueryList();

            // setup upc that exists in IRMAItem
            string upcInIrmaItem = null;
            List<IRMAItem> irmaItems = GetExistingIrmaItems(1);
            upcInIrmaItem = irmaItems.First().identifier;
            this.scanCodes.Add(upcInIrmaItem);
            this.scanCodesInIrmaItem.Add(upcInIrmaItem);

            // setup upc that exists in ScaCode
            string upcInIcon = null;
            List<ScanCode> upcs = GetExistingScanCodes(1);
            upcInIcon = upcs.First().scanCode;
            this.scanCodes.Add(upcInIcon);
            this.scanCodesInIcon.Add(upcInIcon);

            this.query.scanCodes = this.scanCodes;
            List<string> scanCodesNotInIrmaItem = this.scanCodes.Except(this.scanCodesInIrmaItem).ToList();
            List<string> expected = scanCodesNotInIrmaItem.Except(this.scanCodesInIcon).ToList();

            // When
            List<string> actual = this.handler.Execute(this.query);

            // Then
            Assert.AreEqual(expected.Count, actual.Count, "The returned list of scanCodes count did not match the expected scanCode list count.");
            expected.Sort();
            actual.Sort();
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i], "The actual scanCode did not match the expected scanCode.");
            }
        }

        private void BuildIdentifierQueryList()
        {
            string upc;
            for (int i = 0; i < 3; i++)
            {
                upc = "567856785678" + i.ToString();
                if (this.context.ScanCode.Any(sc => sc.scanCode == upc))
                {
                    this.scanCodesInIcon.Add(upc);
                }
                else if (this.context.IRMAItem.Any(ii => ii.identifier == upc))
                {
                    this.scanCodesInIrmaItem.Add(upc);
                }

                this.scanCodes.Add(upc);
            }
        }

        private List<ScanCode> GetExistingScanCodes(int maxRows)
        {
            List<ScanCode> scanCodes = new List<ScanCode>();
            scanCodes = this.context.ScanCode.Take(maxRows).ToList();
            return scanCodes;
        }

        private List<IRMAItem> GetExistingIrmaItems(int maxRows)
        {
            List<IRMAItem> irmaItems = this.context.IRMAItem.Take(maxRows).ToList();
            if (irmaItems.Count == 0)
            {
                for (int i = 0; i < maxRows; i++)
                {
                    IRMAItem item = new IRMAItem
                    {
                        brandName = "test brand",
                        defaultIdentifier = true,
                        departmentSale = false,
                        foodStamp = true,
                        giftCard = false,
                        identifier = "888999777888" + i.ToString(),
                        insertDate = DateTime.Now,
                        itemDescription = "test",
                        merchandiseClassID = this.context.HierarchyClass.First(hc => hc.hierarchyLevel == 5).hierarchyClassID,
                        packageUnit = 1,
                        posDescription = "test",
                        posScaleTare = 0,
                        regioncode = "BS",
                        retailSize = 1,
                        retailUom = "EACH",
                        taxClassID = this.context.HierarchyClass.First(hc => hc.hierarchyID == Hierarchies.Tax).hierarchyClassID,
                        nationalClassID = this.context.HierarchyClass.First(hc => hc.hierarchyID == Hierarchies.National).hierarchyClassID
                    };
                    this.context.IRMAItem.Add(item);
                    irmaItems.Add(item);
                }

                this.context.SaveChanges();
            }

            return irmaItems;
        }
    }
}
