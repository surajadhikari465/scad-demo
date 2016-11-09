print N'Populating StoreGroupType...'

SET IDENTITY_INSERT [dbo].[StoreGroupType] ON 

GO
INSERT [dbo].[StoreGroupType] ([storeGroupTypeID], [storeGroupTypeDesc]) VALUES (1, N'IRMA Store Jurisdiction')
GO
SET IDENTITY_INSERT [dbo].[StoreGroupType] OFF
GO