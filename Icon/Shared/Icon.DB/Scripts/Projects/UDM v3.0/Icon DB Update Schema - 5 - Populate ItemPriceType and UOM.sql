SET IDENTITY_INSERT [dbo].[ItemPriceType] ON 
GO
INSERT [dbo].[ItemPriceType] ([itemPriceTypeID], [itemPriceTypeCode], [itemPriceTypeDesc]) VALUES (1, N'REG', N'Regular Price')
GO
INSERT [dbo].[ItemPriceType] ([itemPriceTypeID], [itemPriceTypeCode], [itemPriceTypeDesc]) VALUES (2, N'TPR', N'Temporary Price Reduction')
GO
SET IDENTITY_INSERT [dbo].[ItemPriceType] OFF
GO


INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (1, N'EA', N'EACH')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (2, N'LB', N'POUND')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (3, N'PH', N'BY COUNT')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (4, N'AV', N'CAPLETS')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (5, N'CA', N'CASE')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (6, N'CT', N'COUNT')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (7, N'DZ', N'DOZENS')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (8, N'FT', N'FEET')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (9, N'FO', N'FLUID OUNCES')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (10, N'GA', N'GALLONS')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (11, N'GR', N'GRAMS')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (12, N'IN', N'INCHES')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (13, N'KG', N'KILOGRAM')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (14, N'LT', N'LITERS')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (15, N'MT', N'METERS')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (16, N'ML', N'MILLILITERS')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (17, N'MM', N'MILLIMETERS')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (18, N'OZ', N'OUNCES')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (19, N'PT', N'PINTS')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (20, N'QT', N'QUARTS')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (21, N'SH', N'SHEETS')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (22, N'SZ', N'SIZE')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (23, N'U2', N'TABLET')
GO
INSERT [dbo].[UOM] ([uomID], [uomCode], [uomName]) VALUES (24, N'YD', N'YARDS')
GO