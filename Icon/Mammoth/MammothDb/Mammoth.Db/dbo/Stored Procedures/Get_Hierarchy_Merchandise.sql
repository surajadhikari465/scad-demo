CREATE PROCEDURE [dbo].[Get_Hierarchy_Merchandise]
	@ItemID INT
AS

SELECT 
	  hc01.HierarchyClassName as [Segment]
	, hc02.HierarchyClassName as [Family]
	, hc03.HierarchyClassName as [Class]
	, hc04.HierarchyClassName as [Brick]
	, hc05.HierarchyClassName as [Subbrick]
FROM dbo.Items i01
INNER JOIN dbo.Hierarchy_Merchandise hm01 ON (i01.HierarchyMerchandiseID = hm01.HierarchyMerchandiseID)
INNER JOIN dbo.HierarchyClass hc01 ON (hm01.SegmentHCID		= hc01.HierarchyClassID)
INNER JOIN dbo.HierarchyClass hc02 ON (hm01.FamilyHCID		= hc02.HierarchyClassID)
INNER JOIN dbo.HierarchyClass hc03 ON (hm01.ClassHCID		= hc03.HierarchyClassID)
INNER JOIN dbo.HierarchyClass hc04 ON (hm01.BrickHCID		= hc04.HierarchyClassID)
INNER JOIN dbo.HierarchyClass hc05 ON (hm01.SubBrickHCID	= hc05.HierarchyClassID)
WHERE i01.ItemID = @ItemID



GO

