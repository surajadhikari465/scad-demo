﻿CREATE PROCEDURE infor.HierarchyClassAddOrUpdate
	@hierarchyClasses infor.HierarchyClassType READONLY,
	@hierarchyClassTraits infor.HierarchyClassTraitType READONLY,
	@regions infor.RegionCodeList READONLY
AS
BEGIN

	--Generate events and messages for Hierarchy Classes
	DECLARE @brandEventTypeId INT		= (SELECT EventId FROM app.EventType WHERE EventName = 'Brand Name Update'),
			@brandHierarchyId INT		= (SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Brands'),
			@financialHierarchyId INT	= (SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Financial'),
			@merchHierarchyId INT		= (SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Merchandise')
	--Add or update Hierarchy Classes
	-- this sp will work even if @hierarchyClasses table has financial and other heirarchy records
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
    
	-- if there are records for financial then call the stored procedure.
	IF EXISTS(SELECT 1 FROM @hierarchyClasses WHERE [HierarchyId] = @financialHierarchyId)
	BEGIN
			EXEC infor.FinancialHierarchyClassAddOrUpdate @hierarchyClasses,@hierarchyClassTraits
	END

	DECLARE @hierarchyMessageTypeId int = (select MessageTypeId from app.MessageType where MessageTypeName = 'Hierarchy'), 
			@readyStatusId int = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'Ready'), 
			@messageActionId int = (select MessageActionId from app.MessageAction where MessageActionName = 'AddOrUpdate')

	INSERT INTO
		app.MessageQueueHierarchy
	SELECT
		MessageTypeId			= @hierarchyMessageTypeId,
		MessageStatusId			= @readyStatusId,
		MessageHistoryId		= null,
		MessageActionId			= @messageActionId,
		InsertDate				= sysdatetime(),
		HierarchyId				= hc.HierarchyId,
		HierarchyName			= h.hierarchyName,
		HierarchyLevelName		= hp.hierarchyLevelName,
		ItemsAttached			= hp.itemsAttached,
		HierarchyClassId		= CASE 
										WHEN hc.HierarchyId = @financialHierarchyId THEN SUBSTRING(hc.hierarchyClassName, charindex('(', hc.hierarchyClassName) + 1, 4)
										ELSE hc.HierarchyClassId
								  END,
		HierarchyClassName		= hc.HierarchyClassName,
		HierarchyLevel			= hp.hierarchyLevel,
		HierarchyParentClassId	= hc.ParentHierarchyClassId,
		null,
		null
	FROM
		@hierarchyClasses hc
		JOIN dbo.Hierarchy h on hc.HierarchyId = h.hierarchyID
		JOIN dbo.HierarchyPrototype hp on hp.hierarchyID = hc.HierarchyId
			and hp.hierarchyLevelName = hc.hierarchyLevelName
	WHERE h.HierarchyId in (@brandHierarchyId, @merchHierarchyId, @financialHierarchyId)
END
go