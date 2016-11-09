CREATE PROCEDURE [infor].[ItemHierarchyClassAddOrUpdate]
	@ItemHierarchyClasses infor.ItemHierarchyClassAddOrUpdateType READONLY
AS
BEGIN
	DECLARE @localeID INT = (SELECT localeID FROM dbo.Locale WHERE localeName = 'Whole Foods')

	DELETE ihc
	FROM ItemHierarchyClass ihc
	JOIN HierarchyClass hc 
	ON ihc.hierarchyClassID = hc.hierarchyClassID
	JOIN @ItemHierarchyClasses s
	ON ihc.itemID = s.ItemId
	AND hc.hierarchyID = s.HierarchyId
	WHERE ihc.hierarchyClassID <> s.hierarchyClassID

	INSERT ItemHierarchyClass
	SELECT ItemId,infor.GetHierarchyClassIdFromInfor(s.HierarchyId, s.HierarchyClassId), @localeID
	FROM  @ItemHierarchyClasses s
	WHERE NOT EXISTS
	(
		SELECT 1
		FROM ItemHierarchyClass ihc
		WHERE ihc.hierarchyClassID = s.HierarchyClassId
			AND ihc.itemID = s.ItemId
	);
	
END

