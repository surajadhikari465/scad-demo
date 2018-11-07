SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[UpdateRetentionPolicy]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].UpdateRetentionPolicy
GO

CREATE PROCEDURE dbo.UpdateRetentionPolicy
(
	@RetentionPolicyId INT,
	@Schema VARCHAR(50),
	@Table VARCHAR(64),
	@ReferenceColumn VARCHAR(50),
	@DaysToKeep INT,
	@TimeToStart INT,
	@TimeToEnd INT,
	@IncludedInDailyPurge bit,
	@DailyPurgeCompleted bit,
	@PurgeJobName VARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON

	--See if there is a current record.
	IF EXISTS (SELECT RetentionPolicyId FROM [RetentionPolicy] (nolock) WHERE RetentionPolicyId = @RetentionPolicyId)
		BEGIN
			--Current record found, UPDATE.
			UPDATE [RetentionPolicy] 
			SET ReferenceColumn = @ReferenceColumn, 
			DaysToKeep = @DaysToKeep, 
			TimeToStart = @TimeToStart, 
			TimeToEnd = @TimeToEnd, 
			IncludedInDailyPurge = @IncludedInDailyPurge, 
			DailyPurgeCompleted = @DailyPurgeCompleted, 
			PurgeJobName = @PurgeJobName
			WHERE RetentionPolicyId = @RetentionPolicyId
		
			SELECT  @RetentionPolicyId
		END
	ELSE
		BEGIN
			--No current record, INSERT.
	
	INSERT INTO [dbo].[RetentionPolicy]
			   ([Schema]
			   ,[Table]
			   ,[ReferenceColumn]
			   ,[DaysToKeep]
			   ,[TimeToStart]
			   ,[TimeToEnd]
			   ,[IncludedInDailyPurge]
			   ,[DailyPurgeCompleted]
			   ,[PurgeJobName]
			   )
		 VALUES
			   (@Schema,
				@Table,
				@ReferenceColumn,
				@DaysToKeep,
				@TimeToStart,
				@TimeToEnd,
				@IncludedInDailyPurge,
				@DailyPurgeCompleted,
				@PurgeJobName
				)
			SELECT scope_identity();
		END

	SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 