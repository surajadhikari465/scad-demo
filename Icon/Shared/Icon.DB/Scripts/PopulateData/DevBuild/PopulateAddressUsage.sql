print N'Populating AddressUsage...'

SET IDENTITY_INSERT [dbo].[AddressUsage] ON 

GO
INSERT [dbo].[AddressUsage] ([addressUsageID], [addressUsageCode], [addressUsageDesc]) VALUES (1, N'SHP', N'Shipping')
GO
SET IDENTITY_INSERT [dbo].[AddressUsage] OFF
GO