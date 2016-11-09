USE [IconDev]
GO
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
INSERT [dbo].[TraitGroup] ([traitGroupID], [traitGroupCode], [traitGroupDesc]) VALUES (7, N'HYT', N'Hierarchy Traits')
GO
SET IDENTITY_INSERT [dbo].[TraitGroup] OFF
GO
SET IDENTITY_INSERT [dbo].[Trait] ON 

GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (0, N'PRD', N'^[a-zA-Z0-9_]*$', N'Product Description', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (1, N'POS', N'^[a-zA-Z0-9_]*$', N'POS Description', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (2, N'PKG', N'^[0-9]*\.?[0-9]+$', N'Package Unit', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (3, N'FSE', N'0|1', N'Food Stamp Eligible', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (4, N'SCT', N'0|1', N'POS Scale Tare', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (5, N'DPT', N'0|1', N'Department Sale', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (6, N'GFT', N'0|1', N'Gift Card', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (7, N'RSZ', N'^[0-9]*\.?[0-9]+$', N'Retail Size', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (8, N'RUM', N'^[0-9]*\.?[0-9]+$', N'Retail UOM', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (9, N'TMD', N'0|1', N'TM Discount Eligible', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (10, N'CSD', N'0|1', N'Case Discount Eligible', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (11, N'PRH', N'0|1', N'Prohibit Discount', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (12, N'AGE', N'^[0-9]*\.?[0-9]+$', N'Age Restrict', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (13, N'RCL', N'0|1', N'Recall', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (14, N'RES', N'^[0-9]*\.?[0-9]+$', N'Restricted Hours', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (15, N'SBW', N'0|1', N'Sold by Weight', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (16, N'FCT', N'0|1', N'Force Tare', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (17, N'QTY', N'0|1', N'Quantity Required', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (18, N'PRQ', N'0|1', N'Price Required', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (19, N'QPR', N'^[0-9]*\.?[0-9]+$', N'Quantity Prohibit', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (20, N'VV', N'0|1', N'Visual Verify', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (21, N'RS', N'0|1', N'Locked For Sale', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (22, N'NA', N'0|1', N'Authorized For Sale', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (23, N'DEL', N'0|1', N'Delete', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (24, N'LOC', N'^[0-9]*\.?[0-9]+$', N'Locale', 3)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (25, N'PRC', N'^[0-9]*\.?[0-9]+$', N'Price', 3)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (26, N'PM', N'^[0-9]*\.?[0-9]+$', N'Price Multiple', 3)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (27, N'ST', N'^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$', N'Price Start Date', 3)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (28, N'END', N'^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$', N'Price End Date', 3)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (29, N'SHT', N'^[a-zA-Z0-9_]*$', N'Short Romance', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (30, N'LNG', N'^[a-zA-Z0-9_]*$', N'Long Romance', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (31, N'GF', N'0|1', N'Gluten Free', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (32, N'PBC', N'0|1', N'Premium Body Care', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (33, N'EX', N'0|1', N'Exclusive', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (34, N'WT', N'0|1', N'Whole Trade', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (35, N'NGM', N'0|1', N'Non GMO', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (36, N'HSH', N'0|1', N'HSH', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (37, N'E2', N'0|1', N'E2', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (38, N'VGN', N'0|1', N'Vegan', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (39, N'VEG', N'0|1', N'Vegetarian', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (40, N'KSH', N'0|1', N'Kosher', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (41, N'ECO', N'Baseline|Premium|Ultra-Premium', N'ECO Scale Rating', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (42, N'OG', N'0|1', N'Organic', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (43, N'ABB', N'^[a-zA-Z0-9_]*$', N'Region Abbreviation', 5)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (44, N'BU', N'^[a-zA-Z0-9_]*$', N'PS Business Unit ID', 5)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (45, N'VER', N'^[0-9]*\.?[0-9]+$', N'ScanCode Version', 6)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (46, N'INS', N'^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$', N'Insert Date', 6)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (47, N'MOD', N'^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$', N'Modified Date', 6)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (48, N'VAL', N'^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$', N'Validation Date', 6)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (49, N'MFM', N'^[a-zA-Z0-9_]*$', N'Merch Fin Mapping', 6)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (50, N'GL', N'^[a-zA-Z0-9_]*$', N'GL Account', 6)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (51, N'ABR', N'^[a-zA-Z0-9_]*$', N'Tax Abbreviation', 6)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (52, N'SBC', N'^[a-zA-Z0-9_]*$', N'Sub Brick Code', 6)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (53, N'FIN', N'^[a-zA-Z0-9_]*$', N'Financial Hierarchy Code', 6)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (54, N'ESB', N'^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$', N'Sent To ESB', 7)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (56, N'SAB', N'^[a-zA-Z0-9_]*$', N'Store Abbreviation', 5)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (57, N'PN', N'^[0-9]{10,11}$', N'Phone Number', 5)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (58, N'CP', N'^[a-zA-Z0-9_]*$', N'Contact Person', 5)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (61, N'NM', N'Bottle Deposit|CRV|Coupon', N'Non Merchandise', 7)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (62, N'LSC', N'^[0-9]*\.?[0-9]+$', N'Linked Scan Code', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (64, N'USR', N'^[a-zA-Z0-9_]*$', N'Modified User', 6)
GO
SET IDENTITY_INSERT [dbo].[Trait] OFF
GO
