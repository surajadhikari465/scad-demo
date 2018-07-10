CREATE PROCEDURE infor.HierarchyClassAddOrUpdate
	@hierarchyClasses infor.HierarchyClassType READONLY,
	@hierarchyClassTraits infor.HierarchyClassTraitType READONLY,
	@regions infor.RegionCodeList READONLY
AS
BEGIN
	--Generate events and messages for Hierarchy Classes
	DECLARE	@financialHierarchyId INT	= (SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Financial')

	--Add or update Hierarchy Classes
	--this sp will work even if @hierarchyClasses table has financial and other heirarchy records
	SET IDENTITY_INSERT dbo.HierarchyClass ON
	
	MERGE dbo.HierarchyClass AS Target
	USING 
		(
			SELECT 
				hc.*, 
				hp.hierarchyLevel
			FROM @hierarchyClasses hc
			JOIN dbo.HierarchyPrototype hp on hc.HierarchyLevelName = hp.HierarchyLevelName
			WHERE hc.[HierarchyId] != @financialHierarchyId
		)AS Source
	ON Target.hierarchyClassID = Source.HierarchyClassId 
	WHEN MATCHED THEN
		UPDATE	
			SET hierarchyClassName = Source.HierarchyClassName
	WHEN NOT MATCHED THEN
		INSERT (hierarchyClassID, hierarchyLevel, hierarchyID, hierarchyParentClassID, hierarchyClassName)
		VALUES (Source.HierarchyClassID, Source.hierarchyLevel, Source.HierarchyId, Source.ParentHierarchyClassId, Source.HierarchyClassName);
	
	SET IDENTITY_INSERT dbo.HierarchyClass OFF

	MERGE dbo.HierarchyClassTrait AS Target
	USING (SELECT * FROM @hierarchyClassTraits WHERE HierarchyClassId IN (SELECT hierarchyClassID FROM @hierarchyClasses WHERE [HierarchyId] != @financialHierarchyId )) AS Source
	ON Target.hierarchyClassID = Source.HierarchyClassId
		AND Target.traitID = Source.TraitId
	WHEN MATCHED THEN
		UPDATE	
			SET traitValue = Source.TraitValue
	WHEN NOT MATCHED THEN
		INSERT (hierarchyClassID, traitID, uomID, traitValue)
		VALUES (Source.HierarchyClassID, Source.TraitId, null, Source.TraitValue);

	INSERT INTO infor.HierarchyClassSequence(HierarchyClassID, InforMessageId, SequenceID)
	SELECT 
		hc.HierarchyClassId,
		hc.InforMessageId,
		hc.SequenceId
	FROM @hierarchyClasses hc
	WHERE hc.SequenceId IS NOT NULL
		AND hc.HierarchyClassId NOT IN
		(
			SELECT HierarchyClassId FROM infor.HierarchyClassSequence
		)

	UPDATE hcs
	SET SequenceID = hc.SequenceId,
		ModifiedDateUtc = SYSUTCDATETIME(),
		InforMessageId = hc.InforMessageId
	FROM @hierarchyClasses hc
	JOIN infor.HierarchyClassSequence hcs ON hc.HierarchyClassId = hcs.HierarchyClassID
	WHERE hc.SequenceId IS NOT NULL
    
	-- if there are records for financial then call the stored procedure.
	IF EXISTS(SELECT 1 FROM @hierarchyClasses WHERE [HierarchyId] = @financialHierarchyId)
	BEGIN
		EXEC infor.FinancialHierarchyClassAddOrUpdate @hierarchyClasses,@hierarchyClassTraits
	END
END
go