print N'Populating Hierarchy...'

SET IDENTITY_INSERT [dbo].[Hierarchy] ON 

GO
INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName]) VALUES (1, N'Merchandise')
GO
INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName]) VALUES (2, N'Brands')
GO
INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName]) VALUES (3, N'Tax')
GO
INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName]) VALUES (4, N'Browsing')
GO
INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName]) VALUES (5, N'Financial')
GO
INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName]) VALUES (6, N'National')
GO
INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName]) VALUES (7, N'Certification Agency Management')
GO
SET IDENTITY_INSERT [dbo].[Hierarchy] OFF
GO