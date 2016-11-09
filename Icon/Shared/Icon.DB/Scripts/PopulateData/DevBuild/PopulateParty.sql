print N'Populating Party...'

SET IDENTITY_INSERT [dbo].[Party] ON 

GO
INSERT [dbo].[Party] ([partyID], [partyTypeID]) VALUES (1, 1)
GO
SET IDENTITY_INSERT [dbo].[Party] OFF
GO