CREATE PROCEDURE dbo.AddOrUpdateHierarchyMerchandiseClass
    @hierarchy dbo.HierarchyMerchandiseClassType READONLY
AS
BEGIN
  SET NOCOUNT ON;

  SELECT DISTINCT Cast(NULL AS INT) SegmentHCID, FamilyHCID, ClassHCID
  INTO #hierarchy
  FROM @hierarchy
  WHERE FamilyHCID IS NOT NULL
    AND ClassHCID IS NOT NULL

  IF(EXISTS(SELECT 1 FROM #hierarchy)) 
  BEGIN
    UPDATE h SET SegmentHCID = hm.SegmentHCID
    FROM #hierarchy h
    JOIN dbo.Hierarchy_Merchandise hm ON hm.FamilyHCID = h.FamilyHCID;

    --Remove incomplete records
    DELETE FROM #hierarchy
    WHERE SegmentHCID IS NULL;

    UPDATE hm SET ClassHCID = h.ClassHCID, ModifiedDate = GetDate()
    FROM dbo.Hierarchy_Merchandise hm
    JOIN #hierarchy h ON h.FamilyHCID = hm.FamilyHCID
    WHERE hm.ClassHCID IS NULL;

    INSERT INTO dbo.Hierarchy_Merchandise(SegmentHCID, FamilyHCID, ClassHCID)
    SELECT h.SegmentHCID, h.FamilyHCID, h.ClassHCID
    FROM #hierarchy h
    LEFT JOIN dbo.Hierarchy_Merchandise hm ON hm.ClassHCID = h.ClassHCID
    WHERE hm.ClassHCID IS NULL;
  END

  DROP TABLE #hierarchy;
END
GO

GRANT EXECUTE ON dbo.AddOrUpdateHierarchyMerchandiseClass TO [MammothRole];
GO