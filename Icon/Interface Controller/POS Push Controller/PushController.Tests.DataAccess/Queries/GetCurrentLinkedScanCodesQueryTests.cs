using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace PushController.Tests.DataAccess.Queries
{
    [TestClass]
    public class GetCurrentLinkedScanCodesQueryTests
    {
        private GetCurrentLinkedScanCodesQueryHandler queryHandler;
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private Mock<ILogger<GetCurrentLinkedScanCodesQueryHandler>> mockLogger;
        private List<string> testScanCodes;
        private List<string> testLinkedScanCodes;
        private List<int> testBusinessUnits;
        private List<Item> testItems;
        private List<Item> testLinkedItems;
        private List<Locale> testLocales;

        [TestInitialize]
        public void Initialize()
        {
            context = new GlobalIconContext(new IconContext());
            mockLogger = new Mock<ILogger<GetCurrentLinkedScanCodesQueryHandler>>();
            queryHandler = new GetCurrentLinkedScanCodesQueryHandler(mockLogger.Object, context);

            testScanCodes = new List<string>
            {
                "22222222227",
                "22222222228"
            };

            testLinkedScanCodes = new List<string>
            {
                "333333333332",
                "333333333331"
            };

            testBusinessUnits = new List<int>
            {
                88888,
                88887
            };

            testItems = new List<Item>();
            testLinkedItems = new List<Item>();
            testLocales = new List<Locale>();

            transaction = context.Context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void StageTestScanCodes(List<string> testScanCodes)
        {
            foreach (var scanCode in testScanCodes)
            {
                Item item = new TestItemBuilder().WithScanCode(scanCode);
                testItems.Add(item);
                context.Context.Item.Add(item);
                context.Context.SaveChanges();
            }
        }

        private void StageTestBusinessUnits(List<int> testBusinessUnits)
        {
            foreach (var businessUnit in testBusinessUnits)
            {
                Locale locale = new TestLocaleBuilder().WithBusinessUnitId(businessUnit).WithLocaleName("TestLocale" + businessUnit.ToString());
                testLocales.Add(locale);
                context.Context.Locale.Add(locale);
                context.Context.SaveChanges();
            }
        }

        private void StageTestLinkedScanCodes(List<string> testLinkedScanCodes)
        {
            foreach (var scanCode in testLinkedScanCodes)
            {
                Item item = new TestItemBuilder().WithScanCode(scanCode);
                testLinkedItems.Add(item);
                context.Context.Item.Add(item);
                context.Context.SaveChanges();
            }
        }

        [TestMethod]
        public void GetCurrentLinkedScanCodesQuery_InputParameterIsEmptyOrNull_NoResultsShouldBeReturned()
        {
            // Given.
            var emptyParameters = new GetCurrentLinkedScanCodesQuery { ScanCodesByBusinessUnit = new List<Tuple<string, int>>() };
            var nullParameters = new GetCurrentLinkedScanCodesQuery { ScanCodesByBusinessUnit = null };

            // When.
            var emptyResults = queryHandler.Execute(emptyParameters);
            var nullResults = queryHandler.Execute(nullParameters);

            // Then.
            Assert.AreEqual(0, emptyResults.Count);
            Assert.AreEqual(0, nullResults.Count);
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void GetCurrentLinkedScanCodesQuery_ScanCodeHasNoLinkedScanCodes_NoResultsShouldBeReturned()
        {
            // Given.
            StageTestScanCodes(new List<string> { testScanCodes[0] });
            StageTestBusinessUnits(new List<int> { testBusinessUnits[0] });

            var parameters = new GetCurrentLinkedScanCodesQuery { ScanCodesByBusinessUnit = new List<Tuple<string, int>> { new Tuple<string, int>(testScanCodes[0], testBusinessUnits[0]) } };

            // When.
            var results = queryHandler.Execute(parameters);

            // Then.
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetCurrentLinkedScanCodesQuery_ScanCodeHasLinkedScanCodeForOneBusinessUnit_OneLinkedScanCodeShouldBeReturned()
        {
            // Given.
            StageTestScanCodes(new List<string> { testScanCodes[0] });
            StageTestBusinessUnits(new List<int> { testBusinessUnits[0] });
            StageTestLinkedScanCodes(new List<string> { testLinkedScanCodes[0] });

            var itemLink = new ItemLink
            {
                parentItemID = testLinkedItems[0].itemID,
                childItemID = testItems[0].itemID,
                localeID = testLocales[0].localeID
            };
            context.Context.ItemLink.Add(itemLink);
            context.Context.SaveChanges();

            var parameters = new GetCurrentLinkedScanCodesQuery { ScanCodesByBusinessUnit = new List<Tuple<string, int>> { new Tuple<string, int>(testScanCodes[0], testBusinessUnits[0]) } };

            // When.
            var results = queryHandler.Execute(parameters);

            // Then.
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(testLinkedScanCodes[0], results[0].LinkedScanCode);
            Assert.AreEqual(testScanCodes[0], results[0].ScanCode);
            Assert.AreEqual(testBusinessUnits[0], results[0].BusinessUnitId);
        }

        [TestMethod]
        public void GetCurrentLinkedScanCodesQuery_ScanCodeHasLinkedScanCodeForTwoBusinessUnits_TwoLinkedScanCodesShouldBeReturned()
        {
            // Given.
            StageTestScanCodes(new List<string> { testScanCodes[0] });
            StageTestBusinessUnits(testBusinessUnits);
            StageTestLinkedScanCodes(new List<string> { testLinkedScanCodes[0] });

            var itemLinks = new List<ItemLink>
            {
                new ItemLink
                {
                    parentItemID = testLinkedItems[0].itemID,
                    childItemID = testItems[0].itemID,
                    localeID = testLocales[0].localeID
                },
                new ItemLink
                {
                    parentItemID = testLinkedItems[0].itemID,
                    childItemID = testItems[0].itemID,
                    localeID = testLocales[1].localeID
                }
            };
            context.Context.ItemLink.AddRange(itemLinks);
            context.Context.SaveChanges();

            var parameters = new GetCurrentLinkedScanCodesQuery
            {
                ScanCodesByBusinessUnit = new List<Tuple<string, int>>
                {
                    new Tuple<string, int>(testScanCodes[0], testBusinessUnits[0]),
                    new Tuple<string, int>(testScanCodes[0], testBusinessUnits[1])
                }
            };

            // When.
            var results = queryHandler.Execute(parameters);

            // Then.
            Assert.AreEqual(itemLinks.Count, results.Count);
            Assert.AreEqual(testLinkedScanCodes[0], results[0].LinkedScanCode);
            Assert.AreEqual(testScanCodes[0], results[0].ScanCode);
            Assert.AreEqual(testBusinessUnits[0], results[0].BusinessUnitId);
            Assert.AreEqual(testLinkedScanCodes[0], results[1].LinkedScanCode);
            Assert.AreEqual(testScanCodes[0], results[1].ScanCode);
            Assert.AreEqual(testBusinessUnits[1], results[1].BusinessUnitId);
        }

        [TestMethod]
        public void GetCurrentLinkedScanCodesQuery_ScanCodeHasTwoDifferentLinkedScanCodesForTwoDifferentBusinessUnits_TwoLinkedScanCodesShouldBeReturned()
        {
            // Given.
            StageTestScanCodes(new List<string> { testScanCodes[0] });
            StageTestBusinessUnits(testBusinessUnits);
            StageTestLinkedScanCodes(testLinkedScanCodes);

            var itemLinks = new List<ItemLink>
            {
                new ItemLink
                {
                    parentItemID = testLinkedItems[0].itemID,
                    childItemID = testItems[0].itemID,
                    localeID = testLocales[0].localeID
                },
                new ItemLink
                {
                    parentItemID = testLinkedItems[1].itemID,
                    childItemID = testItems[0].itemID,
                    localeID = testLocales[1].localeID
                }
            };
            context.Context.ItemLink.AddRange(itemLinks);
            context.Context.SaveChanges();

            var parameters = new GetCurrentLinkedScanCodesQuery
            {
                ScanCodesByBusinessUnit = new List<Tuple<string, int>>
                {
                    new Tuple<string, int>(testScanCodes[0], testBusinessUnits[0]),
                    new Tuple<string, int>(testScanCodes[0], testBusinessUnits[1])
                }
            };

            // When.
            var results = queryHandler.Execute(parameters);

            // Then.
            Assert.AreEqual(itemLinks.Count, results.Count);
            Assert.AreEqual(testLinkedScanCodes[0], results[0].LinkedScanCode);
            Assert.AreEqual(testScanCodes[0], results[0].ScanCode);
            Assert.AreEqual(testBusinessUnits[0], results[0].BusinessUnitId);
            Assert.AreEqual(testLinkedScanCodes[1], results[1].LinkedScanCode);
            Assert.AreEqual(testScanCodes[0], results[1].ScanCode);
            Assert.AreEqual(testBusinessUnits[1], results[1].BusinessUnitId);
        }

        [TestMethod]
        public void GetCurrentLinkedScanCodesQuery_ScanCodeHasLinkedScanCodeForOneBusinessUnitButNotAnother_OneLinkedScanCodesShouldBeReturned()
        {
            // Given.
            StageTestScanCodes(new List<string> { testScanCodes[0] });
            StageTestBusinessUnits(testBusinessUnits);
            StageTestLinkedScanCodes(testLinkedScanCodes);

            var itemLink = new ItemLink
            {
                parentItemID = testLinkedItems[0].itemID,
                childItemID = testItems[0].itemID,
                localeID = testLocales[0].localeID
            };
            context.Context.ItemLink.Add(itemLink);
            context.Context.SaveChanges();

            var parameters = new GetCurrentLinkedScanCodesQuery
            {
                ScanCodesByBusinessUnit = new List<Tuple<string, int>>
                {
                    new Tuple<string, int>(testScanCodes[0], testBusinessUnits[0]),
                    new Tuple<string, int>(testScanCodes[0], testBusinessUnits[1])
                }
            };

            // When.
            var results = queryHandler.Execute(parameters);

            // Then.
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(testLinkedScanCodes[0], results[0].LinkedScanCode);
            Assert.AreEqual(testScanCodes[0], results[0].ScanCode);
            Assert.AreEqual(testBusinessUnits[0], results[0].BusinessUnitId);
        }

        [TestMethod]
        public void GetCurrentLinkedScanCodesQuery_TwoScanCodesEachHaveLinkedScanCodeForOneBusinessUnit_TwoLinkedScanCodesShouldBeReturned()
        {
            // Given.
            StageTestScanCodes(testScanCodes);
            StageTestBusinessUnits(testBusinessUnits);
            StageTestLinkedScanCodes(testLinkedScanCodes);

            var itemLinks = new List<ItemLink>
            {
                new ItemLink
                {
                    parentItemID = testLinkedItems[0].itemID,
                    childItemID = testItems[0].itemID,
                    localeID = testLocales[0].localeID
                },
                new ItemLink
                {
                    parentItemID = testLinkedItems[0].itemID,
                    childItemID = testItems[1].itemID,
                    localeID = testLocales[0].localeID
                }
            };
            context.Context.ItemLink.AddRange(itemLinks);
            context.Context.SaveChanges();

            var parameters = new GetCurrentLinkedScanCodesQuery
            {
                ScanCodesByBusinessUnit = new List<Tuple<string, int>>
                {
                    new Tuple<string, int>(testScanCodes[0], testBusinessUnits[0]),
                    new Tuple<string, int>(testScanCodes[1], testBusinessUnits[0])
                }
            };

            // When.
            var results = queryHandler.Execute(parameters);

            // Then.
            Assert.AreEqual(itemLinks.Count, results.Count);
            Assert.AreEqual(testLinkedScanCodes[0], results[0].LinkedScanCode);
            Assert.AreEqual(testScanCodes[0], results[0].ScanCode);
            Assert.AreEqual(testBusinessUnits[0], results[0].BusinessUnitId);
            Assert.AreEqual(testLinkedScanCodes[0], results[1].LinkedScanCode);
            Assert.AreEqual(testScanCodes[1], results[1].ScanCode);
            Assert.AreEqual(testBusinessUnits[0], results[1].BusinessUnitId);
        }

        [TestMethod]
        public void GetCurrentLinkedScanCodesQuery_TwoScanCodesEachHaveLinkedScanCodeForDifferentBusinessUnits_TwoLinkedScanCodesShouldBeReturned()
        {
            // Given.
            StageTestScanCodes(testScanCodes);
            StageTestBusinessUnits(testBusinessUnits);
            StageTestLinkedScanCodes(testLinkedScanCodes);

            var itemLinks = new List<ItemLink>
            {
                new ItemLink
                {
                    parentItemID = testLinkedItems[0].itemID,
                    childItemID = testItems[0].itemID,
                    localeID = testLocales[0].localeID
                },
                new ItemLink
                {
                    parentItemID = testLinkedItems[0].itemID,
                    childItemID = testItems[1].itemID,
                    localeID = testLocales[1].localeID
                }
            };
            context.Context.ItemLink.AddRange(itemLinks);
            context.Context.SaveChanges();

            var parameters = new GetCurrentLinkedScanCodesQuery
            {
                ScanCodesByBusinessUnit = new List<Tuple<string, int>>
                {
                    new Tuple<string, int>(testScanCodes[0], testBusinessUnits[0]),
                    new Tuple<string, int>(testScanCodes[1], testBusinessUnits[1])
                }
            };

            // When.
            var results = queryHandler.Execute(parameters);

            // Then.
            Assert.AreEqual(itemLinks.Count, results.Count);
            Assert.AreEqual(testLinkedScanCodes[0], results[0].LinkedScanCode);
            Assert.AreEqual(testScanCodes[0], results[0].ScanCode);
            Assert.AreEqual(testBusinessUnits[0], results[0].BusinessUnitId);
            Assert.AreEqual(testLinkedScanCodes[0], results[1].LinkedScanCode);
            Assert.AreEqual(testScanCodes[1], results[1].ScanCode);
            Assert.AreEqual(testBusinessUnits[1], results[1].BusinessUnitId);
        }

        [TestMethod]
        public void GetCurrentLinkedScanCodesQuery_TwoScanCodesEachHaveDifferentLinkedScanCodesForOneBusinessUnit_TwoLinkedScanCodesShouldBeReturned()
        {
            // Given.
            StageTestScanCodes(testScanCodes);
            StageTestBusinessUnits(testBusinessUnits);
            StageTestLinkedScanCodes(testLinkedScanCodes);

            var itemLinks = new List<ItemLink>
            {
                new ItemLink
                {
                    parentItemID = testLinkedItems[0].itemID,
                    childItemID = testItems[0].itemID,
                    localeID = testLocales[0].localeID
                },
                new ItemLink
                {
                    parentItemID = testLinkedItems[1].itemID,
                    childItemID = testItems[1].itemID,
                    localeID = testLocales[0].localeID
                }
            };
            context.Context.ItemLink.AddRange(itemLinks);
            context.Context.SaveChanges();

            var parameters = new GetCurrentLinkedScanCodesQuery
            {
                ScanCodesByBusinessUnit = new List<Tuple<string, int>>
                {
                    new Tuple<string, int>(testScanCodes[0], testBusinessUnits[0]),
                    new Tuple<string, int>(testScanCodes[1], testBusinessUnits[0])
                }
            };

            // When.
            var results = queryHandler.Execute(parameters);

            // Then.
            Assert.AreEqual(itemLinks.Count, results.Count);
            Assert.AreEqual(testLinkedScanCodes[0], results[0].LinkedScanCode);
            Assert.AreEqual(testScanCodes[0], results[0].ScanCode);
            Assert.AreEqual(testBusinessUnits[0], results[0].BusinessUnitId);
            Assert.AreEqual(testLinkedScanCodes[1], results[1].LinkedScanCode);
            Assert.AreEqual(testScanCodes[1], results[1].ScanCode);
            Assert.AreEqual(testBusinessUnits[0], results[1].BusinessUnitId);
        }

        [TestMethod]
        public void GetCurrentLinkedScanCodesQuery_TwoScanCodesEachHaveDifferentLinkedScanCodesForDifferentBusinessUnits_TwoLinkedScanCodesShouldBeReturned()
        {
            // Given.
            StageTestScanCodes(testScanCodes);
            StageTestBusinessUnits(testBusinessUnits);
            StageTestLinkedScanCodes(testLinkedScanCodes);

            var itemLinks = new List<ItemLink>
            {
                new ItemLink
                {
                    parentItemID = testLinkedItems[0].itemID,
                    childItemID = testItems[0].itemID,
                    localeID = testLocales[0].localeID
                },
                new ItemLink
                {
                    parentItemID = testLinkedItems[1].itemID,
                    childItemID = testItems[1].itemID,
                    localeID = testLocales[1].localeID
                }
            };
            context.Context.ItemLink.AddRange(itemLinks);
            context.Context.SaveChanges();

            var parameters = new GetCurrentLinkedScanCodesQuery
            {
                ScanCodesByBusinessUnit = new List<Tuple<string, int>>
                {
                    new Tuple<string, int>(testScanCodes[0], testBusinessUnits[0]),
                    new Tuple<string, int>(testScanCodes[1], testBusinessUnits[1])
                }
            };

            // When.
            var results = queryHandler.Execute(parameters);

            // Then.
            Assert.AreEqual(itemLinks.Count, results.Count);
            Assert.AreEqual(testLinkedScanCodes[0], results[0].LinkedScanCode);
            Assert.AreEqual(testScanCodes[0], results[0].ScanCode);
            Assert.AreEqual(testBusinessUnits[0], results[0].BusinessUnitId);
            Assert.AreEqual(testLinkedScanCodes[1], results[1].LinkedScanCode);
            Assert.AreEqual(testScanCodes[1], results[1].ScanCode);
            Assert.AreEqual(testBusinessUnits[1], results[1].BusinessUnitId);
        }
    }
}
