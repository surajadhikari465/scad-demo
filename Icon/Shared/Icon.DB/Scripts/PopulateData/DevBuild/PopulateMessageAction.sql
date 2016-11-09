print N'Populating MessageAction...'

SET IDENTITY_INSERT [app].[MessageAction] ON 

GO
INSERT [app].[MessageAction] ([MessageActionId], [MessageActionName]) VALUES (1, N'AddOrUpdate')
GO
INSERT [app].[MessageAction] ([MessageActionId], [MessageActionName]) VALUES (2, N'Delete')
GO
SET IDENTITY_INSERT [app].[MessageAction] OFF
GO