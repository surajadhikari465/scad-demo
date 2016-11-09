print N'Populating MilkType...'

SET IDENTITY_INSERT [dbo].[MilkType] ON 

GO
INSERT [dbo].[MilkType] ([MilkTypeId], [Description]) VALUES (1, N'Buffalo Milk')
GO
INSERT [dbo].[MilkType] ([MilkTypeId], [Description]) VALUES (2, N'Cow Milk')
GO
INSERT [dbo].[MilkType] ([MilkTypeId], [Description]) VALUES (3, N'Goat/Sheep Milk')
GO
INSERT [dbo].[MilkType] ([MilkTypeId], [Description]) VALUES (4, N'Goat Milk')
GO
INSERT [dbo].[MilkType] ([MilkTypeId], [Description]) VALUES (5, N'Cow/Sheep Milk')
GO
INSERT [dbo].[MilkType] ([MilkTypeId], [Description]) VALUES (6, N'Cow/Goat Milk')
GO
INSERT [dbo].[MilkType] ([MilkTypeId], [Description]) VALUES (7, N'Cow/Goat/Sheep Milk')
GO
INSERT [dbo].[MilkType] ([MilkTypeId], [Description]) VALUES (8, N'Sheep Milk')
GO
INSERT [dbo].[MilkType] ([MilkTypeId], [Description]) VALUES (9, N'Yak Milk')
GO
SET IDENTITY_INSERT [dbo].[MilkType] OFF
GO