print N'Populating Organization...'

SET IDENTITY_INSERT [dbo].[Organization] ON 

GO
INSERT [dbo].[Organization] ([orgPartyID], [orgTypeID], [parentOrgPartyID], [orgDesc]) VALUES (1, 1, NULL, N'Whole Foods')
GO
SET IDENTITY_INSERT [dbo].[Organization] OFF
GO