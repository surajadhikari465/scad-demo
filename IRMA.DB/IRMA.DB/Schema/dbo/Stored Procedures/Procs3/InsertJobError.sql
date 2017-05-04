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
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertJobError] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertJobError] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertJobError] TO [IRMASchedJobsRole]
    AS [dbo];

