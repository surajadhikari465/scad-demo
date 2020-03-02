CREATE PROCEDURE [dbo].[AddOrUpdateHierarchyNationalSubCategory]
    @hierarchyNational dbo.HierarchyNationalClassType READONLY
AS
BEGIN
  SET NOCOUNT ON;

  SELECT DISTINCT t.FamilyHCID, t.CategoryHCID, t.SubcategoryHCID
  INTO #hierarchyNational
  FROM @hierarchyNational t
  WHERE t.SubcategoryHCID IS NOT NULL
    AND t.CategoryHCID IS NOT NULL
    AND t.t.FamilyHCID IS NOT NULL;

  IF(EXISTS(SELECT 1 FROM #hierarchyNational)) 
  BEGIN
    UPDATE t SET FamilyHCID = h.FamilyHCID
    FROM #hierarchyNational t
    JOIN dbo.Hierarchy_NationalClass h ON h.CategoryHCID = t.CategoryHCID;

    UPDATE h SET h.SubcategoryHCID = t.SubcategoryHCID
    FROM dbo.Hierarchy_NationalClass h
    JOIN #hierarchyNational t ON t.CategoryHCID = h.CategoryHCID AND h.SubcategoryHCID IS NULL;

    INSERT INTO dbo.Hierarchy_NationalClass(FamilyHCID, CategoryHCID, SubcategoryHCID)
    SELECT t.FamilyHCID, t.CategoryHCID, t.SubcategoryHCID
    FROM #hierarchyNational t
    LEFT JOIN dbo.Hierarchy_NationalClass h on h.SubcategoryHCID = t.SubcategoryHCID
    WHERE h.SubcategoryHCID IS NULL;
  END

  DROP TABLE #hierarchyNational;
END
GO

GRANT EXECUTE ON dbo.AddOrUpdateHierarchyNationalSubCategory TO [MammothRole];
GO