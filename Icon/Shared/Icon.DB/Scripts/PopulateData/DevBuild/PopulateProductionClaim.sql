print N'Populating ProductionClaim...'

SET IDENTITY_INSERT [dbo].[ProductionClaim] ON 

GO
INSERT [dbo].[ProductionClaim] ([ProductionClaimId], [Description]) VALUES (1, N'Grass Fed')
GO
INSERT [dbo].[ProductionClaim] ([ProductionClaimId], [Description]) VALUES (2, N'Pasture Raised')
GO
INSERT [dbo].[ProductionClaim] ([ProductionClaimId], [Description]) VALUES (3, N'Free Range')
GO
INSERT [dbo].[ProductionClaim] ([ProductionClaimId], [Description]) VALUES (4, N'Dry Aged')
GO
INSERT [dbo].[ProductionClaim] ([ProductionClaimId], [Description]) VALUES (5, N'Air Chilled')
GO
INSERT [dbo].[ProductionClaim] ([ProductionClaimId], [Description]) VALUES (6, N'Made in House')
GO
SET IDENTITY_INSERT [dbo].[ProductionClaim] OFF
GO