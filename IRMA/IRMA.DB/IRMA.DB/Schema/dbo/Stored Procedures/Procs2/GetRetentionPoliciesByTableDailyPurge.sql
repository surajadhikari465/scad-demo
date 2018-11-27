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

GRANT EXECUTE ON OBJECT::dbo.GetTablesWithRetentionPolicy TO [IRSUser];
GRANT EXECUTE ON OBJECT::dbo.GetTablesWithRetentionPolicy TO [IRMAClientRole];