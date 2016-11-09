print N'Populating MessageType...'

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
INSERT [app].[MessageType] ([MessageTypeId], [MessageTypeName]) VALUES (8, N'CCH Tax Update')
GO
INSERT [app].[MessageType] ([MessageTypeId], [MessageTypeName]) VALUES (9, N'Product Selection Group')
GO
INSERT [app].[MessageType] ([MessageTypeId], [MessageTypeName]) VALUES (10, N'eWIC')
GO
SET IDENTITY_INSERT [app].[MessageType] OFF
GO