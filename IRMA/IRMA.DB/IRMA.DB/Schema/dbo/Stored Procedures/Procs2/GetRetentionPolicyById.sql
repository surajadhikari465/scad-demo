
CREATE PROCEDURE dbo.GetRetentionPolicyById
(
	@RetentionPolicyId INT
)
AS 
BEGIN
		SELECT [RetentionPolicyId]
      ,[Schema]
      ,[Table]
      ,[ReferenceColumn]
      ,[DaysToKeep]
      ,[TimeToStart]
      ,[TimeToEnd]
      ,[IncludedInDailyPurge]
      ,[DailyPurgeCompleted]
      ,[PurgeJobName]
      ,[LastPurgedDateTime]
  FROM [dbo].[RetentionPolicy]
  WHERE RetentionPolicyId = @RetentionPolicyId

  END
  
