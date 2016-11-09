print N'Populating EventType...'

SET IDENTITY_INSERT [vim].[EventType] ON 

GO
INSERT [vim].[EventType] ([EventTypeId], [Name]) VALUES (1, N'Locale Add')
GO
INSERT [vim].[EventType] ([EventTypeId], [Name]) VALUES (2, N'Locale Update')
GO
INSERT [vim].[EventType] ([EventTypeId], [Name]) VALUES (3, N'NationalClassAdd')
GO
INSERT [vim].[EventType] ([EventTypeId], [Name]) VALUES (4, N'NationalClassUpdate')
GO
INSERT [vim].[EventType] ([EventTypeId], [Name]) VALUES (5, N'NationalClassDelete')
GO
SET IDENTITY_INSERT [vim].[EventType] OFF
GO