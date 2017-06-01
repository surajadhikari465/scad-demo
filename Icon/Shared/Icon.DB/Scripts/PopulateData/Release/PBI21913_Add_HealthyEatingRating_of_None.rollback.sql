IF EXISTS (SELECT 1 FROM [dbo].[HealthyEatingRating] WHERE [HealthyEatingRatingId]=0)
BEGIN
	DELETE FROM [dbo].[HealthyEatingRating] WHERE [HealthyEatingRatingId]=0;
END