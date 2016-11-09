print N'Populating LocaleType...'

SET IDENTITY_INSERT [dbo].[LocaleType] ON 

GO
INSERT [dbo].[LocaleType] ([localeTypeID], [localeTypeCode], [localeTypeDesc]) VALUES (1, N'CH', N'Chain')
GO
INSERT [dbo].[LocaleType] ([localeTypeID], [localeTypeCode], [localeTypeDesc]) VALUES (2, N'RG', N'Region')
GO
INSERT [dbo].[LocaleType] ([localeTypeID], [localeTypeCode], [localeTypeDesc]) VALUES (3, N'MT', N'Metro')
GO
INSERT [dbo].[LocaleType] ([localeTypeID], [localeTypeCode], [localeTypeDesc]) VALUES (4, N'ST', N'Store')
GO
SET IDENTITY_INSERT [dbo].[LocaleType] OFF
GO