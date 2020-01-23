SET NOCOUNT ON;

DECLARE @manufacturerHierarchyId int;
SET @manufacturerHierarchyId = (SELECT hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Manufacturer');

SELECT
	hc.hierarchyClassID AS ManufacturerId,
	hc.hierarchyClassName AS ManufacturerName
FROM HierarchyClass hc
WHERE hc.hierarchyID = @manufacturerHierarchyId