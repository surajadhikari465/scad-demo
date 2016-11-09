/****** Add the event types for event queueing purposes ******/

DECLARE @newIrmaItem NVARCHAR(255)
DECLARE @itemUpdate NVARCHAR(255)
DECLARE @itemValidation NVARCHAR(255)
DECLARE @brandNameUpdate NVARCHAR(255)
DECLARE @taxNameUpdate NVARCHAR(255)
DECLARE @newTaxHierarchy NVARCHAR(255)

SET  @newIrmaItem = 'New IRMA Item'
SET	 @itemUpdate = 'Item Update'
SET  @itemValidation = 'Item Validation'
SET  @brandNameUpdate = 'Brand Name Update'
SET  @taxNameUpdate = 'Tax Name Update'
SET  @newTaxHierarchy = 'New Tax Hierarchy'

IF (NOT EXISTS (SELECT [EventName] FROM [Icon].[app].[EventType] WHERE [EventName] = @newIrmaItem))
BEGIN
	INSERT INTO [Icon].[app].[EventType] ([EventName])
	VALUES (@newIrmaItem)
END

IF (NOT EXISTS (SELECT [EventName] FROM [Icon].[app].[EventType] WHERE [EventName] = @itemUpdate))
BEGIN
	INSERT INTO [Icon].[app].[EventType] ([EventName])
	VALUES (@itemUpdate)
END

IF (NOT EXISTS (SELECT [EventName] FROM [Icon].[app].[EventType] WHERE [EventName] = @itemValidation))
BEGIN
	INSERT INTO [Icon].[app].[EventType] ([EventName])
	VALUES (@itemValidation)
END

IF (NOT EXISTS (SELECT [EventName] FROM [Icon].[app].[EventType] WHERE [EventName] = @brandNameUpdate))
BEGIN
	INSERT INTO [Icon].[app].[EventType] ([EventName])
	VALUES (@brandNameUpdate)
END

IF (NOT EXISTS (SELECT [EventName] FROM [Icon].[app].[EventType] WHERE [EventName] = @taxNameUpdate))
BEGIN
	INSERT INTO [Icon].[app].[EventType] ([EventName])
	VALUES (@taxNameUpdate)
END

IF (NOT EXISTS (SELECT [EventName] FROM [Icon].[app].[EventType] WHERE [EventName] = @newTaxHierarchy))
BEGIN
	INSERT INTO [Icon].[app].[EventType] ([EventName])
	VALUES (@newTaxHierarchy)
END
