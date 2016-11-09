print N'Populating mammoth.EventType...'

SET IDENTITY_INSERT [mammoth].[EventType] ON 

GO
INSERT [mammoth].[EventType] ([EventTypeId], [Name]) VALUES (1, N'ProductAdd')
GO
INSERT [mammoth].[EventType] ([EventTypeId], [Name]) VALUES (2, N'ProductUpdate')
GO
INSERT [mammoth].[EventType] ([EventTypeId], [Name]) VALUES (3, N'HierarchyClassAdd')
GO
INSERT [mammoth].[EventType] ([EventTypeId], [Name]) VALUES (4, N'HierarchyClassUpdate')
GO
INSERT [mammoth].[EventType] ([EventTypeId], [Name]) VALUES (5, N'HierarchyClassDelete')
GO
SET IDENTITY_INSERT [mammoth].[EventType] OFF
GO