CREATE PROCEDURE dbo.AddOrUpdateHierarchyNationalCategory
	@hierarchyNational dbo.HierarchyNationalClassType READONLY
AS
BEGIN
  SET NOCOUNT ON;

  SELECT DISTINCT t.FamilyHCID, t.CategoryHCID
  INTO #hierarchyNational
  FROM @hierarchyNational t
  WHERE t.CategoryHCID IS NOT NULL;

  IF(EXISTS(SELECT 1 FROM #hierarchyNational)) 
  BEGIN
    UPDATE h SET CategoryHCID = t.CategoryHCID, ModifiedDate = GetDate()
    FROM dbo.Hierarchy_NationalClass h
    JOIN #hierarchyNational t ON t.FamilyHCID = h.FamilyHCID AND h.CategoryHCID IS NULL;

    INSERT INTO dbo.Hierarchy_NationalClass(FamilyHCID, CategoryHCID)
      SELECT t.FamilyHCID, t.CategoryHCID
      FROM #hierarchyNational t
      LEFT JOIN dbo.Hierarchy_NationalClass h ON h.CategoryHCID = t.CategoryHCID
    WHERE h.CategoryHCID IS NULL;
  END

  DROP TABLE #hierarchyNational;
END
GO

GRANT EXECUTE ON dbo.AddOrUpdateHierarchyNationalCategory TO [MammothRole];
GO