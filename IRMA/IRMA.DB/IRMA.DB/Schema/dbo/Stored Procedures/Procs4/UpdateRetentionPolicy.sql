CREATE PROCEDURE dbo.UpdateRetentionPolicy
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
AS
BEGIN
	SET NOCOUNT ON

  IF NOT Exists(SELECT 1 from sys.tables A
            INNER JOIN sys.schemas B ON B.schema_id = A.schema_id
            WHERE B.name = @Schema and A.name = @Table)
  BEGIN
    RAISERROR('Table and/or Schema does not exist. Please verify your input and try again.', 15, 1);
    RETURN;
  END

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

GRANT EXECUTE ON OBJECT::dbo.UpdateRetentionPolicy TO [IRSUser] AS [dbo];
GO
GRANT EXECUTE ON OBJECT::dbo.UpdateRetentionPolicy TO [IRMAClientRole] AS [dbo];
GO