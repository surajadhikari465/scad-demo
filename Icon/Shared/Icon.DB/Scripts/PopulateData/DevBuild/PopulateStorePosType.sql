print N'Populating StorePosType...'

SET IDENTITY_INSERT [vim].[StorePosType] ON 

GO
INSERT [vim].[StorePosType] ([StorePosTypeId], [Name]) VALUES (1, N'xNEW')
GO
INSERT [vim].[StorePosType] ([StorePosTypeId], [Name]) VALUES (2, N'IBM')
GO
INSERT [vim].[StorePosType] ([StorePosTypeId], [Name]) VALUES (3, N'RTX')
GO
INSERT [vim].[StorePosType] ([StorePosTypeId], [Name]) VALUES (4, N'ACS')
GO
INSERT [vim].[StorePosType] ([StorePosTypeId], [Name]) VALUES (5, N'BEAN')
GO
INSERT [vim].[StorePosType] ([StorePosTypeId], [Name]) VALUES (6, N'CCP')
GO
INSERT [vim].[StorePosType] ([StorePosTypeId], [Name]) VALUES (7, N'CLOSED')
GO
SET IDENTITY_INSERT [vim].[StorePosType] OFF
GO