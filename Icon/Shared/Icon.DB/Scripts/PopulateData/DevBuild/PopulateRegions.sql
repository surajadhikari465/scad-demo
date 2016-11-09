print N'Populating Regions...'

SET IDENTITY_INSERT [app].[Regions] ON 

GO
INSERT [app].[Regions] ([RegionId], [RegionCode], [RegionName]) VALUES (1, N'RM', N'RM')
GO
INSERT [app].[Regions] ([RegionId], [RegionCode], [RegionName]) VALUES (2, N'FL', N'FL')
GO
INSERT [app].[Regions] ([RegionId], [RegionCode], [RegionName]) VALUES (3, N'MA', N'MA')
GO
INSERT [app].[Regions] ([RegionId], [RegionCode], [RegionName]) VALUES (4, N'MW', N'MW')
GO
INSERT [app].[Regions] ([RegionId], [RegionCode], [RegionName]) VALUES (5, N'NA', N'NA')
GO
INSERT [app].[Regions] ([RegionId], [RegionCode], [RegionName]) VALUES (6, N'NC', N'NC')
GO
INSERT [app].[Regions] ([RegionId], [RegionCode], [RegionName]) VALUES (7, N'SO', N'SO')
GO
INSERT [app].[Regions] ([RegionId], [RegionCode], [RegionName]) VALUES (8, N'NE', N'NE')
GO
INSERT [app].[Regions] ([RegionId], [RegionCode], [RegionName]) VALUES (9, N'PN', N'PN')
GO
INSERT [app].[Regions] ([RegionId], [RegionCode], [RegionName]) VALUES (10, N'SP', N'SP')
GO
INSERT [app].[Regions] ([RegionId], [RegionCode], [RegionName]) VALUES (11, N'SW', N'SW')
GO
INSERT [app].[Regions] ([RegionId], [RegionCode], [RegionName]) VALUES (12, N'UK', N'UK')
GO
SET IDENTITY_INSERT [app].[Regions] OFF
GO