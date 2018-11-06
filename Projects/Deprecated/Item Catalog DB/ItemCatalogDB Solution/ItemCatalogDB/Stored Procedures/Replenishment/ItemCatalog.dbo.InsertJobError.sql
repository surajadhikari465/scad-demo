IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[InsertJobError]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[InsertJobError]
GO

CREATE PROCEDURE dbo.InsertJobError
	@Classname VARCHAR(50),
    @ExceptionText VARCHAR(2000)
AS
BEGIN
    SET NOCOUNT ON
	
	-- Create a new entry in the job error table for the given classname.
	INSERT INTO JobErrorLog (Classname, RunDate, ServerName, ExceptionText)
	VALUES (@Classname, GetDate(), HOST_NAME(), @ExceptionText)
    
    SET NOCOUNT OFF
END
GO
  