CREATE PROCEDURE dbo.AddOrUpdateHierarchyMerchandiseBrick
    @hierarchy dbo.HierarchyMerchandiseClassType READONLY
AS
BEGIN
  SET NOCOUNT ON;

  SELECT DISTINCT Cast(NULL AS INT) SegmentHCID, Cast(NULL AS INT) FamilyHCID, ClassHCID, BrickHCID
  INTO #hierarchy
  FROM @hierarchy
  WHERE ClassHCID IS NOT NULL
    AND BrickHCID IS NOT NULL

  IF(EXISTS(SELECT 1 FROM #hierarchy)) 
  BEGIN
    UPDATE h SET SegmentHCID = hm.SegmentHCID, FamilyHCID = hm.FamilyHCID
    FROM #hierarchy h
    JOIN dbo.Hierarchy_Merchandise hm ON hm.ClassHCID = h.ClassHCID;

    --Remove incomplete records
    DELETE FROM #hierarchy
    WHERE SegmentHCID IS NULL OR FamilyHCID IS NULL;

    UPDATE hm SET BrickHCID = h.BrickHCID, ModifiedDate = GetDate()
    FROM dbo.Hierarchy_Merchandise hm
    JOIN #hierarchy h ON h.ClassHCID = hm.ClassHCID
    WHERE hm.BrickHCID IS NULL;

    INSERT INTO dbo.Hierarchy_Merchandise(SegmentHCID, FamilyHCID, ClassHCID, BrickHCID)
    SELECT h.SegmentHCID, h.FamilyHCID, h.ClassHCID, h.BrickHCID
    FROM #hierarchy h
    LEFT JOIN dbo.Hierarchy_Merchandise hm ON hm.BrickHCID = h.BrickHCID
    WHERE hm.BrickHCID IS NULL;
  END

  DROP TABLE #hierarchy;
END
GO

GRANT EXECUTE ON dbo.AddOrUpdateHierarchyMerchandiseBrick TO [MammothRole];
GO