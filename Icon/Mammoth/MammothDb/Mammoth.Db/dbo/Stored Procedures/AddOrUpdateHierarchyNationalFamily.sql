CREATE PROCEDURE [dbo].[AddOrUpdateHierarchyNationalFamily]
    @hierarchyNational dbo.HierarchyNationalClassType READONLY
AS
BEGIN
  SET NOCOUNT ON;

  SELECT DISTINCT t.FamilyHCID
  INTO #hierarchyNational
  FROM @hierarchyNational t
  WHERE t.FamilyHCID IS NOT NULL;

  IF(EXISTS(SELECT 1 FROM #hierarchyNational)) 
  BEGIN
    INSERT INTO dbo.Hierarchy_NationalClass(FamilyHCID)
    SELECT t.FamilyHCID
    FROM #hierarchyNational t
    LEFT JOIN dbo.Hierarchy_NationalClass h ON h.FamilyHCID = t.FamilyHCID
    WHERE h.FamilyHCID IS NULL;
  END

  DROP TABLE #hierarchyNational;
END
GO

GRANT EXECUTE ON dbo.AddOrUpdateHierarchyNationalFamily TO [MammothRole];
GO