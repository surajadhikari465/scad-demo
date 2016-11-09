-- Task 3343
-- this should be done after the udm 3.0 upgrade
/****** Add applications to the App table ******/

DECLARE @WebApp NVARCHAR(255)
DECLARE @InterfaceController NVARCHAR(255)
DECLARE @EmsListenerService NVARCHAR(255)
DECLARE @IconService NVARCHAR(255)
DECLARE @ApiController NVARCHAR(255)

SET  @WebApp = 'Web App'
SET  @InterfaceController = 'Interface Controller'
SET  @EmsListenerService = 'Ems Listener Service'
SET  @IconService = 'Icon Service'
SET  @ApiController = 'API Controller'

IF (NOT EXISTS (SELECT [AppName] FROM [Icon].[app].[App] WHERE [AppName] = @WebApp))
BEGIN
	INSERT INTO [Icon].[app].[App] ([AppName])
	VALUES (@WebApp)
END

IF (NOT EXISTS (SELECT [AppName] FROM [Icon].[app].[App] WHERE [AppName] = @InterfaceController))
BEGIN
	INSERT INTO [Icon].[app].[App] ([AppName])
	VALUES (@InterfaceController)
END

IF (NOT EXISTS (SELECT [AppName] FROM [Icon].[app].[App] WHERE [AppName] = @EmsListenerService))
BEGIN
	INSERT INTO [Icon].[app].[App] ([AppName])
	VALUES (@EmsListenerService)
END

IF (NOT EXISTS (SELECT [AppName] FROM [Icon].[app].[App] WHERE [AppName] = @IconService))
BEGIN
	INSERT INTO [Icon].[app].[App] ([AppName])
	VALUES (@IconService)
END

IF (NOT EXISTS (SELECT [AppName] FROM [Icon].[app].[App] WHERE [AppName] = @ApiController))
BEGIN
	INSERT INTO [Icon].[app].[App] ([AppName])
	VALUES (@ApiController)
END

SET IDENTITY_INSERT [app].[EventType] ON 

GO
INSERT [app].[EventType] ([EventId], [EventName]) VALUES (1, N'New IRMA Item')
GO
INSERT [app].[EventType] ([EventId], [EventName]) VALUES (2, N'Item Update')
GO
INSERT [app].[EventType] ([EventId], [EventName]) VALUES (3, N'Item Validation')
GO
INSERT [app].[EventType] ([EventId], [EventName]) VALUES (4, N'Brand Name Update')
GO
INSERT [app].[EventType] ([EventId], [EventName]) VALUES (5, N'Tax Name Update')
GO
INSERT [app].[EventType] ([EventId], [EventName]) VALUES (6, N'New Tax Hierarchy')
GO
SET IDENTITY_INSERT [app].[EventType] OFF
GO
SET IDENTITY_INSERT [app].[MessageAction] ON 

GO
INSERT [app].[MessageAction] ([MessageActionId], [MessageActionName]) VALUES (1, N'AddOrUpdate')
GO
INSERT [app].[MessageAction] ([MessageActionId], [MessageActionName]) VALUES (2, N'Delete')
GO
SET IDENTITY_INSERT [app].[MessageAction] OFF
GO
SET IDENTITY_INSERT [app].[MessageStatus] ON 

GO
INSERT [app].[MessageStatus] ([MessageStatusId], [MessageStatusName]) VALUES (1, N'Ready')
GO
INSERT [app].[MessageStatus] ([MessageStatusId], [MessageStatusName]) VALUES (2, N'Sent')
GO
INSERT [app].[MessageStatus] ([MessageStatusId], [MessageStatusName]) VALUES (3, N'Failed')
GO
INSERT [app].[MessageStatus] ([MessageStatusId], [MessageStatusName]) VALUES (5, N'Associated')
GO
SET IDENTITY_INSERT [app].[MessageStatus] OFF
GO
SET IDENTITY_INSERT [app].[MessageType] ON 

GO
INSERT [app].[MessageType] ([MessageTypeId], [MessageTypeName]) VALUES (1, N'Locale')
GO
INSERT [app].[MessageType] ([MessageTypeId], [MessageTypeName]) VALUES (2, N'Hierarchy')
GO
INSERT [app].[MessageType] ([MessageTypeId], [MessageTypeName]) VALUES (3, N'Item Locale')
GO
INSERT [app].[MessageType] ([MessageTypeId], [MessageTypeName]) VALUES (4, N'Price')
GO
INSERT [app].[MessageType] ([MessageTypeId], [MessageTypeName]) VALUES (5, N'Department Sale')
GO
INSERT [app].[MessageType] ([MessageTypeId], [MessageTypeName]) VALUES (6, N'Product')
GO
INSERT [app].[MessageType] ([MessageTypeId], [MessageTypeName]) VALUES (7, N'Item Status')
GO
SET IDENTITY_INSERT [app].[MessageType] OFF
GO


