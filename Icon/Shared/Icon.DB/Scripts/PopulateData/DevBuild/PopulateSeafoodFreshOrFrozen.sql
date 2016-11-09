print N'Populating SeafoodFreshOrFrozen...'

SET IDENTITY_INSERT [dbo].[SeafoodFreshOrFrozen] ON 

GO
INSERT [dbo].[SeafoodFreshOrFrozen] ([SeafoodFreshOrFrozenId], [Description]) VALUES (1, N'Fresh')
GO
INSERT [dbo].[SeafoodFreshOrFrozen] ([SeafoodFreshOrFrozenId], [Description]) VALUES (2, N'Previously Frozen')
GO
INSERT [dbo].[SeafoodFreshOrFrozen] ([SeafoodFreshOrFrozenId], [Description]) VALUES (3, N'Frozen')
GO
SET IDENTITY_INSERT [dbo].[SeafoodFreshOrFrozen] OFF
GO