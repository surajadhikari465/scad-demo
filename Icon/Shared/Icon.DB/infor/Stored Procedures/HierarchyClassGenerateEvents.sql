CREATE PROCEDURE [infor].[HierarchyClassGenerateEvents]
	@hierarchyClasses infor.HierarchyClassType READONLY,
	@hierarchyClassTraits infor.HierarchyClassTraitType READONLY,
	@regions infor.RegionCodeList READONLY
AS
BEGIN
	--Generate Hierarchy Class events to IRMA
	DECLARE @brandHierarchyId INT			= (SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Brands'),
			@nationalHierarchyId INT		= (SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'National'),
			@taxHierarchyId INT				= (SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Tax'),
			@brandUpdateEventTypeId INT		= (SELECT EventId FROM app.EventType WHERE EventName = 'Brand Name Update'),
			@brandDeleteEventTypeId INT		= (SELECT EventId FROM app.EventType WHERE EventName = 'Brand Delete'),
			@nationalUpdateEventTypeId INT	= (SELECT EventId FROM app.EventType WHERE EventName = 'National Class Update'),
			@nationalDeleteEventTypeId INT	= (SELECT EventId FROM app.EventType WHERE EventName = 'National Class Delete'),
			@taxUpdateEventTypeId INT		= (SELECT EventId FROM app.EventType WHERE EventName = 'Tax Name Update'),
			@addOrUpdateMessageActionId INT	= (SELECT MessageActionId FROM app.MessageAction WHERE MessageActionName = 'AddOrUpdate'),
			@deleteMessageActionId INT		= (SELECT MessageActionId FROM app.MessageAction WHERE MessageActionName = 'Delete')

	INSERT INTO app.EventQueue(EventId, EventMessage, EventReferenceId, RegionCode)
	SELECT 
		CASE 
			WHEN HierarchyId = @brandHierarchyId AND ActionId = @addOrUpdateMessageActionId THEN @brandUpdateEventTypeId
			WHEN HierarchyId = @brandHierarchyId AND ActionId = @deleteMessageActionId THEN @brandDeleteEventTypeId
			WHEN HierarchyId = @nationalHierarchyId AND ActionId = @addOrUpdateMessageActionId THEN @nationalUpdateEventTypeId
			WHEN HierarchyId = @nationalHierarchyId AND ActionId = @deleteMessageActionId THEN @nationalDeleteEventTypeId
			WHEN HierarchyId = @taxHierarchyId AND ActionId = @addOrUpdateMessageActionId THEN @taxUpdateEventTypeId
		END,
		HierarchyClassName,
		HierarchyClassId,
		RegionCode
	FROM @hierarchyClasses
	CROSS JOIN @regions
	WHERE ActionId IN (@addOrUpdateMessageActionId, @deleteMessageActionId)
		AND HierarchyId IN (@brandHierarchyId, @nationalHierarchyId)
END
GO