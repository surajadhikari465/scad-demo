SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetRetentionPoliciesByTableDailyPurge]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[GetRetentionPoliciesByTableDailyPurge]
GO

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



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 