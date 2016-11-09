print N'Populating TraitGroup...'

SET IDENTITY_INSERT [dbo].[TraitGroup] ON 

GO
INSERT [dbo].[TraitGroup] ([traitGroupID], [traitGroupCode], [traitGroupDesc]) VALUES (1, N'IA', N'Item Attributes')
GO
INSERT [dbo].[TraitGroup] ([traitGroupID], [traitGroupCode], [traitGroupDesc]) VALUES (2, N'ILA', N'Item-Locale Attributes')
GO
INSERT [dbo].[TraitGroup] ([traitGroupID], [traitGroupCode], [traitGroupDesc]) VALUES (3, N'PA', N'Price Attributes')
GO
INSERT [dbo].[TraitGroup] ([traitGroupID], [traitGroupCode], [traitGroupDesc]) VALUES (4, N'ECA', N'eCommerce Attributes')
GO
INSERT [dbo].[TraitGroup] ([traitGroupID], [traitGroupCode], [traitGroupDesc]) VALUES (5, N'LT', N'Locale Traits')
GO
INSERT [dbo].[TraitGroup] ([traitGroupID], [traitGroupCode], [traitGroupDesc]) VALUES (6, N'HT', N'History Traits')
GO
INSERT [dbo].[TraitGroup] ([traitGroupID], [traitGroupCode], [traitGroupDesc]) VALUES (7, N'HYT', N'Hierarchy Class Traits')
GO
SET IDENTITY_INSERT [dbo].[TraitGroup] OFF
GO