print N'Populating ItemType...'

SET IDENTITY_INSERT [dbo].[ItemType] ON 

GO
INSERT [dbo].[ItemType] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (1, N'RTL', N'Retail Sale')
GO
INSERT [dbo].[ItemType] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (2, N'DEP', N'Deposit')
GO
INSERT [dbo].[ItemType] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (3, N'TAR', N'Tare')
GO
INSERT [dbo].[ItemType] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (4, N'RTN', N'Return')
GO
INSERT [dbo].[ItemType] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (5, N'CPN', N'Coupon')
GO
INSERT [dbo].[ItemType] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (6, N'NRT', N'Non-Retail')
GO
INSERT [dbo].[ItemType] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (7, N'FEE', N'Fee')
GO
SET IDENTITY_INSERT [dbo].[ItemType] OFF
GO