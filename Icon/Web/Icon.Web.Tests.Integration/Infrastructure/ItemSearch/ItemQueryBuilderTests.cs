using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Infrastructure.ItemSearch;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;

namespace Icon.Web.Tests.Integration.Infrastructure
{
    [TestClass]
    public class ItemQueryBuilderTests
    {
        [TestMethod]
        public void BuildHasAttributeQuery_ResponseIsInCorrectFormat()
        {
            // Given.
            ItemQueryBuilder builder = new ItemQueryBuilder();

            // When.
            string response = builder.BuildHasAttributeQuery(new DataAccess.Infrastructure.ItemSearch.ItemSearchCriteria("A", AttributeSearchOperator.ContainsAll, "test"));

            // Then.
            Assert.AreEqual(@"JSON_VALUE(ItemAttributesJson, '$.""A""') IS NOT NULL", response);
        }

        [TestMethod]
        public void BuildDoesNotHaveAttributeQuery_ResponseIsInCorrectFormat()
        {
            // Given.
            ItemQueryBuilder builder = new ItemQueryBuilder();

            // When.
            string response = builder.BuildDoesNotHaveAttributeQuery(new DataAccess.Infrastructure.ItemSearch.ItemSearchCriteria("A", AttributeSearchOperator.ContainsAll, "test"));

            // Then.
            Assert.AreEqual(@"JSON_VALUE(ItemAttributesJson, '$.""A""') IS NULL", response);
        }

        [TestMethod]
        public void BuildSearchValue_IsNotSpecialAttribute_ResponseIsInCorrectFormat()
        {
            // Given.
            ItemQueryBuilder builder = new ItemQueryBuilder();

            // When.
            string response = builder.BuildSearchValue("A");

            // Then.
            Assert.AreEqual(@"JSON_VALUE(ItemAttributesJson, '$.""A""')", response);
        }

        [TestMethod]
        public void BuildSearchValue_IsSpecialAttribute_ResponseIsInCorrectFormat()
        {
            // Given.
            ItemQueryBuilder builder = new ItemQueryBuilder();

            // When.
            string response = builder.BuildSearchValue("ItemId");

            // Then.
            Assert.AreEqual("i.ItemId", response);
        }

        [TestMethod]
        public void BuildContainsAnyQuery_ResponseIsInCorrectFormat()
        {
            // Given.
            ItemQueryBuilder builder = new ItemQueryBuilder();

            // When.
            string response = builder.BuildContainsAnyQuery(new ItemSearchCriteria("AttributeName", AttributeSearchOperator.ContainsAny, "ValueA ValueB"));

            // Then.
            Assert.AreEqual(@"JSON_VALUE(ItemAttributesJson, '$.""AttributeName""') LIKE '%ValueA%' OR JSON_VALUE(ItemAttributesJson, '$.""AttributeName""') LIKE '%ValueB%'", response);
        }

        [TestMethod]
        public void BuildContainsAllQuery_ResponseIsInCorrectFormat()
        {
            // Given.
            ItemQueryBuilder builder = new ItemQueryBuilder();

            // When.
            string response = builder.BuildContainsAllQuery(new ItemSearchCriteria("AttributeName", AttributeSearchOperator.ContainsAll, "ValueA ValueB"));

            // Then.
            Assert.AreEqual(@"JSON_VALUE(ItemAttributesJson, '$.""AttributeName""') LIKE '%ValueA%' AND JSON_VALUE(ItemAttributesJson, '$.""AttributeName""') LIKE '%ValueB%'", response);
        }

        [TestMethod]
        public void BuildExactlyAnyQuery_ResponseIsInCorrectFormat()
        {
            // Given.
            ItemQueryBuilder builder = new ItemQueryBuilder();

            // When.
            string response = builder.BuildExactlyAnyQuery(new ItemSearchCriteria("AttributeName", AttributeSearchOperator.ExactlyMatchesAny, "ValueA ValueB"));

            // Then.
            Assert.AreEqual(@"JSON_VALUE(ItemAttributesJson, '$.""AttributeName""') = 'ValueA' OR JSON_VALUE(ItemAttributesJson, '$.""AttributeName""') = 'ValueB'", response);
        }

        [TestMethod]
        public void BuildExactlyAllQuery_ResponseIsInCorrectFormat()
        {
            // Given.
            ItemQueryBuilder builder = new ItemQueryBuilder();

            // When.
            string response = builder.BuildExactlyAllQuery(new ItemSearchCriteria("AttributeName", AttributeSearchOperator.ExactlyMatchesAll, "ValueA ValueB"));

            // Then.
            Assert.AreEqual(@"JSON_VALUE(ItemAttributesJson, '$.""AttributeName""') = 'ValueA ValueB'", response);
        }

        [TestMethod]
        public void BuildWhereClause_WithoutSpecialAttributesResponseIsInCorrectFormat()
        {
            // Given.
            ItemQueryBuilder builder = new ItemQueryBuilder();

            // When.
            string response = builder.BuildItemWhereClause(new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("A",AttributeSearchOperator.ContainsAll,"Value1 Value2"),
                new ItemSearchCriteria("B",AttributeSearchOperator.ContainsAny,"Value3 Value4"),
                new ItemSearchCriteria("C",AttributeSearchOperator.ExactlyMatchesAll,"Value5 Value6"),
                new ItemSearchCriteria("D",AttributeSearchOperator.ExactlyMatchesAny,"Value7 Value8"),
            });

            // Then.
            Assert.AreEqual(@"(JSON_VALUE(ItemAttributesJson, '$.""A""') LIKE '%Value1%' AND JSON_VALUE(ItemAttributesJson, '$.""A""') LIKE '%Value2%') AND (JSON_VALUE(ItemAttributesJson, '$.""B""') LIKE '%Value3%' OR JSON_VALUE(ItemAttributesJson, '$.""B""') LIKE '%Value4%') AND (JSON_VALUE(ItemAttributesJson, '$.""C""') = 'Value5 Value6') AND (JSON_VALUE(ItemAttributesJson, '$.""D""') = 'Value7' OR JSON_VALUE(ItemAttributesJson, '$.""D""') = 'Value8')", response);
        }

        [TestMethod]
        public void BuildWhereClause_WithSpecialAttributesHierarchiesIgnoredAndResponseIsInCorrectFormat()
        {
            // Given.
            ItemQueryBuilder builder = new ItemQueryBuilder();

            // When.
            string response = builder.BuildItemWhereClause(new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("ItemId",AttributeSearchOperator.ContainsAll,"1"),
                new ItemSearchCriteria("ItemTypeDescription",AttributeSearchOperator.ContainsAll,"Value5 Value6"),
                new ItemSearchCriteria("ScanCode",AttributeSearchOperator.ContainsAll,"Value7 Value8"),
                new ItemSearchCriteria("BarcodeType",AttributeSearchOperator.ContainsAll,"Value9 Value10"),
                new ItemSearchCriteria("Brands",AttributeSearchOperator.ContainsAll,"Value11 Value12"),
                new ItemSearchCriteria("Merchandise",AttributeSearchOperator.ContainsAll,"Value13 Value14"),
                new ItemSearchCriteria("Tax",AttributeSearchOperator.ContainsAll,"Value15 Value16"),
                new ItemSearchCriteria("National",AttributeSearchOperator.ContainsAll,"Value17 Value18"),
                new ItemSearchCriteria("Financial",AttributeSearchOperator.ContainsAll,"Value19 Value20"),
                new ItemSearchCriteria("Manufacturer",AttributeSearchOperator.ContainsAll,"Value21 Value22"),
            });

            // Then.
            Assert.AreEqual("(i.ItemId LIKE '%1%') AND (it.ItemTypeDesc LIKE '%Value5%' AND it.ItemTypeDesc LIKE '%Value6%') AND (sc.ScanCode LIKE '%Value7%' AND sc.ScanCode LIKE '%Value8%') AND (bt.BarcodeType LIKE '%Value9%' AND bt.BarcodeType LIKE '%Value10%')", response);
        }

        [TestMethod]
        public void BuildHierarchyTempTableInclude_ResponseIsCorrectFormat()
        {
            // Given.
            ItemQueryBuilder builder = new ItemQueryBuilder();

            // When.
            string response = builder.BuildHierarchyTempTableInclude(new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("ItemId",AttributeSearchOperator.ContainsAll,"1"),
                new ItemSearchCriteria("ItemTypeDescription",AttributeSearchOperator.ContainsAll,"Value5 Value6"),
                new ItemSearchCriteria("ScanCode",AttributeSearchOperator.ContainsAll,"Value7 Value8"),
                new ItemSearchCriteria("BarcodeType",AttributeSearchOperator.ContainsAll,"Value9 Value10"),
                new ItemSearchCriteria("Brands",AttributeSearchOperator.ContainsAll,"Value11 Value12"),
                new ItemSearchCriteria("Merchandise",AttributeSearchOperator.ContainsAll,"Value13 Value14"),
                new ItemSearchCriteria("Tax",AttributeSearchOperator.ContainsAll,"Value15 Value16"),
                new ItemSearchCriteria("National",AttributeSearchOperator.ContainsAll,"Value17 Value18"),
                new ItemSearchCriteria("Financial",AttributeSearchOperator.ContainsAll,"Value19 Value20"),
                new ItemSearchCriteria("Manufacturer",AttributeSearchOperator.ContainsAll,"Value21 Value22"),
            });

            // Then.
            Assert.IsTrue(response.Contains("SELECT * INTO #Brands"));
            Assert.IsTrue(response.Contains("SELECT * INTO #Merchandise"));
            Assert.IsTrue(response.Contains("SELECT * INTO #Tax"));
            Assert.IsTrue(response.Contains("SELECT * INTO #National"));
            Assert.IsTrue(response.Contains("SELECT * INTO #Financial"));
            Assert.IsTrue(response.Contains("SELECT * INTO #Manufacturer"));
        }

        [TestMethod]
        public void BuildHierarchyTempTableInclude_ExactlyAnyFinancial_SurroundsWithParentheses()
        {
            // Given.
            ItemQueryBuilder builder = new ItemQueryBuilder();

            // When.
            string response = builder.BuildHierarchyTempTableInclude(new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("Financial",AttributeSearchOperator.ExactlyMatchesAny,"Value19 Value20"),
            });

            // Then.
            Assert.IsTrue(response.Contains("WHERE h.hierarchyName = 'Financial' AND (hc.hierarchyClassName = 'Value19' OR hc.hierarchyClassName = 'Value20')"),
                $"Failed with actual: {response}, expected to contain: WHERE h.hierarchyName = 'Financial' AND (hc.hierarchyClassName = 'Value19' OR hc.hierarchyClassName = 'Value20')");   
        }

        [TestMethod]
        public void BuildHierarchyTempTableInclude_FinancialExactlyMatchesAll_SurroundsWithParentheses()
        {
            // Given.
            ItemQueryBuilder builder = new ItemQueryBuilder();

            // When.
            string response = builder.BuildHierarchyTempTableInclude(new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("Financial",AttributeSearchOperator.ExactlyMatchesAll,"Value19 Value20"),
            });

            // Then.
            Assert.IsTrue(response.Contains("WHERE h.hierarchyName = 'Financial' AND (hc.hierarchyClassName = 'Value19 Value20')"),
                $"Failed with actual: {response}, expected to contain: WHERE h.hierarchyName = 'Financial' AND (hc.hierarchyClassName = 'Value19 Value20')");

        }

        [TestMethod]
        public void BuildHierarchyTempTableInclude_NationalAndMerchExactlyMatchesAny_SurroundsWithParentheses()
        {
            // Given.
            ItemQueryBuilder builder = new ItemQueryBuilder();

            // When.
            string response = builder.BuildHierarchyTempTableInclude(new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("National",AttributeSearchOperator.ExactlyMatchesAny,"Value19 Value20"),
                new ItemSearchCriteria("Merchandise",AttributeSearchOperator.ExactlyMatchesAny,"Value21 Value22")
            });

            // Then.
            Assert.IsTrue(response.Contains("cte_National WHERE (hierarchyLineage = 'Value19' OR hierarchyLineage = 'Value20')"),
                $"Failed with actual: {response}, expected to contain: WHERE (hierarchyLineage = 'Value19' OR hierarchyLineage = 'Value20')");
            Assert.IsTrue(response.Contains("cte_Merchandise WHERE (hierarchyLineage = 'Value21' OR hierarchyLineage = 'Value22')"),
                $"Failed with actual: {response}, expected to contain: cte_Merchandise WHERE (hierarchyLineage = 'Value21' OR hierarchyLineage = 'Value22')");

        }

        [TestMethod]
        public void BuildHierarchyTempTableInclude_NationalAndMerchExactlyMatchesAll_SurroundsWithParentheses()
        {
            // Given.
            ItemQueryBuilder builder = new ItemQueryBuilder();

            // When.
            string response = builder.BuildHierarchyTempTableInclude(new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("National",AttributeSearchOperator.ExactlyMatchesAll,"Value19 Value20"),
                new ItemSearchCriteria("Merchandise",AttributeSearchOperator.ExactlyMatchesAll,"Value21 Value22"),
            });

            // Then.
            Assert.IsTrue(response.Contains("cte_National WHERE (hierarchyLineage = 'Value19 Value20')"),
                $"Failed with actual: {response}, expected to contain: cte_National WHERE (hc.hierarchyLineage = 'Value19 Value20')");
            Assert.IsTrue(response.Contains("cte_Merchandise WHERE (hierarchyLineage = 'Value21 Value22')"),
                $"Failed with actual: {response}, expected to contain: cte_Merchandise WHERE (hc.hierarchyLineage = 'Value21 Value22')");

        }

        [TestMethod]
        public void BuildHierarchyJoinClause()
        {
            // Given.
            ItemQueryBuilder builder = new ItemQueryBuilder();

            // When.
            string response = builder.BuildHierarchyJoinClause(new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("ItemId",AttributeSearchOperator.ContainsAll,"1"),
                new ItemSearchCriteria("ItemTypeDescription",AttributeSearchOperator.ContainsAll,"Value5 Value6"),
                new ItemSearchCriteria("ScanCode",AttributeSearchOperator.ContainsAll,"Value7 Value8"),
                new ItemSearchCriteria("BarcodeType",AttributeSearchOperator.ContainsAll,"Value9 Value10"),
                new ItemSearchCriteria("Brands",AttributeSearchOperator.ContainsAll,"Value11 Value12"),
                new ItemSearchCriteria("Merchandise",AttributeSearchOperator.ContainsAll,"Value13 Value14"),
                new ItemSearchCriteria("Tax",AttributeSearchOperator.ContainsAll,"Value15 Value16"),
                new ItemSearchCriteria("National",AttributeSearchOperator.ContainsAll,"Value17 Value18"),
                new ItemSearchCriteria("Financial",AttributeSearchOperator.ContainsAll,"Value19 Value20"),
                new ItemSearchCriteria("Manufacturer",AttributeSearchOperator.ContainsAll,"Value21 Value22"),
            });

            // Then.
            Assert.IsTrue(response.Contains("JOIN #Brands on #Brands.itemId = i.ItemId"));
            Assert.IsTrue(response.Contains("JOIN #Merchandise on #Merchandise.itemId = i.ItemId"));
            Assert.IsTrue(response.Contains("JOIN #Tax on #Tax.itemId = i.ItemId"));
            Assert.IsTrue(response.Contains("JOIN #National on #National.itemId = i.ItemId"));
            Assert.IsTrue(response.Contains("JOIN #Financial on #Financial.itemId = i.ItemId"));
            Assert.IsTrue(response.Contains("JOIN #Manufacturer on #Manufacturer.itemId = i.ItemId"));
        }

        [TestMethod]
        public void BuildQuery_InactiveNotSupplied_InactiveCriteriaAdded()
        {
            // Given.
            ItemQueryBuilder builder = new ItemQueryBuilder();

            // When.
            string response = builder.BuildQuery(new DataAccess.Queries.GetItemsParameters()
            {
                ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
                {
                    new ItemSearchCriteria("ItemId", AttributeSearchOperator.ContainsAll, "1")
                }
            });

            // Then.
            Assert.IsTrue(response.Contains(@"Inactive = 'false'"));
        }

        [TestMethod]
        public void BuildQuery_InactiveSupplied_InactiveCriteriaNotAdded()
        {
            // Given.
            ItemQueryBuilder builder = new ItemQueryBuilder();

            // When.
            string response = builder.BuildQuery(new DataAccess.Queries.GetItemsParameters()
            {
                ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
                {
                    new ItemSearchCriteria("ItemId", AttributeSearchOperator.ContainsAll, "1"),
                    new ItemSearchCriteria("Inactive", AttributeSearchOperator.ContainsAll, "true")
                }
            });

            // Then.
            Assert.IsFalse(response.Contains(@"Inactive = 'false'"));
            Assert.IsTrue(response.Contains(@"Inactive LIKE '%true%'"));
        }

        [TestMethod]
        public void BuildSingleWhereClause_ResponseIsInCorrectFormat()
        {
            // Given.
            ItemQueryBuilder builder = new ItemQueryBuilder();

            // When.
            string response = builder.BuildSingleWhereClause(new ItemSearchCriteria("AttributeName", AttributeSearchOperator.ContainsAll, "test"));

            // Then.
            Assert.AreEqual(@"JSON_VALUE(ItemAttributesJson, '$.""AttributeName""') LIKE '%test%'", response);
        }

        [TestMethod]
        public void BuildSingleWhereClause_SqlInjection_QuotesAreEscaped()
        {
            // Given.
            ItemQueryBuilder builder = new ItemQueryBuilder();

            // When.
            string response = builder.BuildSingleWhereClause(new ItemSearchCriteria("AttributeName", AttributeSearchOperator.ContainsAll, "'%test%'"));

            // Then.
            Assert.AreEqual(@"JSON_VALUE(ItemAttributesJson, '$.""AttributeName""') LIKE '%''test''%'", response);
        }

        /// <summary>
        /// This test does not verify correctnes of results. Its purpose is to validate that our query will execute on SQL server.
        /// </summary>
        [TestMethod]
        public void TestAllPossibleCombinationsForOneValue_ShouldNotThrowException()
        {
            /* SQL to generate this test

            IF OBJECT_ID('TEMPDB..#attributes') IS NOT NULL
                DROP TABLE #attributes

            SELECT * INTO #attributes
            FROM (SELECT attributeName FROM dbo.Attributes) data;
            INSERT into #attributes values('ItemTypeDescription')
            INSERT into #attributes values('ScanCode')
            INSERT into #attributes values('BarcodeType')

            print 'new SearchCriteria("ItemId",AttributeSearchOperator.ContainsAll,"1"),'
            print 'new SearchCriteria("Brands",AttributeSearchOperator.ContainsAll,"ImpossibleToFindThisValue"),'
            print 'new SearchCriteria("Merchandise",AttributeSearchOperator.ContainsAll,"ImpossibleToFindThisValue"),'
            print 'new SearchCriteria("Tax",AttributeSearchOperator.ContainsAll,"ImpossibleToFindThisValue"),'
            print 'new SearchCriteria("National",AttributeSearchOperator.ContainsAll,"ImpossibleToFindThisValue"),'
            print 'new SearchCriteria("Financial",AttributeSearchOperator.ContainsAll,"ImpossibleToFindThisValue"),'
            print 'new SearchCriteria("Manufacturer",AttributeSearchOperator.ContainsAll,"ImpossibleToFindThisValue"),'

            DECLARE @attributeName varchar(255)
            DECLARE attributeCursor CURSOR FOR SELECT attributeName from #attributes
            OPEN attributeCursor
            FETCH NEXT FROM attributeCursor
            INTO @attributeName

            WHILE @@FETCH_STATUS = 0
            BEGIN

            print 'new SearchCriteria("' + @attributeName +'",AttributeSearchOperator.ContainsAll,"ImpossibleToFindThisValue"),'
            print 'new SearchCriteria("' + @attributeName +'",AttributeSearchOperator.ContainsAny,"ImpossibleToFindThisValue"),'
            print 'new SearchCriteria("' + @attributeName +'",AttributeSearchOperator.ExactlyMatchesAll,"ImpossibleToFindThisValue"),'
            print 'new SearchCriteria("' + @attributeName +'",AttributeSearchOperator.ExactlyMatchesAny,"ImpossibleToFindThisValue"),'

            FETCH NEXT FROM attributeCursor
            INTO @attributeName
            END
            CLOSE attributeCursor
            DEALLOCATE attributeCursor

            */

            // Given.
            ItemQueryBuilder builder = new ItemQueryBuilder();

            string response = builder.BuildQuery(new DataAccess.Queries.GetItemsParameters()
            {
                Skip = 0,
                Top = 1,
                ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
                {
                    new ItemSearchCriteria("ItemId", AttributeSearchOperator.ContainsAll, "1"),
                    new ItemSearchCriteria("Brands", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Merchandise", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Tax", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("National", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Financial", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Manufacturer", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("365Eligible", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("365Eligible", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("365Eligible", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("365Eligible", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ABF", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ABF", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ABF", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ABF", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Accessory", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Accessory", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Accessory", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Accessory", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("AgeGender", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("AgeGender", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("AgeGender", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("AgeGender", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("AirChilled", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("AirChilled", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("AirChilled", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("AirChilled", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("AlcoholByVolume", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("AlcoholByVolume", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("AlcoholByVolume", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("AlcoholByVolume", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Allocated", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Allocated", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Allocated", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Allocated", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("AnimalWelfareRating", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("AnimalWelfareRating", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("AnimalWelfareRating", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("AnimalWelfareRating", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Appellation", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Appellation", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Appellation", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Appellation", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("BeerStyle", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("BeerStyle", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("BeerStyle", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("BeerStyle", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Biodynamic", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Biodynamic", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Biodynamic", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Biodynamic", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("BlackhawkCommissionDollar", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("BlackhawkCommissionDollar", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("BlackhawkCommissionDollar", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("BlackhawkCommissionDollar", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("BlackhawkCommissionPercent", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("BlackhawkCommissionPercent", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("BlackhawkCommissionPercent", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("BlackhawkCommissionPercent", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CaseinFree", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CaseinFree", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CaseinFree", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CaseinFree", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CategoryManagement", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CategoryManagement", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CategoryManagement", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CategoryManagement", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CheeseAttributeMilkType", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CheeseAttributeMilkType", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CheeseAttributeMilkType", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CheeseAttributeMilkType", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CustomerFriendlyDescription", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CustomerFriendlyDescription", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CustomerFriendlyDescription", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CustomerFriendlyDescription", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CountryofOrigin", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CountryofOrigin", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CountryofOrigin", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CountryofOrigin", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Cube", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Cube", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Cube", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Cube", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DataSource", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DataSource", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DataSource", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DataSource", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DeliverySystem", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DeliverySystem", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DeliverySystem", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DeliverySystem", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DimensionsDataSource", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DimensionsDataSource", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DimensionsDataSource", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DimensionsDataSource", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Disposable", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Disposable", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Disposable", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Disposable", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DrainedWeight", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DrainedWeight", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DrainedWeight", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DrainedWeight", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DrainedWeightUOM", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DrainedWeightUOM", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DrainedWeightUOM", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DrainedWeightUOM", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DryAged", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DryAged", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DryAged", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DryAged", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("EcoScaleRating", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("EcoScaleRating", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("EcoScaleRating", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("EcoScaleRating", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("EStoreEligible", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("EStoreEligible", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("EStoreEligible", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("EStoreEligible", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("EStoreNutritionRequired", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("EStoreNutritionRequired", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("EStoreNutritionRequired", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("EStoreNutritionRequired", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ExclusiveDate", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ExclusiveDate", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ExclusiveDate", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ExclusiveDate", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ExclusiveYesNo", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ExclusiveYesNo", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ExclusiveYesNo", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ExclusiveYesNo", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FairTradeCertified", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FairTradeCertified", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FairTradeCertified", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FairTradeCertified", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FairTradeClaim", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FairTradeClaim", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FairTradeClaim", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FairTradeClaim", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FatFreeClaim", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FatFreeClaim", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FatFreeClaim", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FatFreeClaim", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FlexSignText", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FlexSignText", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FlexSignText", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FlexSignText", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FoodStampEligible", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FoodStampEligible", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FoodStampEligible", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FoodStampEligible", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FragranceFree", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FragranceFree", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FragranceFree", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FragranceFree", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FreeRange", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FreeRange", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FreeRange", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FreeRange", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FreshorFrozen", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FreshorFrozen", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FreshorFrozen", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("FreshorFrozen", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GlobalPricingProgram", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GlobalPricingProgram", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GlobalPricingProgram", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GlobalPricingProgram", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GlutenFree", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GlutenFree", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GlutenFree", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GlutenFree", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GlutenFreeClaim", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GlutenFreeClaim", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GlutenFreeClaim", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GlutenFreeClaim", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GMOTransparency", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GMOTransparency", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GMOTransparency", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GMOTransparency", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GoodBetterBest", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GoodBetterBest", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GoodBetterBest", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GoodBetterBest", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GrassFed", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GrassFed", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GrassFed", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("GrassFed", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Halal", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Halal", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Halal", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Halal", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Hemp", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Hemp", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Hemp", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Hemp", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Homeopathic", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Homeopathic", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Homeopathic", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Homeopathic", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("HormoneFreeClaim", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("HormoneFreeClaim", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("HormoneFreeClaim", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("HormoneFreeClaim", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("HospitalityItem", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("HospitalityItem", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("HospitalityItem", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("HospitalityItem", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ImageMap", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ImageMap", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ImageMap", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ImageMap", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemDepth", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemDepth", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemDepth", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemDepth", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemHeight", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemHeight", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemHeight", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemHeight", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemPack", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemPack", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemPack", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemPack", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemWeight", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemWeight", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemWeight", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemWeight", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemWidth", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemWidth", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemWidth", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemWidth", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("IXOneBrand", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("IXOneBrand", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("IXOneBrand", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("IXOneBrand", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("JuiceContent", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("JuiceContent", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("JuiceContent", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("JuiceContent", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("KitchenDescription", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("KitchenDescription", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("KitchenDescription", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("KitchenDescription", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("KitchenItem", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("KitchenItem", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("KitchenItem", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("KitchenItem", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Kosher", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Kosher", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Kosher", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Kosher", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("KosherClaim", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("KosherClaim", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("KosherClaim", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("KosherClaim", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Labeling", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Labeling", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Labeling", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Labeling", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Line", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Line", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Line", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Line", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("LineExtension", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("LineExtension", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("LineExtension", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("LineExtension", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("LocalLoanProducer", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("LocalLoanProducer", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("LocalLoanProducer", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("LocalLoanProducer", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("LowFatClaim", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("LowFatClaim", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("LowFatClaim", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("LowFatClaim", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("MadeInHouse", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("MadeInHouse", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("MadeInHouse", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("MadeInHouse", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("MadewithOrganicGrapes", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("MadewithOrganicGrapes", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("MadewithOrganicGrapes", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("MadewithOrganicGrapes", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("MSC", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("MSC", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("MSC", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("MSC", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Multipurpose", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Multipurpose", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Multipurpose", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Multipurpose", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("NaturalClaim", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("NaturalClaim", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("NaturalClaim", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("NaturalClaim", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("NonGMO", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("NonGMO", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("NonGMO", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("NonGMO", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("NonGMOClaim", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("NonGMOClaim", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("NonGMOClaim", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("NonGMOClaim", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("NoSulfites", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("NoSulfites", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("NoSulfites", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("NoSulfites", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Notes", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Notes", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Notes", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Notes", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("NutritionRequired", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("NutritionRequired", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("NutritionRequired", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("NutritionRequired", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Organic", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Organic", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Organic", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Organic", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("OrganicClaim", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("OrganicClaim", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("OrganicClaim", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("OrganicClaim", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("OrganicPersonalCare", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("OrganicPersonalCare", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("OrganicPersonalCare", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("OrganicPersonalCare", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Other3PEligible", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Other3PEligible", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Other3PEligible", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Other3PEligible", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("OwnershipBrandLevel", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("OwnershipBrandLevel", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("OwnershipBrandLevel", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("OwnershipBrandLevel", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PackageGroup", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PackageGroup", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PackageGroup", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PackageGroup", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PackageGroupType", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PackageGroupType", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PackageGroupType", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PackageGroupType", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PackagingType", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PackagingType", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PackagingType", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PackagingType", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Paleo", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Paleo", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Paleo", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Paleo", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PaleoClaim", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PaleoClaim", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PaleoClaim", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PaleoClaim", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PastureRaised", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PastureRaised", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PastureRaised", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PastureRaised", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PMDVerified", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PMDVerified", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PMDVerified", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PMDVerified", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("POSDescription", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("POSDescription", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("POSDescription", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("POSDescription", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("POSScaleTare", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("POSScaleTare", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("POSScaleTare", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("POSScaleTare", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PremiumBodyCare", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PremiumBodyCare", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PremiumBodyCare", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PremiumBodyCare", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PriceLine", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PriceLine", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PriceLine", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PriceLine", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Prime", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Prime", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Prime", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Prime", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PrimeNowEligible", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PrimeNowEligible", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PrimeNowEligible", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PrimeNowEligible", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PrivateLabel", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PrivateLabel", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PrivateLabel", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PrivateLabel", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ProductDescription", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ProductDescription", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ProductDescription", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ProductDescription", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ProductFlavororType", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ProductFlavororType", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ProductFlavororType", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ProductFlavororType", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ProhibitDiscount", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ProhibitDiscount", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ProhibitDiscount", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ProhibitDiscount", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RainforestAlliance", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RainforestAlliance", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RainforestAlliance", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RainforestAlliance", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Raw", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Raw", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Raw", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Raw", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RefrigeratedorShelfStable", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RefrigeratedorShelfStable", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RefrigeratedorShelfStable", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RefrigeratedorShelfStable", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RegionalLocalItem", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RegionalLocalItem", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RegionalLocalItem", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RegionalLocalItem", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Rennet", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Rennet", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Rennet", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Rennet", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RetailSize", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RetailSize", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RetailSize", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RetailSize", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SCOItemTareGroup", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SCOItemTareGroup", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SCOItemTareGroup", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SCOItemTareGroup", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SeafoodWildOrFarmRaised", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SeafoodWildOrFarmRaised", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SeafoodWildOrFarmRaised", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SeafoodWildOrFarmRaised", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SeasonalInandOutGifting", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SeasonalInandOutGifting", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SeasonalInandOutGifting", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SeasonalInandOutGifting", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ShelfLife", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ShelfLife", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ShelfLife", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ShelfLife", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SkinType", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SkinType", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SkinType", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SkinType", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SKU", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SKU", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SKU", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SKU", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SmithsonianBirdFriendly", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SmithsonianBirdFriendly", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SmithsonianBirdFriendly", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SmithsonianBirdFriendly", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Smoked", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Smoked", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Smoked", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Smoked", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SoldHotorCold", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SoldHotorCold", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SoldHotorCold", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SoldHotorCold", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TravelSizeSingleUseKit", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TravelSizeSingleUseKit", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TravelSizeSingleUseKit", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TravelSizeSingleUseKit", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TrayDepth", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TrayDepth", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TrayDepth", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TrayDepth", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TrayHeight", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TrayHeight", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TrayHeight", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TrayHeight", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TrayWidth", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TrayWidth", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TrayWidth", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TrayWidth", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("UOM", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("UOM", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("UOM", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("UOM", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("URL1", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("URL1", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("URL1", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("URL1", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ValueAdded", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ValueAdded", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ValueAdded", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ValueAdded", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VariantSize", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VariantSize", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VariantSize", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VariantSize", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Varietal", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Varietal", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Varietal", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Varietal", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Vegan", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Vegan", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Vegan", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Vegan", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VeganClaim", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VeganClaim", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VeganClaim", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VeganClaim", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Vegetarian", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Vegetarian", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Vegetarian", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Vegetarian", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VegetarianClaim", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VegetarianClaim", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VegetarianClaim", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VegetarianClaim", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("WFMEligible", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("WFMEligible", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("WFMEligible", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("WFMEligible", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("WholeTrade", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("WholeTrade", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("WholeTrade", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("WholeTrade", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("HSH", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("HSH", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("HSH", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("HSH", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RecipeName", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RecipeName", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RecipeName", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RecipeName", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Allergens", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Allergens", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Allergens", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Allergens", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Ingredients", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Ingredients", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Ingredients", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Ingredients", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Selenium", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Selenium", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Selenium", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Selenium", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PolyunsaturatedFat", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PolyunsaturatedFat", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PolyunsaturatedFat", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PolyunsaturatedFat", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("MonounsaturatedFat", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("MonounsaturatedFat", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("MonounsaturatedFat", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("MonounsaturatedFat", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PotassiumWeight", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PotassiumWeight", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PotassiumWeight", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PotassiumWeight", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PotassiumPercent", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PotassiumPercent", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PotassiumPercent", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PotassiumPercent", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DietaryFiberPercent", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DietaryFiberPercent", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DietaryFiberPercent", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DietaryFiberPercent", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SolubleFiber", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SolubleFiber", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SolubleFiber", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SolubleFiber", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("InsolubleFiber", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("InsolubleFiber", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("InsolubleFiber", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("InsolubleFiber", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SugarAlcohol", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SugarAlcohol", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SugarAlcohol", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SugarAlcohol", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("OtherCarbohydrates", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("OtherCarbohydrates", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("OtherCarbohydrates", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("OtherCarbohydrates", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ProteinPercent", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ProteinPercent", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ProteinPercent", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ProteinPercent", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Betacarotene", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Betacarotene", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Betacarotene", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Betacarotene", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VitaminD", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VitaminD", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VitaminD", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VitaminD", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VitaminE", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VitaminE", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VitaminE", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VitaminE", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Thiamin", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Thiamin", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Thiamin", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Thiamin", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Riboflavin", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Riboflavin", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Riboflavin", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Riboflavin", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Niacin", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Niacin", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Niacin", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Niacin", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VitaminB6", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VitaminB6", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VitaminB6", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VitaminB6", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Folate", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Folate", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Folate", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Folate", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VitaminB12", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VitaminB12", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VitaminB12", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VitaminB12", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Biotin", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Biotin", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Biotin", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Biotin", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PantothenicAcid", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PantothenicAcid", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PantothenicAcid", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PantothenicAcid", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Phosphorous", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Phosphorous", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Phosphorous", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Phosphorous", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Iodine", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Iodine", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Iodine", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Iodine", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Magnesium", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Magnesium", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Magnesium", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Magnesium", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Zinc", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Zinc", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Zinc", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Zinc", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Copper", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Copper", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Copper", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Copper", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Transfat", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Transfat", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Transfat", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Transfat", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Om6Fatty", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Om6Fatty", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Om6Fatty", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Om6Fatty", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Om3Fatty", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Om3Fatty", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Om3Fatty", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Om3Fatty", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Starch", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Starch", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Starch", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Starch", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Chloride", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Chloride", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Chloride", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Chloride", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Chromium", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Chromium", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Chromium", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Chromium", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VitaminK", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VitaminK", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VitaminK", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("VitaminK", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Manganese", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Manganese", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Manganese", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Manganese", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Molybdenum", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Molybdenum", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Molybdenum", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Molybdenum", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CaloriesFromTransFat", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CaloriesFromTransFat", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CaloriesFromTransFat", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CaloriesFromTransFat", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CaloriesSaturatedFat", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CaloriesSaturatedFat", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CaloriesSaturatedFat", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CaloriesSaturatedFat", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ServingPerContainer", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ServingPerContainer", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ServingPerContainer", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ServingPerContainer", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ServingSizeDesc", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ServingSizeDesc", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ServingSizeDesc", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ServingSizeDesc", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ServingsPerPortion", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ServingsPerPortion", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ServingsPerPortion", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ServingsPerPortion", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ServingUnits", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ServingUnits", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ServingUnits", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ServingUnits", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SizeWeight", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SizeWeight", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SizeWeight", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SizeWeight", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TransfatWeight", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TransfatWeight", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TransfatWeight", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TransfatWeight", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Inactive", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Inactive", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Inactive", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("Inactive", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CreatedBy", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CreatedBy", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CreatedBy", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CreatedBy", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CreatedDateTimeUtc", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CreatedDateTimeUtc", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CreatedDateTimeUtc", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CreatedDateTimeUtc", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ModifiedBy", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ModifiedBy", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ModifiedBy", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ModifiedBy", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ModifiedDateTimeUtc", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ModifiedDateTimeUtc", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ModifiedDateTimeUtc", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ModifiedDateTimeUtc", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DepartmentSale", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DepartmentSale", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DepartmentSale", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DepartmentSale", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TEST", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TEST", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TEST", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TEST", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TEST2", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TEST2", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TEST2", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TEST2", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ExpiryDate", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ExpiryDate", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ExpiryDate", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ExpiryDate", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("APPROVEDQUANTITY", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("APPROVEDQUANTITY", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("APPROVEDQUANTITY", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("APPROVEDQUANTITY", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SOILNUTRIENTS", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SOILNUTRIENTS", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SOILNUTRIENTS", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("SOILNUTRIENTS", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PLANTTYPE", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PLANTTYPE", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PLANTTYPE", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("PLANTTYPE", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RadhocPickList", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RadhocPickList", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RadhocPickList", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RadhocPickList", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DonutGlazeType", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DonutGlazeType", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DonutGlazeType", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("DonutGlazeType", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RAD03", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RAD03", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RAD03", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RAD03", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RAD06", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RAD06", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RAD06", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RAD06", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RAD07", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RAD07", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RAD07", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RAD07", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RAD08", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RAD08", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RAD08", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("RAD08", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ZAD01", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ZAD01", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ZAD01", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ZAD01", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ZAD02", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ZAD02", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ZAD02", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ZAD02", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TestTextAttribute", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TestTextAttribute", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TestTextAttribute", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TestTextAttribute", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TestTextAttribute2", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TestTextAttribute2", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TestTextAttribute2", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TestTextAttribute2", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TestTextAttribute3", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TestTextAttribute3", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TestTextAttribute3", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TestTextAttribute3", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TestTextAttribute4", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TestTextAttribute4", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TestTextAttribute4", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("TestTextAttribute4", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CoffeeRoastType", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CoffeeRoastType", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CoffeeRoastType", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("CoffeeRoastType", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("testspecificspecial", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("testspecificspecial", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("testspecificspecial", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("testspecificspecial", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("testspecific2", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("testspecific2", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("testspecific2", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("testspecific2", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemTypeDescription", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemTypeDescription", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemTypeDescription", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ItemTypeDescription", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ScanCode", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ScanCode", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ScanCode", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("ScanCode", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("BarcodeType", AttributeSearchOperator.ContainsAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("BarcodeType", AttributeSearchOperator.ContainsAny, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("BarcodeType", AttributeSearchOperator.ExactlyMatchesAll, "ImpossibleToFindThisValue"),
                    new ItemSearchCriteria("BarcodeType", AttributeSearchOperator.ExactlyMatchesAny, "ImpossibleToFindThisValue")
                }
            });

            TransactionScope transaction = new TransactionScope();
            SqlConnection connection = SqlConnectionBuilder.CreateIconConnection();

            connection.Open();
            var command = new SqlCommand(response, connection);
            command.ExecuteNonQuery();
        }
    }
}