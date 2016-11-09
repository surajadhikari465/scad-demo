print N'Populating OrganizationType...'

SET IDENTITY_INSERT [dbo].[OrganizationType] ON 

GO
INSERT [dbo].[OrganizationType] ([orgTypeID], [orgTypeCode], [orgTypeDesc]) VALUES (1, N'RT', N'Retailer')
GO
SET IDENTITY_INSERT [dbo].[OrganizationType] OFF
GO