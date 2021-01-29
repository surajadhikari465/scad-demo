SET NOCOUNT ON;

-- Remove script from deploy history
DELETE FROM app.PostDeploymentScriptHistory
WHERE ScriptKey = 'SCM2-386_SubTeamRealignment_Add_3100_1410_Update_1750';

-- Change 1750 Subteam name back
UPDATE HierarchyClass
	SET hierarchyClassName = 'Value Add (1750)'
	WHERE hierarchyLevel = 1
		AND [HierarchyId] = @hierarchyId
		AND hierarchyClassName = 'Misc. Third Party Vendors - PROD (1750)';

--Delete two new subteams from Hierarchy and HierarchyClassTrait tables
DECLARE @HierarchyClassId INT

SELECT @HierarchyClassId = (
	SELECT hierarchyClassID
	FROM dbo.Hierarchy
	WHERE hierarchyClassName = 'Misc. Third Party Vendors - WB (3100)'
	);

DELETE FROM dbo.HierarchyClassTrait
WHERE hierarchyClassID = @HierarchyClassId;

DELETE FROM dbo.Hierarchy
WHERE hierarchyClassID = @HierarchyClassId;

SELECT @HierarchyClassId = (
	SELECT hierarchyClassID
	FROM dbo.Hierarchy
	WHERE hierarchyClassName = 'Misc. Third Party Vendors - GROC (1410)'
	);

DELETE FROM dbo.HierarchyClassTrait
WHERE hierarchyClassID = @HierarchyClassId;

DELETE FROM dbo.Hierarchy
WHERE hierarchyClassID = @HierarchyClassId;
SET NOCOUNT OFF;
GO