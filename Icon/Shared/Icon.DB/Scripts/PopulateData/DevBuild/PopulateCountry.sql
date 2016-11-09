print N'Populating Country...'

SET IDENTITY_INSERT [dbo].[Country] ON 

GO
INSERT [dbo].[Country] ([countryID], [countryCode], [countryName]) VALUES (1, N'USA', N'United States')
GO
INSERT [dbo].[Country] ([countryID], [countryCode], [countryName]) VALUES (2, N'CAN', N'Canada')
GO
INSERT [dbo].[Country] ([countryID], [countryCode], [countryName]) VALUES (3, N'GBR', N'Great Britain')
GO
SET IDENTITY_INSERT [dbo].[Country] OFF
GO