print N'Populating EcoScaleRating...'

SET IDENTITY_INSERT [dbo].[EcoScaleRating] ON 

GO
INSERT [dbo].[EcoScaleRating] ([EcoScaleRatingId], [Description]) VALUES (1, N'Baseline/Orange')
GO
INSERT [dbo].[EcoScaleRating] ([EcoScaleRatingId], [Description]) VALUES (2, N'Premium/Yellow')
GO
INSERT [dbo].[EcoScaleRating] ([EcoScaleRatingId], [Description]) VALUES (3, N'Ultra-Premium/Green')
GO
SET IDENTITY_INSERT [dbo].[EcoScaleRating] OFF
GO