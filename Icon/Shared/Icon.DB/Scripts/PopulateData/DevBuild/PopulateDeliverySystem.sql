print N'Populating DeliverySystem...'

SET IDENTITY_INSERT [dbo].[DeliverySystem] ON 

GO
INSERT [dbo].[DeliverySystem] ([DeliverySystemID], [DeliverySystemCode], [DeliverySystemName]) VALUES (1, N'CAP', N'Capsule')
GO
INSERT [dbo].[DeliverySystem] ([DeliverySystemID], [DeliverySystemCode], [DeliverySystemName]) VALUES (2, N'CHW', N'Chewable')
GO
INSERT [dbo].[DeliverySystem] ([DeliverySystemID], [DeliverySystemCode], [DeliverySystemName]) VALUES (3, N'LZ ', N'Lozenge')
GO
INSERT [dbo].[DeliverySystem] ([DeliverySystemID], [DeliverySystemCode], [DeliverySystemName]) VALUES (4, N'SG', N'Soft Gel')
GO
INSERT [dbo].[DeliverySystem] ([DeliverySystemID], [DeliverySystemCode], [DeliverySystemName]) VALUES (5, N'TB', N'Tablet')
GO
INSERT [dbo].[DeliverySystem] ([DeliverySystemID], [DeliverySystemCode], [DeliverySystemName]) VALUES (6, N'VC', N'Vegicap')
GO
INSERT [dbo].[DeliverySystem] ([DeliverySystemID], [DeliverySystemCode], [DeliverySystemName]) VALUES (7, N'VS', N'Vegetarian Soft Gel')
GO
SET IDENTITY_INSERT [dbo].[DeliverySystem] OFF
GO