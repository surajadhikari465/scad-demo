if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateJobStatusList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateJobStatusList]
GO

CREATE PROCEDURE [dbo].[UpdateJobStatusList]
	@Classname VARCHAR(50),
    @Status	VARCHAR(50),
	@StatusDescription VARCHAR(255),
	@Details VARCHAR(2000),
	@UpdateLastRun bit
AS
BEGIN
    SET NOCOUNT ON
	
	DECLARE @CentralTimeZoneOffset int
	
	SELECT @CentralTimeZoneOffset = CentralTimeZoneOffset FROM Region
	
	-- Update an entry in the job status table for the given classname.
	UPDATE JobStatus SET
		Status = @Status, 
		LastRun = CASE WHEN @UpdateLastRun = 1 THEN DATEADD(hour, @CentralTimeZoneOffset, GETDATE()) ELSE LastRun END,
		ServerName = HOST_NAME(),
		StatusDescription = @StatusDescription,
		Details = @Details
	WHERE
		Classname = @Classname
    
    SET NOCOUNT OFF
END