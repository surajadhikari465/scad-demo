CREATE PROCEDURE infor.ValidateHierarchyClasses
	@hierarchyClasses infor.ValidateHierarchyClassType READONLY,
	@validateSequenceId bit = 0
AS
BEGIN
	DECLARE @inforHierarchyClassListenerAppId INT = (SELECT AppID FROM app.App WHERE AppName = 'Infor Hierarchy Class Listener'),
			@subBrickCodeTraitId INT = (SELECT traitID FROM Trait t WHERE t.traitCode = 'SBC'),
			@taxHierarchyId INT = (SELECT hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Tax'),
			@deleteNonExistantHierarchyClassErrorCode NVARCHAR(50) = 'DeleteNonExistantHierarchyClass',
			@deleteHierarchyClassAssociatedToItemsErrorCode NVARCHAR(50) = 'DeleteHierarchyClassAssociatedToItems',
			@hierarchyMismatchErrorCode NVARCHAR(50) = 'HierarchyMismatch',
			@duplicateHierarchyClassErrorCode NVARCHAR(50) = 'DuplicateHierarchyClass',
			@duplicateSubBrickCodeErrorCode NVARCHAR(50) = 'DuplicateSubBrickCode',
			@duplicateTaxCode NVARCHAR(50) = 'DuplicateTaxCode',
			@invalidSequenceIdErrorCode NVARCHAR(50) = 'InvalidSequenceIDCode'

	CREATE TABLE #ErrorHierarchyClasses  
	(
		HierarchyClassId INT PRIMARY KEY, 
		ErrorCode NVARCHAR(100), 
		ErrorDetails NVARCHAR(255)
	)
	
	CREATE TABLE #HierarchyClasses
	(
		ActionName NVARCHAR(20) NOT NULL,
		HierarchyClassId INT NOT NULL PRIMARY KEY,
		HierarchyClassName NVARCHAR(255) NOT NULL,
		HierarchyLevelName NVARCHAR(100) NOT NULL,
		HierarchyName NVARCHAR(100) NOT NULL,
		HierarchyParentClassId INT NULL,
		SubBrickCode NVARCHAR(255) NULL,
		SequenceId NUMERIC(22, 0) NULL
	)
	
	INSERT INTO #HierarchyClasses
	SELECT 
		ActionName,
		HierarchyClassId,
		HierarchyClassName,
		HierarchyLevelName,
		HierarchyName,
		HierarchyParentClassId,
		SubBrickCode,
		SequenceId
	FROM @hierarchyClasses

	--Validate that delete hierarchy classes exist
	INSERT INTO #ErrorHierarchyClasses
	SELECT 
		hc.HierarchyClassId,
		@deleteNonExistantHierarchyClassErrorCode AS ErrorCode,
		infor.GetValidationError(@deleteNonExistantHierarchyClassErrorCode, @inforHierarchyClassListenerAppId, hc.HierarchyClassName)
	FROM #HierarchyClasses hc
	WHERE hc.ActionName = 'DELETE'
		AND NOT EXISTS 
		(
			SELECT 1 
			FROM dbo.HierarchyClass hc2 
			WHERE hc.HierarchyClassId = hc2.hierarchyClassID
		)

	--Validate that delete hierarchy classes are not associated to items
	INSERT INTO #ErrorHierarchyClasses
	SELECT 
		hc.HierarchyClassId,
		@deleteHierarchyClassAssociatedToItemsErrorCode AS ErrorCode,
		infor.GetValidationError(@deleteHierarchyClassAssociatedToItemsErrorCode, @inforHierarchyClassListenerAppId, hc.HierarchyClassName)
	FROM #HierarchyClasses hc
	WHERE hc.ActionName = 'DELETE'
		AND EXISTS 
		(
			SELECT 1 
			FROM dbo.ItemHierarchyClass ihc 
			WHERE hc.HierarchyClassId = ihc.hierarchyClassID
		)
		AND hc.HierarchyClassId NOT IN 
		(
			SELECT hierarchyClassID 
			FROM #ErrorHierarchyClasses
		)

	--Validate that sent hierarchy classes exist under the same hierarchy in Icon as they were sent under
	INSERT INTO #ErrorHierarchyClasses
	SELECT 
		hc.HierarchyClassId,
		@hierarchyMismatchErrorCode AS ErrorCode,
		infor.GetValidationError(@hierarchyMismatchErrorCode, @inforHierarchyClassListenerAppId, hc.HierarchyName)
	FROM #HierarchyClasses hc
	JOIN dbo.Hierarchy h ON hc.HierarchyName = h.hierarchyName
	JOIN dbo.HierarchyClass hc2 ON hc.HierarchyClassId = hc2.hierarchyClassID
	WHERE hc2.hierarchyID <> h.hierarchyID
		AND hc.HierarchyClassId NOT IN 
		(
			SELECT hierarchyClassID 
			FROM #ErrorHierarchyClasses
		)

	--Validate that the hierarchy class is unique within its hierarchy
	INSERT INTO #ErrorHierarchyClasses
	SELECT 
		hc.HierarchyClassId,
		@duplicateHierarchyClassErrorCode AS ErrorCode,
		infor.GetValidationError(@duplicateHierarchyClassErrorCode, @inforHierarchyClassListenerAppId, hc.HierarchyClassName)
	FROM #HierarchyClasses hc
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
			FROM #ErrorHierarchyClasses
		)
		
	--The first 7 characters of a tax name are the digits that make up the tax code, and it must be unique.
	--That's why the SUBSTRING is pulling out the first 7 characters, so that it can match on and display the tax code.
	INSERT INTO #ErrorHierarchyClasses
	SELECT 
		hc.HierarchyClassId,
		@duplicateTaxCode AS ErrorCode,
		infor.GetValidationError(@duplicateTaxCode, @inforHierarchyClassListenerAppId, SUBSTRING(hc.hierarchyClassName, 1, 7))
	FROM #HierarchyClasses hc
	WHERE hc.hierarchyName = 'Tax'
		AND hc.HierarchyClassId NOT IN 
		(
			SELECT hierarchyClassID 
			FROM #ErrorHierarchyClasses
		)
		AND EXISTS
		(
			SELECT 1 FROM HierarchyClass tax
			WHERE SUBSTRING(tax.hierarchyClassName, 1, 7) = SUBSTRING(hc.hierarchyClassName, 1, 7)
				AND hc.HierarchyClassId <> tax.hierarchyClassID
		)
	
	--Validate that the Sub Brick Code is unique for all sub bricks
	IF EXISTS (SELECT * FROM #HierarchyClasses WHERE HierarchyLevelName = 'Sub Brick')
	BEGIN
		INSERT INTO #ErrorHierarchyClasses
		SELECT 
			hc.HierarchyClassId,
			@duplicateSubBrickCodeErrorCode AS ErrorCode,
			infor.GetValidationError(@duplicateSubBrickCodeErrorCode, @inforHierarchyClassListenerAppId, hc.SubBrickCode)
		FROM #HierarchyClasses hc
		WHERE hc.SubBrickCode IS NOT NULL
			AND hc.HierarchyClassId NOT IN 
			(
				SELECT HierarchyClassId 
				FROM #ErrorHierarchyClasses
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

	--Validate the Sequence ID of the message
	IF @validateSequenceId = 1
	BEGIN
		INSERT INTO #ErrorHierarchyClasses
		SELECT
			hc.HierarchyClassId,
			@invalidSequenceIdErrorCode AS ErrorCode,
			infor.GetValidationError(@invalidSequenceIdErrorCode, @inforHierarchyClassListenerAppId, hc.SequenceID)
		FROM #HierarchyClasses hc
		LEFT JOIN infor.HierarchyClassSequence hcs ON hc.HierarchyClassId = hcs.HierarchyClassID
		WHERE hc.SequenceId IS NOT NULL
			AND hcs.SequenceID IS NOT NULL 
			AND hcs.SequenceID > hc.SequenceId
			AND hc.HierarchyClassId NOT IN 
			(
				SELECT HierarchyClassId 
				FROM #ErrorHierarchyClasses
			)
	END

	SELECT * FROM #ErrorHierarchyClasses
END