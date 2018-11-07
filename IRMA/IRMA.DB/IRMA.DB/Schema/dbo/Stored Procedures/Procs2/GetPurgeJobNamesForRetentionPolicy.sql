
CREATE PROCEDURE dbo.GetPurgeJobNamesForRetentionPolicy
AS
BEGIN

	SELECT DISTINCT PurgeJobName FROM [RetentionPolicy]
	ORDER BY PurgeJobName DESC

END



