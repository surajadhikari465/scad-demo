CREATE PROCEDURE dbo.AddOrUpdateHierarchyMerchandiseSubBrick
    @hierarchy dbo.HierarchyMerchandiseClassType READONLY
AS
BEGIN
  SET NOCOUNT ON;

  SELECT DISTINCT Cast(NULL AS INT) SegmentHCID, Cast(NULL AS INT) FamilyHCID, Cast(NULL AS INT) ClassHCID, BrickHCID, SubBrickHCID
  INTO #hierarchy
  FROM @hierarchy
  WHERE BrickHCID IS NOT NULL
    AND SubBrickHCID IS NOT NULL

  IF(EXISTS(SELECT 1 FROM #hierarchy)) 
  BEGIN
    UPDATE h SET SegmentHCID = hm.SegmentHCID, FamilyHCID = hm.FamilyHCID, ClassHCID = hm.ClassHCID
    FROM #hierarchy h
    JOIN dbo.Hierarchy_Merchandise hm ON hm.BrickHCID = h.BrickHCID;

    --Remove incomplete records
    DELETE FROM #hierarchy
    WHERE SegmentHCID IS NULL OR FamilyHCID IS NULL OR ClassHCID IS NULL;

    UPDATE hm SET SubBrickHCID = h.SubBrickHCID, ModifiedDate = GetDate()
    FROM dbo.Hierarchy_Merchandise hm
    JOIN #hierarchy h ON h.BrickHCID = hm.BrickHCID
    WHERE hm.SubBrickHCID IS NULL;

    INSERT INTO dbo.Hierarchy_Merchandise(SegmentHCID, FamilyHCID, ClassHCID, BrickHCID, SubBrickHCID)
    SELECT h.SegmentHCID, h.FamilyHCID, h.ClassHCID, h.BrickHCID, h.SubBrickHCID
    FROM #hierarchy h
    LEFT JOIN dbo.Hierarchy_Merchandise hm ON hm.SubBrickHCID = h.SubBrickHCID
    WHERE hm.SubBrickHCID IS NULL;
  END

  DROP TABLE #hierarchy;
END
GO

GRANT EXECUTE ON dbo.AddOrUpdateHierarchyMerchandiseSubBrick TO [MammothRole];
GO