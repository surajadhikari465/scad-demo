SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetRetentionPolicyById]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].GetRetentionPolicyById
GO

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
  
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 