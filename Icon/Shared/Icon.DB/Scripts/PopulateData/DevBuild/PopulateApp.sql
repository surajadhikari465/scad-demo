print N'Populating App...'

SET IDENTITY_INSERT [app].[App] ON 

GO
INSERT [app].[App] ([AppID], [AppName]) VALUES (1, N'Web App')
GO
INSERT [app].[App] ([AppID], [AppName]) VALUES (2, N'Interface Controller')
GO
INSERT [app].[App] ([AppID], [AppName]) VALUES (3, N'ESB Subscriber')
GO
INSERT [app].[App] ([AppID], [AppName]) VALUES (4, N'Icon Service')
GO
INSERT [app].[App] ([AppID], [AppName]) VALUES (5, N'API Controller')
GO
INSERT [app].[App] ([AppID], [AppName]) VALUES (6, N'POS Push Controller')
GO
INSERT [app].[App] ([AppID], [AppName]) VALUES (7, N'Global Controller')
GO
INSERT [app].[App] ([AppID], [AppName]) VALUES (8, N'Regional Controller')
GO
INSERT [app].[App] ([AppID], [AppName]) VALUES (9, N'Subteam Controller')
GO
INSERT [app].[App] ([AppID], [AppName]) VALUES (10, N'Icon Data Purge')
GO
INSERT [app].[App] ([AppID], [AppName]) VALUES (11, N'TLog Controller')
GO
INSERT [app].[App] ([AppID], [AppName]) VALUES (12, N'Nutrition Web API')
GO
INSERT [app].[App] ([AppID], [AppName]) VALUES (13, N'Vim Locale Controller')
GO
SET IDENTITY_INSERT [app].[App] OFF
GO