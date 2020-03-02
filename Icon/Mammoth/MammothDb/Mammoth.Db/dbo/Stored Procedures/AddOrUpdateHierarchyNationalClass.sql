CREATE PROCEDURE dbo.AddOrUpdateHierarchyNationalClass
	@hierarchyNational dbo.HierarchyNationalClassType READONLY
AS
BEGIN
  SET NOCOUNT ON;

  SELECT DISTINCT t.FamilyHCID, t.CategoryHCID, t.SubcategoryHCID, t.ClassHCID
  INTO #hierarchyNational
  FROM @hierarchyNational t
  WHERE t.ClassHCID IS NOT NULL
    AND t.SubcategoryHCID IS NOT NULL;

  IF(EXISTS(SELECT 1 FROM #hierarchyNational)) 
  BEGIN
    UPDATE t SET FamilyHCID = h.FamilyHCID, CategoryHCID = h.CategoryHCID
    FROM #hierarchyNational t
    JOIN dbo.Hierarchy_NationalClass h ON h.SubcategoryHCID = t.SubcategoryHCID;

    UPDATE h SET ClassHCID = t.ClassHCID
    FROM dbo.Hierarchy_NationalClass h
    JOIN #hierarchyNational t ON t.SubcategoryHCID = h.SubcategoryHCID AND h.ClassHCID IS NULL;

    INSERT INTO dbo.Hierarchy_NationalClass(FamilyHCID, CategoryHCID, SubcategoryHCID, ClassHCID)
    SELECT t.FamilyHCID, t.CategoryHCID, t.SubcategoryHCID, t.ClassHCID
    FROM #hierarchyNational t
    LEFT JOIN dbo.Hierarchy_NationalClass h on h.ClassHCID = t.ClassHCID
    WHERE h.ClassHCID IS NULL;
  END

  DROP TABLE #hierarchyNational;
END
GO

GRANT EXECUTE ON dbo.AddOrUpdateHierarchyNationalClass TO [MammothRole];
GO