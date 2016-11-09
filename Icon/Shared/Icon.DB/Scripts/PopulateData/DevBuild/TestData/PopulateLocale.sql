print N'Populating Locale...'

SET IDENTITY_INSERT [dbo].[Locale] ON 

GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1, 1, N'Whole Foods', CAST(N'1980-09-20' AS Date), NULL, 1, NULL)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (2, 1, N'Florida', NULL, NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (3, 1, N'Mid Atlantic', NULL, NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (4, 1, N'Mid West', NULL, NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (5, 1, N'North Atlantic', NULL, NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (6, 1, N'Northern California', NULL, NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (7, 1, N'North East', NULL, NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (8, 1, N'Pacific Northwest', NULL, NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (9, 1, N'Rocky Mountain', NULL, NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (10, 1, N'South', NULL, NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (11, 1, N'Southern Pacific', NULL, NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (12, 1, N'Southwest', NULL, NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (13, 1, N'United Kingdom', NULL, NULL, 2, 1)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (14, 1, N'MET_FL', CAST(N'1980-09-21' AS Date), NULL, 3, 2)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (15, 1, N'MET_DC', CAST(N'1980-09-21' AS Date), NULL, 3, 3)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (16, 1, N'MET_KY', CAST(N'1980-09-21' AS Date), NULL, 3, 3)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (17, 1, N'MET_MD', CAST(N'1980-09-21' AS Date), NULL, 3, 3)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (18, 1, N'MET_NJ', CAST(N'1980-09-21' AS Date), NULL, 3, 3)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (19, 1, N'MET_OH', CAST(N'1980-09-21' AS Date), NULL, 3, 3)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (20, 1, N'MET_PA', CAST(N'1980-09-21' AS Date), NULL, 3, 3)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (21, 1, N'MET_VA', CAST(N'1980-09-21' AS Date), NULL, 3, 3)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (22, 1, N'MET_CHI', CAST(N'1980-09-21' AS Date), NULL, 3, 4)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (23, 1, N'MET_IA', CAST(N'1980-09-21' AS Date), NULL, 3, 4)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (24, 1, N'MET_IL', CAST(N'1980-09-21' AS Date), NULL, 3, 4)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (25, 1, N'MET_IN', CAST(N'1980-09-21' AS Date), NULL, 3, 4)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (26, 1, N'MET_MI', CAST(N'1980-09-21' AS Date), NULL, 3, 4)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (27, 1, N'MET_MN', CAST(N'1980-09-21' AS Date), NULL, 3, 4)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (28, 1, N'MET_MO', CAST(N'1980-09-21' AS Date), NULL, 3, 4)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (29, 1, N'MET_NEB', CAST(N'1980-09-21' AS Date), NULL, 3, 4)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (30, 1, N'MET_ON', CAST(N'1980-09-21' AS Date), NULL, 3, 4)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (31, 1, N'MET_WI', CAST(N'1980-09-21' AS Date), NULL, 3, 4)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (32, 1, N'MET_BOS', CAST(N'1980-09-21' AS Date), NULL, 3, 5)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (33, 1, N'MET_NA_OTHER', CAST(N'1980-09-21' AS Date), NULL, 3, 5)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (34, 1, N'MET_EBY', CAST(N'1980-09-21' AS Date), NULL, 3, 6)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (35, 1, N'MET_FRS', CAST(N'1980-09-21' AS Date), NULL, 3, 6)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (36, 1, N'MET_MRN', CAST(N'1980-09-21' AS Date), NULL, 3, 6)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (37, 1, N'MET_NPA', CAST(N'1980-09-21' AS Date), NULL, 3, 6)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (38, 1, N'MET_PEN', CAST(N'1980-09-21' AS Date), NULL, 3, 6)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (39, 1, N'MET_REN', CAST(N'1980-09-21' AS Date), NULL, 3, 6)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (40, 1, N'MET_SAC', CAST(N'1980-09-21' AS Date), NULL, 3, 6)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (41, 1, N'MET_SFO', CAST(N'1980-09-21' AS Date), NULL, 3, 6)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (42, 1, N'MET_SJC', CAST(N'1980-09-21' AS Date), NULL, 3, 6)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (43, 1, N'MET_STZ', CAST(N'1980-09-21' AS Date), NULL, 3, 6)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (44, 1, N'MET_LI', CAST(N'1980-09-21' AS Date), NULL, 3, 7)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (45, 1, N'MET_NE_CT', CAST(N'1980-09-21' AS Date), NULL, 3, 7)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (46, 1, N'MET_NE_NJ', CAST(N'1980-09-21' AS Date), NULL, 3, 7)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (47, 1, N'MET_NYC', CAST(N'1980-09-21' AS Date), NULL, 3, 7)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (48, 1, N'MET_UP_NY', CAST(N'1980-09-21' AS Date), NULL, 3, 7)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (49, 1, N'MET_WESTCH', CAST(N'1980-09-21' AS Date), NULL, 3, 7)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (50, 1, N'MET_CP', CAST(N'1980-09-21' AS Date), NULL, 3, 8)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (51, 1, N'MET_OR', CAST(N'1980-09-21' AS Date), NULL, 3, 8)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (52, 1, N'MET_WA', CAST(N'1980-09-21' AS Date), NULL, 3, 8)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (53, 1, N'MET_CO', CAST(N'1980-09-21' AS Date), NULL, 3, 9)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (54, 1, N'MET_ID', CAST(N'1980-09-21' AS Date), NULL, 3, 9)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (55, 1, N'MET_KS', CAST(N'1980-09-21' AS Date), NULL, 3, 9)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (56, 1, N'MET_NM', CAST(N'1980-09-21' AS Date), NULL, 3, 9)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (57, 1, N'MET_UT', CAST(N'1980-09-21' AS Date), NULL, 3, 9)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (58, 1, N'MET_AL', CAST(N'1980-09-21' AS Date), NULL, 3, 10)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (59, 1, N'MET_GA', CAST(N'1980-09-21' AS Date), NULL, 3, 10)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (60, 1, N'MET_MS', CAST(N'1980-09-21' AS Date), NULL, 3, 10)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (61, 1, N'MET_NC', CAST(N'1980-09-21' AS Date), NULL, 3, 10)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (62, 1, N'MET_SC', CAST(N'1980-09-21' AS Date), NULL, 3, 10)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (63, 1, N'MET_TN', CAST(N'1980-09-21' AS Date), NULL, 3, 10)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (64, 1, N'MET_AZ', CAST(N'1980-09-21' AS Date), NULL, 3, 11)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (65, 1, N'MET_HI', CAST(N'1980-09-21' AS Date), NULL, 3, 11)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (66, 1, N'MET_LA_EAST', CAST(N'1980-09-21' AS Date), NULL, 3, 11)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (67, 1, N'MET_LA_WEST', CAST(N'1980-09-21' AS Date), NULL, 3, 11)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (68, 1, N'MET_NV', CAST(N'1980-09-21' AS Date), NULL, 3, 11)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (69, 1, N'MET_OC', CAST(N'1980-09-21' AS Date), NULL, 3, 11)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (70, 1, N'MET_SD', CAST(N'1980-09-21' AS Date), NULL, 3, 11)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (71, 1, N'MET_AUS', CAST(N'1980-09-21' AS Date), NULL, 3, 12)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (72, 1, N'MET_BR', CAST(N'1980-09-21' AS Date), NULL, 3, 12)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (73, 1, N'MET_DFW', CAST(N'1980-09-21' AS Date), NULL, 3, 12)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (74, 1, N'MET_HOU', CAST(N'1980-09-21' AS Date), NULL, 3, 12)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (75, 1, N'MET_LFY', CAST(N'1980-09-21' AS Date), NULL, 3, 12)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (76, 1, N'MET_LR', CAST(N'1980-09-21' AS Date), NULL, 3, 12)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (77, 1, N'MET_NO', CAST(N'1980-09-21' AS Date), NULL, 3, 12)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (78, 1, N'MET_OK', CAST(N'1980-09-21' AS Date), NULL, 3, 12)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (79, 1, N'MET_SA', CAST(N'1980-09-21' AS Date), NULL, 3, 12)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (80, 1, N'MET_SRV', CAST(N'1980-09-21' AS Date), NULL, 3, 12)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (81, 1, N'MET_ENG', CAST(N'1980-09-21' AS Date), NULL, 3, 13)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (82, 1, N'MET_LDN', CAST(N'1980-09-21' AS Date), NULL, 3, 13)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (83, 1, N'MET_SCT', CAST(N'1980-09-21' AS Date), NULL, 3, 13)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (555, 1, N'Boca Raton', CAST(N'2001-04-25' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (556, 1, N'Orlando', CAST(N'2008-06-25' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (557, 1, N'Biscayne', CAST(N'1999-05-05' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (558, 1, N'Coral Gables', CAST(N'2007-09-26' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (559, 1, N'Coral Springs', CAST(N'1998-09-17' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (560, 1, N'Carrollwood', CAST(N'2012-11-01' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (561, 1, N'Ft. Lauderdale', CAST(N'2001-08-22' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (562, 1, N'Jacksonville', CAST(N'2008-12-10' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (563, 1, N'Naples', CAST(N'2008-09-05' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (564, 1, N'Pinecrest', CAST(N'2007-09-28' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (565, 1, N'Pembroke Pines', CAST(N'2012-03-28' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (566, 1, N'Palm Beach Gardens', CAST(N'2005-10-26' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (567, 1, N'Plantation', CAST(N'1997-04-14' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (568, 1, N'Sarasota', CAST(N'2004-12-08' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (569, 1, N'South Beach', CAST(N'2007-09-28' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (570, 1, N'San Souci', CAST(N'2013-05-01' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (571, 1, N'Tallahassee', CAST(N'2013-10-09' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (572, 1, N'Tampa', CAST(N'2007-09-28' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (573, 1, N'Wellington', CAST(N'2008-11-05' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (574, 1, N'Winter Park', CAST(N'1998-04-29' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (575, 1, N'Clapham Junction', CAST(N'2004-01-31' AS Date), NULL, 4, 82)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (576, 1, N'Camden', CAST(N'2004-01-31' AS Date), NULL, 4, 82)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (577, 1, N'Cheltenham', CAST(N'2012-11-07' AS Date), NULL, 4, 81)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (578, 1, N'Fulham Broadway', CAST(N'2014-04-08' AS Date), NULL, 4, 82)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (579, 1, N'Giffnock', CAST(N'2011-11-16' AS Date), NULL, 4, 83)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (580, 1, N'Kensington High Street', CAST(N'2007-06-10' AS Date), NULL, 4, 82)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (581, 1, N'Picadilly', CAST(N'2012-05-10' AS Date), NULL, 4, 82)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (582, 1, N'Richmond Upon Thames', CAST(N'2013-10-08' AS Date), NULL, 4, 82)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (583, 1, N'Stoke Newington', CAST(N'2004-01-31' AS Date), NULL, 4, 82)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (584, 1, N'Georgetown', CAST(N'1996-01-31' AS Date), NULL, 4, 15)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (585, 1, N'Foggy Bottom', CAST(N'2011-09-06' AS Date), NULL, 4, 15)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (586, 1, N'P Street', CAST(N'2000-12-14' AS Date), NULL, 4, 15)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (587, 1, N'Tenley Town', CAST(N'1996-08-30' AS Date), NULL, 4, 15)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (588, 1, N'Lexington Green', CAST(N'2007-09-28' AS Date), NULL, 4, 16)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (589, 1, N'Louisville', CAST(N'2004-02-12' AS Date), NULL, 4, 16)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (590, 1, N'Annapolis', CAST(N'2009-05-05' AS Date), NULL, 4, 17)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (591, 1, N'Bethesda', CAST(N'1996-08-30' AS Date), NULL, 4, 17)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (592, 1, N'Chevy Chase', CAST(N'2010-05-18' AS Date), NULL, 4, 17)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (593, 1, N'Harbor East', CAST(N'2002-08-29' AS Date), NULL, 4, 17)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (594, 1, N'Kentlands', CAST(N'2001-10-11' AS Date), NULL, 4, 17)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (595, 1, N'Mt. Washington', CAST(N'1996-08-30' AS Date), NULL, 4, 17)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (596, 1, N'White Flint', CAST(N'2011-04-12' AS Date), NULL, 4, 17)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (597, 1, N'Silver Spring', CAST(N'2000-09-21' AS Date), NULL, 4, 17)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (598, 1, N'Cherry Hill', CAST(N'2014-06-18' AS Date), NULL, 4, 18)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (599, 1, N'Marlton', CAST(N'1998-05-21' AS Date), NULL, 4, 18)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (600, 1, N'Princeton', CAST(N'2004-09-16' AS Date), NULL, 4, 18)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (601, 1, N'Chagrin', CAST(N'2007-09-28' AS Date), NULL, 4, 19)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (602, 1, N'Cincinnati', CAST(N'2007-09-28' AS Date), NULL, 4, 19)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (603, 1, N'Cedar Center', CAST(N'2007-03-25' AS Date), NULL, 4, 19)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (604, 1, N'Dublin', CAST(N'2005-09-07' AS Date), NULL, 4, 19)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (605, 1, N'Ohio State - Relo', CAST(N'2013-03-06' AS Date), NULL, 4, 19)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (606, 1, N'Mason', CAST(N'2007-09-28' AS Date), NULL, 4, 19)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (607, 1, N'Glen Mills', CAST(N'2012-03-06' AS Date), NULL, 4, 20)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (608, 1, N'Devon', CAST(N'1996-08-30' AS Date), NULL, 4, 20)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (609, 1, N'Jenkintown', CAST(N'1999-09-22' AS Date), NULL, 4, 20)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (610, 1, N'North Wales', CAST(N'1996-08-30' AS Date), NULL, 4, 20)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (611, 1, N'Philadelphia', CAST(N'1997-01-08' AS Date), NULL, 4, 20)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (612, 1, N'Plymouth Meeting', CAST(N'2010-01-12' AS Date), NULL, 4, 20)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (613, 1, N'Pittsburgh', CAST(N'2002-10-17' AS Date), NULL, 4, 20)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (614, 1, N'South Street', CAST(N'2001-03-22' AS Date), NULL, 4, 20)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (615, 1, N'Wexford', CAST(N'2012-05-02' AS Date), NULL, 4, 20)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (616, 1, N'Wynnewood', CAST(N'1996-08-30' AS Date), NULL, 4, 20)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (617, 1, N'Arlington', CAST(N'1996-02-29' AS Date), NULL, 4, 21)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (618, 1, N'Charlottesville', CAST(N'2011-06-07' AS Date), NULL, 4, 21)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (619, 1, N'Fair Lakes', CAST(N'2007-01-17' AS Date), NULL, 4, 21)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (620, 1, N'Old Town', CAST(N'2006-01-17' AS Date), NULL, 4, 21)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (621, 1, N'Reston', CAST(N'1996-08-30' AS Date), NULL, 4, 21)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (622, 1, N'Short Pump', CAST(N'2008-09-03' AS Date), NULL, 4, 21)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (623, 1, N'Springfield', CAST(N'1996-08-30' AS Date), NULL, 4, 21)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (624, 1, N'Tysons', CAST(N'1996-08-30' AS Date), NULL, 4, 21)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (625, 1, N'Virginia Beach', CAST(N'2012-10-24' AS Date), NULL, 4, 21)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (626, 1, N'Vienna', CAST(N'1996-11-08' AS Date), NULL, 4, 21)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (627, 1, N'West Des Moines', CAST(N'2012-07-18' AS Date), NULL, 4, 23)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (628, 1, N'Sauganash', CAST(N'2007-02-21' AS Date), NULL, 4, 22)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (629, 1, N'Deerfield', CAST(N'2000-08-23' AS Date), NULL, 4, 24)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (630, 1, N'Evanston', CAST(N'1997-12-03' AS Date), NULL, 4, 22)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (631, 1, N'Chicago Ave', CAST(N'2007-09-28' AS Date), NULL, 4, 22)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (632, 1, N'Gold Coast', CAST(N'1999-09-01' AS Date), NULL, 4, 22)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (633, 1, N'Halsted', CAST(N'2007-07-25' AS Date), NULL, 4, 22)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (634, 1, N'Hinsdale', CAST(N'2007-09-28' AS Date), NULL, 4, 24)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (635, 1, N'Kingsbury Sq.', CAST(N'2009-05-20' AS Date), NULL, 4, 22)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (636, 1, N'Kildeer', CAST(N'2013-03-06' AS Date), NULL, 4, 24)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (637, 1, N'Lakeview', CAST(N'1996-02-14' AS Date), NULL, 4, 22)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (638, 1, N'Northbrook', CAST(N'2007-08-29' AS Date), NULL, 4, 24)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (639, 1, N'Naperville', CAST(N'2008-07-16' AS Date), NULL, 4, 24)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (640, 1, N'Orland Park', CAST(N'2012-11-01' AS Date), NULL, 4, 24)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (641, 1, N'Park Ridge', CAST(N'2013-11-06' AS Date), NULL, 4, 22)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (642, 1, N'River Forest', CAST(N'1994-09-21' AS Date), NULL, 4, 22)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (643, 1, N'Schaumburg', CAST(N'2010-05-05' AS Date), NULL, 4, 24)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (644, 1, N'SouthLoop', CAST(N'2007-08-08' AS Date), NULL, 4, 22)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (645, 1, N'Willowbrook Closed', CAST(N'2002-11-06' AS Date), CAST(N'2015-08-26' AS Date), 4, 24)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (646, 1, N'Wheaton', CAST(N'1997-02-05' AS Date), NULL, 4, 24)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (647, 1, N'Carmel', CAST(N'2007-09-28' AS Date), NULL, 4, 25)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (648, 1, N'Eighty-Sixth St', CAST(N'2007-09-28' AS Date), NULL, 4, 25)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (649, 1, N'South Bend', CAST(N'2013-04-10' AS Date), NULL, 4, 25)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (650, 1, N'Ann Arbor', CAST(N'2003-09-24' AS Date), NULL, 4, 26)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (651, 1, N'Cranbrook', CAST(N'2008-09-26' AS Date), NULL, 4, 26)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (652, 1, N'Detroit', CAST(N'2013-06-05' AS Date), NULL, 4, 26)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (653, 1, N'Rochester Hills', CAST(N'2008-08-06' AS Date), NULL, 4, 26)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (654, 1, N'Somerset', CAST(N'1997-12-29' AS Date), NULL, 4, 26)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (655, 1, N'West Bloomfield', CAST(N'2000-02-03' AS Date), NULL, 4, 26)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (656, 1, N'Lake Calhoun', CAST(N'1999-12-01' AS Date), NULL, 4, 27)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (657, 1, N'Edina', CAST(N'2012-04-18' AS Date), NULL, 4, 27)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (658, 1, N'Maple Grove', CAST(N'2013-07-17' AS Date), NULL, 4, 27)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (659, 1, N'Hennepin - Minneapolis', CAST(N'2013-09-25' AS Date), NULL, 4, 27)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (660, 1, N'Minnetonka', CAST(N'2011-10-12' AS Date), NULL, 4, 27)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (661, 1, N'St. Paul', CAST(N'1995-05-14' AS Date), NULL, 4, 27)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (662, 1, N'Galleria', CAST(N'2001-09-26' AS Date), NULL, 4, 28)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (663, 1, N'Town and Country', CAST(N'2008-06-25' AS Date), NULL, 4, 28)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (664, 1, N'Lincoln', CAST(N'2013-12-11' AS Date), NULL, 4, 29)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (665, 1, N'Regency', CAST(N'2005-09-21' AS Date), NULL, 4, 29)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (666, 1, N'Unionville', CAST(N'2012-10-12' AS Date), NULL, 4, 30)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (667, 1, N'Oakville', CAST(N'2005-05-10' AS Date), NULL, 4, 30)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (668, 1, N'Square One', CAST(N'2011-08-10' AS Date), NULL, 4, 30)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (669, 1, N'Yorkville', CAST(N'2002-05-01' AS Date), NULL, 4, 30)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (670, 1, N'Madison', CAST(N'1996-06-05' AS Date), NULL, 4, 31)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (671, 1, N'Milwaukee', CAST(N'2006-09-20' AS Date), NULL, 4, 31)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (672, 1, N'Bishops Corner', CAST(N'2007-09-28' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (673, 1, N'Glastonbury', CAST(N'2008-03-12' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (674, 1, N'W Hartford', CAST(N'2005-10-26' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (675, 1, N'Arlington', CAST(N'2013-09-18' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (676, 1, N'Andover', CAST(N'2007-09-28' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (677, 1, N'Bedford', CAST(N'1999-04-30' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (678, 1, N'Brookline', CAST(N'2013-04-11' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (679, 1, N'Bellingham', CAST(N'1999-04-30' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (680, 1, N'Brighton', CAST(N'1992-10-01' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (681, 1, N'Cambridge', CAST(N'1992-10-01' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (682, 1, N'Charleston', CAST(N'2013-08-07' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (683, 1, N'Charles River', CAST(N'2005-09-14' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (684, 1, N'Dedham', CAST(N'2009-09-02' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (685, 1, N'Fresh Pond', CAST(N'1993-12-02' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (686, 1, N'Framingham', CAST(N'2001-09-29' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (687, 1, N'Hadley', CAST(N'1992-10-01' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (688, 1, N'Hingham', CAST(N'2004-10-14' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (689, 1, N'Hyannis', CAST(N'2014-05-14' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (690, 1, N'Jamaica Plain', CAST(N'2011-10-31' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (691, 1, N'Lynnfield', CAST(N'2013-08-21' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (692, 1, N'Medford', CAST(N'2007-09-28' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (693, 1, N'Melrose', CAST(N'2013-07-24' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (694, 1, N'Newton', CAST(N'1992-10-01' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (695, 1, N'Newtonville', CAST(N'1999-04-30' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (696, 1, N'River Street', CAST(N'2001-08-22' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (697, 1, N'Somerville', CAST(N'2013-09-04' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (698, 1, N'Swampscott', CAST(N'2005-02-16' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (699, 1, N'South Weymouth', CAST(N'2013-07-10' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (700, 1, N'Symphony', CAST(N'1995-01-05' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (701, 1, N'Wayland', CAST(N'1999-04-30' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (702, 1, N'Wellesley Relo', CAST(N'2011-08-22' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (703, 1, N'Woburn', CAST(N'2006-04-05' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (704, 1, N'Portland Bayside', CAST(N'2007-02-14' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (705, 1, N'Cranston', CAST(N'2007-10-24' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (706, 1, N'Providence', CAST(N'1992-10-01' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (707, 1, N'University Heights', CAST(N'2002-05-02' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (708, 1, N'Danbury', CAST(N'2013-05-17' AS Date), NULL, 4, 45)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (709, 1, N'Darien', CAST(N'2010-05-19' AS Date), NULL, 4, 45)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (710, 1, N'Fairfield', CAST(N'2011-06-02' AS Date), NULL, 4, 45)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (711, 1, N'Greenwich', CAST(N'1996-08-30' AS Date), NULL, 4, 45)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (712, 1, N'Milford', CAST(N'2009-11-11' AS Date), NULL, 4, 45)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (713, 1, N'Westport', CAST(N'2007-09-28' AS Date), NULL, 4, 45)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (714, 1, N'Edgewater', CAST(N'2000-05-17' AS Date), NULL, 4, 46)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (715, 1, N'Marlboro', CAST(N'2014-05-21' AS Date), NULL, 4, 46)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (716, 1, N'Union Relo', CAST(N'2008-10-30' AS Date), NULL, 4, 46)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (717, 1, N'Middletown', CAST(N'2005-06-15' AS Date), NULL, 4, 46)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (718, 1, N'Montclair', CAST(N'1996-08-30' AS Date), NULL, 4, 46)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (719, 1, N'Paramus', CAST(N'2009-03-19' AS Date), NULL, 4, 46)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (720, 1, N'Rose City', CAST(N'2002-10-17' AS Date), NULL, 4, 46)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (721, 1, N'Ridgewood', CAST(N'2001-06-28' AS Date), NULL, 4, 46)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (722, 1, N'West Orange', CAST(N'2006-11-01' AS Date), NULL, 4, 46)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (723, 1, N'Albany', CAST(N'2014-06-18' AS Date), NULL, 4, 48)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (724, 1, N'Brooklyn', CAST(N'2013-12-17' AS Date), NULL, 4, 47)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (725, 1, N'Chelsea', CAST(N'2001-02-15' AS Date), NULL, 4, 47)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (726, 1, N'Columbus Circle', CAST(N'2004-02-05' AS Date), NULL, 4, 47)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (727, 1, N'Bowery', CAST(N'2007-03-27' AS Date), NULL, 4, 47)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (728, 1, N'Jericho', CAST(N'2005-10-06' AS Date), NULL, 4, 44)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (729, 1, N'Lake Grove', CAST(N'2010-03-17' AS Date), NULL, 4, 44)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (730, 1, N'Manhasset', CAST(N'1996-08-30' AS Date), NULL, 4, 44)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (731, 1, N'57th Street', CAST(N'2012-08-24' AS Date), NULL, 4, 47)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (732, 1, N'Port Chester', CAST(N'2013-10-22' AS Date), NULL, 4, 49)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (733, 1, N'Tribeca', CAST(N'2008-07-09' AS Date), NULL, 4, 47)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (734, 1, N'Union Square', CAST(N'2005-03-16' AS Date), NULL, 4, 47)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (735, 1, N'Upper West Side', CAST(N'2009-08-27' AS Date), NULL, 4, 47)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (736, 1, N'White Plains', CAST(N'2004-06-03' AS Date), NULL, 4, 49)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (737, 1, N'Yonkers', CAST(N'2011-10-19' AS Date), NULL, 4, 49)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (738, 1, N'Arden Way', CAST(N'2003-06-11' AS Date), NULL, 4, 40)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (739, 1, N'Blithedale', CAST(N'2010-06-09' AS Date), NULL, 4, 36)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (740, 1, N'Blossom Hill', CAST(N'2010-11-10' AS Date), NULL, 4, 42)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (741, 1, N'BERKELEY', CAST(N'1990-08-01' AS Date), NULL, 4, 34)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (742, 1, N'Capitola', CAST(N'2009-07-29' AS Date), NULL, 4, 43)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (743, 1, N'Castro', CAST(N'2013-11-06' AS Date), NULL, 4, 41)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (744, 1, N'Campbell', CAST(N'1995-02-01' AS Date), NULL, 4, 42)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (745, 1, N'Coddingtown', CAST(N'2010-09-22' AS Date), NULL, 4, 37)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (746, 1, N'Davis', CAST(N'2012-10-24' AS Date), NULL, 4, 40)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (747, 1, N'Folsom', CAST(N'2011-10-26' AS Date), NULL, 4, 40)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (748, 1, N'Fremont - Mowry', CAST(N'2013-09-25' AS Date), NULL, 4, 34)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (749, 1, N'Franklin', CAST(N'1996-07-11' AS Date), NULL, 4, 41)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (750, 1, N'Fresno', CAST(N'2000-09-13' AS Date), NULL, 4, 35)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (751, 1, N'Harrison', CAST(N'2007-09-26' AS Date), NULL, 4, 34)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (752, 1, N'Lafayette', CAST(N'2011-05-18' AS Date), NULL, 4, 34)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (753, 1, N'Los Altos', CAST(N'2006-09-13' AS Date), NULL, 4, 38)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (754, 1, N'Los Gatos', CAST(N'1994-04-28' AS Date), NULL, 4, 42)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (755, 1, N'Mill Valley', CAST(N'1992-07-10' AS Date), NULL, 4, 36)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (756, 1, N'Monterey', CAST(N'1998-06-24' AS Date), NULL, 4, 43)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (757, 1, N'Noe Valley', CAST(N'2009-09-30' AS Date), NULL, 4, 41)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (758, 1, N'Novato', CAST(N'2010-04-22' AS Date), NULL, 4, 36)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (759, 1, N'Napa', CAST(N'2008-01-16' AS Date), NULL, 4, 37)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (760, 1, N'Ocean Avenue', CAST(N'2012-08-29' AS Date), NULL, 4, 41)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (761, 1, N'Palo Alto', CAST(N'1989-01-01' AS Date), NULL, 4, 38)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (762, 1, N'Petaluma', CAST(N'2000-02-14' AS Date), NULL, 4, 37)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (763, 1, N'Potrero Hill', CAST(N'2007-09-12' AS Date), NULL, 4, 41)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (764, 1, N'San Ramon', CAST(N'2000-03-29' AS Date), NULL, 4, 34)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (765, 1, N'Redwood City', CAST(N'2004-11-17' AS Date), NULL, 4, 38)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (766, 1, N'Roseville', CAST(N'2008-11-05' AS Date), NULL, 4, 40)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (767, 1, N'Sebastopol', CAST(N'2000-02-14' AS Date), NULL, 4, 37)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (768, 1, N'San Mateo', CAST(N'2003-01-22' AS Date), NULL, 4, 38)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (769, 1, N'Sonoma', CAST(N'2007-06-27' AS Date), NULL, 4, 37)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (770, 1, N'SOMA', CAST(N'2004-01-15' AS Date), NULL, 4, 41)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (771, 1, N'San Rafael', CAST(N'1997-04-09' AS Date), NULL, 4, 36)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (772, 1, N'Santa Rosa', CAST(N'2000-02-14' AS Date), NULL, 4, 37)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (773, 1, N'Stevens Creek', CAST(N'2007-08-22' AS Date), NULL, 4, 42)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (774, 1, N'Haight', CAST(N'2011-02-16' AS Date), NULL, 4, 41)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (775, 1, N'Santa Cruz', CAST(N'2009-03-18' AS Date), NULL, 4, 43)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (776, 1, N'Walnut Creek', CAST(N'2001-01-31' AS Date), NULL, 4, 34)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (777, 1, N'Reno', CAST(N'2008-06-25' AS Date), NULL, 4, 39)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (778, 1, N'Cambie', CAST(N'2009-04-29' AS Date), NULL, 4, 50)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (779, 1, N'Kitsilano', CAST(N'2007-09-28' AS Date), NULL, 4, 50)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (780, 1, N'W Vancouver', CAST(N'2004-09-01' AS Date), NULL, 4, 50)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (781, 1, N'Robson', CAST(N'2007-09-28' AS Date), NULL, 4, 50)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (782, 1, N'Bend', CAST(N'2007-09-28' AS Date), NULL, 4, 51)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (783, 1, N'Bridgeport', CAST(N'2006-11-01' AS Date), NULL, 4, 51)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (784, 1, N'Fremont', CAST(N'2007-09-28' AS Date), NULL, 4, 51)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (785, 1, N'Tigard (Greenway)', CAST(N'2014-05-21' AS Date), NULL, 4, 51)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (786, 1, N'East Portland', CAST(N'2010-01-12' AS Date), NULL, 4, 51)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (787, 1, N'Laurelhurst', CAST(N'2007-09-28' AS Date), NULL, 4, 51)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (788, 1, N'Portland', CAST(N'2002-03-20' AS Date), NULL, 4, 51)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (789, 1, N'Tanasbourne', CAST(N'2008-04-23' AS Date), NULL, 4, 51)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (790, 1, N'Bellevue', CAST(N'2004-06-30' AS Date), NULL, 4, 52)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (791, 1, N'Interbay', CAST(N'2009-10-13' AS Date), NULL, 4, 52)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (792, 1, N'Lynnwood', CAST(N'2012-03-15' AS Date), NULL, 4, 52)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (793, 1, N'Mill Plain', CAST(N'2007-09-28' AS Date), NULL, 4, 51)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (794, 1, N'Redmond', CAST(N'2006-08-30' AS Date), NULL, 4, 52)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (795, 1, N'Roosevelt Square', CAST(N'1999-11-03' AS Date), NULL, 4, 52)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (796, 1, N'Westlake', CAST(N'2006-11-08' AS Date), NULL, 4, 52)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (797, 1, N'Basalt CO', CAST(N'2012-08-15' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (798, 1, N'BelMar', CAST(N'2005-12-07' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (799, 1, N'Baseline', CAST(N'2007-09-28' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (800, 1, N'Cherry Creek', CAST(N'2000-11-01' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (801, 1, N'Colfax', CAST(N'2007-09-28' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (802, 1, N'Colorado Blvd', CAST(N'2007-09-28' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (803, 1, N'Capitol Hill', CAST(N'2007-09-28' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (804, 1, N'Frisco', CAST(N'2014-04-29' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (805, 1, N'Ft. Collins', CAST(N'2004-06-23' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (806, 1, N'Highlands Ranch', CAST(N'2002-03-27' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (807, 1, N'Alpine Ideal', CAST(N'2007-09-28' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (808, 1, N'Governers Ranch', CAST(N'2012-12-19' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (809, 1, N'New Center Point', CAST(N'2007-09-28' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (810, 1, N'Pike''s Peak', CAST(N'2004-03-03' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (811, 1, N'Pearl', CAST(N'1998-02-25' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (812, 1, N'South Glen', CAST(N'2009-06-15' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (813, 1, N'Superior', CAST(N'2007-09-28' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (814, 1, N'Tamarac', CAST(N'2005-09-14' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (815, 1, N'Whole Foods Wine and Spirits', CAST(N'1998-02-25' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (816, 1, N'Westminster', CAST(N'2007-09-28' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (817, 1, N'Washington Park', CAST(N'2007-09-28' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (818, 1, N'Boise', CAST(N'2012-11-14' AS Date), NULL, 4, 54)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (819, 1, N'Metcalf', CAST(N'2002-02-06' AS Date), NULL, 4, 55)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (820, 1, N'Overland Park', CAST(N'2007-09-28' AS Date), NULL, 4, 55)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (821, 1, N'Academy', CAST(N'2002-09-18' AS Date), NULL, 4, 56)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (822, 1, N'Cerrillos', CAST(N'1999-12-01' AS Date), NULL, 4, 56)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (823, 1, N'Indian School Plaza', CAST(N'2007-09-28' AS Date), NULL, 4, 56)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (824, 1, N'St. Francis', CAST(N'2007-09-28' AS Date), NULL, 4, 56)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (825, 1, N'South Valley', CAST(N'2014-05-30' AS Date), NULL, 4, 57)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (826, 1, N'Cottonwood Heights', CAST(N'2007-09-28' AS Date), NULL, 4, 57)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (827, 1, N'Park City', CAST(N'2007-09-28' AS Date), NULL, 4, 57)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (828, 1, N'Sugarhouse', CAST(N'2007-09-28' AS Date), NULL, 4, 57)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (829, 1, N'Trolley Square', CAST(N'2011-03-14' AS Date), NULL, 4, 57)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (830, 1, N'Mountain Brook', CAST(N'2007-02-28' AS Date), NULL, 4, 58)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (831, 1, N'Alpharetta', CAST(N'2001-11-01' AS Date), NULL, 4, 59)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (832, 1, N'Briarcliff', CAST(N'1999-04-28' AS Date), NULL, 4, 59)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (833, 1, N'Cobb', CAST(N'2001-11-01' AS Date), NULL, 4, 59)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (834, 1, N'Duluth', CAST(N'2006-09-20' AS Date), NULL, 4, 59)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (835, 1, N'Merchant''s Walk', CAST(N'2011-07-27' AS Date), NULL, 4, 59)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (836, 1, N'Ponce de Leon', CAST(N'2003-03-12' AS Date), NULL, 4, 59)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (837, 1, N'Savannah', CAST(N'2013-08-13' AS Date), NULL, 4, 59)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (838, 1, N'Sandy Springs', CAST(N'2000-08-31' AS Date), NULL, 4, 59)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (839, 1, N'W Paces Ferry', CAST(N'2005-09-28' AS Date), NULL, 4, 59)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (840, 1, N'Jackson', CAST(N'2014-02-04' AS Date), NULL, 4, 60)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (841, 1, N'Charlotte', CAST(N'2012-08-29' AS Date), NULL, 4, 61)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (842, 1, N'Asheville', CAST(N'2010-05-24' AS Date), NULL, 4, 61)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (843, 1, N'Cary', CAST(N'2000-05-11' AS Date), NULL, 4, 61)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (844, 1, N'Chapel Hill', CAST(N'1991-11-01' AS Date), NULL, 4, 61)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (845, 1, N'Durham', CAST(N'1996-02-29' AS Date), NULL, 4, 61)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (846, 1, N'Greensboro', CAST(N'2012-04-12' AS Date), NULL, 4, 61)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (847, 1, N'North Raleigh', CAST(N'2011-03-16' AS Date), NULL, 4, 61)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (848, 1, N'Raleigh', CAST(N'1992-12-03' AS Date), NULL, 4, 61)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (849, 1, N'Winston-Salem', CAST(N'2000-01-12' AS Date), NULL, 4, 61)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (850, 1, N'Wilmington', CAST(N'2012-05-23' AS Date), NULL, 4, 61)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (851, 1, N'Charleston', CAST(N'2004-05-19' AS Date), NULL, 4, 62)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (852, 1, N'Cross Hill', CAST(N'2012-10-26' AS Date), NULL, 4, 62)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (853, 1, N'Woodruff', CAST(N'2006-04-26' AS Date), NULL, 4, 62)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (854, 1, N'Chattanooga', CAST(N'2010-05-24' AS Date), NULL, 4, 63)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (855, 1, N'Green Hills', CAST(N'2007-11-01' AS Date), NULL, 4, 63)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (856, 1, N'McEwen', CAST(N'2011-05-18' AS Date), NULL, 4, 63)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (857, 1, N'Poplar Avenue', CAST(N'2014-01-14' AS Date), NULL, 4, 63)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (858, 1, N'Chandler', CAST(N'2007-10-26' AS Date), NULL, 4, 64)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (859, 1, N'Camelback', CAST(N'2013-09-18' AS Date), NULL, 4, 64)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (860, 1, N'Flagstaff', CAST(N'2014-04-24' AS Date), NULL, 4, 64)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (861, 1, N'Prescott', CAST(N'2014-04-24' AS Date), NULL, 4, 64)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (862, 1, N'Paradise Valley', CAST(N'2002-11-06' AS Date), NULL, 4, 64)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (863, 1, N'River Road', CAST(N'2013-01-16' AS Date), NULL, 4, 64)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (864, 1, N'Scottsdale', CAST(N'2008-02-27' AS Date), NULL, 4, 64)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (865, 1, N'Sedona', CAST(N'2014-04-24' AS Date), NULL, 4, 64)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (866, 1, N'Speedway', CAST(N'2007-09-28' AS Date), NULL, 4, 64)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (867, 1, N'Tempe', CAST(N'1998-03-04' AS Date), NULL, 4, 64)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (868, 1, N'Arroyo', CAST(N'2007-11-07' AS Date), NULL, 4, 66)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (869, 1, N'Brentwood', CAST(N'1997-10-29' AS Date), NULL, 4, 67)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (870, 1, N'Beverly Hills', CAST(N'1993-09-01' AS Date), NULL, 4, 66)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (871, 1, N'Del Mar', CAST(N'2013-02-27' AS Date), NULL, 4, 70)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (872, 1, N'Encinitas', CAST(N'2011-06-29' AS Date), NULL, 4, 70)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (873, 1, N'Fairfax', CAST(N'2002-05-22' AS Date), NULL, 4, 66)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (874, 1, N'Glendale', CAST(N'2004-05-04' AS Date), NULL, 4, 66)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (875, 1, N'Hillcrest', CAST(N'1997-04-09' AS Date), NULL, 4, 70)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (876, 1, N'West Hollywood', CAST(N'2000-02-16' AS Date), NULL, 4, 66)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (877, 1, N'Huntington Beach', CAST(N'2010-10-13' AS Date), NULL, 4, 69)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (878, 1, N'Jamboree', CAST(N'2007-08-29' AS Date), NULL, 4, 69)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (879, 1, N'Laguna Beach', CAST(N'2007-09-28' AS Date), NULL, 4, 69)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (880, 1, N'La Jolla', CAST(N'1996-11-13' AS Date), NULL, 4, 70)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (881, 1, N'Long Beach', CAST(N'2007-09-28' AS Date), NULL, 4, 69)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (882, 1, N'Laguna Niguel', CAST(N'2012-05-16' AS Date), NULL, 4, 69)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (883, 1, N'Montana Avenue', CAST(N'2007-09-28' AS Date), NULL, 4, 67)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (884, 1, N'Newport Beach', CAST(N'2012-09-19' AS Date), NULL, 4, 69)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (885, 1, N'Thousand Oaks', CAST(N'2005-03-09' AS Date), NULL, 4, 67)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (886, 1, N'Oxnard', CAST(N'2013-06-19' AS Date), NULL, 4, 67)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (887, 1, N'Pasadena', CAST(N'1999-05-05' AS Date), NULL, 4, 66)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (888, 1, N'Pacifc Coast Highway', CAST(N'2007-04-26' AS Date), NULL, 4, 69)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (889, 1, N'Porter Ranch', CAST(N'2001-11-01' AS Date), NULL, 4, 67)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (890, 1, N'Redondo Beach', CAST(N'1993-09-01' AS Date), NULL, 4, 69)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (891, 1, N'Santa Barbara', CAST(N'2009-10-07' AS Date), NULL, 4, 67)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (892, 1, N'Sherman Oaks', CAST(N'1993-09-01' AS Date), NULL, 4, 66)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (893, 1, N'Sherman Oaks West', CAST(N'1996-01-17' AS Date), NULL, 4, 66)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (894, 1, N'San Luis Obispo', CAST(N'2014-04-24' AS Date), NULL, 4, 67)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (895, 1, N'Santa Monica', CAST(N'2003-07-16' AS Date), NULL, 4, 67)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (896, 1, N'Torrance', CAST(N'1999-09-22' AS Date), NULL, 4, 69)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (897, 1, N'Tarzana', CAST(N'2010-05-12' AS Date), NULL, 4, 67)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (898, 1, N'Valencia', CAST(N'2004-08-25' AS Date), NULL, 4, 66)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (899, 1, N'Venice', CAST(N'2008-09-03' AS Date), NULL, 4, 67)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (900, 1, N'Woodland Hills', CAST(N'1999-10-27' AS Date), NULL, 4, 67)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (901, 1, N'West LA', CAST(N'1996-07-23' AS Date), NULL, 4, 67)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (902, 1, N'Wilshire Blvd', CAST(N'2007-09-28' AS Date), NULL, 4, 67)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (903, 1, N'Westwood', CAST(N'2003-02-05' AS Date), NULL, 4, 66)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (904, 1, N'Kailua Oahu', CAST(N'2012-04-18' AS Date), NULL, 4, 65)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (905, 1, N'Kahala Mall Oahu', CAST(N'2008-09-10' AS Date), NULL, 4, 65)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (906, 1, N'Maui Mall Maui', CAST(N'2010-02-24' AS Date), NULL, 4, 65)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (907, 1, N'Ft. Apache', CAST(N'2003-08-27' AS Date), NULL, 4, 68)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (908, 1, N'Henderson', CAST(N'2006-04-05' AS Date), NULL, 4, 68)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (909, 1, N'Las Vegas Boulevard', CAST(N'2008-10-08' AS Date), NULL, 4, 68)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (910, 1, N'Tenaya', CAST(N'2007-09-28' AS Date), NULL, 4, 68)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (911, 1, N'Little Rock', CAST(N'2007-09-28' AS Date), CAST(N'2015-02-16' AS Date), 4, 76)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (912, 1, N'Arabella Station', CAST(N'2002-12-04' AS Date), NULL, 4, 77)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (913, 1, N'New Orleans', CAST(N'2014-02-04' AS Date), NULL, 4, 77)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (914, 1, N'Baton Rouge', CAST(N'2005-07-20' AS Date), NULL, 4, 72)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (915, 1, N'Veterans', CAST(N'2005-05-18' AS Date), NULL, 4, 77)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (916, 1, N'Oklahoma City', CAST(N'2011-10-12' AS Date), NULL, 4, 78)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (917, 1, N'Tulsa', CAST(N'2007-09-28' AS Date), NULL, 4, 78)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (918, 1, N'Yale', CAST(N'2013-11-19' AS Date), NULL, 4, 78)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (919, 1, N'Arbor Trails', CAST(N'2012-06-19' AS Date), NULL, 4, 71)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (920, 1, N'Addison', CAST(N'2013-07-16' AS Date), NULL, 4, 73)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (921, 1, N'Bee Cave', CAST(N'2012-05-16' AS Date), NULL, 4, 71)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (922, 1, N'Bellaire', CAST(N'2000-12-14' AS Date), NULL, 4, 74)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (923, 1, N'Champions', CAST(N'2014-06-25' AS Date), NULL, 4, 74)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (924, 1, N'Domain', CAST(N'2014-01-15' AS Date), NULL, 4, 71)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (925, 1, N'Preston Forest', CAST(N'2006-12-06' AS Date), NULL, 4, 73)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (926, 1, N'Fairview', CAST(N'2010-11-03' AS Date), NULL, 4, 73)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (927, 1, N'Gateway', CAST(N'1995-05-31' AS Date), NULL, 4, 71)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (928, 1, N'Highland Park', CAST(N'2002-06-12' AS Date), NULL, 4, 73)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (929, 1, N'Kirby', CAST(N'2000-05-17' AS Date), NULL, 4, 74)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (930, 1, N'Katy', CAST(N'2013-01-30' AS Date), NULL, 4, 74)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (931, 1, N'Lamar Culinary Center', CAST(N'2005-03-03' AS Date), NULL, 4, 71)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (932, 1, N'Lakewood', CAST(N'2009-03-02' AS Date), NULL, 4, 73)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (933, 1, N'Lamar', CAST(N'2005-03-03' AS Date), NULL, 4, 71)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (934, 1, N'Las Cimas Cafe', CAST(N'2013-01-14' AS Date), NULL, 4, 71)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (935, 1, N'Montrose', CAST(N'2011-06-22' AS Date), NULL, 4, 74)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (936, 1, N'Park Lane', CAST(N'2010-03-15' AS Date), NULL, 4, 73)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (937, 1, N'Parkway', CAST(N'1999-08-11' AS Date), NULL, 4, 73)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (938, 1, N'Plano', CAST(N'1994-11-02' AS Date), NULL, 4, 73)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (939, 1, N'Quarry', CAST(N'1997-10-29' AS Date), NULL, 4, 79)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (940, 1, N'Sugarland', CAST(N'2007-12-05' AS Date), NULL, 4, 74)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (941, 1, N'San Antonio (Vineyard)', CAST(N'2012-09-19' AS Date), NULL, 4, 79)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (942, 1, N'Wilcrest', CAST(N'1991-09-01' AS Date), NULL, 4, 74)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (943, 1, N'Woodway', CAST(N'1993-10-21' AS Date), CAST(N'2015-04-04' AS Date), 4, 74)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (944, 1, N'Post Oak', CAST(N'2014-10-01' AS Date), NULL, 4, 74)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (945, 1, N'Clearwater', CAST(N'2014-08-24' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (946, 1, N'Columbia', CAST(N'2014-08-20' AS Date), NULL, 4, 15)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (947, 1, N'North Nashua', CAST(N'2014-08-13' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (948, 1, N'Gilman', CAST(N'2014-11-04' AS Date), NULL, 4, 34)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (949, 1, N'Wichita', CAST(N'2014-09-03' AS Date), NULL, 4, 55)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (950, 1, N'Avalon', CAST(N'2014-10-14' AS Date), NULL, 4, 59)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (951, 1, N'Kenilworth', CAST(N'2014-08-28' AS Date), NULL, 4, 61)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (952, 1, N'Lake Norman', CAST(N'2014-09-21' AS Date), NULL, 4, 61)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (953, 1, N'Augusta', CAST(N'2014-09-11' AS Date), NULL, 4, 59)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (954, 1, N'Hilton Head Island', CAST(N'2014-07-30' AS Date), NULL, 4, 62)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (955, 1, N'Palm Desert', CAST(N'2014-09-24' AS Date), NULL, 4, 66)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (956, 1, N'Oracle 2', CAST(N'2014-08-14' AS Date), NULL, 4, 64)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (957, 1, N'Highland Village', CAST(N'2014-09-24' AS Date), NULL, 4, 73)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (958, 1, N'Colleyville', CAST(N'2014-07-08' AS Date), NULL, 4, 73)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (959, 1, N'Ambassador Caffery', CAST(N'2014-09-24' AS Date), NULL, 4, 75)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (960, 1, N'Bradburn', CAST(N'2014-12-09' AS Date), NULL, 4, 53)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (961, 1, N'The Alameda', CAST(N'2014-12-09' AS Date), NULL, 4, 42)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (962, 1, N'Ink Block', CAST(N'2015-01-09' AS Date), NULL, 4, 32)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (963, 1, N'West Palm Beach', CAST(N'2015-02-25' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (964, 1, N'Davie', CAST(N'2015-02-11' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (965, 1, N'Streeterville', CAST(N'2015-01-28' AS Date), NULL, 4, 22)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (966, 1, N'Pompano Beach', CAST(N'2015-01-28' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (967, 1, N'Upper East Side', CAST(N'2015-02-18' AS Date), NULL, 4, 47)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (968, 1, N'Bowman', CAST(N'2015-02-18' AS Date), NULL, 4, 76)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (969, 1, N'DePaul', CAST(N'2015-02-25' AS Date), NULL, 4, 22)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (970, 1, N'Woodlands', CAST(N'2015-03-18' AS Date), NULL, 4, 74)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (971, 1, N'Knoxville', CAST(N'2015-03-24' AS Date), NULL, 4, 63)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (972, 1, N'West Loop', CAST(N'2015-03-25' AS Date), NULL, 4, 22)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (973, 1, N'Voss', CAST(N'2015-04-08' AS Date), NULL, 4, 74)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (974, 1, N'My Street Grocery 365', CAST(N'2014-05-21' AS Date), NULL, 4, 51)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (975, 1, N'Yonge and Sheppard', CAST(N'2014-09-24' AS Date), NULL, 4, 30)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (976, 1, N'Lansdowne Park', CAST(N'2014-11-19' AS Date), NULL, 4, 30)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (977, 1, N'Miami', CAST(N'2015-01-14' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (978, 1, N'Olathe', CAST(N'2015-04-22' AS Date), NULL, 4, 55)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (979, 1, N'Morristown', CAST(N'2015-04-22' AS Date), NULL, 4, 46)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (980, 1, N'Chicago Edgewater', CAST(N'2015-04-29' AS Date), NULL, 4, 22)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (981, 1, N'Chambers Bay', CAST(N'2015-05-05' AS Date), NULL, 4, 52)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (982, 1, N'New Dublin', CAST(N'2015-05-20' AS Date), NULL, 4, 34)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (983, 1, N'Elmhurst', CAST(N'2015-05-27' AS Date), NULL, 4, 24)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (984, 1, N'Dayton', CAST(N'2015-06-03' AS Date), NULL, 4, 19)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (985, 1, N'Playa Vista', CAST(N'2015-06-17' AS Date), NULL, 4, 67)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (986, 1, N'Green Bay Road', CAST(N'2015-07-29' AS Date), NULL, 4, 22)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (987, 1, N'Ashburn', CAST(N'2015-07-29' AS Date), NULL, 4, 21)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (988, 1, N'Clark', CAST(N'2015-08-07' AS Date), NULL, 4, 46)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (989, 1, N'Uptown Dallas', CAST(N'2015-08-12' AS Date), NULL, 4, 73)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (990, 1, N'Mobile', CAST(N'2015-09-22' AS Date), NULL, 4, 58)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (991, 1, N'Germantown', CAST(N'2015-08-18' AS Date), NULL, 4, 63)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (992, 1, N'Willowbrook', CAST(N'2015-08-26' AS Date), NULL, 4, 24)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (993, 1, N'Easton', CAST(N'2015-09-02' AS Date), NULL, 4, 19)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (994, 1, N'Rocky River', CAST(N'2015-09-23' AS Date), NULL, 4, 19)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (995, 1, N'Schererville', CAST(N'2015-09-23' AS Date), NULL, 4, 22)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (996, 1, N'Downtown LA', CAST(N'2015-10-28' AS Date), NULL, 4, 66)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (997, 1, N'Newport News', CAST(N'2015-11-04' AS Date), NULL, 4, 21)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (998, 1, N'Huntsville', CAST(N'2015-11-13' AS Date), NULL, 4, 58)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (999, 1, N'Altamonte Springs', CAST(N'2016-01-06' AS Date), NULL, 4, 14)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1100, 1, N'365', CAST(N'2015-11-13' AS Date), NULL, 1, NULL)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1101, 1, N'365_Florida', NULL, NULL, 2, 1100)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1102, 1, N'365_Mid Atlantic', NULL, NULL, 2, 1100)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1103, 1, N'365_Mid West', NULL, NULL, 2, 1100)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1104, 1, N'365_North Atlantic', NULL, NULL, 2, 1100)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1105, 1, N'365_Northern California', NULL, NULL, 2, 1100)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1106, 1, N'365_North East', NULL, NULL, 2, 1100)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1107, 1, N'365_Pacific Northwest', NULL, NULL, 2, 1100)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1108, 1, N'365_Rocky Mountain', NULL, NULL, 2, 1100)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1109, 1, N'365_South', NULL, NULL, 2, 1100)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1110, 1, N'365_Southern Pacific', NULL, NULL, 2, 1100)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1111, 1, N'365_Southwest', NULL, NULL, 2, 1100)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1112, 1, N'365_United Kingdom', NULL, NULL, 2, 1100)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1113, 1, N'365_MET_FL', CAST(N'1980-09-21' AS Date), NULL, 3, 1101)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1114, 1, N'365_MET_DC', CAST(N'1980-09-21' AS Date), NULL, 3, 1102)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1115, 1, N'365_MET_KY', CAST(N'1980-09-21' AS Date), NULL, 3, 1102)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1116, 1, N'365_MET_MD', CAST(N'1980-09-21' AS Date), NULL, 3, 1102)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1117, 1, N'365_MET_NJ', CAST(N'1980-09-21' AS Date), NULL, 3, 1102)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1118, 1, N'365_MET_OH', CAST(N'1980-09-21' AS Date), NULL, 3, 1102)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1119, 1, N'365_MET_PA', CAST(N'1980-09-21' AS Date), NULL, 3, 1102)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1120, 1, N'365_MET_VA', CAST(N'1980-09-21' AS Date), NULL, 3, 1102)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1121, 1, N'365_MET_CHI', CAST(N'1980-09-21' AS Date), NULL, 3, 1103)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1122, 1, N'365_MET_IA', CAST(N'1980-09-21' AS Date), NULL, 3, 1103)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1123, 1, N'365_MET_IL', CAST(N'1980-09-21' AS Date), NULL, 3, 1103)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1124, 1, N'365_MET_IN', CAST(N'1980-09-21' AS Date), NULL, 3, 1103)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1125, 1, N'365_MET_MI', CAST(N'1980-09-21' AS Date), NULL, 3, 1103)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1126, 1, N'365_MET_MN', CAST(N'1980-09-21' AS Date), NULL, 3, 1103)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1127, 1, N'365_MET_MO', CAST(N'1980-09-21' AS Date), NULL, 3, 1103)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1128, 1, N'365_MET_NEB', CAST(N'1980-09-21' AS Date), NULL, 3, 1103)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1129, 1, N'365_MET_ON', CAST(N'1980-09-21' AS Date), NULL, 3, 1103)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1130, 1, N'365_MET_WI', CAST(N'1980-09-21' AS Date), NULL, 3, 1103)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1131, 1, N'365_MET_BOS', CAST(N'1980-09-21' AS Date), NULL, 3, 1104)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1132, 1, N'365_MET_NA_OTHER', CAST(N'1980-09-21' AS Date), NULL, 3, 1104)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1133, 1, N'365_MET_EBY', CAST(N'1980-09-21' AS Date), NULL, 3, 1105)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1134, 1, N'365_MET_FRS', CAST(N'1980-09-21' AS Date), NULL, 3, 1105)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1135, 1, N'365_MET_MRN', CAST(N'1980-09-21' AS Date), NULL, 3, 1105)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1136, 1, N'365_MET_NPA', CAST(N'1980-09-21' AS Date), NULL, 3, 1105)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1137, 1, N'365_MET_PEN', CAST(N'1980-09-21' AS Date), NULL, 3, 1105)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1138, 1, N'365_MET_REN', CAST(N'1980-09-21' AS Date), NULL, 3, 1105)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1139, 1, N'365_MET_SAC', CAST(N'1980-09-21' AS Date), NULL, 3, 1105)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1140, 1, N'365_MET_SFO', CAST(N'1980-09-21' AS Date), NULL, 3, 1105)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1141, 1, N'365_MET_SJC', CAST(N'1980-09-21' AS Date), NULL, 3, 1105)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1142, 1, N'365_MET_STZ', CAST(N'1980-09-21' AS Date), NULL, 3, 1105)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1143, 1, N'365_MET_LI', CAST(N'1980-09-21' AS Date), NULL, 3, 1106)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1144, 1, N'365_MET_NE_CT', CAST(N'1980-09-21' AS Date), NULL, 3, 1106)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1145, 1, N'365_MET_NE_NJ', CAST(N'1980-09-21' AS Date), NULL, 3, 1106)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1146, 1, N'365_MET_NYC', CAST(N'1980-09-21' AS Date), NULL, 3, 1106)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1147, 1, N'365_MET_UP_NY', CAST(N'1980-09-21' AS Date), NULL, 3, 1106)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1148, 1, N'365_MET_WESTCH', CAST(N'1980-09-21' AS Date), NULL, 3, 1106)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1149, 1, N'365_MET_CP', CAST(N'1980-09-21' AS Date), NULL, 3, 1107)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1150, 1, N'365_MET_OR', CAST(N'1980-09-21' AS Date), NULL, 3, 1107)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1151, 1, N'365_MET_WA', CAST(N'1980-09-21' AS Date), NULL, 3, 1107)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1152, 1, N'365_MET_CO', CAST(N'1980-09-21' AS Date), NULL, 3, 1108)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1153, 1, N'365_MET_ID', CAST(N'1980-09-21' AS Date), NULL, 3, 1108)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1154, 1, N'365_MET_KS', CAST(N'1980-09-21' AS Date), NULL, 3, 1108)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1155, 1, N'365_MET_NM', CAST(N'1980-09-21' AS Date), NULL, 3, 1108)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1156, 1, N'365_MET_UT', CAST(N'1980-09-21' AS Date), NULL, 3, 1108)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1157, 1, N'365_MET_AL', CAST(N'1980-09-21' AS Date), NULL, 3, 1109)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1158, 1, N'365_MET_GA', CAST(N'1980-09-21' AS Date), NULL, 3, 1109)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1159, 1, N'365_MET_MS', CAST(N'1980-09-21' AS Date), NULL, 3, 1109)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1160, 1, N'365_MET_NC', CAST(N'1980-09-21' AS Date), NULL, 3, 1109)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1161, 1, N'365_MET_SC', CAST(N'1980-09-21' AS Date), NULL, 3, 1109)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1162, 1, N'365_MET_TN', CAST(N'1980-09-21' AS Date), NULL, 3, 1109)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1163, 1, N'365_MET_AZ', CAST(N'1980-09-21' AS Date), NULL, 3, 1110)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1164, 1, N'365_MET_HI', CAST(N'1980-09-21' AS Date), NULL, 3, 1110)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1165, 1, N'365_MET_LA_EAST', CAST(N'1980-09-21' AS Date), NULL, 3, 1110)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1166, 1, N'365_MET_LA_WEST', CAST(N'1980-09-21' AS Date), NULL, 3, 1110)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1167, 1, N'365_MET_NV', CAST(N'1980-09-21' AS Date), NULL, 3, 1110)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1168, 1, N'365_MET_OC', CAST(N'1980-09-21' AS Date), NULL, 3, 1110)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1169, 1, N'365_MET_SD', CAST(N'1980-09-21' AS Date), NULL, 3, 1110)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1170, 1, N'365_MET_AUS', CAST(N'1980-09-21' AS Date), NULL, 3, 1111)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1171, 1, N'365_MET_BR', CAST(N'1980-09-21' AS Date), NULL, 3, 1111)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1172, 1, N'365_MET_DFW', CAST(N'1980-09-21' AS Date), NULL, 3, 1111)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1173, 1, N'365_MET_HOU', CAST(N'1980-09-21' AS Date), NULL, 3, 1111)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1174, 1, N'365_MET_LFY', CAST(N'1980-09-21' AS Date), NULL, 3, 1111)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1175, 1, N'365_MET_LR', CAST(N'1980-09-21' AS Date), NULL, 3, 1111)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1176, 1, N'365_MET_NO', CAST(N'1980-09-21' AS Date), NULL, 3, 1111)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1177, 1, N'365_MET_OK', CAST(N'1980-09-21' AS Date), NULL, 3, 1111)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1178, 1, N'365_MET_SA', CAST(N'1980-09-21' AS Date), NULL, 3, 1111)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1179, 1, N'365_MET_SRV', CAST(N'1980-09-21' AS Date), NULL, 3, 1111)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1180, 1, N'365_MET_ENG', CAST(N'1980-09-21' AS Date), NULL, 3, 1112)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1181, 1, N'365_MET_LDN', CAST(N'1980-09-21' AS Date), NULL, 3, 1112)
GO
INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1182, 1, N'365_MET_SCT', CAST(N'1980-09-21' AS Date), NULL, 3, 1112)
GO
SET IDENTITY_INSERT [dbo].[Locale] OFF
GO
