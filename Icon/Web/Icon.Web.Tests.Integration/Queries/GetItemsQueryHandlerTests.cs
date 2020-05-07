using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Infrastructure.ItemSearch;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetItemsQueryHandlerTests
    {
        private GetItemsQueryHandler queryHandler;
        private GetItemsParameters parameters;
        private IDbConnection dbConnection;
        private TransactionScope transaction;
        private ItemTestHelper itemTestHelper;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            dbConnection = SqlConnectionBuilder.CreateIconConnection();
            queryHandler = new GetItemsQueryHandler(dbConnection, new ItemQueryBuilder());
            parameters = new GetItemsParameters();
            parameters.Top = 20;
            itemTestHelper = new ItemTestHelper();
            itemTestHelper.Initialize(dbConnection, initializeTestItem: false);
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
            // Disposing the connection explicitly because some tests fail non-deterministically otherwise.
            dbConnection.Dispose();
        }

        [TestMethod]
        public void GetItems_SearchByItemIdAndItemExists_ReturnItem()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(item);
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("ItemId", AttributeSearchOperator.ExactlyMatchesAll, item.ItemId.ToString())
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(1, results.TotalRecordsCount);
            Assert.AreEqual(1, results.Items.Count());
            AssertItemResultsAreEqual(item, results.Items.Single());
        }

        [TestMethod]
        public void GetItems_SearchByScanCodeAndItemsExists_ReturnsMultipleItems()
        {
            //Given
            const string item1ScanCode = "654987169985";
            const string item2ScanCode = "201689762035";

            var item = itemTestHelper.CreateDefaultTestItem();
            item.ScanCode = item1ScanCode;
            itemTestHelper.SaveItem(item);

            var item2 = itemTestHelper.CreateDefaultTestItem();
            item2.ScanCode = item2ScanCode;
            itemTestHelper.SaveItem(item2);

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("ScanCode",AttributeSearchOperator.ContainsAny, $"{item1ScanCode} {item2ScanCode}")
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(2, results.TotalRecordsCount);
            Assert.AreEqual(2, results.Items.Count());
            AssertItemResultsAreEqual(item, results.Items.FirstOrDefault(i => i.ScanCode == item1ScanCode));
            AssertItemResultsAreEqual(item2, results.Items.FirstOrDefault(i => i.ScanCode == item2ScanCode));
        }

        [TestMethod]
        public void GetItems_SearchByItemIdItemExistsAndInactiveIsTrue_DoesNotReturnItem()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            item.ItemAttributesJson = "{\"Inactive\":\"true\"}";
            itemTestHelper.SaveItem(item);
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                 new ItemSearchCriteria("ItemId", AttributeSearchOperator.ExactlyMatchesAll, item.ItemId.ToString())
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(0, results.TotalRecordsCount);
        }

        [TestMethod]
        public void GetItems_SearchByItemIdItemExistsAndInactiveIsFalse_ReturnItem()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            item.ItemAttributesJson = "{\"Inactive\":\"False\"}";
            itemTestHelper.SaveItem(item);
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("ItemId",AttributeSearchOperator.ContainsAll, item.ItemId.ToString())
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(1, results.TotalRecordsCount);
            Assert.AreEqual(1, results.Items.Count());
            AssertItemResultsAreEqual(item, results.Items.Single());
        }

        [TestMethod]
        public void GetItems_SearchByItemIdAndItemExistsAndIsActive_ReturnItem()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            item.ItemAttributesJson = "{\"Inactive\":\"False\"}";
            itemTestHelper.SaveItem(item);

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("ItemId",AttributeSearchOperator.ContainsAll, item.ItemId.ToString())
            };


            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(1, results.TotalRecordsCount);
        }

        [TestMethod]
        public void GetItems_SearchByItemIdAndItemDoesNotExists_ReturnNoItems()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(item);
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("ItemId",AttributeSearchOperator.ContainsAll, "-1")
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(0, results.TotalRecordsCount);
            Assert.AreEqual(0, results.Items.Count());
        }

        [TestMethod]
        public void GetItems_SearchByScanCodeAndItemExists_ReturnItem()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(item);
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("ScanCode",AttributeSearchOperator.ContainsAll, item.ScanCode)
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(1, results.TotalRecordsCount);
            Assert.AreEqual(1, results.Items.Count());
            AssertItemResultsAreEqual(item, results.Items.Single());
        }

        [TestMethod]
        public void GetItems_SearchByScanCodeAndItemDoesNotExists_ReturnNoItems()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(item);

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("ScanCode",AttributeSearchOperator.ContainsAll, "Test")
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(0, results.TotalRecordsCount);
            Assert.AreEqual(0, results.Items.Count());
        }

        [TestMethod]
        public void GetItems_SearchByMerchandiseHierarchyClassIdAndItemExists_ReturnItem()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(item);
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("Merchandise", AttributeSearchOperator.ContainsAll,item.Merchandise)
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(1, results.TotalRecordsCount);
            Assert.AreEqual(1, results.Items.Count());
            AssertItemResultsAreEqual(item, results.Items.Single());
        }

        [TestMethod]
        public void GetItems_SearchByMerchandiseHierarchyClassIdAndItemDoesNotExists_ReturnNoItems()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(item);

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("Merchandise",AttributeSearchOperator.ExactlyMatchesAll,itemTestHelper.TestHierarchyClasses["Merchandise"].Last().HierarchyClassName)
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(0, results.TotalRecordsCount);
            Assert.AreEqual(0, results.Items.Count());
        }

        [TestMethod]
        public void GetItems_SearchByBrandsHierarchyClassIdAndItemExists_ReturnItem()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(item);
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("Brands", AttributeSearchOperator.ExactlyMatchesAll,item.Brands)
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(1, results.TotalRecordsCount);
            Assert.AreEqual(1, results.Items.Count());
            AssertItemResultsAreEqual(item, results.Items.Single());
        }

        [TestMethod]
        public void GetItems_SearchByBrandsHierarchyClassIdAndItemDoesNotExists_ReturnNoItems()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(item);

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("Brands", AttributeSearchOperator.ExactlyMatchesAll,itemTestHelper.TestHierarchyClasses["Brands"].Last().HierarchyName)
            };
            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(0, results.TotalRecordsCount);
            Assert.AreEqual(0, results.Items.Count());
        }
        [TestMethod]
        public void GetItems_SearchByTaxHierarchyClassIdAndItemExists_ReturnItem()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(item);

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("Tax",AttributeSearchOperator.ContainsAll,$@"""{itemTestHelper.TestHierarchyClasses["Tax"].First().HierarchyClassName}""")
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(1, results.TotalRecordsCount);
            Assert.AreEqual(1, results.Items.Count());
            AssertItemResultsAreEqual(item, results.Items.Single());
        }

        [TestMethod]
        public void GetItems_SearchByTaxHierarchyClassIdAndItemDoesNotExists_ReturnNoItems()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(item);

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("Tax",AttributeSearchOperator.ExactlyMatchesAll, itemTestHelper.TestHierarchyClasses["Tax"].Last().HierarchyClassName)
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(0, results.TotalRecordsCount);
            Assert.AreEqual(0, results.Items.Count());
        }

        [TestMethod]
        public void GetItems_SearchByManufacturerHierarchyClassIdAndItemExists_ReturnItem()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(item);
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("Manufacturer",AttributeSearchOperator.ExactlyMatchesAll, item.Manufacturer)
            };



            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(1, results.TotalRecordsCount);
            Assert.AreEqual(1, results.Items.Count());
            AssertItemResultsAreEqual(item, results.Items.Single());
        }

        [TestMethod]
        public void GetItems_SearchByManufacturerHierarchyClassIdAndItemDoesNotExists_ReturnNoItems()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(item);

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("Manufacturer",AttributeSearchOperator.ContainsAll,itemTestHelper.TestHierarchyClasses["Manufacturer"].Last().HierarchyClassName)
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(0, results.TotalRecordsCount);
            Assert.AreEqual(0, results.Items.Count());
        }

        [TestMethod]
        public void GetItems_SearchByFinancialHierarchyClassIdAndItemExists_ReturnItem()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(item);
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("Financial", AttributeSearchOperator.ExactlyMatchesAll, item.Financial)
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(1, results.TotalRecordsCount);
            Assert.AreEqual(1, results.Items.Count());
            AssertItemResultsAreEqual(item, results.Items.Single());
        }

        [TestMethod]
        public void GetItems_SearchByFinancialHierarchyClassIdAndItemDoesNotExists_ReturnNoItems()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(item);

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("Financial", AttributeSearchOperator.ExactlyMatchesAll, itemTestHelper.TestHierarchyClasses["Financial"].Last().HierarchyClassName)
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(0, results.TotalRecordsCount);
            Assert.AreEqual(0, results.Items.Count());
        }

        [TestMethod]
        public void GetItems_SearchByNationalHierarchyClassIdAndItemExists_ReturnItem()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(item);

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("National",AttributeSearchOperator.ContainsAll,$@"""{item.National}""")
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(1, results.TotalRecordsCount);
            Assert.AreEqual(1, results.Items.Count());
            AssertItemResultsAreEqual(item, results.Items.Single());
        }

        [TestMethod]
        public void GetItems_SearchByNationalHierarchyClassIdAndItemDoesNotExists_ReturnNoItems()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(item);

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("National",AttributeSearchOperator.ExactlyMatchesAll,itemTestHelper.TestHierarchyClasses["National"].Last().HierarchyClassName)
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(0, results.TotalRecordsCount);
            Assert.AreEqual(0, results.Items.Count());
        }

        [TestMethod]
        public void GetItems_SearchByItemAttributesJsonAndItemExists_ReturnItem()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            item.ItemAttributesJson = JsonConvert.SerializeObject(new
            { TestAttribute = "Test", Inactive = "false" });
            itemTestHelper.SaveItem(item);
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("TestAttribute", AttributeSearchOperator.ExactlyMatchesAll, "Test" )
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(1, results.TotalRecordsCount);
            Assert.AreEqual(1, results.Items.Count());
            AssertItemResultsAreEqual(item, results.Items.Single());
        }

        [TestMethod]
        public void GetItems_SearchByItemAttributesJsonAndItemDoesNotExists_ReturnNoItems()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            item.ItemAttributesJson = JsonConvert.SerializeObject(new { TestAttribute = "Test" });
            itemTestHelper.SaveItem(item);

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("TestAttribute", AttributeSearchOperator.ContainsAll, "NotFound" )
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(0, results.TotalRecordsCount);
            Assert.AreEqual(0, results.Items.Count());
        }

        [TestMethod]
        public void GetItems_OrderByValueIsNotSet_ShouldReturnItemsOrderedByScanCodeInAscendingOrder()
        {
            //Given
            var items = new List<ItemDbModel>();
            for (int i = 0; i < 10; i++)
            {
                var item = itemTestHelper.CreateDefaultTestItem();
                item.ScanCode = "00000" + i;
                items.Add(itemTestHelper.SaveItem(item));
            }

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("Brands", AttributeSearchOperator.ExactlyMatchesAll, itemTestHelper.TestHierarchyClasses["Brands"].First().HierarchyClassName)
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(10, results.TotalRecordsCount);
            Assert.AreEqual(10, results.Items.Count());
            List<ItemDbModel> itemsList = results.Items.ToList();
            Assert.AreEqual("000000", itemsList[0].ScanCode);
            Assert.AreEqual("000001", itemsList[1].ScanCode);
            Assert.AreEqual("000002", itemsList[2].ScanCode);
            Assert.AreEqual("000003", itemsList[3].ScanCode);
            Assert.AreEqual("000004", itemsList[4].ScanCode);
            Assert.AreEqual("000005", itemsList[5].ScanCode);
            Assert.AreEqual("000006", itemsList[6].ScanCode);
            Assert.AreEqual("000007", itemsList[7].ScanCode);
            Assert.AreEqual("000008", itemsList[8].ScanCode);
            Assert.AreEqual("000009", itemsList[9].ScanCode);
        }

        [TestMethod]
        public void GetItems_OrderByValueIsSetToScanCodeAndOrderByOrderIsDesc_ShouldReturnItemsOrderedByScanCodeInDescendingOrder()
        {
            //Given
            var items = new List<ItemDbModel>();
            for (int i = 0; i < 10; i++)
            {
                var item = itemTestHelper.CreateDefaultTestItem();
                item.ScanCode = "00000" + i;
                items.Add(itemTestHelper.SaveItem(item));
            }
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("Brands", AttributeSearchOperator.ExactlyMatchesAll, itemTestHelper.TestHierarchyClasses["Brands"].First().HierarchyClassName)
            };
            parameters.OrderByValue = "ScanCode";
            parameters.OrderByOrder = "DESC";

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(10, results.TotalRecordsCount);
            Assert.AreEqual(10, results.Items.Count());
            List<ItemDbModel> itemsList = results.Items.ToList();
            Assert.AreEqual("000009", itemsList[0].ScanCode);
            Assert.AreEqual("000008", itemsList[1].ScanCode);
            Assert.AreEqual("000007", itemsList[2].ScanCode);
            Assert.AreEqual("000006", itemsList[3].ScanCode);
            Assert.AreEqual("000005", itemsList[4].ScanCode);
            Assert.AreEqual("000004", itemsList[5].ScanCode);
            Assert.AreEqual("000003", itemsList[6].ScanCode);
            Assert.AreEqual("000002", itemsList[7].ScanCode);
            Assert.AreEqual("000001", itemsList[8].ScanCode);
            Assert.AreEqual("000000", itemsList[9].ScanCode);
        }

        [TestMethod]
        public void GetItems_OrderByValueIsSetToItemIdAndOrderByOrderIsAscending_ShouldReturnItemsOrderedByItemIdInAscendingOrder()
        {
            //Given
            var items = new List<ItemDbModel>();
            string scanCodes = string.Empty;
            for (int i = 0; i < 10; i++)
            {
                var item = itemTestHelper.CreateDefaultTestItem();
                item.ScanCode = "00000" + i;
                items.Add(itemTestHelper.SaveItem(item));
                scanCodes += $"{item.ScanCode} ";
            }

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("ScanCode", AttributeSearchOperator.ExactlyMatchesAny,scanCodes)
            };

            parameters.OrderByValue = "itemID";
            parameters.OrderByOrder = "ASC";

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(10, results.TotalRecordsCount);
            Assert.AreEqual(10, results.Items.Count());
            var orderedItems = items
                .OrderBy(i => i.ItemId)
                .ToList();
            List<ItemDbModel> itemsList = results.Items.ToList();
            for (int i = 0; i < orderedItems.Count; i++)
            {
                AssertItemResultsAreEqual(orderedItems[i], itemsList[i]);
            }
        }

        [TestMethod]
        public void GetItems_OrderByValueIsSetToItemTypeDescriptionAndOrderByOrderIsAscending_ShouldReturnItemsOrderedByItemTypeDescirptionInAscendingOrder()
        {
            //Given
            var items = new List<ItemDbModel>();
            string scanCodes = string.Empty;
            for (int i = 0; i < 10; i++)
            {
                var item = itemTestHelper.CreateDefaultTestItem();
                item.ScanCode = "00000" + i;
                if (i % 2 == 0)
                {
                    item.ItemTypeId = itemTestHelper.TestItemTypes.Last().ItemTypeId;
                }
                items.Add(itemTestHelper.SaveItem(item));
                scanCodes += $"{item.ScanCode} ";
            }
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("ScanCode", AttributeSearchOperator.ExactlyMatchesAny,scanCodes)
            };
            parameters.OrderByValue = "ItemTypeDescription";
            parameters.OrderByOrder = "ASC";

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(10, results.TotalRecordsCount);
            Assert.AreEqual(10, results.Items.Count());
            var orderedItems = items
                .OrderBy(i => i.ItemTypeId)
                .ThenBy(i => i.ScanCode)
                .ToList();
            List<ItemDbModel> itemsList = results.Items.ToList();
            for (int i = 0; i < orderedItems.Count; i++)
            {
                AssertItemResultsAreEqual(orderedItems[i], itemsList[i]);
            }
        }

        [TestMethod]
        public void GetItems_OrderByValueIsSetToBrandHierarchyClassIdAndOrderByOrderIsAscending_ShouldReturnItemsOrderedByBrandHierarchyClassIdInAscendingOrder()
        {
            //Given
            var items = new List<ItemDbModel>();

            string scanCodes = string.Empty;
            for (int i = 0; i < 10; i++)
            {
                var item = itemTestHelper.CreateDefaultTestItem();
                item.ScanCode = "00000" + i;
                if (i % 2 == 0)
                {
                    item.BrandsHierarchyClassId = itemTestHelper.TestHierarchyClasses["Brands"].Last().HierarchyClassId;
                    item.Brands = itemTestHelper.TestHierarchyClasses["Brands"].Last().HierarchyLineage;
                }
                items.Add(itemTestHelper.SaveItem(item));
                scanCodes += $"{item.ScanCode} ";
            }

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("ScanCode", AttributeSearchOperator.ExactlyMatchesAny, scanCodes)
            };

            parameters.OrderByValue = "BrandsHierarchyClassId";
            parameters.OrderByOrder = "ASC";

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(10, results.TotalRecordsCount);
            Assert.AreEqual(10, results.Items.Count());
            var orderedItems = items
                .OrderBy(i => i.Brands)
                .ThenBy(i => i.ScanCode)
                .ToList();
            List<ItemDbModel> itemsList = results.Items.ToList();
            for (int i = 0; i < orderedItems.Count; i++)
            {
                AssertItemResultsAreEqual(orderedItems[i], itemsList[i]);
            }
        }

        [TestMethod]
        public void GetItems_OrderByValueIsSetToMerchandiseHierarchyClassIdAndOrderByOrderIsAscending_ShouldReturnItemsOrderedByMerchandiseHierarchyClassIdInAscendingOrder()
        {
            //Given
            var items = new List<ItemDbModel>();
            string scanCodes = string.Empty;
            for (int i = 0; i < 10; i++)
            {
                var item = itemTestHelper.CreateDefaultTestItem();
                item.ScanCode = "00000" + i;
                if (i % 2 == 0)
                {
                    item.MerchandiseHierarchyClassId = itemTestHelper.TestHierarchyClasses["Merchandise"].Last().HierarchyClassId;
                    item.Merchandise = itemTestHelper.TestHierarchyClasses["Merchandise"].Last().HierarchyLineage;
                }
                items.Add(itemTestHelper.SaveItem(item));
                scanCodes += $"{item.ScanCode} ";
            }

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("ScanCode", AttributeSearchOperator.ExactlyMatchesAny,scanCodes)
            };
            parameters.OrderByValue = "MerchandiseHierarchyClassId";
            parameters.OrderByOrder = "ASC";

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(10, results.TotalRecordsCount);
            Assert.AreEqual(10, results.Items.Count());
            var orderedItems = items
                .OrderBy(i => i.MerchandiseHierarchyClassId)
                .ThenBy(i => i.ScanCode)
                .ToList();
            List<ItemDbModel> itemsList = results.Items.ToList();
            for (int i = 0; i < orderedItems.Count; i++)
            {
                AssertItemResultsAreEqual(orderedItems[i], itemsList[i]);
            }
        }

        [TestMethod]
        public void GetItems_OrderByValueIsSetToTaxHierarchyClassIdAndOrderByOrderIsAscending_ShouldReturnItemsOrderedByTaxHierarchyClassIdInAscendingOrder()
        {
            //Given
            var items = new List<ItemDbModel>();
            string scanCodes = string.Empty;
            for (int i = 0; i < 10; i++)
            {
                var item = itemTestHelper.CreateDefaultTestItem();
                item.ScanCode = "00000" + i;
                if (i % 2 == 0)
                {
                    item.TaxHierarchyClassId = itemTestHelper.TestHierarchyClasses["Tax"].Last().HierarchyClassId;
                    item.Tax = itemTestHelper.TestHierarchyClasses["Tax"].Last().HierarchyLineage;
                }
                items.Add(itemTestHelper.SaveItem(item));
                scanCodes += $"{item.ScanCode} ";
            }

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("ScanCode", AttributeSearchOperator.ExactlyMatchesAny,scanCodes)
            };
            parameters.OrderByValue = "TaxHierarchyClassId";
            parameters.OrderByOrder = "ASC";

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(10, results.TotalRecordsCount);
            Assert.AreEqual(10, results.Items.Count());
            var orderedItems = items
                .OrderBy(i => i.TaxHierarchyClassId)
                .ThenBy(i => i.ScanCode)
                .ToList();
            List<ItemDbModel> itemsList = results.Items.ToList();
            for (int i = 0; i < orderedItems.Count; i++)
            {
                AssertItemResultsAreEqual(orderedItems[i], itemsList[i]);
            }
        }

        [TestMethod]
        public void GetItems_OrderByValueIsSetToFinancialHierarchyClassIdAndOrderByOrderIsAscending_ShouldReturnItemsOrderedByFinancialHierarchyClassIdInAscendingOrder()
        {
            //Given
            var items = new List<ItemDbModel>();
            string scanCodes = string.Empty;
            for (int i = 0; i < 10; i++)
            {
                var item = itemTestHelper.CreateDefaultTestItem();
                item.ScanCode = "00000" + i;
                if (i % 2 == 0)
                {
                    item.FinancialHierarchyClassId = itemTestHelper.TestHierarchyClasses["Financial"].Last().HierarchyClassId;
                    item.Brands = itemTestHelper.TestHierarchyClasses["Financial"].Last().HierarchyLineage;
                }
                items.Add(itemTestHelper.SaveItem(item));
                scanCodes += $"{item.ScanCode} ";
            }

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("ScanCode", AttributeSearchOperator.ExactlyMatchesAny, scanCodes)
            };

            parameters.OrderByValue = "FinancialHierarchyClassId";
            parameters.OrderByOrder = "ASC";

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(10, results.TotalRecordsCount);
            Assert.AreEqual(10, results.Items.Count());
            var orderedItems = items
                .OrderBy(i => i.FinancialHierarchyClassId)
                .ThenBy(i => i.ScanCode)
                .ToList();
            List<ItemDbModel> itemsList = results.Items.ToList();
            for (int i = 0; i < orderedItems.Count; i++)
            {
                AssertItemResultsAreEqual(orderedItems[i], itemsList[i]);
            }
        }

        [TestMethod]
        public void GetItems_OrderByValueIsSetToNationalHierarchyClassIdAndOrderByOrderIsAscending_ShouldReturnItemsOrderedByNationalHierarchyClassIdInAscendingOrder()
        {
            //Given
            var items = new List<ItemDbModel>();
            string scanCodes = string.Empty;
            for (int i = 0; i < 10; i++)
            {
                var item = itemTestHelper.CreateDefaultTestItem();
                item.ScanCode = "00000" + i;
                if (i % 2 == 0)
                {
                    item.NationalHierarchyClassId = itemTestHelper.TestHierarchyClasses["National"].Last().HierarchyClassId;
                    item.National = itemTestHelper.TestHierarchyClasses["National"].Last().HierarchyLineage;
                }
                items.Add(itemTestHelper.SaveItem(item));
                scanCodes += $"{item.ScanCode} ";
            }
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("ScanCode", AttributeSearchOperator.ExactlyMatchesAny,scanCodes)
            };
            parameters.OrderByValue = "NationalHierarchyClassId";
            parameters.OrderByOrder = "ASC";

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(10, results.TotalRecordsCount);
            Assert.AreEqual(10, results.Items.Count());
            var orderedItems = items
                .OrderBy(i => i.NationalHierarchyClassId)
                .ThenBy(i => i.ScanCode)
                .ToList();
            List<ItemDbModel> itemsList = results.Items.ToList();
            for (int i = 0; i < orderedItems.Count; i++)
            {
                AssertItemResultsAreEqual(orderedItems[i], itemsList[i]);
            }
        }

        [TestMethod]
        public void GetItems_OrderByAscendingUsingAttribute_ShouldSortAscendingByAttribute()
        {
            var items = new List<ItemDbModel>();

            var item1 = itemTestHelper.CreateDefaultTestItem();
            item1.ScanCode = "000002";
            item1.ItemAttributesJson = JsonConvert.SerializeObject(new Dictionary<string, string>()
            {
                    { "Notes", $"3685bb78-b331-4079-a39e-87d5fa21cc63" },
                    { "AlcoholByVolume", $"2" },
                    { "Inactive", "false"}

            });
            items.Add(itemTestHelper.SaveItem(item1));

            var item2 = itemTestHelper.CreateDefaultTestItem();
            item2.ScanCode = "000001";
            item2.ItemAttributesJson = JsonConvert.SerializeObject(new Dictionary<string, string>()
            {
                    { "Notes", $"3685bb78-b331-4079-a39e-87d5fa21cc63" },
                    { "AlcoholByVolume", $"1" },
                    { "Inactive", "false"}

            });
            items.Add(itemTestHelper.SaveItem(item2));

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("Notes", AttributeSearchOperator.ContainsAll, "3685bb78-b331-4079-a39e-87d5fa21cc63" )
            };

            parameters.OrderByValue = "AlcoholByVolume";
            parameters.OrderByOrder = "ASC";

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual("000001", results.Items.ToList()[0].ScanCode);
            Assert.AreEqual("000002", results.Items.ToList()[1].ScanCode);
        }

        [TestMethod]
        public void GetItems_OrderByDescendingUsingAttribute_ShouldSortDescendingByAttribute()
        {
            var items = new List<ItemDbModel>();
            var item1 = itemTestHelper.CreateDefaultTestItem();
            item1.ScanCode = "000001";
            item1.ItemAttributesJson = JsonConvert.SerializeObject(new Dictionary<string, string>()
            {
                { "Notes", $"3685bb78-b331-4079-a39e-87d5fa21cc63" },
                { "AlcoholByVolume", $"1" },
                { "Inactive", "false"}
            });
            items.Add(itemTestHelper.SaveItem(item1));

            var item2 = itemTestHelper.CreateDefaultTestItem();
            item2.ScanCode = "000002";
            item2.ItemAttributesJson = JsonConvert.SerializeObject(new Dictionary<string, string>()
            {
                { "Notes", $"3685bb78-b331-4079-a39e-87d5fa21cc63" },
                { "AlcoholByVolume", $"2" },
                { "Inactive", "false"}
            });
            items.Add(itemTestHelper.SaveItem(item2));

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("Notes", AttributeSearchOperator.ContainsAll, "3685bb78-b331-4079-a39e-87d5fa21cc63" )
            };

            parameters.OrderByValue = "AlcoholByVolume";
            parameters.OrderByOrder = "DESC";

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual("000002", results.Items.ToList()[0].ScanCode);
            Assert.AreEqual("000001", results.Items.ToList()[1].ScanCode);
        }

        [TestMethod]
        public void GetItems_TopIs5AndNumberOfItemsMatchedIs10_ShouldOnlyReturn5()
        {
            //Given
            parameters.Top = 5;
            var item = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(item);
            for (int i = 0; i < 9; i++)
            {
                item = itemTestHelper.CreateDefaultTestItem();
                item.ScanCode = "00000" + i;
                itemTestHelper.SaveItem(item);
            }
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("Brands", AttributeSearchOperator.ExactlyMatchesAll, itemTestHelper.TestHierarchyClasses["Brands"].First().HierarchyClassName)
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(10, results.TotalRecordsCount);
            Assert.AreEqual(5, results.Items.Count());
            List<ItemDbModel> itemsList = results.Items.ToList();
            Assert.AreEqual("9999999999999", itemsList[0].ScanCode);
            Assert.AreEqual("000000", itemsList[1].ScanCode);
            Assert.AreEqual("000001", itemsList[2].ScanCode);
            Assert.AreEqual("000002", itemsList[3].ScanCode);
            Assert.AreEqual("000003", itemsList[4].ScanCode);
        }


        [TestMethod]
        public void GetItems_HasAttribute_ItemHasAttributeAndIsReturned()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            item.ItemAttributesJson = "{\"Inactive\":\"false\",\"testAttribute\":\"Test\"}";
            itemTestHelper.SaveItem(item);
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("testAttribute", AttributeSearchOperator.HasAttribute, string.Empty),
                new ItemSearchCriteria("ItemId", AttributeSearchOperator.ExactlyMatchesAny, item.ItemId.ToString())
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(1, results.TotalRecordsCount);
            Assert.AreEqual(item.ItemId, results.Items.First().ItemId);
        }


        [TestMethod]
        public void GetItems_HasAttribute_NoItemsHaveAttributeAndNoRecordsReturned()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(item);
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("AttributeThatDoesNotExist", AttributeSearchOperator.HasAttribute, string.Empty),
                new ItemSearchCriteria("ItemId", AttributeSearchOperator.ExactlyMatchesAny, item.ItemId.ToString())
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(0, results.TotalRecordsCount);
        }

        [TestMethod]
        public void GetItems_DoesNotHaveAttribute_ItemHasAttributeAndIsNotReturned()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            item.ItemAttributesJson = "{\"Inactive\":\"false\",\"testAttribute\":\"Test\"}";
            itemTestHelper.SaveItem(item);         
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("testAttribute", AttributeSearchOperator.DoesNotHaveAttribute, string.Empty),
                new ItemSearchCriteria("ItemId", AttributeSearchOperator.ExactlyMatchesAny, item.ItemId.ToString())
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(0, results.TotalRecordsCount);
        }


        [TestMethod]
        public void GetItems_DoesNotHaveAttribute_NoItemsHaveAttributeRecordsReturned()
        {
            //Given
            var item = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(item);
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("AttributeThatDoesNotExist", AttributeSearchOperator.DoesNotHaveAttribute, string.Empty),
                new ItemSearchCriteria("ItemId", AttributeSearchOperator.ExactlyMatchesAny, item.ItemId.ToString())
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(1, results.TotalRecordsCount);
            Assert.AreEqual(item.ItemId, results.Items.First().ItemId);
        }
        [TestMethod]
        public void GetItems_TopIs5AndNumberOfItemsMatchedIs10AndSkipIs5_ShouldOnlyReturn5AndSkipFirst5Items()
        {
            //Given
            parameters.Top = 5;
            var item = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(item);
            for (int i = 0; i < 9; i++)
            {
                item = itemTestHelper.CreateDefaultTestItem();
                item.ScanCode = "00000" + i;
                itemTestHelper.SaveItem(item);
            }
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("Brands", AttributeSearchOperator.ExactlyMatchesAll, itemTestHelper.TestHierarchyClasses["Brands"].First().HierarchyClassName)
            };
            parameters.Skip = 5;

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(10, results.TotalRecordsCount);
            Assert.AreEqual(5, results.Items.Count());
            List<ItemDbModel> itemsList = results.Items.ToList();
            Assert.AreEqual("000004", itemsList[0].ScanCode);
            Assert.AreEqual("000005", itemsList[1].ScanCode);
            Assert.AreEqual("000006", itemsList[2].ScanCode);
            Assert.AreEqual("000007", itemsList[3].ScanCode);
            Assert.AreEqual("000008", itemsList[4].ScanCode);
        }


        [TestMethod]
        public void GetItems_TopIs5AndNumberOfItemsMatchedIs10AndSkipIs10_ShouldReturn0Items()
        {
            //Given
            parameters.Top = 5;
            parameters.Skip = 10;
            var item = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(item);
            for (int i = 0; i < 9; i++)
            {
                item = itemTestHelper.CreateDefaultTestItem();
                item.ScanCode = "00000" + i;
                itemTestHelper.SaveItem(item);
            }
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("Brands", AttributeSearchOperator.ExactlyMatchesAll, itemTestHelper.TestHierarchyClasses["Brands"].First().HierarchyClassName)
            };
            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(10, results.TotalRecordsCount);
            Assert.AreEqual(0, results.Items.Count());
        }

        [TestMethod]
        public void GetItems_SearchByMultipleJsonAttributesAndBrandsHierarchyClassIdAnd3ItemsMatch_ShouldReturn3Items()
        {
            //Given
            var expectedItems = new List<ItemDbModel>();
            for (int i = 0; i < 3; i++)
            {
                var item = itemTestHelper.CreateDefaultTestItem();
                item.ScanCode = "000" + i;
                item.ItemAttributesJson = JsonConvert.SerializeObject(
                    new
                    {
                        TestAttribute1 = "TestAttribute1Find",
                        TestAttribute2 = "TestAttribute2Find",
                        TestAttribute3 = "TestAttribute3",
                        Inactive = "false"
                    });
                expectedItems.Add(itemTestHelper.SaveItem(item));
            }
            var items = new List<ItemDbModel>();
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                var item = itemTestHelper.CreateDefaultTestItem();
                item.ScanCode = "00000" + i;
                item.ItemAttributesJson = JsonConvert.SerializeObject(
                    new
                    {
                        TestAttribute1 = "TestAttribute1" + i,
                        TestAttribute2 = "TestAttribute2" + random.Next(),
                        TestAttribute3 = "TestAttribute3" + i,
                        Inactive = "false"
                    });
                items.Add(itemTestHelper.SaveItem(item));
            }
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("Brands", AttributeSearchOperator.ContainsAll, itemTestHelper.TestHierarchyClasses["Brands"].First().HierarchyClassName)
            };

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("TestAttribute1", AttributeSearchOperator.ExactlyMatchesAll, "TestAttribute1Find" ),
                new ItemSearchCriteria("TestAttribute2", AttributeSearchOperator.ExactlyMatchesAll, "TestAttribute2Find" )
            };


            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(3, results.TotalRecordsCount);
            Assert.AreEqual(3, results.Items.Count());
            List<ItemDbModel> itemsList = results.Items.OrderBy(i => i.ScanCode).ToList();
            for (int i = 0; i < expectedItems.Count; i++)
            {
                AssertItemResultsAreEqual(expectedItems[i], itemsList[i]);
            }
        }

        [TestMethod]
        public void GetItems_SearchByScanCodeButNoItemExists_ShouldReturn0Items()
        {
            //Given
            var items = new List<ItemDbModel>();
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                var item = itemTestHelper.CreateDefaultTestItem();
                item.ScanCode = "00000" + i;
                item.ItemAttributesJson = JsonConvert.SerializeObject(
                    new
                    {
                        TestAttribute1 = "TestAttribute1" + i,
                        TestAttribute2 = "TestAttribute2" + random.Next(),
                        TestAttribute3 = "TestAttribute3" + i
                    });
                items.Add(itemTestHelper.SaveItem(item));
            }

            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("ScanCode",AttributeSearchOperator.ExactlyMatchesAll, "0000000000")
            };

            //When
            var results = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(0, results.TotalRecordsCount);
            Assert.AreEqual(0, results.Items.Count());
        }

        private static void AssertItemResultsAreEqual(ItemDbModel expectedItem, ItemDbModel actualItem)
        {
            Assert.AreEqual(expectedItem.BrandsHierarchyClassId, actualItem.BrandsHierarchyClassId);
            Assert.AreEqual(expectedItem.FinancialHierarchyClassId, actualItem.FinancialHierarchyClassId);
            Assert.AreEqual(expectedItem.ItemAttributesJson, actualItem.ItemAttributesJson);
            Assert.AreEqual(expectedItem.ItemId, actualItem.ItemId);
            Assert.AreEqual(expectedItem.ItemTypeId, actualItem.ItemTypeId);
            Assert.AreEqual(expectedItem.MerchandiseHierarchyClassId, actualItem.MerchandiseHierarchyClassId);
            Assert.AreEqual(expectedItem.NationalHierarchyClassId, actualItem.NationalHierarchyClassId);
            Assert.AreEqual(expectedItem.ScanCode, actualItem.ScanCode);
            Assert.AreEqual(expectedItem.BarcodeTypeId, actualItem.BarcodeTypeId);
            Assert.AreEqual(expectedItem.TaxHierarchyClassId, actualItem.TaxHierarchyClassId);
        }
    }
}
