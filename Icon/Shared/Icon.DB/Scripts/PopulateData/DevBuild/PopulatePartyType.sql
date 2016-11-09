print N'Populating PartyType...'

SET IDENTITY_INSERT [dbo].[PartyType] ON 

GO
INSERT [dbo].[PartyType] ([partyTypeID], [partyTypeCode], [partyTypeDesc]) VALUES (1, N'ORG', N'Organization')
GO
SET IDENTITY_INSERT [dbo].[PartyType] OFF
GO