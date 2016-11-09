print N'Populating app.EventType...'

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
INSERT [app].[EventType] ([EventId], [EventName]) VALUES (7, N'Sub Team Update')
GO
INSERT [app].[EventType] ([EventId], [EventName]) VALUES (8, N'Item Sub Team Update')
GO
INSERT [app].[EventType] ([EventId], [EventName]) VALUES (9, N'Nutrition Update')
GO
INSERT [app].[EventType] ([EventId], [EventName]) VALUES (10, N'Nutrition Add')
GO
SET IDENTITY_INSERT [app].[EventType] OFF
GO