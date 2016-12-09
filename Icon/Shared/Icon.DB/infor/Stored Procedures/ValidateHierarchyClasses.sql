CREATE PROCEDURE infor.ValidateHierarchyClasses
	@hierarchyClasses infor.ValidateHierarchyClassType READONLY
AS
BEGIN
	DECLARE @inforHierarchyClassListenerAppId INT = (SELECT AppID FROM app.App WHERE AppName = 'Infor Hierarchy Class Listener'),
			@subBrickCodeTraitId INT = (SELECT traitID FROM Trait t WHERE t.traitCode = 'SBC'),
			@taxHierarchyId INT = (SELECT hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Tax'),
			@hierarchyMismatchErrorCode NVARCHAR(50) = 'HierarchyMismatch',
			@duplicateHierarchyClassErrorCode NVARCHAR(50) = 'DuplicateHierarchyClass',
			@duplicateSubBrickCodeErrorCode NVARCHAR(50) = 'DuplicateSubBrickCode',
			@duplicateTaxCode NVARCHAR(50) = 'DuplicateTaxCode'
	DECLARE @errorHierarchyClasses TABLE (HierarchyClassId INT, ErrorCode NVARCHAR(100), ErrorDetails NVARCHAR(255))

	INSERT INTO @errorHierarchyClasses
	SELECT 
		hc.HierarchyClassId,
		@hierarchyMismatchErrorCode AS ErrorCode,
		infor.GetValidationError(@hierarchyMismatchErrorCode, @inforHierarchyClassListenerAppId, hc.HierarchyName)
	FROM @hierarchyClasses hc
	JOIN dbo.Hierarchy h ON hc.HierarchyName = h.hierarchyName
	JOIN dbo.HierarchyClass hc2 ON hc.HierarchyClassId = hc2.hierarchyClassID
	WHERE hc2.hierarchyID <> h.hierarchyID

	INSERT INTO @errorHierarchyClasses
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
		AND hc.HierarchyClassId NOT IN 
		(
			SELECT hierarchyClassID 
			FROM @errorHierarchyClasses
		)
		
	--The first 7 characters of a tax name are the digits that make up the tax code, and it must be unique.
	--That's why the SUBSTRING is pulling out the first 7 characters, so that it can match on and display the tax code.
	INSERT INTO @errorHierarchyClasses
	SELECT 
		hc.HierarchyClassId,
		@duplicateTaxCode AS ErrorCode,
		infor.GetValidationError(@duplicateTaxCode, @inforHierarchyClassListenerAppId, SUBSTRING(hc.hierarchyClassName, 1, 7))
	FROM @hierarchyClasses hc
	WHERE hc.hierarchyName = 'Tax'
		AND hc.HierarchyClassId NOT IN 
		(
			SELECT hierarchyClassID 
			FROM @errorHierarchyClasses
		)
		AND EXISTS
		(
			SELECT 1 FROM HierarchyClass tax
			WHERE SUBSTRING(tax.hierarchyClassName, 1, 7) = SUBSTRING(hc.hierarchyClassName, 1, 7)
				AND hc.HierarchyClassId <> tax.hierarchyClassID
		)

	IF EXISTS (SELECT * FROM @hierarchyClasses WHERE HierarchyLevelName = 'Sub Brick')
	BEGIN
		INSERT INTO @errorHierarchyClasses
		SELECT 
			hc.HierarchyClassId,
			@duplicateSubBrickCodeErrorCode AS ErrorCode,
			infor.GetValidationError(@duplicateSubBrickCodeErrorCode, @inforHierarchyClassListenerAppId, hc.SubBrickCode)
		FROM @hierarchyClasses hc
		WHERE hc.SubBrickCode IS NOT NULL
			AND hc.HierarchyClassId NOT IN 
			(
				SELECT HierarchyClassId 
				FROM @errorHierarchyClasses
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

	SELECT * FROM @errorHierarchyClasses
END