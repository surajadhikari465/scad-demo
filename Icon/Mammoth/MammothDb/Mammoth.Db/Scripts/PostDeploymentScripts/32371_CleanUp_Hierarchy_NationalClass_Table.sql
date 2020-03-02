SET NOCOUNT ON;
DECLARE @ids AS TABLE(HierarchyNationalClassID INT);

--Duplicate records
;WITH cte AS(
  SELECT HierarchyNationalClassID, Row_Number() OVER(PARTITION BY FamilyHCID, CategoryHCID, SubcategoryHCID, ClassHCID ORDER BY HierarchyNationalClassID ) rowID
  FROM dbo.Hierarchy_NationalClass)
    INSERT INTO @ids(HierarchyNationalClassID)
    SELECT HierarchyNationalClassID FROM cte WHERE rowID > 1;

INSERT INTO @ids(HierarchyNationalClassID)
SELECT h.HierarchyNationalClassID
FROM dbo.Hierarchy_NationalClass h
LEFT JOIN @ids t on t.HierarchyNationalClassID = h.HierarchyNationalClassID 
WHERE t.HierarchyNationalClassID is null
  AND(FamilyHCID IS NULL
   OR (CategoryHCID IS NULL AND SubcategoryHCID IS NULL AND ClassHCID IS NULL)
   OR (CategoryHCID IS NULL AND SubcategoryHCID IS NOT NULL)
   OR (SubcategoryHCID IS NULL AND ClassHCID IS NOT NULL));

--Reset invalid Items.HierarchyNationalClassID
UPDATE i SET HierarchyNationalClassID = null
FROM dbo.Items i
LEFT JOIN dbo.Hierarchy_NationalClass h ON h.HierarchyNationalClassID = i.HierarchyNationalClassID
WHERE i.HierarchyNationalClassID is not null and h.HierarchyNationalClassID is null;

UPDATE i SET HierarchyNationalClassID = null
FROM dbo.Items i
JOIN @ids t ON t.HierarchyNationalClassID = i.HierarchyNationalClassID;

--Delete invalid/duplicate records from dbo.Hierarchy_NationalClass
DELETE h
FROM dbo.Hierarchy_NationalClass h
JOIN @ids t ON t.HierarchyNationalClassID = h.HierarchyNationalClassID; 