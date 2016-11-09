CREATE PROCEDURE infor.ValidateHierarchyClasses
	@hierarchyClasses infor.ValidateHierarchyClassType READONLY
AS
BEGIN
	DECLARE @inforHierarchyClassListenerAppId int = (SELECT AppID FROM app.App WHERE AppName = 'Infor Hierarchy Class Listener'),
			@subBrickCodeTraitId int = (SELECT traitID FROM Trait t where t.traitCode = 'SBC'),
			@hierarchyMismatchErrorCode nvarchar(50) = 'HierarchyMismatch',
			@duplicateHierarchyClassErrorCode nvarchar(50) = 'DuplicateHierarchyClass',
			@duplicateSubBrickCodeErrorCode nvarchar(50) = 'DuplicateSubBrickCode'
	DECLARE @tempHierarchyClasses TABLE (HierarchyClassId int, ErrorCode nvarchar(100), ErrorDetails nvarchar(255))

	INSERT INTO @tempHierarchyClasses
	SELECT 
		hc.HierarchyClassId,
		@hierarchyMismatchErrorCode AS ErrorCode,
		infor.GetValidationError(@hierarchyMismatchErrorCode, @inforHierarchyClassListenerAppId, hc.HierarchyName)
	FROM @hierarchyClasses hc
	JOIN dbo.Hierarchy h ON hc.HierarchyName = h.hierarchyName
	JOIN dbo.HierarchyClass hc2 ON hc.HierarchyClassId = hc2.hierarchyClassID
	WHERE hc2.hierarchyID <> h.hierarchyID

	INSERT INTO @tempHierarchyClasses
	SELECT 
		hc.HierarchyClassId,
		@duplicateHierarchyClassErrorCode AS ErrorCode,
		infor.GetValidationError(@duplicateHierarchyClassErrorCode, @inforHierarchyClassListenerAppId, hc.HierarchyClassName)
	FROM @hierarchyClasses hc
	JOIN dbo.Hierarchy h ON hc.HierarchyName = h.hierarchyName
	JOIN dbo.HierarchyPrototype hp ON hc.HierarchyLevelName = hp.hierarchyLevelName
	JOIN dbo.HierarchyClass hc2 ON hc.HierarchyClassName = hc2.hierarchyClassName
	WHERE hc.HierarchyClassId <> hc2.hierarchyClassID
		AND hc2.hierarchyID = h.hierarchyID
		AND hc2.hierarchyLevel = hp.hierarchyLevel
		AND (hc.HierarchyParentClassId IS NULL AND hc2.hierarchyParentClassID IS NULL 
				OR hc.HierarchyParentClassId = hc2.hierarchyParentClassID)

	IF EXISTS (SELECT * FROM @hierarchyClasses WHERE HierarchyLevelName = 'Sub Brick')
	BEGIN
		INSERT INTO @tempHierarchyClasses
		SELECT 
			hc.HierarchyClassId,
			@duplicateSubBrickCodeErrorCode AS ErrorCode,
			infor.GetValidationError(@duplicateSubBrickCodeErrorCode, @inforHierarchyClassListenerAppId, hc.SubBrickCode)
		FROM @hierarchyClasses hc
		WHERE hc.SubBrickCode IS NOT NULL
			AND hc.HierarchyClassId NOT IN 
			(
				SELECT HierarchyClassId 
				FROM @tempHierarchyClasses
			)
			AND EXISTS 
			(
				SELECT 1 
				FROM HierarchyClassTrait subBrickCode
				WHERE 
					subBrickCode.traitID = @subBrickCodeTraitId
					AND subBrickCode.HierarchyClassID <> hc.HierarchyClassId
					AND subBrickCode.traitValue = hc.SubBrickCode
			)
	END

	SELECT * FROM @tempHierarchyClasses
END