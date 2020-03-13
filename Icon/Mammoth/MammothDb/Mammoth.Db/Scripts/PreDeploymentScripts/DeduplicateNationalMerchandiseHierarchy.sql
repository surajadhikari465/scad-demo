--32371_Deduplicate_Hierarchy_Merchandise
SET NOCOUNT ON;
DECLARE @ids AS TABLE(HierarchyClassID INT);

--32842: Deduplicate Hierarchy_NationalClass
--Delete invalid records
DELETE FROM dbo.Hierarchy_NationalClass
WHERE FamilyHCID IS NULL
  OR (CategoryHCID IS NULL AND SubcategoryHCID IS NULL AND ClassHCID IS NULL)
  OR (CategoryHCID IS NULL AND SubcategoryHCID IS NOT NULL)
  OR (SubcategoryHCID IS NULL AND ClassHCID IS NOT NULL);

--Delete duplicate records
;WITH cte AS(
  SELECT HierarchyNationalClassID, Row_Number() OVER(PARTITION BY FamilyHCID, CategoryHCID, SubcategoryHCID, ClassHCID ORDER BY HierarchyNationalClassID ) rowID
  FROM dbo.Hierarchy_NationalClass)
    DELETE FROM cte where rowID > 1;

--Reset invalid Items.HierarchyNationalClassID
UPDATE i SET HierarchyNationalClassID = NULL
FROM dbo.Items i
LEFT JOIN dbo.Hierarchy_NationalClass h ON h.HierarchyNationalClassID = i.HierarchyNationalClassID
WHERE i.HierarchyNationalClassID IS NOT NULL AND h.HierarchyNationalClassID IS NULL;



--32842_Deduplicate_Hierarchy_Merchandise
--Delete invalid records
DELETE FROM dbo.Hierarchy_Merchandise
WHERE SegmentHCID IS NULL
  OR FamilyHCID IS NULL
  OR ClassHCID IS NULL
  OR BrickHCID IS NULL; --SubBrickHCID IS NULL; SubBrickHCID can be NULL. Verified with Ben and Kevin

--Delete duplicate records
;WITH cte AS(
  SELECT HierarchyMerchandiseID, Row_Number() OVER(PARTITION BY SegmentHCID, FamilyHCID, ClassHCID, BrickHCID, SubBrickHCID ORDER BY HierarchyMerchandiseID) rowID
  FROM dbo.Hierarchy_Merchandise)
  DELETE FROM cte where rowID > 1;

UPDATE i SET HierarchyMerchandiseID = NULL
FROM dbo.Items i
LEFT JOIN dbo.Hierarchy_Merchandise h ON h.HierarchyMerchandiseID = i.HierarchyMerchandiseID
WHERE i.HierarchyMerchandiseID IS NOT NULL AND h.HierarchyMerchandiseID IS NULL;