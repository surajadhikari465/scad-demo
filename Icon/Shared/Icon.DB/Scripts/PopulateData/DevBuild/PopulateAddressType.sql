print N'Populating AddressType...'

SET IDENTITY_INSERT [dbo].[AddressType] ON 

GO
INSERT [dbo].[AddressType] ([addressTypeID], [addressTypeCode], [addressTypeDesc]) VALUES (1, N'PHY', N'Physical Address')
GO
SET IDENTITY_INSERT [dbo].[AddressType] OFF
GO