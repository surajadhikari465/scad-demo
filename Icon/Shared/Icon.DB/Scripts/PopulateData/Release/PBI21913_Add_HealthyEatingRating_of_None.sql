
-- Product Backlog Item 21913: Update Icon db to allow 0 - None to be added to HSH ref table
DECLARE @scriptKey VARCHAR(128) = 'Add_HealthyEatingRating_of_None'

IF(NOT EXISTS(SELECT * FROM [app].[PostDeploymentScriptHistory] WHERE ScriptKey = @scriptKey))
	BEGIN
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

		IF NOT EXISTS (
			SELECT 1 FROM [dbo].[HealthyEatingRating] WHERE [HealthyEatingRatingId]=0)
		BEGIN
			SET IDENTITY_INSERT [dbo].[HealthyEatingRating] ON;
			INSERT INTO [dbo].[HealthyEatingRating]
				([HealthyEatingRatingId], [Description])
				VALUES	(0, 'None');
			SET IDENTITY_INSERT [dbo].[HealthyEatingRating] OFF;
		END
		
		INSERT INTO [app].[PostDeploymentScriptHistory] (ScriptKey, RunTime)
			VALUES (@scriptKey, getdate())
	END
ELSE
BEGIN
	PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO