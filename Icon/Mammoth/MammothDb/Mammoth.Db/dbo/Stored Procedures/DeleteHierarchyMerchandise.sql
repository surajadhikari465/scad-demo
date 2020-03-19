CREATE PROCEDURE dbo.DeleteHierarchyMerchandise
    @hcID dbo.IntListType READONLY
AS
BEGIN
  SET NOCOUNT ON;

  SELECT DISTINCT [Value] AS HCID
  INTO #hcID
  FROM @hcID
  WHERE [Value] IS NOT NULL;

  CREATE TABLE #temp(HierarchyMerchandiseID INT, SegmentHCID INT, FamilyHCID INT, ClassHCID INT, BrickHCID INT, rowID INT);
  CREATE INDEX IX_tempHierarchyMerchandiseID ON #temp(HierarchyMerchandiseID); 

  --Segment
  DELETE hm
  FROM dbo.Hierarchy_Merchandise hm
  INNER JOIN #hcID h ON h.HCID = hm.SegmentHCID;

  --Family
  INSERT INTO #temp(HierarchyMerchandiseID, SegmentHCID, FamilyHCID, rowID)
    SELECT hm.HierarchyMerchandiseID, hm.SegmentHCID, hm.FamilyHCID, Row_Number() OVER(PARTITION BY hm.FamilyHCID ORDER BY(hm.HierarchyMerchandiseID)) rowID
    FROM dbo.Hierarchy_Merchandise hm
    INNER JOIN #hcID h ON h.HCID = hm.FamilyHCID;

  IF(Exists(SELECT 1 FROM #temp))
  BEGIN
    DELETE hm
    FROM dbo.Hierarchy_Merchandise hm
    INNER JOIN #temp h ON h.HierarchyMerchandiseID = hm.HierarchyMerchandiseID
    WHERE h.rowID > 1;

    ;WITH cte AS(SELECT hm.HierarchyMerchandiseID, Row_Number() OVER(PARTITION BY hm.SegmentHCID ORDER BY(hm.HierarchyMerchandiseID)) rowID
                 FROM dbo.Hierarchy_Merchandise hm
                 INNER JOIN #temp h ON h.SegmentHCID = hm.SegmentHCID AND h.FamilyHCID = IsNull(hm.FamilyHCID, h.FamilyHCID) 
                 WHERE h.rowID = 1)
      DELETE hm
      FROM dbo.Hierarchy_Merchandise hm
      INNER JOIN cte h ON h.HierarchyMerchandiseID = hm.HierarchyMerchandiseID
      WHERE h.rowID > 1;

    UPDATE hm SET FamilyHCID = NULL, ClassHCID = NULL, BrickHCID = NULL, SubBrickHCID = NULL, ModifiedDate = GetDate()
    FROM dbo.Hierarchy_Merchandise hm
    INNER JOIN #temp h ON h.HierarchyMerchandiseID = hm.HierarchyMerchandiseID
    WHERE h.rowID = 1;
  END

  --Class
  TRUNCATE TABLE #temp;

  INSERT INTO #temp(HierarchyMerchandiseID, SegmentHCID, FamilyHCID, rowID)
    SELECT hm.HierarchyMerchandiseID, hm.SegmentHCID, hm.familyHCID, Row_Number() OVER(PARTITION BY hm.ClassHCID ORDER BY(hm.HierarchyMerchandiseID)) rowID
    FROM dbo.Hierarchy_Merchandise hm
    INNER JOIN #hcID h ON h.HCID = hm.ClassHCID;

  IF(Exists(SELECT 1 FROM #temp))
  BEGIN
    DELETE hm
    FROM dbo.Hierarchy_Merchandise hm
    INNER JOIN #temp h ON h.HierarchyMerchandiseID = hm.HierarchyMerchandiseID
    WHERE h.rowID > 1;
    
    ;WITH cte AS(SELECT hm.HierarchyMerchandiseID, Row_Number() OVER(PARTITION BY hm.SegmentHCID, hm.FamilyHCID ORDER BY(hm.HierarchyMerchandiseID)) rowID
                 FROM dbo.Hierarchy_Merchandise hm
                 INNER JOIN #temp h ON h.SegmentHCID = hm.SegmentHCID AND IsNull(h.FamilyHCID, 0) = IsNull(hm.FamilyHCID, 0)
                 WHERE h.rowID = 1)
      DELETE hm
      FROM dbo.Hierarchy_Merchandise hm
      INNER JOIN cte h ON h.HierarchyMerchandiseID = hm.HierarchyMerchandiseID
      WHERE h.rowID > 1;

    UPDATE hm SET ClassHCID = NULL, BrickHCID = NULL, SubBrickHCID = NULL, ModifiedDate = GetDate()
    FROM dbo.Hierarchy_Merchandise hm
    INNER JOIN #temp h ON h.HierarchyMerchandiseID = hm.HierarchyMerchandiseID
    WHERE h.rowID = 1; 
  END

  --Brick
  TRUNCATE TABLE #temp;

  INSERT INTO #temp(HierarchyMerchandiseID, SegmentHCID, FamilyHCID, ClassHCID, rowID)
    SELECT hm.HierarchyMerchandiseID, hm.SegmentHCID, hm.FamilyHCID, hm.ClassHCID, Row_Number() OVER(PARTITION BY hm.BrickHCID ORDER BY(hm.HierarchyMerchandiseID)) rowID
    FROM dbo.Hierarchy_Merchandise hm
    INNER JOIN #hcID h ON h.HCID = hm.BrickHCID;

  IF(Exists(SELECT 1 FROM #temp))
  BEGIN
    DELETE hm
    FROM dbo.Hierarchy_Merchandise hm
    INNER JOIN #temp h ON h.HierarchyMerchandiseID = hm.HierarchyMerchandiseID
    WHERE h.rowID > 1;

    ;WITH cte AS(SELECT hm.HierarchyMerchandiseID, Row_Number() OVER(PARTITION BY hm.SegmentHCID, hm.FamilyHCID, hm.ClassHCID ORDER BY(hm.HierarchyMerchandiseID)) rowID
                 FROM dbo.Hierarchy_Merchandise hm
                 INNER JOIN #temp h ON h.SegmentHCID = hm.SegmentHCID AND IsNull(h.FamilyHCID, 0) = IsNull(hm.FamilyHCID, 0) AND IsNull(h.ClassHCID, 0) = IsNull(hm.ClassHCID, 0)
                 WHERE h.rowID = 1)
      DELETE hm
      FROM dbo.Hierarchy_Merchandise hm
      INNER JOIN cte h ON h.HierarchyMerchandiseID = hm.HierarchyMerchandiseID
      WHERE h.rowID > 1;

    UPDATE hm SET BrickHCID = NULL, SubBrickHCID = NULL, ModifiedDate = GetDate()
    FROM dbo.Hierarchy_Merchandise hm
    INNER JOIN #temp h ON h.HierarchyMerchandiseID = hm.HierarchyMerchandiseID
    WHERE h.rowID = 1; 
  END

  --SubBrick
  TRUNCATE TABLE #temp;

  INSERT INTO #temp(HierarchyMerchandiseID, SegmentHCID, FamilyHCID, ClassHCID, BrickHCID, rowID)
    SELECT hm.HierarchyMerchandiseID, hm.SegmentHCID, hm.FamilyHCID, hm.ClassHCID, hm.BrickHCID, Row_Number() OVER(PARTITION BY hm.SubBrickHCID ORDER BY(hm.HierarchyMerchandiseID)) rowID
    FROM dbo.Hierarchy_Merchandise hm
    INNER JOIN #hcID h ON h.HCID = hm.SubBrickHCID;

  IF(Exists(SELECT 1 FROM #temp))
  BEGIN
    DELETE hm
    FROM dbo.Hierarchy_Merchandise hm
    INNER JOIN #temp h ON h.HierarchyMerchandiseID = hm.HierarchyMerchandiseID
    WHERE h.rowID > 1;

    ;WITH cte AS(SELECT hm.HierarchyMerchandiseID, Row_Number() OVER(PARTITION BY hm.SegmentHCID, hm.FamilyHCID, hm.ClassHCID, hm.BrickHCID ORDER BY(hm.HierarchyMerchandiseID)) rowID
                 FROM dbo.Hierarchy_Merchandise hm
                 INNER JOIN #temp h ON h.SegmentHCID = hm.SegmentHCID AND IsNull(h.FamilyHCID, 0) = IsNull(hm.FamilyHCID, 0) AND IsNull(h.ClassHCID, 0) = IsNull(hm.ClassHCID, 0)  AND IsNull(h.BrickHCID, 0) = IsNull(hm.BrickHCID, 0)
                 WHERE h.rowID = 1)
      DELETE hm
      FROM dbo.Hierarchy_Merchandise hm
      INNER JOIN cte h ON h.HierarchyMerchandiseID = hm.HierarchyMerchandiseID
      WHERE h.rowID > 1;

    UPDATE hm SET SubBrickHCID = NULL, ModifiedDate = GetDate()
    FROM dbo.Hierarchy_Merchandise hm
    INNER JOIN #temp h ON h.HierarchyMerchandiseID = hm.HierarchyMerchandiseID
    WHERE h.rowID = 1; 
  END
END
GO

GRANT EXECUTE ON dbo.DeleteHierarchyMerchandise TO [MammothRole];
GO
