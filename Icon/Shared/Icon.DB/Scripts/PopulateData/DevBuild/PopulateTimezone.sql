print N'Populating Timezone...'

SET IDENTITY_INSERT [dbo].[Timezone] ON 

GO
INSERT [dbo].[Timezone] ([timezoneID], [timezoneCode], [timezoneName], [gmtOffset], [dstStart], [dstEnd]) VALUES (1, N'HST', N'(UTC-10:00) Hawaii', -10, NULL, NULL)
GO
INSERT [dbo].[Timezone] ([timezoneID], [timezoneCode], [timezoneName], [gmtOffset], [dstStart], [dstEnd]) VALUES (2, N'AKST', N'(UTC-09:00) Alaska', -9, NULL, NULL)
GO
INSERT [dbo].[Timezone] ([timezoneID], [timezoneCode], [timezoneName], [gmtOffset], [dstStart], [dstEnd]) VALUES (3, N'PST', N'(UTC-08:00) Pacific Time (US & Canada)', -8, NULL, NULL)
GO
INSERT [dbo].[Timezone] ([timezoneID], [timezoneCode], [timezoneName], [gmtOffset], [dstStart], [dstEnd]) VALUES (4, N'MST', N'(UTC-07:00) Mountain Time (US & Canada)', -7, NULL, NULL)
GO
INSERT [dbo].[Timezone] ([timezoneID], [timezoneCode], [timezoneName], [gmtOffset], [dstStart], [dstEnd]) VALUES (5, N'CST', N'(UTC-06:00) Central Time (US & Canada)', -6, NULL, NULL)
GO
INSERT [dbo].[Timezone] ([timezoneID], [timezoneCode], [timezoneName], [gmtOffset], [dstStart], [dstEnd]) VALUES (6, N'EST', N'(UTC-05:00) Eastern Time (US & Canada)', -5, NULL, NULL)
GO
INSERT [dbo].[Timezone] ([timezoneID], [timezoneCode], [timezoneName], [gmtOffset], [dstStart], [dstEnd]) VALUES (7, N'ADT', N'(UTC-04:00) Atlantic Time (Canada)', -4, NULL, NULL)
GO
INSERT [dbo].[Timezone] ([timezoneID], [timezoneCode], [timezoneName], [gmtOffset], [dstStart], [dstEnd]) VALUES (8, N'NST', N'(UTC-03:30) Newfoundland Time (Canada)', -3, NULL, NULL)
GO
INSERT [dbo].[Timezone] ([timezoneID], [timezoneCode], [timezoneName], [gmtOffset], [dstStart], [dstEnd]) VALUES (9, N'GMT', N'(UTC) Dublin, Edinburgh, Lisbon, London', 0, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Timezone] OFF
GO