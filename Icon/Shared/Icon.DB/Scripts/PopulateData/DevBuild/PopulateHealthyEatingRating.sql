print N'Populating HealthyEatingRating...'

SET IDENTITY_INSERT [dbo].[HealthyEatingRating] ON 

GO
INSERT [dbo].[HealthyEatingRating] ([HealthyEatingRatingId], [Description]) VALUES (1, N'Good')
GO
INSERT [dbo].[HealthyEatingRating] ([HealthyEatingRatingId], [Description]) VALUES (2, N'Better')
GO
INSERT [dbo].[HealthyEatingRating] ([HealthyEatingRatingId], [Description]) VALUES (3, N'Best')
GO
SET IDENTITY_INSERT [dbo].[HealthyEatingRating] OFF
GO