print N'Populating ScanCodeType...'

SET IDENTITY_INSERT [dbo].[ScanCodeType] ON 

GO
INSERT [dbo].[ScanCodeType] ([scanCodeTypeID], [scanCodeTypeDesc]) VALUES (1, N'UPC')
GO
INSERT [dbo].[ScanCodeType] ([scanCodeTypeID], [scanCodeTypeDesc]) VALUES (2, N'POS PLU')
GO
INSERT [dbo].[ScanCodeType] ([scanCodeTypeID], [scanCodeTypeDesc]) VALUES (3, N'Scale PLU')
GO
SET IDENTITY_INSERT [dbo].[ScanCodeType] OFF
GO