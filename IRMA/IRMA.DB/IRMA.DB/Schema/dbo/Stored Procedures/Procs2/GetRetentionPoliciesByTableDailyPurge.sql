CREATE PROCEDURE dbo.GetRetentionPoliciesByTableDailyPurge
	@Table VARCHAR(64) = NULL,
	@IncludedInDailyPurge bit = NULL
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
  WHERE [Table] = IsNull(@Table,[Table]) and IncludedInDailyPurge = IsNull(@IncludedInDailyPurge ,IncludedInDailyPurge)
  ORDER BY [Table]
END 
GO

GRANT EXECUTE ON OBJECT::dbo.GetRetentionPoliciesByTableDailyPurge TO [IRSUser];
GO
GRANT EXECUTE ON OBJECT::dbo.GetRetentionPoliciesByTableDailyPurge TO [IRMAClientRole];
GO