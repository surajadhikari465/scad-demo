CREATE PROCEDURE dbo.AddOrUpdateHierarchyMerchandiseFamily
    @hierarchy dbo.HierarchyMerchandiseClassType READONLY
AS
BEGIN
  SET NOCOUNT ON;

  SELECT DISTINCT SegmentHCID, FamilyHCID
  INTO #hierarchy
  FROM @hierarchy
  WHERE SegmentHCID IS NOT NULL
    AND FamilyHCID IS NOT NULL;

  IF(EXISTS(SELECT 1 FROM #hierarchy)) 
  BEGIN
    UPDATE hm SET FamilyHCID = h.FamilyHCID, ModifiedDate = GetDate()
    FROM dbo.Hierarchy_Merchandise hm
    JOIN #hierarchy h ON h.SegmentHCID = hm.SegmentHCID
    WHERE hm.FamilyHCID IS NULL;

    INSERT INTO dbo.Hierarchy_Merchandise(SegmentHCID, FamilyHCID)
    SELECT h.SegmentHCID, h.FamilyHCID
    FROM #hierarchy h
    LEFT JOIN dbo.Hierarchy_Merchandise hm ON hm.FamilyHCID = h.FamilyHCID
    WHERE hm.FamilyHCID IS NULL;
  END

  DROP TABLE #hierarchy;
END
GO

GRANT EXECUTE ON dbo.AddOrUpdateHierarchyMerchandiseFamily TO [MammothRole];
GO