CREATE PROCEDURE dbo.Get_Hierarchy_NationalClass @ItemID INT
AS

SELECT 
	  hc01.HierarchyClassName as [Family]
	, hc02.HierarchyClassName as [Category]
	, hc03.HierarchyClassName as [Subcategory]
	, hc04.HierarchyClassName as [Class]
FROM dbo.Items i01
INNER JOIN dbo.Hierarchy_NationalClass hn01 ON (i01.HierarchyNationalClassID = hn01.HierarchyNationalClassID)
INNER JOIN dbo.HierarchyClass hc01 ON (hn01.FamilyHCID		= hc01.HierarchyClassID)
INNER JOIN dbo.HierarchyClass hc02 ON (hn01.CategoryHCID	= hc02.HierarchyClassID)
INNER JOIN dbo.HierarchyClass hc03 ON (hn01.SubcategoryHCID	= hc03.HierarchyClassID)
INNER JOIN dbo.HierarchyClass hc04 ON (hn01.ClassHCID		= hc04.HierarchyClassID)
WHERE i01.ItemID = @ItemID


GO

