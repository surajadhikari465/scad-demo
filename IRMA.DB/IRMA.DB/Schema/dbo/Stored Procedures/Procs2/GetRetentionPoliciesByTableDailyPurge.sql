
CREATE PROCEDURE dbo.GetRetentionPoliciesByTableDailyPurge
(
	@Table VARCHAR(64),
	@IncludedInDailyPurge bit
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
  WHERE [Table] = IsNull(@Table,[Table]) and IncludedInDailyPurge = IsNull(@IncludedInDailyPurge ,IncludedInDailyPurge)
  order by [table] asc
END 

