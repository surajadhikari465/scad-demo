print N'Populating SeafoodCatchType...'

SET IDENTITY_INSERT [dbo].[SeafoodCatchType] ON 

GO
INSERT [dbo].[SeafoodCatchType] ([SeafoodCatchTypeId], [Description]) VALUES (1, N'Wild')
GO
INSERT [dbo].[SeafoodCatchType] ([SeafoodCatchTypeId], [Description]) VALUES (2, N'Farm Raised')
GO
SET IDENTITY_INSERT [dbo].[SeafoodCatchType] OFF
GO