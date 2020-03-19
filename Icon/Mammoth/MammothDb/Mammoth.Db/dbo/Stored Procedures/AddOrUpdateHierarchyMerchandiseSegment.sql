CREATE PROCEDURE dbo.AddOrUpdateHierarchyMerchandiseSegment
    @hierarchy dbo.HierarchyMerchandiseClassType READONLY
AS
BEGIN
  SET NOCOUNT ON;

  SELECT DISTINCT SegmentHCID
  INTO #hierarchy
  FROM @hierarchy
  WHERE SegmentHCID IS NOT NULL;

  IF(EXISTS(SELECT 1 FROM #hierarchy)) 
  BEGIN
    INSERT INTO dbo.Hierarchy_Merchandise(SegmentHCID)
    SELECT h.SegmentHCID
    FROM #hierarchy h
    LEFT JOIN dbo.Hierarchy_Merchandise hm ON hm.SegmentHCID = h.SegmentHCID
    WHERE hm.SegmentHCID IS NULL;
  END

  DROP TABLE #hierarchy;
END
GO

GRANT EXECUTE ON dbo.AddOrUpdateHierarchyMerchandiseSegment TO [MammothRole];
GO