/****** Add the message types for message history purposes ******/

DECLARE @locales NVARCHAR(255)
DECLARE @hierarchies NVARCHAR(255)
DECLARE @itemLocales NVARCHAR(255)
DECLARE @prices NVARCHAR(255)
DECLARE @departmentSales NVARCHAR(255)
DECLARE @items NVARCHAR(255)
DECLARE @cchTaxUpdate NVARCHAR(255)

SET  @locales = 'Locale'
SET	 @hierarchies = 'Hierarchy'
SET  @itemLocales = 'Item Locale'
SET  @prices = 'Price'
SET  @departmentSales = 'Department Sale'
SET  @items = 'Product'
SET  @cchTaxUpdate = 'CCH Tax Update'

IF (NOT EXISTS (SELECT [MessageTypeName] FROM [Icon].[app].[MessageType] WHERE [MessageTypeName] = @locales))
BEGIN
	INSERT INTO [Icon].[app].[MessageType] ([MessageTypeName])
	VALUES (@locales)
END

IF (NOT EXISTS (SELECT [MessageTypeName] FROM [Icon].[app].[MessageType] WHERE [MessageTypeName] = @hierarchies))
BEGIN
	INSERT INTO [Icon].[app].[MessageType] ([MessageTypeName])
	VALUES (@hierarchies)
END

IF (NOT EXISTS (SELECT [MessageTypeName] FROM [Icon].[app].[MessageType] WHERE [MessageTypeName] = @itemLocales))
BEGIN
	INSERT INTO [Icon].[app].[MessageType] ([MessageTypeName])
	VALUES (@itemLocales)
END

IF (NOT EXISTS (SELECT [MessageTypeName] FROM [Icon].[app].[MessageType] WHERE [MessageTypeName] = @prices))
BEGIN
	INSERT INTO [Icon].[app].[MessageType] ([MessageTypeName])
	VALUES (@prices)
END

IF (NOT EXISTS (SELECT [MessageTypeName] FROM [Icon].[app].[MessageType] WHERE [MessageTypeName] = @departmentSales))
BEGIN
	INSERT INTO [Icon].[app].[MessageType] ([MessageTypeName])
	VALUES (@departmentSales)
END

IF (NOT EXISTS (SELECT [MessageTypeName] FROM [Icon].[app].[MessageType] WHERE [MessageTypeName] = @items))
BEGIN
	INSERT INTO [Icon].[app].[MessageType] ([MessageTypeName])
	VALUES (@items)
END

IF (NOT EXISTS (SELECT [MessageTypeName] FROM [Icon].[app].[MessageType] WHERE [MessageTypeName] = @cchTaxUpdate))
BEGIN
	INSERT INTO [Icon].[app].[MessageType] ([MessageTypeName])
	VALUES (@cchTaxUpdate)
END