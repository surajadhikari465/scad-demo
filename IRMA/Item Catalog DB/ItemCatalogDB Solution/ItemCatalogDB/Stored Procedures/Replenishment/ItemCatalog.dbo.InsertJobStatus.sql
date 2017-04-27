IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[InsertJobStatus]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[InsertJobStatus]
GO

CREATE PROCEDURE dbo.InsertJobStatus
	@Classname VARCHAR(50),
    @Status	VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON
	
	-- Create a new entry in the job status table for the given classname.
	INSERT INTO JobStatus (Classname, Status, LastRun, ServerName)
	VALUES (@Classname, @Status, GetDate(), HOST_NAME())
    
    SET NOCOUNT OFF
END
GO
 