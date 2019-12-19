using Icon.Web.DataAccess.Infrastructure.ItemSearch;
using Icon.Web.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Icon.Web.DataAccess.Infrastructure
{
    /// <summary>
    /// This class is responsible for building the dynamic item search query.
    /// </summary>
    public class ItemQueryBuilder : IItemQueryBuilder
    {
        /// <summary>
        /// If a query includes filtering by hierarchy lineage we dynamically add a query to search for items
        /// that are matched by the query into a temp table. Then we add an inner join on the temp table to the item query to filter records.
        /// We do this because it's a lot faster than trying to build all of the lineages and filter in the main item query. 
        /// </summary>
        private static Dictionary<string, string> HierarchySelectQueries = new Dictionary<string, string>()
        {
            {"Merchandise", @"

            ;WITH cte_Merchandise(itemId,hierarchyLineage) as
            (

	            SELECT
			            ihc.itemID
			            ,hclv1.hierarchyClassName + ' | ' + hclv2.hierarchyClassName + ' | ' + hclv3.hierarchyClassName + ' | ' + hclv4.hierarchyClassName + ' | ' + hclv5.hierarchyClassName + ': ' + #MFM.hierarchyClassName AS HierarchyLineage
		                FROM dbo.ItemHierarchyClass ihc
		                JOIN dbo.HierarchyClass hclv5 on hclv5.hierarchyClassID = ihc.hierarchyClassID
		                JOIN dbo.HierarchyClass hclv4 ON hclv5.hierarchyParentClassID = hclv4.hierarchyClassID
		                JOIN dbo.HierarchyClass hclv3 ON hclv4.hierarchyParentClassID = hclv3.hierarchyClassID
		                JOIN dbo.HierarchyClass hclv2 ON hclv3.hierarchyParentClassID = hclv2.hierarchyClassID
		                JOIN dbo.HierarchyClass hclv1 ON hclv2.hierarchyParentClassID = hclv1.hierarchyClassID
		                JOIN #MFM ON #MFM.hierarchyClassID = hclv5.hierarchyClassID
		                JOIN dbo.Hierarchy h ON hclv5.hierarchyID = h.hierarchyID
		                WHERE h.hierarchyName = 'Merchandise'

            )

             SELECT * INTO #Merchandise FROM cte_Merchandise WHERE {{whereClause}}"},

            {"Brands", @"SELECT * INTO #Brands
            FROM
            (
		                SELECT
			                ihc.itemID
		                FROM dbo.ItemHierarchyClass ihc
		                JOIN dbo.HierarchyClass hc ON ihc.hierarchyClassID = hc.hierarchyClassID
		                JOIN dbo.Hierarchy h ON hc.hierarchyID = h.hierarchyID
		                WHERE h.hierarchyName = 'Brands' AND {{whereClause}}

            ) data;" },

            {"Tax", @"SELECT * INTO #Tax
            FROM
            (
		                SELECT
			                ihc.itemID
		                FROM dbo.ItemHierarchyClass ihc
		                JOIN dbo.HierarchyClass hc ON ihc.hierarchyClassID = hc.hierarchyClassID
		                JOIN dbo.Hierarchy h ON hc.hierarchyID = h.hierarchyID
		                WHERE h.hierarchyName = 'Tax'  AND {{whereClause}}

            ) data;" },

            {"Financial", @"SELECT * INTO #Financial
            FROM
            (
		                SELECT
			                ihc.itemID
		                FROM dbo.ItemHierarchyClass ihc
		                JOIN dbo.HierarchyClass hc ON ihc.hierarchyClassID = hc.hierarchyClassID
		                JOIN dbo.Hierarchy h ON hc.hierarchyID = h.hierarchyID
		                WHERE h.hierarchyName = 'Financial' AND {{whereClause}}

            ) data;" },

            { "National",  @";WITH cte_National(itemId,hierarchyLineage) as
            (
            SELECT
			    ihc.itemID
			    ,hclv1.hierarchyClassName + ' | ' + hclv2.hierarchyClassName + ' | ' + hclv3.hierarchyClassName + ' | ' + hclv4.hierarchyClassName + ': ' + hct.traitValue AS HierarchyLineage
		    FROM dbo.ItemHierarchyClass ihc
		    JOIN dbo.HierarchyClass hclv4 ON hclv4.hierarchyClassID = ihc.hierarchyClassID
		    JOIN dbo.HierarchyClass hclv3 ON hclv4.hierarchyParentClassID = hclv3.hierarchyClassID
		    JOIN dbo.HierarchyClass hclv2 ON hclv3.hierarchyParentClassID = hclv2.hierarchyClassID
		    JOIN dbo.HierarchyClass hclv1 ON hclv2.hierarchyParentClassID = hclv1.hierarchyClassID
		    JOIN #NCC hct ON hclv4.hierarchyClassID = hct.hierarchyClassID
		    JOIN dbo.Hierarchy h ON hclv4.HIERARCHYID = h.HIERARCHYID
		    WHERE h.hierarchyName = 'National'
            )
            SELECT * INTO #National FROM cte_National WHERE {{whereClause}}" },

            { "Manufacturer", @"SELECT * INTO #Manufacturer
            FROM
            (
		                SELECT
			                ihc.itemID
		                FROM dbo.ItemHierarchyClass ihc
		                JOIN dbo.HierarchyClass hc ON ihc.hierarchyClassID = hc.hierarchyClassID
		                JOIN dbo.Hierarchy h ON hc.hierarchyID = h.hierarchyID
		                WHERE h.hierarchyName = 'Manufacturer' AND  {{whereClause}}

            ) data;" }
        };

        private static Dictionary<string, string> SpecialSearchAttributes = new Dictionary<string, string>()
        {
             {"ItemId","i.ItemId" },
             {"ItemTypeDescription","it.ItemTypeDesc" },
             {"ScanCode","sc.ScanCode" },
             {"BarcodeType","bt.BarcodeType" },
             {"Brands","hc.hierarchyClassName" },
             {"Merchandise","hierarchyLineage" },
             {"Tax","hc.hierarchyClassName" },
             {"National","hierarchyLineage" },
             {"Financial", "hc.hierarchyClassName" },
             {"Manufacturer","hc.hierarchyClassName" }
        };

        private static Dictionary<string, string> SpecialOrderByAttributes = new Dictionary<string, string>()
        {
             {"ItemId","ItemId" },
             {"ItemTypeDescription","ItemTypeDescription" },
             {"ScanCode","ScanCode" },
             {"BarcodeType","BarcodeType" },
             {"BrandsHierarchyClassId","Brands" },
             {"MerchandiseHierarchyClassId","Merchandise" },
             {"TaxHierarchyClassId","Tax" },
             {"NationalHierarchyClassId","NationalClass" },
             {"FinancialHierarchyClassId","Financial" },
             {"ManufacturerHierarchyClassId","Manufacturer" }
        };

        public bool IsAttributeHierarchy(string attributeName)
        {
            return attributeName == "Brands" ||
                attributeName == "Merchandise" ||
                attributeName == "Tax" ||
                attributeName == "National" ||
                attributeName == "Financial" ||
                attributeName == "Manufacturer";
        }

        /// <summary>
        /// If you touch this function you must compare the execution plan in Sentry before and after. You have to check if searching by item attributes 
        /// and by hierarchies still performs well.
        /// This function is the main function where the query is constructed.
        /// There are tmp tables(#MFM,#NCC) for finding the Merch/Financial mapping and the National class code.
        /// Because traitValues are strings and hierarchyClassId is an int it's faster to build these temp tables and convert the data
        /// than to add them as a join or subquery.
        /// This query contains APPLY operators that allow the derived tables to filter on itemId. There is a slight performance gain from
        /// using APPLY here but JOIN would work as well. 
        /// Results are sent to a temp table so that we can get the record count without executing the query again. This helps with performance.
        /// We fiter on Item.Inactive at the very end. That column does not index well and I found it's a better to just filter at the end.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string BuildQuery(GetItemsParameters parameters)
        {
            string itemWhereClause = $"{this.BuildItemWhereClause(parameters.ItemAttributeJsonParameters)}";
            string hierarchyJoinClause = this.BuildHierarchyJoinClause(parameters.ItemAttributeJsonParameters);
            string hierarchyTempTableInclude = this.BuildHierarchyTempTableInclude(parameters.ItemAttributeJsonParameters);
            string select = $@"
IF OBJECT_ID('TEMPDB..#MFM') IS NOT NULL
    DROP TABLE #MFM
IF OBJECT_ID('TEMPDB..#NCC') IS NOT NULL
    DROP TABLE #NCC
IF OBJECT_ID('TEMPDB..#results') IS NOT NULL
    DROP TABLE #results
IF OBJECT_ID('TEMPDB..#brands') IS NOT NULL
    DROP TABLE #brands
IF OBJECT_ID('TEMPDB..#merchandise') IS NOT NULL
    DROP TABLE #merchandise
IF OBJECT_ID('TEMPDB..#tax') IS NOT NULL
    DROP TABLE #tax
IF OBJECT_ID('TEMPDB..#financial') IS NOT NULL
    DROP TABLE #financial
IF OBJECT_ID('TEMPDB..#national') IS NOT NULL
    DROP TABLE #national
IF OBJECT_ID('TEMPDB..#manufacturer') IS NOT NULL
    DROP TABLE #manufacturer

DECLARE @mfmTraitId int;
set @mfmTraitId=(SELECT traitID
							FROM dbo.Trait
							WHERE TraitCode = 'MFM')

DECLARE @nccTraitId int;
set @nccTraitId=(SELECT traitID
							FROM dbo.Trait
							WHERE TraitCode = 'NCC')

SELECT * into #MFM
FROM
    (SELECT hct.hierarchyClassId,
    CAST(hct.traitValue as INT) as traitValue, 
	hc.hierarchyClassName
    FROM dbo.hierarchyClassTrait hct
	JOIN dbo.HierarchyClass hc on hc.hierarchyClassId = traitValue
    WHERE hct.traitId=@mfmTraitId
) data;
CREATE UNIQUE NONCLUSTERED	INDEX ix_#MFM ON #MFM (hierarchyClassId);

SELECT * into #NCC
FROM
(
    SELECT hct.hierarchyClassId,
    hct.traitValue as traitValue
    FROM dbo.hierarchyClassTrait hct
    WHERE hct.traitId=@nccTraitId
) data;
CREATE UNIQUE NONCLUSTERED	INDEX ix_#NCC ON #NCC (hierarchyClassId);

{hierarchyTempTableInclude}

SELECT * into #results
FROM
(
    SELECT
	    i.ItemId ItemId,
	    i.ItemTypeId AS ItemTypeId,
	    i.ItemAttributesJson AS ItemAttributesJson,
	    i.Inactive as Inactive,
	    it.ItemTypeCode as ItemTypeCode,
	    it.ItemTypeDesc as ItemTypeDescription,
	    sc.scanCode AS ScanCode,
	    sc.scanCodeTypeID AS ScanCodeTypeId,
	    sc.BarcodeTypeId AS BarcodeTypeId,
	    sct.scanCodeTypeDesc as ScanCodeTypeDescription,
	    bt.BarcodeType as BarcodeType,
	    brandsHierarchy.hierarchyClassID AS BrandsHierarchyClassId,
	    merchandiseHierarchy.hierarchyClassID AS MerchandiseHierarchyClassId,
	    taxHierarchy.hierarchyClassID AS TaxHierarchyClassId,
	    financialHierarchy.hierarchyClassID AS FinancialHierarchyClassId,
	    nationalHierarchy.hierarchyClassID AS NationalHierarchyClassId,
	    manufacturerHierarchy.hierarchyClassID AS ManufacturerHierarchyClassId,
	    brandsHierarchy.hierarchyLineage as Brands,
	    merchandiseHierarchy.HierarchyLineage as Merchandise,
	    taxHierarchy.HierarchyLineage as Tax,
	    nationalHierarchy.HierarchyLineage as NationalClass,
	    financialHierarchy.HierarchyLineage as Financial,
	    manufacturerHierarchy.HierarchyLineage as Manufacturer

    FROM dbo.Item i
    JOIN dbo.ScanCode sc on i.ItemId = sc.itemID
    JOIN dbo.ScanCodeType sct on sct.scanCodeTypeID = sc.scanCodeTypeID
    JOIN dbo.BarcodeType bt on bt.BarcodeTypeId = sc.BarcodeTypeId
    JOIN dbo.ItemType it on it.itemTypeID = i.ItemTypeId
	{hierarchyJoinClause}
    CROSS APPLY (
		    SELECT
                TOP 1
			    ihc.itemID
			    ,hclv5.HIERARCHYID AS HIERARCHYID
			    ,hclv5.hierarchyClassID AS HierarchyClassId
			    ,hclv5.hierarchyClassName AS HierarchyClassName
			    ,hclv5.hierarchyLevel AS HierarchyLevel
			    ,hclv1.hierarchyClassName + ' | ' + hclv2.hierarchyClassName + ' | ' + hclv3.hierarchyClassName + ' | ' + hclv4.hierarchyClassName + ' | ' + hclv5.hierarchyClassName + ': ' + #MFM.hierarchyClassName AS HierarchyLineage
			    ,hclv2.hierarchyParentClassID AS HierarchyParentClassId
		    FROM dbo.ItemHierarchyClass ihc
		    JOIN dbo.HierarchyClass hclv5 on hclv5.hierarchyClassID = ihc.hierarchyClassID
		    JOIN dbo.HierarchyClass hclv4 ON hclv5.hierarchyParentClassID = hclv4.hierarchyClassID
		    JOIN dbo.HierarchyClass hclv3 ON hclv4.hierarchyParentClassID = hclv3.hierarchyClassID
		    JOIN dbo.HierarchyClass hclv2 ON hclv3.hierarchyParentClassID = hclv2.hierarchyClassID
		    JOIN dbo.HierarchyClass hclv1 ON hclv2.hierarchyParentClassID = hclv1.hierarchyClassID
		    JOIN #MFM ON #MFM.hierarchyClassID = hclv5.hierarchyClassID
		    JOIN dbo.Hierarchy h ON hclv5.hierarchyID = h.hierarchyID
		    WHERE h.hierarchyName = 'Merchandise' AND ihc.itemId = i.ItemId
	    ) merchandiseHierarchy
    CROSS APPLY  (
		    SELECT
                TOP 1
			    ihc.itemID,
			    ihc.hierarchyClassId,
			    hc.hierarchyClassName,
			    hc.hierarchyClassName AS HierarchyLineage
		    FROM dbo.ItemHierarchyClass ihc
		    JOIN dbo.HierarchyClass hc ON ihc.hierarchyClassID = hc.hierarchyClassID
		    JOIN dbo.Hierarchy h ON hc.hierarchyID = h.hierarchyID
		    WHERE h.hierarchyName = 'Brands' AND ihc.itemId = i.ItemId
	    ) brandsHierarchy
    CROSS APPLY  (
		    SELECT
                TOP 1
			    ihc.itemID,
			    ihc.hierarchyClassId,
			    hc.hierarchyClassName,
			    hc.hierarchyClassName AS HierarchyLineage
		    FROM dbo.ItemHierarchyClass ihc
		    JOIN dbo.HierarchyClass hc ON ihc.hierarchyClassID = hc.hierarchyClassID
		    JOIN dbo.Hierarchy h ON hc.hierarchyID = h.hierarchyID
		    WHERE h.hierarchyName = 'Tax' AND ihc.itemId = i.ItemId
	    ) taxHierarchy
    CROSS APPLY  (
		    SELECT
			    TOP 1
                ihc.itemID,
			    ihc.hierarchyClassId,
			    hc.hierarchyClassName,
			    hc.hierarchyClassName AS HierarchyLineage
		    FROM dbo.ItemHierarchyClass ihc
		    JOIN dbo.HierarchyClass hc ON ihc.hierarchyClassID = hc.hierarchyClassID
		    JOIN dbo.Hierarchy h ON hc.hierarchyID = h.hierarchyID
		    WHERE h.hierarchyName = 'Financial' AND ihc.itemId = i.ItemId
	    ) financialHierarchy
    CROSS APPLY  (
		    SELECT
                TOP 1
			    ihc.itemID
			    ,ihc.hierarchyClassId
			    ,hclv4.HIERARCHYID AS HIERARCHYID
			    ,hclv4.hierarchyClassName AS HierarchyClassName
			    ,hclv4.hierarchyLevel AS HierarchyLevel
			    ,hclv1.hierarchyClassName + ' | ' + hclv2.hierarchyClassName + ' | ' + hclv3.hierarchyClassName + ' | ' + hclv4.hierarchyClassName + ': ' + hct.traitValue AS HierarchyLineage
			    ,hclv4.hierarchyParentClassID AS HierarchyParentClassId
		    FROM dbo.ItemHierarchyClass ihc
		    JOIN dbo.HierarchyClass hclv4 ON hclv4.hierarchyClassID = ihc.hierarchyClassID
		    JOIN dbo.HierarchyClass hclv3 ON hclv4.hierarchyParentClassID = hclv3.hierarchyClassID
		    JOIN dbo.HierarchyClass hclv2 ON hclv3.hierarchyParentClassID = hclv2.hierarchyClassID
		    JOIN dbo.HierarchyClass hclv1 ON hclv2.hierarchyParentClassID = hclv1.hierarchyClassID
		    JOIN #NCC hct ON hclv4.hierarchyClassID = hct.hierarchyClassID
		    JOIN dbo.Hierarchy h ON hclv4.HIERARCHYID = h.HIERARCHYID
		    WHERE h.hierarchyName = 'National' AND ihc.itemId = i.ItemId
	    ) nationalHierarchy
    OUTER APPLY (
		    SELECT
                TOP 1
			    ihc.itemID,
			    ihc.hierarchyClassId,
			    hc.hierarchyClassName,
			    hc.hierarchyClassName as HierarchyLineage
		    FROM dbo.ItemHierarchyClass ihc
		    JOIN dbo.HierarchyClass hc ON ihc.hierarchyClassID = hc.hierarchyClassID
		    JOIN dbo.Hierarchy h ON hc.hierarchyID = h.hierarchyID
		    WHERE h.hierarchyName = 'Manufacturer' AND ihc.itemId = i.ItemId
	    ) manufacturerHierarchy

        {(!string.IsNullOrWhiteSpace(itemWhereClause) ? $"WHERE ({itemWhereClause})" : "")}
) data";

            string orderBy = $"ORDER BY {this.BuildOrderByValue(parameters.OrderByValue)} {parameters.OrderByOrder}{(parameters.OrderByValue != "ScanCode" ? ", ScanCode ASC":null)} OFFSET {parameters.Skip} ROWS FETCH NEXT {parameters.Top} ROWS ONLY";


            string query = $@"{select}
                select * FROM #results WHERE {this.BuildInactiveWhereClause(parameters.ItemAttributeJsonParameters)} {orderBy}
                select COUNT(*) FROM #results WHERE {this.BuildInactiveWhereClause(parameters.ItemAttributeJsonParameters)}";

            return query;
        }

        public string BuildItemWhereClause(List<ItemSearchCriteria> searchCriteria)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ItemSearchCriteria criteria in searchCriteria)
            {
                if (criteria.AttributeName == "Inactive" || this.IsAttributeHierarchy(criteria.AttributeName))
                {
                    continue;
                }
                sb.Append($"({this.BuildSingleWhereClause(criteria)})");
                if (criteria != searchCriteria.Last())
                {
                    sb.Append(" AND ");
                }
            }
            var x = sb.ToString();
            return $"{x.TrimEnd(" AND ".ToCharArray())}";
        }

        public string BuildHierarchyJoinClause(List<ItemSearchCriteria> searchCriteria)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ItemSearchCriteria criteria in searchCriteria)
            {
                if (this.IsAttributeHierarchy(criteria.AttributeName))
                {
                    sb.Append($@"JOIN #{criteria.AttributeName} on #{criteria.AttributeName}.itemId = i.ItemId" + Environment.NewLine);
                }
            }
            return sb.ToString();
        }

        public string BuildHierarchyTempTableInclude(List<ItemSearchCriteria> searchCriteria)
        {
            StringBuilder sb = new StringBuilder();

            foreach (ItemSearchCriteria criteria in searchCriteria)
            {
                if (this.IsAttributeHierarchy(criteria.AttributeName))
                {
                    sb.Append(HierarchySelectQueries[criteria.AttributeName].Replace("{{whereClause}}", this.BuildSingleWhereClause(criteria)));
                }
            }

            return sb.ToString();
        }

        public string BuildSingleWhereClause(ItemSearchCriteria criteria)
        {
            if (criteria.SearchOperator == AttributeSearchOperator.ContainsAll)
            {
                return this.BuildContainsAllQuery(criteria);
            }
            else if (criteria.SearchOperator == AttributeSearchOperator.ContainsAny)
            {
                return this.BuildContainsAnyQuery(criteria);
            }
            else if (criteria.SearchOperator == AttributeSearchOperator.ExactlyMatchesAll)
            {
                return this.BuildExactlyAllQuery(criteria);
            }
            else if (criteria.SearchOperator == AttributeSearchOperator.ExactlyMatchesAny)
            {
                return this.BuildExactlyAnyQuery(criteria);
            }
            else if (criteria.SearchOperator == AttributeSearchOperator.HasAttribute)
            {
                return this.BuildHasAttributeQuery(criteria);
            }
            else if (criteria.SearchOperator == AttributeSearchOperator.DoesNotHaveAttribute)
            {
                return this.BuildDoesNotHaveAttributeQuery(criteria);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public string BuildSearchValue(string attributeName)
        {
            if (SpecialSearchAttributes.ContainsKey(attributeName))
            {
                return SpecialSearchAttributes[attributeName];
            }
            else
            {
                return $@"JSON_VALUE(ItemAttributesJson, '$.""{attributeName}""')";
            }
        }

        public string BuildOrderByValue(string attributeName)
        {
            if (SpecialOrderByAttributes.ContainsKey(attributeName))
            {
                return SpecialOrderByAttributes[attributeName];
            }
            else
            {
                return $@"JSON_VALUE(ItemAttributesJson, '$.""{attributeName}""')";
            }
        }

        public string BuildContainsAllQuery(ItemSearchCriteria searchCriteria)
        {
            string response = string.Empty;
            foreach (string value in searchCriteria.Values)
            {
                response += $"{this.BuildSearchValue(searchCriteria.AttributeName)} LIKE '%{value}%'";
                if (value != searchCriteria.Values.Last())
                {
                    response += " AND ";
                }
            }

            return response.TrimEnd(" AND ".ToCharArray());
        }

        public string BuildContainsAnyQuery(ItemSearchCriteria searchCriteria)
        {
            string response = string.Empty;
            foreach (string value in searchCriteria.Values)
            {
                response += $"{this.BuildSearchValue(searchCriteria.AttributeName)} LIKE '%{value}%'";
                if (value != searchCriteria.Values.Last())
                {
                    response += " OR ";
                }
            }

            return response.TrimEnd(" OR ".ToCharArray());
        }

        public string BuildExactlyAnyQuery(ItemSearchCriteria searchCriteria)
        {
            string response = string.Empty;
            foreach (string value in searchCriteria.Values)
            {
                response += $"{this.BuildSearchValue(searchCriteria.AttributeName)} = '{value}'";
                if (value != searchCriteria.Values.Last())
                {
                    response += " OR ";
                }
            }

            return response.TrimEnd(" OR ".ToCharArray());
        }

        public string BuildExactlyAllQuery(ItemSearchCriteria searchCriteria)
        {
            string response = string.Empty;
            foreach (string value in searchCriteria.Values)
            {
                response += $"{this.BuildSearchValue(searchCriteria.AttributeName)} = '{value}'";
                if (value != searchCriteria.Values.Last())
                {
                    response += " OR ";
                }
            }
            return response.TrimEnd(" OR ".ToCharArray());
        }

        public string BuildHasAttributeQuery(ItemSearchCriteria searchCriteria)
        {
            return $"{this.BuildSearchValue(searchCriteria.AttributeName)} IS NOT NULL";
        }

        public string BuildDoesNotHaveAttributeQuery(ItemSearchCriteria searchCriteria)
        {
            return $"{this.BuildSearchValue(searchCriteria.AttributeName)} IS NULL";
        }

        private string BuildInactiveWhereClause(List<ItemSearchCriteria> searchCriteria)
        {
            var inactiveSearchCriteria = searchCriteria.FirstOrDefault(x => x.AttributeName == "Inactive");

            if (inactiveSearchCriteria == null)
            {
                return $"Inactive = 'false'";
            }
            else
            {
                return $"Inactive LIKE '%{inactiveSearchCriteria.Values.First()}%'";
            }
        }
    }
}