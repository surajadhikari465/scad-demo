truncate table app.MessageQueueProductSelectionGroup
truncate table app.ProductSelectionGroup

SET IDENTITY_INSERT [app].[MessageQueueProductSelectionGroup] ON 

GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (1, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 1, N'005_GC_ACTIVATION_VANTIV_BARCODE', 1, N'Consumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (2, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 3, N'010_GC_ACTIVATION_VANTIV_MAGSTRIPE', 1, N'Consumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (4, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 4, N'020_GC_ACTIVATION_VALUE_LINK_MAGSTRIPE', 1, N'Consumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (5, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 5, N'025_GC_TOPUP_VANTIV_MAGSTRIPE', 1, N'Consumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (6, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 6, N'030_GC_TOPUP_VALUE_LINK_MAGSTRIPE', 1, N'Consumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (7, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 7, N'035_GC_CASHOUT_All', 1, N'Consumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (8, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 8, N'040_GC_BLACKHAWK', 1, N'Consumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (9, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 9, N'Bag_Fee', 1, N'Consumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (10, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 10, N'DonationsGroup', 1, N'Consumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (11, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 11, N'DonationsGroup', 1, N'Consumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (12, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 12, N'DonationsGroup', 1, N'Consumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (13, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 13, N'DonationsGroup', 1, N'Consumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (14, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 14, N'CaseDiscountEligible', 1, N'Consumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (15, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 15, N'Food_Stamp', 1, N'Consumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (16, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 16, N'DateRestriction', 1, N'Consumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (17, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 17, N'Prohibit_Case_Discount', 1, N'Consumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (18, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 18, N'Prohibit_Discount_Items', 1, N'Consumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (19, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 19, N'Prohibit_TM_Discount', 1, N'Consumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (20, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 20, N'Restrict 18', 1, N'Consumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (21, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 21, N'Restrict 21', 1, N'Consumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (22, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 22, N'Blackhawk Gift Card Closed', 2, N'OnlineConsumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (23, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 23, N'Blackhawk Gift Card Open', 2, N'OnlineConsumable', NULL, NULL)
GO
INSERT [app].[MessageQueueProductSelectionGroup] ([MessageQueueId], [InsertDate], [MessageTypeId], [MessageStatusId], [MessageHistoryId], [MessageActionId], [ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [ProductSelectionGroupTypeName], [InProcessBy], [ProcessedDate]) VALUES (24, CAST(GETDATE() AS DateTime2), 9, 1, NULL, 1, 24, N'SuppressPrice', 1, N'Consumable', NULL, NULL)
GO
SET IDENTITY_INSERT [app].[MessageQueueProductSelectionGroup] OFF
GO
SET IDENTITY_INSERT [app].[ProductSelectionGroup] ON 

GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (1, N'005_GC_ACTIVATION_VANTIV_BARCODE', 1, NULL, NULL, 84648)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (2, N'005_GC_ACTIVATION_VANTIV_BARCODE', 1, NULL, NULL, 84650)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (3, N'010_GC_ACTIVATION_VANTIV_MAGSTRIPE', 1, NULL, NULL, 84651)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (4, N'020_GC_ACTIVATION_VALUE_LINK_MAGSTRIPE', 1, NULL, NULL, 84647)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (5, N'025_GC_TOPUP_VANTIV_MAGSTRIPE', 1, NULL, NULL, 85716)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (6, N'030_GC_TOPUP_VALUE_LINK_MAGSTRIPE', 1, NULL, NULL, 85715)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (7, N'035_GC_CASHOUT_All', 1, NULL, NULL, 85714)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (9, N'Bag_Fee', 1, NULL, NULL, 84533)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (10, N'DonationsGroup', 1, NULL, NULL, 84805)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (11, N'DonationsGroup', 1, NULL, NULL, 84808)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (12, N'DonationsGroup', 1, NULL, NULL, 87270)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (13, N'DonationsGroup', 1, NULL, NULL, 87272)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (14, N'CaseDiscountEligible', 1, 10, N'1', NULL)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (15, N'Food_Stamp', 1, 3, N'1', NULL)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (16, N'DateRestriction', 1, 14, N'1', NULL)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (17, N'Prohibit_Case_Discount', 1, 10, N'0', NULL)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (18, N'Prohibit_Discount_Items', 1, 11, N'1', NULL)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (19, N'Prohibit_TM_Discount', 1, 9, N'0', NULL)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (20, N'Restrict 18', 1, 12, N'1', NULL)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (21, N'Restrict 21', 1, 12, N'2', NULL)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (22, N'Blackhawk Gift Card Closed', 2, NULL, NULL, 86393)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (23, N'Blackhawk Gift Card Open', 2, NULL, NULL, 98628)
GO
INSERT [app].[ProductSelectionGroup] ([ProductSelectionGroupId], [ProductSelectionGroupName], [ProductSelectionGroupTypeId], [TraitId], [TraitValue], [MerchandiseHierarchyClassId]) VALUES (24, N'SuppressPrice', 1, NULL, NULL, 84517)
GO
SET IDENTITY_INSERT [app].[ProductSelectionGroup] OFF
GO
