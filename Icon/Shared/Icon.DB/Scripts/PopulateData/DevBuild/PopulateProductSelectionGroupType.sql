print N'Populating ProductSelectionGroupType...'

SET IDENTITY_INSERT [app].[ProductSelectionGroupType] ON 

GO
INSERT [app].[ProductSelectionGroupType] ([ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName]) VALUES (1, N'Consumable')
GO
INSERT [app].[ProductSelectionGroupType] ([ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName]) VALUES (2, N'OnlineConsumable')
GO
SET IDENTITY_INSERT [app].[ProductSelectionGroupType] OFF
GO