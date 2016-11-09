print N'Populating ItemPriceType...'

SET IDENTITY_INSERT [dbo].[ItemPriceType] ON 

GO
INSERT [dbo].[ItemPriceType] ([itemPriceTypeID], [itemPriceTypeCode], [itemPriceTypeDesc]) VALUES (1, N'REG', N'Regular Price')
GO
INSERT [dbo].[ItemPriceType] ([itemPriceTypeID], [itemPriceTypeCode], [itemPriceTypeDesc]) VALUES (2, N'TPR', N'Temporary Price Reduction')
GO
SET IDENTITY_INSERT [dbo].[ItemPriceType] OFF
GO