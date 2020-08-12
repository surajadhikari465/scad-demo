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
    }       
}