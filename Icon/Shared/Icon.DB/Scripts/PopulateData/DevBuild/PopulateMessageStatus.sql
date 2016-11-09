print N'Populating MessageStatus...'

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
INSERT [app].[MessageStatus] ([MessageStatusId], [MessageStatusName]) VALUES (6, N'Staged')
GO
INSERT [app].[MessageStatus] ([MessageStatusId], [MessageStatusName]) VALUES (7, N'Consumed')
GO
SET IDENTITY_INSERT [app].[MessageStatus] OFF
GO